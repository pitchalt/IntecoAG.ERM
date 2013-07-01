using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;
//

namespace IntecoAG.ERM.FM.ReportHelper {

    [NavigationItem("Money")]
    [NonPersistent]
    public class fmCRHUnpayedRequestReportParameters : ReportParametersObjectBase {
        public fmCRHUnpayedRequestReportParameters(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Other = 50000m;
        }

        public override CriteriaOperator GetCriteria() {
            return CriteriaOperator.Parse("ReportDate = ?", ReportDate);
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            if (SortByName) {
                sorting.Add(new SortProperty("ReportDate", SortingDirection.Ascending));
                //sorting.Add(new SortProperty("crmWorkPlan.Current.Supplier.Name", SortingDirection.Ascending));
            }
            return sorting;
        }

        #region ПАРАМЕТРЫ НА ФОРМЕ

        // Дата отчёта
        private DateTime _ReportDateStart;
        [Custom("Caption", "Начальная дата")]
        public DateTime ReportDateStart {
            get {
                return _ReportDateStart;
            }
            set {
                _ReportDateStart = value;
            }
        }

        // Дата отчёта
        private DateTime _ReportDate;
        [Custom("Caption", "Конечная дата")]
        public DateTime ReportDate {
            get { return _ReportDate; }
            set { _ReportDate = value; }
        }

        // Граница Прочие
        private Decimal _Other;
        [Browsable(false)]
        [Custom("Caption", "Граница 'Прочие'")]
        public Decimal Other {
            get {
                return _Other;
            }
            set {
                _Other = value;
            }
        }

        // Вид отчёта - сжатый или полный (без обязательств или с ними)
        private Boolean _ReportMode = true;
        [Browsable(false)]
        [Custom("Caption", "Отчёт в краткой форме")]
        public Boolean ReportMode {
            get {
                return _ReportMode;
            }
            set {
                _ReportMode = value;
            }
        }

        // Сортировать ли
        private bool sortByName;
        [Browsable(false)]
        [Custom("Caption", "Сортировать по имени")]
        public bool SortByName {
            get { return sortByName; }
            set { sortByName = value; }
        }

        List<fmCPRPaymentRequestObligation> usedObligationList = new List<fmCPRPaymentRequestObligation>();
        List<fmCRHUnpayedRequestContractList> resultByContractList = new List<fmCRHUnpayedRequestContractList>();
        List<fmCRHUnpayedRequestContractList> resultByNonContractList = new List<fmCRHUnpayedRequestContractList>();

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Генерация записей отчёта на указанную дату
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="ReportDate"></param>
        public List<fmCRHUnpayedRequestNonPersistent> GenerateReportContent() {

            crmCParty ourParty = GetOurParty(Session);
            resultByContractList.Clear();
            resultByNonContractList.Clear();            
            usedObligationList.Clear();

            //XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(ssn);
            XPQuery<fmCPRPaymentRequestObligation> paymentRequestObligations = new XPQuery<fmCPRPaymentRequestObligation>(Session);

            // Все абсолютно обязтельства к оплате на заданный интервал дат, для которых нет платёжек отправленных в банк
            var querySecAll = (from paymentRequestObligation in paymentRequestObligations
                            where paymentRequestObligation.PaymentRequestBase.State == PaymentRequestStates.IN_PAYMENT
                               && (paymentRequestObligation.PaymentRequestBase.DateFinance.Date >= this.ReportDateStart.Date &&
                               paymentRequestObligation.PaymentRequestBase.DateFinance.Date < this.ReportDate.AddDays(1).Date)
                            select paymentRequestObligation).Distinct().ToList();

            List<fmCPRPaymentRequestObligation> ObligationByKazahkstanList = new List<fmCPRPaymentRequestObligation>();
            List<fmCPRPaymentRequestObligation> ObligationByEnergeticList = new List<fmCPRPaymentRequestObligation>();
            List<fmCPRPaymentRequestObligation> ObligationByPPRList = new List<fmCPRPaymentRequestObligation>();

            // Казахстан
            foreach (var obl in querySecAll) {
                if (obl.Order != null && obl.Order.Subject != null && ((obl.Order.Code.Length == 8 && obl.Order.Code.EndsWith("24")) || obl.Order.Subject.Direction.Code == "ОЗ")) {
                    ObligationByKazahkstanList.Add(obl);
                }
            }
            var querySecKaz = from paymentRequestObligation in ObligationByKazahkstanList
                             select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 3, "Счета по договорам", 0, "Поля падения", ourParty, querySecKaz);

            // Энергетика
            var querySecAll_Without_Kaz = querySecAll.Except(ObligationByKazahkstanList);
            foreach (var obl in querySecAll_Without_Kaz) {
                if (obl.Order != null && (obl.Order.Code == "23220000" || obl.Order.Code == "23210000")) {
                    ObligationByEnergeticList.Add(obl);
                }
            }
            var querySecEnerg = from paymentRequestObligation in ObligationByEnergeticList
                              select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 4, "Счета по договорам", 0, "Энергетика", ourParty, querySecEnerg);

