//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//   K2NEServiceBroker provides additional functionality to K2 using a Custom Service Broker.
//   Copyright (C) 2016  K2NE GmbH.

//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   any later version.

//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.

//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using K2Field.K2NE.ServiceBroker.ServiceObjects.URM;
using K2Field.K2NE.ServiceBroker.ServiceObjects;
using K2Field.K2NE.ServiceBroker.ServiceObjects.Client_API;
using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Hosting.Server.Interfaces;
using K2Field.K2NE.ServiceBroker.ServiceObjects.PowerShell;
using K2Field.K2NE.ServiceBroker.ServiceObjects.ExcelServices;
using K2Field.K2NE.ServiceBroker.Properties;

namespace K2Field.K2NE.ServiceBroker
{
    public class K2NEServiceBroker : ServiceAssemblyBase, IHostableType
    {
        #region Private Properties
        private static readonly object serviceObjectToTypeLock = new object();
        private static readonly object serviceObjectLock = new object();
        private static Dictionary<string, Type> _serviceObjectToType = new Dictionary<string, Type>();
        private List<ServiceObjectBase> _serviceObjects;
        private object syncobject = new object();
        #endregion Private Properties


        #region Internal properties for ServiceObjectBase's child classes.
        internal Logger HostServiceLogger { get; private set; }
        internal IIdentityService IdentityService { get; private set; }
        internal static ISecurityManager SecurityManager { get; private set; }
        internal K2Connection K2Connection { get; private set; }
        #endregion Internal properties for ServiceObjectBase's child classes.



        #region Private Methods
        /// <summary>
        /// Cache of all service object that we have. We load this into a static object in the hope to re-use it as often as possible.
        /// </summary>
        private IEnumerable<ServiceObjectBase> ServiceObjectClasses
        {
            get
            {
                if (_serviceObjects == null)
                {
                    lock (serviceObjectLock)
                    {
                        if (_serviceObjects == null)
                        {
                            _serviceObjects = new List<ServiceObjectBase>
                            {
                                new ManagementWorklistSO(this),
                                new ErrorLogSO(this),
                                new IdentitySO(this),
                                new WorklistSO(this),
                                new OutOfOfficeClientSO(this),
                                new OutOfOfficeSO(this),
                                new ProcessInstanceManagementSO(this),
                                new ProcessInstanceClientSO(this),
                                new RoleManagementSO(this),
                                new ActiveDirectorySO(this),
                                new WorkingHoursConfigurationSO(this),
                                new GroupSO(this),
                                new UserSO(this),
                                new ADOSMOQuerySO(this),
                                new PowerShellVariablesSO(this),
                                new SimplePowerShellSO(this),
                                new DynamicPowerShellSO(this),
                                new ExcelDocumentServicesSO(this)
                            };

                        }
                    }
                }
                return _serviceObjects;
            }
        }

