using System;
using System.Collections.Generic;
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
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.FM.AVT {

    public class fmCAVTBookBuhStructXMLLoader : Excel2003XmlReader {

        protected IObjectSpace ObjectSpace;
        protected fmCAVTBookBuhStruct StructBook;
        private IFormatProvider FormatProvider;
        private IList<fmCAVTInvoiceTransferType> _TransferTypes;
        private IList<fmCAVTInvoiceOperationType> _OperationTypes;
        private IList<csNDSRate> _VatRates;

        public fmCAVTBookBuhStructXMLLoader(IObjectSpace os, fmCAVTBookBuhStruct struct_book, Stream stream)
            : base(stream) {
            ObjectSpace = os;
            StructBook = struct_book;
            FormatProvider = CultureInfo.InvariantCulture;
            _TransferTypes = os.GetObjects<fmCAVTInvoiceTransferType>();
            _OperationTypes = os.GetObjects<fmCAVTInvoiceOperationType>();
            _VatRates = os.GetObjects<csNDSRate>();
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
                        if (value == null || String.IsNullOrEmpty(value.Trim()) || value == "Итог") {
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
                        CurRecord.TransferType = _TransferTypes.Where(rec => rec.Code == value).FirstOrDefault();
                        break;
                    case 4:
                        if (value.Length == 1)
                            value = '0' + value;
                        CurRecord.OperationType = _OperationTypes.Where(rec => rec.Code == value).FirstOrDefault();
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
                        String inn = value == null ? String.Empty : value.Trim();
                        CurRecord.PartnerInn = inn;
                        break;
                    case 15:
                        String kpp = value == null ? String.Empty : value.Trim();
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
                        CurRecord.SummCost = value_decimal;
                        break;
                    case 22:
                        value_double = Double.Parse(value, FormatProvider);
                        value_decimal = Decimal.Round(new Decimal(value_double), 2);
                        CurRecord.SummVAT = value_decimal;
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
                        switch (value) {
                            case "3":
                                CurRecord.BayVATRate = _VatRates.FirstOrDefault(x => x.Code == "10%");
                                break;
                            case "4":
                                CurRecord.BayVATRate = _VatRates.FirstOrDefault(x => x.Code == "0%");
                                break;
                            case "5":
                                CurRecord.BayVATRate = _VatRates.FirstOrDefault(x => x.Code == "БЕЗ НДС");
                                break;
                            default:
                                CurRecord.BayVATRate = _VatRates.FirstOrDefault(x => x.Code == "18%");
                                break;
                        }
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
//                        CurRecord.SaleVATRate = null;
                        switch (value) {
                            case "3":
                                CurRecord.SaleVATRate = _VatRates.FirstOrDefault(x => x.Code == "10%");
                                break;
                            case "4":
                                CurRecord.SaleVATRate = _VatRates.FirstOrDefault(x => x.Code == "0%");
                                break;
                            default:
                                CurRecord.SaleVATRate = _VatRates.FirstOrDefault(x => x.Code == "18%");
                                break;
                        }
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
            }
            catch (FormatException e) {
                throw new FormatException(String.Format("Лист: {0}, Строка: {1}, Колонка: {2}, Значение '{3}'", sheet, row, column, value), e);
            }
        }
    }

    //    [VisibleInReports]
    public class fmCAVTBookBuhStructLogic {

        private IObjectSpace _ObjectSpace;
        private IList<fmCAVTInvoiceType> _InvoiceTypes;
        private IList<fmCAVTInvoiceTransferType> _InvoiceTransferTypes;
        private IList<fmCAVTInvoiceOperationType> _InvoiceOperationTypes;

        public fmCAVTBookBuhStructLogic(IObjectSpace os) {
            _ObjectSpace = os;
            _InvoiceTypes = os.GetObjects<fmCAVTInvoiceType>();
            _InvoiceTransferTypes = os.GetObjects<fmCAVTInvoiceTransferType>();
            _InvoiceOperationTypes = os.GetObjects<fmCAVTInvoiceOperationType>();
        }
        public static fmCAVTBookBuhStruct Import(fmCAVTBookBuhStruct struct_book, IObjectSpace os, Stream stream) {
            fmCAVTBookBuhStructXMLLoader loader = new fmCAVTBookBuhStructXMLLoader(os, struct_book, stream);
            os.Delete(struct_book.InInvoiceRecords);
            os.Delete(struct_book.OutInvoiceRecords);
            loader.Load();
            struct_book.StatusSet(fmCAVTBookBuhStructStatus.BUH_STRUCT_IMPORTED);
            return struct_book;
        }
        enum InvoiceType {
            InvoiceIn,
            InvoiceOut
        }

        public fmCAVTBookBuhStruct Process(fmCAVTBookBuhStruct struct_book, IObjectSpace os) {
            if (struct_book.Party == null)
                throw new InvalidDataException("Заполните головную организацию");
            foreach (var record in struct_book.InInvoiceRecords) {
                ProcessRecord(struct_book, os, record, InvoiceType.InvoiceIn);
            }
            foreach (var record in struct_book.OutInvoiceRecords) {
                ProcessRecord(struct_book, os, record, InvoiceType.InvoiceOut);
            }
            struct_book.StatusSet(fmCAVTBookBuhStructStatus.BUH_STRUCT_PROCESSED);
            return struct_book;
        }

        private void ProcessRecord(fmCAVTBookBuhStruct struct_book, IObjectSpace os, fmCAVTBookBuhStructRecord record, InvoiceType invoice_type) {
            crmCParty party;
            if (String.IsNullOrEmpty(record.PartnerInn))
                throw new InvalidDataException("Незаполнен ИНН Рег: " + record.InvoiceRegNumber);
            //                    crmCLegalPerson leg_person;
            //                    crmCLegalPersonUnit legunit_person;
            //                    person = os.GetObjects<crmCPerson>(new BinaryOperator("INN", record.PartnerInn)).FirstOrDefault();
            if (invoice_type == InvoiceType.InvoiceOut && record.SaleDate < new DateTime(1990, 01, 01) && record.SaleSummVAT != 0)
                record.SaleDate = record.TransferDate;
            if (record.PartnerParty == null) {
                if (record.InvoiceType == "ГТД") {
                    party = os.GetObjects<crmCParty>(new BinaryOperator("Code", record.PartnerInn)).FirstOrDefault();
                }
                else
                    if (record.PartnerInn == "-") {
                        if (record.InvoiceType == "СФЗ" || record.PartnerName.Trim().ToUpper() == "НАСЕЛЕНИЕ") {
                            party = os.GetObjects<crmCParty>(new BinaryOperator("Code", "2706")).FirstOrDefault();
                            //                    record.InvoiceType = "СФЗ";
                        }
                        else {
                            String[] fio = record.PartnerName.Split(' ');
                            if (fio.Length != 3)
                                throw new InvalidDataException("Незаполнен ФИО физического лица '" + fio + "' Рег: " + record.InvoiceRegNumber);
                            crmCPhysicalParty person = os.GetObjects<crmCPhysicalParty>(
                                new BinaryOperator("LastName", fio[0]) &
                                new BinaryOperator("FirstName", fio[1]) &
                                new BinaryOperator("MiddleName", fio[2])
                                , true).FirstOrDefault();
                            if (person == null) {
                                person = os.CreateObject<crmCPhysicalParty>();
                                person.INN = "-";
                                person.Name = record.PartnerName;
                                person.NameHandmake = record.PartnerName;
                                person.LastName = fio[0];
                                person.FirstName = fio[1];
                                person.MiddleName = fio[2];
                                person.AddressLegal.City = record.PartnerSity;
                                person.AddressLegal.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                                //                        person.AddressFact.City = record.PartnerSity;
                                //                        person.AddressFact.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                                //                        party = person.Party;
                            }

                            party = person.Party;
                        }
                    }
                    else {
                        if (String.IsNullOrEmpty(record.PartnerKpp) || record.PartnerKpp == "-") {
                            if (record.PartnerInn.Length != 12)
                                throw new FormatException("Некорректная длина ИНН Рег: " + record.InvoiceRegNumber);
                            crmCBusinessman person = os.GetObjects<crmCBusinessman>(new BinaryOperator("INN", record.PartnerInn), true).FirstOrDefault();
                            //                         phys_person = 
                            if (person == null) {
                                person = os.CreateObject<crmCBusinessman>();
                                person.INN = record.PartnerInn;
                                String[] name_comps = record.PartnerName.Split(' ');
                                if (name_comps.Length >= 2)
                                    person.LastName = name_comps[1];
                                else
                                    person.LastName = "-";
                                if (name_comps.Length >= 3)
                                    person.FirstName = name_comps[2];
                                else
                                    person.FirstName = "-";
                                if (name_comps.Length == 4)
                                    person.MiddleName = name_comps[3];
                                else
                                    person.MiddleName = "-";
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
                            if (record.PartnerInn.Length != 10)
                                throw new FormatException("Некорректная длина ИНН Рег: " + record.InvoiceRegNumber);
                            if (record.PartnerKpp.Length != 9)
                                throw new FormatException("Некорректная длина КПП Рег: " + record.InvoiceRegNumber);
                            party = os.GetObjects<crmCParty>(new BinaryOperator("INN", record.PartnerInn) &
                                                            new BinaryOperator("KPP", record.PartnerKpp), true).FirstOrDefault();
                            if (party == null) {
                                crmCLegalPerson person;
                                person = os.GetObjects<crmCLegalPerson>(new BinaryOperator("INN", record.PartnerInn), true).FirstOrDefault();
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
                                if (person.Party.KPP == record.PartnerKpp) {
                                    party = person.Party;
                                }
                                else {
                                    crmCLegalPersonUnit unit = person.LegalPersonUnits.FirstOrDefault(x => x.KPP == record.PartnerKpp);
                                    if (unit == null) {
                                        unit = os.CreateObject<crmCLegalPersonUnit>();
                                        person.LegalPersonUnits.Add(unit);
                                        unit.KPP = record.PartnerKpp;
                                        unit.Name = record.PartnerName;
                                        unit.AddressFact.City = record.PartnerSity;
                                        unit.AddressFact.AddressHandmake = "РФ " + record.PartnerSity + " " + record.PartnerAddress;
                                    }
                                    party = unit.Party;
                                }
                            }
                        }
                        //                        person = os.GetObjects<crmCPerson>(new BinaryOperator("INN", record.PartnerInn)).FirstOrDefault()
                    }
                record.PartnerParty = party;
            }
            if (record.InvoiceType == "СЧФ" || record.InvoiceType == "УПД" || record.InvoiceType == "СФА") {
                fmCAVTInvoiceBase invoice;

                if (invoice_type == InvoiceType.InvoiceOut) {
                    invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("RegNumber", record.InvoiceRegNumber),
                                             new BinaryOperator("Date", record.InvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", record.InvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", struct_book.Party)
                        //                                         new BinaryOperator("Customer", record.PartnerParty)
                                             ), true
                                             );
                    if (invoice == null) {
                        invoice = os.CreateObject<fmCAVTInvoiceBase>();
                        invoice.RegNumber = record.InvoiceRegNumber;
                        invoice.Number = record.InvoiceNumber;
                        invoice.Date = record.InvoiceDate;
                        invoice.Supplier = struct_book.Party;
                        invoice.Customer = record.PartnerParty;
                    }
                    if (invoice != null) {
                        if (invoice.Customer != record.PartnerParty ||
                            invoice.Supplier != struct_book.Party)
                            throw new InvalidDataException("Счет фактура с регистрационным номером " + record.InvoiceRegNumber + " уже зарегистрирован с другим партнером " + invoice.Customer.Name);
                        if (invoice.InvoiceType == null && invoice.Number != null && invoice.Number.Length > 0) {
                            foreach (fmCAVTInvoiceType inv_type in _InvoiceTypes) {
                                if (inv_type.InvoiceDirection == fmAVTInvoiceDirection.AVTInvoiceOut &&
                                    inv_type.Prefix == invoice.Number.Substring(0, 1))
                                    invoice.InvoiceType = inv_type;
                            }
                        }
                        invoice.SummCost = record.SummCost;
                        invoice.SummAVT = record.SummVAT;
                        record.Invoice = invoice;
                    }
                }
                if (invoice_type == InvoiceType.InvoiceIn) {
                    invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("RegNumber", record.InvoiceRegNumber),
                                             new BinaryOperator("Date", record.InvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", record.InvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                        //                                         new BinaryOperator("Supplier", record.PartnerParty),
                                             new BinaryOperator("Customer", struct_book.Party)
                                             ), true
                                             );
                    if (invoice == null) {
                        invoice = os.CreateObject<fmCAVTInvoiceBase>();
                        invoice.RegNumber = record.InvoiceRegNumber;
                        invoice.Number = record.InvoiceNumber;
                        invoice.Date = record.InvoiceDate;
                        invoice.Supplier = record.PartnerParty;
                        invoice.Customer = struct_book.Party;
                    }
                    if (invoice != null) {
                        if (invoice.Supplier != record.PartnerParty ||
                            invoice.Customer != struct_book.Party)
                            throw new InvalidDataException("Счет фактура с регистрационным номером " + record.InvoiceRegNumber + " уже зарегистрирован с другим партнером " + invoice.Supplier.Name);
                        invoice.SummCost = record.SummCost;
                        invoice.SummAVT = record.SummVAT;
                        record.Invoice = invoice;
                    }
                }
            }
        }
    }

}