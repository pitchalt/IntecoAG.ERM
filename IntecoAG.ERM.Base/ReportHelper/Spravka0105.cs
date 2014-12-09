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

using IntecoAG.ERM.CS;
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
    public class Spravka0105 : BaseReportHelper
    {
        public Spravka0105(Session ses) : base(ses) { }

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

        //private DateTime _ObligationUnitDateTime;
        ///// <summary>
        ///// DateTime - Дата
        ///// Дата и время обязательства
        ///// </summary>
        //public DateTime ObligationUnitDateTime {
        //    get { return _ObligationUnitDateTime; }
        //    set { SetPropertyValue<DateTime>("ObligationUnitDateTime", ref _ObligationUnitDateTime, value); }
        //}

        //private string _ObligationUnitCode;
        ///// <summary>
        ///// ObligationUnitCode - Код
        ///// Код обязательства
        ///// </summary>
        //public string ObligationUnitCode {
        //    get { return _ObligationUnitCode; }
        //    set { SetPropertyValue<string>("ObligationUnitCode", ref _ObligationUnitCode, value); }
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

        private Guid _StageOid;
        /// <summary>
        /// StageOid 
        /// Идентификатор этапа
        /// </summary>
        public Guid StageOid {
            get { return _StageOid; }
            set { SetPropertyValue<Guid>("StageOid", ref _StageOid, value); }
        }

        private string _StageName;
        /// <summary>
        /// StageName
        /// Наименование этапа
        /// </summary>
        public string StageName {
            get { return _StageName; }
            set { SetPropertyValue<string>("StageName", ref _StageName, value); }
        }

        private int _ObligationUnitMonth;
        /// <summary>
        /// ObligationUnitMonth - Номер месяц обязательства
        /// Номер месяц обязательства
        /// </summary>
        public int ObligationUnitMonth {
            get { return _ObligationUnitMonth; }
            set { SetPropertyValue<int>("ObligationUnitMonth", ref _ObligationUnitMonth, value); }
        }

        private string _ObligationUnitMonthName;
        /// <summary>
        /// ObligationUnitMonthName - Название месяца обязательства
        /// Название месяца обязательства
        /// </summary>
        public string ObligationUnitMonthName {
            get { return _ObligationUnitMonthName; }
            set { SetPropertyValue<string>("ObligationUnitMonthName", ref _ObligationUnitMonthName, value); }
        }


        // -- Суммы --
        private decimal _DeliveryCostPlan;
        /// <summary>
        /// DeliveryCostPlan - Сумма плана основных обязательств
        /// План дебета
        /// </summary>
        public decimal DeliveryCostPlan {
            get { return _DeliveryCostPlan; }
            set { SetPropertyValue<decimal>("DeliveryCostPlan", ref _DeliveryCostPlan, value); }
        }

        private decimal _PaymentCostPlan;
        /// <summary>
        /// PaymentCostPlan - Сумма плана по обязателоствам оплаты
        /// План кредита
        /// </summary>
        public decimal PaymentCostPlan {
            get { return _PaymentCostPlan; }
            set { SetPropertyValue<decimal>("PaymentCostPlan", ref _PaymentCostPlan, value); }
        }


        private decimal _DeliveryCostFact;
        /// <summary>
        /// DeliveryCostFact - Сумма основных обязателств по факту
        /// Факт дебета
        /// </summary>
        public decimal DeliveryCostFact {
            get { return _DeliveryCostFact; }
            set { SetPropertyValue<decimal>("DeliveryCostFact", ref _DeliveryCostFact, value); }
        }

        private decimal _PaymentCostFact;
        /// <summary>
        /// PaymentCostFact - Сумма факта по обязательствам оплаты
        /// Факт кредита
        /// </summary>
        public decimal PaymentCostFact {
            get { return _PaymentCostFact; }
            set { SetPropertyValue<decimal>("PaymentCostFact", ref _PaymentCostFact, value); }
        }


        //private string _ValutaName;
        ///// <summary>
        ///// ValutaName
        ///// Валюта договора
        ///// </summary>
        //public string ValutaName {
        //    get { return _ValutaName; }
        //    set {
        //        SetPropertyValue<string>("ValutaName", ref _ValutaName, value);
        //    }
        //}

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
                                       registerRow.fmOrder,
                                       registerRow.ContragentParty,
                                       registerRow.Contract,
                                       registerRow.ContractDeal,
                                       registerRow.Stage,
                                       registerRow.ObligationUnitMonth
                                   }
                                     into registerGroup
                                     select new {
                                         RegGroup = registerGroup,

                                         PrimaryPartyOid = registerGroup.Key.PrimaryParty.Oid,
                                         PrimaryPartyName = registerGroup.Key.PrimaryParty.Person.NameFull,

                                         SubjectOid = registerGroup.Key.Subject.Oid,
                                         SubjectName = registerGroup.Key.Subject.Code,

                                         OrderOid = registerGroup.Key.fmOrder.Oid,
                                         OrderName = registerGroup.Key.fmOrder.Subject.Code,

                                         ContragentPartyOid = registerGroup.Key.ContragentParty.Oid,
                                         ContragentPartyName = registerGroup.Key.ContragentParty.Person.NameFull,

                                         ContractOid = registerGroup.Key.Contract.Oid,
                                         //!!!Паша нужно подставит ьправильное значение
                                         //ContractName = registerGroup.Key.Contract.Delo,
                                         ContractName = " ",

                                         ContractDealOid = registerGroup.Key.ContractDeal.Oid,
                                         ContractDealName = registerGroup.Key.ContractDeal.Name,

                                         StageOid = registerGroup.Key.Stage.Oid,
                                         StageName = registerGroup.Key.Stage.Code,

                                         //ValutaName = registerGroup.Key.ObligationUnit.Valuta.Code,

                                         ObligationUnitMonth = registerGroup.Key.ObligationUnitMonth,
                                         ObligationUnitMonthName = registerGroup.Key.ObligationUnitMonth,

                                         DeliveryCostPlan =
                                         (from regRow in dbRegister
                                          where
                                              regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                              & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                              & regRow.Contract == registerGroup.Key.Contract
                                              & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                              & regRow.Stage == registerGroup.Key.Stage
                                              & regRow.ObligationUnit.DatePlane.Month == registerGroup.Key.ObligationUnitMonth
                                              & regRow.fmOrder == registerGroup.Key.fmOrder
                                              & regRow.PlaneFact == PlaneFact.PLAN
                                              & (regRow.ObligationUnit as crmDeliveryUnit) != null
                                          select regRow.CreditCostInRUR).Sum()
                                             ,
                                         PaymentCostPlan =
                                         (from regRow in dbRegister
                                          where
                                              regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                              & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                              & regRow.Contract == registerGroup.Key.Contract
                                              & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                              & regRow.Stage == registerGroup.Key.Stage
                                              & regRow.ObligationUnit.DatePlane.Month == registerGroup.Key.ObligationUnitMonth
                                              & regRow.fmOrder == registerGroup.Key.fmOrder
                                              & regRow.PlaneFact == PlaneFact.PLAN
                                              & (regRow.ObligationUnit as crmPaymentUnit) != null
                                          select regRow.CreditCostInRUR).Sum()
                                             ,
                                         DeliveryCostFact =
                                         (from regRow in dbRegister
                                          where
                                              regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                              & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                              & regRow.Contract == registerGroup.Key.Contract
                                              & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                              & regRow.Stage == registerGroup.Key.Stage
                                              & regRow.ObligationUnit.DatePlane.Month == registerGroup.Key.ObligationUnitMonth
                                              & regRow.fmOrder == registerGroup.Key.fmOrder
                                              & regRow.PlaneFact == PlaneFact.FACT
                                              & (regRow.ObligationUnit as crmDeliveryUnit) != null
                                          select regRow.DebitCostInRUR).Sum()
                                             ,
                                         PaymentCostFact =
                                         (from regRow in dbRegister
                                          where
                                              regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                              & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                              & regRow.Contract == registerGroup.Key.Contract
                                              & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                              & regRow.Stage == registerGroup.Key.Stage
                                              & regRow.ObligationUnit.DatePlane.Month == registerGroup.Key.ObligationUnitMonth
                                              & regRow.fmOrder == registerGroup.Key.fmOrder
                                              & regRow.PlaneFact == PlaneFact.FACT
                                              & (regRow.ObligationUnit as crmPaymentUnit) != null
                                          select regRow.DebitCostInRUR).Sum()

                                     }; 

            //XPCollection xpColSpravka0105 = new XPCollection(ssn, typeof(Spravka0105));
            List<BaseReportHelper> xpColSpravka0105 = new List<BaseReportHelper>();


            // Для каждой группы формируем запись
            foreach (var grp in registerGroups) {
                foreach (var item in grp.RegGroup) {

                    Spravka0105 sprObj = new Spravka0105(ssn);

                    //sprObj.Oid = item.Oid;

                    sprObj.PrimaryPartyOid = grp.PrimaryPartyOid;
                    sprObj.ContragentPartyOid = grp.ContragentPartyOid;
                    sprObj.ContractOid = grp.ContractOid;
                    sprObj.ContractDealOid = grp.ContractDealOid;

                    sprObj.ObligationUnitMonth = grp.ObligationUnitMonth;
                    sprObj.ObligationUnitMonthName = GetMonthNameByNumber(grp.ObligationUnitMonth).ToString();

                    sprObj.StageOid = grp.StageOid;

                    sprObj.SubjectOid = grp.SubjectOid;
                    sprObj.SubjectName = grp.SubjectName;
                    sprObj.OrderOid = grp.OrderOid;
                    sprObj.OrderName = grp.OrderName;


                    //sprObj.ObligationUnitDateTime = grp.ObligationUnitDateTime;
                    //sprObj.ObligationUnitCode = grp.ObligationUnitCode;

                    sprObj.PrimaryPartyName = grp.PrimaryPartyName;    //item.PrimaryParty.PartyName;
                    sprObj.ContragentPartyName = grp.ContragentPartyName;    //item.ContragentParty.PartyName;
                    sprObj.ContractName = grp.ContractName;
                    sprObj.ContractDealName = grp.ContractDealName;   // item.ContractDeal.ContractDocument.FullName;
                    sprObj.StageName = grp.StageName;

                    //sprObj.ValutaName = grp.ValutaName;

                    // Вычислить 4-е величины
                    sprObj.DeliveryCostPlan = grp.DeliveryCostPlan;
                    sprObj.PaymentCostPlan = grp.PaymentCostPlan;
                    sprObj.DeliveryCostFact = grp.DeliveryCostFact;
                    sprObj.PaymentCostFact = grp.PaymentCostFact;

                    xpColSpravka0105.Add(sprObj);

                }
            }

            return xpColSpravka0105;
        }

        #endregion

        private Month GetMonthNameByNumber(int monthNumber) {
            if ((int)Month.January == monthNumber) return Month.January;
            if ((int)Month.February == monthNumber) return Month.February;
            if ((int)Month.March == monthNumber) return Month.March;
            if ((int)Month.April == monthNumber) return Month.April;
            if ((int)Month.May == monthNumber) return Month.May;
            if ((int)Month.June == monthNumber) return Month.June;
            if ((int)Month.July == monthNumber) return Month.July;
            if ((int)Month.August == monthNumber) return Month.August;
            if ((int)Month.September == monthNumber) return Month.September;
            if ((int)Month.October == monthNumber) return Month.October;
            if ((int)Month.November == monthNumber) return Month.November;
            //if ((int)month.December == monthNumber) return month.December;
            return Month.December;
        }
    }
}