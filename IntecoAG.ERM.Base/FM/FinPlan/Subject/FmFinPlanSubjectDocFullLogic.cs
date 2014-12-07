using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;


namespace IntecoAG.ERM.FM.FinPlan.Subject {

    public class FmFinPlanSubjectDocXMLLoader {
        protected NameTable NameTable = null;
        protected object nBook;
        protected object nSheet;
        protected object nTable;
        protected object nColumn;
        protected object nRow;
        protected object nCell;
        protected object nData;
        protected IObjectSpace ObjectSpace;
        protected FmFinPlanSubjectDocFull TargetDoc;
        protected XmlReader Reader;

        public FmFinPlanSubjectDocXMLLoader(IObjectSpace os, FmFinPlanSubjectDocFull doc, Stream stream) {
            ObjectSpace = os;
            TargetDoc = doc;
            NameTable = new NameTable();
            nBook = NameTable.Add("Workbook");
            nSheet = NameTable.Add("Worksheet");
            nTable = NameTable.Add("Table");
            nColumn = NameTable.Add("Column");
            nRow = NameTable.Add("Row");
            nCell = NameTable.Add("Cell");
            nData = NameTable.Add("Data");
            //
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.NameTable = NameTable;
            Reader = XmlReader.Create(stream, settings);
        }

        public void Load() {

            Reader.Read();
            LoadBook();
            Reader.Close();
        }

        protected void LoadBook() {
            if (Reader.IsEmptyElement) {
                System.Console.WriteLine("WorkBook/");
                Reader.ReadStartElement((String)nBook);
            }
            else {
                System.Console.WriteLine("WorkBook");
                Reader.ReadStartElement((String)nBook);
                while (Reader.Read()) {
                    if (Reader.NodeType == XmlNodeType.Element || Reader.NodeType == XmlNodeType.EndElement) {
                        object LocalName = Reader.LocalName;
                        if (LocalName == nSheet)
                            LoadSheet();
                        if (LocalName == nBook) {
                            System.Console.WriteLine("/WorkBook");
                            Reader.ReadEndElement();
                            break;
                        }
                    }
                }
            }
        }

        protected void LoadSheet() {
            String SheetName = Reader.GetAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet");
            if (Reader.IsEmptyElement) {
                System.Console.WriteLine("Worksheet/ " + SheetName);
                Reader.ReadStartElement((String)nSheet);
            }
            else {
                System.Console.WriteLine("Worksheet " + SheetName);
                switch (SheetName) {
                    case "БСР":
                        LoadSheetCost();
                        break;
                    case "БДДС":
                        LoadSheetUnknow();
                        break;
                    case "Соисполнители":
                        LoadSheetUnknow();
                        break;
                    case "ТМЦ":
                        LoadSheetUnknow();
                        break;
                    case "Нормативы":
                        LoadSheetUnknow();
                        break;
                    default:
                        LoadSheetUnknow();
                        break;
                }
            }
        }

        protected void LoadSheetUnknow() {
            Reader.ReadStartElement((String)nSheet);
            System.Console.WriteLine("Load Sheet Unknow");
            Boolean IsWorkSheet = true;
            Boolean IsTable = false;
            while (Reader.Read()) {
                if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                    continue;
                object LocalName = Reader.LocalName;
                if (IsWorkSheet) {
                    if (LocalName == nSheet) {
                        if (Reader.NodeType == XmlNodeType.EndElement) {
                            System.Console.WriteLine("/Worksheet");
                            Reader.ReadEndElement();
                            break;
                        }
                        else
                            throw new XmlException("Wait /Worksheet");
                    }
                    if (LocalName == nTable && Reader.NodeType == XmlNodeType.Element) {
                        if (Reader.IsEmptyElement) {
                            System.Console.WriteLine("Table/");
                            Reader.ReadStartElement();
                            continue;
                        }
                        else {
                            System.Console.WriteLine("Table");
                            Reader.ReadStartElement((String)nTable);
                            IsWorkSheet = false;
                            IsTable = true;
                            continue;
                        }
                    }
                }
                if (IsTable) {
                    if (LocalName == nTable) {
                        if (Reader.NodeType == XmlNodeType.EndElement) {
                            System.Console.WriteLine("/Table");
                            Reader.ReadEndElement();
                            IsWorkSheet = true;
                            IsTable = false;
                            continue;
                        }
                        else
                            throw new XmlException("Wait /Table");
                    }
                }
            }
        }

