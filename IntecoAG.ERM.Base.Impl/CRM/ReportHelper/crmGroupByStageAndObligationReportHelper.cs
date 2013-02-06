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

using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.CRM.ReportHelper
{

    /// <summary>
    /// Класс crmGroupByStageAndObligationReportHelper для создания отчёта "Вариант справки группировка обязательств по этапам и поставкам"
    /// </summary>
    //[NonPersistent]
    [VisibleInReports]
    [Persistent("crmGroupByStageAndObligationReportHelper")]
    public class crmGroupByStageAndObligationReportHelper : BaseObject
    {
        public crmGroupByStageAndObligationReportHelper(Session ses) : base(ses) { }
/*
        public crmGroupByStageAndObligationReportHelper(Session ses,
            Guid prmObligationUnitOid,
            DateTime prmObligationUnitDateTime,
            string prmObligationUnitCode,
            string prmPrimaryPartyName,
            string prmContragentPartyName,
            string prmContractName,
            string prmContractDealName,
            Guid prmStageOid,
            string prmStageName,
            decimal prmDebetPlan,
            decimal prmCreditPlan,
            decimal prmDebetFact,
            decimal prmCreditFact,
            string prmDebitCreditValutaName,
            decimal prmDebetInRURPlan,
            decimal prmCreditInRURPlan,
            decimal prmDebetInRURFact,
            decimal prmCreditInRURFact
            ) : base(ses) {

            ObligationUnitOid = prmObligationUnitOid;
            ObligationUnitDateTime = prmObligationUnitDateTime;
            ObligationUnitCode = prmObligationUnitCode;
            PrimaryPartyName = prmPrimaryPartyName;
            ContragentPartyName = prmContragentPartyName;
            ContractName = prmContractName;
            ContractDealName = prmContractDealName;
            StageOid = prmStageOid;
            StageName = prmStageName;
            DebetPlan = prmDebetPlan;
            CreditPlan = prmCreditPlan;
            DebetFact = prmDebetFact;
            CreditFact = prmCreditFact;
            DebitCreditValutaName = prmDebitCreditValutaName;
            DebetInRURPlan = prmDebetInRURPlan;
            CreditInRURPlan = prmCreditInRURPlan;
            DebetInRURFact = prmDebetInRURFact;
            CreditInRURFact = prmCreditInRURFact;

        }
*/
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

        private string _PrimaryPartyName;
        /// <summary>
        /// PrimaryPartyName - Организация
        /// Наименование первичного контрагента
        /// </summary>
        public string PrimaryPartyName {
            get { return _PrimaryPartyName; }
            set { SetPropertyValue<string>("PrimaryPartyName", ref _PrimaryPartyName, value); }
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

        // Формирование коллекции для отчёта
        public static XPCollection<crmGroupByStageAndObligationReportHelper> CreateReportDataSource(Session ssn, crmPartyRu prmContragentParty) {

            XPCollection<crmGroupByStageAndObligationReportHelper> Res = new XPCollection<crmGroupByStageAndObligationReportHelper>(ssn);

            CriteriaOperator op = null;

            if (prmContragentParty != null) {
                OperandProperty prop = new OperandProperty("PrimaryParty");
                op = prop == prmContragentParty;
            }

            XPCollection<crmDebtorCreditorDebtRegister> dbRegister = new XPCollection<crmDebtorCreditorDebtRegister>(ssn, op, null);
            dbRegister.Criteria = op;
            dbRegister.Reload();
            if (!dbRegister.IsLoaded) dbRegister.Load();

            // Справка устроена таким образом. На каждое обязательство имеется совокупность троек (дата, план, факт). В каждой
            // такой тройке дата всегда заполнена, а из план или факт заполнен только один из них
            // Это означает, что идёт цикл по всем обязательствам и для каждого из них формируются записи с указанными тройками внутри

            IList<Guid> obligationGuidList = new List<Guid>();
            foreach (crmDebtorCreditorDebtRegister item in dbRegister) {
                if (item.ObligationUnit != null && !obligationGuidList.Contains(item.ObligationUnit.Oid) )
                    obligationGuidList.Add(item.ObligationUnit.Oid);
            }

            // Из-за малого объёма коллекции dbRegister оптимизация не нужна

            foreach (Guid obligationGuid in obligationGuidList) {
                foreach (crmDebtorCreditorDebtRegister item in dbRegister) {
                    if (item.ObligationUnit.Oid == obligationGuid) {

                        crmGroupByStageAndObligationReportHelper obj = new crmGroupByStageAndObligationReportHelper(ssn);

                        obj.ObligationUnitOid = item.ObligationUnit.Oid;
                        obj.ObligationUnitDateTime = item.ObligationUnitDateTime;
                        obj.ObligationUnitCode = item.ObligationUnit.Code;
                        obj.PrimaryPartyName = item.PrimaryParty.PartyName;
                        obj.ContragentPartyName = item.ContragentParty.PartyName;
                        obj.ContractName = item.Contract.Delo;
                        obj.ContractDealName = item.ContractDeal.Name;   // item.ContractDeal.ContractDocument.FullName;
                        obj.StageOid = item.Stage.Oid;
                        obj.StageName = item.Stage.Code;

                        if (item.PlaneFact == PlaneFact.PLAN) {

                            obj.DebetPlan = item.DebitCost;
                            obj.DebetInRURPlan = item.DebitCostInRUR;
                            obj.CreditPlan = item.CreditCost;
                            obj.CreditInRURPlan = item.CreditCostInRUR;
                            obj.DebitCreditValutaName = item.CreditValuta.Code;

                            obj.DebetFact = 0;
                            obj.CreditFact = 0;
                            obj.DebetInRURFact = 0;
                            obj.CreditInRURFact = 0;

                        } else if (item.PlaneFact == PlaneFact.FACT) {

                            obj.DebetFact = item.DebitCost;
                            obj.CreditFact = item.CreditCost;
                            obj.DebetInRURFact = item.DebitCostInRUR;
                            obj.CreditInRURFact = item.CreditCostInRUR;
                            obj.DebitCreditValutaName = item.CreditValuta.Code;

                            obj.DebetPlan = 0;
                            obj.CreditPlan = 0;
                            obj.DebetInRURPlan = 0;
                            obj.CreditInRURPlan = 0;

                        }

                        Res.Add(obj);

                    }
                }
            }


            /*
            foreach (crmDebtorCreditorDebtRegister item in dbRegister) {

                // Здесь надо вести поиски. Нельзя брать только План или только Факт, т.к. они могут не быть попарными
                // Если в текущей записи План, то ищем соответствующий Факт и наоборот
                // Поиск ведётся в пределах той же коллекции dbRegister. Ключом для поиска является 
                // совокупность полей: item.Stage.Oid, item.ObligationUnit

                if (item.PlaneFact == PlaneFact.PLAN) {

                    crmGroupByStageAndObligationReportHelper obj = new crmGroupByStageAndObligationReportHelper(ssn);

                    obj.ObligationUnitOid = item.ObligationUnit.Oid;
                    obj.ObligationUnitDateTime = item.ObligationUnitDateTime;
                    obj.ObligationUnitCode = item.ObligationUnit.Code;
                    obj.PrimaryPartyName = item.PrimaryParty.PartyName;
                    obj.ContragentPartyName = item.ContragentParty.PartyName;
                    obj.ContractName = item.Contract.Delo;
                    obj.ContractDealName = item.ContractDeal.ContractDocument.FullName;
                    obj.StageOid = item.Stage.Oid;
                    obj.StageName = item.Stage.Code;

                    obj.DebetPlan = item.DebitCost;
                    obj.DebetInRURPlan = item.DebitCostInRUR;
                    obj.CreditPlan = item.CreditCost;
                    obj.CreditInRURPlan = item.CreditCostInRUR;
                    obj.DebitCreditValutaName = item.CreditValuta.Code;

                    // Обойдёмся без LINQ
                    foreach (crmDebtorCreditorDebtRegister itemDual in dbRegister) {
                        if (itemDual.PlaneFact == PlaneFact.FACT
                            && item.ObligationUnit.Oid == itemDual.ObligationUnit.Oid
                            && item.Stage.Oid == itemDual.Stage.Oid) {
                            
                            obj.DebetFact = itemDual.DebitCost;
                            obj.CreditFact = itemDual.CreditCost;
                            obj.DebetInRURFact = itemDual.DebitCostInRUR;
                            obj.CreditInRURFact = itemDual.CreditCostInRUR;

                            break;
                        }
                    }
                    Res.Add(obj);

                } else if (item.PlaneFact == PlaneFact.FACT) {

                    crmGroupByStageAndObligationReportHelper obj = new crmGroupByStageAndObligationReportHelper(ssn);

                    obj.ObligationUnitOid = item.ObligationUnit.Oid;
                    obj.ObligationUnitDateTime = item.ObligationUnitDateTime;
                    obj.ObligationUnitCode = item.ObligationUnit.Code;
                    obj.PrimaryPartyName = item.PrimaryParty.PartyName;
                    obj.ContragentPartyName = item.ContragentParty.PartyName;
                    obj.ContractName = item.Contract.Delo;
                    obj.ContractDealName = item.ContractDeal.ContractDocument.FullName;
                    obj.StageOid = item.Stage.Oid;
                    obj.StageName = item.Stage.Code;

                    obj.DebetFact = item.DebitCost;
                    obj.CreditFact = item.CreditCost;
                    obj.DebetInRURFact = item.DebitCostInRUR;
                    obj.CreditInRURFact = item.CreditCostInRUR;
                    obj.DebitCreditValutaName = item.CreditValuta.Code;

                    // Обойдёмся без LINQ
                    foreach (crmDebtorCreditorDebtRegister itemDual in dbRegister) {
                        if (itemDual.PlaneFact == PlaneFact.PLAN
                            && item.ObligationUnit.Oid == itemDual.ObligationUnit.Oid
                            && item.Stage.Oid == itemDual.Stage.Oid) {
                            
                            obj.DebetPlan = itemDual.DebitCost;
                            obj.CreditPlan = itemDual.CreditCost;
                            obj.DebetInRURPlan = itemDual.DebitCostInRUR;
                            obj.CreditInRURPlan = itemDual.CreditCostInRUR;

                            break;
                        }
                    }
                    Res.Add(obj);

                }

            }
            */

            return Res;
        }

        #endregion

    }
}