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
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
//
namespace ImportData {
    class Program {
        static IDictionary<Int32, String> xlsLetters = new Dictionary<Int32, String>();
        static IDictionary<Int32, String> xlsSheetReference = new Dictionary<Int32, String>();
        static object misValue = System.Reflection.Missing.Value;
        struct FinanceTema {
            public String dir, code, name, desc;
        }
        struct FinanceOrder {
            public Int32 buh_int_num;
            public String buh_account, code, subj, name_short, name_full, desc,
                            data_from, data_to, base_doc, work_type, finans, source, army, nds_mode, acc_mode;
            public Decimal koeff_ozm, koeff_kb;
            public Boolean is_closed;
        }
        struct ExcelParty {
            public String code, type, close, name, legal, inn, kpp, lpt, country, city, addr;
        }
        struct ExcelDogovor {
            public String type, dog_id, dog_num, dop_id, dop_num, staff_code, dep_code, customer_code, supplier_code;
            public DateTime dog_date, dop_date;
            public hrmStaff reg_staff;
            public hrmDepartment reg_dep;
            public hrmDepartment dep;
        }
        static String UpFirstCase(String val) {
            String str = val.Trim().ToLower();
            if (str.Length < 3) return str;
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        static IDataLayer dl = null;
        static crmCLegalPersonUnit UpdateLegalPersonUnit(Session ses, ExcelParty ep) {
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
            return lpu;
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
                dl = XpoDefault.GetDataLayer("XpoProvider=Postgres;Server=npomash;User Id=pg_adm;Password='flesh*token=across';Database=ermdev;Encoding=UNICODE;", AutoCreateOption.SchemaAlreadyExists);
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
            return lp;
        }
        //
        static crmCLegalPerson GetLegalPerson(Session ses, String code) {
            crmCLegalPerson rc = null;
            XPQuery<crmCParty> Partys = new XPQuery<crmCParty>(ses);
            var qp = from p in Partys
                     where p.Code == code
                     select p;
            foreach (crmCParty p in qp) {
                rc = (crmCLegalPerson)p.ComponentObject;
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
        static crmCParty GetParty(Session ses, String code) {
            crmCParty party = null;
            XPQuery<crmCParty> partys = new XPQuery<crmCParty>(ses);
            var qc = from pe in partys
                     where pe.Code == code
                     select pe;
            foreach (crmCParty pi in qc) {
                party = pi;
                break;
            }
            return party;
        }

        static hrmDepartment GetDepartment(Session ses, String code) {
            XPQuery<hrmDepartment> deps = new XPQuery<hrmDepartment>(ses);
            var q = from dep in deps
                    where dep.Code == code
                    select dep;
            foreach (hrmDepartment dep in q) {
                return dep;
            }
            return null;
        }
        //
        static hrmStaff GetStaff(Session ses, String code) {
            XPQuery<hrmStaff> staffs = new XPQuery<hrmStaff>(ses);
            var q = from staff in staffs
                    where staff.BuhCode == code
                    select staff;
            foreach (hrmStaff staff in q) {
                return staff;
            }
            return null;
        }
        static T GetObject<T>(Session ses, String code) where T : csCComponent, csICodedComponent {
            XPQuery<T> objs = new XPQuery<T>(ses);
            var q = from obj in objs
                    where obj.Code == code
                    select obj;
            foreach (T obj in q) {
                return obj;
            }
            return null;
        }

        //
        static crmContract GetContract(Session ses, ExcelDogovor dog) {
            XPQuery<crmContract> contracts = new XPQuery<crmContract>(ses);
            var q = from contract in contracts
                    where contract.ContractDocument.Number == dog.dog_num &&
                          contract.ContractDocument.Date == dog.dog_date
                    select contract;
            foreach (crmContract contract in q) {
                return contract;
            }
            crmContract new_contract = new crmContract(ses);
            new_contract.ContractDocument = new crmContractDocument(ses);
            new_contract.ContractDocument.Number = dog.dog_num;
            new_contract.ContractDocument.Date = dog.dog_date;
            new_contract.ContractDocument.DocumentCategory = GetDocumentCategory(ses, "ДОГ");
            new_contract.UserRegistrator = dog.reg_staff;
            new_contract.DepartmentRegistrator = dog.reg_dep;
            return new_contract;
        }
        static crmContractDocumentType GetDocumentCategory(Session ses, String code) {
            XPQuery<crmContractDocumentType> cats = new XPQuery<crmContractDocumentType>(ses);
            var q = from cat in cats
                    where cat.Code == code
                    select cat;
            foreach (crmContractDocumentType cat in q) {
                return cat;
            }
            return null;
        }
        //
        static void Main(string[] args) {
            Session bses = GetSession();
            int i = 1;
            if (args[0] == "city") {
                using (UnitOfWork ses = bses.BeginNestedUnitOfWork()) {
                    XPQuery<csAddress> aq = new XPQuery<csAddress>(ses);
                    var q = from address in aq
                            where (address.City == null ||
                                   address.City == "" ||
                                   address.City == " ") &&
                                  !(address.AddressString == null ||
                                    address.AddressString == "" ||
                                    address.AddressString == " ")
                            select address;
                    foreach (csAddress address in q) {
                        System.Console.WriteLine(address.AddressString + " : " +
                                                 address.AddressHandmake + " : " +
                                                 address.City);
                        address.City = "Не задан";
                    }
                    System.Console.WriteLine(q.Count());
                    ses.CommitChanges();
                }

            }
            if (args[0] == "party") {
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet = null;

                xlApp = new Excel.ApplicationClass();

                // Открытие документа (книги) excel
                xlWorkBook = xlApp.Workbooks.Open("e:\\party2.xls");

                // Выбирается Лист (если существует)
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //
                //try {
                    while (true) {
                        using (UnitOfWork ses = bses.BeginNestedUnitOfWork()) {
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
                            if (xlWorkSheet.get_Range("E" + i).Value2 != null)
                                ep.country = UpFirstCase(xlWorkSheet.get_Range("E" + i).Value2.ToString());
                            else
                                ep.country = String.Empty;
                            if (xlWorkSheet.get_Range("F" + i).Value2 != null)
                                ep.city = UpFirstCase(xlWorkSheet.get_Range("F" + i).Value2.ToString());
                            else
                                ep.city = "-";
                            if (xlWorkSheet.get_Range("G" + i).Value2 != null)
                                ep.legal = xlWorkSheet.get_Range("G" + i).Value2.ToString();
                            else
                                ep.legal = String.Empty;
                            if (xlWorkSheet.get_Range("H" + i).Value2 != null)
                                ep.lpt = xlWorkSheet.get_Range("H" + i).Value2.ToString();
                            else
                                ep.lpt = "-";
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
                                    //lp.Save();
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
                                    //php.Save();
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
                                    //bp.Save();
                                    break;
                                case "ФИЛИАЛ":
                                    crmCLegalPersonUnit lpu = UpdateLegalPersonUnit(ses, ep);
                                    //lpu.Save();
                                    break;
                                case "ПРОЧИЕ":
                                    crmCParty party2 = new crmCParty(ses);
                                    party2.Code = ep.code;
                                    party2.Name = ep.name;
                                    party2.INN = ep.inn;
                                    party2.AddressFact.Country = GetCountry(ses, ep.country);
                                    party2.AddressFact.City = ep.city;
                                    party2.AddressFact.AddressHandmake = ep.addr;
                                    if (!String.IsNullOrEmpty(ep.close.Trim()))
                                        party2.IsClosed = true;
                                    //party2.Save();
                                    break;
                                case "ДОМ":
                                    crmCParty party = new crmCParty(ses);
                                    party.Code = ep.code;
                                    party.Name = ep.name;
                                    party.INN = ep.inn;
                                    party.AddressFact.Country = GetCountry(ses, ep.country);
                                    party.AddressFact.City = ep.city;
                                    party.AddressFact.AddressHandmake = ep.addr;
                                    if (!String.IsNullOrEmpty(ep.close.Trim()))
                                        party.IsClosed = true;
                                    //party.Save();
                                    break;
                                default:
                                    throw new NotSupportedException();
                            }

                            //MessageBox.Show(xlWorkSheet.get_Range("B1", "B1").Value2.ToString());

                            System.Console.WriteLine(ep.type + ' ' + ep.code + ' ' + ep.name + ' ' + ep.addr);
                            ses.CommitChanges();
                        }
                    }
                //}
                //catch (Exception e) {
                //        if (xlWorkSheet != null) releaseObject(xlWorkSheet);
                //        if (xlWorkBook != null) releaseObject(xlWorkBook);
                //        if (xlApp != null) releaseObject(xlApp);
                //        xlWorkSheet = null;
                //        xlWorkBook = null;
                //        xlApp = null;
                //        GC.Collect();
                //        System.Console.WriteLine(e);
                //        throw new Exception("Error clear", e);
                //    }

                // Закрытие книги
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }
            if (args[0] == "dogovor") {
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet = null;

                xlApp = new Excel.ApplicationClass();

                // Открытие документа (книги) excel
                xlWorkBook = xlApp.Workbooks.Open("e:\\Договора.xlsx");

                // Выбирается Лист (если существует)
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //
                try {
                    while (true) {
                        using (UnitOfWork ses = bses.BeginNestedUnitOfWork()) {
                            i++;
                            ExcelDogovor dog = new ExcelDogovor();
                            //                    Excel.Range range = xlWorkSheet.get_Range("A" + i, "P" + i);
                            //                    Excel.Range range2 = xlWorkSheet.get_Range("A1");
                            if (xlWorkSheet.get_Range("A" + i).Value2 == null) break;
                            dog.staff_code = xlWorkSheet.get_Range("A" + i).Value2.ToString();
                            dog.dep_code = xlWorkSheet.get_Range("B" + i).Value2.ToString();
                            dog.type = xlWorkSheet.get_Range("C" + i).Value2.ToString();

                            dog.dog_id = xlWorkSheet.get_Range("F" + i).Value2.ToString();
                            dog.dog_num = xlWorkSheet.get_Range("G" + i).Value2.ToString();
                            String str_date = xlWorkSheet.get_Range("H" + i).Value2.ToString();
                            dog.dog_date = DateTime.Parse(str_date.Substring(0, 4) + "." +
                                                          str_date.Substring(4, 2) + "." +
                                                          str_date.Substring(6, 2));
                            if (xlWorkSheet.get_Range("I" + i).Value2 != null) {
                                dog.dop_id = xlWorkSheet.get_Range("I" + i).Value2.ToString();
                                dog.dop_num = xlWorkSheet.get_Range("J" + i).Value2.ToString();
                                str_date = xlWorkSheet.get_Range("K" + i).Value2.ToString();
                                dog.dop_date = DateTime.Parse(str_date.Substring(0, 4) + "." +
                                                              str_date.Substring(4, 2) + "." +
                                                              str_date.Substring(6, 2));
                                //dog.dop_date = (DateTime)xlWorkSheet.get_Range("K" + i).Value2;
                            }

                            dog.customer_code = xlWorkSheet.get_Range("L" + i).Value2.ToString();
                            dog.supplier_code = xlWorkSheet.get_Range("P" + i).Value2.ToString();

                            crmCParty cust = GetParty(ses, dog.customer_code);
                            if (cust == null) new KeyNotFoundException(dog.customer_code);
                            crmCParty supl = GetParty(ses, dog.supplier_code);
                            if (supl == null) new KeyNotFoundException(dog.supplier_code);

                            dog.dep = GetDepartment(ses, dog.dep_code);
                            if (dog.dep == null) new KeyNotFoundException(dog.dep_code);

                            dog.reg_dep = GetDepartment(ses, "00-056");
                            if (dog.reg_dep == null) new KeyNotFoundException("00-056");

                            dog.reg_staff = GetStaff(ses, dog.staff_code);
                            if (dog.reg_staff == null) new KeyNotFoundException(dog.staff_code);

                            crmContract contract = GetContract(ses, dog);
                            crmContractDeal deal = new crmDealWithStage(ses);

                            deal.State = DealStates.DEAL_FORMATION;

                            deal.CuratorDepartment = dog.dep;
                            deal.UserRegistrator = dog.reg_staff;
                            deal.DepartmentRegistrator = dog.reg_dep;
                            deal.Customer = cust;
                            deal.Supplier = supl;

                            contract.ContractDeals.Add(deal);
                            if (dog.type == "ДОГ") {
                                deal.ContractKind = ContractKind.CONTRACT;
                                deal.ContractDocument = contract.ContractDocument;
                            }
                            else {
                                deal.ContractKind = ContractKind.ADDENDUM;
                                crmContractDocument contract_document = new crmContractDocument(ses);
                                contract_document.DocumentCategory = GetDocumentCategory(ses, "ДС");
                                contract_document.Number = dog.dop_num;
                                contract_document.Date = dog.dop_date;
                                deal.ContractDocument = contract_document;
                                contract.ContractDocuments.Add(contract_document);
                            }

                            System.Console.WriteLine(dog.type + ' ' + dog.dog_id + ' ' + dog.dog_num + ' ' + dog.dog_date + ' ' + dog.customer_code + ' ' + dog.supplier_code);
                            ses.CommitChanges();
                        }
                    }
                }
                catch (Exception e) {
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
            if (args[0] == "subject") {
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet = null;

                xlApp = new Excel.ApplicationClass();

                // Открытие документа (книги) excel
                xlWorkBook = xlApp.Workbooks.Open("e:\\subject.xlsx");

                // Выбирается Лист (если существует)
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //
                try {
                    while (true) {
                        using (UnitOfWork ses = bses.BeginNestedUnitOfWork()) {
                            i++;
                            FinanceTema ft = new FinanceTema();
                            if (xlWorkSheet.get_Range("A" + i).Value2 == null) break;
                            ft.dir = xlWorkSheet.get_Range("A" + i).Value2.ToString();
                            ft.code = xlWorkSheet.get_Range("B" + i).Value2.ToString();
                            ft.name = xlWorkSheet.get_Range("C" + i).Value2.ToString();
                            if (xlWorkSheet.get_Range("D" + i).Value2 != null)
                                ft.desc = xlWorkSheet.get_Range("D" + i).Value2.ToString();
                            else
                                ft.desc = ft.name;
                            fmCDirection dir = GetObject<fmCDirection>(ses, ft.dir);
                            fmCSubjectExt subj = GetObject<fmCSubjectExt>(ses, ft.code);
                            if (subj == null) {
                                subj = new fmCSubjectExt(ses);
                            }
                            subj.Code = ft.code;
                            subj.Direction = dir;
                            subj.Name = ft.name;
                            subj.NameFull = ft.name;
                            subj.Description = ft.desc;
                            if (subj.Name.Length > 80) {
                                subj.Name = subj.Name.Substring(0, 80);
                            }
                            subj.Save();
                            System.Console.WriteLine(ft.dir + ' ' + ft.code + ' ' + ft.name);
                            ses.CommitChanges();
                        }
                    }
                }
                catch (Exception e) {
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
            if (args[0] == "order") {
                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet = null;

                xlApp = new Excel.ApplicationClass();

                // Открытие документа (книги) excel
                xlWorkBook = xlApp.Workbooks.Open("e:\\order2.xlsx");

                // Выбирается Лист (если существует)
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //
                try {
                    while (true) {
                        using (UnitOfWork ses = bses.BeginNestedUnitOfWork()) {
                            i++;
                            FinanceOrder fo = new FinanceOrder();
                            if (xlWorkSheet.get_Range("A" + i).Value2 == null) break;
                            if (xlWorkSheet.get_Range("A" + i).Value2 is Double)
                                fo.buh_int_num = (Int32)((Double)xlWorkSheet.get_Range("A" + i).Value2);
                            if (xlWorkSheet.get_Range("B" + i).Value2 != null)
                                fo.buh_account = xlWorkSheet.get_Range("B" + i).Value2.ToString();
                            else
                                fo.buh_account = "";
                            fo.code = xlWorkSheet.get_Range("C" + i).Value2.ToString();
                            if (xlWorkSheet.get_Range("D" + i).Value2 != null)
                                fo.is_closed = true;
                            else
                                fo.is_closed = false;
                            if (xlWorkSheet.get_Range("E" + i).Value2 != null)
                                fo.subj = xlWorkSheet.get_Range("E" + i).Value2.ToString();
                            else
                                fo.subj = "";
                            if (xlWorkSheet.get_Range("F" + i).Value2 != null)
                                fo.name_short = xlWorkSheet.get_Range("F" + i).Value2.ToString();
                            else
                                fo.name_short = "";
                            if (xlWorkSheet.get_Range("G" + i).Value2 != null)
                                fo.name_full = xlWorkSheet.get_Range("G" + i).Value2.ToString();
                            else
                                fo.name_full = "";
                            if (xlWorkSheet.get_Range("H" + i).Value2 != null)
                                fo.desc = xlWorkSheet.get_Range("H" + i).Value2.ToString();
                            else
                                fo.desc = "";
                            if (xlWorkSheet.get_Range("I" + i).Value2 != null)
                                fo.data_from = xlWorkSheet.get_Range("I" + i).Value2.ToString();
                            else
                                fo.data_from = "";
                            if (xlWorkSheet.get_Range("J" + i).Value2 != null)
                                fo.data_to = xlWorkSheet.get_Range("J" + i).Value2.ToString();
                            else
                                fo.data_to = "";
                            if (xlWorkSheet.get_Range("K" + i).Value2 != null)
                                fo.base_doc = xlWorkSheet.get_Range("K" + i).Value2.ToString();
                            else
                                fo.base_doc = "";
                            if (xlWorkSheet.get_Range("L" + i).Value2 != null)
                                fo.work_type = xlWorkSheet.get_Range("L" + i).Value2.ToString();
                            else
                                fo.work_type = "";
                            if (xlWorkSheet.get_Range("M" + i).Value2 != null)
                                fo.finans = xlWorkSheet.get_Range("M" + i).Value2.ToString();
                            else
                                fo.finans = "";
                            if (xlWorkSheet.get_Range("N" + i).Value2 != null)
                                fo.source = xlWorkSheet.get_Range("N" + i).Value2.ToString();
                            else
                                fo.source = "";
                            if (xlWorkSheet.get_Range("O" + i).Value2 != null)
                                fo.army = xlWorkSheet.get_Range("O" + i).Value2.ToString();
                            else
                                fo.army = "";
                            if (xlWorkSheet.get_Range("P" + i).Value2 != null)
                                fo.nds_mode = xlWorkSheet.get_Range("P" + i).Value2.ToString();
                            else
                                fo.nds_mode = "";
                            if (xlWorkSheet.get_Range("Q" + i).Value2 != null)
                                fo.acc_mode = xlWorkSheet.get_Range("Q" + i).Value2.ToString();
                            else
                                fo.acc_mode = "";
                            if (xlWorkSheet.get_Range("R" + i).Value2 != null)
                                if (xlWorkSheet.get_Range("R" + i).Value2 is Double)
                                    fo.koeff_ozm = (Decimal)((Double)xlWorkSheet.get_Range("R" + i).Value2);
                            if (xlWorkSheet.get_Range("S" + i).Value2 != null)
                                if (xlWorkSheet.get_Range("S" + i).Value2 is Double)
                                    fo.koeff_kb = (Decimal)((Double)xlWorkSheet.get_Range("S" + i).Value2);
                            fmCSubjectExt subj = GetObject<fmCSubjectExt>(ses, fo.subj);
                            fmCOrderExt ord = GetObject<fmCOrderExt>(ses, fo.code);
                            if (ord == null) {
                                ord = new fmCOrderExt(ses);
                                ord.Status = fmIOrderStatus.Loaded;
                            }
                            ord.Code = fo.code;
                            ord.Subject = subj;
                            ord.IsClosed = fo.is_closed;
                            ord.Name = fo.name_short;
                            ord.NameFull = fo.name_full;
                            ord.Description = fo.desc;
                            if (!String.IsNullOrEmpty(fo.data_from))
                                ord.DateBegin = new DateTime(Int32.Parse(fo.data_from.Substring(0, 4)),
                                                             Int32.Parse(fo.data_from.Substring(4, 2)),
                                                             Int32.Parse(fo.data_from.Substring(6, 2)));
                            if (!String.IsNullOrEmpty(fo.data_to))
                                ord.DateEnd = new DateTime(Int32.Parse(fo.data_to.Substring(0, 4)),
                                                           Int32.Parse(fo.data_to.Substring(4, 2)),
                                                           Int32.Parse(fo.data_to.Substring(6, 2)));
                            if (!String.IsNullOrEmpty(fo.acc_mode))
                                ord.AnalitycAccouterType = GetObject<fmСOrderAnalitycAccouterType>(ses, fo.acc_mode);
                            if (!String.IsNullOrEmpty(fo.nds_mode))
                                ord.AnalitycAVT = GetObject<fmСOrderAnalitycAVT>(ses, fo.nds_mode);
                            ord.BuhAccount = fo.buh_account;
                            ord.BuhIntNum = fo.buh_int_num;
                            ord.KoeffKB = fo.koeff_kb;
                            ord.KoeffOZM = fo.koeff_ozm;
                            ord.SourceOther = fo.base_doc;
                            //                                ord.
                            if (!String.IsNullOrEmpty(fo.work_type))
                                ord.AnalitycWorkType = GetObject<fmСOrderAnalitycWorkType>(ses, fo.work_type);
                            if (!String.IsNullOrEmpty(fo.acc_mode))
                                ord.AnalitycFinanceSource = GetObject<fmСOrderAnalitycFinanceSource>(ses, fo.finans);
                            if (!String.IsNullOrEmpty(fo.acc_mode))
                                ord.AnalitycOrderSource = GetObject<fmСOrderAnalitycOrderSource>(ses, fo.source);
                            if (!String.IsNullOrEmpty(fo.acc_mode))
                                ord.AnalitycMilitary = GetObject<fmСOrderAnalitycMilitary>(ses, fo.army);
                            System.Console.WriteLine(fo.subj + ' ' + fo.code + ' ' + fo.name_short);
                            ses.CommitChanges();
                        }
                    }
                }
                catch (Exception e) {
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