        /// <summary>
        /// helper property to get the type of the service object, to be able to initialize a specific instance of it.
        /// </summary>
        private Dictionary<string, Type> ServiceObjectToType
        {
            get
            {
                lock (serviceObjectToTypeLock)
                {
                    if (_serviceObjectToType.Count != 0)
                    {
                        return _serviceObjectToType;
                    }

                    _serviceObjectToType = new Dictionary<string, Type>();
                    foreach (ServiceObjectBase soBase in ServiceObjectClasses)
                    {
                        List<ServiceObject> serviceObjs = soBase.DescribeServiceObjects();
                        foreach (ServiceObject so in serviceObjs)
                        {
                            _serviceObjectToType.Add(so.Name, soBase.GetType());
                        }
                    }
                }
                return _serviceObjectToType;
            }
        }
        private ServiceFolder InitializeServiceFolder(string folderName, string description)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = "Other";
                description = "Other";
            }
            foreach (ServiceFolder sf in Service.ServiceFolders)
            {
                if (string.Compare(sf.Name, folderName) == 0)
                {
                    return sf;
                }
            }
            ServiceFolder newSf = new ServiceFolder(folderName, new MetaData(folderName, description));
            Service.ServiceFolders.Create(newSf);
            return newSf;
        }

        #endregion

        #region Constructor
        /// <summary>
        /// A new instance is called for every new connection that is created to the K2 server. A new instance of this class is not created
        /// when the connection remains open. One connection can be used for multiple things.
        /// </summary>
        public K2NEServiceBroker()
        {
        }
        #endregion

        #region Public overrides for ServiceAssemblyBase
        public override string DescribeSchema()
        {
            Service.Name = "K2NEServiceBroker";
            Service.MetaData.DisplayName = "K2NE's General Purpose Service Broker";
            Service.MetaData.Description = "A Service Broker that provides various functional service objects that aid the implementation of a K2 project.";

            bool requireServiceFolders = false;
            foreach (ServiceObjectBase entry in ServiceObjectClasses)
            {
                if (!string.IsNullOrEmpty(entry.ServiceFolder))
                {
                    requireServiceFolders = true;
                }
            }

            foreach (ServiceObjectBase entry in ServiceObjectClasses)
            {
                List<ServiceObject> serviceObjects = entry.DescribeServiceObjects();
                foreach (ServiceObject so in serviceObjects)
                {
                    Service.ServiceObjects.Create(so);
                    if (requireServiceFolders)
                    {
                        ServiceFolder sf = InitializeServiceFolder(entry.ServiceFolder, entry.ServiceFolder);
                        sf.Add(so);
                    }
                }
            }

            return base.DescribeSchema();
        }
        public override void Execute()
        {
            // Value can't be set in K2Connection constructor, because the SmartBroker sets the UserName value after Init
            K2Connection.UserName = Service.ServiceConfiguration.ServiceAuthentication.UserName;

            ServiceObject so = Service.ServiceObjects[0];
            try
            {
                //TODO: improve performance? http://bloggingabout.net/blogs/vagif/archive/2010/04/02/don-t-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions.aspx

                // This creates an instance of the object responsible to handle the execution.
                // We can't cache the instance itself, as that gives threading issue because the 
                // object can be re-used by the k2 host server for multiple different SMO calls
                // so we always need to know which ServiceObject we actually want to execute and 
                // create an instance first. This is  "late" initalization. We can also not keep a list of 
                // service objects that have been instanciated around in memory as this would be to resource 
                // intensive and slow (as we would constantly initialize all).
                if (so == null || string.IsNullOrEmpty(so.Name))
                {
                    throw new ApplicationException(Resources.SOIsNotSet);
                }
                if (!ServiceObjectToType.ContainsKey(so.Name))
                {
                    throw new ApplicationException(string.Format(Resources.IsNotValidSO, so.Name));
                }
                Type soType = ServiceObjectToType[so.Name];
                object[] constParams = new object[] { this };
                ServiceObjectBase soInstance = Activator.CreateInstance(soType, constParams) as ServiceObjectBase;

                soInstance.Execute();
                ServicePackage.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.AppendFormat("Exception.Message: {0}\n", ex.Message);
                error.AppendFormat("Exception.StackTrace: {0}\n", ex.StackTrace);

                Exception innerEx = ex;
                int i = 0;
                while (innerEx.InnerException != null)
                {
                    error.AppendFormat("{0} InnerException.Message: {1}\n", i, innerEx.InnerException.Message);
                    error.AppendFormat("{0} InnerException.StackTrace: {1}\n\n", i, innerEx.InnerException.StackTrace);
                    innerEx = innerEx.InnerException;
                    i++;
                }

                error.AppendLine();
                if (base.Service.ServiceObjects.Count > 0)
                {
                    foreach (ServiceObject executingSo in base.Service.ServiceObjects)
                    {
                        error.AppendFormat("Service Object Name: {0}\n", executingSo.Name);
                        foreach (Method method in executingSo.Methods)
                        {
                            error.AppendFormat("Service Object Methods: {0}\n", method.Name);
                        }

                        foreach (Property prop in executingSo.Properties)
                        {
                            string val = prop.Value as string;
                            if (!string.IsNullOrEmpty(val))
                            {
                                error.AppendFormat("[{0}].{1}: {2}\n", executingSo.Name, prop.Name, val);
                            }
                            else
                            {
                                error.AppendFormat("[{0}].{1}: [String.Empty]\n", executingSo.Name, prop.Name);
                            }
                        }
                    }
                }


                ServicePackage.ServiceMessages.Add(error.ToString(), MessageSeverity.Error);
                ServicePackage.IsSuccessful = false;
            }
        }
        public override string GetConfigSection()
        {
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.EnvironmentToUse, false, ""); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.DefaultCulture, true, "EN-us"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.Platform, false, "ASP"); //Checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AdMaxResultSize, false, "1000"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.LDAPPaths, false, "LDAP://DC=denallix,DC=COM"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.NetbiosNames, false, "Denallix"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.ChangeContainsToStartsWith, true, "true"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AdditionalADProps, false, ""); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.ADOSMOQueries, false, ""); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.ADOSMOQueriesFile, false, ""); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AllowPowershellScript, true, "true"); //checked
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.PowerShellSubdirectories, false, "PowerShellScripts"); //checked
            return base.GetConfigSection();
        }
        public void Init(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            lock (syncobject)
            {
                if (HostServiceLogger == null)
                {
                    HostServiceLogger = new Logger(serviceMarshalling.GetHostedService(typeof(SourceCode.Logging.ILogger)) as SourceCode.Logging.ILogger);
                    HostServiceLogger.LogDebug("Logger loaded from ServiceMarshalling");
                }

                if (IdentityService == null)
                {
                    IdentityService = serviceMarshalling.GetHostedService(typeof(IIdentityService)) as IIdentityService;
                }
                if (SecurityManager == null)
                {
                    SecurityManager = serverMarshaling.GetSecurityManagerContext();
                }

                K2Connection = new K2Connection(serviceMarshalling, serverMarshaling);
            }
        }

        public override void Extend() { }
        public void Unload()
        {
            HostServiceLogger.Dispose();
        }
        #endregion Public overrides for ServiceAssemblyBase


    }
}
