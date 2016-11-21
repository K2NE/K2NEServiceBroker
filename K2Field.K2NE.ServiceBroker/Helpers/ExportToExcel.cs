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


namespace K2Field.K2NE.ServiceBroker.Helpers
{
    [Serializable]
    public class CreateExcel : MarshalByRefObject
    {
        private string cStr = string.Empty;
        private UInt32Value _numberStyleId;
        private UInt32Value _doubleStyleId;
        private UInt32Value _dateStyleId;
        private UInt32Value _headerStyleId;

        public string ConnectionString
        {
            get { return cStr; }
            set { cStr = value; }
        }

        private List<string> cellHeaders = null;


        public CreateExcel()
        {
        }

        /// <summary>
        /// Method to create a filename and provide a file as an ouput string.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        public string GetExcelFromADOQuery(DataTable results, string fname)
        {
            try
            {
                fname = TrimFileName(fname);

                // Save the exported file to the temporary file folder
                string filename = string.IsNullOrEmpty(fname) ? Guid.NewGuid().ToString() : fname;
                filename += ".xlsx";

                cellHeaders = new List<string>();
                for (int i = 0; i < results.Columns.Count; i++)
                {
                    cellHeaders.Add(GetExcelColumnName(i + 1));
                }

                byte[] objByte = ExportToExcel(results, cellHeaders);

                string tmpFile = new FileObject(filename, Convert.ToBase64String(objByte, 0, objByte.Count(), Base64FormattingOptions.None)).ToString();

                return tmpFile;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Excel creation -  Msg : " + ex.Message);
            }
        }

        /// <summary>
        /// Returns the column caption for the given row & column index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns></returns>
        private string Get(int columnIndex, int rowIndex, List<string> cHeaders)
        {
            return cHeaders.ElementAt(columnIndex) + (rowIndex + 1).ToString();
        }

        /// <summary>
        ///  Generate an excel file with data and return as array of byte
        /// </summary>
        /// <param name="datatable">DataTable object</param>
        /// <param name="filepath">The Path of exported excel file</param>
        public byte[] ExportToExcel(DataTable datatable, List<string> cellheaders)
        {
            cellHeaders = cellheaders;
            MemoryStream mem = new MemoryStream();
            // Initialize an instance of  SpreadSheet Document 
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook))
            {
                CreateExcelFile(spreadsheetDocument, datatable);
            }