        protected enum ReadState {
            READ_SHEET = 1,
            READ_TABLE = 2,
            READ_COLUMN = 3,
            READ_ROW = 4,
            READ_CELL = 5
        }
        protected void LoadSheetCost() {
            Reader.ReadStartElement((String)nSheet);
            System.Console.WriteLine("Load Sheet Unknow");
            ReadState current_state = ReadState.READ_SHEET;
            Int32 cur_row = 0;
            Int32 cur_col = 0;
            String index = null;
            String merge = null;
            String type = null;
            String value = null;
            while (Reader.Read()) {
                if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                    continue;
                object LocalName = Reader.LocalName;
                switch (current_state) {
                    case ReadState.READ_SHEET:
                        if (LocalName == nSheet) {
                            if (Reader.NodeType == XmlNodeType.EndElement) {
                                System.Console.WriteLine("/Worksheet");
                                Reader.ReadEndElement();
                                return;
                            }
                            else
                                throw new XmlException("Wait /Worksheet");
                        }
                        if (LocalName == nTable) {
                            if (Reader.IsEmptyElement) {
                                System.Console.WriteLine("Table/");
                                Reader.ReadStartElement();
                            }
                            else {
                                System.Console.WriteLine("Table");
                                Reader.ReadStartElement((String)nTable);
                                cur_row = 0;
                                current_state = ReadState.READ_TABLE;
                            }
                            continue;
                        }
                        break;
                    case ReadState.READ_TABLE:
                        if (LocalName == nColumn) {
                            Reader.ReadStartElement((String)nColumn);
                            continue;
                        }
                        if (LocalName == nRow) {
                            index = Reader.GetAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet");
                            if (index == null)
                                cur_row++;
                            else
                                cur_row = Int32.Parse(index);
                            if (Reader.IsEmptyElement) {
                                System.Console.WriteLine("Row/ " + cur_row.ToString());
                                Reader.ReadStartElement((String)nRow);
                            }
                            else {
                                System.Console.WriteLine("Row " + cur_row.ToString());
                                Reader.ReadStartElement((String)nRow);
                                cur_col = 0;
                                current_state = ReadState.READ_ROW;
                            }
                            continue;
                        }
                        if (LocalName == nTable) {
                            System.Console.WriteLine("/Table");
                            Reader.ReadEndElement();
                            current_state = ReadState.READ_SHEET;
                            continue;
                        }
                        break;
                    case ReadState.READ_ROW:
                        if (LocalName == nCell) {
                            index = Reader.GetAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet");
                            merge = Reader.GetAttribute("MergeAcross", "urn:schemas-microsoft-com:office:spreadsheet");
                            if (index == null)
                                cur_col++;
                            else
                                cur_col = Int32.Parse(index);
                            if (Reader.IsEmptyElement) {
                                Reader.ReadStartElement((String)nCell);
                            }
                            else {
                                Reader.ReadStartElement((String)nCell);
                                current_state = ReadState.READ_CELL;
                            }
                            if (merge != null)
                                cur_col += Int32.Parse(merge);
                            continue;
                        }
                        if (LocalName == nRow) {
                            Reader.ReadEndElement();
                            current_state = ReadState.READ_TABLE;
                            continue;
                        }
                        break;
                    case ReadState.READ_CELL:
                        if (LocalName == nData) {
//                            if (Reader.NamespaceURI != "")
//                                continue;
                            if (Reader.NodeType == XmlNodeType.Element) {
                                type = Reader.GetAttribute("Type", "urn:schemas-microsoft-com:office:spreadsheet");
                                Reader.ReadStartElement((String)nData, "urn:schemas-microsoft-com:office:spreadsheet");
                                value = Reader.ReadString();
                                ProcessCell(cur_row, cur_col, type, value);
//                                Reader.ReadEndElement();
//                                current_state = ReadState.READ_ROW;
                            }
                            continue;
                        }
                        if (LocalName == nCell) {
                            Reader.ReadEndElement();
                            current_state = ReadState.READ_ROW;
                            continue;
                        }
                        break;
                }
            }
        }
        protected void ProcessCell(Int32 row, Int32 column, String type, String value) {
            System.Console.WriteLine("({0},{1}):{2}:'{3}'", row, column, type, value);
        }
        protected void LoadDocRow(FmFinPlanDocLine line, Int32 column) {
            if (Reader.IsEmptyElement) {
                System.Console.WriteLine("Row/ " + column.ToString());
                Reader.ReadStartElement((String)nRow);
            }
            else {
                System.Console.WriteLine("Row " + column.ToString());
                Reader.ReadStartElement((String)nRow);
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nRow) {
                        if (Reader.NodeType == XmlNodeType.EndElement) {
                            Reader.ReadEndElement();
                            break;
                        }
                    }
                }
            }
        }
    }


    public static class FmFinPlanSubjectDocFullLogic {

        public static void LoadDocFromXML(IObjectSpace os, FmFinPlanSubjectDocFull doc, Stream stream) {
            FmFinPlanSubjectDocXMLLoader loader = new FmFinPlanSubjectDocXMLLoader(os, doc, stream);

            doc.Clean();
            loader.Load();

        }



    }
}
