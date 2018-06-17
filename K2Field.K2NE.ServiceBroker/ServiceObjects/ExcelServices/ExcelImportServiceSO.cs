using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using SOC = SourceCode.SmartObjects.Client;
//using SourceCode.SmartObjects.Client;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using Attributes = SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlTypes;
using SourceCode.Workflow.Management;
using K2Field.K2NE.ServiceBroker.Helpers;
using K2Field.K2NE.ServiceBroker.Properties;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.ExcelServices
{
    public class ExcelImportServiceSO: ServiceObjectBase
    {
        public ExcelImportServiceSO(K2NEServiceBroker api) : base(api) { }

        //For Excel cell reference processing
        private static List<char> Letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ' };

        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ExcelService;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> soList = new List<ServiceObject>();

            ServiceObject so = Helper.CreateServiceObject("ExcelImportService", "Excel Import Service SO.");

            FileProperty excelFile = new FileProperty(Constants.SOProperties.ExcelImportServices.ExcelFile, new MetaData(), String.Empty, String.Empty);
            excelFile.MetaData.DisplayName = Constants.SOProperties.ExcelImportServices.ExcelFile;
            excelFile.MetaData.Description = "Excel File";
            so.Properties.Add(excelFile);

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.SmartObject, SoType.Text, "SmartObject Name"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.Results, SoType.Text, "Import Results"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.CreateMethodName, SoType.Text, "Create Method Name.  Default is Create if not specified."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.HeaderRowSpaces, SoType.Text, "Action to take (Remove or Replace) when a space character is found in column names in the header row."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.TransactionIDName, SoType.Text, "This allows you to specify an optional ID property name on the SmartObject which identifies the uploaded records."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.TransactionIDValue, SoType.Text, "This allows you to specify an optional transaction ID value to identify the uploaded records."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.FQN, SoType.Text, "Logging the user who load the Excel data."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelImportServices.SheetName, SoType.Text, "The name of the Sheet. By default getting the first sheet."));

            //UploadExcelDataToASmartObject
            Method mUploadExcelDataToASmartObject = Helper.CreateMethod(Constants.Methods.ExcelImportServices.UploadExcelDataToASmartObject, "Upload Excel Data to a SmartObject", MethodType.Read);
            mUploadExcelDataToASmartObject.ReturnProperties.Add(Constants.SOProperties.ExcelImportServices.Results);
            
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.ExcelFile);
            mUploadExcelDataToASmartObject.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelImportServices.ExcelFile);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.SheetName);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.SmartObject);
            mUploadExcelDataToASmartObject.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelImportServices.SmartObject);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.CreateMethodName);            
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.HeaderRowSpaces);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.TransactionIDName);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.TransactionIDValue);
            mUploadExcelDataToASmartObject.InputProperties.Add(Constants.SOProperties.ExcelImportServices.FQN);
            
            so.Methods.Add(mUploadExcelDataToASmartObject);
            
            soList.Add(so);

            return soList;
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ExcelImportServices.UploadExcelDataToASmartObject:
                    UploadExcelDataToASmartObject();
                    break;
            }
        }

        private void UploadExcelDataToASmartObject()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelImportServices.ExcelFile, true);
            string smartObject = GetStringProperty(Constants.SOProperties.ExcelImportServices.SmartObject, true);
            string createMethodName = GetStringProperty(Constants.SOProperties.ExcelImportServices.CreateMethodName, false);
            string headerRowSpaces = GetStringProperty(Constants.SOProperties.ExcelImportServices.HeaderRowSpaces, false);
            string transactionIDName = GetStringProperty(Constants.SOProperties.ExcelImportServices.TransactionIDName, false);
            string transactionIDValue = GetStringProperty(Constants.SOProperties.ExcelImportServices.TransactionIDValue, false);
            string FQN = GetStringProperty(Constants.SOProperties.ExcelImportServices.FQN, false);
            string sheetName = GetStringProperty(Constants.SOProperties.ExcelImportServices.SheetName, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            
            string result = Import(excelFile, sheetName, smartObject, createMethodName, headerRowSpaces, transactionIDName, transactionIDValue, FQN);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.ExcelImportServices.Results] = result;

            results.Rows.Add(dr);
        }

        /// <summary>
        /// Import data from Excel file into SmartObject
        /// </summary>
        private string Import(FileProperty excelFile, string sheetName, string smartObject, string createMethodName, string headerRowSpaces, string transactionIDName,
            string transactionIDValue, string FQN)
        {
            string result = string.Empty;

            byte[] bFile = Convert.FromBase64String(excelFile.Content);
            MemoryStream stream = new MemoryStream(bFile);

            // arrays to store the imported column names and also the SmartObject column names
            // this will be used later to get a list of matching columns.
            string[] dsColumnNames = null, soColumnNames = null;

            // To store returned values from Excel file
            DataTable dt;

            // Read Data in excel file
            try
            {
                dt = ReadExcelFile(stream, sheetName);

                if (dt.Rows.Count == 0)
                {
                    result = "0 rows imported";
                    return result;
                }
                else
                {
                    result = dt.Rows.Count.ToString() + " rows found. ";
                    if (dt.Columns.Count == 0)
                    {
                        result = "0 columns found.";
                        return result;
                    }
                    else
                    {
                        // populate an array of column names
                        dsColumnNames = new string[dt.Columns.Count];
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (headerRowSpaces != null && headerRowSpaces.Trim().ToLower() == "remove")
                            {
                                col.ColumnName = col.ColumnName.Replace(" ", "");
                            }
                            else  // by default replace spaces with underscores as SmartObject system names do that
                            {
                                col.ColumnName = col.ColumnName.Replace(" ", "_");
                            }
                            // just get rid of other (non-underscore or hyphen) punctuation which is also invalid
                            col.ColumnName = Regex.Replace(col.ColumnName, @"[\p{P}\p{S}-[-_]]", "");
                            dsColumnNames[col.Ordinal] = col.ColumnName.ToLower();
                        }
                        result += "Columns found: " + string.Join(",", dsColumnNames);
                    }
                }
            }
            catch (IOException ex)
            {
                result = "Unable to Read from Excel File: " + ex.Message;
                return result;
            }

            // If able to read data from Excel File, continue to bulk insert into SmartObject.
            try
            {
                SOC.SmartObjectClientServer smoClientServer = this.ServiceBroker.K2Connection.GetConnection<SOC.SmartObjectClientServer>();

                // get the list of columns from the SmartObject, call the Create method by default
                using (smoClientServer.Connection)
                {
                    SOC.SmartObject soImport = smoClientServer.GetSmartObject(smartObject);

                    // populate an array of column names
                    soColumnNames = new string[soImport.Properties.Count];
                    for (int i = 0; i < soImport.Properties.Count; i++)
                    {
                        soColumnNames[i] = soImport.Properties[i].Name.ToLower();
                    }

                    var arrMatchingCols = dsColumnNames.Join(soColumnNames, dscol => dscol, socol => socol, (dscol, socol) => socol).ToList();

                    if (arrMatchingCols.Count == 0)
                    {
                        result = "No matching columns found in SmartObject and Excel Data.";
                        return result;
                    }

                    // Bulk Insert into SmartObject
                    try
                    {
                        using (SOC.SmartObjectList inputList = new SOC.SmartObjectList())
                        {
                            // If the CreateMethodName is not specified, use the default value of "Create"
                            if (createMethodName == null || createMethodName == string.Empty)
                            {
                                soImport.MethodToExecute = "Create";
                            }
                            else
                            {
                                soImport.MethodToExecute = createMethodName;
                            }

                            string sTransactionIDName = string.Empty;
                            if (!(transactionIDName == null || transactionIDName == string.Empty))
                            {
                                sTransactionIDName = transactionIDName.ToLower();
                            }

                            string sTransactionIDValue = string.Empty;
                            if (!(transactionIDValue == null || transactionIDValue == string.Empty))
                            {
                                sTransactionIDValue = transactionIDValue;
                            }

                            if (arrMatchingCols.Contains(sTransactionIDName))
                            {
                                throw new ApplicationException(Resources.ExcelImportTransactionIDNameExist); 
                            }

                            if (arrMatchingCols.Contains("FQN"))
                            {
                                throw new ApplicationException(Resources.ExcelImportFQNExist);
                            }

                            foreach (DataRow dr in dt.Rows)
                            {
                                SOC.SmartObject newSmartObject = soImport.Clone();
                                // loop through collection of matching fields and insert the values
                                foreach (string sColName in arrMatchingCols)
                                {
                                    // for handling date and datetime types
                                    DateTime tmpDate;
                                    int nonblankCharCount = dr[sColName].ToString().Length;  //It is plausible that there will valid cases of null/empty cells

                                    // handle Date amd DateTime columns correctly.  Convert Excel datetime format (double) to .NET DateTime type
                                    if (nonblankCharCount > 0 && newSmartObject.Properties[sColName].Type == SOC.PropertyType.DateTime) // DateTime column
                                    {
                                        tmpDate = DateTime.FromOADate(Convert.ToDouble(dr[sColName].ToString()));
                                        newSmartObject.Properties[sColName].Value = String.Concat(tmpDate.ToShortDateString(), " ", tmpDate.ToShortTimeString());
                                    }
                                    else if (nonblankCharCount > 0 && newSmartObject.Properties[sColName].Type == SOC.PropertyType.Date) // Date column
                                    {
                                        tmpDate = DateTime.FromOADate(Convert.ToDouble(dr[sColName].ToString()));
                                        newSmartObject.Properties[sColName].Value = tmpDate.ToShortDateString();
                                    }
                                    else if (nonblankCharCount > 0 && newSmartObject.Properties[sColName].Type == SOC.PropertyType.Time) // Time column
                                    {
                                        tmpDate = DateTime.FromOADate(Convert.ToDouble(dr[sColName].ToString()));
                                        newSmartObject.Properties[sColName].Value = tmpDate.ToShortTimeString();
                                    }
                                    else // not a Date or DateTime column
                                    {
                                        newSmartObject.Properties[sColName].Value = dr[sColName].ToString();
                                    }
                                }

                                // Add the transaction ID value for identification purposes.
                                if (sTransactionIDName != string.Empty && sTransactionIDValue != string.Empty)
                                {
                                    // replace spaces with underscores as SmartObject system names do that
                                    newSmartObject.Properties[sTransactionIDName.Replace(" ", "_")].Value = sTransactionIDValue;
                                }

                                if (!string.IsNullOrEmpty(FQN))
                                {
                                    newSmartObject.Properties["FQN"].Value = FQN;
                                }

                                inputList.SmartObjectsList.Add(newSmartObject);
                            }
                            try
                            {
                                smoClientServer.ExecuteBulkScalar(soImport, inputList);
                            }
                            catch (SOC.SmartObjectException ex)
                            {
                                result = ex.Message + ex.StackTrace;
                                foreach (SOC.SmartObjectExceptionData smOExBrokerData in ex.BrokerData)
                                {
                                    result += smOExBrokerData.Message + "  Before failure, " + result;
                                }
                                return result;
                            }

                            // Indicate the Transaction ID name and value in the results field
                            if (sTransactionIDName != string.Empty && sTransactionIDValue != string.Empty)
                            {
                                result += ". " + sTransactionIDName + ":" + sTransactionIDValue;
                            }
                        }
                        // free objects
                        arrMatchingCols = null;
                        soImport = null;
                    }
                    catch (ApplicationException ex)
                    {
                        result = "Unable to insert data into SmartObject: " + ex.Message;
                        return result;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                result = "Unable to connect to K2 server: " + ex.Message;
            }
            return result;
        }


        #region Helper functions

        /// <summary>
        ///  Read Data from selected excel file on client
        /// </summary>
        /// <param name="stream">The whole source Excel file passed in through memory stream, usually the server will do this.</param>
        /// <returns>Successfully extracted rows.</returns>
        private DataTable ReadExcelFile(MemoryStream stream, string sheetName)
        {
            // Initializate an instance of DataTable
            DataTable dt = new DataTable();

            try
            {
                // Use SpreadSheetDocument class of Open XML SDK to open excel file
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
                {
                    // Get Workbook Part of Spread Sheet Document
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                    // Get all sheets in spread sheet document 
                    IEnumerable<Sheet> sheetcollection = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                    // Get relationship Id
                    string relationshipId = string.Empty;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        relationshipId = sheetcollection.First().Id.Value;
                    }
                    else
                    {
                        Sheet sheet = sheetcollection.Where(sh => string.Compare(sh.Name, sheetName) == 0).FirstOrDefault();
                        if(sheet != null)
                        {
                            relationshipId = sheet.Id.Value;
                        }
                        else
                        {
                            throw new ApplicationException(string.Format(Resources.ExcelImportSheetNotExist, sheetName));
                        }
                    }

                    // Get sheet1 Part of Spread Sheet Document
                    WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);

                    // Get Data in Excel file
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();

                    if (rowcollection.Count() == 0)
                    {
                        return dt;
                    }

                    // Add columns (identified by a conventional header row)
                    foreach (Cell cell in rowcollection.ElementAt(0))
                    {
                        dt.Columns.Add(GetValueOfCell(spreadsheetDocument, cell));
                    }

                    // Add rows into DataTable
                    foreach (Row row in rowcollection)
                    {
                        DataRow temprow = dt.NewRow();
                        int currentColumnIndex = 0;
                        for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                        {
                            //Excel does not create an element for empty cells. This can lead to apparently "offset" data in the row. See http://stackoverflow.com/questions/3837981/reading-excel-open-xml-is-ignoring-blank-cells
                            //Overcome this with some refe
                            int cellColumnIndex = (int)GetColumnIndexFromRefName(GetColumnRefName(row.Descendants<Cell>().ElementAt(i).CellReference));
                            while (currentColumnIndex < cellColumnIndex)
                            {   //then we need to recreate the blank that was skipped over by Excel's efficient non-storage of empties
                                temprow[currentColumnIndex] = string.Empty;
                                currentColumnIndex++;
                            }

                            temprow[currentColumnIndex] = GetValueOfCell(spreadsheetDocument, row.Descendants<Cell>().ElementAt(i));
                            currentColumnIndex++;
                        }

                        // Add the row to DataTable
                        // note the rows include header row
                        if (!AreAllColumnsEmpty(temprow))
                            dt.Rows.Add(temprow);
                    }
                }

                // Here remove header row
                dt.Rows.RemoveAt(0);
                return dt;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }

        /// <summary>
        ///  Get Value in Cell 
        /// </summary>
        /// <param name="spreadsheetdocument">SpreadSheet Document</param>
        /// <param name="cell">Cell in SpreadSheet Document</param>
        /// <returns>The value in cell</returns>
        private static string GetValueOfCell(SpreadsheetDocument spreadsheetdocument, Cell cell)
        {
            // Get value in Cell
            SharedStringTablePart sharedString = spreadsheetdocument.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return string.Empty;
            }

            string cellValue = cell.CellValue.InnerText;

            // The condition that the Cell DataType is SharedString
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
            }
            else
            {
                return cellValue;
            }
        }


        /// <summary>
        /// Given a letter-number cell name reference, parses the specified cell element to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        private static string GetColumnRefName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed. But if three is believed to be needed, question your requirements strongly!
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromRefName(string columnName)
        {
            int? columnIndex = null;
            
            char[] colLetters = columnName.ToCharArray();

            if (colLetters.Count() <= 2)
            {

                int index = 0;
                foreach (char col in colLetters)
                {
                    int? indexValue = Letters.IndexOf(col);

                    if (indexValue != -1)
                    {
                        // The first letter of a two digit column needs some extra calculations
                        if (index == 0 && colLetters.Count() == 2)
                        {
                            columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
                        }
                        else
                        {
                            columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
                        }
                    }

                    index++;
                }
            }

            return columnIndex;
        }

        private static bool AreAllColumnsEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }
            else
            {
                foreach (var value in dr.ItemArray)
                {
                    if (value != null && !string.IsNullOrEmpty(value as string))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion
    }
}