            return mem.ToArray();
        }

        /// <summary>
        ///  Create SpreadSheet Document and Fill datas
        /// </summary>
        /// <param name="spreadsheetdoc">SpreadSheet Document</param>
        /// <param name="table">DataTable Object</param>
        private void CreateExcelFile(SpreadsheetDocument spreadsheetdoc, DataTable table)
        {
            // Initialize an instance of WorkbookPart
            WorkbookPart workBookPart = spreadsheetdoc.AddWorkbookPart();

            // Create WorkBook 
            CreateWorkBookPart(workBookPart);

            // Add WorkSheetPart into WorkBook
            WorksheetPart worksheetPart1 = workBookPart.AddNewPart<WorksheetPart>("rId1");
            CreateWorkSheetPart(worksheetPart1, table);

            // Save workbook
            workBookPart.Workbook.Save();
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
                SheetId = Convert.ToUInt32(1),
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

            Row row1 = new Row()
            {
                RowIndex = rowIndex++,
                Spans = new ListValue<StringValue>() { InnerText = "1:3" },
                DyDescent = 0.25D
            };


            ;            // Add columns in DataTable to columns collection of SpreadSheet Document 
            for (int columnindex = 0; columnindex < table.Columns.Count; columnindex++)
            {
                Cell cell = new Cell()
                {
                    CellReference = new CreateExcel().Get(columnindex, (Convert.ToInt32((UInt32)rowIndex) - 2), this.cellHeaders),
                    //StyleIndex = _headerStyleId,
                    DataType = GetCellType(table.Columns[columnindex].ColumnName.GetType().ToString())

                };

                // Get Value of DataTable and append the value to cell of spreadsheet document
                CellValue cellValue = new CellValue();
                cellValue.Text = table.Columns[columnindex].ColumnName.ToString();
                cell.Append(cellValue);

                row1.Append(cell);
            }

            // Add row to sheet
            sheetData1.Append(row1);

            // Add rows in DataTable to rows collection of SpreadSheet Document 
            for (int rIndex = 0; rIndex < table.Rows.Count; rIndex++)
            {
                Row row = new Row()
                {
                    RowIndex = rowIndex++,
                    Spans = new ListValue<StringValue>() { InnerText = "1:3" },
                    DyDescent = 0.25D
                };

                for (int cIndex = 0; cIndex < table.Columns.Count; cIndex++)
                {
                    Cell cell = new Cell()
                    {
                        CellReference = new CreateExcel().Get(cIndex, (Convert.ToInt32((UInt32)rowIndex) - 2), this.cellHeaders),
                        //StyleIndex = GetCellStyle(table.Columns[cIndex]),
                        DataType = GetCellType(table.Rows[rIndex][cIndex].GetType().ToString())

                    };

                    CellValue cellValue = new CellValue();
                    cellValue.Text = table.Rows[rIndex][cIndex].ToString();
                    cell.Append(cellValue);
                    row.Append(cell);
                }

                // Add row to Sheet Data
                sheetData1.Append(row);
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
                //Celltype date corrupts the excel so keeping its datatype as string
                //case "System.DateTime":
                //return DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                case "System.Decimal":
                case "System.Double":
                case "System.Int64":
                case "System.Int32":
                    return DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                default:
                    return DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            }

        }


        private UInt32Value GetCellStyle(DataColumn col)
        {
            switch (col.DataType.FullName.ToString())
            {
                case "System.DateTime":
                    return _dateStyleId;
                case " System.Decimal":
                case "System.Int64":
                case "System.Int32":
                    return _numberStyleId;
                case "System.Double":
                    return _doubleStyleId;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Generates content of sharedStringTablePart
        /// </summary>
        /// <param name="sharedStringTablePart">SharedStringTablePart Object</param>
        /// <param name="table">DataTable Object</param>
        private void CreateSharedStringTablePart(SharedStringTablePart sharedStringTablePart, DataTable table)
        {
            UInt32Value stringCount = Convert.ToUInt32(table.Rows.Count) + Convert.ToUInt32(table.Columns.Count);

            // Initialize an instance of SharedString Table
            SharedStringTable sharedStringTable = new SharedStringTable()
            {
                Count = stringCount,
                UniqueCount = stringCount
            };

            // Add columns of DataTable to sharedString iteam
            for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                SharedStringItem sharedStringItem = new SharedStringItem();
                Text text = new Text();
                text.Text = table.Columns[columnIndex].ColumnName;
                sharedStringItem.Append(text);

                // Add sharedstring item to sharedstring Table
                sharedStringTable.Append(sharedStringItem);
            }

            // Add rows of DataTable to sharedString iteam
            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                SharedStringItem sharedStringItem = new SharedStringItem();
                Text text = new Text();
                text.Text = table.Rows[rowIndex][0].ToString();
                sharedStringItem.Append(text);
                sharedStringTable.Append(sharedStringItem);
            }

            sharedStringTablePart.SharedStringTable = sharedStringTable;
        }

        private UInt32Value GetHeaderStylesheet(Stylesheet styleSheet)
        {

            //build the formatted header style
            UInt32Value headerFontIndex =
                createFont(
                    styleSheet,
                    "Calibri",
                    14,
                    true,
                    System.Drawing.Color.Black);
            //set the background color style
            UInt32Value headerFillIndex =
                createFill(
                    styleSheet,
                    System.Drawing.Color.White);
            //create the cell style by combining font/background
            UInt32Value headerStyleIndex =
                createCellFormat(
                    styleSheet,
                    headerFontIndex,
                    headerFillIndex,
                    null);

            return headerStyleIndex;
        }


        /// <summary>
        /// Creates a new font and appends it to the workbook's stylesheet
        /// </summary>
        /// <param name="styleSheet">The stylesheet for the current WorkBook</param>
        /// <param name="fontName">The font name.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="isBold">Set to true for bold font.</param>
        /// <param name="foreColor">The font color.</param>
        /// <returns>The index of the font.</returns>
        private UInt32Value createFont(
            Stylesheet styleSheet,
            string fontName,
            Nullable<double> fontSize,
            bool isBold,
            System.Drawing.Color foreColor)
        {

            Font font = new Font();

            if (!string.IsNullOrEmpty(fontName))
            {
                FontName name = new FontName()
                {
                    Val = fontName
                };
                font.Append(name);
            }

            if (fontSize.HasValue)
            {
                FontSize size = new FontSize()
                {
                    Val = fontSize.Value
                };
                font.Append(size);
            }

            if (isBold == true)
            {
                Bold bold = new Bold();
                font.Append(bold);
            }

            if (foreColor != null)
            {
                Color color = new Color()
                {
                    Rgb = new HexBinaryValue()
                    {
                        Value =
                            System.Drawing.ColorTranslator.ToHtml(
                                System.Drawing.Color.FromArgb(
                                    foreColor.A,
                                    foreColor.R,
                                    foreColor.G,
                                    foreColor.B)).Replace("#", "")
                    }
                };
                font.Append(color);
            }
            styleSheet.Fonts.Append(font);
            UInt32Value result = styleSheet.Fonts.Count;
            styleSheet.Fonts.Count++;
            return result;
        }

        /// <summary>
        /// Creates a new Fill object and appends it to the WorkBook's stylesheet.
        /// </summary>
        /// <param name="styleSheet">The stylesheet for the current WorkBook.</param>
        /// <param name="fillColor">The background color for the fill.</param>
        /// <returns></returns>
        private UInt32Value createFill(
            Stylesheet styleSheet,
            System.Drawing.Color fillColor)
        {
            Fill fill = new Fill(
                new PatternFill(
                    new ForegroundColor()
                    {
                        Rgb = new HexBinaryValue()
                        {
                            Value =
                            System.Drawing.ColorTranslator.ToHtml(
                                System.Drawing.Color.FromArgb(
                                    fillColor.A,
                                    fillColor.R,
                                    fillColor.G,
                                    fillColor.B)).Replace("#", "")
                        }
                    })
                {
                    PatternType = PatternValues.Solid
                }
            );
            styleSheet.Fills.Append(fill);

            UInt32Value result = styleSheet.Fills.Count;
            styleSheet.Fills.Count++;
            return result;
        }

        private UInt32Value createCellFormat(
            Stylesheet styleSheet,
            UInt32Value fontIndex,
            UInt32Value fillIndex,
            UInt32Value numberFormatId)
        {
            CellFormat cellFormat = new CellFormat();

            if (fontIndex != null)
                cellFormat.FontId = fontIndex;

            if (fillIndex != null)
                cellFormat.FillId = fillIndex;

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


        ///// <summary>
        ///// Generates content of workbookStylesPart
        ///// </summary>
        ///// <param name="workbookStylesPart">WorkbookStylesPart Object</param>
        //private void CreateWorkBookStylesPart(WorkbookStylesPart workbookStylesPart)
        //{
        //    // Define Style of Sheet in workbook
        //    Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
        //    stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        //    stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

        //    // Initialize  an instance of fonts
        //    Fonts fonts = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

        //    // Initialize  an instance of font,fontsize,color
        //    Font font = new Font();
        //    FontSize fontSize = new FontSize() { Val = 14D };
        //    Color color = new Color() { Theme = (UInt32Value)1U };
        //    FontName fontName = new FontName() { Val = "Calibri" };
        //    FontFamilyNumbering fontFamilyNumbering = new FontFamilyNumbering() { Val = 2 };
        //    FontScheme fontScheme = new FontScheme() { Val = FontSchemeValues.Minor };

        //    // Add elements to font
        //    font.Append(fontSize);
        //    font.Append(color);
        //    font.Append(fontName);
        //    font.Append(fontFamilyNumbering);
        //    font.Append(fontScheme);

        //    fonts.Append(font);

        //    // Define the StylesheetExtensionList Class. When the object is serialized out as xml, its qualified name is x:extLst
        //    StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

        //    // Define the StylesheetExtension Class
        //    StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        //    stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        //    DocumentFormat.OpenXml.Office2010.Excel.SlicerStyles slicerStyles1 = new DocumentFormat.OpenXml.Office2010.Excel.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

        //    stylesheetExtension1.Append(slicerStyles1);
        //    stylesheetExtensionList1.Append(stylesheetExtension1);

        //    // Add elements to stylesheet
        //    stylesheet1.Append(fonts);
        //    stylesheet1.Append(stylesheetExtensionList1);

        //    // Set the style of workbook
        //    workbookStylesPart.Stylesheet = stylesheet1;
        //}

        /// <summary>
        /// Generates content of workbookStylesPart
        /// </summary>
        /// <param name="workbookStylesPart">WorkbookStylesPart Object</param>
        private void CreateWorkBookStylesPart(WorkbookStylesPart workbookStylesPart)
        {
            var StyleSheet = new Stylesheet();

            // Create "fonts" node.
            var Fonts = new Fonts();
            Fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()
            {
                FontName = new FontName() { Val = "Calibri" },
                FontSize = new FontSize() { Val = 14 },
                FontFamilyNumbering = new FontFamilyNumbering() { Val = 2 },
            });

            Fonts.Count = (uint)Fonts.ChildElements.Count;

            // Create "fills" node.
            var Fills = new Fills();
            Fills.Append(new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.None }
            });
            Fills.Append(new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 }
            });

            Fills.Count = (uint)Fills.ChildElements.Count;

            // Create "borders" node.
            var Borders = new Borders();
            Borders.Append(new Border()
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            });

            Borders.Count = (uint)Borders.ChildElements.Count;

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
            StyleSheet.Append(Fonts);
            StyleSheet.Append(Fills);
            StyleSheet.Append(Borders);
            StyleSheet.Append(CellStyleFormats);
            StyleSheet.Append(CellFormats);
            StyleSheet.Append(CellStyles);

            //_headerStyleId = GetHeaderStylesheet(StyleSheet);
            _dateStyleId = createCellFormat(StyleSheet, null, null, UInt32Value.FromUInt32(14));
            _numberStyleId = createCellFormat(StyleSheet, null, null, UInt32Value.FromUInt32(3));
            _doubleStyleId = createCellFormat(StyleSheet, null, null, UInt32Value.FromUInt32(4));

            // Set the style of workbook
            workbookStylesPart.Stylesheet = StyleSheet;
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
            return res.ToUpper();
        }

        private string TrimFileName(string fileName)
        {
            if (fileName.EndsWith(".xls"))
            {
                return fileName.Substring(0, fileName.Length - ".xls".Length);
            }

            if (fileName.EndsWith(".xlsx"))
            {
                return fileName.Substring(0, fileName.Length - ".xlsx".Length);
            }

            return fileName;
        }
    }

    public class CreateExcelWrapper : IDisposable
    {
        private CreateExcel _createExcelFile = null;
        private AppDomain _app;
        private string _currentAssemblyPath;

        /// <summary>  
        /// Constructor.  
        /// </summary>  
        /// <param name="zipFilename">A name of new zip-file.  
        public CreateExcel GetCreateExcelByNewDomain(string connectionString)
        {
            _currentAssemblyPath = Assembly.GetExecutingAssembly().CodeBase;

            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
            };

            Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            evidence.AddHost(new Zone(SecurityZone.MyComputer));

            _app = AppDomain.CreateDomain("Processor AppDomain", evidence, appDomainSetup);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _createExcelFile =
                (CreateExcel)
                    _app.CreateInstanceFromAndUnwrap(_currentAssemblyPath, typeof(CreateExcel).FullName);
            _createExcelFile.ConnectionString = connectionString;

            return _createExcelFile;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(_currentAssemblyPath);
        }

        public void Dispose()
        {
            AppDomain.Unload(_app);
        }
    }

    public class FileObject : NamedObject
    {

        #region Constructors (2)

        public FileObject(string name, string value)
            : base(name, value)
        {
        }

        public FileObject(string inputValue)
            : base(inputValue)
        {
        }

        #endregion Constructors

        #region Methods (2)

        // Public Methods (2) 
        public override void FromValue(string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                using (StringReader reader = new StringReader(inputValue))
                {
                    XPathDocument xDoc = new XPathDocument(reader);
                    XPathNavigator xNav = xDoc.CreateNavigator();
                    string fileNameValue = xNav.SelectSingleNode("file/name").InnerXml;
                    string contentValue = xNav.SelectSingleNode("file/content").InnerXml;
                    if (fileNameValue == SCNULL)
                        this.Name = string.Empty;
                    else
                        this.Name = fileNameValue;
                    if (contentValue == SCNULL)
                        this.Value = string.Empty;
                    else
                        this.Value = contentValue;
                }
            }
            else
            {
                this.Value = string.Empty;
                this.Name = string.Empty;
            }
        }

        public override string ToValue()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<file>");
            if (this.Name == null)
            {
                sb.Append("<name/>");
            }
            else
            {
                sb.Append("<name>");
                sb.Append(this.Name);
                sb.Append("</name>");
            }
            if (this.Value == null)
            {
                sb.Append("<content/>");
            }
            else
            {
                sb.Append("<content>");
                sb.Append(this.Value);
                sb.Append("</content>");
            }
            sb.Append("</file>");
            return sb.ToString();
        }

        #endregion Methods

    }

    #region Object Classes

    public abstract class NamedObject
    {

        #region Fields (3)

        private string _name;
        private string _value;
        protected const string SCNULL = "scnull";

        #endregion Fields

        #region Constructors (2)

        public NamedObject(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public NamedObject(string inputValue)
        {
            this.FromValue(inputValue);
        }

        #endregion Constructors

        #region Properties (2)

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (3) 

        public abstract void FromValue(string inputValue);

        public override string ToString()
        {
            return ToValue();
        }

        public abstract string ToValue();

        #endregion Methods

    }

    #endregion
}
