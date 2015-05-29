using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Hosting.Server.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace K2Field.K2NE.ServiceBroker
{
    public class K2NEServiceBroker : ServiceAssemblyBase, IHostableType
    {
        private static readonly object serviceObjectToTypeLock = new object();
        private static readonly object serviceObjectLock = new object();
        private static Dictionary<string, Type> serviceObjectToType = new Dictionary<string, Type>();
        private List<ServiceObjectBase> serviceObjects;
        public Logger Logger { get; private set; }

        private List<ServiceObjectBase> ServiceObjects
        {
            get
            {
                if (serviceObjects == null)
                {
                    lock (serviceObjectLock)
                    {
                        if (serviceObjects == null)
                        {
                            serviceObjects = new List<ServiceObjectBase>();
                            serviceObjects.Add(new ManagementWorklistSO(this));
                            serviceObjects.Add(new ErrorLogSO(this));
                            serviceObjects.Add(new IdentitySO(this));
                            serviceObjects.Add(new WorklistSO(this));
                            serviceObjects.Add(new OutOfOfficeSO(this));
                            serviceObjects.Add(new ProcessInstanceManagementSO(this));
                            serviceObjects.Add(new RoleManagementSO(this));
                            serviceObjects.Add(new ActiveDirectorySO(this));
                            serviceObjects.Add(new WorkingHoursConfigurationSO(this));
                        }
                    }
                }
                return serviceObjects;
            }
        }

        private Dictionary<string, Type> ServiceObjectToType
        {
            get
            {
                if (serviceObjectToType.Count == 0)
                {
                    lock (serviceObjectToTypeLock)
                    {
                        if (serviceObjectToType.Count == 0)
                        {
                            serviceObjectToType = new Dictionary<string, Type>();
                            foreach (ServiceObjectBase soBase in ServiceObjects)
                            {
                                foreach (ServiceObject so in soBase.DescribeServiceObjects())
                                {
                                    serviceObjectToType.Add(so.Name, soBase.GetType());
                                }
                            }
                        }
                    }
                }
                return serviceObjectToType;
            }
        }

        private ServiceFolder InitializeServiceFolder(string folderName, string description)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = "Other";
                description = "Other";
            }
            foreach (ServiceFolder sf in this.Service.ServiceFolders)
            {
                if (string.Compare(sf.Name, folderName) == 0)
                    return sf;
            }
            ServiceFolder newSf = new ServiceFolder(folderName, new MetaData(folderName, description));
            this.Service.ServiceFolders.Add(newSf);
            return newSf;
        }



        /// <summary>
        /// A new instance is called for every new connection that is created to the K2 server. A new instance of this class is not created
        /// when the connection remains open. One connection can be used for multiple things.
        /// </summary>
        public K2NEServiceBroker()
        {
            Logger = new Logger();
        }




        #region Public overrides for ServiceAssemblyBase

        public override string DescribeSchema()
        {
            this.Service.Name = "K2NEServiceBroker";
            this.Service.MetaData.DisplayName = "K2NE's General Purpose Service Broker";
            this.Service.MetaData.Description = "A Service Broker that provides various functional service objects that aid the implementation of a K2 project.";

            bool requireServiceFolders = false;
            foreach (ServiceObjectBase entry in ServiceObjects)
            {
                if (!string.IsNullOrEmpty(entry.ServiceFolder))
                {
                    requireServiceFolders = true;
                }
            }

            foreach (ServiceObjectBase entry in ServiceObjects)
            {
                foreach (ServiceObject so in entry.DescribeServiceObjects())
                {
                    this.Service.ServiceObjects.Add(so);
                    if (requireServiceFolders) {
                        ServiceFolder sf = InitializeServiceFolder(entry.ServiceFolder, entry.ServiceFolder);
                        sf.Add(so);
                    }
                }
            }

            return base.DescribeSchema();
        }
     
     
        public override void Execute()
        {
            ServiceObject so = this.Service.ServiceObjects[0];
            try
            {
                //TODO: improve performance? http://bloggingabout.net/blogs/vagif/archive/2010/04/02/don-t-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions.aspx


                // This creates an instance of the object responsible to handle the execution.
                // We can't cache the instance itself, as that gives threading issue because the object can be re-used by the k2 host server for multiple different SMO calls
                // so we always need to know which ServiceObject we actually want to execute and create an instance first. This is  "late" initalization. We can also not keep a list of 
                // service objects that have been instanciated around in memory as this would be to resource intensive and slow (as we would constantly initialize all).
                Type soType = ServiceObjectToType[so.Name];
                object[] constParams = new object[] { this };
                ServiceObjectBase soInstance = Activator.CreateInstance(soType, constParams) as ServiceObjectBase;

                soInstance.Execute();
                ServicePackage.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.AppendFormat("Exception.Message: {0}", ex.Message);
                error.AppendFormat("Exception.StackTrace: {0}", ex.Message);

                Exception innerEx = ex;
                int i = 0;
                while (innerEx.InnerException != null)
                {
                    error.AppendFormat("{0} InnerException.Message: {1}", i, ex.InnerException.Message);
                    error.AppendFormat("{0} InnerException.StackTrace: {1}", i, ex.InnerException.StackTrace);
                    innerEx = innerEx.InnerException;
                    i++;
                }
                ServicePackage.ServiceMessages.Add(error.ToString(), MessageSeverity.Error);
                ServicePackage.IsSuccessful = false;
            }
        }

        public override string GetConfigSection()
        {
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.WorkflowManagmentPort, true, "5555");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.WorkflowClientPort, true, "5252");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.EnvironmentToUse, false, "");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.DefaultCulture, true, "EN-us");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.Platform, false, "ASP");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AdMaxResultSize, false, "1000");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.LDAPPath, false, "LDAP://DC=denallix,DC=COM");
            this.Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.NetbiosName, false, "Denallix");
            return base.GetConfigSection();
        }
        public override void Extend() { }
        #endregion Public overrides for ServiceAssemblyBase

        void IHostableType.Init(IServiceMarshalling ServiceMarshalling, IServerMarshaling ServerMarshaling)
        {
            if (Logger.SelfLoaded == true)
            {
                string type = typeof(SourceCode.Logging.Logger).ToString();
                if (ServiceMarshalling.IsServiceHosted(type))
                {
                    Logger = new Logger((SourceCode.Logging.Logger)ServiceMarshalling.GetHostedService(type));
                    Logger.LogDebug("Logger loaded from ServiceMarshalling");
                }
            }
        }

        void IHostableType.Unload()
        {
            Logger.LogDebug("Service Broker unloaded.");
        }
    }
}
