using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Reflection;//
using System.Security;//
using System.Security.Policy;//
using DocumentFormat.OpenXml;//
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Xml.XPath;
using K2Field.K2NE.ServiceBroker.Properties;


namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public class CreateExcel
    {
        private UInt32Value _dateStyleId;
        private UInt32Value _NumbStyleId;
        public CreateExcel()
        {
        }

        /// <summary>
        /// Method to create a filename and provide a file as an ouput string.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ConvertDataTable2Excelfile(DataTable results, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(Resources.FileNameIsRequired);
            }
            try
            {
                if (!fileName.EndsWith(".xlsx"))
                {
                    fileName += ".xlsx";
                }


                byte[] objByte = ExportToExcel(results);
                string content = Convert.ToBase64String(objByte, 0, objByte.Count(), Base64FormattingOptions.None);
                return string.Format("<file><name>{0}</name><content>{1}</content></file>", fileName, content);
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.ErrorCreatingExcelFile, ex);
            }
        }

        private List<string> GetCellHeaders(DataTable results)
        {
            List<string> cellHeaders = new List<string>();
            for (int i = 0; i < results.Columns.Count; i++)
            {
                cellHeaders.Add(GetExcelColumnName(i + 1));
            }
            return cellHeaders;
        }

        /// <summary>
        /// Returns the column caption for the given row & column index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        private string GetCellReference(int columnIndex, int rowIndex, List<string> cHeaders)
        {
            return cHeaders.ElementAt(columnIndex) + (rowIndex + 1).ToString();
        }

        /// <summary>
        ///  Generate an excel file with data and return as array of byte
        /// </summary>
        /// <param name="datatable">DataTable object</param>
        /// <param name="filepath">The Path of exported excel file</param>
        private byte[] ExportToExcel(DataTable datatable)
        {
            MemoryStream mem = new MemoryStream();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook))
            {
                // Initialize an instance of WorkbookPart
                WorkbookPart workBookPart = spreadsheetDocument.AddWorkbookPart();

                // Create WorkBook 
                CreateWorkBookPart(workBookPart);

                // Add styling that we need.
                WorkbookStylesPart workbookStylesPart = workBookPart.AddNewPart<WorkbookStylesPart>("rId3");
                CreateWorkBookStylesPart(workbookStylesPart);

                // Add WorkSheetPart into WorkBook
                WorksheetPart worksheetPart1 = workBookPart.AddNewPart<WorksheetPart>("rId1");
                CreateWorkSheetPart(worksheetPart1, datatable);

                // Save workbook
                workBookPart.Workbook.Save();
            }

            return mem.ToArray();
        }
        private void CreateWorkBookStylesPart(WorkbookStylesPart workbookStylesPart)
        {
            Stylesheet styleSheet = new Stylesheet();

            Fonts fonts = new Fonts();
            fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()
            {
                FontName = new FontName() { Val = "Calibri" },
                FontSize = new FontSize() { Val = 11 },
                FontFamilyNumbering = new FontFamilyNumbering() { Val = 2 },
            });
            fonts.Count = (uint)fonts.ChildElements.Count;

            Fills fills = new Fills();
            fills.Append(new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.None }
            });

            fills.Count = (uint)fills.ChildElements.Count;

            // Create "borders" node.
            Borders borders = new Borders();
            borders.Append(new Border()
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            });

            borders.Count = (uint)borders.ChildElements.Count;

            // Create "cellStyleXfs" node.
            var CellStyleFormats = new CellStyleFormats();
            CellStyleFormats.Append(new CellFormat()
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 0,
                BorderId = 0
            });
            CellStyleFormats.Count = (uint)CellStyleFormats.ChildElements.Count;

            // Create "cellXfs" node.
            var CellFormats = new CellFormats();
            CellFormats.Append(new CellFormat()
            {
                BorderId = 0,
                FillId = 0,
                FontId = 0,
                FormatId = 0,
                NumberFormatId = 0,
                //ApplyNumberFormat = true
            });

            CellFormats.Count = (uint)CellFormats.ChildElements.Count;



            // Create "cellStyles" node.
            var CellStyles = new CellStyles();
            CellStyles.Append(new CellStyle()
            {
                Name = "Normal",
                FormatId = 0,
                BuiltinId = 0
            });
            CellStyles.Count = (uint)CellStyles.ChildElements.Count;

            //// Append all nodes in order.
            styleSheet.Append(fonts);
            styleSheet.Append(fills);
            styleSheet.Append(borders);
            styleSheet.Append(CellStyleFormats);
            styleSheet.Append(CellFormats);
            styleSheet.Append(CellStyles);

            _dateStyleId = CreateCellFormat(styleSheet, null, null, UInt32Value.FromUInt32(22));
            _NumbStyleId = CreateCellFormat(styleSheet, null, null, UInt32Value.FromUInt32(1));

            // Set the style of workbook
            workbookStylesPart.Stylesheet = styleSheet;
        }

        /// <summary>
        /// Create an Workbook instance and add its children
        /// </summary>
        /// <param name="workbookPart">WorkbookPart Object</param>
        private void CreateWorkBookPart(WorkbookPart workbookPart)
        {
            Workbook workbook = new Workbook();
            Sheets sheets = new Sheets();

            // Initilize an instance of Sheet Object
            Sheet sheet1 = new Sheet()
            {
                Name = "Sheet1",
                SheetId = 1,
                Id = "rId1"
            };

            // Add the sheet into sheets collection
            sheets.Append(sheet1);

            CalculationProperties calculationProperties1 = new CalculationProperties()
            {
                CalculationId = (UInt32Value)111222U
            };

            // Add elements into workbook
            workbook.Append(sheets);
            workbook.Append(calculationProperties1);
            workbookPart.Workbook = workbook;
        }

        /// <summary>
        ///  Generates content of worksheetPart
        /// </summary>
        /// <param name="worksheetPart">WorksheetPart Object</param>
        /// <param name="table">DataTable Object</param>
        private void CreateWorkSheetPart(WorksheetPart worksheetPart, DataTable table)
        {
            // Initialize worksheet and set the properties
            Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            SheetViews sheetViews1 = new SheetViews();

            // Initialize an instance of the sheetview class
            SheetView sheetView1 = new SheetView()
            {
                WorkbookViewId = (UInt32Value)0U
            };

            Selection selection = new Selection() { ActiveCell = "A1" };
            sheetView1.Append(selection);

            sheetViews1.Append(sheetView1);
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties()
            {
                DefaultRowHeight = 15D,
                DyDescent = 0.25D
            };

            SheetData sheetData1 = new SheetData();
            UInt32Value rowIndex = 1U;
            PageMargins pageMargins1 = new PageMargins()
            {
                Left = 0.7D,
                Right = 0.7D,
                Top = 0.75D,
                Bottom = 0.75D,
                Header = 0.3D,
                Footer = 0.3D
            };

            Row headerRow = new Row()
            {
                RowIndex = rowIndex++,
                Spans = new ListValue<StringValue>() { InnerText = "1:3" },
                DyDescent = 0.25D
            };


            List<string> cellHeaders = GetCellHeaders(table);

            // Add columns in DataTable to columns collection of SpreadSheet Document 
            for (int columnindex = 0; columnindex < table.Columns.Count; columnindex++)
            {
                Cell cell = new Cell()
                {
                    CellReference = GetCellReference(columnindex, (Convert.ToInt32((UInt32)rowIndex) - 2), cellHeaders),
                    DataType = GetCellType(table.Columns[columnindex].ColumnName.GetType().ToString())
                };

                // Get Value of DataTable and append the value to cell of spreadsheet document
                CellValue cellValue = new CellValue();
                cellValue.Text = table.Columns[columnindex].ColumnName.ToString();
                cell.Append(cellValue);

                headerRow.Append(cell);
            }

            // Add row to sheet
            sheetData1.Append(headerRow);

            // Add rows in DataTable to rows collection of SpreadSheet Document 
            for (int rIndex = 0; rIndex < table.Rows.Count; rIndex++)
            {
                Row dataRow = new Row()
                {
                    RowIndex = rowIndex++,
                    Spans = new ListValue<StringValue>() { InnerText = "1:3" },
                    DyDescent = 0.25D
                };


                for (int cIndex = 0; cIndex < table.Columns.Count; cIndex++)
                {
                    Cell cell = new Cell();
                    cell.CellReference = GetCellReference(cIndex, (Convert.ToInt32((UInt32)rowIndex) - 2), cellHeaders);
                    cell.DataType = GetCellType(table.Rows[rIndex][cIndex].GetType());
                    cell.CellValue = new CellValue();

                    if (table.Rows[rIndex][cIndex].GetType() == typeof(System.DateTime))
                    {
                        cell.CellValue.Text = Convert.ToDateTime(table.Rows[rIndex][cIndex].ToString()).ToOADate().ToString();
                        cell.StyleIndex = _dateStyleId;
                    }
                    else if (table.Rows[rIndex][cIndex].GetType() == typeof(System.Int64) || table.Rows[rIndex][cIndex].GetType() == typeof(System.Int32))
                    {
                        cell.CellValue.Text = table.Rows[rIndex][cIndex].ToString();
                        cell.StyleIndex = _NumbStyleId;
                    }
                    else
                    {
                        cell.CellValue.Text = table.Rows[rIndex][cIndex].ToString();
                    }

                    dataRow.Append(cell);
                }

                // Add row to Sheet Data
                sheetData1.Append(dataRow);
            }

            // Add elements to worksheet
            worksheet1.Append(sheetViews1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
            worksheet1.Append(pageMargins1);

            worksheetPart.Worksheet = worksheet1;
        }

        //Method to assign cell proper datatype as per the data.
        private CellValues GetCellType(string col)
        {
            switch (col)
            {
                case "System.DateTime":
                    return DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                case "System.Decimal":
                case "System.Double":
                case "System.Int64":
                case "System.Int32":
                    return DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                default:
                    return DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            }

        }




        private UInt32Value CreateCellFormat(Stylesheet styleSheet, UInt32Value fontIndex, UInt32Value fillIndex, UInt32Value numberFormatId)
        {
            CellFormat cellFormat = new CellFormat();

            if (fontIndex != null)
            {
                cellFormat.FontId = fontIndex;
            }

            if (fillIndex != null)
            {
                cellFormat.FillId = fillIndex;
            }

            if (numberFormatId != null)
            {
                cellFormat.NumberFormatId = numberFormatId;
                cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            }


            styleSheet.CellFormats.Append(cellFormat);

            UInt32Value result = styleSheet.CellFormats.Count;
            styleSheet.CellFormats.Count++;
            return result;
        }


        public string GetExcelColumnName(int colNum)
        {
            String res = "";
            int quot = colNum;
            int rem;
            while (quot > 0)
            {
                quot = quot - 1;
                rem = quot % 26;
                quot = quot / 26;
                res = (char)(rem + 97) + res;
            }
            return res;
        }
    }
}
