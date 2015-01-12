using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Import;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    public class fmCAVTBookBuhStructXMLLoader : Excel2003XmlReader {

        protected IObjectSpace ObjectSpace;
        protected fmCAVTBookBuhStruct StructBook;
        private IFormatProvider FormatProvider;

        public fmCAVTBookBuhStructXMLLoader(IObjectSpace os, fmCAVTBookBuhStruct struct_book, Stream stream)
            : base(stream) {
            ObjectSpace = os;
            StructBook = struct_book;
            FormatProvider = CultureInfo.InvariantCulture;
            //            Valutas = ObjectSpace.GetObjects<csValuta>();
            //            VatRates = ObjectSpace.GetObjects<csNDSRate>();
        }

        private fmCAVTBookBuhStructRecord CurRecord;
        //        private Int32 CurLine;

        protected override void ProcessCell(string sheet, int row, int column, string type, string value) {
            Double value_double;
            Decimal value_decimal;
            if (sheet != "СФ Полученные" && sheet != "СФ Выставленные" || row < 4)
                return;
            if (column > 1 && (CurRecord == null || CurRecord.RowNumber != row ||
                sheet == "СФ Полученные" && CurRecord.InInvoiceStructRecord == null ||
                sheet == "СФ Выставленные" && CurRecord.OutInvoiceStructRecord == null))
                return;
            try {
                switch (column) {
                    case 1:
                        if (value == null || String.IsNullOrEmpty(value.Trim())) {
                            CurRecord = null;
                            //                        CurRow = 0;
                        }
                        else {
                            CurRecord = ObjectSpace.CreateObject<fmCAVTBookBuhStructRecord>();
                            CurRecord.RowNumber = row;
                            //                        CurRow = row;
                            CurRecord.InvoiceRegNumber = value;
                            if (sheet == "СФ Полученные") {
                                StructBook.InInvoiceRecords.Add(CurRecord);
                            }
                            if (sheet == "СФ Выставленные") {
                                StructBook.OutInvoiceRecords.Add(CurRecord);
                            }
                        }
                        break;
                    case 2:
                        CurRecord.InvoiceType = value;
                        break;
                    case 3:
                        CurRecord.TransferType = value;
                        break;
                    case 4:
                        CurRecord.OperationType = value;
                        break;
                    case 5:
                        CurRecord.InvoiceNumber = value;
                        break;
                    case 6:
                        CurRecord.InvoiceDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 7:
                        CurRecord.InvoiceChangeNumber = value;
                        break;
                    case 8:
                        CurRecord.InvoiceChangeDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 9:
                        CurRecord.InvoiceCorrectNumber = value;
                        break;
                    case 10:
                        CurRecord.InvoiceCorrectDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 11:
                        CurRecord.InvoiceCorrectChangeNumber = value;
                        break;
                    case 12:
                        CurRecord.InvoiceCorrectChangeDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 13:
                        CurRecord.TransferDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 14:
                        String inn = value.Trim();
                        if (inn.Length != 10 && inn.Length != 12)
                            throw new FormatException("Некорректная длина ИНН");
                        CurRecord.PartnerInn = inn;
                        break;
                    case 15:
                        String kpp = value == null ? String.Empty : value.Trim();
                        if (kpp.Length != 9)
                            throw new FormatException("Некорректная длина КПП");
                        CurRecord.PartnerKpp = value;
                        break;
                    case 16:
                        CurRecord.PartnerName = value;
                        break;
                    case 17:
                        CurRecord.PartnerCountry = value;
                        break;
                    case 18:
                        CurRecord.PartnerSity = value;
                        break;
                    case 19:
                        CurRecord.PartnerAddress = value;
                        break;
                    case 20:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummAll = value_decimal;
                        break;
                    case 21:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummAll = value_decimal;
                        break;
                    case 22:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummAll = value_decimal;
                        break;
                    case 23:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummIncCost = value_decimal;
                        break;
                    case 24:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummIncVAT = value_decimal;
                        break;
                    case 25:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummDecCost = value_decimal;
                        break;
                    case 26:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummDecVAT = value_decimal;
                        break;
                    case 27:
                        CurRecord.BayDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 28:
                        CurRecord.BayVATRate = null;
                        break;
                    case 29:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.BaySummAll = value_decimal;
                        break;
                    case 30:
                        CurRecord.BayAccCode = value;
                        break;
                    case 31:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.BaySummVAT = value_decimal;
                        break;
                    case 32:
                        CurRecord.SaleDate = DateTime.Parse(value, FormatProvider);
                        break;
                    case 33:
                        CurRecord.SaleVATRate = null;
                        break;
                    case 34:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SaleSummAll = value_decimal;
                        break;
                    case 35:
                        CurRecord.SaleAccCode = value;
                        break;
                    case 36:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SaleSummVAT = value_decimal;
                        break;
                }
            } catch (FormatException e) {
                throw new FormatException(String.Format("Лист: {0}, Строка: {1}, Колонка: {2}, Значение '{3}'", sheet, row, column, value), e);
            }
        }
    }

    //    [VisibleInReports]
    public static class fmCAVTBookBuhStructLogic {

        public static fmCAVTBookBuhStruct Import(fmCAVTBookBuhStruct struct_book, IObjectSpace os, Stream stream) {
            fmCAVTBookBuhStructXMLLoader loader = new fmCAVTBookBuhStructXMLLoader(os, struct_book, stream);
            os.Delete(struct_book.InInvoiceRecords);
            os.Delete(struct_book.OutInvoiceRecords);
            loader.Load();
            return struct_book;
        }

        public static fmCAVTBookBuhStruct Process(fmCAVTBookBuhStruct struct_book, IObjectSpace os) {
            foreach (var record in struct_book.InInvoiceRecords) {
                ProcessRecord(struct_book, os, record);
            }
            foreach (var record in struct_book.OutInvoiceRecords) {
                ProcessRecord(struct_book, os, record);
            }
            return struct_book;
        }

        private static void ProcessRecord(fmCAVTBookBuhStruct struct_book, IObjectSpace os, fmCAVTBookBuhStructRecord record) {
            crmCParty party;
            if (String.IsNullOrEmpty(record.PartnerInn))
                return;
            //                    crmCLegalPerson leg_person;
            //                    crmCLegalPersonUnit legunit_person;
            //                    person = os.GetObjects<crmCPerson>(new BinaryOperator("INN", record.PartnerInn)).FirstOrDefault();
            if (String.IsNullOrEmpty(record.PartnerKpp)) {
                crmCBusinessman person = os.GetObjects<crmCBusinessman>(new BinaryOperator("INN", record.PartnerInn), true).FirstOrDefault();
                //                         phys_person = 
                if (person == null) {
                    person = os.CreateObject<crmCBusinessman>();
                    person.INN = record.PartnerInn;
                    String [] name_comps = record.PartnerName.Split();
                    if (name_comps.Length == 2)
                        person.LastName = name_comps[1];
                    else
                        person.LastName = "-";
                    if (name_comps.Length == 3)
                        person.FirstName = name_comps[2];
                    else
                        person.FirstName = "-";
                    person.Name = record.PartnerName;
                    person.NameHandmake = record.PartnerName;
                    person.AddressLegal.City = record.PartnerSity;
                    person.AddressLegal.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                    person.AddressFact.City = record.PartnerSity;
                    person.AddressFact.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                }
                party = person.Party;
            }
            else {
                crmCLegalPerson person = os.GetObjects<crmCLegalPerson>(new BinaryOperator("INN", record.PartnerInn), true).FirstOrDefault();
                if (person == null) {
                    person = os.CreateObject<crmCLegalPerson>();
                    person.INN = record.PartnerInn;
                    person.KPP = record.PartnerKpp;
                    person.Name = record.PartnerName;
                    person.AddressLegal.City = record.PartnerSity;
                    person.AddressLegal.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                    person.AddressFact.City = record.PartnerSity;
                    person.AddressFact.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                    party = person.Party;
                }
                if (person.KPP == record.PartnerKpp) 
                    party = person.Party;
                else {
                    crmCLegalPersonUnit unit = person.LegalPersonUnits.FirstOrDefault(x => x.KPP == record.PartnerKpp);
                    if (unit == null) {
                        unit = os.CreateObject<crmCLegalPersonUnit>();
                        unit.KPP = record.PartnerKpp;
                        unit.Name = record.PartnerName;
                        unit.AddressFact.City = record.PartnerSity;
                        unit.AddressFact.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                    }
                    party = unit.Party;
                }
                //                        person = os.GetObjects<crmCPerson>(new BinaryOperator("INN", record.PartnerInn)).FirstOrDefault()
            }
            record.PartnerParty = party;
        }
    }

}