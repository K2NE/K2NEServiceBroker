using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Authoring;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Management;
using SourceCode.SmartObjects.Services.Management;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    /// <summary>
    /// SmartObjectHelper
    /// </summary>
    public static class SmartObjectHelper
    {
        public static Guid SmartBoxServiceInstanceGuid = new Guid("e5609413-d844-4325-98c3-db3cacbd406d");

        public static void CompareDataTables(DataTable dataTable1, DataTable dataTable2)
        {
            Assert.AreEqual(dataTable1.Rows.Count, dataTable2.Rows.Count);
            Assert.AreEqual(dataTable1.Columns.Count, dataTable2.Columns.Count);

            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                DataRow dataRow1 = dataTable1.Rows[i];
                DataRow dataRow2 = dataTable2.Rows[i];

                foreach (DataColumn dataColumn1 in dataTable1.Columns)
                {
                    var dataColumn2 = dataTable2.Columns[dataColumn1.ColumnName];
                    Assert.AreEqual(dataRow1[dataColumn1], dataRow2[dataColumn2]);
                }
            }
        }

        public static bool ContainsSmartObject(SmartObjectManagementServer server, string systemName)
        {
            return server.GetSmartObjects(systemName).SmartObjectList.Any();
        }

        public static void DeleteSmartObject(SmartObjectManagementServer server, string systemName)
        {
            if (ContainsSmartObject(server, systemName))
            {
                server.DeleteSmartObject(systemName, true);
            }
        }

        public static SmartObject ExecuteBulkScalar(SmartObjectClientServer clientServer, SmartObject smartObject, DataTable inputTable)
        {
            try
            {
                return clientServer.ExecuteBulkScalar(smartObject, inputTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static SmartObjectList ExecuteList(SmartObjectClientServer clientServer, SmartObject smartObject, ExecuteListOptions options = null)
        {
            if (options == null)
            {
                options = new ExecuteListOptions();
            }

            try
            {
                return clientServer.ExecuteList(smartObject, options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static DataTable ExecuteListDataTable(SmartObjectClientServer clientServer, SmartObject smartObject, ExecuteListOptions options = null)
        {
            if (options == null)
            {
                options = new ExecuteListOptions();
            }

            try
            {
                return clientServer.ExecuteListDataTable(smartObject, options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static SmartObjectReader ExecuteListReader(SmartObjectClientServer clientServer, SmartObject smartObject, ExecuteListReaderOptions options = null)
        {
            if (options == null)
            {
                options = new ExecuteListReaderOptions();
            }

            try
            {
                return clientServer.ExecuteListReader(smartObject, options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static SmartObject ExecuteScalar(SmartObjectClientServer clientServer, SmartObject smartObject)
        {
            try
            {
                return clientServer.ExecuteScalar(smartObject);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static DataTable ExecuteSQLQueryDataTable(SmartObjectClientServer clientServer, string query)
        {
            try
            {
                return clientServer.ExecuteSQLQueryDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionMessage());
            }
        }

        public static ServiceConfigInfo GetServiceConfigInfo(Guid serviceTypeGuid)
        {
            var connection = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (connection.Connection)
            {
                string serviceInstanceConfigXml = connection.GetServiceInstanceConfig(serviceTypeGuid);
                return ServiceConfigInfo.Create(serviceInstanceConfigXml);
            }
        }

        public static ServiceInstance GetServiceInstance(ServiceInstanceSettings serviceInstanceSettings)
        {
            var connection = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (connection.Connection)
            {
                var serviceInstance = ServiceInstance.Create(connection.GetServiceInstance(serviceInstanceSettings.Guid, ServiceExplorerLevel.ServiceObject));
                return serviceInstance;
            }
        }

        public static ServiceInstanceInfo GetServiceInstance(Guid guid)
        {
            var serviceManagementServer = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (serviceManagementServer.Connection)
            {
                var serviceInstanceXml = serviceManagementServer.GetServiceInstanceCompact(guid);
                if (string.IsNullOrEmpty(serviceInstanceXml))
                {
                    return null;
                }
                else
                {
                    return ServiceInstanceInfo.Create(serviceInstanceXml);
                }
            }
        }

        public static ServiceObject GetServiceObject(ServiceInstanceSettings serviceInstanceSettings, string serviceObjectName)
        {
            var connection = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (connection.Connection)
            {
                var serviceObject = ServiceObject.Create(connection.GetServiceInstanceServiceObject(serviceInstanceSettings.Guid, serviceObjectName));
                return serviceObject;
            }
        }

        public static ServiceTypeInfo GetServiceType(Guid guid)
        {
            var serviceManagementServer = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (serviceManagementServer.Connection)
            {
                var serviceTypeXml = serviceManagementServer.GetServiceType(guid);
                if (string.IsNullOrEmpty(serviceTypeXml))
                {
                    return null;
                }
                else
                {
                    return ServiceTypeInfo.Create(serviceTypeXml);
                }
            }
        }

        public static ServiceTypeInfo GetServiceTypeInfo(Guid serviceTypeGuid)
        {
            var connection = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (connection.Connection)
            {
                string serviceTypeInfoXml = connection.GetServiceType(serviceTypeGuid);
                return ServiceTypeInfo.Create(serviceTypeInfoXml);
            }
        }

        public static SmartObject GetSmartObject(SmartObjectClientServer clientServer, string serviceObjectName, ServiceInstanceSettings serviceInstanceSettings)
        {
            var smartObjectName = GetSmartObjectName(serviceObjectName, serviceInstanceSettings);
            return clientServer.GetSmartObject(smartObjectName);
        }

        public static SmartObjectDefinition GetSmartObjectDefinition(string smartObjectName)
        {
            var managementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (managementServer.Connection)
            {
                return SmartObjectDefinition.Create(managementServer.GetSmartObjectDefinition(smartObjectName));
            }
        }

        public static string GetSmartObjectName(string serviceObjectName, ServiceInstanceSettings serviceInstanceSettings)
        {
            var managementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (managementServer.Connection)
            {
                var preSmartObjectName = string.Concat(serviceInstanceSettings.Name, "_");
                var smartObjectExplorer = managementServer.GetSmartObjects(SearchProperty.SystemName, SearchOperator.EndsWith, string.Concat("_", serviceObjectName));
                return (from s in smartObjectExplorer.SmartObjectList
                        where s.Name.StartsWith(preSmartObjectName)
                        select s.Name).FirstOrDefault();
            }
        }

        public static IEnumerable<SmartObjectInfo> GetSmartObjects(Guid[] guids)
        {
            var managementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (managementServer.Connection)
            {
                return managementServer.GetSmartObjects(guids).SmartObjectList.ToArray();
            }
        }

        public static void PublishSmartObjects(SmartObjectDefinitionsPublish publishSmO)
        {
            var managementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (managementServer.Connection)
            {
                // Delete SmartObjects
                foreach (SmartObjectDefinition smartObject in publishSmO.SmartObjects)
                {
                    SmartObjectHelper.DeleteSmartObject(managementServer, smartObject.Name);
                }

                managementServer.PublishSmartObjects(publishSmO.ToPublishXml());
            }
        }

        public static void PublishSmartObjectsFromResources(Assembly assembly, string category)
        {
            var publishSmO = new SmartObjectDefinitionsPublish();

            // Get the SmartObjects from the embeded resources of the assembly
            foreach (var resource in assembly.GetManifestResourceNames())
            {
                if (resource.IndexOf(".sodx", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    using (var streamSodx = assembly.GetManifestResourceStream(resource))
                    {
                        var resourceSmartObject = SmartObjectDefinition.Create(streamSodx);
                        resourceSmartObject.AddDeploymentCategory(category);
                        publishSmO.SmartObjects.Add(resourceSmartObject);
                    }
                }
            }

            // Publish the SmartObjects
            PublishSmartObjects(publishSmO);
        }

        public static void VerifyAllReturnPropertiesHasValues(SmartMethodBase method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            foreach (SmartProperty returnProperty in method.ReturnProperties)
            {
                Assert.IsFalse(string.IsNullOrEmpty(returnProperty.Value));
            }
        }

        public static void VerifyAllReturnPropertiesHasValues(SmartObject smartObject)
        {
            var method = smartObject.GetMethod(smartObject.MethodToExecute);
            SmartObjectHelper.VerifyAllReturnPropertiesHasValues(method);
        }

        public static void VerifyPaging(SmartObjectClientServer clientServer, SmartObject smartObject, int pageSize)
        {
            var totalDataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            Assert.IsTrue(totalDataTable.Rows.Count > 0);

            for (int pageNumber = 1; totalDataTable.GetCondition(pageNumber, pageSize); pageNumber++)
            {
                var options = new ExecuteListReaderOptions
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    IncludeTotalRecordCount = (pageNumber % 2) == 1
                };

                var pagedReader = SmartObjectHelper.ExecuteListReader(clientServer, smartObject, options);
                if (options.IncludeTotalRecordCount)
                {
                    Assert.AreEqual(totalDataTable.Rows.Count, pagedReader.TotalRecordCount);
                }
                else
                {
                    Assert.AreEqual(-1, pagedReader.TotalRecordCount);
                }

                var pagedResults = new DataTable();
                pagedResults.Load(pagedReader);

                SmartObjectHelper.CompareDataTables(totalDataTable.GetPagedResult(pageNumber, pageSize),
                    pagedResults);
            }
        }
    }
}