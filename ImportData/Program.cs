using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
//
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
//
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace ImportData {
    class Program {
        static IDictionary<Int32, String> xlsLetters = new Dictionary<Int32, String>();
        static IDictionary<Int32, String> xlsSheetReference = new Dictionary<Int32, String>();
        static object misValue = System.Reflection.Missing.Value;
        struct ExcelParty {
            public String code, type, close, name, legal, inn, kpp, lpt, country, city, addr;
        }
        static String UpFirstCase(String val) {
            String str = val.Trim().ToLower();
            if (str.Length < 3) return str;
            return str.Substring(0,1).ToUpper() + str.Substring(1);
        }
        static IDataLayer dl = null;
        static void UpdateLegalPersonUnit(Session ses, ExcelParty ep) {
            crmCLegalPersonUnit lpu = null;
            XPQuery<crmCLegalPersonUnit> Partys = new XPQuery<crmCLegalPersonUnit>(ses);
            var qp = from p in Partys
                     where p.Code == ep.code
                     select p;
            foreach (crmCLegalPersonUnit p in qp) {
                lpu = p;
                break;
            }
            if (lpu == null)
                lpu = new crmCLegalPersonUnit(ses);
            crmCLegalPerson lp;
            if (String.IsNullOrEmpty(ep.legal.Trim())) {
                lp = NewLegalPerson(ses, ep);
                lp.Code = "";
                lp.AddressLegal.AddressHandmake = "";
                lp.AddressFact.AddressHandmake = "";
                lp.Save();
            }
            else {
                lp = GetLegalPerson(ses, ep.legal);
            }
            lpu.LegalPerson = lp;
            lpu.Code = ep.code;
            lpu.Name = ep.name;
            lpu.INN = ep.inn;
            lpu.AddressFact.Country = GetCountry(ses, ep.country);
            lpu.AddressFact.City = ep.city;
            lpu.AddressFact.AddressHandmake = ep.addr;
            if (!String.IsNullOrEmpty(ep.close.Trim()))
                lpu.IsClosed = true;
            lpu.Save();
        }

        static void releaseObject(object obj) {
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            }
            catch (Exception ex) {
                System.Console.WriteLine(ex);
            }
            finally {
                GC.Collect();
            }
        }

        static Session GetSession() {
            if (dl == null) {
                dl = XpoDefault.GetDataLayer("XpoProvider=Postgres;Server=alt-dev.otd1101;User Id=pg_adm;Password='flesh*token=across';Database=stage2;Encoding=UNICODE;", AutoCreateOption.SchemaAlreadyExists);
                XpoDefault.DataLayer = dl;
                XpoDefault.Session = null;
            }
            return new Session(dl);
        }
        //
        static crmCLegalPerson NewLegalPerson(Session ses, ExcelParty ep) { 
            crmCLegalPerson lp = new crmCLegalPerson(ses);
            lp.Code = ep.code;
            lp.Name = ep.name;
            lp.INN = ep.inn;
            lp.KPP = ep.kpp;
            lp.PersonType = GetPersonType(ses, ep.lpt);
            lp.AddressLegal.Country = GetCountry(ses, ep.country);
            lp.AddressLegal.City = ep.city;
            lp.AddressLegal.AddressHandmake = ep.addr;
            lp.AddressFact.Country = lp.AddressLegal.Country;
            lp.AddressFact.City = ep.city;
            lp.AddressFact.AddressHandmake = ep.addr;
            if (!String.IsNullOrEmpty(ep.close.Trim()))
                lp.IsClosed = true;
            lp.Save();
            return lp;
        }
        //
        static crmCLegalPerson GetLegalPerson(Session ses, String code) {
            crmCLegalPerson rc = null;
            XPQuery<crmPartyRu> Partys = new XPQuery<crmPartyRu>(ses);
            var qp = from p in Partys
                     where p.Code == code
                     select p;
            foreach (crmPartyRu p in qp) {
                rc = (crmCLegalPerson) p.ComponentObject;
                break;
            }
            return rc;
        }
        //
        static csCountry GetCountry(Session ses, String name) {
            if (String.IsNullOrEmpty(name.Trim()))
                name = "НЕОПРЕДЕЛЕННАЯ";
            csCountry rc = null;
            XPQuery<csCountry> Countrys = new XPQuery<csCountry>(ses);
            var qc = from pe in Countrys
                     where pe.NameRuShortLow == name
                     select pe;
            foreach (csCountry pi in qc) {
                rc = pi;
                break;
            }
            if (rc == null) {
                rc = new csCountry(ses);
                rc.NameRuShortLow = name;
                rc.Save();
            }
            return rc;
        }
        //
        static crmPersonType GetPersonType(Session ses, String code) {
            crmPersonType pt = null;
            XPQuery<crmPersonType> Persons = new XPQuery<crmPersonType>(ses);
            var qc = from pe in Persons 
                     where pe.Code == code
                     select pe;
            foreach (crmPersonType pi in qc) {
                pt = pi;
                break;
            }
            if (pt == null) {
                pt = new crmPersonType(ses);
                pt.Code = code;
                pt.Save();
            }
            return pt;
        }
        //
        static void Main(string[] args) {
            Session ses = GetSession();
            int i = 1;
            if (args[0] == "party") {
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet = null;

                xlApp = new Excel.ApplicationClass();

                // Открытие документа (книги) excel
                xlWorkBook = xlApp.Workbooks.Open("e:\\party.xlsx");

                // Выбирается Лист (если существует)
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //
                try {
                    while (true) {
                        i++;
                        ExcelParty ep = new ExcelParty();
                        //                    Excel.Range range = xlWorkSheet.get_Range("A" + i, "P" + i);
                        //                    Excel.Range range2 = xlWorkSheet.get_Range("A1");
                        if (xlWorkSheet.get_Range("A" + i).Value2 == null) break;
                        ep.code = xlWorkSheet.get_Range("A" + i).Value2.ToString();
                        ep.type = xlWorkSheet.get_Range("B" + i).Value2.ToString();
                        if (xlWorkSheet.get_Range("C" + i).Value2 != null)
                            ep.close = xlWorkSheet.get_Range("C" + i).Value2.ToString();
                        else
                            ep.close = String.Empty;
                        ep.country = UpFirstCase(xlWorkSheet.get_Range("E" + i).Value2.ToString());
                        ep.city = UpFirstCase(xlWorkSheet.get_Range("F" + i).Value2.ToString());
                        if (xlWorkSheet.get_Range("G" + i).Value2 != null)
                            ep.legal = xlWorkSheet.get_Range("G" + i).Value2.ToString();
                        else
                            ep.legal = String.Empty;
                        ep.lpt = xlWorkSheet.get_Range("H" + i).Value2.ToString();
                        ep.inn = xlWorkSheet.get_Range("I" + i).Value2.ToString();
                        if (xlWorkSheet.get_Range("J" + i).Value2 != null)
                            ep.kpp = xlWorkSheet.get_Range("J" + i).Value2.ToString();
                        else
                            ep.kpp = String.Empty;
                        ep.name = xlWorkSheet.get_Range("K" + i).Value2.ToString();
                        ep.addr = xlWorkSheet.get_Range("L" + i).Value2.ToString();
                        //                    party.NameFull = party.Name;
                        //                    party.AddressFact.AddressHandmake = xlWorkSheet.get_Range("L" + i, "L" + i).Value2.ToString();
                        //                    party.INN = xlWorkSheet.get_Range("I" + i, "I" + i).Value2.ToString();
                        //                    party.KPP = xlWorkSheet.get_Range("J" + i, "J" + i).Value2.ToString();
                        switch (ep.type) {
                            case "ЛЮ":
                                crmCLegalPerson lp = NewLegalPerson(ses, ep);
                                lp.Save();
                                break;
                            case "ЛФ":
                                crmCPhysicalParty php = new crmCPhysicalParty(ses);
                                php.Code = ep.code;
                                php.NameHandmake = ep.name;
                                php.INN = ep.inn;
                                php.AddressLegal.Country = GetCountry(ses, ep.country);
                                php.AddressLegal.City = ep.city;
                                php.AddressLegal.AddressHandmake = ep.addr;
                                php.AddressFact.Country = php.AddressLegal.Country;
                                php.AddressFact.City = ep.city;
                                php.AddressFact.AddressHandmake = ep.addr;
                                if (!String.IsNullOrEmpty(ep.close.Trim()))
                                    php.IsClosed = true;
                                php.Save();
                                break;
                            case "ИП":
                                crmCBusinessman bp = new crmCBusinessman(ses);
                                bp.Code = ep.code;
                                bp.NameHandmake = ep.name;
                                bp.INN = ep.inn;
                                bp.PersonType = GetPersonType(ses, ep.lpt);
                                bp.AddressLegal.Country = GetCountry(ses, ep.country);
                                bp.AddressLegal.City = ep.city;
                                bp.AddressLegal.AddressHandmake = ep.addr;
                                bp.AddressFact.Country = bp.AddressLegal.Country;
                                bp.AddressFact.City = ep.city;
                                bp.AddressFact.AddressHandmake = ep.addr;
                                if (!String.IsNullOrEmpty(ep.close.Trim()))
                                    bp.IsClosed = true;
                                bp.Save();
                                break;
                            case "ФИЛИАЛ":
                                UpdateLegalPersonUnit(ses, ep);
                                break;
                            case "ПРОЧИЕ":
                            case "ДОМ":
                                crmPartyRu party = new crmPartyRu(ses);
                                party.Code = ep.code;
                                party.Name = ep.name;
                                party.INN = ep.inn;
                                party.AddressFact.Country = GetCountry(ses, ep.country);
                                party.AddressFact.City = ep.city;
                                party.AddressFact.AddressHandmake = ep.addr;
                                if (!String.IsNullOrEmpty(ep.close.Trim()))
                                    party.IsClosed = true;
                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        //MessageBox.Show(xlWorkSheet.get_Range("B1", "B1").Value2.ToString());

                        System.Console.WriteLine(ep.type + ' ' + ep.code + ' ' + ep.name + ' ' + ep.addr);
                    }
                } catch (Exception e) {
                    if (xlWorkSheet != null) releaseObject(xlWorkSheet);
                    if (xlWorkBook != null) releaseObject(xlWorkBook);
                    if (xlApp != null) releaseObject(xlApp);
                    xlWorkSheet = null;
                    xlWorkBook = null;
                    xlApp = null;
                    GC.Collect();
                    System.Console.WriteLine(e);
                    throw e;
                }
                    
                // Закрытие книги
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }

        }
    }
}
