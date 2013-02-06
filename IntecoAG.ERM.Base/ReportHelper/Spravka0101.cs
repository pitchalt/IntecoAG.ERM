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
using System.Collections;
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
    public class Spravka0101 : BaseReportHelper
    {
        public Spravka0101(Session ssn) : base(ssn) { }

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


        private decimal _DebetPlan;
        /// <summary>
        /// DebetPlan - План дебета
        /// План дебета
        /// </summary>
        public decimal DebetPlan {
            get { return _DebetPlan; }
            set { SetPropertyValue<decimal>("DebetPlan", ref _DebetPlan, value); }
        }

        private decimal _CreditPlan;
        /// <summary>
        /// CreditPlan - План кредита
        /// План кредита
        /// </summary>
        public decimal CreditPlan {
            get { return _CreditPlan; }
            set { SetPropertyValue<decimal>("CreditPlan", ref _CreditPlan, value); }
        }


        private decimal _DebetFact;
        /// <summary>
        /// DebetFact - Факт дебета
        /// Факт дебета
        /// </summary>
        public decimal DebetFact {
            get { return _DebetFact; }
            set { SetPropertyValue<decimal>("DebetFact", ref _DebetFact, value); }
        }

        private decimal _CreditFact;
        /// <summary>
        /// CreditFact - Факт кредита
        /// Факт кредита
        /// </summary>
        public decimal CreditFact {
            get { return _CreditFact; }
            set { SetPropertyValue<decimal>("CreditFact", ref _CreditFact, value); }
        }

        private string _DebitCreditValutaName;
        /// <summary>
        /// DebitCreditValutaName
        /// Валюта договора
        /// </summary>
        public string DebitCreditValutaName {
            get { return _DebitCreditValutaName; }
            set {
                SetPropertyValue<string>("DebitCreditValutaName", ref _DebitCreditValutaName, value);
            }
        }


        //---


        private decimal _DebetInRURPlan;
        /// <summary>
        /// DebetInRURPlan - План дебета
        /// План дебета
        /// </summary>
        public decimal DebetInRURPlan {
            get { return _DebetInRURPlan; }
            set { SetPropertyValue<decimal>("DebetInRURPlan", ref _DebetInRURPlan, value); }
        }

        private decimal _CreditInRURPlan;
        /// <summary>
        /// CreditInRURPlan - План кредита
        /// План кредита
        /// </summary>
        public decimal CreditInRURPlan {
            get { return _CreditInRURPlan; }
            set { SetPropertyValue<decimal>("CreditInRURPlan", ref _CreditInRURPlan, value); }
        }


        private decimal _DebetInRURFact;
        /// <summary>
        /// DebetInRURFact - Факт дебета
        /// Факт дебета
        /// </summary>
        public decimal DebetInRURFact {
            get { return _DebetInRURFact; }
            set { SetPropertyValue<decimal>("DebetInRURFact", ref _DebetInRURFact, value); }
        }

        private decimal _CreditInRURFact;
        /// <summary>
        /// CreditInRURFact - Факт кредита
        /// Факт кредита
        /// </summary>
        public decimal CreditInRURFact {
            get { return _CreditInRURFact; }
            set { SetPropertyValue<decimal>("CreditInRURFact", ref _CreditInRURFact, value); }
        }

        #endregion


        #region МЕТОДЫ

        public override List<BaseReportHelper> CreateReportListSource(Session ssn, CriteriaOperator criteria) {

            XPClassInfo classInfo = ssn.GetClassInfo(this.GetType());

            XPCollection<crmDebtorCreditorDebtRegister> dbRegister = new XPCollection<crmDebtorCreditorDebtRegister>(ssn, criteria, null);
            dbRegister.Criteria = criteria;
            dbRegister.Reload();
            if (!dbRegister.IsLoaded) dbRegister.Load();

            Session ses = this.Session;
            // Linq по коллекции
            var registerGroups = from registerRow in dbRegister
                                select new Spravka0101 (this.Session)
                                {
                                     ObligationUnitOid = registerRow.ObligationUnit.Oid
                                     ,ObligationUnitDateTime = registerRow.ObligationUnit.DatePlane
                                     ,ObligationUnitCode = registerRow.ObligationUnit.Code

                                     ,PrimaryPartyOid = registerRow.PrimaryParty.Oid
                                     ,PrimaryPartyName = registerRow.PrimaryParty.Person.NameFull
                                     ,ContragentPartyOid = registerRow.ContragentParty.Oid
                                     ,ContragentPartyName = registerRow.ContragentParty.Person.NameFull
                                     ,ContractOid = registerRow.Contract.Oid
                                        //!!!Паша нужно подставит ьправильное значение
                                        //,ContractName = registerRow.Contract.Delo
                                     ,ContractName = " "
                                     ,ContractDealOid = registerRow.ContractDeal.Oid
                                     ,ContractDealName = registerRow.ContractDeal.Name
                                     ,StageOid = registerRow.Stage.Oid
                                     ,StageName = registerRow.Stage.Code


                                     ,DebetPlan = (registerRow.PlaneFact == PlaneFact.PLAN) ? registerRow.DebitCost : 0
                                     ,DebetInRURPlan = (registerRow.PlaneFact == PlaneFact.PLAN) ? registerRow.DebitCostInRUR : 0
                                     ,CreditPlan = (registerRow.PlaneFact == PlaneFact.PLAN) ? registerRow.CreditCost : 0
                                     ,CreditInRURPlan = (registerRow.PlaneFact == PlaneFact.PLAN) ? registerRow.CreditCostInRUR : 0
                                     ,DebitCreditValutaName = registerRow.CreditValuta.Code

                                     ,DebetFact = (registerRow.PlaneFact == PlaneFact.FACT) ? registerRow.DebitCost : 0
                                     ,CreditFact = (registerRow.PlaneFact == PlaneFact.FACT) ? registerRow.CreditCost : 0
                                     ,DebetInRURFact = (registerRow.PlaneFact == PlaneFact.FACT) ? registerRow.DebitCostInRUR : 0
                                     ,CreditInRURFact = (registerRow.PlaneFact == PlaneFact.FACT) ? registerRow.CreditCostInRUR : 0
                                };

            List<BaseReportHelper> xpColSpravka0101 = new List<BaseReportHelper>();

            // Для каждой группы формируем запись
            foreach (var elem in registerGroups) {
                xpColSpravka0101.Add(elem);
            }

            return xpColSpravka0101;
        }

        #endregion

    }
}