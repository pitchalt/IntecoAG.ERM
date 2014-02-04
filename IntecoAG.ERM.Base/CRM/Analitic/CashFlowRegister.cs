#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.ComponentModel;
//
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Docs;
//using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Статусы заявки
    /// </summary>
    public enum CashFlowRegisterSection {
        /// <summary>
        /// Секция Договор/План
        /// </summary>
        CONTRACT_PLAN = 1,
        /// <summary>
        /// Секция CashFlow
        /// </summary>
        CASH_FLOW = 2,
        /// <summary>
        /// Секция привязок заявок и платёжных документов
        /// </summary>
        REPAYMENT_JOURNAL = 3,
        /// <summary>
        /// Секция загрузок документов выписок
        /// </summary>
        OPERATION_JOURNAL = 4
    }

    /// <summary>
    /// Класс crmCashFlowRegister
    /// </summary>
    //[DefaultClassOptions]
    [VisibleInReports]
    [Persistent("crmCashFlowRegister")]
    [NavigationItem("Money")]
    public class crmCashFlowRegister : crmBaseRegister
    {
        public crmCashFlowRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        // crmCommonBaseRegister
        private crmContract _Contract;   // +
        private crmContractDeal _ContractDeal;   // +
        // УДАЛЕНО 2012-05-10 private crmStage _Stage;
        private fmCOrder _fmOrder;   // +
        private crmFinancialDeal _FinancialDeal;   // +
        private fmCostItem _CostItem;   // +
        private fmCSubject _Subject;   // +
        //private PlaneFact _PlaneFact;   // УДАЛИТЬ! 2012-05-10

        // ===================================


        // crmBasePrimaryPartyContragentRegister
        private DateTime _ObligationUnitDateTime;   // УДАЛИТЬ! 2012-05-10
        private crmObligationUnit _ObligationUnit;   // НЕ ИЗВЕСТНО, НУЖНО ЛИ ЭТО ПОЛЕ! 2012-05-10
        private crmCParty _PrimaryParty;   // +   ПЕРЕИМЕНОВАТЬ
        private crmCParty _ContragentParty;   // +   ПЕРЕИМЕНОВАТЬ

        // Новые поля
        private crmCParty _PrimaryPartyReal;   // +   ПЕРЕИМЕНОВАТЬ ???
        private crmCParty _ContragentPartyReal;   // +   ПЕРЕИМЕНОВАТЬ ???
        // ===

        // УДАЛЕНО 2012-05-10 private crmStage _StageTech;

        // ==================================

        private crmPaymentItem _PaymentItem;   // НЕ ИЗВЕСТНО, НУЖНО ЛИ ЭТО ПОЛЕ! 2012-05-10
        private decimal _PaymentCost;   // УДАЛИТЬ! 2012-05-10 ЗАМЕНИТЬ НА SumIn и SumOut
        private csValuta _PaymentValuta;   // УДАЛИТЬ! 2012-05-10 ЗАМЕНИТЬ НА ValutaPayment (см. ниже)
        private decimal _Cost;   // УДАЛИТЬ! 2012-05-10 ЗАМЕНИТЬ НА SumObligationIn и SumObligationOut
        private csValuta _Valuta;   // ПЕРЕИМЕНОВАТЬ В ValutaObligation
        
        //private decimal _CostInRUR;   // УДАЛИТЬ! 2012-05-10 И ДОБАВИТЬ SumInAcc, SunOutAcc
        
        private csValuta _ValutaAcc; // Валюта учёта
        private decimal _SumInAcc;   // Сумма поступления в валюте учёта
        private decimal _SumOutAcc;   // Сумма убытия в валюте учёта
        private decimal _SumBalanceAcc;   // SumInAcc - SumOutAcc


        // Источник записи 
        private Guid _SourceGUID;
        private Type _SourceType;

        private CashFlowRegisterSection _Section;   // Секция регистра

        private csValuta _ValutaPayment; // Валюта расчёта
        private decimal _SumIn;   // Сумма поступления в валюте расчётов
        private decimal _SumOut;   // Сумма убытия в валюте расчётов
        private decimal _SumBalance;   // SumIn - SumOut

        private csValuta _ValutaObligation; // Валюта обязательств
        private decimal _SumObligationIn;   // Сумма поступления в валюте учёта (обязательств)
        private decimal _SumObligationOut;   // Сумма убытия в валюте учёта (обязательств)
        private decimal _SumObligationBalance;   // SumObligationIn - SumObligationOut  (в валюте учёта (обязательств))

        private crmBank _Bank;
        private crmBankAccount _BankAccount;

        private csCDocRCB _PaymentDocument;   // Платёжный документ, для которого подбирается документ выписки

        private DateTime _OperationDate;   // Дата операции
        //private fmCSAStatementAccountDoc _StatementAccountDoc;   // Документ выписки. Основание изменения в регистре - НЕ НУЖНО!

        private Guid _PaymentRequestObligationGUID;   // Для секции REPAYMENT_TASK, GUID для fmCPRPaymentRequestObligation

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Договор
        /// </summary>
        public crmContract Contract {
            get {
                return _Contract;
            }
            set {
                SetPropertyValue<crmContract>("Contract", ref _Contract, value);
            }
        }

        /// <summary>
        /// Простой договор (Ведомость)	Договор, по которому непосредственно осуществляется выполнение обязательств.
        /// </summary>
        public crmContractDeal ContractDeal {
            get {
                return _ContractDeal;
            }
            set {
                SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value);
            }
        }

        /*
        /// <summary>
        /// Этап
        /// </summary>
        public crmStage Stage {
            get {
                return _Stage;
            }
            set {
                SetPropertyValue<crmStage>("Stage", ref _Stage, value);
            }
        }
        */

        /// <summary>
        /// Ссылка на Заказ
        /// </summary>
        public fmCOrder fmOrder {
            get {
                return _fmOrder;
            }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _fmOrder, value);
            }
        }

        // FinancialDeal - временно вместо него Stage с типом Finance
        /// <summary>
        /// Финансовая сделка
        /// </summary>
        public crmFinancialDeal FinancialDeal {
            get {
                return _FinancialDeal;
            }
            set {
                SetPropertyValue<crmFinancialDeal>("FinancialDeal", ref _FinancialDeal, value);
            }
        }


        /// <summary>
        /// Статья затрат (ДДС)
        /// </summary>
        public fmCostItem CostItem {
            get {
                return _CostItem;
            }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        /// <summary>
        /// Тема
        /// </summary>
        public fmCSubject Subject {
            get {
                return _Subject;
            }
            set {
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
            }
        }

        /*
        /// <summary>
        /// План/Факт
        /// </summary>
        public PlaneFact PlaneFact {
            get {
                return _PlaneFact;
            }
            set {
                SetPropertyValue<PlaneFact>("PlaneFact", ref _PlaneFact, value);
            }
        }
        */

        /// <summary>
        /// План/Факт
        /// </summary>
        public CashFlowRegisterSection Section {
            get {
                return _Section;
            }
            set {
                SetPropertyValue<CashFlowRegisterSection>("Section", ref _Section, value);
            }
        }


        [Browsable(false)]
        [Indexed]
        public virtual Guid SourceGUID {
            get {
                return _SourceGUID;
            }
            set {
                SetPropertyValue<Guid>("SourceGUID", ref _SourceGUID, value);
            }
        }

        //[Browsable(false)]
        [ValueConverter(typeof(ConverterType2String))]
        public virtual Type SourceType {
            get {
                return _SourceType;
            }
            set {
                SetPropertyValue<Type>("SourceType", ref _SourceType, value);
            }
        }

        // =====================================


        /// <summary>
        /// Дата и время обязательства
        /// </summary>
        public DateTime ObligationUnitDateTime {
            get {
                return _ObligationUnitDateTime;
            }
            set {
                SetPropertyValue<DateTime>("ObligationUnitDateTime", ref _ObligationUnitDateTime, value);
            }
        }


        /// <summary>
        /// Месяц обязательства
        /// </summary>
        public int ObligationUnitMonth {
            get {
                return ObligationUnitDateTime.Month;
            }
        }

        /// <summary>
        /// Квартал обязательства
        /// </summary>
        public int ObligationUnitQuarter {
            get {
                if (ObligationUnitMonth <= 3)
                    return 1;
                else if (ObligationUnitMonth <= 6)
                    return 2;
                else if (ObligationUnitMonth <= 9)
                    return 3;
                return 4;
            }
        }

        /// <summary>
        /// Год обязательства
        /// </summary>
        public int ObligationUnitYear {
            get {
                return ObligationUnitDateTime.Year;
            }
        }

        /// <summary>
        /// Обязательство
        /// </summary>
        public crmObligationUnit ObligationUnit {
            get {
                return _ObligationUnit;
            }
            set {
                SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value);
            }
        }

        /// <summary>
        /// Получатель оплаты, он же Исполнитель/Поставщик
        /// </summary>
        public crmCParty PrimaryParty {
            get {
                return _PrimaryParty;
            }
            set {
                SetPropertyValue<crmCParty>("PrimaryParty", ref _PrimaryParty, value);
            }
        }

        /// <summary>
        /// Плательщик, он же Заказчик/Полкупатель
        /// </summary>
        public crmCParty ContragentParty {
            get {
                return _ContragentParty;
            }
            set {
                SetPropertyValue<crmCParty>("ContragentParty", ref _ContragentParty, value);
            }
        }

        /// <summary>
        /// Получатель оплаты Real
        /// </summary>
        public crmCParty PrimaryPartyReal {
            get {
                return _PrimaryPartyReal;
            }
            set {
                SetPropertyValue<crmCParty>("PrimaryPartyReal", ref _PrimaryPartyReal, value);
            }
        }

        /// <summary>
        /// Плательщик Real
        /// </summary>
        public crmCParty ContragentPartyReal {
            get {
                return _ContragentPartyReal;
            }
            set {
                SetPropertyValue<crmCParty>("ContragentPartyReal", ref _ContragentPartyReal, value);
            }
        }

        /*
        /// <summary>
        /// Технический этап 1
        /// </summary>
        public crmStage StageTech {
            get {
                return _StageTech;
            }
            set {
                SetPropertyValue<crmStage>("StageTech", ref _StageTech, value);
            }
        }
        */

        // =====================================

        /// <summary>
        /// Пункт обязательств оплаты
        /// </summary>
        public virtual crmPaymentItem PaymentItem {
            get { return _PaymentItem; }
            set { SetPropertyValue<crmPaymentItem>("PaymentItem", ref _PaymentItem, value); }
        }

        /// <summary>
        /// Стоимость в валюте расчёта
        /// </summary>
        public decimal PaymentCost {
            get { return _PaymentCost; }
            set { SetPropertyValue<decimal>("PaymentCost", ref _PaymentCost, value); }
        }

        /// <summary>
        /// Валюта расчёта
        /// </summary>
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
            }
        }

        /// <summary>
        /// Стоимость в валюте договора
        /// </summary>
        public decimal Cost {
            get { return _Cost; }
            set { SetPropertyValue<decimal>("Cost", ref _Cost, value); }
        }

        /// <summary>
        /// Валюта договора
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /*
        /// <summary>
        /// Стоимость в рублях
        /// </summary>
        public decimal CostInRUR {
            get { return _CostInRUR; }
            set { SetPropertyValue<decimal>("CostInRUR", ref _CostInRUR, value); }
        }
        */

        // НОВЫЕ СВОЙСТВА


        /// <summary>
        /// Валюта расчёта
        /// </summary>
        public csValuta ValutaPayment {
            get {
                return _ValutaPayment;
            }
            set {
                SetPropertyValue<csValuta>("ValutaPayment", ref _ValutaPayment, value);
            }
        }

        /// <summary>
        ///Сумма поступления в валюте расчётов
        /// </summary>
        public decimal SumIn {
            get {
                return _SumIn;
            }
            set {
                SetPropertyValue<decimal>("SumIn", ref _SumIn, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в валюте расчётов
        /// </summary>
        public decimal SumOut {
            get {
                return _SumOut;
            }
            set {
                SetPropertyValue<decimal>("SumOut", ref _SumOut, value);
                if (!IsLoading) {
                    UpdateBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumIn - SumOut
        /// </summary>
        public decimal SumBalance {
            get {
                return _SumBalance;
            }
            set {
                SetPropertyValue<decimal>("SumBalance", ref _SumBalance, value);
            }
        }



        /// <summary>
        /// Валюта обязательств
        /// </summary>
        public csValuta ValutaObligation {
            get {
                return _ValutaObligation;
            }
            set {
                SetPropertyValue<csValuta>("ValutaObligation", ref _ValutaObligation, value);
            }
        }

        /// <summary>
        /// Сумма поступления в валюте обязательств
        /// </summary>
        public decimal SumObligationIn {
            get {
                return _SumObligationIn;
            }
            set {
                SetPropertyValue<decimal>("SumObligationIn", ref _SumObligationIn, value);
                if (!IsLoading) {
                    UpdateObligationBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в валюте обязательств
        /// </summary>
        public decimal SumObligationOut {
            get {
                return _SumObligationOut;
            }
            set {
                SetPropertyValue<decimal>("SumObligationOut", ref _SumObligationOut, value);
                if (!IsLoading) {
                    UpdateObligationBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumObligationIn - SumObligationOut в валюте обязательств
        /// </summary>
        public decimal SumObligationBalance {
            get {
                return _SumObligationBalance;
            }
            set {
                SetPropertyValue<decimal>("SumObligationBalance", ref _SumObligationBalance, value);
            }
        }

        //==================

        /// <summary>
        /// Валюта учёта
        /// </summary>
        public csValuta ValutaAcc {
            get {
                return _ValutaAcc;
            }
            set {
                SetPropertyValue<csValuta>("ValutaAcc", ref _ValutaAcc, value);
            }
        }

        /// <summary>
        /// Сумма поступления в валюте учёта
        /// </summary>
        public decimal SumInAcc {
            get {
                return _SumInAcc;
            }
            set {
                SetPropertyValue<decimal>("SumInAcc", ref _SumInAcc, value);
                if (!IsLoading) {
                    UpdateAccBalance();
                }
            }
        }

        /// <summary>
        /// Сумма убытия в валюте учёта
        /// </summary>
        public decimal SumOutAcc {
            get {
                return _SumOutAcc;
            }
            set {
                SetPropertyValue<decimal>("SumOutAcc", ref _SumOutAcc, value);
                if (!IsLoading) {
                    UpdateAccBalance();
                }
            }
        }

        /// <summary>
        /// Баланс SumInAcc - SumOutAcc в валюте учёта
        /// </summary>
        public decimal SumBalanceAcc {
            get {
                return _SumBalanceAcc;
            }
            set {
                SetPropertyValue<decimal>("SumBalanceAcc", ref _SumBalanceAcc, value);
            }
        }

       /// <summary>
        /// Платёжный документ, для которого подбирается документ выписки
        /// </summary>
        //[DataSourceCriteria("StatementAccountIndicator = false")]   // Те платёжные документы, что непогашены, т.е. сумма которых не исчерпана выписками
        public csCDocRCB PaymentDocument {
            get {
                return _PaymentDocument;
            }
            set {
                SetPropertyValue<csCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }

        /// <summary>
        /// Банк
        /// </summary>
        public crmBank Bank {
            get {
                return _Bank;
            }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }

        /// <summary>
        /// Счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get {
                return _BankAccount;
            }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// Дата операции
        /// </summary>
        public DateTime OperationDate {
            get {
                return _OperationDate;
            }
            set {
                SetPropertyValue<DateTime>("OperationDate", ref _OperationDate, value);
            }
        }

        /// <summary>
        /// Для секции REPAYMENT_TASK, GUID для fmCPRPaymentRequestObligation
        /// </summary>
        public Guid PaymentRequestObligationGUID {
            get {
                return _PaymentRequestObligationGUID;
            }
            set {
                SetPropertyValue<Guid>("PaymentRequestObligationGUID", ref _PaymentRequestObligationGUID, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        private void UpdateBalance() {
            SumBalance = SumIn - SumOut;
        }

        private void UpdateObligationBalance() {
            SumObligationBalance = SumObligationIn - SumObligationOut;
        }

        private void UpdateAccBalance() {
            SumBalanceAcc = SumInAcc - SumOutAcc;
        }

        #endregion

    }
}