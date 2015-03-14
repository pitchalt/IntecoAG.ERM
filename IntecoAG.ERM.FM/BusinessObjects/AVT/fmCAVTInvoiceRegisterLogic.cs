using System;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    static public class fmCAVTInvoiceRegisterLogic {

        [FixedLengthRecord]
        public class fmCAVTInvoiceImport {
            [FieldFixedLength(16)]
            public String SF_INT_NUMBER;
            [FieldFixedLength(9)]
            public String SF_CREATE_DATE;
            [FieldFixedLength(9)]
            public String SF_CHANGE_DATE;
            [FieldFixedLength(2)]
            public String SF_OPER_TYPE;
            [FieldFixedLength(2)]
            public String SF_TRANS_TYPE;
            [FieldFixedLength(9)]
            public String SF_TRANS_DATE;
            [FieldFixedLength(6)]
            public String SF_VO_CODE;
            [FieldFixedLength(7)]
            public String SF_PERIOD;
            [FieldFixedLength(20)]
            public String SF_REGNUM;
            [FieldFixedLength(20)]
            public String SF_NUMBER;
            [FieldFixedLength(9)]
            public String SF_DATE;
            [FieldFixedLength(20)]
            public String SF_PRAV_NUMBER;
            [FieldFixedLength(9)]
            public String SF_PRAV_DATE;
            [FieldFixedLength(3)]
            public String SF_VA_CODE;
            [FieldFixedLength(1)]
            public String IS_COR;
            [FieldFixedLength(6)]
            public String COR_VO_CODE;
            [FieldFixedLength(20)]
            public String COR_SF_NUMBER;
            [FieldFixedLength(9)]
            public String COR_SF_DATE;
            [FieldFixedLength(20)]
            public String COR_SF_PRAV_NUMBER;
            [FieldFixedLength(9)]
            public String COR_SF_PRAV_DATE;
            [FieldFixedLength(17)]
            public String SUMM_COST;
            [FieldFixedLength(17)]
            public String SUMM_NDS;
            [FieldFixedLength(17)]
            public String SUMM_ALL;
            [FieldFixedLength(17)]
            public String SUMM_ADD_COST;
            [FieldFixedLength(17)]
            public String SUMM_ADD_NDS;
            [FieldFixedLength(17)]
            public String SUMM_ADD_ALL;
            [FieldFixedLength(17)]
            public String SUMM_SUB_COST;
            [FieldFixedLength(17)]
            public String SUMM_SUB_NDS;
            [FieldFixedLength(17)]
            public String SUMM_SUB_ALL;
        }

        [DelimitedRecord(";")]
        public class fmCAVTInvoiceAvansImport {
            public String   VO_CODE;
            public String   SF_NUMBER;
            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public DateTime SF_DATE;
            public String   PP_NUMBER;
            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public DateTime PP_DATE;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal SUMM_ALL;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal SUMM_VAT;
            public String   TEXT;
        }
        static public void ImportInvoices(IObjectSpace os, fmCAVTInvoiceRegister register, String file_name) { 
//            OpenFileDialog dialog = new OpenFileDialog();
//            if (dialog.ShowDialog() == DialogResult.OK) {
            FixedFileEngine engine = new FixedFileEngine(typeof(fmCAVTInvoiceImport));
            fmCAVTInvoiceImport[] imp_res = (fmCAVTInvoiceImport[])engine.ReadFile(file_name);
            IList<fmCAVTInvoiceType> inv_types = os.GetObjects<fmCAVTInvoiceType>();
            IList<fmCAVTInvoiceTransferType> inv_transfer_types = os.GetObjects<fmCAVTInvoiceTransferType>();
            IList<fmCAVTInvoiceOperationType> inv_oper_types = os.GetObjects<fmCAVTInvoiceOperationType>();
            Int32 count = 0;
            UInt32 seq_num = 0;
            foreach (fmCAVTInvoiceImport imp_rec in imp_res) {
                imp_rec.SF_VO_CODE = imp_rec.SF_VO_CODE.Trim();
                imp_rec.SF_INT_NUMBER = imp_rec.SF_INT_NUMBER.Trim();
                imp_rec.SF_NUMBER = imp_rec.SF_NUMBER.Trim();
                Decimal summ_cost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                Decimal summ_nds = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                Decimal summ_sub_cost = Decimal.Parse(imp_rec.SUMM_SUB_COST.Trim().Replace('.', ','));
                Decimal summ_sub_nds = Decimal.Parse(imp_rec.SUMM_SUB_NDS.Trim().Replace('.', ','));
                DateTime sf_date = default(DateTime);
                DateTime.TryParseExact(imp_rec.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sf_date);
                fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
                    XPQuery<fmCAVTInvoiceBase>.TransformExpression(
                    ((ObjectSpace)os).Session,
                    rec => rec.Supplier.Code == imp_rec.SF_VO_CODE &&
                           rec.RegNumber == imp_rec.SF_INT_NUMBER &&
                           rec.Date >= sf_date &&
                           rec.Date < sf_date.AddDays(1)
                    ));
                //if (invoice == null) {
                //    count++;
                //    System.Console.WriteLine(imp_rec.SF_INT_NUMBER + " " + imp_rec.SF_NUMBER + " " + imp_rec.SF_DATE + " " + summ_cost + " " + summ_nds);
                //}
                if (invoice == null) {
                    crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", imp_rec.SF_VO_CODE)).FirstOrDefault();
                    invoice = os.CreateObject<fmCAVTInvoiceBase>();
                    invoice.RegNumber = imp_rec.SF_INT_NUMBER;
                    invoice.Number = imp_rec.SF_NUMBER;
                    invoice.Date = sf_date;
                    invoice.Supplier = party;
                    invoice.Customer = register.Party;
                    invoice.SummAVT = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                    invoice.SummCost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                }
                else {
                    invoice.SummAVT = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                    invoice.SummCost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                    fmCAVTInvoiceRegisterLine line_check = os.FindObject<fmCAVTInvoiceRegisterLine>(
                            CriteriaOperator.And(new BinaryOperator("InvoiceVersion", invoice.Current)), true);
                    if (line_check != null) continue;
                }
                fmCAVTInvoiceRegisterLine line = register.InLines.Where(rec => rec.Invoice == invoice).FirstOrDefault();
                if (line == null) {
                    line = os.CreateObject<fmCAVTInvoiceRegisterLine>();
                    register.InLines.Add(line);
                }
                line.SequenceNumber = seq_num++;
                //                line_doc.DateTransfer = invoice.Date;
                line.Invoice = invoice;
                line.TransferType = inv_transfer_types.Where(rec => rec.Code == "1").FirstOrDefault();
                if (String.IsNullOrEmpty(imp_rec.SF_TRANS_DATE.Trim())) {
                    DateTime trans_date = default(DateTime);
                    DateTime.TryParseExact(imp_rec.SF_TRANS_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out trans_date );
                    line.DateTransfer = trans_date;
                }
                if (line.DateTransfer < sf_date)
                    line.DateTransfer = sf_date;
                line.OperationType = inv_oper_types.Where(rec => rec.Code == imp_rec.SF_OPER_TYPE.Trim() ||
                                                               rec.Code == '0' + imp_rec.SF_OPER_TYPE.Trim()).FirstOrDefault();
            }
            System.Console.WriteLine("All " + count);
        }
        static public void ImportAvansData(IObjectSpace os, String file_name) {
            fmCAVTInvoiceType inv_type = os.GetObjects<fmCAVTInvoiceType>().FirstOrDefault(x => x.Prefix == "ю");
            FileHelperEngine<fmCAVTInvoiceAvansImport> engine = new FileHelperEngine<fmCAVTInvoiceAvansImport>();
            engine.Options.IgnoreFirstLines = 1;
            fmCAVTInvoiceAvansImport[] avans = engine.ReadFile(file_name);
            foreach (fmCAVTInvoiceAvansImport avan in avans) {
                avan.VO_CODE = avan.VO_CODE.Trim();
                avan.SF_NUMBER = avan.SF_NUMBER.Trim();
                avan.PP_NUMBER = avan.PP_NUMBER.Trim();
                avan.TEXT = avan.TEXT.Trim();
                fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
                    XPQuery<fmCAVTInvoiceBase>.TransformExpression(
                    ((ObjectSpace)os).Session,
                    rec => rec.Customer.Code == avan.VO_CODE &&
                            rec.Number == avan.SF_NUMBER &&
                            rec.Date >= avan.SF_DATE.Date &&
                            rec.Date <= avan.SF_DATE.Date.AddDays(1)
                    ));
                fmCAVTInvoiceLine line = null;
                if (invoice == null) {
                    invoice = os.CreateObject<fmCAVTInvoiceBase>();
                    invoice.Number = avan.SF_NUMBER;
                    invoice.Date = avan.SF_DATE.Date;
                    invoice.Customer = os.GetObjects<crmCParty>(new BinaryOperator("Code", avan.VO_CODE)).FirstOrDefault();
                    invoice.Supplier = os.GetObjects<crmCParty>(new BinaryOperator("Code", "2518")).FirstOrDefault();
                }
                if (invoice != null) {
                    invoice.InvoiceType = inv_type;
                    if (invoice.Lines.Count > 0)
                        line = invoice.Lines[0];
                    if (line == null) {
                        line = os.CreateObject<fmCAVTInvoiceLine>();
                        invoice.Lines.Add(line);
                    }
                    line.NomenclatureText = avan.TEXT;
                    line.AVTSumm = avan.SUMM_VAT;
                    line.Cost = avan.SUMM_ALL - avan.SUMM_VAT;
                    //                    fmCAVTInvoiceVersion.Payment pay = new fmCAVTInvoiceVersion.Payment() {
                    fmCAVTInvoiceVersion.fmCAVTInvoicePayment pay = null;
                    if (invoice.PaymentsList.Count > 0)
                        pay = invoice.PaymentsList[0];
                    if (pay == null) {
                        pay = os.CreateObject<fmCAVTInvoiceVersion.fmCAVTInvoicePayment>();
                        invoice.PaymentsList.Add(pay);
                    }
                    pay.Number = avan.PP_NUMBER;
                    pay.Date = avan.PP_DATE;
                }

            }
        }
        static public void ImportBuhData(IObjectSpace os, fmCAVTInvoiceRegister register) {
            UInt32 seq_num = 1;
            IList<fmCAVTInvoiceType> inv_types = os.GetObjects<fmCAVTInvoiceType>();
            IList<fmCAVTInvoiceTransferType> inv_transfer_types = os.GetObjects<fmCAVTInvoiceTransferType>();
            IList<fmCAVTInvoiceOperationType> inv_oper_types = os.GetObjects<fmCAVTInvoiceOperationType>();
            foreach (var invrec in register.BookBuhImport.BookBuhRecords.
                    Where(rec => rec.BookType == "P" && (rec.RecordType == "PAY" || rec.RecordType == "AIN" || rec.RecordType == "EAT") ||
                                 rec.BookType == "B" && (rec.RecordType == "PAY")).
                    GroupBy(rec => new { rec.AVTInvoicePartyCode, rec.AVTInvoiceType, rec.AVTInvoiceNumber, rec.AVTInvoiceDate }).
                    OrderBy(rec => rec.Key.AVTInvoiceDate.ToString("yyyyMMdd") + rec.Key.AVTInvoiceNumber)) {
                if (String.IsNullOrEmpty(invrec.Key.AVTInvoiceNumber.Trim()) || String.IsNullOrEmpty(invrec.Key.AVTInvoicePartyCode.Trim())) continue;
                if (invrec.Key.AVTInvoiceType != "явт" && invrec.Key.AVTInvoiceType != "ятю" && invrec.Key.AVTInvoiceType != "ятб") continue;
                crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", invrec.Key.AVTInvoicePartyCode)).FirstOrDefault();

                fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("Number", invrec.Key.AVTInvoiceNumber),
                                             new BinaryOperator("Date", invrec.Key.AVTInvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", invrec.Key.AVTInvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", register.Party)), true
                    //                                             , new BinaryOperator("Customer", party))
                                             );
                if (invoice == null && invrec.Key.AVTInvoiceType == "явт") {
                    invoice = os.CreateObject<fmCAVTInvoiceBase>();
                    invoice.Number = invrec.Key.AVTInvoiceNumber;
                    invoice.Date = invrec.Key.AVTInvoiceDate;
                    invoice.Supplier = register.Party;
                    invoice.Customer = party;
                }
                if (invoice != null) {
                    if (invoice.InvoiceType == null && invoice.Number != null && invoice.Number.Length > 0) {
                        foreach (fmCAVTInvoiceType inv_type in inv_types) {
                            if (inv_type.InvoiceDirection == fmAVTInvoiceDirection.AVTInvoiceOut &&
                                inv_type.Prefix == invoice.Number.Substring(0, 1))
                                invoice.InvoiceType = inv_type;
                        }
                    }
                    fmCAVTInvoiceRegisterLine line_check = os.FindObject<fmCAVTInvoiceRegisterLine>(
                        CriteriaOperator.And(new BinaryOperator("InvoiceVersion", invoice.Current)), true);
                    if (line_check != null) continue;
//                    if (invrec.Key.AVTInvoiceType != "ятб") {
//                        foreach (fmCAVTBookBuhRecord rec in invrec) {
//                            invoice.SummAVT = invoice.SummAVT + rec.SummVAT;
//                            invoice.SummCost = invoice.SummCost + rec.SummAll - rec.SummVAT;
//                        }
//                    }
                    if (invoice.Customer == null) {
                        invoice.Customer = party;
                    }
                    if (invrec.Key.AVTInvoiceNumber.StartsWith("5")) {
                        invoice.Customer = invoice.Customer;
                    }
                    if (invoice.SummAVT == 0 && invoice.SummCost == 0) {
                        foreach (fmCAVTBookBuhRecord rec in invrec) {
                            decimal summ_vat = rec.SummVAT + rec.SummVATCost + rec.SummVATCost + rec.SummVATExp;
                            invoice.SummAVT = invoice.SummAVT + summ_vat;
                            if (rec.NDSRate == "2")
                                invoice.SummCost = invoice.SummCost + Decimal.Round(summ_vat * 100m / 18m, 2);
                            else if (rec.NDSRate == "3")
                                invoice.SummCost = invoice.SummCost + Decimal.Round(summ_vat * 100m / 10m, 2);
                        }
                    }
                    fmCAVTInvoiceRegisterLine line = register.OutLines.Where(rec => rec.Invoice == invoice).FirstOrDefault();
                    if (line == null) {
                        line = os.CreateObject<fmCAVTInvoiceRegisterLine>();
                        register.OutLines.Add(line);
                    }
                    line.SequenceNumber = seq_num++;
                    line.DateTransfer = invoice.Date;
                    line.Invoice = invoice;
                    line.TransferType = inv_transfer_types.Where(rec => rec.Code == "1").FirstOrDefault();
                    if (invrec.Key.AVTInvoiceType == "явт")
                        line.OperationType = inv_oper_types.Where(rec => rec.Code == "01").FirstOrDefault();
                    if (invrec.Key.AVTInvoiceType == "ятю")
                        line.OperationType = inv_oper_types.Where(rec => rec.Code == "02").FirstOrDefault();
                    if (invrec.Key.AVTInvoiceType == "ятб") {
                        if (invrec.Where(rec => rec.RecordType == "SMN").Count() != 0)
                            line.OperationType = inv_oper_types.Where(rec => rec.Code == "08").FirstOrDefault();
                        if (invrec.Where(rec => rec.RecordType == "EAT").Count() != 0)
                            line.OperationType = inv_oper_types.Where(rec => rec.Code == "07").FirstOrDefault();
                    }
                }
            }
            seq_num = 1;
            foreach (var invrec in register.BookBuhImport.BookBuhRecords.
                    Where(rec => rec.BookType == "B" && (rec.RecordType == "BAY" || rec.RecordType == "AON" ) ||
                                 rec.BookType == "P" && (rec.RecordType == "BAY")).
                    GroupBy(rec => new { rec.AVTInvoicePartyCode, rec.AVTInvoiceType, rec.AVTInvoiceRegNumber, rec.AVTInvoiceNumber, rec.AVTInvoiceDate }).
                    OrderBy(rec => rec.Key.AVTInvoiceDate.ToString("yyyyMMdd") + rec.Key.AVTInvoiceRegNumber)) {
                if (String.IsNullOrEmpty(invrec.Key.AVTInvoiceNumber.Trim()) || String.IsNullOrEmpty(invrec.Key.AVTInvoicePartyCode.Trim())) continue;
                if (invrec.Key.AVTInvoiceType != "явт" && invrec.Key.AVTInvoiceType != "ятб") continue;
                crmCParty party = os.FindObject<crmCParty>(new BinaryOperator("Code", invrec.Key.AVTInvoicePartyCode));
                fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
                        CriteriaOperator.And(new BinaryOperator("Number", invrec.Key.AVTInvoiceNumber),
                                             new BinaryOperator("Date", invrec.Key.AVTInvoiceDate.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("Date", invrec.Key.AVTInvoiceDate.Date.AddDays(1), BinaryOperatorType.Less),
                                             new BinaryOperator("Supplier", party)), true
                    //                                             , new BinaryOperator("Customer", party))
                                             );

                //
                if (invoice == null) {
                    invoice = os.CreateObject<fmCAVTInvoiceBase>();
                    invoice.RegNumber = invrec.Key.AVTInvoiceRegNumber;
                    invoice.Number = invrec.Key.AVTInvoiceNumber;
                    invoice.Date = invrec.Key.AVTInvoiceDate;
                    invoice.Supplier = party;
                    invoice.Customer = register.Party;
                }
                else {
                    fmCAVTInvoiceRegisterLine line_check = os.FindObject<fmCAVTInvoiceRegisterLine>(
                            CriteriaOperator.And(new BinaryOperator("InvoiceVersion", invoice.Current)), true);
                    if (line_check != null) continue;
                }
                Decimal SummVAT_18 = invrec.Where(buhrec => buhrec.NDSRate == "2").Sum(buhrec => buhrec.SummVATCost + buhrec.SummVATExp + buhrec.SummVAT);
                Decimal SummVAT_10 = invrec.Where(buhrec => buhrec.NDSRate == "3").Sum(buhrec => buhrec.SummVATCost + buhrec.SummVATExp + buhrec.SummVAT);
                Decimal SummNoVAT = invrec.Where(buhrec => buhrec.NDSRate == "5").Sum(buhrec => buhrec.SummAll);
//                Decimal SummCost_18 = Decimal.Round(SummVAT_18 * 100m / 18m, 2);
//                Decimal SummCost_10 = Decimal.Round(SummVAT_10 * 100m / 10m, 2);
//                SummCost_NoVAT = SummCost_NoVAT +
//                    Decimal.Round(SummNoVAT_18 * 118m / 18m, 2) +
//                    Decimal.Round(SummNoVAT_10 * 110m / 10m, 2);

                invoice.SummAVT = SummVAT_18 + SummVAT_10;
                invoice.SummCost = SummNoVAT +
                                    Decimal.Round(SummVAT_18 * 100m / 18m, 2) +
                                    Decimal.Round(SummVAT_10 * 100m / 10m, 2);

                fmCAVTInvoiceRegisterLine line = register.InLines.Where(rec => rec.Invoice == invoice).FirstOrDefault();
                if (line == null) {
                    line = os.CreateObject<fmCAVTInvoiceRegisterLine>();
                    register.InLines.Add(line);
                }
                line.SequenceNumber = seq_num++;
                //                line_doc.DateTransfer = invoice.Date;
                line.Invoice = invoice;
                line.TransferType = inv_transfer_types.Where(rec => rec.Code == "1").FirstOrDefault();
                line.DateTransfer = invrec.Select(rec => rec.BuhDocDate).Min();
                if (line.DateTransfer < DateTime.ParseExact("20130101", "yyyyMMdd", null))
                    line.DateTransfer = DateTime.ParseExact("20130131", "yyyyMMdd", null);
                if (line.DateTransfer < line.Invoice.Date)
                    line.DateTransfer = line.Invoice.Date;
                if (invrec.Key.AVTInvoiceType == "явт")
                    line.OperationType = inv_oper_types.Where(rec => rec.Code == "01").FirstOrDefault();
                if (invrec.Key.AVTInvoiceType == "ятб") {
                    if (invrec.Where(rec => rec.RecordType == "AON").Count() != 0)
                        line.OperationType = inv_oper_types.Where(rec => rec.Code == "02").FirstOrDefault();
                    if (invrec.Where(rec => rec.RecordType == "SMN").Count() != 0)
                        line.OperationType = inv_oper_types.Where(rec => rec.Code == "08").FirstOrDefault();
                    if (invrec.Where(rec => rec.RecordType == "EAT").Count() != 0)
                        line.OperationType = inv_oper_types.Where(rec => rec.Code == "07").FirstOrDefault();
                }
            }
        }
        static public void RegisterLineReNumber(IObjectSpace os, IList<fmCAVTInvoiceRegisterLine> lines) {
            IList<fmCAVTInvoiceRegisterLine> sort_lines = lines.OrderBy(line => line.DateTransfer).ToList();
            uint number = 0;
            foreach (fmCAVTInvoiceRegisterLine line in sort_lines) {
                number++;
                line.SequenceNumber = number;
                if (line.Invoice.Valuta == null) {
                    line.Invoice.Valuta = os.FindObject<CS.Nomenclature.csValuta>(new BinaryOperator("Code", "RUB"));
                }
            }
        }

    }
}
