using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SourceCode.Hosting.Client.BaseAPI;
//using SourceCode.SmartObjects.Client;
//using SourceCode.SmartObjects.Client.Filters;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Client;
using System.Text.RegularExpressions;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public class ExcelServicesHelper
    {        
        public static string GetCellValueFromString(string fileContent, string worksheetName, string cellCoordinates)
        {
            string value = null;
            MemoryStream fileStream = new MemoryStream(System.Convert.FromBase64String(fileContent), true);

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName).FirstOrDefault();

                if (theSheet == null) { throw new ArgumentException("This Worksheet doesn't exist."); }

                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
                Cell theCell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == cellCoordinates).FirstOrDefault();

                if (theCell != null)
                {
                    value = theCell.CellValue.InnerText;
                    if (theCell.DataType != null)
                    {
                        switch (theCell.DataType.Value)
                        {
                            case CellValues.SharedString:
                                var stringTable =
                                    wbPart.GetPartsOfType<SharedStringTablePart>()
                                    .FirstOrDefault();
                                if (stringTable != null)
                                {
                                    value =
                                        stringTable.SharedStringTable
                                        .ElementAt(int.Parse(value)).InnerText;
                                }
                                break;

                            case CellValues.Boolean:
                                switch (value)
                                {
                                    case "0":
                                        value = "FALSE";
                                        break;
                                    default:
                                        value = "TRUE";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return value;
        }
        public static string SaveCellValueToString(string fileContent, string worksheetName, string cellCoordinates, string cellValue)
        {
            MemoryStream fileStream = new MemoryStream();
            fileStream.Write(System.Convert.FromBase64String(fileContent), 0, int.Parse(System.Convert.FromBase64String(fileContent).Length.ToString()));

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, true))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName).FirstOrDefault();
                if (theSheet == null) { throw new ArgumentException("This Worksheet doesn't exist."); }

                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
                Cell theCell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == cellCoordinates).FirstOrDefault();
                if (theCell == null)
                {
                    theCell = InsertCellInWorksheet(GetColumnName(cellCoordinates), GetRowIndex(cellCoordinates), wsPart);
                }

                theCell.CellValue = new CellValue(cellValue);
                //theCell.DataType = new EnumValue<CellValues>(CellValues.String);
                theCell.DataType = DefineCellDataType(cellValue);

                wsPart.Worksheet.Save();
                wbPart.Workbook.Save();

                byte[] fileByte = fileStream.ToArray();
                fileContent = System.Convert.ToBase64String(fileByte).ToString();
            }

            return fileContent;
        }
        
        public static string SaveMultipleCellValuesToString(string fileContent, string worksheetName, string multipleCellCoordinates, string multipleCellValue)
        {
            MemoryStream fileStream = new MemoryStream();
            fileStream.Write(System.Convert.FromBase64String(fileContent), 0, int.Parse(System.Convert.FromBase64String(fileContent).Length.ToString()));

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, true))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName).FirstOrDefault();
                if (theSheet == null) { throw new ArgumentException("This Worksheet doesn't exist."); }

                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
                
                string[] cellCoordinates = multipleCellCoordinates.Trim().Split(';');
                string[] cellValues = multipleCellValue.Trim().Split(';');

                for (int i = 0; i < cellCoordinates.Length; i++)
                {
                    if (cellValues[i] != null)
                    {
                        Cell theCell =
                            wsPart.Worksheet.Descendants<Cell>()
                                .Where(c => c.CellReference == cellCoordinates[i])
                                .FirstOrDefault();
                        if (theCell == null)
                        {
                            theCell = InsertCellInWorksheet(GetColumnName(cellCoordinates[i]),
                                GetRowIndex(cellCoordinates[i]),
                                wsPart);
                        }

                        theCell.CellValue = new CellValue(cellValues[i]);
                        theCell.DataType = DefineCellDataType(cellValues[i]);


                    }
                }

                wsPart.Worksheet.Save();
                wbPart.Workbook.Save();

                byte[] fileByte = fileStream.ToArray();
                fileContent = System.Convert.ToBase64String(fileByte).ToString();
            }

            return fileContent;
        }

        public static string GetMultipleCellValueFromString(string fileContent, string worksheetName, string multipleCellCoordinates)
        {
            string values = null;
            MemoryStream fileStream = new MemoryStream(System.Convert.FromBase64String(fileContent), true);

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName).FirstOrDefault();

                if (theSheet == null) { throw new ArgumentException("This Worksheet doesn't exist."); }

                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));

                //TODO
                string[] cellCoordinates = multipleCellCoordinates.Trim().Split(';');

                for (int i = 0; i < cellCoordinates.Length; i++)
                {
                    Cell theCell =
                        wsPart.Worksheet.Descendants<Cell>()
                            .Where(c => c.CellReference == cellCoordinates[i])
                            .FirstOrDefault();

                    string value = String.Empty;

                    if (theCell != null)
                    {
                        value = theCell.CellValue.InnerText;
                        if (theCell.DataType != null)
                        {
                            switch (theCell.DataType.Value)
                            {
                                case CellValues.SharedString:
                                    var stringTable =
                                        wbPart.GetPartsOfType<SharedStringTablePart>()
                                            .FirstOrDefault();
                                    if (stringTable != null)
                                    {
                                        value =
                                            stringTable.SharedStringTable
                                                .ElementAt(int.Parse(value)).InnerText;
                                    }
                                    break;

                                case CellValues.Boolean:
                                    switch (value)
                                    {
                                        case "0":
                                            value = "FALSE";
                                            break;
                                        default:
                                            value = "TRUE";
                                            break;
                                    }
                                    break;
                            }
                        }

                    }
                    if (!String.IsNullOrEmpty(values))
                    {
                        values += ';' + value;
                    }
                    else
                    {
                        values += value;
                    }
                }
            }
            return values;
        }

        public static Sheets GetSheetNamesFromString(string fileString)
        {
            XDocument xdoc = XDocument.Parse(fileString, LoadOptions.None);
            fileString = xdoc.Root.Elements("content").First().Value;
            MemoryStream fileStream = new MemoryStream(System.Convert.FromBase64String(fileString), true);
            Sheets theSheets = null;

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileStream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                theSheets = wbPart.Workbook.Sheets;
            }
            return theSheets;
        }

        public static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
        
        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
        private static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        private static EnumValue<CellValues> DefineCellDataType(string cellValue)
        {
            //check date type
            //DateTime dateTime;
            //if(DateTime.TryParse(cellValue, out dateTime))
            //{
            //   return new EnumValue<CellValues>(CellValues.Date);
            //}

            ////check bool
            //bool logic;
            //if(Boolean.TryParse(cellValue, out logic))
            //{
            //    return new EnumValue<CellValues>(CellValues.Boolean);
            //}

            decimal number;
            if(Decimal.TryParse(cellValue, out number))
            {
                return new EnumValue<CellValues>(CellValues.Number);
            }

            return new EnumValue<CellValues>(CellValues.String);
        }
    }
}
