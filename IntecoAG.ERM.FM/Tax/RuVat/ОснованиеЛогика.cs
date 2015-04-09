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
using IntecoAG.ERM.FM.AVT;
//
namespace IntecoAG.ERM.FM.Tax.RuVat {
    public static class ОснованиеЛогика {
        [FixedLengthRecord]
        public class InvoiceImport {
            [FieldFixedLength(1)]
            public String SF_IO_TYPE;
            [FieldFixedLength(3)]
            public String SF_TYPE;
            [FieldFixedLength(3)]
            public String SF_TYPE_ORIG;
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
            [FieldFixedLength(250)]
            public String SF_NUMBER;
            [FieldFixedLength(8)]
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
            public String VO_CODE;
            public String SF_NUMBER;
            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public DateTime SF_DATE;
            public String PP_NUMBER;
            [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
            public DateTime PP_DATE;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal SUMM_ALL;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal SUMM_VAT;
            public String TEXT;
        }
        static public void ImportInvoices(IObjectSpace os, String file_name) {
            //            OpenFileDialog dialog = new OpenFileDialog();
            //            if (dialog.ShowDialog() == DialogResult.OK) {
            FixedFileEngine engine = new FixedFileEngine(typeof(InvoiceImport));
            InvoiceImport[] imp_res = (InvoiceImport[])engine.ReadFile(file_name);
            IList<fmCAVTInvoiceType> inv_types = os.GetObjects<fmCAVTInvoiceType>();
            IList<fmCAVTInvoiceTransferType> inv_transfer_types = os.GetObjects<fmCAVTInvoiceTransferType>();
            IList<fmCAVTInvoiceOperationType> inv_oper_types = os.GetObjects<fmCAVTInvoiceOperationType>();
            Int32 count = 0;
            fmCAVTInvoiceType sf_sfz_type = os.GetObjects<fmCAVTInvoiceType>().First(x => x.Prefix == "Z");
            foreach (InvoiceImport imp_rec in imp_res) {
                imp_rec.SF_VO_CODE = imp_rec.SF_VO_CODE.Trim();
                imp_rec.SF_INT_NUMBER = imp_rec.SF_INT_NUMBER.Trim();
                imp_rec.SF_NUMBER = imp_rec.SF_NUMBER.Trim();
                Decimal summ_cost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                Decimal summ_nds = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                Decimal summ_sub_cost = Decimal.Parse(imp_rec.SUMM_SUB_COST.Trim().Replace('.', ','));
                Decimal summ_sub_nds = Decimal.Parse(imp_rec.SUMM_SUB_NDS.Trim().Replace('.', ','));
                DateTime sf_date = default(DateTime);
                DateTime.TryParseExact(imp_rec.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sf_date);
                crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", imp_rec.SF_VO_CODE)).FirstOrDefault();
                if (party == null) {
                    System.Console.WriteLine("SF " + imp_rec.SF_NUMBER + " party not found (" + imp_rec.SF_VO_CODE + ")");
                    continue;
                }
                Основание.ТипИсточника ts;
                if (imp_rec.SF_IO_TYPE == "I")
                    ts = Основание.ТипИсточника.ВХОДЯЩИЙ;
                else if (imp_rec.SF_IO_TYPE == "O")
                    ts = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                else
                    throw new ArgumentOutOfRangeException("SF " + imp_rec.SF_NUMBER + " неопределен тип входящий/исходящий");
                Основание.ТипОснования tsf;
                switch (imp_rec.SF_TYPE) { 
                    case "СЧФ":
                        tsf = Основание.ТипОснования.СЧФ;
                        break;
                    case "УПД":
                        tsf = Основание.ТипОснования.УПД;
                        break;
                    case "СФА":
                        tsf = Основание.ТипОснования.СФА;
                        break;
                    case "СФЗ":
                        tsf = Основание.ТипОснования.СФЗ;
                        break;
                    case "СЧГ":
                        tsf = Основание.ТипОснования.СЧГ;
                        break;
                    case "БЖД":
                        tsf = Основание.ТипОснования.БЖД;
                        break;
                    case "СФВ":
                        tsf = Основание.ТипОснования.СФВ;
                        break;
                    case "БСО":
                        tsf = Основание.ТипОснования.БСО;
                        break;
                    case "ЧЕК":
                        tsf = Основание.ТипОснования.ЧЕК;
                        break;
                    default:
                        System.Console.WriteLine("SF: " + imp_rec.SF_NUMBER + " странный тип (" + imp_rec.SF_TYPE + ")");
                        continue;
                }
                String inn = "";
                String kpp = "";
                ЛицоТип party_type = ЛицоТип.НЕЗАДАН;
                if (party.Person != null) {
                    if (party.Person.Address.Country.CodeAlfa2 == "RU") {
//                        Type party.ComponentTypeComponentObject.GetType();
                        if (party.ComponentType == typeof(crmCLegalPerson) ||
                            party.ComponentType == typeof(crmCLegalPersonUnit)) {
                            party_type = ЛицоТип.ЮР_ЛИЦО;
                            inn = party.INN;
                            if (inn.Length == 9)
                                inn = "0" + inn;
                            kpp = party.KPP;
                            if (inn.Length == 8)
                                kpp = "0" + kpp;
                            if (inn.Length != 10) {
                                System.Console.WriteLine("Party: " + party.Code + " fail INN (" + inn + ")");
                                continue;
                            }
                            if (kpp.Length != 9) {
                                System.Console.WriteLine("Party: " + party.Code + " fail KPP (" + kpp + ")");
                                continue;
                            }
                        }
                        else {
                            if (party.ComponentType == typeof(crmCBusinessman)) {
                                party_type = ЛицоТип.ПРЕДПРИНИМАТЕЛЬ;
                                inn = party.INN;
                                if (inn.Length == 11)
                                    inn = "0" + inn;
                                if (inn.Length != 12) {
                                    System.Console.WriteLine("Party: " + party.Code + " fail INN (" + inn + ")");
                                    continue;
                                }
                            }
                            else
                                if (party.ComponentType == typeof(crmCPhysicalParty)) {
                                    party_type = ЛицоТип.ФИЗ_ЛИЦО;
                                }
                        }
                    }
                    else {
                        party_type = ЛицоТип.ИНО_ПАРТНЕР;
                        System.Console.WriteLine("Party: " + party.Code + " инопартнер ");
                    }
                }
                if (party.Code == "2706") {
                    party_type = ЛицоТип.РОЗНИЦА;
                }
                if (imp_rec.SF_NUMBER == "150200305" || 
                    imp_rec.SF_NUMBER == "К0200001" ||
                    imp_rec.SF_NUMBER == "150200651" ||
                    imp_rec.SF_NUMBER == "150300200" ||
                    imp_rec.SF_NUMBER == "93409/1" ||
                    imp_rec.SF_NUMBER == "23535" ||
                    imp_rec.SF_NUMBER == "23538" ||
                    imp_rec.SF_NUMBER == "ОК2010344 487755" ||
                    imp_rec.SF_NUMBER == "ЖМ2010190 469141" ||
                    imp_rec.SF_NUMBER == "26653" ||
                    imp_rec.SF_NUMBER == "К0200001" ||
                    imp_rec.SF_NUMBER == "93409/1" ||
                    imp_rec.SF_NUMBER == "140100722" ||
                    party.Code == "7630" || 
                    party.Code == "7974" || 
                    party.Code == "955" 
                    )
                    continue;
                //
                String sale_inn = "5012039795";
                if (ts == Основание.ТипИсточника.ВХОДЯЩИЙ)
                    sale_inn = inn;
                Основание sf = os.FindObject<Основание>(
                    XPQuery<Основание>.TransformExpression(
                    ((ObjectSpace)os).Session,
                    rec => 
                        rec.ИннПродавца == sale_inn &&
                        rec.Номер == imp_rec.SF_NUMBER &&
                        rec.Дата >= sf_date &&
                        rec.Дата < sf_date.AddDays(1)
                    ));
                if (sf == null) {
                    sf = os.CreateObject<Основание>();
                    sf.Источник = ts;
                    sf.ИНН = inn;
                    sf.Номер = imp_rec.SF_NUMBER;
                    sf.Дата = sf_date;
                    sf.КПП = kpp;
                }
                sf.Корректировка = Основание.ТипПодчиненности.ОСНОВНОЙ;
                sf.Источник = ts;
                sf.Тип = tsf;
                sf.ЛицоТип = party_type;
                ОснованиеДокумент sfdoc = null;
                String sfdoc_sver = imp_rec.SF_PRAV_NUMBER.Trim();
                if (String.IsNullOrEmpty(sfdoc_sver))
                    sfdoc_sver = "0";
                UInt16 sfdoc_ver = 0; 
                UInt16.TryParse(sfdoc_sver, out sfdoc_ver);
                DateTime sfdoc_date = default(DateTime);
                DateTime.TryParseExact(imp_rec.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sfdoc_date);
                foreach (ОснованиеДокумент doc in sf.Документы) {
                    if (doc.НомерИсправления == sfdoc_ver) {
                        sfdoc = doc;
                        break;
                    }
                }
                if (sfdoc == null) {
                    sfdoc = os.CreateObject<ОснованиеДокумент>();
                    sf.Документы.Add(sfdoc);
                    sfdoc.НомерИсправления = sfdoc_ver;
                    if (sf.ДействующийДокумент.НомерИсправления < sfdoc.НомерИсправления) {
                        sf.ДействующийДокумент = sfdoc;
                    }
                }
                sfdoc.ДатаИсправления = sfdoc_date;
                sfdoc.РегНомер = imp_rec.SF_REGNUM.Trim();
                if (sf.Тип == Основание.ТипОснования.СФЗ && String.IsNullOrEmpty(sfdoc.РегНомер)) {
                    Int32 IntNumber = fmCAVTInvoiceNumberGenerator.GenerateNumber(((ObjectSpace)os).Session, sf.ДействующийДокумент.CID, sf_sfz_type, sf.Дата, 0);
                    sfdoc.РегНомер = sf_sfz_type.Prefix + sf.Дата.ToString("yyyyMM").Substring(2, 4) + IntNumber.ToString("00000");
                }
                sfdoc.КодПартнера = party.Code;
                sfdoc.НаименКонтрагента = party.Name;
                sfdoc.СуммаВсего = summ_cost + summ_nds;
                sfdoc.СуммаНДС = summ_nds;
                sfdoc.СуммаВсегоУвел = sfdoc.СуммаВсего + summ_sub_cost;
                sfdoc.СуммаНДСУвел = sfdoc.СуммаНДС + summ_sub_nds;
                //fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
                //    XPQuery<fmCAVTInvoiceBase>.TransformExpression(
                //    ((ObjectSpace)os).Session,
                //    rec => rec.Supplier.Code == imp_rec.SF_VO_CODE &&
                //           rec.RegNumber == imp_rec.SF_INT_NUMBER &&
                //           rec.Date >= sf_date &&
                //           rec.Date < sf_date.AddDays(1)
                //    ));
                ////if (invoice == null) {
                ////    count++;
                ////    System.Console.WriteLine(imp_rec.SF_INT_NUMBER + " " + imp_rec.SF_NUMBER + " " + imp_rec.SF_DATE + " " + summ_cost + " " + summ_nds);
                ////}
                //if (invoice == null) {
                //    crmCParty party = os.GetObjects<crmCParty>(new BinaryOperator("Code", imp_rec.SF_VO_CODE)).FirstOrDefault();
                //    invoice = os.CreateObject<fmCAVTInvoiceBase>();
                //    invoice.RegNumber = imp_rec.SF_INT_NUMBER;
                //    invoice.Number = imp_rec.SF_NUMBER;
                //    invoice.Date = sf_date;
                //    invoice.Supplier = party;
                //    invoice.Customer = register.Party;
                //    invoice.SummAVT = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                //    invoice.SummCost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                //}
                //else {
                //    invoice.SummAVT = Decimal.Parse(imp_rec.SUMM_NDS.Trim().Replace('.', ','));
                //    invoice.SummCost = Decimal.Parse(imp_rec.SUMM_COST.Trim().Replace('.', ','));
                //    fmCAVTInvoiceRegisterLine line_check = os.FindObject<fmCAVTInvoiceRegisterLine>(
                //            CriteriaOperator.And(new BinaryOperator("InvoiceVersion", invoice.Current)), true);
                //    if (line_check != null) continue;
                //}
                //fmCAVTInvoiceRegisterLine line = register.InLines.Where(rec => rec.Invoice == invoice).FirstOrDefault();
                //if (line == null) {
                //    line = os.CreateObject<fmCAVTInvoiceRegisterLine>();
                //    register.InLines.Add(line);
                //}
                //line.SequenceNumber = seq_num++;
                ////                line_doc.DateTransfer = invoice.Date;
                //line.Invoice = invoice;
                //line.TransferType = inv_transfer_types.Where(rec => rec.Code == "1").FirstOrDefault();
                //if (String.IsNullOrEmpty(imp_rec.SF_TRANS_DATE.Trim())) {
                //    DateTime trans_date = default(DateTime);
                //    DateTime.TryParseExact(imp_rec.SF_TRANS_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out trans_date);
                //    line.DateTransfer = trans_date;
                //}
                //if (line.DateTransfer < sf_date)
                //    line.DateTransfer = sf_date;
                //line.OperationType = inv_oper_types.Where(rec => rec.Code == imp_rec.SF_OPER_TYPE.Trim() ||
                //                                               rec.Code == '0' + imp_rec.SF_OPER_TYPE.Trim()).FirstOrDefault();
            }
            System.Console.WriteLine("All " + count);
        }

