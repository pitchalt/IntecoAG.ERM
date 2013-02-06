using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.Base;
//
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Analitic;
//
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.ReportHelper {

    [NavigationItem("Money")]
    [NonPersistent]
    public class fmCRHPayedRequestReportParameters : ReportParametersObjectBase {
        public fmCRHPayedRequestReportParameters(Session session) : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();

            CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
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

        private XPQuery<crmCashFlowRegister> CashFlowRegisters;

        // Дата отчёта
        private DateTime _ReportDateStart;
        //[Custom("Caption", "Начальная дата")]
        [Custom("Caption", "Дата")]
        public DateTime ReportDateStart {
            get {
                return _ReportDateStart;
            }
            set {
                DateTime oldReportDateStart = _ReportDateStart;
                //_ReportDateStart = value;
                SetPropertyValue<DateTime>("ReportDateStart", ref _ReportDateStart, value);
                if (!IsLoading && _ReportDateStart != oldReportDateStart) {
                    CostItem = null;
                    Order = null;
                    PrimaryParty = null;
                    ContragentParty = null;
                    Subject = null;
                    Bank = null;
                    BankAccount = null;
                    //OnChanged("CostItem");
                    //OnChanged("Order");
                    //OnChanged("PrimaryParty");
                    //OnChanged("ContragentParty");
                    //OnChanged("Subject");
                    //OnChanged("Bank");
                    //OnChanged("BankAccount");
                    ReportDate = value;
                }
            }
        }

        // Дата отчёта
        private DateTime _ReportDate;
        [Browsable(false)]
        [Custom("Caption", "Конечная дата")]
        public DateTime ReportDate {
            get { return _ReportDate; }
            set {
                /*DateTime oldReportDate = _ReportDate;*/
                //_ReportDate = value;
                SetPropertyValue<DateTime>("ReportDate", ref _ReportDate, value);
                /*
                if (!IsLoading && _ReportDate != oldReportDate) {
                    CostItem = null;
                    Order = null;
                    PrimaryParty = null;
                    ContragentParty = null;
                    Subject = null;
                    Bank = null;
                    BankAccount = null;
                    //OnChanged("CostItem");
                    //OnChanged("Order");
                    //OnChanged("PrimaryParty");
                    //OnChanged("ContragentParty");
                    //OnChanged("Subject");
                    //OnChanged("Bank");
                    //OnChanged("BankAccount");
                }
                */
            }
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

        // Прочие поля фильтра
        //private crmContract _Contract;
        //private crmContractDeal _ContractDeal;


        #region fmOrders

        private fmCOrder _Order;
        [Custom("Caption", "Заказ")]
        [DataSourceProperty("Orders")]
        public fmCOrder Order {
            get {
                return _Order;
            }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
            }
        }

        private XPCollection<fmCOrder> Orders {
            get {
                //XPQuery<fmCOrder> orders = new XPQuery<fmCOrder>(Session);
                //var queryOrders = from order in orders
                //                  select order;

                XPCollection<fmCOrder> orderCol = new XPCollection<fmCOrder>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var queryOrders = (from cashFlowRecord in CashFlowRegisters
                                              where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                                 && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                              select cashFlowRecord.fmOrder).Distinct().ToList();

                foreach (var order in queryOrders) {
                    orderCol.Add(order);
                }
                return orderCol;
            }
        }

        #endregion

        
        #region fmCostItem

        private fmCostItem _CostItem;
        [Custom("Caption", "Статья ДДС")]
        [DataSourceProperty("CostItems")]
        public fmCostItem CostItem {
            get {
                return _CostItem;
            }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        private XPCollection<fmCostItem> CostItems {
            get {
                //XPQuery<fmCostItem> costItems = new XPQuery<fmCostItem>(Session);
                //var queryCostItems = from costItem in costItems
                //                     select costItem;

                XPCollection<fmCostItem> costItemCol = new XPCollection<fmCostItem>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var queryCostItems = (from cashFlowRecord in CashFlowRegisters
                                   where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                      && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                   select cashFlowRecord.CostItem).Distinct().ToList();

                foreach (var costItem in queryCostItems) {
                    costItemCol.Add(costItem);
                }
                return costItemCol;
            }
        }

        #endregion


        #region fmCSubject

        private fmCSubject _Subject;
        [Custom("Caption", "Тема")]
        [DataSourceProperty("Subjects")]
        public fmCSubject Subject {
            get {
                return _Subject;
            }
            set {
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
            }
        }

        private XPCollection<fmCSubject> Subjects {
            get {
                //XPQuery<fmCSubject> subjects = new XPQuery<fmCSubject>(Session);
                //var querySubjects = from subject in subjects
                //                    select subject;

                XPCollection<fmCSubject> subjectCol = new XPCollection<fmCSubject>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var querySubjects = (from cashFlowRecord in CashFlowRegisters
                                      where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                         && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                      select cashFlowRecord.Subject).Distinct().ToList();

                foreach (var subject in querySubjects) {
                    subjectCol.Add(subject);
                }
                return subjectCol;
            }
        }

        #endregion


        #region crmCParty: PrimaryParty

        private crmCParty _PrimaryParty;
        [Custom("Caption", "Заказчик")]
        [DataSourceProperty("PrimaryPartys")]
        public crmCParty PrimaryParty {
            get {
                return _PrimaryParty;
            }
            set {
                SetPropertyValue<crmCParty>("PrimaryParty", ref _PrimaryParty, value);
            }
        }

        private XPCollection<crmCParty> PrimaryPartys {
            get {
                //XPQuery<crmCParty> primaryPartys = new XPQuery<crmCParty>(Session);
                //var queryPrimaryPartys = from primaryParty in primaryPartys
                //                    select primaryParty;

                XPCollection<crmCParty> primaryPartyCol = new XPCollection<crmCParty>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var queryPrimaryPartys = (from cashFlowRecord in CashFlowRegisters
                                     where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                        && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                     select cashFlowRecord.PrimaryParty).Distinct().ToList();

                foreach (var primaryParty in queryPrimaryPartys) {
                    primaryPartyCol.Add(primaryParty);
                }
                return primaryPartyCol;
            }
        }

        #endregion


        #region crmCParty: ContragentParty

        private crmCParty _ContragentParty;
        [Custom("Caption", "Исполнитель")]
        [DataSourceProperty("ContragentPartys")]
        public crmCParty ContragentParty {
            get {
                return _ContragentParty;
            }
            set {
                SetPropertyValue<crmCParty>("ContragentParty", ref _ContragentParty, value);
            }
        }

        private XPCollection<crmCParty> ContragentPartys {
            get {
                //XPQuery<crmCParty> contragentPartys = new XPQuery<crmCParty>(Session);
                //var queryContragentPartys = from contragentParty in contragentPartys
                //                         select contragentParty;

                XPCollection<crmCParty> contragentPartyCol = new XPCollection<crmCParty>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var queryContragentPartys = (from cashFlowRecord in CashFlowRegisters
                                          where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                             && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                          select cashFlowRecord.ContragentParty).Distinct().ToList();

                foreach (var contragentParty in queryContragentPartys) {
                    contragentPartyCol.Add(contragentParty);
                }
                return contragentPartyCol;
            }
        }

        #endregion


        //private csValuta _ValutaPayment;
        //private csValuta _ValutaObligation;

        #region Banks
        private crmBank _Bank;
        [Custom("Caption", "Банк")]
        [DataSourceProperty("Banks")]
        public crmBank Bank {
            get {
                return _Bank;
            }
            set {
                crmBank oldBank = _Bank;
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
                if (!IsLoading && _Bank != oldBank) {
                    BankAccount = null;
                    OnChanged("BankAccount");
                }
            }
        }

        private XPCollection<crmBank> Banks {
            get {
                //XPQuery<crmBank> banks = new XPQuery<crmBank>(Session);
                //var queryBanks = from bank in banks
                //                 select bank;

                XPCollection<crmBank> bankCol = new XPCollection<crmBank>(Session, false);

                //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                var queryBanks = (from cashFlowRecord in CashFlowRegisters
                                             where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                                && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                             select cashFlowRecord.Bank).Distinct().ToList();

                foreach (var bank in queryBanks) {
                    bankCol.Add(bank);
                }
                return bankCol;
            }
        }
        #endregion

        #region BankAccounts
        private crmBankAccount _BankAccount;

        /// <summary>
        /// Счета в этом банке 
        /// </summary>
        [Custom("Caption", "Счёт")]
        [DataSourceProperty("BankAccounts")]
        public crmBankAccount BankAccount {
            get {
                return _BankAccount;
            }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        private XPCollection<crmBankAccount> BankAccounts {
            get {
                XPCollection<crmBankAccount> bankAccountCol = new XPCollection<crmBankAccount>(Session, false);
                if (!IsLoading) {
                    //XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(Session);
                    //var queryBankAccounts = from bankAccount in bankAccounts
                    //                        where bankAccount.Bank == this.Bank
                    //                        select bankAccount;

                    //XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
                    var queryBankAccounts = (from cashFlowRecord in CashFlowRegisters
                                      where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date
                                         && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                         && cashFlowRecord.BankAccount.Bank == this.Bank
                                      select cashFlowRecord.BankAccount).Distinct().ToList();

                    foreach (var bankAccount in queryBankAccounts) {
                        bankAccountCol.Add(bankAccount);
                    }
                }
                return bankAccountCol;
            }
        }
        #endregion

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Генерация записей отчёта по указанным условиям
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="ReportDate"></param>
        public List<fmCRHPayedRequestNonPersistent> GenerateReportContent() {

            /*
            // Из fmCPRRegistrator
                    this.InvoiceDate = value.ExtDocDate;
                    this.InvoiceNumber = value.ExtDocNumber;
                    this.Valuta = value.Valuta;
                    this.PaymentValuta = value.PaymentValuta;
                    Receiver PaymentRequest.PartyPayReceiver;

            */

            List<fmCRHPayedRequestNonPersistent> Res = new List<fmCRHPayedRequestNonPersistent>();
            //crmCParty ourParty = GetOurParty(Session);

            XPQuery<crmCashFlowRegister> CashFlowRegisters = new XPQuery<crmCashFlowRegister>(Session);
            XPQuery<fmCPRPaymentRequestObligation> PaymentRequestObligations = new XPQuery<fmCPRPaymentRequestObligation>(Session);
            XPQuery<fmCPRRegistrator> PaymentRegistrators = new XPQuery<fmCPRRegistrator>(Session);

            var queryCashFlowRegisters = (from cashFlowRecord in CashFlowRegisters
                                          where (cashFlowRecord.OperationDate.Date >= this.ReportDateStart.Date 
                                             && cashFlowRecord.OperationDate.Date < this.ReportDate.AddDays(1).Date)
                                             && (this.Bank == null || cashFlowRecord.Bank == this.Bank)
                                             && (this.BankAccount == null || cashFlowRecord.BankAccount == this.BankAccount)
                                             && (this.ContragentParty == null || cashFlowRecord.ContragentParty == this.ContragentParty)
                                             && (this.PrimaryParty == null || cashFlowRecord.PrimaryParty == this.PrimaryParty)
                                             && (this.CostItem == null || cashFlowRecord.CostItem == this.CostItem)
                                             && (this.Order == null || cashFlowRecord.fmOrder == this.Order)
                                             && (this.Subject == null || cashFlowRecord.Subject == this.Subject)
                                             && (cashFlowRecord.SumOut >= 0)
                                          select cashFlowRecord).Distinct().ToList();
            foreach (var cashFlowRecord in queryCashFlowRegisters) {
                var paymentRequestObligations = (from pro in PaymentRequestObligations
                                                      where pro.Oid == cashFlowRecord.PaymentRequestObligationGUID
                                                         && pro.PaymentRequestBase.State == PaymentRequestStates.PAYED
                                                      select pro).FirstOrDefault();
                if (paymentRequestObligations != null) {
                    fmCRHPayedRequestNonPersistent payedRequest = new fmCRHPayedRequestNonPersistent(Session);
                    payedRequest.ReportDateStart = this.ReportDateStart;
                    payedRequest.ReportDate = this.ReportDate;
                    //payedRequest.CashFlowRecord = cashFlowRecord;

                    payedRequest.PaymentRequestObligation = paymentRequestObligations;

                    payedRequest.ValutaByObligation = cashFlowRecord.ValutaObligation;
                    payedRequest.SumByObligation = cashFlowRecord.SumObligationOut;
                    payedRequest.SumByFact = cashFlowRecord.SumOut;

                    payedRequest.SumObligationOut = cashFlowRecord.SumObligationOut;
                    payedRequest.SumOut = cashFlowRecord.SumOut;

                    payedRequest.PaymentDocument = cashFlowRecord.PaymentDocument;
                    payedRequest.OperationDate = cashFlowRecord.OperationDate;
                    payedRequest.BankAccount = cashFlowRecord.BankAccount;
                    payedRequest.fmOrder = cashFlowRecord.fmOrder;
                    payedRequest.CostItem = cashFlowRecord.CostItem;


                    // Ссылка на запись журнала регистрации (возможно, с него надо было начинать формирование отчёта)
                    var paymentRegistrator = (from pr in PaymentRegistrators
                                              where pr.PaymentRequest == paymentRequestObligations.PaymentRequestBase
                                              select pr).FirstOrDefault();
                    payedRequest.PaymentRegistrator = paymentRegistrator;

                    Res.Add(payedRequest);
                }
            }

            // Подсчёт сумм
            foreach (fmCRHPayedRequestNonPersistent item in Res) {
                item.SumByObligation = item.SumObligationOut;
                //item.SumByObligation = Res.Where(
                //    r => r.PaymentRegistrator == item.PaymentRegistrator
                //        && r.PaymentRequestObligation == item.PaymentRequestObligation
                //        && r.ValutaByObligation == item.ValutaByObligation
                //        //&& r.PaymentDocument == item.PaymentDocument
                //        && r.OperationDate == item.OperationDate
                //        && r.BankAccount == item.BankAccount
                //        && r.fmOrder == item.fmOrder
                //        && r.CostItem == item.CostItem
                //).Sum(r => r.SumObligationOut);
                item.SumByFact = item.SumOut;
                //item.SumByFact = Res.Where(
                //    r => r.PaymentRegistrator == item.PaymentRegistrator
                //        && r.PaymentRequestObligation == item.PaymentRequestObligation
                //        && r.ValutaByObligation == item.ValutaByObligation
                //        //&& r.PaymentDocument == item.PaymentDocument
                //        && r.OperationDate == item.OperationDate
                //        && r.BankAccount == item.BankAccount
                //        && r.fmOrder == item.fmOrder
                //        && r.CostItem == item.CostItem
                //).Sum(r => r.SumOut);
            }

            //// Обнуление сумм
            //foreach (fmCRHPayedRequestNonPersistent item in Res) {
            //    item.SumObligationOut = 0;
            //    item.SumOut = 0;
            //}

            //List<fmCRHPayedRequestNonPersistent> Res1 = new List<fmCRHPayedRequestNonPersistent>();
            //Res1 = (from item in Res
            //        select item).Distinct().ToList();

            return Res;
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

        #endregion

    }

}
