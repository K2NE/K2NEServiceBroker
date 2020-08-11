using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT = SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;
using SourceCode.Workflow.Management;
using System.Data.SqlClient;

using SourceCode.Data.SmartObjectsClient;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.CompressToZip
{
    class FilesToZip : ServiceObjectBase
    {
        public FilesToZip(K2NEServiceBroker api) : base(api) { }



        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.CompressToZip;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> soList = new List<ServiceObject>();

            ServiceObject so = Helper.CreateServiceObject("FilestoZip", "Files To Zip");
            
            //so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.FilesToZip.ZipFile, SoType.File, "Zip File"));
            FileProperty zipFile = new FileProperty(Constants.SOProperties.FilesToZip.ZipFile, new MetaData(), String.Empty, String.Empty);
            zipFile.MetaData.DisplayName = Constants.SOProperties.FilesToZip.ZipFile;
            zipFile.MetaData.Description = "Zip File";
            so.Properties.Add(zipFile);

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.FilesToZip.FileName, SoType.Text, "Zip File Name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.FilesToZip.ADOSMOQuery, SoType.Text, "Query to get the Files. File in the first column"));
           
            //FilesToZip
            Method mFilesToZipSmartObject = Helper.CreateMethod(Constants.Methods.FilesToZip.FilesToZipMethod, "Compress Files to Zip", MethodType.Read);

            mFilesToZipSmartObject.InputProperties.Add(Constants.SOProperties.FilesToZip.ADOSMOQuery);
            mFilesToZipSmartObject.Validation.RequiredProperties.Add(Constants.SOProperties.FilesToZip.ADOSMOQuery);
            mFilesToZipSmartObject.InputProperties.Add(Constants.SOProperties.FilesToZip.FileName);
            mFilesToZipSmartObject.Validation.RequiredProperties.Add(Constants.SOProperties.FilesToZip.FileName);
            mFilesToZipSmartObject.ReturnProperties.Add(Constants.SOProperties.FilesToZip.ZipFile);

            so.Methods.Add(mFilesToZipSmartObject);

            soList.Add(so);

            return soList;
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.FilesToZip.FilesToZipMethod:
                    FilesToZipMethod();
                    break;
            }
        }


        private void FilesToZipMethod()
        {
            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            string fileName = GetStringProperty(Constants.SOProperties.FilesToZip.FileName, true);
            string query = GetStringProperty(Constants.SOProperties.FilesToZip.ADOSMOQuery, true);

            DataTable SOQueryResult = new DataTable();

            using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString))
            using (SOCommand command = new SOCommand(query, connection))
            using (SODataAdapter adapter = new SODataAdapter(command))
            {
                connection.DirectExecution = true;
                adapter.Fill(SOQueryResult);
            }
            string xmlZipFile = string.Empty;

            if (SOQueryResult.Rows.Count > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (DataRow row in SOQueryResult.Rows)
                        {
                            XElement fileXml = XElement.Parse(row[0].ToString());
                            
                            var demoFile = archive.CreateEntry(fileXml.Element("name").Value);

                            using (var entryStream = demoFile.Open())
                            using (var b = new BinaryWriter(entryStream))
                            {
                                b.Write(Convert.FromBase64String(fileXml.Element("content").Value));
                            }
                        }
                    }
                    
                    string content = Convert.ToBase64String(memoryStream.ToArray());
                    xmlZipFile = string.Format("<file><name>{0}.zip</name><content>{1}</content></file>", fileName, content);                    
                }
            }

            DataRow dr = results.NewRow();
            //Calling the helper method with dataresult and expecting a File in return.
            CreateExcel excel = new CreateExcel();
            dr[Constants.SOProperties.FilesToZip.ZipFile] = xmlZipFile;

            results.Rows.Add(dr);

        }
    }
}
