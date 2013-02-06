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
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.Metadata;

using System.Data;
using System.Linq;
//using DevExpress.Xpo;

using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.Module.ReportHelper
{

    /// <summary>
    /// Класс crmGroupByStageAndObligationReportHelper для создания отчёта "Вариант справки группировка обязательств по этапам и поставкам"
    /// </summary>
    //[NonPersistent]
    [VisibleInReports]
    //[Persistent("crmGroupByStageAndObligationReportHelper")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class Spravka0104 : BaseReportHelper
    {
        public Spravka0104(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //private Guid _ObligationUnitOid;
        ///// <summary>
        ///// ObligationUnitOid 
        ///// Идентификатор обязательства
        ///// </summary>
        //public Guid ObligationUnitOid {
        //    get { return _ObligationUnitOid; }
        //    set { SetPropertyValue<Guid>("ObligationUnitOid", ref _ObligationUnitOid, value); }
        //}

        private Guid _PrimaryPartyOid;
        /// <summary>
        /// PrimaryPartyOid
        /// Ключ первичного контрагента
        /// </summary>
        public Guid PrimaryPartyOid {
            get { return _PrimaryPartyOid; }
            set { SetPropertyValue<Guid>("PrimaryPartyOid", ref _PrimaryPartyOid, value); }
        }

        private string _PrimaryPartyName;
        /// <summary>
        /// PrimaryPartyName - Организация
        /// Наименование первичного контрагента
        /// </summary>
        public string PrimaryPartyName {
            get { return _PrimaryPartyName; }
            set { SetPropertyValue<string>("PrimaryPartyName", ref _PrimaryPartyName, value); }
        }


        private Guid _ContragentPartyOid;
        /// <summary>
        /// ContragentPartyOid
        /// Ключ контрагента
        /// </summary>
        public Guid ContragentPartyOid {
            get { return _ContragentPartyOid; }
            set { SetPropertyValue<Guid>("ContragentPartyOid", ref _ContragentPartyOid, value); }
        }

        private string _ContragentPartyName;
        /// <summary>
        /// ContragentPartyName 
        /// Наименование контрагента
        /// </summary>
        public string ContragentPartyName {
            get { return _ContragentPartyName; }
            set { SetPropertyValue<string>("ContragentPartyName", ref _ContragentPartyName, value); }
        }

        private Guid _ContractOid;
        /// <summary>
        /// ContractOid
        /// Ключ контракта
        /// </summary>
        public Guid ContractOid {
            get { return _ContractOid; }
            set { SetPropertyValue<Guid>("ContractOid", ref _ContractOid, value); }
        }

        private string _ContractName;
        /// <summary>
        /// ContractName
        /// Наименование договороа
        /// </summary>
        public string ContractName {
            get { return _ContractName; }
            set {
                SetPropertyValue<string>("ContractName", ref _ContractName, value);
            }
        }

        private Guid _ContractDealOid;
        /// <summary>
        /// ContractDealOid
        /// Ключ ведомости
        /// </summary>
        public Guid ContractDealOid {
            get { return _ContractDealOid; }
            set { SetPropertyValue<Guid>("ContractDealOid", ref _ContractDealOid, value); }
        }

        private string _ContractDealName;
        /// <summary>
        /// ContractDealName
        /// Наименвоание простого договора (Ведомости)
        /// </summary>
        public string ContractDealName {
            get { return _ContractDealName; }
            set {
                SetPropertyValue<string>("ContractDealName", ref _ContractDealName, value);
            }
        }


        private Guid _OrderOid;
        /// <summary>
        /// OrderOid 
        /// Идентификатор заказа
        /// </summary>
        public Guid OrderOid {
            get { return _OrderOid; }
            set { SetPropertyValue<Guid>("OrderOid", ref _OrderOid, value); }
        }

        private string _OrderName;
        /// <summary>
        /// OrderName
        /// Имя заказа
        /// </summary>
        public string OrderName {
            get { return _OrderName; }
            set { SetPropertyValue<string>("OrderName", ref _OrderName, value); }
        }


        private Guid _SubjectOid;
        /// <summary>
        /// SubjectOid 
        /// Идентификатор темы
        /// </summary>
        public Guid SubjectOid {
            get { return _SubjectOid; }
            set { SetPropertyValue<Guid>("SubjectOid", ref _SubjectOid, value); }
        }

        private string _SubjectName;
        /// <summary>
        /// SubjectName
        /// Название темы
        /// </summary>
        public string SubjectName {
            get { return _SubjectName; }
            set { SetPropertyValue<string>("SubjectName", ref _SubjectName, value); }
        }


        // Для учёта месяцев надо делать либо свойство типа массива 12 чисел, либо напрямую прописывать все свойства
        // Попробую сделать массивами

        // -- Числа --

        /*
        private IList<decimal> _PlanByMonth;
        /// <summary>
        /// PlanByMonth - Суммы планов оплат по месяцам
        /// Суммы планов оплат по месяцам
        /// </summary>
        public IList<decimal> PlanByMonth {
            get { return _PlanByMonth; }
            set { SetPropertyValue<IList<decimal>>("PlanByMonth", ref _PlanByMonth, value); }
        }

        // Номера месяцев с ненулевыми данными по планам
        private IList<int> _PlanMonthNumbers;
        /// <summary>
        /// PlanMonthNumbers - Номера месяцев с ненулевыми данными по планам
        /// Номера месяцев с ненулевыми данными по планам
        /// </summary>
        public IList<int> PlanMonthNumbers {
            get { return _PlanMonthNumbers; }
            set { SetPropertyValue<IList<int>>("PlanMonthNumbers", ref _PlanMonthNumbers, value); }
        }


        private IList<decimal> _FactByMonth;
        /// <summary>
        /// Fact01 - Суммы фактов оплат по месяцам
        /// Суммы фактов оплат по месяцам
        /// </summary>
        public IList<decimal> FactByMonth {
            get { return _FactByMonth; }
            set { SetPropertyValue<IList<decimal>>("FactByMonth", ref _FactByMonth, value); }
        }

        // Номера месяцев с ненулевыми данными по фактам
        private IList<int> _FactMonthNumbers;
        /// <summary>
        /// FactMonthNumbers - Номера месяцев с ненулевыми данными по фактам
        /// Номера месяцев с ненулевыми данными по фактам
        /// </summary>
        public IList<int> FactMonthNumbers {
            get { return _FactMonthNumbers; }
            set { SetPropertyValue<IList<int>>("FactMonthNumbers", ref _FactMonthNumbers, value); }
        }
        */


        // =========== ПЛАН ==============
        private decimal _Plan01;
        /// <summary>
        /// Plan01 - Суммы плана оплаты за 01 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan01 {
            get { return _Plan01; }
            set { SetPropertyValue<decimal>("Plan01", ref _Plan01, value); }
        }

        private decimal _Plan02;
        /// <summary>
        /// Plan02 - Суммы плана оплаты за 02 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan02 {
            get { return _Plan02; }
            set { SetPropertyValue<decimal>("Plan02", ref _Plan02, value); }
        }

        private decimal _Plan03;
        /// <summary>
        /// Plan03 - Суммы плана оплаты за 03 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan03 {
            get { return _Plan03; }
            set { SetPropertyValue<decimal>("Plan03", ref _Plan03, value); }
        }

        private decimal _Plan04;
        /// <summary>
        /// Plan04 - Суммы плана оплаты за 04 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan04 {
            get { return _Plan04; }
            set { SetPropertyValue<decimal>("Plan04", ref _Plan04, value); }
        }

        private decimal _Plan05;
        /// <summary>
        /// Plan05 - Суммы плана оплаты за 05 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan05 {
            get { return _Plan05; }
            set { SetPropertyValue<decimal>("Plan05", ref _Plan05, value); }
        }

        private decimal _Plan06;
        /// <summary>
        /// Plan06 - Суммы плана оплаты за 06 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan06 {
            get { return _Plan06; }
            set { SetPropertyValue<decimal>("Plan06", ref _Plan06, value); }
        }

        private decimal _Plan07;
        /// <summary>
        /// Plan07 - Суммы плана оплаты за 07 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan07 {
            get { return _Plan07; }
            set { SetPropertyValue<decimal>("Plan07", ref _Plan07, value); }
        }

        private decimal _Plan08;
        /// <summary>
        /// Plan08 - Суммы плана оплаты за 08 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan08 {
            get { return _Plan08; }
            set { SetPropertyValue<decimal>("Plan08", ref _Plan08, value); }
        }

        private decimal _Plan09;
        /// <summary>
        /// Plan09 - Суммы плана оплаты за 09 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan09 {
            get { return _Plan09; }
            set { SetPropertyValue<decimal>("Plan09", ref _Plan09, value); }
        }

        private decimal _Plan10;
        /// <summary>
        /// Plan10 - Суммы плана оплаты за 10 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan10 {
            get { return _Plan10; }
            set { SetPropertyValue<decimal>("Plan10", ref _Plan10, value); }
        }

        private decimal _Plan11;
        /// <summary>
        /// Plan11 - Суммы плана оплаты за 11 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan11 {
            get { return _Plan11; }
            set { SetPropertyValue<decimal>("Plan11", ref _Plan11, value); }
        }

        private decimal _Plan12;
        /// <summary>
        /// Plan12 - Суммы плана оплаты за 12 месяц
        /// Суммы плана оплаты за 01 месяц
        /// </summary>
        public decimal Plan12 {
            get { return _Plan12; }
            set { SetPropertyValue<decimal>("Plan12", ref _Plan12, value); }
        }
        // =========== ПЛАН ==============


        // =========== ФАКТ ==============
        private decimal _Fact01;
        /// <summary>
        /// Fact01 - Суммы факта оплаты за 01 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact01 {
            get { return _Fact01; }
            set { SetPropertyValue<decimal>("Fact01", ref _Fact01, value); }
        }

        private decimal _Fact02;
        /// <summary>
        /// Fact02 - Суммы факта оплаты за 02 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact02 {
            get { return _Fact02; }
            set { SetPropertyValue<decimal>("Fact02", ref _Fact02, value); }
        }

        private decimal _Fact03;
        /// <summary>
        /// Fact03 - Суммы факта оплаты за 03 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact03 {
            get { return _Fact03; }
            set { SetPropertyValue<decimal>("Fact03", ref _Fact03, value); }
        }

        private decimal _Fact04;
        /// <summary>
        /// Fact04 - Суммы факта оплаты за 04 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact04 {
            get { return _Fact04; }
            set { SetPropertyValue<decimal>("Fact04", ref _Fact04, value); }
        }

        private decimal _Fact05;
        /// <summary>
        /// Fact05 - Суммы факта оплаты за 05 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact05 {
            get { return _Fact05; }
            set { SetPropertyValue<decimal>("Fact05", ref _Fact05, value); }
        }

        private decimal _Fact06;
        /// <summary>
        /// Fact06 - Суммы факта оплаты за 06 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact06 {
            get { return _Fact06; }
            set { SetPropertyValue<decimal>("Fact06", ref _Fact06, value); }
        }

        private decimal _Fact07;
        /// <summary>
        /// Fact07 - Суммы факта оплаты за 07 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact07 {
            get { return _Fact07; }
            set { SetPropertyValue<decimal>("Fact07", ref _Fact07, value); }
        }

        private decimal _Fact08;
        /// <summary>
        /// Fact08 - Суммы факта оплаты за 08 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact08 {
            get { return _Fact08; }
            set { SetPropertyValue<decimal>("Fact08", ref _Fact08, value); }
        }

        private decimal _Fact09;
        /// <summary>
        /// Fact09 - Суммы факта оплаты за 09 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact09 {
            get { return _Fact09; }
            set { SetPropertyValue<decimal>("Fact09", ref _Fact09, value); }
        }

        private decimal _Fact10;
        /// <summary>
        /// Fact10 - Суммы факта оплаты за 10 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact10 {
            get { return _Fact10; }
            set { SetPropertyValue<decimal>("Fact10", ref _Fact10, value); }
        }

        private decimal _Fact11;
        /// <summary>
        /// Fact11 - Суммы факта оплаты за 11 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact11 {
            get { return _Fact11; }
            set { SetPropertyValue<decimal>("Fact11", ref _Fact11, value); }
        }

        private decimal _Fact12;
        /// <summary>
        /// Fact12 - Суммы факта оплаты за 12 месяц
        /// Суммы факта оплаты за 01 месяц
        /// </summary>
        public decimal Fact12 {
            get { return _Fact12; }
            set { SetPropertyValue<decimal>("Fact12", ref _Fact12, value); }
        }
        // =========== ФАКТ ==============



        // -- Итоги --
        private decimal _PlanPaymentResult;
        /// <summary>
        /// PlanResult - Итог по плану за все месяцы по оплатам
        /// Итог по плану за все месяцы по оплатам
        /// </summary>
        public decimal PlanPaymentResult {
            get { return _PlanPaymentResult; }
            set { SetPropertyValue<decimal>("PlanPaymentResult", ref _PlanPaymentResult, value); }
        }


        private decimal _FactPaymentResult;
        /// <summary>
        /// FactResult - Итог по факту за все месяцы по оплатам
        /// Итог по факту за все месяцы по оплатам
        /// </summary>
        public decimal FactPaymentResult {
            get { return _FactPaymentResult; }
            set { SetPropertyValue<decimal>("FactPaymentResult", ref _FactPaymentResult, value); }
        }


        private string _ValutaName;
        /// <summary>
        /// ValutaName
        /// Валюта договора
        /// </summary>
        public string ValutaName {
            get { return _ValutaName; }
            set {
                SetPropertyValue<string>("ValutaName", ref _ValutaName, value);
            }
        }

        #endregion


        #region МЕТОДЫ


        public override List<BaseReportHelper> CreateReportListSource(Session ssn, CriteriaOperator criteria) {

            XPClassInfo classInfo = ssn.GetClassInfo(this.GetType());

            XPCollection<crmDebtorCreditorDebtRegister> dbRegister = new XPCollection<crmDebtorCreditorDebtRegister>(ssn, criteria, null);
            dbRegister.Criteria = criteria;
            dbRegister.Reload();
            if (!dbRegister.IsLoaded) dbRegister.Load();

            // Linq по коллекции
            var registerGroups = from registerRow in dbRegister
                                 group registerRow by
                                   new {
                                       registerRow.PrimaryParty,
                                       registerRow.Subject,
                                       registerRow.ContragentParty,
                                       registerRow.Contract,
                                       registerRow.ContractDeal,
                                       registerRow.fmOrder
                                   }
                                     into registerGroup
                                     select new {
                                         RegGroup = registerGroup,

                                         PrimaryPartyOid = registerGroup.Key.PrimaryParty.Oid,
                                         PrimaryPartyName = registerGroup.Key.PrimaryParty.Person.NameFull,
                                         ContragentPartyOid = registerGroup.Key.ContragentParty.Oid,
                                         ContragentPartyName = registerGroup.Key.ContragentParty.Person.NameFull,
                                         ContractOid = registerGroup.Key.Contract.Oid,
                                         //!!!Паша нужно подставит ьправильное значение
                                         //ContractName = registerGroup.Key.Contract.Delo,
                                         ContractName = " ",
                                         ContractDealOid = registerGroup.Key.ContractDeal.Oid,
                                         ContractDealName = registerGroup.Key.ContractDeal.Name
                                             //,StageOid = registerGroup.Key.Stage.Oid
                                             //,StageName = registerGroup.Key.Stage.Code
                                             //,ValutaName = registerGroup.Key.ObligationUnit.Valuta.Code

                                         //,ObligationUnitOid = registerGroup.Key.ObligationUnit.Oid
                                             //,ObligationUnitDateTime = registerGroup.Key.ObligationUnit.DatePlane
                                             //,ObligationUnitCode = registerGroup.Key.ObligationUnit.Code // Уточнить!

                                         // ======== План =========
                                         ,
                                         Plan01 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 1
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan02 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 2
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan03 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 3
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan04 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 4
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan05 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 5
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan06 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 6
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan07 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 7
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan08 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 8
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan09 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 9
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan10 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 10
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan11 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 11
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Plan12 =
                                             (from regRow in dbRegister
                                              where
                                                  regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                  & regRow.Subject == registerGroup.Key.Subject
                                                  & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                  & regRow.Contract == registerGroup.Key.Contract
                                                  & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                  & regRow.fmOrder == registerGroup.Key.fmOrder
                                                  & regRow.ObligationUnitMonth == 12
                                                  & regRow.PlaneFact == PlaneFact.PLAN
                                                  & (regRow.ObligationUnit as crmPaymentUnit) != null
                                              select regRow.CreditCostInRUR).Sum()
                                             // ======== План =========



                                         // ======== План =========
                                         ,
                                         Fact01 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 1
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact02 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 2
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact03 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 3
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact04 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 4
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact05 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 5
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact06 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 6
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact07 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 7
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact08 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 8
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact09 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 9
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact10 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 10
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact11 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 11
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         ,
                                         Fact12 =
                                            (from regRow in dbRegister
                                             where
                                                 regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                                 & regRow.Subject == registerGroup.Key.Subject
                                                 & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                                 & regRow.Contract == registerGroup.Key.Contract
                                                 & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                                 & regRow.fmOrder == registerGroup.Key.fmOrder
                                                 & regRow.ObligationUnitMonth == 12
                                                 & regRow.PlaneFact == PlaneFact.FACT
                                                 & (regRow.ObligationUnit as crmPaymentUnit) != null
                                             select regRow.CreditCostInRUR).Sum()
                                         // ======== План =========

                                     }; 


            //XPCollection xpColSpravka0104 = new XPCollection(ssn, typeof(Spravka0104));
            List<BaseReportHelper> xpColSpravka0104 = new List<BaseReportHelper>();


            // Для каждой группы формируем запись
            foreach (var grp in registerGroups) {
                foreach (var item in grp.RegGroup) {

                    Spravka0104 sprObj = new Spravka0104(ssn);

                    sprObj.PrimaryPartyOid = grp.PrimaryPartyOid;
                    sprObj.ContragentPartyOid = grp.ContragentPartyOid;
                    sprObj.ContractOid = grp.ContractOid;
                    sprObj.ContractDealOid = grp.ContractDealOid;

                    sprObj.SubjectOid = item.Subject.Oid;
                    sprObj.OrderOid = item.fmOrder.Oid;

                    //sprObj.ObligationUnitOid = grp.ObligationUnitOid;
                    //sprObj.StageOid = grp.StageOid;

                    //sprObj.ObligationUnitDateTime = grp.ObligationUnitDateTime;
                    //sprObj.ObligationUnitCode = grp.ObligationUnitCode;

                    sprObj.PrimaryPartyName = grp.PrimaryPartyName;    //item.PrimaryParty.PartyName;
                    sprObj.ContragentPartyName = grp.ContragentPartyName;    //item.ContragentParty.PartyName;
                    sprObj.ContractName = grp.ContractName;
                    sprObj.ContractDealName = grp.ContractDealName;   // item.ContractDeal.ContractDocument.FullName;
                    //sprObj.StageName = grp.StageName;

                    sprObj.SubjectName = item.Subject.Name;
                    sprObj.OrderName = item.fmOrder.Subject.Code;   // SHU 2011-12-15 Изменить на нормальное!!!


                    //sprObj.ValutaName = grp.ValutaName;

                    sprObj.Plan01 = grp.Plan01;
                    sprObj.Plan02 = grp.Plan02;
                    sprObj.Plan03 = grp.Plan03;
                    sprObj.Plan04 = grp.Plan04;
                    sprObj.Plan05 = grp.Plan05;
                    sprObj.Plan06 = grp.Plan06;
                    sprObj.Plan07 = grp.Plan07;
                    sprObj.Plan08 = grp.Plan08;
                    sprObj.Plan09 = grp.Plan09;
                    sprObj.Plan10 = grp.Plan10;
                    sprObj.Plan11 = grp.Plan11;
                    sprObj.Plan12 = grp.Plan12;

                    sprObj.Fact01 = grp.Fact01;
                    sprObj.Fact02 = grp.Fact02;
                    sprObj.Fact03 = grp.Fact03;
                    sprObj.Fact04 = grp.Fact04;
                    sprObj.Fact05 = grp.Fact05;
                    sprObj.Fact06 = grp.Fact06;
                    sprObj.Fact07 = grp.Fact07;
                    sprObj.Fact08 = grp.Fact08;
                    sprObj.Fact09 = grp.Fact09;
                    sprObj.Fact10 = grp.Fact10;
                    sprObj.Fact11 = grp.Fact11;
                    sprObj.Fact12 = grp.Fact12;

                    sprObj.PlanPaymentResult =
                        grp.Plan01
                        + grp.Plan02
                        + grp.Plan03
                        + grp.Plan04
                        + grp.Plan05
                        + grp.Plan06
                        + grp.Plan07
                        + grp.Plan08
                        + grp.Plan09
                        + grp.Plan10
                        + grp.Plan11
                        + grp.Plan12;

                    sprObj.FactPaymentResult =
                        grp.Fact01
                        + grp.Fact02
                        + grp.Fact03
                        + grp.Fact04
                        + grp.Fact05
                        + grp.Fact06
                        + grp.Fact07
                        + grp.Fact08
                        + grp.Fact09
                        + grp.Fact10
                        + grp.Fact11
                        + grp.Fact12;






                    //sprObj.Oid = item.Oid;

                    xpColSpravka0104.Add(sprObj);

                }
            }

            return xpColSpravka0104;
        }

        #endregion

    }
}