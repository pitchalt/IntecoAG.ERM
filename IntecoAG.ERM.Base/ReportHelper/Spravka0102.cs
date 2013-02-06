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
    public class Spravka0102 : BaseReportHelper
    {
        public Spravka0102(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private Guid _ObligationUnitOid;
        /// <summary>
        /// ObligationUnitOid 
        /// Идентификатор обязательства
        /// </summary>
        public Guid ObligationUnitOid {
            get { return _ObligationUnitOid; }
            set { SetPropertyValue<Guid>("ObligationUnitOid", ref _ObligationUnitOid, value); }
        }

        private DateTime _ObligationUnitDateTime;
        /// <summary>
        /// DateTime - Дата
        /// Дата и время обязательства
        /// </summary>
        public DateTime ObligationUnitDateTime {
            get { return _ObligationUnitDateTime; }
            set { SetPropertyValue<DateTime>("ObligationUnitDateTime", ref _ObligationUnitDateTime, value); }
        }

        private string _ObligationUnitCode;
        /// <summary>
        /// ObligationUnitCode - Код
        /// Код обязательства
        /// </summary>
        public string ObligationUnitCode {
            get { return _ObligationUnitCode; }
            set { SetPropertyValue<string>("ObligationUnitCode", ref _ObligationUnitCode, value); }
        }


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

        private decimal _DeliveryCostPlanFact;
        /// <summary>
        /// DeliveryCostPlanFact - Разность план - факт по основным обязательствам
        /// План дебета
        /// </summary>
        public decimal DeliveryCostPlanFact {
            get { return _DeliveryCostPlanFact; }
            set { SetPropertyValue<decimal>("DeliveryCostPlanFact", ref _DeliveryCostPlanFact, value); }
        }

        private decimal _PaymentCostPlanFact;
        /// <summary>
        /// PaymentCostPlanFact - Разность план - факт по обязательствам оплаты
        /// План кредита
        /// </summary>
        public decimal PaymentCostPlanFact {
            get { return _PaymentCostPlanFact; }
            set { SetPropertyValue<decimal>("PaymentCostPlanFact", ref _PaymentCostPlanFact, value); }
        }

        private decimal _ResultPlanFact;
        /// <summary>
        /// ResultPlanFact - DeliveryCostPlanFact - PaymentCostPlanFact
        /// План кредита
        /// </summary>
        public decimal ResultPlanFact {
            get { return _ResultPlanFact; }
            set { SetPropertyValue<decimal>("ResultPlanFact", ref _ResultPlanFact, value); }
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

        //public override XPCollection CreateReportXPCollectionSource(Session ssn, CriteriaOperator criteria) {
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
                                       registerRow.ContragentParty,
                                       registerRow.Contract,
                                       registerRow.ContractDeal,
                                       registerRow.Stage,
                                       registerRow.ObligationUnit
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
//                                         ContractName = registerGroup.Key.Contract.Delo,
                                         ContractName = " ",
                                         ContractDealOid = registerGroup.Key.ContractDeal.Oid,
                                         ContractDealName = registerGroup.Key.ContractDeal.Name,
                                         StageOid = registerGroup.Key.Stage.Oid,
                                         StageName = registerGroup.Key.Stage.Code,

                                         ValutaName = registerGroup.Key.ObligationUnit.Valuta.Code,

                                         ObligationUnitOid = registerGroup.Key.ObligationUnit.Oid,
                                         ObligationUnitDateTime = registerGroup.Key.ObligationUnit.DatePlane, // Уточнить!
                                         ObligationUnitCode = registerGroup.Key.ObligationUnit.Code,


                                         DeliveryCostPlan =
                                         (from regRow in dbRegister
                                          where
                                              regRow.PrimaryParty == registerGroup.Key.PrimaryParty
                                              & regRow.ContragentParty == registerGroup.Key.ContragentParty
                                              & regRow.Contract == registerGroup.Key.Contract
                                              & regRow.ContractDeal == registerGroup.Key.ContractDeal
                                              & regRow.Stage == registerGroup.Key.Stage
                                              & regRow.ObligationUnit == registerGroup.Key.ObligationUnit
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
                                              & regRow.ObligationUnit == registerGroup.Key.ObligationUnit
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
                                              & regRow.ObligationUnit == registerGroup.Key.ObligationUnit
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
                                              & regRow.ObligationUnit == registerGroup.Key.ObligationUnit
                                              & regRow.PlaneFact == PlaneFact.FACT
                                              & (regRow.ObligationUnit as crmPaymentUnit) != null
                                          select regRow.DebitCostInRUR).Sum()

                                     };

            /*
            var results = from item in registerGroups
                          select new {
                             PrimaryPartyOid = item.PrimaryPartyOid
                            ,ContragentPartyOid = item.ContragentPartyOid
                            ,ContractOid = item.ContractOid
                            ,ContractDealOid = item.ContractDealOid
                            ,ObligationUnitOid = item.ObligationUnitOid
                            ,item.StageOid

                            ,item.ObligationUnitDateTime
                            ,item.ObligationUnitCode

                            ,item.PrimaryPartyName
                            ,item.ContragentPartyName
                            ,item.ContractName
                            ,item.ContractDealName
                            ,item.StageName

                            ,item.ValutaName

                            ,item.DeliveryCostPlan
                            ,item.PaymentCostPlan
                            ,item.DeliveryCostFact
                            ,item.PaymentCostFact

                            ,DeliveryCostPlanFact = Convert.ToDecimal(item.DeliveryCostPlan) - Convert.ToDecimal(item.DeliveryCostFact)
                            ,PaymentCostPlanFact = Convert.ToDecimal(item.PaymentCostPlan) - Convert.ToDecimal(item.PaymentCostFact)
                            ,ResultPlanFact = Convert.ToDecimal(DeliveryCostPlanFact) - Convert.ToDecimal(PaymentCostPlanFact)
                        };
            */

            //XPCollection<Spravka0102> xpColSpravka0102 = new XPCollection<Spravka0102>(ssn);

            //XPCollection xpColSpravka0102 = new XPCollection(ssn, typeof(Spravka0102));
            List<BaseReportHelper> xpColSpravka0102 = new List<BaseReportHelper>();

            
            // Для каждой группы формируем запись
            foreach (var grp in registerGroups) {
                foreach (var item in grp.RegGroup) {

                    Spravka0102 sprObj = new Spravka0102(ssn);

                    //sprObj.Oid = item.Oid;

                    sprObj.PrimaryPartyOid = grp.PrimaryPartyOid;
                    sprObj.ContragentPartyOid = grp.ContragentPartyOid;
                    sprObj.ContractOid = grp.ContractOid;
                    sprObj.ContractDealOid = grp.ContractDealOid;
                    sprObj.ObligationUnitOid = grp.ObligationUnitOid;
                    sprObj.StageOid = grp.StageOid;

                    sprObj.ObligationUnitDateTime = grp.ObligationUnitDateTime;
                    sprObj.ObligationUnitCode = grp.ObligationUnitCode;

                    sprObj.PrimaryPartyName = grp.PrimaryPartyName;    //item.PrimaryParty.PartyName;
                    sprObj.ContragentPartyName = grp.ContragentPartyName;    //item.ContragentParty.PartyName;
                    sprObj.ContractName = grp.ContractName;
                    sprObj.ContractDealName = grp.ContractDealName;   // item.ContractDeal.ContractDocument.FullName;
                    sprObj.StageName = grp.StageName;

                    sprObj.ValutaName = grp.ValutaName;

                    // Вычислить 4-е величины
                    sprObj.DeliveryCostPlan = grp.DeliveryCostPlan;
                    sprObj.PaymentCostPlan = grp.PaymentCostPlan;
                    sprObj.DeliveryCostFact = grp.DeliveryCostFact;
                    sprObj.PaymentCostFact = grp.PaymentCostFact;


                    sprObj.DeliveryCostPlanFact = Convert.ToDecimal(sprObj.DeliveryCostPlan) - Convert.ToDecimal(sprObj.DeliveryCostFact);
                    sprObj.PaymentCostPlanFact = Convert.ToDecimal(sprObj.PaymentCostPlan) - Convert.ToDecimal(sprObj.PaymentCostFact);
                    sprObj.ResultPlanFact = Convert.ToDecimal(sprObj.DeliveryCostPlanFact) - Convert.ToDecimal(sprObj.PaymentCostPlanFact);


                    xpColSpravka0102.Add(sprObj);

                }
            }
            
            /*
            // Для каждой группы формируем запись
            foreach (var grp in results) {
//                foreach (var item in results) {

                    Spravka0102 sprObj = new Spravka0102(ssn);

                    //sprObj.Oid = item.Oid;

                    sprObj.PrimaryPartyOid = grp.PrimaryPartyOid;
                    sprObj.ContragentPartyOid = grp.ContragentPartyOid;
                    sprObj.ContractOid = grp.ContractOid;
                    sprObj.ContractDealOid = grp.ContractDealOid;
                    sprObj.ObligationUnitOid = grp.ObligationUnitOid;
                    sprObj.StageOid = grp.StageOid;

                    sprObj.ObligationUnitDateTime = grp.ObligationUnitDateTime;
                    sprObj.ObligationUnitCode = grp.ObligationUnitCode;

                    sprObj.PrimaryPartyName = grp.PrimaryPartyName;    //item.PrimaryParty.PartyName;
                    sprObj.ContragentPartyName = grp.ContragentPartyName;    //item.ContragentParty.PartyName;
                    sprObj.ContractName = grp.ContractName;
                    sprObj.ContractDealName = grp.ContractDealName;   // item.ContractDeal.ContractDocument.FullName;
                    sprObj.StageName = grp.StageName;

                    sprObj.ValutaName = grp.ValutaName;

                    // Вычислить 4-е величины
                    sprObj.DeliveryCostPlan = grp.DeliveryCostPlan;
                    sprObj.PaymentCostPlan = grp.PaymentCostPlan;
                    sprObj.DeliveryCostFact = grp.DeliveryCostFact;
                    sprObj.PaymentCostFact = grp.PaymentCostFact;


                    sprObj.DeliveryCostPlanFact = Convert.ToDecimal(sprObj.DeliveryCostPlan) - Convert.ToDecimal(sprObj.DeliveryCostFact);
                    sprObj.PaymentCostPlanFact = Convert.ToDecimal(sprObj.PaymentCostPlan) - Convert.ToDecimal(sprObj.PaymentCostFact);
                    sprObj.ResultPlanFact = Convert.ToDecimal(sprObj.DeliveryCostPlanFact) - Convert.ToDecimal(sprObj.PaymentCostPlanFact);


                    xpColSpravka0102.Add(sprObj);

//                }
            }
            */

            return xpColSpravka0102;
        }

        
        #endregion

    }
}