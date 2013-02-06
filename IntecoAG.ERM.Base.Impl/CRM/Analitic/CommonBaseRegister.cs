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

using DevExpress.Xpo;

using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
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
    public abstract class crmCommonBaseRegister : crmBaseRegister
    {
        public crmCommonBaseRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        private crmContract _Contract;
        private crmContractDeal _ContractDeal;
        private crmStage _Stage;
        private fmCOrder _fmOrder;
        private crmFinancialDeal _FinancialDeal;
        private fmCostItem _CostItem;
        private fmCSubject _Subject;
        private PlaneFact _PlaneFact;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Договор
        /// </summary>
        public crmContract Contract {
            get { return _Contract; }
            set {
                SetPropertyValue<crmContract>("Contract", ref _Contract, value);
            }
        }

        /// <summary>
        /// Простой договор (Ведомость)	Договор, по которому непосредственно осуществляется выполнение обязательств.
        /// </summary>
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set {
                SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value);
            }
        }

        /// <summary>
        /// Этап
        /// </summary>
        public crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        }

        /// <summary>
        /// Ссылка на Заказ
        /// </summary>
        public fmCOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _fmOrder, value);
            }
        }

        // FinancialDeal - временно вместо него Stage с типом Finance
        /// <summary>
        /// Финансовая сделка
        /// </summary>
        public crmFinancialDeal FinancialDeal {
            get { return _FinancialDeal; }
            set { SetPropertyValue<crmFinancialDeal>("FinancialDeal", ref _FinancialDeal, value); }
        }


        /// <summary>
        /// Статья затрат (ДДС)
        /// </summary>
        public fmCostItem CostItem {
            get { return _CostItem; }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        /// <summary>
        /// Тема
        /// </summary>
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
            }
        }

        /// <summary>
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