using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.EnvironmentSettings.Client;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;

namespace K2Field.K2NE.ServiceBroker
{
    public abstract class ServiceObjectBase
    {
        #region private variables
        private ConnectionSetup connectionSetup = null;
        private SCConnectionStringBuilder baseConnection = null;

        private static Dictionary<string, string> environmentFields = new Dictionary<string, string>();
        private static Object envLock = new Object();
        private Mutex envMutex;

        private static Regex wordMatchRegex = null;

        #endregion private variables

        #region Protected Methods and properties that are useful for the child class
        /// <summary>
        /// The serviceBroker object that is currently being used/executed. Property values (etc) will be taken from this object.
        /// </summary>
        protected K2NEServiceBroker ServiceBroker
        {
            get;
            private set;
        }



        /// <summary>
        /// This is the connectionstring for any baseAPI Connection (management, smartobjects, etc).
        /// </summary>
        protected string BaseAPIConnectionString
        {
            get
            {
                return GetBaseConnection(WorkflowManagementPort);
            }

        }

        public string LDAPPaths
        {
            get
            {
                return this.ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.LDAPPaths].ToString();
            }
        }
        public bool ChangeContainsToStartWith
        {
            get
            {
                return bool.Parse(ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.ChangeContainsToStartsWith].ToString());
            }
        }

        public int ADMaxResultSize
        {
            get
            {
                return int.Parse(this.ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.AdMaxResultSize].ToString());

            }
        }

        public string[] AdditionalADProps
        {
            get
            {
                if (!ServiceBroker.Service.ServiceConfiguration.Contains(Constants.ConfigurationProperties.AdditionalADProps))
                {
                    return new string[0];
                }

                string adProps = ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.AdditionalADProps].ToString();
                if (!string.IsNullOrEmpty(adProps))
                {
                    return adProps.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return new string[0];
            }
        }


        /// <summary>
        /// This is the k2 client api connectionSetup object that can be used to create a connection.
        /// </summary>
        protected ConnectionSetup K2ClientConnectionSetup
        {
            get
            {
                if (connectionSetup == null)
                {
                    connectionSetup = new ConnectionSetup();
                    connectionSetup.ConnectionString = GetBaseConnection(WorkflowClientPort);
                }
                return connectionSetup;
            }
        }

        /// <summary>
        /// The default culture to use.
        /// </summary>
        protected string DefaultCulture
        {
            get
            {
                return ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.DefaultCulture].ToString();
            }
        }

        protected string Platform
        {
            get
            {
                return ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.Platform].ToString();
            }
        }

        public string NetBiosNames
        {
            get
            {

                return ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.NetbiosNames].ToString();
            }
        }


        /// <summary>
        /// Returns the FQN for the user calling the SMO.
        /// </summary>
        protected string CallingFQN
        {
            get
            {
                //TODO: FQN string might not read thread, or at least support the other authentication modes.
                switch (ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.AuthenticationMode)
                {
                    case AuthenticationMode.Impersonate:
                    case AuthenticationMode.ServiceAccount:
                        return ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
                    default:
                        return "K2:" + System.Threading.Thread.CurrentPrincipal.Identity.Name;
                }

            }
        }



        /// <summary>
        /// Provides a dictionary with the environment fields
        /// </summary>
        protected Dictionary<string, string> EnvironmentFields
        {
            get
            {
                if (environmentFields.Count == 0)
                {
                    lock (envLock)
                    {
                        if (environmentFields.Count == 0)
                        {
                            LoadDefaultEnvironmentFields();
                        }
                    }
                }
                return environmentFields;
            }
            set
            {
                lock (envLock)
                {
                    LoadDefaultEnvironmentFields();
                }
            }
        }



        #region Protected helper methods for property value retrieval
        /// <summary>
        /// Returns the value of the string property with the given value from the current SErviceObject.
        /// Method should always be called in the context of a 'Execute()'.
        /// 
        /// Setting isRequired might cause this method to throw an exception if the property was not found or the value is string empty.
        /// 
        /// If isRequired is not set, String.Empty is returned when the property does not exist.
        /// </summary>
        /// <param name="name">Name of the property to retrieve.</param>
        /// <param name="isRequired"></param>
        /// <returns></returns>

        protected string GetStringProperty(string name, bool isRequired = false)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
            {
                if (isRequired)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredPropertyNotFound, name));
                return string.Empty;
            }
            string val = p.Value as string;
            if (isRequired && string.IsNullOrEmpty(val))
                throw new ArgumentException(string.Format("{0} is required but is empty.", name));

            return val;
        }



        protected string GetStringParameter(string name, bool isRequired = false)
        {
            MethodParameter p = ServiceBroker.Service.ServiceObjects[0].Methods[0].MethodParameters[name];
            if (p == null)
            {
                if (isRequired)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredParameterNotFound, name));
                return string.Empty;
            }
            string val = p.Value as string;
            if (isRequired && string.IsNullOrEmpty(val))
                throw new ArgumentException(string.Format("{0} is required but is empty.", name));

            return val;
        }


        /// <summary>
        /// Retrieve an integer property with the given name from the current ServiceObject.
        /// Method should always be called in the context of a 'Execute()'.
        /// 
        /// The isRequired value is optional. Setting it to true will cause an exception if the property is not found or
        /// if the value could not be parsed to an integer.
        /// If false or nothing is supplied, the method returns 0 (zero) when it cannot determin the value.
        /// </summary>
        /// <param name="name">Name of the property to retrieve.</param>
        /// <param name="isRequred">Specify if the field must exist and needs to be parsable to int.</param>
        /// <returns>0 or the value of the property.</returns>
        protected int GetIntProperty(string name, bool isRequred = false)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
            {
                if (isRequred)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredPropertyNotFound, name));
                return 0;
            }
            string val = p.Value as string;
            int ret;
            if (int.TryParse(val, out ret))
                return ret;
            if (isRequred)
                throw new ArgumentException(string.Format("{0} could not be parsed to a Integer", name));

            return 0;
        }

        protected short GetShortProperty(string name, bool isRequired = false)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
            {
                if (isRequired)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredPropertyNotFound, name));
                return 0;
            }
            string val = p.Value as string;
            short ret;
            if (short.TryParse(val, out ret))
                return ret;
            if (isRequired)
                throw new ArgumentException(string.Format("{0} could not be parsed to a Integer", name));

            return 0;
        }

        protected bool GetBoolProperty(string name)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
                return false;
            string val = p.Value as string;
            bool ret;
            if (bool.TryParse(val, out ret))
            {
                return ret;
            }
            return false;
        }

        protected byte GetByteProperty(string name, bool isRequired = false)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
            {
                if (isRequired)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredPropertyNotFound, name));
                return 0;
            }
            string val = p.Value as string;
            byte ret;
            if (byte.TryParse(val, out ret))
                return ret;
            if (isRequired)
                throw new ArgumentException(string.Format("{0} could not be parsed to a Byte.", name));
            return 0;
        }

        protected Guid GetGuidProperty(string name, bool isRequired = false)
        {
            Property p = ServiceBroker.Service.ServiceObjects[0].Properties[name];
            if (p == null)
            {
                if (isRequired)
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.RequiredPropertyNotFound, name));
                return Guid.Empty;
            }
            string val = p.Value as string;
            Guid ret;
            if (Guid.TryParse(val, out ret))
                return ret;
            if (isRequired)
                throw new ArgumentException(string.Format("{0} could not be parsed to a Guid.", name));
            return Guid.Empty;
        }

        #endregion Protected helper methods for property value retrieval

        #endregion Protected Methods and properties that are useful for the child class

        #region Private properties

        private string EnvironmentToUseConfiguration
        {
            get
            {
                return ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.EnvironmentToUse] as string;
            }
        }


        private uint WorkflowClientPort
        {
            get
            {
                return uint.Parse(ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.WorkflowClientPort].ToString());
            }
        }

        private uint WorkflowManagementPort
        {
            get
            {
                return uint.Parse(ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.WorkflowManagmentPort].ToString());
            }
        }

        #endregion Private properties

        #region Private helper methods

        private string GetBaseConnection(uint port)
        {
            if (this.baseConnection == null)
            {
                this.baseConnection = new SCConnectionStringBuilder();
                this.baseConnection.Authenticate = true;
                this.baseConnection.Host = "localhost"; //hardcoded, always connect to yourself. Works better on NLB environments.
                this.baseConnection.Integrated = true;
                this.baseConnection.IsPrimaryLogin = true;
                this.baseConnection.Port = port;
            }
            return this.baseConnection.ToString();
        }


        private string ReplaceEnvironmentFields(string value)
        {
            // Use of regex: http://stackoverflow.com/questions/379328/c-sharp-named-parameters-to-a-string-that-replace-to-the-parameter-values
            Dictionary<string, string> vals = new Dictionary<string, string>();


            foreach (KeyValuePair<string, string> entry in EnvironmentFields)
            {
                vals.Add(string.Concat("Environment.", entry.Key), entry.Value);
            }

            return wordMatchRegex.Replace(value, delegate(Match match)
            {
                string key = match.Groups[1].Value;
                foreach (KeyValuePair<string, string> val in vals)
                {
                    if (string.Compare(val.Key, key, true) == 0) // the replace needs to be case insensative as well, if we use containsKey() it is not.
                        return val.Value;
                }
                return match.Value;
            });
        }


        private void LoadDefaultEnvironmentFields()
        {

            bool mutexCreated;

            MutexSecurity mutexsecurity = new MutexSecurity();
            mutexsecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));
            envMutex = new Mutex(false, "Global\\environmentMutext", out mutexCreated, mutexsecurity);
            envMutex.WaitOne();
            Dictionary<string, string> tempEnv = new Dictionary<string, string>();
            EnvironmentSettingsManager environmentManager = new EnvironmentSettingsManager(false, true);
            environmentManager.ConnectToServer(BaseAPIConnectionString);
            environmentManager.InitializeSettingsManager();


            EnvironmentInstance env = environmentManager.EnvironmentTemplates.DefaultTemplate.DefaultEnvironment;
            if (!string.IsNullOrEmpty(EnvironmentToUseConfiguration))
            {
                env = environmentManager.EnvironmentTemplates.DefaultTemplate.Environments[EnvironmentToUseConfiguration];
            }

            environmentManager.GetEnvironmentFields(env);
            foreach (EnvironmentField field in env.EnvironmentFields)
            {
                // For some reason, a environment can have 2 environment fields with the same name. Go figure.
                if (!tempEnv.ContainsKey(field.FieldName))
                {
                    tempEnv.Add(field.FieldName, field.Value);
                }
            }

            if (!tempEnv.ContainsKey("SmartForms Runtime"))
            {
                foreach (EnvironmentField field in env.EnvironmentFields)
                {
                    if (field.IsDefaultField && string.Compare(field.FieldTypeId, "EC07ABC1-259D-40CE-9485-04EEB3E5DCF0", true) == 0)
                    {
                        tempEnv.Add("SmartForms Runtime", field.Value);
                        break;
                    }
                }
            }

            environmentFields = tempEnv;
            envMutex.ReleaseMutex();
        }

        #endregion Private Helper methods

        #region Abstract methods
        /// <summary>
        /// Public method, required because we want to set _serviceBroker.
        /// </summary>
        /// <param name="broker"></param>
        public ServiceObjectBase(K2NEServiceBroker broker)
        {
            ServiceBroker = broker;

            //bool mutexCreated;

            //MutexSecurity mutexsecurity = new MutexSecurity();
            //mutexsecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));


            //try
            //{
            //    envMutex = Mutex.OpenExisting("Global\\environmentMutext", MutexRights.Synchronize | MutexRights.Modify);
            //}
            //catch (Exception ex)
            //{
            //    envMutex = new Mutex(false, "Global\\environmentMutext", out mutexCreated, mutexsecurity);
            //}

            if (wordMatchRegex == null)
            {
                // Match a word, a dot, another word with possibly a special character in it. And, maybe after that a third word with a dot in front of it. Examples:
                // "Environment.SimpleField"
                // "Environment.SimpleField with a space"
                // "ProcessInstance.Originator.DisplayName"
                wordMatchRegex = new Regex(@"\{(\w*\.[\w\s]*(\.[\w\s]*)?)\}", RegexOptions.Compiled); // Compiled regex, so we'd like to store it.
            }
        }

        /// <summary>
        /// Method to return ServiceObjects. This is then used to describe service objects in the main class.
        /// A list is returned, so you can return multiple service objects.
        /// </summary>
        /// <returns></returns>
        public abstract List<ServiceObject> DescribeServiceObjects();


        public virtual string ServiceFolder
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets called when executing the service object.
        /// </summary>
        public abstract void Execute();

        #endregion Abstract methods




    }
}
