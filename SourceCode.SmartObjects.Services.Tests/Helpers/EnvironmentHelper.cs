using System;
using System.Collections.Generic;
using SourceCode.EnvironmentSettings.Client;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    public static class EnvironmentHelper
    {
        private readonly static Dictionary<string, string> _cachedEnvironmentFields = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public static string GetEnvironmentFieldByName(string name)
        {
            string value;
            if (_cachedEnvironmentFields.TryGetValue(name, out value))
            {
                return value;
            }

            var server = GetEnvironmentSettingsManager();
            using (server)
            {
                var template = server.EnvironmentTemplates.DefaultTemplate;
                var environment = template.DefaultEnvironment;

                server.GetEnvironmentFields(environment);

                var field = environment.EnvironmentFields.GetItemByName(name);
                _cachedEnvironmentFields[name] = field.Value;

                return field.Value;
            }
        }

        private static EnvironmentSettingsManager GetEnvironmentSettingsManager()
        {
            var environmentSettingsManager = new EnvironmentSettingsManager(false, false);

            environmentSettingsManager.ConnectToServer(ConnectionHelper.SmartObjectConnectionStringBuilder.ConnectionString);
            environmentSettingsManager.InitializeSettingsManager(true);

            return environmentSettingsManager;
        }

        public static class FieldNames
        {
            internal const string SmartFormsRuntime = "SmartForms Runtime";
        }
    }
}