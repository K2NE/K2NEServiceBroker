using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.ExcelServices
{
    public class ExcelDocumentServicesSO : ServiceObjectBase
    {
        public ExcelDocumentServicesSO(K2NEServiceBroker api) : base(api) { }

        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ExcelServices;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> soList = new List<ServiceObject>();

            ServiceObject so = Helper.CreateServiceObject("ExcelDocumentServices", "Excel Document Services SMO.");

            FileProperty excelFile = new FileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, new MetaData(), String.Empty, String.Empty);
            excelFile.MetaData.DisplayName = Constants.SOProperties.ExcelDocumentServices.ExcelFile;
            excelFile.MetaData.Description = "Excel File";
            so.Properties.Add(excelFile);

            FileProperty updatedExcelFile = new FileProperty(Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile, new MetaData(), String.Empty, String.Empty);
            updatedExcelFile.MetaData.DisplayName = Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile;
            updatedExcelFile.MetaData.Description = "Excel File";
            so.Properties.Add(updatedExcelFile);

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, SoType.Text, "The name of the sheet"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.CellCoordinates, SoType.Text, "Coordinates of the cell"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.CellName, SoType.Text, "Name of the cell"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.CellValue, SoType.Memo, "Value of the cell"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates, SoType.Text, "Multiple coordinates of the cell separated by semicolons"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellValues, SoType.Memo, "Multiple values of the cell separated by semicolons"));

            //GetCellValue
            Method mGetCellValue = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.GetCellValue, "Returns the value of the cell", MethodType.Read);
            mGetCellValue.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellValue);

            mGetCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellCoordinates);
            mGetCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellCoordinates);

            so.Methods.Add(mGetCellValue);

            //SaveCellValue
            Method mSaveCellValue = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.SaveCellValue, "Save the new value in the cell.", MethodType.Update);
            mSaveCellValue.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile);

            mSaveCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mSaveCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mSaveCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mSaveCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mSaveCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellCoordinates);
            mSaveCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellCoordinates);
            mSaveCellValue.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellValue);
            mSaveCellValue.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellValue);

            so.Methods.Add(mSaveCellValue);

            //GetWorksheetNames
            Method mGetWorksheetNames = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.GetWorkSheetNames, "Returns the sequence of sheet names.", MethodType.List);
            mGetWorksheetNames.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);

            mGetWorksheetNames.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetWorksheetNames.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);

            so.Methods.Add(mGetWorksheetNames);

            //SaveMultipleCellValues
            Method mSaveMultipleCellValues = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.SaveMultipleCellValues, "Save the new multiple values in the cell", MethodType.Update);
            mSaveMultipleCellValues.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile);

            mSaveMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mSaveMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mSaveMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mSaveMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mSaveMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);
            mSaveMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);
            mSaveMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellValues);
            mSaveMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellValues);

            so.Methods.Add(mSaveMultipleCellValues);

            //GetMultipleCellValues
            Method mGetMultipleCellValues = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.GetMultipleCellValues, "Save the new multiple values in the cell", MethodType.Read);
            mGetMultipleCellValues.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellValues);

            mGetMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetMultipleCellValues.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);
            mGetMultipleCellValues.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);

            so.Methods.Add(mGetMultipleCellValues);

            //GetMultipleCellValuesList
            Method mGetMultipleCellValuesList = Helper.CreateMethod(Constants.Methods.ExcelDocumentServices.GetMultipleCellValuesList, "Save the new multiple values in the cell", MethodType.List);
            mGetMultipleCellValuesList.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellName);
            mGetMultipleCellValuesList.ReturnProperties.Add(Constants.SOProperties.ExcelDocumentServices.CellValue);

            mGetMultipleCellValuesList.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetMultipleCellValuesList.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.ExcelFile);
            mGetMultipleCellValuesList.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetMultipleCellValuesList.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.WorksheetName);
            mGetMultipleCellValuesList.InputProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);
            mGetMultipleCellValuesList.Validation.RequiredProperties.Add(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates);

            so.Methods.Add(mGetMultipleCellValuesList);

            soList.Add(so);


            return soList;
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ExcelDocumentServices.GetCellValue:
                    GetCellValue();
                    break;
                case Constants.Methods.ExcelDocumentServices.SaveCellValue:
                    SaveCellValue();
                    break;
                case Constants.Methods.ExcelDocumentServices.GetWorkSheetNames:
                    GetWorkSheetNames();
                    break;
                case Constants.Methods.ExcelDocumentServices.GetMultipleCellValues:
                    GetMultipleCellValues();
                    break;
                case Constants.Methods.ExcelDocumentServices.SaveMultipleCellValues:
                    SaveMultipleCellValues();
                    break;
                case Constants.Methods.ExcelDocumentServices.GetMultipleCellValuesList:
                    GetMultipleCellValuesList();
                    break;

            }
        }

        private void GetCellValue()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);
            string worksheetName = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, true);
            string cellCoordinates = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.CellCoordinates, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                string smoCellValue = ExcelServicesHelper.GetCellValueFromString(excelFile.Content, worksheetName, cellCoordinates);

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ExcelDocumentServices.CellValue] = smoCellValue;

                results.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveCellValue()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);
            string worksheetName = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, true);
            string cellCoordinates = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.CellCoordinates, true);
            string cellValue = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.CellValue, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                excelFile.Content = ExcelServicesHelper.SaveCellValueToString(excelFile.Content, worksheetName, cellCoordinates, cellValue);

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile] = excelFile.Value;

                results.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetWorkSheetNames()
        {
            //Get input properties
            string excelFile = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                Sheets smoWorksheets = ExcelServicesHelper.GetSheetNamesFromString(excelFile);

                foreach (Sheet sheet in smoWorksheets)
                {
                    DataRow dr = results.NewRow();
                    dr[Constants.SOProperties.ExcelDocumentServices.WorksheetName] = sheet.Name;
                    results.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetMultipleCellValues()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);
            string worksheetName = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, true);
            string multipleCellCoordinates = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                string smoCellValues = ExcelServicesHelper.GetMultipleCellValueFromString(excelFile.Content, worksheetName, multipleCellCoordinates);

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ExcelDocumentServices.MultipleCellValues] = smoCellValues;

                results.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetMultipleCellValuesList()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);
            string worksheetName = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, true);
            string multipleCellCoordinates = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                string smoCellValues = ExcelServicesHelper.GetMultipleCellValueFromString(excelFile.Content, worksheetName, multipleCellCoordinates);

                //DataRow dr = results.NewRow();

                string[] cellNames = multipleCellCoordinates.Split(';');
                string[] cellValues = smoCellValues.Split(';');

                for(int i = 0; i< cellNames.Length; i++)
                {
                    DataRow dr = results.NewRow();

                    dr[Constants.SOProperties.ExcelDocumentServices.CellName] = cellNames[i];
                    dr[Constants.SOProperties.ExcelDocumentServices.CellValue] = cellValues[i];
                    results.Rows.Add(dr);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveMultipleCellValues()
        {
            //Get input properties
            FileProperty excelFile = GetFileProperty(Constants.SOProperties.ExcelDocumentServices.ExcelFile, true);
            string worksheetName = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.WorksheetName, true);
            string multipleCellCoordinates = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellCoordinates, true);
            string multipleCellValues = GetStringProperty(Constants.SOProperties.ExcelDocumentServices.MultipleCellValues, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            try
            {
                excelFile.Content = ExcelServicesHelper.SaveMultipleCellValuesToString(excelFile.Content, worksheetName, multipleCellCoordinates, multipleCellValues);

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ExcelDocumentServices.UpdatedExcelFile] = excelFile.Value;

                results.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
