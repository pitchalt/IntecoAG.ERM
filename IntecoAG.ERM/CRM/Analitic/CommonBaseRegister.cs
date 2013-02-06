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

using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{
    public enum PlaneFact {
        PLAN = 1,
        FACT = 2
    }

    // Этот класс содержит как бы базис "фазового" пространства аналитических признаков.

    /// <summary>
    /// Класс crmBaseRegister
    /// </summary>
    [NonPersistent]
    public class crmCommonBaseRegister : crmBaseRegister
    {
        public crmCommonBaseRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmContract _Contract;
        /// <summary>
        /// Contract
        /// Договор
        /// </summary>
        public crmContract Contract {
            get { return _Contract; }
            set {
                SetPropertyValue<crmContract>("Contract", ref _Contract, value);
            }
        }

        private crmContractDeal _ContractDeal;
        /// <summary>
        /// ContractDeal
        /// Простой договор (Ведомость)	Договор, по которому непосредственно осуществляется выполнение обязательств.
        /// </summary>
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set {
                SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value);
            }
        }

        private crmStage _Stage;
        /// <summary>
        /// Stage
        /// Этап
        /// </summary>
        public crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        }

        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _fmOrder;
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmOrder>("Order", ref _fmOrder, value);
            }
        }

        // FinancialDeal - временно вместо него Stage с типом Finance
        private crmFinancialDeal _FinancialDeal;
        /// <summary>
        /// FinancialDeal
        /// Финансовая сделка
        /// </summary>
        public crmFinancialDeal FinancialDeal {
            get { return _FinancialDeal; }
            set { SetPropertyValue<crmFinancialDeal>("FinancialDeal", ref _FinancialDeal, value); }
        }


        /// <summary>
        /// CostItem
        /// Статья затрат (ДДС)
        /// </summary>
        private fmCostItem _CostItem;
        public fmCostItem CostItem {
            get { return _CostItem; }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        private SubjectClass _Subject;
        /// <summary>
        /// Subject
        /// Тема
        /// </summary>
        public SubjectClass Subject {
            get { return _Subject; }
            set {
                SetPropertyValue<SubjectClass>("Subject", ref _Subject, value);
            }
        }

        private PlaneFact _PlaneFact;
        /// <summary>
        /// PlaneFact
        /// План/Факт
        /// </summary>
        public PlaneFact PlaneFact {
            get { return _PlaneFact; }
            set {
                SetPropertyValue<PlaneFact>("PlaneFact", ref _PlaneFact, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}