            // ППР
            var querySecAll_Without_Kaz_Energ = (querySecAll.Except(ObligationByKazahkstanList)).Except(ObligationByEnergeticList);
            foreach (var obl in querySecAll_Without_Kaz_Energ) {
                if (obl.Order != null 
                    && obl.Order.Code.Length == 8 
                    && (obl.Order.Code.EndsWith("05") 
                        || obl.Order.Code.EndsWith("09")
                        || obl.Order.Code.EndsWith("10")
                        || obl.Order.Code.EndsWith("11")
                        || obl.Order.Code.EndsWith("12")
                        || obl.Order.Code.EndsWith("13")
                        || obl.Order.Code.EndsWith("22")
                        )) {
                    ObligationByPPRList.Add(obl);
                }
            }
            var querySecPPR = from paymentRequestObligation in ObligationByPPRList
                                select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 5, "Счета по договорам", 0, "ППР", ourParty, querySecPPR);



            // Списки - только договорных и только недоговорных
            var querySecAll_Without_Kaz_Energ_PPR = ((querySecAll.Except(ObligationByKazahkstanList)).Except(ObligationByEnergeticList)).Except(ObligationByPPRList);
            List<fmCPRPaymentRequestObligation> ObligationByContractList = new List<fmCPRPaymentRequestObligation>();
            List<fmCPRPaymentRequestObligation> ObligationByNonContractList = new List<fmCPRPaymentRequestObligation>();
            foreach (var obl in querySecAll_Without_Kaz_Energ_PPR) {
                if (obl.PaymentRequestBase as fmCPRPaymentRequestContract != null) {
                    ObligationByContractList.Add(obl);
                } else {
                    ObligationByNonContractList.Add(obl);                
                }
            }

            // В совершенно отдельный раздел должны попадать заявки, относящиеся к Казахстану: направление == "ОЗ" или код сметы == 24
            // Затем Энергетика - это заявки, имеющие конкретный список заказов (fmOrder == 23220000 или == 23210000)
            // ППР отдельный раздел, пределямый по смете: смета - в списке 05,09,10,11, 12,13,22
            // В каждом из разделов - Договорном и не договорном - выделить ПТР, которое пределятся по одноименному направлению.


            // ДОГОВОРНЫЕ

            var querySecContract = from paymentRequestObligation in ObligationByContractList
                              where paymentRequestObligation.Order != null && paymentRequestObligation.Order.AnalitycOrderSource != null
                              select paymentRequestObligation;

            // Формирование 1-го раздела, 0-го подраздела ПТР. В него входят все заявки с направлением ПТР
            // Собранные из договорных
            var querySec10 = from paymentRequestObligation in querySecContract
                             where (paymentRequestObligation.Order != null && paymentRequestObligation.Order.Subject != null && paymentRequestObligation.Order.Subject.Direction != null && paymentRequestObligation.Order.Subject.Direction.Code == "ПТР")
                             select paymentRequestObligation;
            // Собранные из НЕдоговорных
            var querySec20 = from paymentRequestObligation in ObligationByNonContractList
                             where (paymentRequestObligation.Order != null && paymentRequestObligation.Order.Subject != null && paymentRequestObligation.Order.Subject.Direction != null && paymentRequestObligation.Order.Subject.Direction.Code == "ПТР")
                             select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 1, "Счета по договорам", 0, "ПТР", ourParty, querySec10.Union(querySec20));

            // Формирование 1-го раздела, 1-го подраздела Госзаказ. В него входят все заявки, источник заказа которых ГЗ.ГОЗ и ГЗ.Прочие, основанные на договорах
            // Только договорные!
            var querySecContract_Without_PTR = querySecContract.Except(querySec10);
            var querySec11 = from paymentRequestObligation in querySecContract_Without_PTR
                             where (paymentRequestObligation.Order.AnalitycOrderSource.IsGZ && !paymentRequestObligation.Order.AnalitycRegion.IsVED)
                            select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 1, "Счета по договорам", 1, "Госзаказ", ourParty, querySec11);

            // Формирвоание 1-го раздела, 2-го подраздела ВЭД. В него входят все заявки, источник заказа которых ГЗ.ГОЗ.ВТС, а также источник заказа (fmCOrderAnalitycOrderSource) - ВЭД.ВТС, ВЭД.Прочие
            // Только договорные!
            var querySec12 = from paymentRequestObligation in querySecContract_Without_PTR
                            where (paymentRequestObligation.Order.AnalitycRegion != null &&
                                  paymentRequestObligation.Order.AnalitycRegion.IsVED
//                                  paymentRequestObligation.Order.AnalitycOrderSource.Code == "ГЗ.ГОЗ.ВТС"
//                                || paymentRequestObligation.Order.AnalitycOrderSource.Code == "ВЭД.ВТС"
//                                || paymentRequestObligation.Order.AnalitycOrderSource.Code == "ВЭД.Прочие"
                                )
                            select paymentRequestObligation;
            ByAnalitycSourceCode(Session, 1, "Счета по договорам", 2, "ВЭД", ourParty, querySec12);

            // Формирвоание 1-го раздела, 3-го подраздела Прочие. В него входят все заявки 1-го раздела, не вошедшие в 1-й и 2-й подразделы
            // Только договорные!
            // !!!!!!! ПО ПРОСЬБЕ ЛЕСЮК Л.М. РАЗДЕЛ "Счета по договорам" - "Прочие (по договорам)" ПРИСОЕДИНЁН К РАЗДЕЛУ Прочие по разовым счетам
            var querySec13a = from paymentRequestObligation in querySecContract_Without_PTR
                            select paymentRequestObligation;
            var querySec13 = (querySec13a.Except(querySec11)).Except(querySec12);
            //ByAnalitycSourceCode(Session, 1, "Счета по договорам", 3, "Прочие (по договорам)", ourParty, querySec13);


            // НЕ ДОГОВОРНЫЕ

            // Формирование 2-го раздела, 0-го подраздела ПТР. В него входят все заявки с направлением ПТР
            // Только НЕ договорные!
            // !!!!!!! ПО ПРОСЬБЕ ЛЕСЮК Л.М. РАЗДЕЛ "Прочие счета" - "ПТР" ПРИСОЕДИНЁН К РАЗДЕЛУ ПТР В СЕКЦИИ ПО ДОГОВОРАМ
            //var querySec20 = from paymentRequestObligation in ObligationByNonContractList
            //                 where (paymentRequestObligation.Order != null && paymentRequestObligation.Order.Subject != null && paymentRequestObligation.Order.Subject.Direction != null && paymentRequestObligation.Order.Subject.Direction.Code == "ПТР")
            //                 select paymentRequestObligation;
            //ByCostItemCode(Session, 2, "Прочие счета", 0, "ПТР", ourParty, querySec20);

            var ObligationByNonContractList_Without_PTR = ObligationByNonContractList.Except(querySec20);

            // Формирование 2-го раздела. В него входят все заявки, в которых Статья ДДС: 6001 - 6009 без 6003
            // Только НЕ договорные!
            var querySec21 = from paymentRequestObligation in ObligationByNonContractList_Without_PTR
                             where paymentRequestObligation.CostItem != null 
                                && (paymentRequestObligation.CostItem.Code == "6001" 
                                    || paymentRequestObligation.CostItem.Code == "6002")
                            select paymentRequestObligation;
            List<fmCPRPaymentRequestObligation> querySec21List = new List<fmCPRPaymentRequestObligation>();
            querySec21List = querySec21.ToList();
            for (int i = 6004; i <= 6009; i++) {
                var query = from paymentRequestObligation in ObligationByNonContractList
                            where paymentRequestObligation.CostItem.Code == i.ToString()
                            select paymentRequestObligation;
                //querySec21 = querySec21.Union(query);
                querySec21List.AddRange(query.ToList());
            }
            var querySec21A = from paymentRequestObligation in querySec21List
                              select paymentRequestObligation;
            ByCostItemCode(Session, 2, "Прочие счета", 1, "Материалы", ourParty, querySec21A);

            // Формирование 2-го раздела. В него входят все заявки, в которых Статья ДДС: 2000 - 2099 или CostItem == 5002 или CostItem == 5003
            // Только НЕ договорные!
            var querySec22 = from paymentRequestObligation in ObligationByNonContractList_Without_PTR
                             where paymentRequestObligation.CostItem != null 
                             && (paymentRequestObligation.CostItem.Code == "2000"
                               || paymentRequestObligation.CostItem.Code == "5002"
                               || paymentRequestObligation.CostItem.Code == "5003")
                            select paymentRequestObligation;
            List<fmCPRPaymentRequestObligation> querySec22List = new List<fmCPRPaymentRequestObligation>();
            querySec22List = querySec22.ToList();
            for (int i = 2001; i <= 2099; i++) {
                var query = from paymentRequestObligation in ObligationByNonContractList
                            where paymentRequestObligation.CostItem != null 
                                && paymentRequestObligation.CostItem.Code == i.ToString()
                            select paymentRequestObligation;
                //querySec22.Union(query);
                querySec22List.AddRange(query.ToList());
            }
            var querySec22A = from paymentRequestObligation in querySec22List
                              select paymentRequestObligation;
            ByCostItemCode(Session, 2, "Прочие счета", 2, "Зарплата", ourParty, querySec22A);

            // Формирование 2-го раздела. В него входят все заявки, в которых Статья ДДС: 5000 - 5100, кроме 5002 и 5003
            // Только НЕ договорные!
            var querySec23 = from paymentRequestObligation in ObligationByNonContractList_Without_PTR
                             where paymentRequestObligation.CostItem != null 
                             && (paymentRequestObligation.CostItem.Code == "5000"
                               || paymentRequestObligation.CostItem.Code == "5001")
                            select paymentRequestObligation;
            List<fmCPRPaymentRequestObligation> querySec23List = new List<fmCPRPaymentRequestObligation>();
            querySec23List = querySec23.ToList();
            for (int i = 5004; i <= 5100; i++) {
                var query = from paymentRequestObligation in ObligationByNonContractList
                            where paymentRequestObligation.CostItem != null 
                                && paymentRequestObligation.CostItem.Code == i.ToString()
                            select paymentRequestObligation;
                //querySec23.Union(query);
                querySec23List.AddRange(query.ToList());
            }
            var querySec23A = from paymentRequestObligation in querySec23List
                              select paymentRequestObligation;
            ByCostItemCode(Session, 2, "Прочие счета", 3, "Налоги", ourParty, querySec23A);

            // Формирование 2-го раздела. 4-го подраздела Прочие. В него входят все заявки, которые не договорные и не вошли в предыдущие разделы
            // Только НЕ договорные объединены с договорными Прочими
            var querySec210a = from paymentRequestObligation in ObligationByNonContractList_Without_PTR
                            select paymentRequestObligation;
            var querySec210 = querySec210a.Except(querySec23A).Except(querySec22A).Except(querySec21A);
            ByCostItemCode(Session, 2, "Прочие счета", 4, "Прочие", ourParty, querySec210.Union(querySec13));


            fmCRHUnpayedRequestNonPersistent uRNP = new fmCRHUnpayedRequestNonPersistent(Session);
            uRNP.ReportDate = this.ReportDate;
            uRNP.ReportDateStart = this.ReportDateStart;

            List<fmCRHUnpayedRequestNonPersistent> uRNPList = new List<fmCRHUnpayedRequestNonPersistent>();

            // Формирование коллекций

            // Казахстан (ОЗ или Поля падения)
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 3 && r.SubSectionNumber == 0)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestKazahkstanLines.Add(unpayedRequestLine);
            }

            // Энеогетика
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 4 && r.SubSectionNumber == 0)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestEnergeticLines.Add(unpayedRequestLine);
            }

            // ППР
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 5 && r.SubSectionNumber == 0)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestPPRLines.Add(unpayedRequestLine);
            }

            // По договору - Госзаказ
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 1)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestContractLines.Add(unpayedRequestLine);
            }

            // По договору - ПТР
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 0)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestContractPTRLines.Add(unpayedRequestLine);
            }

            // По договору - ВЭД
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 2)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestContractVEDLines.Add(unpayedRequestLine);
            }

            // По договору - Прочие
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 3)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestContractOtherLines.Add(unpayedRequestLine);
            }


            // НЕ по договору - ПТР
            Dictionary<crmCParty, List<string>> OrgCommentList = new Dictionary<crmCParty, List<string>>();

            // НЕ по договору - ПТР
            OrgCommentList.Clear();
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 0)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                
                if (!string.IsNullOrEmpty(unpayedRequestLine.Comment) && unpayedRequestLine.Comment.Trim() != "") {
                    if (!OrgCommentList.Keys.Contains(unpayedRequestLine.Party)) {
                        List<string> list = new List<string>();
                        list.Add(unpayedRequestLine.Comment.Trim());
                        OrgCommentList.Add(unpayedRequestLine.Party, list);
                    } else {
                        OrgCommentList[unpayedRequestLine.Party].Add(unpayedRequestLine.Comment.Trim());
                    }
                }
                
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestNonContractPTRLines.Add(unpayedRequestLine);
            }
            SetOrganizationComment(uRNP.UnpayedRequestNonContractPTRLines, OrgCommentList);
            //if (OrgCommentList.Count > 0) {
            //    string organizationComment = string.Join(",", OrgCommentList.ToArray());
            //    for (int i = 0; i < uRNP.UnpayedRequestNonContractPTRLines.Count(); i++) {
            //         uRNP.UnpayedRequestNonContractPTRLines[i].OrganizationComment = organizationComment;
            //    } 
            //}

            // НЕ по договору - Материалы
            OrgCommentList.Clear();
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 1)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);
                
                if (!string.IsNullOrEmpty(unpayedRequestLine.Comment) && unpayedRequestLine.Comment.Trim() != "") {
                    if (!OrgCommentList.Keys.Contains(unpayedRequestLine.Party)) {
                        List<string> list = new List<string>();
                        list.Add(unpayedRequestLine.Comment.Trim());
                        OrgCommentList.Add(unpayedRequestLine.Party, list);
                    } else {
                        OrgCommentList[unpayedRequestLine.Party].Add(unpayedRequestLine.Comment.Trim());
                    }
                }
                
                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestNonContractLines.Add(unpayedRequestLine);
            }
            SetOrganizationComment(uRNP.UnpayedRequestNonContractLines, OrgCommentList);

            // НЕ по договору - Зарплата
            OrgCommentList.Clear();
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 2)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);

                if (!string.IsNullOrEmpty(unpayedRequestLine.Comment) && unpayedRequestLine.Comment.Trim() != "") {
                    if (!OrgCommentList.Keys.Contains(unpayedRequestLine.Party)) {
                        List<string> list = new List<string>();
                        list.Add(unpayedRequestLine.Comment.Trim());
                        OrgCommentList.Add(unpayedRequestLine.Party, list);
                    } else {
                        OrgCommentList[unpayedRequestLine.Party].Add(unpayedRequestLine.Comment.Trim());
                    }
                }

                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestNonContractZPLines.Add(unpayedRequestLine);
            }
            SetOrganizationComment(uRNP.UnpayedRequestNonContractZPLines, OrgCommentList);

            // НЕ по договору - Налоги
            OrgCommentList.Clear();
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 3)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);

                if (!string.IsNullOrEmpty(unpayedRequestLine.Comment) && unpayedRequestLine.Comment.Trim() != "") {
                    if (!OrgCommentList.Keys.Contains(unpayedRequestLine.Party)) {
                        List<string> list = new List<string>();
                        list.Add(unpayedRequestLine.Comment.Trim());
                        OrgCommentList.Add(unpayedRequestLine.Party, list);
                    } else {
                        OrgCommentList[unpayedRequestLine.Party].Add(unpayedRequestLine.Comment.Trim());
                    }
                }

                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestNonContractNLGLines.Add(unpayedRequestLine);
            }
            SetOrganizationComment(uRNP.UnpayedRequestNonContractNLGLines, OrgCommentList);

            // НЕ по договору - Прочие
            OrgCommentList.Clear();
            foreach (fmCRHUnpayedRequestContractList unpayedRequestList in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 4)) {
                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(Session);
                SetLineValue(unpayedRequestLine, unpayedRequestList);

                if (!string.IsNullOrEmpty(unpayedRequestLine.Comment) && unpayedRequestLine.Comment.Trim() != "") {
                    if (!OrgCommentList.Keys.Contains(unpayedRequestLine.Party)) {
                        List<string> list = new List<string>();
                        list.Add(unpayedRequestLine.Comment.Trim());
                        OrgCommentList.Add(unpayedRequestLine.Party, list);
                    } else {
                        OrgCommentList[unpayedRequestLine.Party].Add(unpayedRequestLine.Comment.Trim());
                    }
                }

                unpayedRequestLine.UnpayedRequest = uRNP;
                uRNP.UnpayedRequestNonContractOtherLines.Add(unpayedRequestLine);
            }
            SetOrganizationComment(uRNP.UnpayedRequestNonContractOtherLines, OrgCommentList);


            // Суммы по подсекциям
            
            // Казахстан
            List<csValuta> valutaListContractKAZ = new List<csValuta>();
            var valutaListAllContractKAZ = (valutaListContractKAZ
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 3 && r.SubSectionNumber == 0)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 3 && r.SubSectionNumber == 0)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractKAZ) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestKazahkstanLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestKazahkstanLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestKazahkstanSumLines.Add(uRVC);
                }
            }

            // Энергетика
            List<csValuta> valutaListContractEnerg = new List<csValuta>();
            var valutaListAllContractEnerg = (valutaListContractEnerg
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 4 && r.SubSectionNumber == 0)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 4 && r.SubSectionNumber == 0)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractEnerg) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestEnergeticLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestEnergeticLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestEnergeticSumLines.Add(uRVC);
                }
            }

            // ППР
            List<csValuta> valutaListContractPPR = new List<csValuta>();
            var valutaListAllContractPPR = (valutaListContractPPR
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 5 && r.SubSectionNumber == 0)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 5 && r.SubSectionNumber == 0)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractPPR) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestPPRLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestPPRLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestPPRSumLines.Add(uRVC);
                }
            }


            // По договору - ПТР
            List<csValuta> valutaListContractPTR = new List<csValuta>();
            var valutaListAllContractPTR = (valutaListContractPTR
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 0)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 0)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractPTR) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestContractPTRLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestContractPTRLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestContractPTRSumLines.Add(uRVC);
                }
            }

            // По договору - Госзаказ
            List<csValuta> valutaListContractGZ = new List<csValuta>();
            var valutaListAllContractGZ = (valutaListContractGZ
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 1)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 1)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractGZ) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestContractLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestContractLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestContractSectionGZSumLines.Add(uRVC);
                }
            }

            // По договору - ВЭД
            List<csValuta> valutaListContractVED = new List<csValuta>();
            var valutaListAllContractVED = (valutaListContractVED
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 2)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 2)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractVED) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestContractVEDLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestContractVEDLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestContractSectionVEDSumLines.Add(uRVC);
                }
            }

            // По договору - Прочие
            List<csValuta> valutaListContractOther = new List<csValuta>();
            var valutaListAllContractOther = (valutaListContractOther
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 3)
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList.Where(r => r.SectionNumber == 1 && r.SubSectionNumber == 3)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllContractOther) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestContractOtherLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestContractOtherLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestContractSectionOtherSumLines.Add(uRVC);
                }
            }



            // НЕ по договору - ПТР
            List<csValuta> valutaListNonContractPTR = new List<csValuta>();
            var valutaListAllNonContractPTR = (valutaListNonContractPTR
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 0)
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 0)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllNonContractPTR) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestNonContractPTRLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestNonContractPTRLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractPTRSumLines.Add(uRVC);
                }
            }

            // НЕ по договору - Материалы
            List<csValuta> valutaListNonContractMAT = new List<csValuta>();
            var valutaListAllNonContractMAT = (valutaListNonContractMAT
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 1)
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 1)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllNonContractMAT) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestNonContractLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestNonContractLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractMATSumLines.Add(uRVC);
                }
            }

            // НЕ по договору - Зарплата
            List<csValuta> valutaListNonContractZR = new List<csValuta>();
            var valutaListAllNonContractZR = (valutaListNonContractZR
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 2)
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 2)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllNonContractZR) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestNonContractZPLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestNonContractZPLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractZRSumLines.Add(uRVC);
                }
            }

            // НЕ по договору - Налоги
            List<csValuta> valutaListNonContractNLG = new List<csValuta>();
            var valutaListAllNonContractNLG = (valutaListNonContractNLG
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 3)
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 3)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllNonContractNLG) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestNonContractNLGLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestNonContractNLGLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractNLGSumLines.Add(uRVC);
                }
            }

            // НЕ по договору - Прочие
            List<csValuta> valutaListNonContractOther = new List<csValuta>();
            var valutaListAllNonContractOther = (valutaListNonContractOther
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 4)
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList.Where(r => r.SectionNumber == 2 && r.SubSectionNumber == 4)
                       select obl.ValutaPayment)).Distinct();

            foreach (var valuta in valutaListAllNonContractOther) {
                fmCRHUnpayedRequestValutaCourseList uRVC = new fmCRHUnpayedRequestValutaCourseList(Session);
                uRVC.Valuta = valuta;

                uRVC.ReportResultInPaymentValuta = uRNP.UnpayedRequestNonContractOtherLines.Where(r => r.ValutaPayment == valuta).Sum(r => r.SumPayment);
                uRVC.ReportResultInObligationValuta = uRNP.UnpayedRequestNonContractOtherLines.Where(r => r.ValutaObligation == valuta).Sum(r => r.SumObligation);

                // Интересуют только суммы платежа, поэтому проверка на 0
                if (uRVC.ReportResultInPaymentValuta != 0) {
                    uRVC.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractOtherSumLines.Add(uRVC);
                }
            }


            // ОБЩИЕ СУММЫ ПО 2-м РАЗДЕЛАМ И ОТЧЁТУ В ЦЕЛОМ

            // Список курсов валют
            // Итоги по всему отчёту
            List<csValuta> valutaList = new List<csValuta>();
            var valutaListAll = valutaList
                .Union(from obl in resultByContractList select obl.ValutaObligation)
                .Union(from obl in resultByContractList select obl.ValutaPayment)
                .Union(from obl in resultByNonContractList select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList select obl.ValutaPayment);

            XPQuery<csCNMValutaCourse> valutaCourses = new XPQuery<csCNMValutaCourse>(Session);
            var queryValutaCourses = from valutaCourse in valutaCourses
                                     where valutaCourse.CourseDate.Date == this.ReportDate.Date
                                     select valutaCourse;
            foreach (var valutaCourse in queryValutaCourses) {
                if (valutaListAll.Contains(valutaCourse.Valuta)) {   // && valutaCourse.Valuta.Code != "RUB") {
                    fmCRHUnpayedRequestValutaCourseList unpayedRequestValutaCourse = new fmCRHUnpayedRequestValutaCourseList(Session);
                    unpayedRequestValutaCourse.ConversionCount = valutaCourse.ConversionCount;
                    unpayedRequestValutaCourse.Course = valutaCourse.Course;
                    unpayedRequestValutaCourse.CourseDate = valutaCourse.CourseDate.Date;
                    unpayedRequestValutaCourse.Valuta = valutaCourse.Valuta;

                    //unpayedRequestValutaCourse.ReportResultInPaymentValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Payment");
                    //unpayedRequestValutaCourse.ReportResultInObligationValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Obligation");

                    unpayedRequestValutaCourse.ReportResultInPaymentValuta = resultByContractList.Where(r => r.ValutaPayment == valutaCourse.Valuta).Sum(r => r.SumPayment)
                                                                           + resultByNonContractList.Where(r => r.ValutaPayment == valutaCourse.Valuta).Sum(r => r.SumPayment);
                    unpayedRequestValutaCourse.ReportResultInObligationValuta = resultByContractList.Where(r => r.ValutaObligation == valutaCourse.Valuta).Sum(r => r.SumObligation)
                                                                              + resultByNonContractList.Where(r => r.ValutaObligation == valutaCourse.Valuta).Sum(r => r.SumObligation);;

                    // Интересуют только суммы платежа, поэтому проверка на 0
                    if (unpayedRequestValutaCourse.ReportResultInPaymentValuta != 0) {
                        unpayedRequestValutaCourse.UnpayedRequest = uRNP;
                        uRNP.ValutaCourses.Add(unpayedRequestValutaCourse);
                    }
                }
            }

            // Итоги по секции Договоров
            List<csValuta> valutaListContract = new List<csValuta>();
            var valutaListAllContract = (valutaListContract
                .Union(from obl in resultByContractList
                       select obl.ValutaObligation)
                .Union(from obl in resultByContractList
                       select obl.ValutaPayment)).Distinct();

            XPQuery<csCNMValutaCourse> valutaCoursesContract = new XPQuery<csCNMValutaCourse>(Session);
            var queryValutaCoursesContract = from valutaCourse in valutaCourses
                                     where valutaCourse.CourseDate.Date == this.ReportDate.Date
                                     select valutaCourse;
            foreach (var valutaCourse in queryValutaCoursesContract) {
                if (valutaListAllContract.Contains(valutaCourse.Valuta)) {
                    fmCRHUnpayedRequestValutaCourseList unpayedRequestValutaCourse = new fmCRHUnpayedRequestValutaCourseList(Session);
                    unpayedRequestValutaCourse.ConversionCount = valutaCourse.ConversionCount;
                    unpayedRequestValutaCourse.Course = valutaCourse.Course;
                    unpayedRequestValutaCourse.CourseDate = valutaCourse.CourseDate.Date;
                    unpayedRequestValutaCourse.Valuta = valutaCourse.Valuta;

                    //unpayedRequestValutaCourse.ReportResultInPaymentValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Payment");
                    //unpayedRequestValutaCourse.ReportResultInObligationValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Obligation");

                    unpayedRequestValutaCourse.ReportResultInPaymentValuta = resultByContractList.Where(r => r.ValutaPayment == valutaCourse.Valuta).Sum(r => r.SumPayment);
                    unpayedRequestValutaCourse.ReportResultInObligationValuta = resultByContractList.Where(r => r.ValutaObligation == valutaCourse.Valuta).Sum(r => r.SumObligation);

                    unpayedRequestValutaCourse.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestContractSectionLines.Add(unpayedRequestValutaCourse);
                }
            }

            // Итоги по секции НЕ Договоров
            List<csValuta> valutaListNonContract = new List<csValuta>();
            var valutaListAllNonContract = valutaListNonContract
                .Union(from obl in resultByNonContractList
                       select obl.ValutaObligation)
                .Union(from obl in resultByNonContractList
                       select obl.ValutaPayment);

            XPQuery<csCNMValutaCourse> valutaCoursesNonContract = new XPQuery<csCNMValutaCourse>(Session);
            var queryValutaCoursesNonContract = from valutaCourse in valutaCourses
                                             where valutaCourse.CourseDate.Date == this.ReportDate.Date
                                             select valutaCourse;
            foreach (var valutaCourse in queryValutaCoursesNonContract) {
                if (valutaListAllNonContract.Contains(valutaCourse.Valuta)) {
                    fmCRHUnpayedRequestValutaCourseList unpayedRequestValutaCourse = new fmCRHUnpayedRequestValutaCourseList(Session);
                    unpayedRequestValutaCourse.ConversionCount = valutaCourse.ConversionCount;
                    unpayedRequestValutaCourse.Course = valutaCourse.Course;
                    unpayedRequestValutaCourse.CourseDate = valutaCourse.CourseDate.Date;
                    unpayedRequestValutaCourse.Valuta = valutaCourse.Valuta;

                    //unpayedRequestValutaCourse.ReportResultInPaymentValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Payment");
                    //unpayedRequestValutaCourse.ReportResultInObligationValuta = GetReportResultInValuta(uRNP, valutaCourse.Valuta, "Obligation");

                    unpayedRequestValutaCourse.ReportResultInPaymentValuta = resultByNonContractList.Where(r => r.ValutaPayment == valutaCourse.Valuta).Sum(r => r.SumPayment);
                    unpayedRequestValutaCourse.ReportResultInObligationValuta = resultByNonContractList.Where(r => r.ValutaObligation == valutaCourse.Valuta).Sum(r => r.SumObligation);

                    unpayedRequestValutaCourse.UnpayedRequest = uRNP;
                    uRNP.UnpayedRequestNonContractSectionLines.Add(unpayedRequestValutaCourse);
                }
            }

            uRNPList.Add(uRNP);

            return uRNPList;
        }

        private void SetLineValue(fmCRHUnpayedRequestContractList unpayedRequestLine, fmCRHUnpayedRequestContractList unpayedRequestList) {
            unpayedRequestLine.Comment = unpayedRequestList.Comment;
            unpayedRequestLine.CourseEUR = unpayedRequestList.CourseEUR;
            unpayedRequestLine.CourseUSD = unpayedRequestList.CourseUSD;
            unpayedRequestLine.GroupName = unpayedRequestList.GroupName;
            unpayedRequestLine.GroupNumber = unpayedRequestList.GroupNumber;
            unpayedRequestLine.Party = unpayedRequestList.Party;
            unpayedRequestLine.PaymentRequest = unpayedRequestList.PaymentRequest;
            unpayedRequestLine.ReportDate = unpayedRequestList.ReportDate;
            unpayedRequestLine.ReportDateStart = unpayedRequestList.ReportDateStart;
            unpayedRequestLine.SectionName = unpayedRequestList.SectionName;
            unpayedRequestLine.SectionNumber = unpayedRequestList.SectionNumber;
            unpayedRequestLine.SourceGroupGUID = unpayedRequestList.SourceGroupGUID;
            unpayedRequestLine.SourceGroupType = unpayedRequestList.SourceGroupType;
            unpayedRequestLine.SumEUR = unpayedRequestList.SumEUR;
            unpayedRequestLine.SumRUB = unpayedRequestList.SumRUB;
            unpayedRequestLine.SumUSD = unpayedRequestList.SumUSD;

            unpayedRequestLine.SumObligation = unpayedRequestList.SumObligation;
            unpayedRequestLine.SumPayment = unpayedRequestList.SumPayment;

            unpayedRequestLine.PaymentRequestObligation = unpayedRequestList.PaymentRequestObligation;
            unpayedRequestLine.ValutaPayment = unpayedRequestList.ValutaPayment;
            unpayedRequestLine.ValutaObligation = unpayedRequestList.ValutaObligation;
            unpayedRequestLine.CostItem = unpayedRequestList.CostItem;
            unpayedRequestLine.Order = unpayedRequestList.Order;
            unpayedRequestLine.Subject = unpayedRequestList.Subject;
            unpayedRequestLine.SubjectGuid = unpayedRequestList.SubjectGuid;
            unpayedRequestLine.SubjectName = unpayedRequestList.SubjectName;

            unpayedRequestLine.SubSectionNumber = unpayedRequestList.SubSectionNumber;
            unpayedRequestLine.SubSectionName = unpayedRequestList.SubSectionName;

            //unpayedRequestLine.UnpayedRequest = uRNP;
            //Lines.Add(unpayedRequestLine);
        }

        private void SetOrganizationComment(List<fmCRHUnpayedRequestContractList> unpayedRequestLines, Dictionary<crmCParty, List<string>> OrgCommentList) {
            if (OrgCommentList.Count > 0) {
                var qOrgs = (from org in OrgCommentList
                             select org.Key).Distinct();
                foreach (var org in qOrgs) {
                    List<string> commentList = (OrgCommentList[org].Distinct()).ToList();
                    string organizationComment = string.Join(",", commentList.ToArray());
                    for (int i = 0; i < unpayedRequestLines.Count(); i++) {
                        if (unpayedRequestLines[i].Party == org) {
                            unpayedRequestLines[i].OrganizationComment = organizationComment;
                        }
                    }
                }
            }
        }

        private decimal GetReportResultInValuta(fmCRHUnpayedRequestNonPersistent unpayedRequest, csValuta valuta, string mode) {
            return GetReportResultByContractInValuta(unpayedRequest, valuta, mode) + GetReportResultByNonContractInValuta(unpayedRequest, valuta, mode);
        }

        private decimal GetReportResultByContractInValuta(fmCRHUnpayedRequestNonPersistent unpayedRequest, csValuta valuta, string mode) {
            Decimal sum = 0;
            if (mode == "Payment") {
                sum = (from urLine in unpayedRequest.UnpayedRequestContractLines
                        where urLine.ValutaPayment == valuta
                        select urLine.SumPayment).Sum();
            }
            if (mode == "Obligation") {
                sum = (from urLine in unpayedRequest.UnpayedRequestContractLines
                        where urLine.ValutaObligation == valuta
                        select urLine.SumObligation).Sum();
            }

            return sum;
        }

        private decimal GetReportResultByContractInValuta(fmCRHUnpayedRequest unpayedRequest, csValuta valuta, string mode) {
            Decimal sum = 0;
            if (mode == "Payment") {
                sum = (from urLine in unpayedRequest.UnpayedRequestContractLines
                       where urLine.ValutaPayment == valuta
                       select urLine.SumPayment).Sum();
            }
            if (mode == "Obligation") {
                sum = (from urLine in unpayedRequest.UnpayedRequestContractLines
                       where urLine.ValutaObligation == valuta
                       select urLine.SumObligation).Sum();
            }

            return sum;
        }

        private decimal GetReportResultByNonContractInValuta(fmCRHUnpayedRequestNonPersistent unpayedRequest, csValuta valuta, string mode) {
            Decimal sum = 0;
            if (mode == "Payment") {
                sum = (from urLine in unpayedRequest.UnpayedRequestNonContractLines
                        where urLine.ValutaPayment == valuta
                        select urLine.SumPayment).Sum();
            }
            if (mode == "Obligation") {
                sum = (from urLine in unpayedRequest.UnpayedRequestNonContractLines
                        where urLine.ValutaObligation == valuta
                        select urLine.SumObligation).Sum();
            }

            return sum;
        }

        private decimal GetReportResultByNonContractInValuta(fmCRHUnpayedRequest unpayedRequest, csValuta valuta, string mode) {
            Decimal sum = 0;
            if (mode == "Payment") {
                sum = (from urLine in unpayedRequest.UnpayedRequestNonContractLines
                       where urLine.ValutaPayment == valuta
                       select urLine.SumPayment).Sum();
            }
            if (mode == "Obligation") {
                sum = (from urLine in unpayedRequest.UnpayedRequestNonContractLines
                       where urLine.ValutaObligation == valuta
                       select urLine.SumObligation).Sum();
            }

            return sum;
        }

        private decimal GetReportResultInValuta(fmCRHUnpayedRequest unpayedRequest, csValuta valuta, string mode) {
            return GetReportResultByContractInValuta(unpayedRequest, valuta, mode) + GetReportResultByNonContractInValuta(unpayedRequest, valuta, mode);
        }

        private void ByAnalitycSourceCode(Session ssn, Int32 sectionNumber, String sectionName, Int32 subSectionNumber, String subSectionName, crmCParty ourParty, IEnumerable<fmCPRPaymentRequestObligation> query) {
            foreach (fmCPRPaymentRequestObligation obl in query) {

                if (usedObligationList.Contains(obl))
                    continue;

                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(ssn);

                String groupName = "";
                if (obl.Order != null && obl.Order.Subject != null) {
                    groupName = obl.Order.Subject.Direction.Code;
                }

                Guid subjectGuid = Guid.NewGuid();
                String subjectName = "";
                if (obl.Order != null && obl.Order.Subject != null) {
                    subjectGuid = obl.Order.Subject.Oid;
                    subjectName = obl.Order.Subject.Name;
                }

                SetLineFields(sectionNumber, sectionName, subSectionNumber, subSectionName, subjectGuid, subjectName, 1, groupName, ourParty, obl, unpayedRequestLine);

                resultByContractList.Add(unpayedRequestLine);
                usedObligationList.Add(obl);
            }
        }

        /*
        private void ByDirectionCode(Session ssn, Int32 sectionNumber, String sectionName, Int32 subSectionNumber, String subSectionName, crmCParty ourParty, IEnumerable<fmCPRPaymentRequestObligation> query) {
            foreach (fmCPRPaymentRequestObligation obl in query) {

                if (usedObligationList.Contains(obl))
                    continue;

                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(ssn);

                String groupName = "";
                if (obl.Order != null && obl.Order.Subject != null && obl.Order.Subject.Direction != null) {
                    groupName = obl.Order.Subject.Direction.Code;
                }

                Guid subjectGuid = Guid.NewGuid();
                String subjectName = "";
                if (obl.Order != null && obl.Order.Subject != null) {
                    subjectGuid = obl.Order.Subject.Oid;
                    subjectName = obl.Order.Subject.Name;
                }

                SetLineFields(sectionNumber, sectionName, subSectionNumber, subSectionName, subjectGuid, subjectName, 1, groupName, ourParty, obl, unpayedRequestLine);

                resultByContractList.Add(unpayedRequestLine);
                usedObligationList.Add(obl);
            }
        }
        */

        private void ByCostItemCode(Session ssn, Int32 sectionNumber, String sectionName, Int32 subSectionNumber, String subSectionName, crmCParty ourParty, IEnumerable<fmCPRPaymentRequestObligation> query) {
            foreach (fmCPRPaymentRequestObligation obl in query) {

                if (usedObligationList.Contains(obl))
                    continue;

                fmCRHUnpayedRequestContractList unpayedRequestLine = new fmCRHUnpayedRequestContractList(ssn);
                
                Guid subjectGuid = Guid.NewGuid();
                String subjectName = "";
                // В этом случае по темам группировать не надо
                //if (obl.Order != null && obl.Order.Subject != null) {
                //    subjectGuid = obl.Order.Subject.Oid;
                //    subjectName = obl.Order.Subject.Name;
                //}

                SetLineFields(sectionNumber, sectionName, subSectionNumber, subSectionName, subjectGuid, subjectName, 1, "", ourParty, obl, unpayedRequestLine);

                resultByNonContractList.Add(unpayedRequestLine);
                usedObligationList.Add(obl);
            }
        }

        private void SetLineFields(Int32 sectionNumber, String sectionName, Int32 subSectionNumber, String subSectionName, Guid subjectGuid, String subjectName, Int32 groupNumber, String groupName, crmCParty ourParty, fmCPRPaymentRequestObligation obl, fmCRHUnpayedRequestContractList unpayedRequestLine) {
            fmCPRPaymentRequest paymentRequest = obl.PaymentRequestBase;
            csValuta requestValuta = paymentRequest.Valuta;

            unpayedRequestLine.SectionNumber = sectionNumber;
            unpayedRequestLine.SectionName = sectionName;

            unpayedRequestLine.SubSectionNumber = subSectionNumber;
            unpayedRequestLine.SubSectionName = subSectionName;

            unpayedRequestLine.SubjectGuid = subjectGuid;
            unpayedRequestLine.SubjectName = subjectName;

            unpayedRequestLine.GroupName = groupName;
            unpayedRequestLine.GroupNumber = groupNumber;

            unpayedRequestLine.Party = (ourParty.INN == (paymentRequest.PartyPayReceiver.INN) ? paymentRequest.PartyPaySender : paymentRequest.PartyPayReceiver);
            unpayedRequestLine.PaymentRequest = paymentRequest;
            unpayedRequestLine.ReportDate = ReportDate;

            //unpayedRequestLine.SourceGroupGUID = ;
            //unpayedRequestLine.SourceGroupType = ;

            unpayedRequestLine.SumEUR = ((requestValuta == GetValutaByCode("EUR")) ? paymentRequest.Summ : 0);
            unpayedRequestLine.SumUSD = ((requestValuta == GetValutaByCode("USD")) ? paymentRequest.Summ : 0);

            if (requestValuta == GetValutaByCode("RUB")) {
                unpayedRequestLine.SumRUB = obl.Summ;
            } else {
                // Курс берётся на следующий день, т.к. в этот день будет утверждение стоимостей на платёрке
                unpayedRequestLine.SumRUB = (unpayedRequestLine.SumEUR + unpayedRequestLine.SumUSD) * csCNMValutaCourse.GetCrossCourceOnDate(Session, ReportDate.AddDays(1), requestValuta, GetValutaByCode("RUB"));
            }

            unpayedRequestLine.SumObligation = obl.Summ;
            // Курс берётся на следующий день, т.к. в этот день будет утверждение стоимостей на платёрке
            unpayedRequestLine.SumPayment = obl.Summ * csCNMValutaCourse.GetCrossCourceOnDate(Session, ReportDate.AddDays(1), obl.Valuta, paymentRequest.PaymentValuta);

            unpayedRequestLine.CourseEUR = GetCourseValutaByCode("EUR");
            unpayedRequestLine.CourseUSD = GetCourseValutaByCode("USD");

            unpayedRequestLine.Comment = paymentRequest.Comment;

            unpayedRequestLine.PaymentRequestObligation = obl;
            unpayedRequestLine.ValutaPayment = paymentRequest.PaymentValuta;
            unpayedRequestLine.ValutaObligation = obl.Valuta;
            unpayedRequestLine.CostItem = obl.CostItem;
            unpayedRequestLine.Order = obl.Order;
            if (obl.Order != null) {
                unpayedRequestLine.Subject = obl.Order.Subject;
            }
        }

        private crmCParty GetOurParty(Session ssn) {
            // Наша организация
            crmCParty _OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    _OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(ssn).Party;
                }
            }
            return _OurParty;
        }

        private csValuta GetValutaByCode(string code) {
            XPQuery<csValuta> valutas = new XPQuery<csValuta>(Session);
            csValuta Valuta = (from valuta in valutas
                               where valuta.Code.ToUpper() == code.ToUpper()
                               select valuta).FirstOrDefault();
            return Valuta;
        }

        private Decimal GetCourseValutaByCode(string code) {
            // Курс берётся на следующий день, т.к. в этот день будет утверждение стоимостей на платёрке
            csValuta valuta = GetValutaByCode(code);
            XPQuery<csCNMValutaCourse> valutaCourses = new XPQuery<csCNMValutaCourse>(Session);
            var queryCourses = from valutaCourse in valutaCourses
                               where valutaCourse.Valuta == valuta
                                  && valutaCourse.CourseDate.Date == this.ReportDate.Date.AddDays(1)
                               select valutaCourse;
            foreach (var valutaCourse in queryCourses) {
                return valutaCourse.Course;
            }
            return 0;
        }

        #endregion

    }

}
