using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace IntecoAG.ERM.CS.Import {
    public abstract class Excel2003XmlReader {

            protected NameTable NameTable = null;
            protected object nBook;
            protected object nSheet;
            protected object nTable;
            protected object nColumn;
            protected object nRow;
            protected object nCell;
            protected object nData;
            protected XmlReader Reader;

            protected String CurSheet;
            protected Int32 CurCol = 0;
            protected Int32 CurRow = 0;


            public Excel2003XmlReader(Stream stream) {
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

            public virtual void Load() {
                while (Reader.Read()) {
                    object LocalName = Reader.LocalName;
                    if (LocalName == nBook && Reader.IsStartElement()) {
                        if (!Reader.IsEmptyElement) {
                            LoadBook();
                        }
                        continue;
                    }
                }
                Reader.Close();
            }

            protected virtual void LoadBook() {
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nSheet && Reader.IsStartElement()) {
                        CurSheet = Reader.GetAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet");
                        if (!Reader.IsEmptyElement) {
                            LoadSheet();
                        }
                        continue;
                    }
                    if (LocalName == nBook && Reader.NodeType == XmlNodeType.EndElement) {
                        break;
                    }
                }
            }

            protected virtual void LoadSheet() {
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nTable && Reader.IsStartElement()) {
                        if (!Reader.IsEmptyElement) {
                            LoadTable();
                        }
                        continue;
                    }
                    if (LocalName == nSheet && Reader.NodeType == XmlNodeType.EndElement) {
                        break;
                    }
                }
            }
            protected virtual void LoadTable() {
                String index = null;
                CurRow = 0;
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nRow && Reader.IsStartElement()) {
                        index = Reader.GetAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet");
                        if (index == null)
                            CurRow++;
                        else
                            CurRow = Int32.Parse(index);
                        if (!Reader.IsEmptyElement) {
                            LoadRow();
                        }
                        continue;
                    }
                    if (LocalName == nTable && Reader.NodeType == XmlNodeType.EndElement) {
                        break;
                    }
                }
            }

            protected virtual void LoadRow() {
                String index = null;
                String merge = null;
                CurCol = 0;
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nCell && Reader.IsStartElement()) {
                        index = Reader.GetAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet");
                        merge = Reader.GetAttribute("MergeAcross", "urn:schemas-microsoft-com:office:spreadsheet");
                        if (index == null)
                            CurCol++;
                        else
                            CurCol = Int32.Parse(index);
                        if (!Reader.IsEmptyElement) {
                            LoadCell();
                        }
                        if (merge != null)
                            CurCol += Int32.Parse(merge);
                        continue;
                    }
                    if (LocalName == nRow && Reader.NodeType == XmlNodeType.EndElement) {
                        break;
                    }
                }
            }

            protected void LoadCell() {
                String type = null;
                String value = null;
                while (Reader.Read()) {
                    if (Reader.NodeType != XmlNodeType.Element && Reader.NodeType != XmlNodeType.EndElement)
                        continue;
                    object LocalName = Reader.LocalName;
                    if (LocalName == nData && Reader.IsStartElement()) {
                        type = Reader.GetAttribute("Type", "urn:schemas-microsoft-com:office:spreadsheet");
                        Reader.ReadStartElement((String)nData, "urn:schemas-microsoft-com:office:spreadsheet");
                        value = Reader.ReadString();
                        ProcessCell(CurSheet, CurRow, CurCol, type, value);
                        continue;
                    }
                    if (LocalName == nCell && Reader.NodeType == XmlNodeType.EndElement) {
                        break;
                    }
                }
            }

            protected abstract void ProcessCell(String sheet, Int32 row, Int32 column, String type, String value);

        }
}
