using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    static public class fmCAVTBookVATLogic {

        static public void ImportBuhData(IObjectSpace os, fmCAVTBookVAT book) {
            if (book.BookVATType == fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN)
                ImportBookPayMain(os, book);
            if (book.BookVATType == fmCAVTBookVAT.fmCAVTBookVATType.PAY_2014)
                ImportBookPayMain2014(os, book);
            if (book.BookVATType == fmCAVTBookVAT.fmCAVTBookVATType.BAY_MAIN)
                ImportBookBayMain(os, book);
        }

        static public void ImportBookPayMain(IObjectSpace os, fmCAVTBookVAT book) {
            Int32 kvartal = Int32.Parse(book.PeriodKV);
            String period_m1 = book.PeriodYYYY + ((kvartal - 1) * 3 + 1).ToString("00");
            String period_m2 = book.PeriodYYYY + ((kvartal - 1) * 3 + 2).ToString("00");
            String period_m3 = book.PeriodYYYY + ((kvartal - 1) * 3 + 3).ToString("00");
            os.Delete(book.BookVATRecords);
            IList<fmCAVTBookBuhRecord> buhrecs = os.GetObjects<fmCAVTBookBuhRecord>(
                XPQuery<fmCAVTBookBuhRecord>.TransformExpression(((ObjectSpace)os).Session,
                rec => rec.BookType == "P" && (
                       rec.PeriodOtchet == period_m1 ||
                       rec.PeriodOtchet == period_m2 ||
                       rec.PeriodOtchet == period_m3 
//                       &&
//                       (rec.RecordType != "AON" ||
//                        rec.RecordType == "AON" &&
//                        rec.RecordSummType == "NDS" &&
//                        rec.SummVAT != 0)
                    )
                ));
            var invoice_buhrecs = buhrecs.GroupBy(rec => new {
                rec.RecordType,
                rec.AVTInvoicePartyCode,
                rec.AVTInvoiceType,
                rec.AVTInvoiceNumber,
                rec.AVTInvoiceDate
            }).OrderBy(key => key.Key.AVTInvoicePartyCode);
            UInt32 seq_num = 1;
            foreach (var invoice_buhrec in invoice_buhrecs) {
                if (String.IsNullOrEmpty(invoice_buhrec.Key.AVTInvoiceType.Trim())) continue;
                if (invoice_buhrec.Key.AVTInvoiceType.Trim() == "ÒÄ") continue;
                fmCAVTBookVATRecord rec = os.CreateObject<fmCAVTBookVATRecord>();
                crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", invoice_buhrec.Key.AVTInvoicePartyCode)).FirstOrDefault();
                rec.BuhRecordType = invoice_buhrec.Key.RecordType;
                rec.Party = party;
                rec.VATInvoiceType = invoice_buhrec.Key.AVTInvoiceType;
                rec.VATInvoiceNumber = invoice_buhrec.Key.AVTInvoiceNumber;
                rec.VATInvoiceDate = invoice_buhrec.Key.AVTInvoiceDate;
                rec.Invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("Number", invoice_buhrec.Key.AVTInvoiceNumber),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", book.Party)));
                rec.PayDate = invoice_buhrec.Select(buhrec => buhrec.PayDocDate).Max();
                rec.BuhDate = invoice_buhrec.Select(buhrec => buhrec.BuhDocDate).Min();
                if (invoice_buhrec.Key.AVTInvoiceType == "ÑÔÇ") rec.PayDate = rec.BuhDate;
                rec.SummVAT_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVAT);
                if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                    rec.SummCost_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummAll) - rec.SummVAT_18;
                else
                    rec.SummCost_18 = Decimal.Round(rec.SummVAT_18 * 100m / 18m, 2);
                
                rec.SummVAT_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVAT);
                if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                    rec.SummCost_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummAll) - rec.SummVAT_10;
                else
                    rec.SummCost_10 = Decimal.Round(rec.SummVAT_10 * 100m / 10m, 2);
                if (rec.SummCost_10 < 0) {
                    rec.SummCost_10 = Decimal.Round(rec.SummVAT_10 * 100m / 10m, 2);
                }
                rec.SummCost_0 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "4").Sum(buhrec => buhrec.SummAll);
                rec.SummCost_NoVAT = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "5").Sum(buhrec => buhrec.SummAll);
                if (rec.SummVAT_18 == 0 && rec.SummVAT_10 == 0 && rec.SummCost_0 == 0 && rec.SummCost_NoVAT == 0) {
                    os.Delete(rec);
                    continue;
                }
                rec.SequenceNumber = seq_num++;
                book.BookVATRecords.Add(rec);
                switch (invoice_buhrec.Key.RecordType) {
                    case "PAY":
                        break;
                    case "AIN":
                        break;
                    case "AON":
                        break;
                    case "EAT":
                        break;
                    case "EXP":
                        break;
                    case "SMN":
                        break;
                    case "SPC":
                        break;
                    default:
                        break;
                }
            }
        }
        static public void ImportBookPayMain2014(IObjectSpace os, fmCAVTBookVAT book) {
            Int32 kvartal = Int32.Parse(book.PeriodKV);
            String period_m1 = book.PeriodYYYY + ((kvartal - 1) * 3 + 1).ToString("00");
            String period_m2 = book.PeriodYYYY + ((kvartal - 1) * 3 + 2).ToString("00");
            String period_m3 = book.PeriodYYYY + ((kvartal - 1) * 3 + 3).ToString("00");
            os.Delete(book.BookVATRecords);
            IList<fmCAVTBookBuhRecord> buhrecs = os.GetObjects<fmCAVTBookBuhRecord>(
                XPQuery<fmCAVTBookBuhRecord>.TransformExpression(((ObjectSpace)os).Session,
                rec => rec.BookType == "P" && (
                       rec.PeriodOtchet == period_m1 ||
                       rec.PeriodOtchet == period_m2 ||
                       rec.PeriodOtchet == period_m3
                    //                       &&
                    //                       (rec.RecordType != "AON" ||
                    //                        rec.RecordType == "AON" &&
                    //                        rec.RecordSummType == "NDS" &&
                    //                        rec.SummVAT != 0)
                    )
                ));
            var invoice_buhrecs = buhrecs.GroupBy(rec => new {
                rec.RecordType,
                rec.AVTInvoicePartyCode,
                rec.AVTInvoiceType,
                rec.AVTInvoiceNumber,
                rec.AVTInvoiceDate
            }).OrderBy(key => key.Key.AVTInvoicePartyCode);
            UInt32 seq_num = 1;
            foreach (var invoice_buhrec in invoice_buhrecs) {
                if (String.IsNullOrEmpty(invoice_buhrec.Key.AVTInvoiceType.Trim())) continue;
                if (invoice_buhrec.Key.AVTInvoiceType.Trim() == "ÒÄ") continue;
                fmCAVTBookVATRecord rec = os.CreateObject<fmCAVTBookVATRecord>();
                crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", invoice_buhrec.Key.AVTInvoicePartyCode)).FirstOrDefault();
                rec.BuhRecordType = invoice_buhrec.Key.RecordType;
                rec.Party = party;
                rec.VATInvoiceType = invoice_buhrec.Key.AVTInvoiceType;
                rec.VATInvoiceNumber = invoice_buhrec.Key.AVTInvoiceNumber;
                rec.VATInvoiceDate = invoice_buhrec.Key.AVTInvoiceDate;
                rec.Invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("Number", invoice_buhrec.Key.AVTInvoiceNumber),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", book.Party)));
                rec.PayDate = invoice_buhrec.Select(buhrec => buhrec.PayDocDate).Max();
                rec.BuhDate = invoice_buhrec.Select(buhrec => buhrec.BuhDocDate).Min();
                if (invoice_buhrec.Key.AVTInvoiceType == "ÑÔÇ") rec.PayDate = rec.BuhDate;
                rec.SummVAT_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVAT);
                if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                    rec.SummCost_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummAll) - rec.SummVAT_18;
                else
                    rec.SummCost_18 = Decimal.Round(rec.SummVAT_18 * 100m / 18m, 2);

                rec.SummVAT_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVAT);
                if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                    rec.SummCost_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummAll) - rec.SummVAT_10;
                else
                    rec.SummCost_10 = Decimal.Round(rec.SummVAT_10 * 100m / 10m, 2);
                if (rec.SummCost_10 < 0) {
                    rec.SummCost_10 = Decimal.Round(rec.SummVAT_10 * 100m / 10m, 2);
                }
                rec.SummCost_0 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "4").Sum(buhrec => buhrec.SummAll);
                rec.SummCost_NoVAT = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "5").Sum(buhrec => buhrec.SummAll);
                if (rec.SummVAT_18 == 0 && rec.SummVAT_10 == 0 && rec.SummCost_0 == 0 && rec.SummCost_NoVAT == 0) {
                    os.Delete(rec);
                    continue;
                }
                rec.SequenceNumber = seq_num++;
                book.BookVATRecords.Add(rec);
                switch (invoice_buhrec.Key.RecordType) {
                    case "PAY":
                        break;
                    case "AIN":
                        break;
                    case "AON":
                        break;
                    case "EAT":
                        break;
                    case "EXP":
                        break;
                    case "SMN":
                        break;
                    case "SPC":
                        break;
                    default:
                        break;
                }
            }
            IList<fmCAVTBookBuhStruct> book_structs = os.GetObjects<fmCAVTBookBuhStruct>();
            foreach (var book_struct in book_structs) {
                foreach (var record in book_struct.OutInvoiceRecords) { 
                    if (record.SaleSummVAT != 0) {
                        fmCAVTBookVATRecord book_rec = os.CreateObject<fmCAVTBookVATRecord>();
//                        rec.BuhRecordType = invoice_buhrec.Key.RecordType;
                        book_rec.Party = record.PartnerParty;
                        book_rec.VATInvoiceType = record.InvoiceType;
                        book_rec.VATInvoiceNumber = record.InvoiceNumber;
                        book_rec.VATInvoiceDate = record.InvoiceDate;
                        book_rec.Invoice = record.Invoice;
//                        rec.PayDate = record.;
                        book_rec.BuhDate = record.InvoiceDate;
                        if (record.InvoiceType == "ÑÔÇ") book_rec.PayDate = book_rec.BuhDate;
//                        if (record.Nds)
//                        book_rec.SummVAT_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVAT);
                        book_rec.SummVAT_18 = record.SaleSummVAT;
                        book_rec.SummCost_18 = record.SaleSummAll - record.SaleSummVAT;
//                        if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                        //    book_rec.SummCost_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummAll) - book_rec.SummVAT_18;
                        //else
                        //    book_rec.SummCost_18 = Decimal.Round(book_rec.SummVAT_18 * 100m / 18m, 2);

                        //book_rec.SummVAT_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVAT);
                        //if (invoice_buhrec.Key.AVTInvoiceType != "ÑÔÂ")
                        //    book_rec.SummCost_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummAll) - book_rec.SummVAT_10;
                        //else
                        //    book_rec.SummCost_10 = Decimal.Round(book_rec.SummVAT_10 * 100m / 10m, 2);
                        //if (book_rec.SummCost_10 < 0) {
                        //    book_rec.SummCost_10 = Decimal.Round(book_rec.SummVAT_10 * 100m / 10m, 2);
                        //}
                        //book_rec.SummCost_0 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "4").Sum(buhrec => buhrec.SummAll);
                        //book_rec.SummCost_NoVAT = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "5").Sum(buhrec => buhrec.SummAll);
                        if (book_rec.SummVAT_18 == 0 && book_rec.SummVAT_10 == 0 && book_rec.SummCost_0 == 0 && book_rec.SummCost_NoVAT == 0) {
                            os.Delete(book_rec);
                            continue;
                        }
                        book_rec.SequenceNumber = seq_num++;
                        book.BookVATRecords.Add(book_rec);
                    }
                }
            }
        }
        static public void ImportBookBayMain(IObjectSpace os, fmCAVTBookVAT book) {
            Int32 kvartal = Int32.Parse(book.PeriodKV);
            String period_m1 = book.PeriodYYYY + ((kvartal - 1) * 3 + 1).ToString("00");
            String period_m2 = book.PeriodYYYY + ((kvartal - 1) * 3 + 2).ToString("00");
            String period_m3 = book.PeriodYYYY + ((kvartal - 1) * 3 + 3).ToString("00");
            os.Delete(book.BookVATRecords);
            IList<fmCAVTBookBuhRecord> buhrecs = os.GetObjects<fmCAVTBookBuhRecord>(
                XPQuery<fmCAVTBookBuhRecord>.TransformExpression(((ObjectSpace)os).Session,
                rec => rec.BookType == "B" && (
                       rec.PeriodOtchet == period_m1 ||
                       rec.PeriodOtchet == period_m2 ||
                       rec.PeriodOtchet == period_m3
                       //&& (rec.RecordType != "AON" ||
                       // rec.RecordType == "AON" &&
                       // rec.RecordSummType == "NDS" &&
                       // rec.SummVAT != 0)
                        )
                ));
            var invoice_buhrecs = buhrecs.GroupBy(rec => new {
                rec.RecordType,
                rec.AVTInvoicePartyCode,
                rec.AVTInvoiceType,
                rec.AVTInvoiceRegNumber,
                rec.AVTInvoiceNumber,
                rec.AVTInvoiceDate
            }).OrderBy(key => key.Key.AVTInvoicePartyCode);
            //
            UInt32 seq_num = 1;
            foreach (var invoice_buhrec in invoice_buhrecs) {
                if (String.IsNullOrEmpty(invoice_buhrec.Key.AVTInvoiceType.Trim())) continue;
                if (invoice_buhrec.Key.AVTInvoiceType.Trim() == "ÒÄ") continue;
                fmCAVTBookVATRecord rec = os.CreateObject<fmCAVTBookVATRecord>();
                crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", invoice_buhrec.Key.AVTInvoicePartyCode)).FirstOrDefault();
                rec.BuhRecordType = invoice_buhrec.Key.RecordType;
                rec.Party = party;
                rec.VATInvoiceType = invoice_buhrec.Key.AVTInvoiceType;
                rec.VATInvoiceRegNumber = invoice_buhrec.Key.AVTInvoiceRegNumber;
                rec.VATInvoiceNumber = invoice_buhrec.Key.AVTInvoiceNumber;
                rec.VATInvoiceDate = invoice_buhrec.Key.AVTInvoiceDate;
                rec.Invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("Number", invoice_buhrec.Key.AVTInvoiceNumber),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", invoice_buhrec.Key.AVTInvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", party)));
                rec.PayDate = invoice_buhrec.Select(buhrec => buhrec.PayDocDate).Max();
                rec.BuhDate = invoice_buhrec.Select(buhrec => buhrec.BuhDocDate).Min();
                Decimal SummNoVAT_18;
                Decimal SummNoVAT_10;
                rec.SummVAT_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVAT);
                SummNoVAT_18 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVATCost);
                rec.SummVAT_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVAT);
                SummNoVAT_10 = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVATCost);
                rec.SummCost_NoVAT = invoice_buhrec.Where(buhrec => buhrec.NDSRate == "5").Sum(buhrec => buhrec.SummAll);
                rec.SummCost_18 = Decimal.Round(rec.SummVAT_18 * 100m / 18m, 2);
                rec.SummCost_10 = Decimal.Round(rec.SummVAT_10 * 100m / 10m, 2);
                rec.SummCost_NoVAT = rec.SummCost_NoVAT +
                    Decimal.Round(SummNoVAT_18 * 118m / 18m, 2) +
                    Decimal.Round(SummNoVAT_10 * 110m / 10m, 2);
                if (rec.SummAll == 0 && rec.SummVAT_18 == 0 && rec.SummVAT_10 == 0) {
                    os.Delete(rec);
                    continue;
                }
                rec.SequenceNumber = seq_num++;
                book.BookVATRecords.Add(rec);
                switch (invoice_buhrec.Key.RecordType) {
                    case "PAY":
                        break;
                    case "AIN":
                        break;
                    case "AON":
                        break;
                    case "EAT":
                        break;
                    case "EXP":
                        break;
                    case "SMN":
                        break;
                    case "SPC":
                        break;
                    default:
                        break;
                }
            }
        }

        static public fmCAVTBookVAT LocateBook(IObjectSpace os, crmCParty organ, fmCAVTBookVAT.fmCAVTBookVATType type, String period) {
            IList<fmCAVTBookVAT> books = os.GetObjects<fmCAVTBookVAT>(
                XPQuery<fmCAVTBookVAT>.TransformExpression(((ObjectSpace)os).Session,
                    b => b.Party == organ &&
                         b.Period == period), true).ToList();
            fmCAVTBookVAT book_main = null;
            foreach (fmCAVTBookVAT book in books) {
                if ((type == fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN ||
                     type == fmCAVTBookVAT.fmCAVTBookVATType.PAY_ADD) &&
                     book.BookVATType == fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN) {
                    book_main = book;
                    break;
                }
                if ((type == fmCAVTBookVAT.fmCAVTBookVATType.BAY_MAIN ||
                     type == fmCAVTBookVAT.fmCAVTBookVATType.BAY_ADD) &&
                     book.BookVATType == fmCAVTBookVAT.fmCAVTBookVATType.BAY_MAIN) {
                    book_main = book;
                    break;
                }
            }
            if (book_main == null) {

                if (type == fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN ||
                    type == fmCAVTBookVAT.fmCAVTBookVATType.PAY_ADD) {
                }
            }
            return book_main;
        }
        static public fmCAVTBookVAT LocateBookMain(IObjectSpace os, crmCParty organ, fmCAVTBookVAT.fmCAVTBookVATType type, String period) {
            if (type != fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN &&
                type != fmCAVTBookVAT.fmCAVTBookVATType.BAY_MAIN)
                throw new ArgumentException();
            fmCAVTBookVAT book = os.FindObject<fmCAVTBookVAT>(
                XPQuery<fmCAVTBookVAT>.TransformExpression(((ObjectSpace)os).Session,
                    b => b.Party == organ &&
                         b.BookVATType == type &&
                         b.Period == period), true);
            if (book == null) {
                book = os.CreateObject<fmCAVTBookVAT>();
                book.Party = organ;
                book.BookVATType = type;
                book.Period = period;
            }
            return book;
        }
        static public fmCAVTBookVAT LocateBookAdd(IObjectSpace os, crmCParty organ, fmCAVTBookVAT.fmCAVTBookVATType type, String period) {
            if (type != fmCAVTBookVAT.fmCAVTBookVATType.PAY_ADD &&
                type != fmCAVTBookVAT.fmCAVTBookVATType.BAY_ADD)
                throw new ArgumentException();
            fmCAVTBookVAT book_main = null;
            if (type == fmCAVTBookVAT.fmCAVTBookVATType.PAY_ADD)
                book_main = LocateBookMain(os, organ, fmCAVTBookVAT.fmCAVTBookVATType.PAY_MAIN, period);
            if (type == fmCAVTBookVAT.fmCAVTBookVATType.BAY_ADD)
                book_main = LocateBookMain(os, organ, fmCAVTBookVAT.fmCAVTBookVATType.BAY_MAIN, period);
            return book_main;
        }
    }

}