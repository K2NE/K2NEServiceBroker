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

        //public Logger Logger { get; private set; }
        public IIdentityService IdentityService { get; private set; }
        public ISecurityManager SecurityManager { get; private set; }



        #region Private Methods
        private IEnumerable<ServiceObjectBase> ServiceObjects
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
                                new OutOfOfficeSO(this),
                                new ProcessInstanceManagementSO(this),
                                new ProcessInstanceClientSO(this),
                                new RoleManagementSO(this),
                                new ActiveDirectorySO(this),
                                new WorkingHoursConfigurationSO(this),
                                new GroupSO(this),
                                new UserSO(this)
                            };

                        }
                    }
                }
                return _serviceObjects;
            }
        }
        private Dictionary<string, Type> ServiceObjectToType
        {
            get
            {
                if (_serviceObjectToType.Count != 0) return _serviceObjectToType;
                lock (serviceObjectToTypeLock)
                {
                    if (_serviceObjectToType.Count != 0) return _serviceObjectToType;
                    _serviceObjectToType = new Dictionary<string, Type>();
                    foreach (var soBase in ServiceObjects)
                    {
                        foreach (var so in soBase.DescribeServiceObjects())
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
            Service.ServiceFolders.Add(newSf);
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
            foreach (ServiceObjectBase entry in ServiceObjects)
            {
                if (!string.IsNullOrEmpty(entry.ServiceFolder))
                {
                    requireServiceFolders = true;
                }
            }

            foreach (var entry in ServiceObjects)
            {
                foreach (var so in entry.DescribeServiceObjects())
                {
                    Service.ServiceObjects.Add(so);
                    if (requireServiceFolders)
                    {
                        var sf = InitializeServiceFolder(entry.ServiceFolder, entry.ServiceFolder);
                        sf.Add(so);
                    }
                }
            }
            return base.DescribeSchema();
        }
        public override void Execute()
        {
            var so = Service.ServiceObjects[0];
            try
            {
                //TODO: improve performance? http://bloggingabout.net/blogs/vagif/archive/2010/04/02/don-t-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions.aspx


                // This creates an instance of the object responsible to handle the execution.
                // We can't cache the instance itself, as that gives threading issue because the object can be re-used by the k2 host server for multiple different SMO calls
                // so we always need to know which ServiceObject we actually want to execute and create an instance first. This is  "late" initalization. We can also not keep a list of 
                // service objects that have been instanciated around in memory as this would be to resource intensive and slow (as we would constantly initialize all).
                var soType = ServiceObjectToType[so.Name];
                var constParams = new object[] { this };
                var soInstance = Activator.CreateInstance(soType, constParams) as ServiceObjectBase;

                soInstance.Execute();
                ServicePackage.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                var error = new StringBuilder();
                error.AppendFormat("Exception.Message: {0}", ex.Message);
                error.AppendFormat("Exception.StackTrace: {0}", ex.Message);

                var innerEx = ex;
                var i = 0;
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
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.WorkflowManagmentPort, true, "5555");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.WorkflowClientPort, true, "5252");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.EnvironmentToUse, false, "");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.DefaultCulture, true, "EN-us");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.Platform, false, "ASP");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AdMaxResultSize, false, "1000");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.LDAPPaths, false, "LDAP://DC=denallix,DC=COM");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.NetbiosNames, false, "Denallix");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.ChangeContainsToStartsWith, true, "true");
            Service.ServiceConfiguration.Add(Constants.ConfigurationProperties.AdditionalADProps, false, "");
            return base.GetConfigSection();
        }
        public void Init(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            lock (syncobject)
            {
                //if (Logger == null)
                //{
                //    Logger = new Logger(serviceMarshalling.GetHostedService(typeof(SourceCode.Logging.ILogger)) as SourceCode.Logging.ILogger);
                //    Logger.LogDebug("Logger loaded from ServiceMarshalling");
                //}

                if (IdentityService == null)
                {
                    IdentityService = serviceMarshalling.GetHostedService(typeof(IIdentityService)) as IIdentityService;
                }
                if (SecurityManager == null)
                {
                    SecurityManager = serverMarshaling.GetSecurityManagerContext();
                }

            }
            

        }
        public override void Extend() { }
        public void Unload()
        {
            //Logger.Dispose();
        }
        #endregion Public overrides for ServiceAssemblyBase


    }
}
