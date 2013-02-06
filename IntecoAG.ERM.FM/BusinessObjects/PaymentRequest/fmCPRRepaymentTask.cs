using System;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Погашение заявок на оплату (не счетов). 
    // В случае, если статус задачи Unknown, то задача обрабатывается в отделе, указанном представителем финансового отдела

    /// <summary>
    /// Статусы задачи
    /// </summary>
    public enum RepaymentTaskStates {
        /// <summary>
        /// На обработку
        /// </summary>
        FOR_PROCESSING = 1,
        /// <summary>
        /// На обработку как неопознанный платёж
        /// </summary>
        UNKNOWN = 2,
        /// <summary>
        /// Обработана и закрыта
        /// </summary>
        CLOSED = 3
    }

    //[NavigationItem("Money")]
    [Persistent("fmPRRepaymentTask")]
    public class fmCPRRepaymentTask : csCComponent
    {
        public fmCPRRepaymentTask(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRRepaymentTask);
            this.CID = Guid.NewGuid();

            this.Date = DateTime.Now;
            this.State = RepaymentTaskStates.FOR_PROCESSING;

            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            csCSecurityUser user1 = SessionHelper.GetObjectInSession<csCSecurityUser>(user, Session);
            if (user1 != null && user1.Staff != null) {
                TaskPerformer = user1.Staff.Department;
            }

            //paymentRequestObligationList = new List<fmCPRPaymentRequestObligation>();
        }

        #region ПОЛЯ КЛАССА

        private crmCParty OurParty;


        // Служебные поля
        private DateTime _Date;   // Дата создания задачи

        // Документ, для которого рассматривается задача
        private fmCDocRCB _PaymentDocument;   // Платёжный документ

        // Информационнные поля
        private crmBankAccount _BankAccount;   // Счёт
        //private Decimal _OperationRegisterSum;   // Сумма по регистру банковских операций
        //private Decimal _RepaymentRegisterSum;   // Сумма по регистру привязки

        // Поля управления
        private string _Comment; // Комментарий
        /*
        private hrmDepartment _Curator; // Подразделение-куратор (Финансовый отдел)
        private hrmStaff _Manager;   // Сотрудник, ответственный за исполнение задачи
        */
        private hrmDepartment _TaskPerformer;   // Подразделение-исполнитель задачи
        private RepaymentTaskStates _State;   // Статус задачи

        // Список заявок для выбора
        private XPCollection<PaymentRequestListItem> _AllRequests;

        private Decimal _Accuracy = 0.01m;  // Сопоставление с точностью до копейки

        private List<fmCPRPaymentRequestObligation> paymentRequestObligationList = new List<fmCPRPaymentRequestObligation>();

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime Date {
            get {
                return _Date;
            }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public RepaymentTaskStates State {
            get {
                return _State;
            }
            set {
                SetPropertyValue<RepaymentTaskStates>("State", ref _State, value);
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment {
            get {
                return _Comment;
            }
            set {
                SetPropertyValue<string>("Comment", ref _Comment, value);
            }
        }

        /*
        /// <summary>
        /// Подразделение-куратор (Финансовый отдел)
        /// </summary>
        [RuleRequiredField]
        public hrmDepartment Curator {
            get {
                return _Curator;
            }
            set {
                SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value);
            }
        }

        /// <summary>
        /// Сотрудник, ответственный за исполнение задачи
        /// </summary>
        [RuleRequiredField]
        [DataSourceProperty("Curator.Staffs", DataSourcePropertyIsNullMode.SelectAll)]
        public hrmStaff Manager {
            get {
                return _Manager;
            }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }
        */

        /// <summary>
        /// Подразделение-исполнитель задачи
        /// </summary>
        //[RuleRequiredField]
        public hrmDepartment TaskPerformer {
            get {
                return _TaskPerformer;
            }
            set {
                SetPropertyValue<hrmDepartment>("TaskPerformer", ref _TaskPerformer, value);
            }
        }

        /// <summary>
        /// Счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// Ссылка на платёжный документ
        /// </summary>
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }

        /// <summary>
        /// Сумма по регистру банковских операций OperationJournal
        /// </summary>
        public Decimal OperationRegisterSum {
            get {
                if (!IsLoading) {
                    return CalculateOperationRegisterSum();
                }
                return 0;
            }
        }

        /// <summary>
        /// Сумма по регистру привязки RepaymentJurnal
        /// </summary>
        public Decimal RepaymentRegisterSum {
            get {
                if (!IsLoading) {
                    return CalculateRepaymentRegisterSum();
                }
                return 0;
            }
        }


        /// <summary>
        /// Список ассоциированных Заявок и сумм
        /// </summary>
        [Aggregated]
        [Association("fmCPRRepaymentTask-fmCPRRepaymentTaskLines", typeof(fmCPRRepaymentTaskLine))]
        public XPCollection<fmCPRRepaymentTaskLine> RepaymentTaskLines {
            get { return GetCollection<fmCPRRepaymentTaskLine>("RepaymentTaskLines"); }
        }

        public XPCollection<PaymentRequestListItem> AllRequests {
            get {
                if (_AllRequests == null) {
                    _AllRequests = new XPCollection<PaymentRequestListItem>(Session, false);
                }
                return _AllRequests;
            }
        }

        #endregion

        #region МЕТОДЫ

        private Decimal CalculateOperationRegisterSum() {
            OurParty = GetOurParty();

            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            Decimal operationRegisterSum = (from operationJournal in operationJournals
                                     where operationJournal.PaymentDocument == PaymentDocument
                                     //select operationJournal.SumIn).RequestSum();
                                     select (operationJournal.PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) ? operationJournal.SumIn : operationJournal.SumOut).Sum();
            return operationRegisterSum;
        }

        private Decimal CalculateRepaymentRegisterSum() {
            OurParty = GetOurParty();

            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            Decimal repaymentRegisterSum = (from repaymentJournal in repaymentJournals
                                     where repaymentJournal.PaymentDocument == PaymentDocument
                                     //select repaymentJournal.SumIn).RequestSum();
                                     select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) ? repaymentJournal.SumIn : repaymentJournal.SumOut).Sum();
            return repaymentRegisterSum;
        }


        private Boolean CalculateCashFlowRegisterSumOnDate(DateTime operationDate, fmCPRRepaymentTaskLine line, out Decimal sumIn, out Decimal sumOut, out Decimal sumObligationIn, out Decimal sumObligationOut, out Decimal sumInAcc, out Decimal sumOutAcc) {
            fmCPRPaymentRequest request = line.PaymentRequest;
            Boolean isCashFlowRegister = false;
            XPQuery<crmCashFlowRegister> cashFlowRegisters = new XPQuery<crmCashFlowRegister>(this.Session, true);
            var cashFlowRegisterQuery = from cashFlowRegister in cashFlowRegisters
                                            where cashFlowRegister.PaymentDocument == PaymentDocument
                                               && cashFlowRegister.Token == request.Oid
                                               && cashFlowRegister.PaymentRequestObligationGUID == line.LevelObject.Oid
                                               && cashFlowRegister.OperationDate.Date == operationDate.Date
                                               && cashFlowRegister.Section == CashFlowRegisterSection.REPAYMENT_JOURNAL
                                            select cashFlowRegister;

            sumIn = 0;   // cashFlowRegisterQuery.Sum(r => r.SumIn);
            sumOut = 0;   // cashFlowRegisterQuery.Sum(r => r.SumOut);
            sumObligationIn = 0;   // cashFlowRegisterQuery.Sum(r => r.SumObligationIn);
            sumObligationOut = 0;   // cashFlowRegisterQuery.Sum(r => r.SumObligationOut);
            sumInAcc = 0;   // cashFlowRegisterQuery.Sum(r => r.SumInAcc);
            sumOutAcc = 0;   // cashFlowRegisterQuery.Sum(r => r.SumOutAcc);
            foreach (var cashFlowRegister in cashFlowRegisterQuery) {
                sumIn += cashFlowRegister.SumIn;
                sumOut += cashFlowRegister.SumOut;
                sumObligationIn += cashFlowRegister.SumObligationIn;
                sumObligationOut += cashFlowRegister.SumObligationOut;
                sumInAcc += cashFlowRegister.SumInAcc;
                sumOutAcc += cashFlowRegister.SumOutAcc;
                isCashFlowRegister = true;
            }
            return isCashFlowRegister;
        }

        private void CalculateCashFlowRegisterSumOnDateAndRequest(DateTime operationDate, fmCPRPaymentRequest request, out Decimal sumIn, out Decimal sumOut, out Decimal sumObligationIn, out Decimal sumObligationOut, out Decimal sumInAcc, out Decimal sumOutAcc) {
            XPQuery<crmCashFlowRegister> cashFlowRegisters = new XPQuery<crmCashFlowRegister>(this.Session, true);
            var cashFlowRegisterQuery = from cashFlowRegister in cashFlowRegisters
                                        where cashFlowRegister.PaymentDocument == PaymentDocument
                                           && cashFlowRegister.Token == request.Oid
                                           && cashFlowRegister.OperationDate.Date == operationDate.Date
                                           && cashFlowRegister.Section == CashFlowRegisterSection.REPAYMENT_JOURNAL
                                        select cashFlowRegister;

            sumIn = cashFlowRegisterQuery.Sum(r => r.SumIn);
            sumOut = cashFlowRegisterQuery.Sum(r => r.SumOut);

            sumObligationIn = cashFlowRegisterQuery.Sum(r => r.SumObligationIn);
            sumObligationOut = cashFlowRegisterQuery.Sum(r => r.SumObligationOut);

            sumInAcc = cashFlowRegisterQuery.Sum(r => r.SumInAcc);
            sumOutAcc = cashFlowRegisterQuery.Sum(r => r.SumOutAcc);
        }

        /// <summary>
        /// Список всех заявок, годных для присоединения к платёжному документу
        /// </summary>
        /// <returns></returns>
        public void FillRequestList() {
            //XPCollection<PaymentRequestListItem> requestListItems = new XPCollection<PaymentRequestListItem>(this.Session, false);
            XPQuery<fmCPRPaymentRequest> requests = new XPQuery<fmCPRPaymentRequest>(this.Session, true);
            var queryRequests = from request in requests
                                where (request.State == PaymentRequestStates.IN_PAYMENT || request.State == PaymentRequestStates.IN_BANK)
                                select request;
            foreach (fmCPRPaymentRequest request in queryRequests) {
                //requestListItems.Add(AddPaymentRequestList(request, this, 0, false));
                //AllRequests.Add(new PaymentRequestListItem(Session, request, this, 0, false));
                AddRequestToAll(request, false);
            }

            // Добавить в коллекцию те заявки, которые уже есть в Lines
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                //var SubLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == topLine);
                //foreach (var subLine in SubLines) {
                //    fmCPRPaymentRequest request = subLine.PaymentRequest;
                //    int PRCount = AllRequests.Where(p => p.PaymentRequest == request).Count();
                //    if (PRCount == 0) {
                //        //requestListItems.Add(AddPaymentRequestList(request, this, 0, false));
                //        AllRequests.Add(new PaymentRequestListItem(Session, request, this, 0, false));
                //    }
                //}

                //XPCollection<fmCPRRepaymentTaskLine> SubLines = GettTaskSubLineCollection(topLine);
                //foreach (var subLine in SubLines) {
                foreach (fmCPRRepaymentTaskLine subLine in topLine.Lines) {
                    fmCPRPaymentRequest request = subLine.PaymentRequest;
                    int PRCount = AllRequests.Where(p => p.PaymentRequest == request).Count();
                    if (PRCount == 0) {
                        //requestListItems.Add(AddPaymentRequestList(request, this, 0, false));
                        //AllRequests.Add(new PaymentRequestListItem(Session, request, this, 0, false));
                        AddRequestToAll(request, false);
                    }
                }
            }

            //foreach (fmCPRRepaymentTaskLine line in this.RepaymentTaskLines) {
            //    fmCPRPaymentRequest request = line.PaymentRequest;
            //    if (request != null) {
            //        int PRCount = AllRequests.Where(p => p.PaymentRequest == request).Count();
            //        if (PRCount == 0) {
            //            //requestListItems.Add(AddPaymentRequestList(request, this, 0, false));
            //            AllRequests.Add(new PaymentRequestListItem(Session, request, this, 0, false));
            //        }
            //    }
            //}
            //return requestListItems;

            SetAllUsingSign();   // Установка признаков, что заявка находится в списке Lines
        }

        /// <summary>
        /// Добавить заявку в список всех заявок
        /// </summary>
        /// <param name="request"></param>
        /// <param name="priority"></param>
        /// <param name="isUsed"></param>
        public void AddRequestToAll(fmCPRPaymentRequest request, Boolean isUsed) {
            AllRequests.Add(new PaymentRequestListItem(Session, request, this, isUsed));
        }

        //private PaymentRequestListItem AddPaymentRequestList(fmCPRPaymentRequest request, fmCPRRepaymentTask RepaymentTask, Int16 priority, Boolean isUsed) {
        //    PaymentRequestListItem prListItem = new PaymentRequestListItem(Session);
        //    prListItem.PaymentRequest = request;
        //    prListItem.RepaymentTask = this;
        //    prListItem.Priority = 0;
        //    prListItem.IsUsed = false;
        //    return prListItem;
        //}

        /// <summary>
        /// Добавление заявки в RepaymentTaskLines
        /// </summary>
        /// <param name="request"></param>
        private void AddRequestToLineStructure(fmCPRPaymentRequest request) {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                //int countSublines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == topLine && rtl.PaymentRequest == request).Count();
                //if (countSublines == 0) {
                //    //fmCPRRepaymentTaskLine rtLine = new fmCPRRepaymentTaskLine(Session, request, topLine.PaymentDate, 0, 0, this, topLine);
                //    fmCPRRepaymentTaskLine rtLine = new fmCPRRepaymentTaskLine(Session, request, topLine.PaymentDate, 0, 0, null, topLine);
                //    topLine.Lines.Add(rtLine);
                //}
                fmCPRRepaymentTaskLine subLine = FindTaskSubLine(topLine, request);
                if (subLine == null) {
                    fmCPRRepaymentTaskLine rtLine = new fmCPRRepaymentTaskLine(Session, 1, null, request, topLine.PaymentDate, 0, 0, null, topLine);   //, true);
                    topLine.Lines.Add(rtLine);

                    // Добавленние 3-его уровня разбивки по этапам
                    foreach (fmCPRPaymentRequestObligation elem in request.PaySettlmentOfObligations) {
                        fmCPRRepaymentTaskLine rtSubLine = new fmCPRRepaymentTaskLine(Session, 2, elem, request, rtLine.PaymentDate, 0, elem.Summ, null, rtLine);   //, false);
                        rtLine.Lines.Add(rtSubLine);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление заявки из RepaymentTaskLines
        /// </summary>
        /// <param name="request"></param>
        public void RemoveRequestFromLineStructure(fmCPRPaymentRequest request) {
            // Удаление из списка Lines
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                //var SubLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == topLine && rtl.PaymentRequest == request);
                //foreach (var subLine in SubLines) {
                //    topLine.Lines.Remove(subLine);
                //}

                fmCPRRepaymentTaskLine subLine = FindTaskSubLine(topLine, request);
                if (subLine != null) {
                    // Удаление 3-го уровня
                    while (subLine.Lines.Count > 0) {
                        Decimal sumIn, sumOut, sumObligationIn, sumObligationOut, sumInAcc, sumOutAcc;
                        DateTime operationDate = subLine.Lines[0].PaymentDate;
                        CalculateCashFlowRegisterSumOnDateAndRequest(operationDate, request, out sumIn, out sumOut, out sumObligationIn, out sumObligationOut, out sumInAcc, out sumOutAcc);

                        // В CashFlow добавляется поправочная запись с отрицательными значениями, чтобы погасить стоимости, произощещие от удаляемой заявки
                        DeleteCFRegisterRecord(subLine.Lines[0], sumIn, sumOut, sumObligationIn, sumObligationOut, sumInAcc, sumOutAcc);
                        subLine.Lines.Remove(subLine.Lines[0]);
                    }

                    topLine.Lines.Remove(subLine);
                }
            }

            // Признак, что request не используется в списке Lines
            SetUsingSign(request, false);

            // Пересчёт сумм
            RecalculateAll();
        }

        /*
        private void SetAllUsingSign() {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                //var SubLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == topLine);
                //foreach (var subLine in SubLines) {
                //    SetUsingSign(subLine.PaymentRequest, true);
                //}

                XPCollection<fmCPRRepaymentTaskLine> SubLines = GettTaskSubLineCollection(topLine);
                foreach (var subLine in SubLines) {
                    SetUsingSign(subLine.PaymentRequest, true);
                }
                break;   // Так как заявки во всех узлах одни и те же, то достаточно выполнить процедуру установки для одного узла
            }
        }
        */

        private void SetAllUsingSign() {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                foreach (fmCPRRepaymentTaskLine subLine in topLine.Lines) {
                    SetUsingSign(subLine.PaymentRequest, true);
                }
                break;   // Так как заявки во всех узлах одни и те же, то достаточно выполнить процедуру установки для одного узла
            }
        }

        private void SetUsingSign(fmCPRPaymentRequest request, Boolean isUsed) {
            var paymentRequestListItems = AllRequests.Where(r => r.PaymentRequest == request);
            foreach (var item in paymentRequestListItems) {
                item.IsUsed = isUsed;
            }
        }

        /// <summary>
        /// Начальное заполнение списка "Дата, сумма, Заявка=null, Задача"
        /// </summary>
        public void FillRepaymentTaskLines() {
            FillTaskLinesByOperationJournal();
            FillTaskLinesByRepaymentJournal();
            //FillSumByRepaymentJournal();
            FillSumByCashFlowRegister();
            SetAllUsingSign();   // Установка признаков, что заявка находится в списке Lines
            RecalculateAll();
        }

        public void FillTaskLinesByOperationJournal() {
            OurParty = GetOurParty();

            // Записи из журнала операций
            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            var queryOperationJournals = from operationJournal in operationJournals
                                         where operationJournal.PaymentDocument == PaymentDocument
                                         group operationJournal by new {
                                             operationJournal.OperationDate
                                         }
                                             into registerGroup
                                             select new {
                                                 OperationDate = registerGroup.Key.OperationDate,
                                                 OperationSumIn = registerGroup.Sum(row => row.SumIn),
                                                 OperationSumOut = registerGroup.Sum(row => row.SumOut)
                                             };

            foreach (var operationJournal in queryOperationJournals) {
                Decimal sum = 0;
                //if (PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) {   // 2012-04-13 ??? Как тут сделать правильно ???
                if (PaymentDocument.PaymentReceiverRequisites.Party == OurParty) {   // 2012-04-13 ??? Как тут сделать правильно ???
                    sum = operationJournal.OperationSumIn;
                } else {
                    sum = operationJournal.OperationSumOut;
                }
                fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session, 0, null, null, operationJournal.OperationDate, sum, 0, this, null);   //, true);
                this.RepaymentTaskLines.Add(rtl);
            }
        }

        public void FillTaskLinesByRepaymentJournal() {
            // Добавление строк в fmCPRRepaymentTaskLine, которые порождены уже привязанными заявками, не имеющими отражения
            // в fmCPRRepaymentTaskLine, с датами, не отражёнными в fmCSAOperationJournal
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            var queryRegs = from repaymentJournal in repaymentJournals
                            where repaymentJournal.PaymentDocument == PaymentDocument
                            group repaymentJournals by
                                new {
                                    repaymentJournal.PaymentDate,
                                    repaymentJournal.PaymentRequest
                                } into registerGroup
                            select new {
                                registerGroup.Key.PaymentDate,
                                registerGroup.Key.PaymentRequest
                            };
                            //registerGroup;
            foreach (var item in queryRegs) {
                var taskLines = this.RepaymentTaskLines.Where(p => p.PaymentDate == item.PaymentDate);
                fmCPRRepaymentTaskLine parentLine = null;
                if (taskLines.Count() == 0) {
                    // Добавление записи с нулевой суммой (которая символизирует сумму из журнала операций)
                    fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session, 0, null, null, item.PaymentDate, 0, 0, this, null);   //, true);
                    this.RepaymentTaskLines.Add(rtl);
                    parentLine = rtl;
                } else {
                    parentLine = taskLines.First();
                }
                AddRequestToLineStructure(item.PaymentRequest);
                //fmCPRRepaymentTaskLine rtLine = new fmCPRRepaymentTaskLine(Session, item.Key.PaymentRequest, item.Key.PaymentDate, 0, item.Key.PaymentRequest.Summ, this, parentLine);
                //this.RepaymentTaskLines.Add(rtLine);
                //parentLine.RequestSum = parentLine.RequestSum + rtLine.RequestSum;
            }
        }

        /// <summary>
        /// Перевычисление всех сумм в корневых записях (суммирование по подчинённым записям)
        /// </summary>
        public void RecalculateAll() {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                foreach (fmCPRRepaymentTaskLine LineLevelOne in topLine.Lines) {
                    RecalculateAll(LineLevelOne);
                }
                RecalculateAll(topLine);
            }
        }

        /// <summary>
        /// Перевычисление всех сумм в корневой записи, заданной параметром parentLine
        /// </summary>
        public void RecalculateAll(fmCPRRepaymentTaskLine parentLine) {
            //Decimal sum = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == parentLine).Sum(rtl => rtl.RequestSum);
            //parentLine.RequestSum = sum;

            parentLine.RequestSum = GetSubLevekSum(parentLine);

            //Decimal sum = 0;
            //XPCollection<fmCPRRepaymentTaskLine> SubLines = GettTaskSubLineCollection(parentLine);
            //foreach (var subLine in SubLines) {
            //    sum += subLine.RequestSum;
            //}
            //parentLine.RequestSum = sum;
        }

        private Decimal GetSubLevekSum(fmCPRRepaymentTaskLine parentLine) {
            Decimal sum = 0;
            foreach (fmCPRRepaymentTaskLine subLine in parentLine.Lines) {
                sum += subLine.RequestSum;
            }
            return sum;
        }

        public void FillSumByRepaymentJournal() {
            OurParty = GetOurParty();

            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var topLine in topLines) {
                //var SubLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == topLine);
                //foreach (var subLine in SubLines) {
                //    fmCPRPaymentRequest request = subLine.PaymentRequest;
                //    XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
                //    Decimal repaymentSum = (from repaymentJournal in repaymentJournals
                //                            where repaymentJournal.PaymentRequest == request
                //                               && repaymentJournal.PaymentDocument == this.PaymentDocument
                //                               && repaymentJournal.PaymentDate == topLine.PaymentDate
                //                            select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) ? repaymentJournal.SumIn : repaymentJournal.SumOut).Sum();

                //    // ??? ПРАВИЛЬНО ЛИ БЕРЁТСЯ СУММА С ПРИМЕНЕНИЕМ INN и т.д. ???
                //}

                //XPCollection<fmCPRRepaymentTaskLine> SubLines = GettTaskSubLineCollection(topLine);
                //foreach (var subLine in SubLines) {
                foreach (fmCPRRepaymentTaskLine subLine in topLine.Lines) {
                    fmCPRPaymentRequest request = subLine.PaymentRequest;
                    XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
                    Decimal repaymentSum = (from repaymentJournal in repaymentJournals
                                            where repaymentJournal.PaymentRequest == request
                                               && repaymentJournal.PaymentDocument == this.PaymentDocument
                                               && repaymentJournal.PaymentDate == topLine.PaymentDate
                                            select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party == OurParty) ? repaymentJournal.SumIn : repaymentJournal.SumOut).Sum();
                                            //select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) ? repaymentJournal.SumIn : repaymentJournal.SumOut).Sum();
                    subLine.RequestSum = repaymentSum;
                    // ??? ПРАВИЛЬНО ЛИ БЕРЁТСЯ СУММА С ПРИМЕНЕНИЕМ INN и т.д. ???
                }
            }
        }

        public void FillSumByCashFlowRegister() {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var lineLevelZero in topLines) {
                foreach (fmCPRRepaymentTaskLine lineLevelOne in lineLevelZero.Lines) {
                    foreach (fmCPRRepaymentTaskLine lineLevelTwo in lineLevelOne.Lines) {
                        Decimal sumIn, sumOut, sumObligationIn, sumObligationOut, sumInAcc, sumOutAcc;
                        DateTime operationDate = lineLevelZero.PaymentDate;
                        Boolean isCashFlowRegister = CalculateCashFlowRegisterSumOnDate(operationDate, lineLevelTwo, out sumIn, out sumOut, out sumObligationIn, out sumObligationOut, out sumInAcc, out sumOutAcc);
                        lineLevelTwo.RequestSum = sumIn + sumOut;   // Все суммы неотрицательны. В валюте платежа ValutaPayment
                        //lineLevelTwo.IsCashFlowRegister = isCashFlowRegister;
                        if (isCashFlowRegister && !paymentRequestObligationList.Contains(lineLevelTwo.LevelObject)) {
                            paymentRequestObligationList.Add(lineLevelTwo.LevelObject);
                        }
                    }
                }
            }
        }

        /*
        public void SetIsCashFlowProperty() {
            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);
            foreach (var lineLevelZero in topLines) {
                lineLevelZero.IsCashFlowRegister = true;
                foreach (fmCPRRepaymentTaskLine lineLevelOne in lineLevelZero.Lines) {
                    lineLevelOne.IsCashFlowRegister = true;
                    foreach (fmCPRRepaymentTaskLine lineLevelTwo in lineLevelOne.Lines) {
                        lineLevelTwo.IsCashFlowRegister = true;
                    }
                }
            }
        }

        protected override void OnSaved() {
            SetIsCashFlowProperty();
            base.OnSaved();
        }
        */

        /// <summary>
        /// Привязать заявку
        /// </summary>
        /// <param name="request"></param>
        public void DoBindingRequest(fmCPRPaymentRequest request, Boolean WriteToRepaymentJournal, Decimal distributeSum) {
            if (GetRequestRestSum(request, crmBankAccount.GetValutaByBankAccount(Session, this.BankAccount)) > 0) {
                AddRequestToLineStructure(request);

                // Автоматическое распределение суммы
                DistributeRequestSum(request, WriteToRepaymentJournal, distributeSum);

                // Признак использованности
                SetUsingSign(request, true);
            }
        }

        /// <summary>
        /// Ручная привязка по кнопке
        /// </summary>
        /// <param name="request"></param>
        public void ManualBinding(fmCPRRepaymentTask.PaymentRequestListItem line) {
            // Если заявка уже есть в Lines, то ничего не делаем, пользватель сам вручную правит суммы.
            if (line.IsUsed) 
                return;
            
            // Если заявка Оплачена
            if (line.PaymentRequest.State == PaymentRequestStates.PAYED)
                return;

            // Добавление заявки
            DoBindingRequest(line.PaymentRequest, true, 0);
        }

        private void DistributeRequestSum(fmCPRPaymentRequest request, Boolean WriteToRepaymentJournal, Decimal distributeSum) {
            // Нереализованная часть суммы заявки в валюте платежа
            Decimal startSum = distributeSum;
            if (WriteToRepaymentJournal) {
                startSum = GetRequestRestSum(request, crmBankAccount.GetValutaByBankAccount(Session, this.BankAccount));
            }

            var topLines = from repaymentTaskLine in this.RepaymentTaskLines
                           where repaymentTaskLine.ParentLine == null
                           orderby repaymentTaskLine.PaymentDate ascending
                           select repaymentTaskLine;
            foreach (var topLine in topLines) {
                // Величина непокрытия текущего платёжного документа заявками
                Decimal diffSum = topLine.OperationJournalSum - topLine.RequestSum;
                if (diffSum > 0) {
                    Decimal newSum = Math.Min(diffSum, startSum);
                    fmCPRRepaymentTaskLine subLine = FindTaskSubLine(topLine, request);
                    if (subLine != null) {
                        // Распределение суммы subLine.RequestSum по записям 3-го уровня
                        if (request.Summ != 0) {
                            // Исходная сумма по 3-му уровню
                            Decimal totalLevel3 = 0;
                            foreach (fmCPRRepaymentTaskLine lineLevel3 in subLine.Lines) {
                                fmCPRPaymentRequestObligation pro = lineLevel3.LevelObject as fmCPRPaymentRequestObligation;
                                if (pro != null) {
                                    totalLevel3 += pro.Summ;
                                }
                            }
                            
                            foreach (fmCPRRepaymentTaskLine lineLevel3 in subLine.Lines) {
                                fmCPRPaymentRequestObligation pro = lineLevel3.LevelObject as fmCPRPaymentRequestObligation;
                                if (pro != null) {
                                    //Decimal proSum = Math.Round((pro.Summ / totalLevel3) * newSum, 4);   //subLine.RequestSum, 4);
                                    Decimal proSum = (pro.Summ / totalLevel3) * newSum;   //subLine.RequestSum, 4);
                                    lineLevel3.RequestSum = proSum;
                                }
                            }

                        }
                        //Decimal level2Sum = request.Summ;

                        // Разница между level2Sum и суммой по подуровню в результате ошибки округления
                        Decimal level3Sum = 0;
                        foreach (fmCPRRepaymentTaskLine lineLevel3 in subLine.Lines) {
                            fmCPRPaymentRequestObligation pro = lineLevel3.LevelObject as fmCPRPaymentRequestObligation;
                            if (pro != null) {
                                level3Sum += lineLevel3.RequestSum;
                            }
                        }

                        Decimal diff = level3Sum - newSum;
                        if (diff != 0) {
                            // Внесение поправки: нахождение самого большого по модулю значения и изменение этого значения
                            // (без помощи linq)
                            Decimal level3Max = 0;
                            foreach (fmCPRRepaymentTaskLine lineLevel3 in subLine.Lines) {
                                fmCPRPaymentRequestObligation pro = lineLevel3.LevelObject as fmCPRPaymentRequestObligation;
                                if (pro != null && Math.Abs(lineLevel3.RequestSum) > level3Max) {
                                    level3Max = Math.Abs(lineLevel3.RequestSum);
                                }
                            }

                            foreach (fmCPRRepaymentTaskLine lineLevel3 in subLine.Lines) {
                                fmCPRPaymentRequestObligation pro = lineLevel3.LevelObject as fmCPRPaymentRequestObligation;
                                if (pro != null && Math.Abs(lineLevel3.RequestSum) == level3Max) {
                                    lineLevel3.RequestSum -= diff;
                                }
                            }
                        }

                        //subLine.RequestSum = newSum;   // Назначение, а не добавление. т.к. заявка добавлена в Lines и сумма в ней поэтому == 0
                        startSum -= newSum;

                        if (WriteToRepaymentJournal) {
                            CreateRepaymentJournalRecord(this.PaymentDocument, request, newSum, BankAccount, this);
                        }
                        CreateCFRegister(topLine);

                        //// Если сумма заявки исчерпана, то ставим ей статус ОПЛАЧЕНА
                        //Decimal restSum = GetRequestRestSum(request, crmBankAccount.GetValutaByBankAccount(Session, this.BankAccount));
                        //if (restSum <= 0) {
                        //    request.State = PaymentRequestStates.PAYED;
                        //}

                        if (startSum <= 0) {
                            request.State = PaymentRequestStates.PAYED;
                            return;
                        }
                    }
                }
            }
        }

        public void CreateRepaymentJournalRecord(fmCDocRCB paymentDocument, fmCPRPaymentRequest paymentRequest, Decimal Sum, crmBankAccount BankAccount, fmCPRRepaymentTask RepaymentTask) {
            fmCPRRepaymentJurnal newRepaymentJournalRecord;  //= new fmCPRRepaymentJurnal(Session);
            //newRepaymentJournalRecord.BankAccount = BankAccount;
            //newRepaymentJournalRecord.PaymentDocument = paymentDocument;
            //newRepaymentJournalRecord.PaymentRequest = paymentRequest;
            //newRepaymentJournalRecord.PlaneFact = CRM.Contract.Analitic.PlaneFact.FACT;

            //newRepaymentJournalRecord.ValutaObligation = paymentRequest.Valuta;  // Валюта обязательств
            //newRepaymentJournalRecord.ValutaPayment = paymentRequest.PaymentValuta;   // Валюта платежа

            OurParty = GetOurParty();

            //if (OurParty.INN == paymentRequest.PartyPayReceiver.INN) {
            if (OurParty == paymentRequest.PartyPayReceiver) {
                newRepaymentJournalRecord = new fmCPRRepaymentJurnal(Session,
                    RepaymentTask, 
                    paymentDocument, 
                    paymentRequest, 
                    BankAccount, 
                    Sum,
                    /*Math.Round(Sum * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.ReceivedByPayerBankDate, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4),*/
                    Sum * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.ReceivedByPayerBankDate, paymentRequest.PaymentValuta, paymentRequest.Valuta),
                    /*
                    paymentRequest.Summ,
                    Math.Round(paymentRequest.Summ * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.ReceivedByPayerBankDate, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4),
                    */
                    0, 
                    0, 
                    paymentDocument.ReceivedByPayerBankDate, 
                    paymentRequest.PaymentValuta,
                    paymentRequest.Valuta,
                    CRM.Contract.Analitic.PlaneFact.FACT);

                //newRepaymentJournalRecord.SumIn = paymentRequest.Summ;
                //newRepaymentJournalRecord.PaymentDate = paymentDocument.ReceivedByPayerBankDate;
                //newRepaymentJournalRecord.SumObligationIn = Math.Round(paymentRequest.Summ * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.ReceivedByPayerBankDate, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4);
            }
            //if (OurParty.INN == paymentRequest.PartyPaySender.INN) {
            if (OurParty == paymentRequest.PartyPaySender) {
                newRepaymentJournalRecord = new fmCPRRepaymentJurnal(Session,
                    RepaymentTask, 
                    paymentDocument, 
                    paymentRequest, 
                    BankAccount,
                    0,
                    0,
                    /*
                    paymentRequest.Summ,
                    Math.Round(paymentRequest.Summ * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.DeductedFromPayerAccount, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4),
                    */
                    Sum,
                    /*Math.Round(Sum * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.DeductedFromPayerAccount, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4),*/
                    Sum * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.DeductedFromPayerAccount, paymentRequest.PaymentValuta, paymentRequest.Valuta),
                    paymentDocument.DeductedFromPayerAccount, 
                    paymentRequest.PaymentValuta, 
                    paymentRequest.Valuta,
                    CRM.Contract.Analitic.PlaneFact.FACT);

                //newRepaymentJournalRecord.SumOut = paymentRequest.Summ;
                //newRepaymentJournalRecord.PaymentDate = paymentDocument.DeductedFromPayerAccount;
                //newRepaymentJournalRecord.SumObligationOut = Math.Round(paymentRequest.Summ * csCNMValutaCourse.GetCrossCourceOnDate(Session, paymentDocument.DeductedFromPayerAccount, paymentRequest.PaymentValuta, paymentRequest.Valuta), 4);
            }
        }

        private void CreateCFRegister(fmCPRRepaymentTaskLine topLine) {
            OurParty = GetOurParty();
        
            foreach (fmCPRRepaymentTaskLine LineLevelOne in topLine.Lines) {
                foreach (fmCPRRepaymentTaskLine LineLevelTwo in LineLevelOne.Lines) {
                    CreateCFRegisterRecord(LineLevelTwo, OurParty);
                }
            }
        }

        private void CreateCFRegisterRecord(fmCPRRepaymentTaskLine line, crmCParty ourParty) {
            //if (line.IsCashFlowRegister)
            //    return;
            if (paymentRequestObligationList.Contains(line.LevelObject)) {
                return;
            }

            crmCashFlowRegister cfr = new crmCashFlowRegister(Session);

            Guid token = line.PaymentRequest.Oid;
            cfr.Token = token;
            cfr.Section = CashFlowRegisterSection.REPAYMENT_JOURNAL;

            cfr.SourceGUID = this.Oid;
            cfr.SourceType = this.GetType();

            cfr.PaymentDocument = this.PaymentDocument;
            cfr.BankAccount = this.BankAccount;
            cfr.Bank = this.BankAccount.Bank;
            cfr.OperationDate = this.PaymentDocument.GetAccountDateChange();

            fmCPRPaymentRequestContract paymentRequestContract = line.PaymentRequest as fmCPRPaymentRequestContract;
            //if (paymentRequestContract != null && paymentRequestContract.ContractDeal != null) {
            //    cfr.Contract = paymentRequestContract.ContractDeal.Contract;
            //}
            if (paymentRequestContract != null) {
                cfr.Contract = paymentRequestContract.Contract;
                cfr.ContractDeal = paymentRequestContract.ContractDeal;
            }

            cfr.fmOrder = line.Order;
            cfr.CostItem = line.CostItem;
            cfr.Subject = (line.Order != null) ? line.Order.Subject : null;

            cfr.PrimaryParty = line.PaymentRequest.PartyPaySender;
            cfr.ContragentParty = line.PaymentRequest.PartyPayReceiver;

            //cfr.ObligationUnit = 
            //cfr.PaymentItem = 

            cfr.ValutaPayment = this.PaymentDocument.GetAccountValuta();
            cfr.ValutaObligation = line.LevelObject.Valuta;

            // В валюте платежа
            //cfr.SumIn = this.PaymentDocument.GetSumIn(this.BankAccount);
            //cfr.SumOut = this.PaymentDocument.GetSumOut(this.BankAccount);

            if (this.PaymentDocument.PaymentReceiverRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumIn = line.RequestSum; //this.PaymentDocument.PaymentCost;
            }
            if (this.PaymentDocument.PaymentPayerRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                cfr.SumOut = line.RequestSum; //this.PaymentDocument.PaymentCost;
            }

            // В валюте обязательств
            if (this.PaymentDocument.PaymentReceiverRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentReceiverRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumObligationIn = Math.Round(cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation), 4);   //line.RequestSum;
                cfr.SumObligationIn = cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation);   //line.RequestSum;
            }
            if (this.PaymentDocument.PaymentPayerRequisites.INN == ourParty.INN) {   // && this.PaymentDocument.PaymentPayerRequisites.StatementOfAccount.BankAccount == this.BankAccount) {
                //cfr.SumObligationOut = Math.Round(cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation), 4);   //line.RequestSum;
                cfr.SumObligationOut = cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, cfr.ValutaObligation);   //line.RequestSum;
            }

            // В рублях
            try {
                //cfr.SumInAcc = Math.Round(cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode("RUB")), 4);
                //cfr.SumOutAcc = Math.Round(cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode("RUB")), 4);

                cfr.SumInAcc = cfr.SumIn * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode("RUB"));
                cfr.SumOutAcc = cfr.SumOut * csCNMValutaCourse.GetCrossCourceOnDate(Session, this.PaymentDocument.GetAccountDateChange(), cfr.ValutaPayment, GetValutaByCode("RUB"));
            }
            catch {
            }

            //if (this.PaymentReceiverRequisites.INN == OurParty.INN && this.PaymentReceiverRequisites.StatementOfAccount.BankAccount == bankAccount) {
            //    Res = this.PaymentCost;
            //}

            cfr.PaymentRequestObligationGUID = line.LevelObject.Oid;

            //line.IsCashFlowRegister = true;
            if (!paymentRequestObligationList.Contains(line.LevelObject)) {
                paymentRequestObligationList.Add(line.LevelObject);
            }
        }

        private void DeleteCFRegisterRecord(fmCPRRepaymentTaskLine line, Decimal AntiSumIn, Decimal AntiSumOut, Decimal AntiSumObligationIn, Decimal AntiSumObligationOut, Decimal AntiSumInAcc, Decimal AntiSumOutAcc) {

            crmCashFlowRegister cfr = new crmCashFlowRegister(Session);

            Guid token = line.PaymentRequest.Oid;
            cfr.Token = token;
            cfr.Section = CashFlowRegisterSection.REPAYMENT_JOURNAL;

            cfr.SourceGUID = this.Oid;
            cfr.SourceType = this.GetType();

            cfr.PaymentDocument = this.PaymentDocument;
            cfr.BankAccount = this.BankAccount;
            cfr.Bank = this.BankAccount.Bank;
            cfr.OperationDate = this.PaymentDocument.GetAccountDateChange();

            fmCPRPaymentRequestContract paymentRequestContract = line.PaymentRequest as fmCPRPaymentRequestContract;
            //if (paymentRequestContract != null && paymentRequestContract.ContractDeal != null) {
            //    cfr.Contract = paymentRequestContract.ContractDeal.Contract;
            //}
            if (paymentRequestContract != null) {
                cfr.Contract = paymentRequestContract.Contract;
                cfr.ContractDeal = paymentRequestContract.ContractDeal;
            }

            cfr.fmOrder = line.Order;
            cfr.CostItem = line.CostItem;
            cfr.Subject = (line.Order != null) ? line.Order.Subject : null;

            cfr.PrimaryParty = line.PaymentRequest.PartyPaySender;
            cfr.ContragentParty = line.PaymentRequest.PartyPayReceiver;

            //cfr.ObligationUnit = 
            //cfr.PaymentItem = 

            cfr.ValutaPayment = this.PaymentDocument.GetAccountValuta();
            cfr.ValutaObligation = line.LevelObject.Valuta;

            // В валюте платежа
            //cfr.SumIn = this.PaymentDocument.GetSumIn(this.BankAccount);
            //cfr.SumOut = this.PaymentDocument.GetSumOut(this.BankAccount);

            cfr.SumIn = -AntiSumIn;
            cfr.SumOut = -AntiSumOut;

            // В валюте обязательств
            cfr.SumObligationIn = -AntiSumObligationIn;
            cfr.SumObligationOut = -AntiSumObligationOut;

            // В рублях
            cfr.SumInAcc = -AntiSumInAcc;
            cfr.SumOutAcc = -AntiSumOutAcc;

            //if (this.PaymentReceiverRequisites.INN == OurParty.INN && this.PaymentReceiverRequisites.StatementOfAccount.BankAccount == bankAccount) {
            //    Res = this.PaymentCost;
            //}
        }

        private csValuta GetValutaByCode(string code) {
            XPQuery<csValuta> valutas = new XPQuery<csValuta>(Session);
            csValuta Valuta = (from valuta in valutas
                                  where valuta.Code.ToUpper() == code.ToUpper()
                                  select valuta).FirstOrDefault();
            return Valuta;
        }

        private fmCPRRepaymentTaskLine FindTaskSubLine(fmCPRRepaymentTaskLine parent, fmCPRPaymentRequest request) {
            fmCPRRepaymentTaskLine taskLine = null;
            CriteriaOperator criteria = CriteriaOperator.And(new BinaryOperator(new OperandProperty("ParentLine"), new ConstantValue(parent), BinaryOperatorType.Equal),
                                                             new BinaryOperator(new OperandProperty("PaymentRequest"), new ConstantValue(request), BinaryOperatorType.Equal));
            taskLine = Session.FindObject<fmCPRRepaymentTaskLine>(PersistentCriteriaEvaluationBehavior.InTransaction, criteria);
            return taskLine;
        }

        /*
        private XPCollection<fmCPRRepaymentTaskLine> GettTaskSubLineCollection(fmCPRRepaymentTaskLine parent) {
            CriteriaOperator criteria = CriteriaOperator.And(new BinaryOperator(new OperandProperty("ParentLine"), new ConstantValue(parent), BinaryOperatorType.Equal));
            return new XPCollection<fmCPRRepaymentTaskLine>(PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, criteria);
        }
        */

        /// <summary>
        /// Неиспользованный остаток заявки, приведённый к валюте valuta по (кросс-)курсу на дату изменения счёта
        /// </summary>
        /// <param name="request"></param>
        /// <param name="valuta"></param>
        /// <returns></returns>
        private Decimal GetRequestRestSum(fmCPRPaymentRequest request, csValuta valuta) {
            /*
            // Заглушка: request.PaymentSumm
            Decimal sumInObligationValuta = request.Summ - request.PaymentSumm;   // В валюте обязательств
            DateTime AccountChangedDate = this.PaymentDocument.ReceivedByPayerBankDate;
            if (AccountChangedDate == DateTime.MinValue) {
                AccountChangedDate = this.PaymentDocument.DeductedFromPayerAccount;
            }
            Decimal sumInPaymentValuta = Math.Round(sumInObligationValuta * csCNMValutaCourse.GetCrossCourceOnDate(Session, AccountChangedDate, request.Valuta, valuta), 4);
            return sumInPaymentValuta;
            */


            DateTime AccountChangedDate = this.PaymentDocument.ReceivedByPayerBankDate.Date;
            if (AccountChangedDate == DateTime.MinValue) AccountChangedDate = this.PaymentDocument.DeductedFromPayerAccount.Date;
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            //Decimal repaymentSum = (from repaymentJournal in repaymentJournals
            //                        where repaymentJournal.PaymentRequest == request
            //                        select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party == OurParty) ? repaymentJournal.SumObligationIn : repaymentJournal.SumObligationOut).Sum();
            //Decimal repaymentSumIn = 0;
            //Decimal repaymentSumOut = 0;
            Decimal sumIn = 0;
            Decimal sumOut = 0;
            Decimal sumObligationIn = 0;
            Decimal sumObligationOut = 0;
            foreach (fmCPRRepaymentJurnal repaymentJournal in repaymentJournals.Where(rj => rj.PaymentRequest == request)) {
                //if (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party == OurParty)
                //    repaymentSumIn = repaymentSumIn + repaymentJournal.SumObligationIn;
                //else
                //    repaymentSumOut = repaymentSumOut + repaymentJournal.SumObligationOut;
                sumIn = sumIn + repaymentJournal.SumIn;
                sumOut = sumOut + repaymentJournal.SumOut;
                // По меньшей мере, одна из сумм sumObligationIn или sumObligationOut равна нулю
                sumObligationIn = sumObligationIn + repaymentJournal.SumObligationIn;
                sumObligationOut = sumObligationOut + repaymentJournal.SumObligationOut;
            }
            //Decimal repaymentSumIn = (from repaymentJournal in repaymentJournals
            //                        where (repaymentJournal.PaymentRequest == request) &&
            //                              (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party == OurParty) 
            //                        select repaymentJournal.SumObligationIn).Sum();
            //Decimal repaymentSumOut = (from repaymentJournal in repaymentJournals
            //                          where (repaymentJournal.PaymentRequest == request) &&
            //                                (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party != OurParty)
            //                          select repaymentJournal.SumObligationOut).Sum();
//            Decimal repaymentSum = repaymentSumIn + repaymentSumOut;
            Decimal sumInObligationValuta = request.Summ - (sumObligationIn + sumObligationOut);
            //Decimal sumInPaymentValuta = Math.Round(sumInObligationValuta * csCNMValutaCourse.GetCrossCourceOnDate(Session, AccountChangedDate, request.Valuta, valuta), 4);
            Decimal sumInPaymentValuta = sumInObligationValuta * csCNMValutaCourse.GetCrossCourceOnDate(Session, AccountChangedDate, request.Valuta, valuta);
            return sumInPaymentValuta;
        }

        /// <summary>
        /// Неиспользованный остаток заявки, приведённый в валюте обязательств
        /// </summary>
        /// <param name="request"></param>
        /// <param name="valuta"></param>
        /// <returns></returns>
        private Decimal GetRequestRestSum(fmCPRPaymentRequest request) {
            // Заглушка: request.PaymentSumm
            Decimal sumInObligationValuta = request.Summ - request.PaymentSumm;   // В валюте обязательств
            return sumInObligationValuta;
        }

        /*
        /// <summary>
        /// Удаление зявки из привязки. В журнале привязок вычисляется сумма этой заявки и добавляется запись с 
        /// суммой, обратной к вычисленной
        /// </summary>
        /// <param name="request"></param>
        public void RemoveRequest(fmCPRPaymentRequest request) {

            Decimal startSum = request.Summ;
            XPQuery<fmCPRRepaymentTaskLine> repaymentTaskLines = new XPQuery<fmCPRRepaymentTaskLine>(this.Session, true);
            var queryRepaymentTaskLines = from repaymentTaskLine in repaymentTaskLines
                                          where repaymentTaskLine.PaymentRequest == null
                                             && repaymentTaskLine.RepaymentTask == this
                                             && repaymentTaskLine.RequestSum > 0
                                          orderby repaymentTaskLine.PaymentDate
                                          select repaymentTaskLine;
            foreach (var repaymentTaskLine in queryRepaymentTaskLines) {
                // Подсчёт покрытия текущего платежа заявками
                Decimal coverSum = (from repaymentTaskLineCover in repaymentTaskLines
                                    where repaymentTaskLineCover.PaymentRequest != null
                                       && repaymentTaskLineCover.RepaymentTask == this
                                       && repaymentTaskLineCover.PaymentDate == repaymentTaskLine.PaymentDate
                                    select repaymentTaskLineCover.RequestSum).Sum();
                Decimal diffSum = repaymentTaskLine.RequestSum - coverSum;
                Decimal newSum = Math.Min(diffSum, startSum);
                if (diffSum > 0) {
                    // Создание записи на разность
                    fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session, request, repaymentTaskLine.PaymentDate, 0, newSum, this, null);
                    startSum -= newSum;
                    if (startSum <= 0) return;
                }
            }
        }
        */
        
        private crmCParty GetOurParty() {
            if (OurParty != null)
                return OurParty;
            // Наша организация
            crmCParty pOurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    pOurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            return pOurParty;
        }

        public void ClearRequestCollection() {
            //_AllRequests = null;
        }

        /// <summary>
        /// Закрыть финансовую заявку
        /// </summary>
        public void CloseUnknownPayment() {
            if (this.State != RepaymentTaskStates.UNKNOWN)
                return;

            OurParty = GetOurParty();

            var topLines = this.RepaymentTaskLines.Where(rtl => rtl.ParentLine == null);

            // Условием закрытия задачи является тот факт, что сумма платёжного документа покрыта заявками
            // Напоминание: this.OperationRegisterSum - сумма заявки из операционного журнала
            Decimal bindingSum = topLines.Sum(e => e.RequestSum);
            Decimal sumDoc = Math.Abs(this.OperationRegisterSum - bindingSum);

            if (Decimal.Compare(Math.Abs(sumDoc), _Accuracy) > 0) {
                // Сумма платёжного документа не покрыта заявками
                return;
            }

            // Перебор заявок, прикреплённых к платёжному документу. Если сумма заявки полностью погашена, то ей
            // присваивается статус Payed. Погашенность заявки проверяется по базе и по транзакциям.
            // Вычисление ведётся в валюте обязательств
            foreach (var topLine in topLines) {
                //XPCollection<fmCPRRepaymentTaskLine> SubLines = GettTaskSubLineCollection(topLine);
                //foreach (var subLine in SubLines) {
                foreach (fmCPRRepaymentTaskLine subLine in topLine.Lines) {
                    fmCPRPaymentRequest request = subLine.PaymentRequest;
                    
                    //Decimal restSum = GetRequestRestSum(request);   // В валюте обязательств
                    //if (Decimal.Compare(Math.Abs(restSum), _Accuracy) <= 0) {
                    //    request.State = PaymentRequestStates.PAYED;
                    //}

                    XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
                    Decimal repaymentSum = (from repaymentJournal in repaymentJournals
                                            where repaymentJournal.PaymentRequest == request
                                            //&& repaymentJournal.PaymentDocument == this.PaymentDocument
                                            //&& repaymentJournal.PaymentDate == topLine.PaymentDate
                                            //select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.INN == OurParty.INN) ? repaymentJournal.SumIn : repaymentJournal.SumOut).Sum();
                                            select (repaymentJournal.PaymentDocument.PaymentReceiverRequisites.Party == OurParty) ? repaymentJournal.SumObligationIn : repaymentJournal.SumObligationOut).Sum();

                    if (Decimal.Compare(Math.Abs(request.Summ) - Math.Abs(repaymentSum), _Accuracy) <= 0) {
                        request.State = PaymentRequestStates.PAYED;
                    }
                }
                break;    // Все нужные заявки перечислены уже в 1-м попавшемся узле.
            }

            this.State = RepaymentTaskStates.CLOSED;
            this.PaymentDocument.State = PaymentDocProcessingStates.PROCESSED;
        }

        #endregion

        #region Классы

        [NonPersistent]
        public class PaymentRequestListItem : BaseObject {

            public PaymentRequestListItem(Session session)
                : base(session) {
            }

            //public PaymentRequestListItem(Session session, fmCPRPaymentRequest request, fmCPRRepaymentTask repaymentTask, Int16 priority, Boolean isUsed)
            public PaymentRequestListItem(Session session, fmCPRPaymentRequest request, fmCPRRepaymentTask repaymentTask, Boolean isUsed)
                : base(session) {
                    _PaymentRequest = request;
                    _RepaymentTask = repaymentTask;
                    //_Priority = priority;
                    _IsUsed = isUsed;
            }

            private fmCPRPaymentRequest _PaymentRequest;
            private fmCPRRepaymentTask _RepaymentTask;
            private Int16 _Priority = -1;
            private Boolean _IsUsed;

            /// <summary>
            /// Ссылка на заявку
            /// </summary>
            [ExpandObjectMembers(ExpandObjectMembers.InListView)]
            public fmCPRPaymentRequest PaymentRequest {
                get { return _PaymentRequest; }
                set {
                    SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
                }
            }

            /// <summary>
            /// Ссылка на задачу 
            /// </summary>
            [Browsable(false)]
            public fmCPRRepaymentTask RepaymentTask {
                get { return _RepaymentTask; }
                set {
                    SetPropertyValue<fmCPRRepaymentTask>("RepaymentTask", ref _RepaymentTask, value);
                }
            }

            /// <summary>
            /// Степень соответствия заявки платёжному документу по 5-ти бальной шкале (4 - полное соответствие, 0 - наименьшее соотвествтие)
            /// </summary>
            [Appearance("fmCPRRepaymentTask.PaymentRequestListItem.Priority.BackColor.1", BackColor = "240, 255, 240", Criteria = "Priority==1")]
            [Appearance("fmCPRRepaymentTask.PaymentRequestListItem.Priority.BackColor.2", BackColor = "225, 240, 225", Criteria = "Priority==2")]
            [Appearance("fmCPRRepaymentTask.PaymentRequestListItem.Priority.BackColor.3", BackColor = "200, 225, 200", Criteria = "Priority==3")]
            [Appearance("fmCPRRepaymentTask.PaymentRequestListItem.Priority.BackColor.4", BackColor = "180, 200, 180", Criteria = "Priority==4")]
            [Appearance("fmCPRRepaymentTask.PaymentRequestListItem.Priority.BackColor.5", BackColor = "150, 180, 150", Criteria = "Priority==5")]
            public Int16 Priority {
                get {
                    if (!IsLoading) {
                        AssignPriority();
                    }
                    return _Priority;
                }
                //set {
                //    SetPropertyValue<Int16>("Priority", ref _Priority, value);
                //}
            }

            /// <summary>
            /// Признак того, что заявка уже находится в списке Lines
            /// </summary>
            public Boolean IsUsed {
                get {
                    return _IsUsed;
                }
                set {
                    SetPropertyValue<Boolean>("IsUsed", ref _IsUsed, value);
                }
            }
 
            #region МЕТОДЫ

            private void AssignPriority() {
                if (_Priority != 0)
                    return;
                //_Priority = 5;
                //return;
                if ((PaymentRequest.PayDate.Date == RepaymentTask.PaymentDocument.DeductedFromPayerAccount.Date || PaymentRequest.PayDate.Date == RepaymentTask.PaymentDocument.ReceivedByPayerBankDate.Date)) {
                    _Priority++;
                }
                if (PaymentRequest.PaymentSumm == RepaymentTask.PaymentDocument.PaymentCost) {
                    _Priority++;
                }
                if (PaymentRequest.Number == RepaymentTask.PaymentDocument.DocNumber) {
                    _Priority++;
                }
                if (PaymentRequest.PartyPayReceiver == RepaymentTask.PaymentDocument.PaymentReceiverRequisites.Party) {
                    _Priority++;
                }
                if (PaymentRequest.PartyPaySender == RepaymentTask.PaymentDocument.PaymentPayerRequisites.Party) {
                    _Priority++;
                }
                // Хотел соответствие по банковскому счёту, но такое не предусмотрено в заявке.
                //if (PaymentRequest. == RepaymentTask.PaymentDocument.PaymentPayerRequisitess.Party) {
                //    Res++;
                //}
                //return _Priority;
            }

            #endregion

        }

        #endregion
    }

}