        //static public void ImportAvansData(IObjectSpace os, String file_name) {
        //    fmCAVTInvoiceType inv_type = os.GetObjects<fmCAVTInvoiceType>().FirstOrDefault(x => x.Prefix == "А");
        //    FileHelperEngine<fmCAVTInvoiceAvansImport> engine = new FileHelperEngine<fmCAVTInvoiceAvansImport>();
        //    engine.Options.IgnoreFirstLines = 1;
        //    fmCAVTInvoiceAvansImport[] avans = engine.ReadFile(file_name);
        //    foreach (fmCAVTInvoiceAvansImport avan in avans) {
        //        avan.VO_CODE = avan.VO_CODE.Trim();
        //        avan.SF_NUMBER = avan.SF_NUMBER.Trim();
        //        avan.PP_NUMBER = avan.PP_NUMBER.Trim();
        //        avan.TEXT = avan.TEXT.Trim();
        //        fmCAVTInvoiceBase invoice = os.FindObject<fmCAVTInvoiceBase>(
        //            XPQuery<fmCAVTInvoiceBase>.TransformExpression(
        //            ((ObjectSpace)os).Session,
        //            rec => rec.Customer.Code == avan.VO_CODE &&
        //                    rec.Number == avan.SF_NUMBER &&
        //                    rec.Date >= avan.SF_DATE.Date &&
        //                    rec.Date <= avan.SF_DATE.Date.AddDays(1)
        //            ));
        //        fmCAVTInvoiceLine line = null;
        //        if (invoice == null) {
        //            invoice = os.CreateObject<fmCAVTInvoiceBase>();
        //            invoice.Number = avan.SF_NUMBER;
        //            invoice.Date = avan.SF_DATE.Date;
        //            invoice.Customer = os.GetObjects<crmCParty>(new BinaryOperator("Code", avan.VO_CODE)).FirstOrDefault();
        //            invoice.Supplier = os.GetObjects<crmCParty>(new BinaryOperator("Code", "2518")).FirstOrDefault();
        //        }
        //        if (invoice != null) {
        //            invoice.InvoiceType = inv_type;
        //            if (invoice.Lines.Count > 0)
        //                line = invoice.Lines[0];
        //            if (line == null) {
        //                line = os.CreateObject<fmCAVTInvoiceLine>();
        //                invoice.Lines.Add(line);
        //            }
        //            line.NomenclatureText = avan.TEXT;
        //            line.AVTSumm = avan.SUMM_VAT;
        //            line.Cost = avan.SUMM_ALL - avan.SUMM_VAT;
        //            //                    fmCAVTInvoiceVersion.Payment pay = new fmCAVTInvoiceVersion.Payment() {
        //            fmCAVTInvoiceVersion.fmCAVTInvoicePayment pay = null;
        //            if (invoice.PaymentsList.Count > 0)
        //                pay = invoice.PaymentsList[0];
        //            if (pay == null) {
        //                pay = os.CreateObject<fmCAVTInvoiceVersion.fmCAVTInvoicePayment>();
        //                invoice.PaymentsList.Add(pay);
        //            }
        //            pay.Number = avan.PP_NUMBER;
        //            pay.Date = avan.PP_DATE;
        //        }

        //    }
        //}
    }
}
