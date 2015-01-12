using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Import;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    public class FmFinPlanSubjectDocXMLLoader : Excel2003XmlReader {

        protected IObjectSpace ObjectSpace;
        protected FmFinPlanSubjectDocFull TargetDoc;

        public FmFinPlanSubjectDocXMLLoader(IObjectSpace os, FmFinPlanSubjectDocFull doc, Stream stream)
            : base(stream) {
            ObjectSpace = os;
            TargetDoc = doc;
            NormLines();
            FormatProvider = CultureInfo.InvariantCulture;
            Valutas = ObjectSpace.GetObjects<csValuta>();
            VatRates = ObjectSpace.GetObjects<csNDSRate>();
        }

        protected override void ProcessCell(String sheet_name, Int32 row, Int32 column, String type, String value) {
            //            System.Console.WriteLine("{0}({1},{2}) {3}:'{4}'", sheet, row, column, type, value);
            //            if (sheet_name != "БСР")
            if (sheet_name == "БСР" && row == 2 && column == 12) {
                fmCOrder order = ObjectSpace.FindObject<fmCOrder>(
                    new BinaryOperator("Code", value));
                if (order == null || order.Subject != TargetDoc.Subject)
                    throw new InvalidDataException("Заказ неверный или тема не соответствует заказу");
                TargetDoc.Order = order;
                return;
            }
            if (sheet_name == "БСР" && row == 2 && column == 16) {
                TargetDoc.Valuta = Valutas.FirstOrDefault(x => x.Code == ValutaCodeConvert(value));
                foreach (var line in TargetDoc.SubLines) {
                    line.Valuta = TargetDoc.Valuta;
                }
            }
            if (sheet_name == "БСР" && row == 4 && column == 9) {
                BeginYear = (Int16)(Int32.Parse(value) - 1);
            }
            if (sheet_name == "БСР" && row == 4 && column == 16) {
                TargetDoc.VatRate = VatRates.FirstOrDefault(x => x.Code == VatCodeConvert(value));
                foreach (var line in TargetDoc.SubLines) {
                    line.VatRate = TargetDoc.VatRate;
                }
            }
            foreach (var line in TargetDoc.SubLines) {
                if (line.LineName == sheet_name) {
                    LoadTableCell(line, row, column, type, value);
                    break;
                }
            }
        }


        protected IDictionary<FmFinPlanSheetType, IList<FmFinPlanDocLine>> DocNormLines = new Dictionary<FmFinPlanSheetType, IList<FmFinPlanDocLine>>();

        protected void NormLines() {
            foreach (var line in TargetDoc.SubLines) {
                DocNormLines[line.Sheet] = NormLines(line);
            }
        }
        protected IList<FmFinPlanDocLine> NormLines(FmFinPlanDocLine line) {
            IList<FmFinPlanDocLine> result = new List<FmFinPlanDocLine>();
            NormLines(line, result);
            return result;
        }
        protected void NormLines(FmFinPlanDocLine line, IList<FmFinPlanDocLine> norm_list) {
            norm_list.Add(line);
            foreach (var sub_line in line.SubLines)
                NormLines(sub_line, norm_list);
        }

        protected void LoadTableCell(FmFinPlanDocLine top_line, Int32 row, Int32 column, String type, String value) {
            // System.Console.WriteLine("{0}({1},{2}) {3}:'{4}'", table.LineCode, row, column, type, value);
            IList<FmFinPlanDocLine> norm_list = DocNormLines[top_line.Sheet];
            switch (top_line.Sheet) {
                case FmFinPlanSheetType.FMFPS_COST:
                    if (9 < row && row < 55 && 2 < column)
                        LoadTimeCell(norm_list[row - 9], 3, column, type, value);
                    break;
                case FmFinPlanSheetType.FMFPS_CASH:
                    if (9 < row && row < 39 && 2 < column)
                        LoadTimeCell(norm_list[row - 9], 3, column, type, value);
                    break;
                case FmFinPlanSheetType.FMFPS_PARTY:
                    if (row == 10)
                        LoadTimeCell(top_line.SubLines[0].SubLines[0], 6, column, type, value);
                    if (row == 11)
                        LoadTimeCell(top_line.SubLines[0].SubLines[1], 6, column, type, value);
                    if (row == 12)
                        LoadTimeCell(top_line.SubLines[0].SubLines[1].SubLines[0], 6, column, type, value);
                    if (row == 13)
                        LoadTimeCell(top_line.SubLines[0].SubLines[1].SubLines[1], 6, column, type, value);
                    if (13 < row && row < 814)
                        LoadPartyCell(top_line, row, column, type, value);
                    break;
                case FmFinPlanSheetType.FMFPS_MATERIAL:
                    if (9 < row && row < 31 && 2 < column)
                        LoadTimeCell(norm_list[row - 9], 4, column, type, value);
                    break;
                case FmFinPlanSheetType.FMFPS_NORMATIV:
                    if (9 < row && row < 22 && 2 < column)
                        LoadTimeCell(norm_list[row - 9], 3, column, type, value);
                    break;
                default:
                    break;
            }
        }

        protected FmFinPlanDocLine[] PartyLines = new FmFinPlanDocLine[100];

        protected String PartyCode;

        protected IList<csValuta> Valutas;
        protected IList<csNDSRate> VatRates;

        protected String VatCodeConvert(String code) {
            switch (code) {
                case "Без НДС":
                    return "БЕЗ НДС";
                case "18%":
                    return "18%";
                case "0% Экспорт":
                    return "0%";
                default:
                    return null;
            }
        }
        protected String ValutaCodeConvert(String code) {
            switch (code) {
                case "РУБ":
                    return "RUB";
                case "ДОЛ":
                    return "USD";
                case "ЕВР":
                    return "EUR";
                default:
                    return null;
            }
        }

        protected void LoadPartyCell(FmFinPlanDocLine top_line, Int32 row, Int32 column, String type, String value) {
            Int32 line_num = (row - 14) / 8;
            Int32 line_type = row - 14 - line_num * 8;
            if (line_type == 0) {
                if (column == 1)
                    PartyCode = value;
                if (column == 3) {
                    if (!String.IsNullOrEmpty(value)) {
                        PartyLines[line_num] = new FmFinPlanDocLine(((ObjectSpace)ObjectSpace).Session, FmFinPlanLineType.FMFPL_PARTY_PARTY, top_line, FmFinPlanTotalType.FMFPT_HIERARCHICAL,
                            PartyCode, value, HrmStructItemType.HRM_STRUCT_UNKNOW);
                        PartyLines[line_num].PartyName = value;
                    }
                }
            }
            if (PartyLines[line_num] != null) {
                if (column == 4) {
                    if (line_type == 0) {
                        PartyLines[line_num].DealNumber = value;
                    }
                    if (line_type == 1) {
                        PartyLines[line_num].DealAddNumber = value;
                    }
                    if (line_type == 2) {
                        PartyLines[line_num].Valuta = Valutas.FirstOrDefault(x => x.Code == ValutaCodeConvert(value));
                    }
                    if (line_type == 3) {
                        PartyLines[line_num].VatRate = VatRates.FirstOrDefault(x => x.Code == VatCodeConvert(value));
                    }
                    if (line_type == 4) {
                    }
                    if (line_type == 5) {
                    }
                    if (line_type == 6) {
                    }
                    if (line_type == 7) {
                    }
                }
                if (column > 5) {
                    FmFinPlanDocLine line_time = null;
                    if (line_type == 0)
                        line_time = PartyLines[line_num].SubLines[0];
                    if (line_type == 1)
                        line_time = PartyLines[line_num].SubLines[1];
                    if (line_type == 2)
                        line_time = PartyLines[line_num].SubLines[1].SubLines[0];
                    if (line_type == 3)
                        line_time = PartyLines[line_num].SubLines[1].SubLines[1];
                    if (line_type == 4)
                        line_time = PartyLines[line_num].SubLines[2];
                    if (line_type == 5)
                        line_time = PartyLines[line_num].SubLines[3];
                    if (line_type == 6)
                        line_time = PartyLines[line_num].SubLines[3].SubLines[0];
                    if (line_type == 7)
                        line_time = PartyLines[line_num].SubLines[3].SubLines[1];
                    LoadTimeCell(line_time, 6, column, type, value);
                }
            }
        }

        private Int16 BeginYear = 0;
        private IFormatProvider FormatProvider;

        protected void LoadTimeCell(FmFinPlanDocLine line_doc, Int32 start_column, Int32 column, String type, String value) {
            Int16 year;
            Int16 quarter;
            Int16 month;
            Decimal value_decimal = 0;
            if (type != "Number")
                return;
            try {
                Double dval = Double.Parse(value, FormatProvider);
                value_decimal = Decimal.Round(new Decimal(dval), 3);
            }
            catch (FormatException e) {
                throw new FormatException(String.Format("Лист: {0}, Колонка: {1}/{2}, Строка: {3}, Значение {4}", line_doc.Sheet, start_column, column, line_doc.LineCode, value), e);
            }
            if (value_decimal == 0)
                return;

            if (start_column <= column && column <= start_column + 18) {
                if (column < start_column + 6) {
                    year = BeginYear;
                    month = (Int16)(column - start_column + 8);
                }
                else {
                    year = (Int16)(BeginYear + 1);
                    month = (Int16)(column - start_column - 5);
                }
                quarter = (Int16)((month - 1) / 3 + 1);
                if (month == 13) {
                    quarter = 0;
                    month = 0;
                }
            }
            else if (start_column + 19 <= column && column <= start_column + 63) {
                year = (Int16)((column - start_column - 19) / 5 + BeginYear + 2);
                quarter = (Int16)(column - start_column - 18 - (year - BeginYear - 2) * 5);
                if (quarter == 5)
                    quarter = 0;
                month = 0;
            }
            else if (start_column + 64 == column) {
                year = 0;
                quarter = 0;
                month = 0;
            }
            else
                return;
            //            System.Console.WriteLine(line_doc.LineCode + " {5} {0}/{1}/{2} {3}:'{4}'", year, quarter, month, type, value, column);

            FmFinPlanDocTime line_top = line_doc.LineTime;
            FmFinPlanDocTime line_year = null;
            FmFinPlanDocTime line_quarter = null;
            FmFinPlanDocTime line_month = null;
            FmFinPlanDocTime line_target = null;
            if (year != 0) {
                line_year = line_top.SubTimes.FirstOrDefault(x => x.Year == year);
                if (line_year == null) {
                    line_year = ObjectSpace.CreateObject<FmFinPlanDocTime>();
                    line_top.SubTimes.Add(line_year);
                    line_year.TimeTypeSet(FmFinPlanTimeType.FMFPT_YEAR);
                    line_year.YearSet(year);
                }
                if (quarter != 0) {
                    line_quarter = line_year.SubTimes.FirstOrDefault(x => x.Quarter == quarter);
                    if (line_quarter == null) {
                        line_quarter = ObjectSpace.CreateObject<FmFinPlanDocTime>();
                        line_year.SubTimes.Add(line_quarter);
                        line_quarter.TimeTypeSet(FmFinPlanTimeType.FMFPT_QUARTER);
                        line_quarter.YearSet(year);
                        line_quarter.QuarterSet(quarter);
                    }
                    if (month != 0) {
                        line_month = line_quarter.SubTimes.FirstOrDefault(x => x.Month == month);
                        if (line_month == null) {
                            line_month = ObjectSpace.CreateObject<FmFinPlanDocTime>();
                            line_quarter.SubTimes.Add(line_month);
                            line_month.TimeTypeSet(FmFinPlanTimeType.FMFPT_MONTH);
                            line_month.YearSet(year);
                            line_month.QuarterSet(quarter);
                            line_month.MonthSet(month);
                        }
                        line_target = line_month;
                    }
                    else {
                        line_target = line_quarter;
                    }
                }
                else {
                    line_target = line_year;
                }
            }
            else {
                line_target = line_top;
            }
            line_target.ValueManual = value_decimal;
        }
    }

    public class FmFinPlanSubjectDocTransactLocal {
        protected IObjectSpace ObjectSpace;
        protected FmFinPlanSubjectDocFull Document;
        private IList<fmCostItem> _CostItems;
        protected IList<fmCostItem> CostItems {
            get {
                return _CostItems;
            }
        }
        public FmFinPlanSubjectDocTransactLocal(IObjectSpace os, FmFinPlanSubjectDocFull doc) {
            ObjectSpace = os;
            _CostItems = os.GetObjects<fmCostItem>();
            Document = doc;
        }

        public void TransactLocal() {
            OperationFill();
        }
        protected void OperationFill() {
            OperationFill(Document.SubLines);
        }
        protected void OperationFill(XPCollection<FmFinPlanDocLine> lines) {
            foreach (var line in lines) {
                OperationFill(line);
                OperationFill(line.SubLines);
            }
        }

        protected void OperationFill(FmFinPlanDocLine line) {
            OperationFill(line, line.SubTimes);
        }

        protected void OperationFill(FmFinPlanDocLine line, XPCollection<FmFinPlanDocTime> times) {
            foreach (var time in times) {
                OperationFill(line, time);
                OperationFill(line, time.SubTimes);
            }
        }
        protected void OperationFill(FmFinPlanDocLine line, FmFinPlanDocTime time) {
            if (time.TimeType == FmFinPlanTimeType.FMFPT_TOTAL ||
                time.SubTimes.Count != 0 ||
                time.ValueManual == 0)
                return;
            //            FmJournalOperation oper = os.CreateObject<FmJournalOperation>();
            //            doc.DocOperations.Add(oper);
            switch (line.Sheet) {
                case FmFinPlanSheetType.FMFPS_COST:
                    MakeLineCostOperations(line, time);
                    break;
                case FmFinPlanSheetType.FMFPS_CASH:
                    break;
                case FmFinPlanSheetType.FMFPS_PARTY:
                    MakeLinePartyOperations(line, time);
                    break;
                case FmFinPlanSheetType.FMFPS_NORMATIV:
                    break;
                default:
                    break;
            }
        }

        protected void OperationFillDate(FmJournalOperation oper, FmFinPlanDocTime time) {
            switch (time.TimeType) {
                case FmFinPlanTimeType.FMFPT_YEAR:
                    oper.Date = new DateTime(time.Year, 12, 31);
                    break;
                case FmFinPlanTimeType.FMFPT_QUARTER:
                    oper.Date = new DateTime(time.Year, time.Quarter * 3, 1).AddMonths(1).AddDays(-1);
                    break;
                case FmFinPlanTimeType.FMFPT_MONTH:
                    oper.Date = new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                default:
                    oper.Date = new DateTime(1899, 12, 31);
                    break;
            }
        }

        protected FmJournalOperation MakeOperation(FmFinPlanDocLine line, FmFinPlanDocTime time) {
            FmJournalOperation oper = ObjectSpace.CreateObject<FmJournalOperation>();
            Document.DocOperations.Add(oper);
            OperationFillDate(oper, time);
            return oper;
        }

        protected void MakeLineCostOperations(FmFinPlanDocLine line, FmFinPlanDocTime time) {
            //            FmJournalOperation oper = null;
            switch (line.LineType) {
                case FmFinPlanLineType.FMFPL_COST_SALE:
                    break;
                //                    oper = MakeOperation(os, doc, line, time);
                //                    oper.DepartmentStructItem =  
                //                    break;
            }

        }

        protected void MakeLinePartyOperations(FmFinPlanDocLine line, FmFinPlanDocTime time) {
            FmJournalOperation oper = null;
            switch (line.LineType) {
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_RUB_COST:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_O_PAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.CREDIT;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.BalanceSumma = time.ValueManual;
                    oper.BalanceValuta = line.TopLine.Valuta;
                    oper.Party = line.TopLine.Party;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_RUB_PAY_POST:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_O_PAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.DEBET;
                    oper.FinAccountBalanceType = FinOperationType.CREDIT;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.BalanceSumma = time.ValueManual;
                    oper.BalanceValuta = line.TopLine.TopLine.Valuta;
                    oper.Party = line.TopLine.TopLine.Party;
                    //
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_CASH;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.BalanceSumma = time.ValueManual;
                    oper.BalanceValuta = line.TopLine.TopLine.Valuta;
                    oper.PayType = PaymentRequest.fmPRPayType.POSTPAYMENT;
                    oper.Party = line.TopLine.TopLine.Party;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_RUB_PAY_PRE:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_PREPAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.DEBET;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.BalanceSumma = time.ValueManual;
                    oper.BalanceValuta = line.TopLine.TopLine.Valuta;
                    oper.Party = line.TopLine.TopLine.Party;
                    //
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_CASH;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.BalanceSumma = time.ValueManual;
                    oper.BalanceValuta = line.TopLine.TopLine.Valuta;
                    oper.PayType = PaymentRequest.fmPRPayType.PREPAYMENT;
                    oper.Party = line.TopLine.TopLine.Party;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_VAL_COST:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_O_PAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.CREDIT;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.ObligationSumma = time.ValueManual;
                    oper.ObligationValuta = line.TopLine.Valuta;
                    oper.Party = line.TopLine.Party;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_VAL_PAY_POST:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_O_PAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.DEBET;
                    oper.FinAccountBalanceType = FinOperationType.CREDIT;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.ObligationValuta = line.TopLine.TopLine.Valuta;
                    oper.ObligationSumma = time.ValueManual;
                    oper.Party = line.TopLine.TopLine.Party;
                    //
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_CASH;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.ObligationValuta = line.TopLine.TopLine.Valuta;
                    oper.ObligationSumma = time.ValueManual;
                    oper.PayType = PaymentRequest.fmPRPayType.POSTPAYMENT;
                    oper.Party = line.TopLine.TopLine.Party;
                    break;
                case FmFinPlanLineType.FMFPL_PARTY_PARTY_VAL_PAY_PRE:
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_PREPAY_SUPPLIER;
                    oper.FinOperationType = FinOperationType.DEBET;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.ObligationValuta = line.TopLine.TopLine.Valuta;
                    oper.ObligationSumma = time.ValueManual;
                    oper.Party = line.TopLine.TopLine.Party;
                    //
                    oper = MakeOperation(line, time);
                    oper.FinAccountType = FinAccountType.ACC_A_CASH;
                    oper.FinOperationType = FinOperationType.CREDIT;
                    oper.FinAccountBalanceType = FinOperationType.DEBET;
                    oper.CostItem = CostItems.FirstOrDefault(x => x.Code == "7001");
                    oper.ObligationValuta = line.TopLine.TopLine.Valuta;
                    oper.ObligationSumma = time.ValueManual;
                    oper.PayType = PaymentRequest.fmPRPayType.PREPAYMENT;
                    oper.Party = line.TopLine.TopLine.Party;
                    break;
            }

        }
    }

    public static class FmFinPlanSubjectDocFullLogic {

        public static void ReLoadDocFromXML(IObjectSpace os, FmFinPlanSubjectDocFull doc, Stream stream) {
            LoadDocFromXML(os, doc, stream);
            TransactLocal(os, doc);
        }

        public static void ReMakeOperations(IObjectSpace os, FmFinPlanSubjectDocFull doc) {
            TransactLocal(os, doc);
        }

        public static void LoadDocFromXML(IObjectSpace os, FmFinPlanSubjectDocFull doc, Stream stream) {
            FmFinPlanSubjectDocXMLLoader loader = new FmFinPlanSubjectDocXMLLoader(os, doc, stream);
            doc.LinesClean();
            loader.Load();
        }

        public static void TransactLocal(IObjectSpace os, FmFinPlanSubjectDocFull doc) {
            FmFinPlanSubjectDocTransactLocal transact = new FmFinPlanSubjectDocTransactLocal(os, doc);
            doc.DocOperationsClean();
            transact.TransactLocal();
        }

        public static void TransactToSubject(IObjectSpace os, FmFinPlanSubjectDocFull doc) {
            FmFinPlanSubjectLogic.TransactPlan0(os, doc.FinPlanSubject, doc);
//            doc.FinPlanSubject.Transact(doc);
        }

    }
}
