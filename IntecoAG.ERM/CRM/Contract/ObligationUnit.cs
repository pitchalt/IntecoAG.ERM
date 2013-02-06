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
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс PaymentUnit, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmObligationUnit")]
    public abstract partial class crmObligationUnit : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmObligationUnit(Session session) : base(session) { }
        public crmObligationUnit(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }

        #region ПОЛЯ КЛАССА

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Code;
        [Size(15)]
        [RuleRequiredField("crmObligationUnit.Code.Required", "Save")]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }
        /// <summary>
        /// crmOrder Ссылка на Заказ
        /// </summary>
        private fmOrder _fmOrder;
        public virtual fmOrder fmOrder {
            get { return _fmOrder; }
            set { 
                SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value);
            }
        }

        /// <summary>
        /// DatePlane - планируемая дата оплаты
        /// <summary>
        private DateTime _DatePlane;
        [RuleRequiredField("crmObligationUnit.DatePlan.Required", "Save")]
        public DateTime DatePlane {
            get { return _DatePlane; }
            set { SetPropertyValue<DateTime>("DatePlane", ref _DatePlane, value); }
        }

        /// <summary>
        /// DateStart
        /// <summary>
        private DateTime _DateBegin;
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }

        /// <summary>
        /// DateStop
        /// </summary>
        private DateTime _DateEnd;
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }

        // Плательщик, он же Заказчик/Полкупатель
        private crmContractParty _Debitor;
        public crmContractParty Debitor {
            get { return _Debitor; }
            set { SetPropertyValue<crmContractParty>("Debitor", ref _Debitor, value); }
        }

        // Получатель оплаты, он же Испольнитель/Поставщиу
        private crmContractParty _Creditor;
        public crmContractParty Creditor {
            get { return _Creditor; }
            set { SetPropertyValue<crmContractParty>("Creditor", ref _Creditor, value); }
        }


        /// <summary>
        /// Stage Привязка к этапу
        /// </summary>
        //private crmStage _Stage;
        //public virtual crmStage Stage {
        //    get { return _Stage; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        //}
        [NonPersistent]
        public virtual crmStage Stage {
            get { return null; }
        }

        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public virtual crmCostModel CostModel {
            get { return _CostModel; }
            set {
                crmCostModel cm = value;
                if (!IsLoading) {
                    //    foreach (crmStage ss in SubStages)
                    //        ss.CurrentCostModel = value;
                    if (Stage != null)
                        cm = Stage.CostModel;
                    UpdateCurrentCost(cm, this.Valuta);
                }
                SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, cm);
            }
        }
        private crmCost _CurrentCost;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmCost CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCost>("CurrentCost", ref _CurrentCost, value); }
        }
        //
        private Valuta _Valuta;
        public virtual Valuta Valuta {
            get { return _Valuta; }
            set {
                Valuta val = value;
                if (!IsLoading) {
                    if (Stage != null)
                        val = Stage.Valuta;
                    UpdateCurrentCost(this.CostModel, val);
                }
                SetPropertyValue<Valuta>("Valuta", ref _Valuta, val);
            }
        }
        //
        [Association("crmObligationUnit-crmCost", typeof(crmCost)), Aggregated]
        public XPCollection<crmCost> ObligationUnitCosts {
            get { return GetCollection<crmCost>("ObligationUnitCosts"); }
        }
        #endregion


        #region МЕТОДЫ КЛАССА
        void UpdateCurrentCost(crmCostModel cm, Valuta vl) {
            CurrentCost = null;
            if (cm == null || vl == null) return;
            foreach (crmCost sp in ObligationUnitCosts) {
                if (sp.CostModel == cm && sp.Valuta == vl) {
                    CurrentCost = sp;
                    break;
                }
            }
            if (CurrentCost == null) {
                CurrentCost = new crmCost(this.Session);
                CurrentCost.CostModel = cm;
                CurrentCost.Valuta = vl;
                ObligationUnitCosts.Add(CurrentCost);
            }
        }

        public void UpdateCost(crmCost sp, Boolean mode) {
            if (sp == null) return;
            crmCost old_cost = this.CurrentCost;
            UpdateCurrentCost(sp.CostModel, sp.Valuta);
            if (this.CurrentCost != null) {
                this.CurrentCost.UpdateCost(sp, mode);
                this.CurrentCost = old_cost;
            }
            if (Stage != null)
                Stage.UpdateCost(sp, mode);

        }


        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}