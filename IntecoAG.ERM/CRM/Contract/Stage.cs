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
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;



//*****************************************************************************************************//
// Из документации.
//*****************************************************************************************************//
// Stage – этап работ 
// - DateBegin, DateEnd – атрибуты задающие дату начала и окончания этапа посредством 
// CS.CRM.Contract..Time этот атрибут отличается от DateStart и DateStop, которые задают 
// абсолютные даты эти атрибуты должны автоматически заполняться через ввод DateBegin и DateEnd
//
//*****************************************************************************************************//

namespace IntecoAG.ERM.CRM.Contract
{
    public enum DisplaySummMode {
        DISPLAY_FULL = 1,
        DISPLAY_COST = 2
    }

    public enum StageType {
        AGREGATE = 1,
        FINANCE = 2,
        TECHNICAL = 3
    }
    /// <summary>
    /// Класс для поддержки структуры этапов проекта
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Code")]
    [Persistent("crmStage")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class crmStage : VersionRecord, ITreeNode// CS.Work.csWork
    {
        public crmStage(Session session) : base(session) { }
        public crmStage(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Дата начала события
        /// </summary>
        private DateTime _DateBegin;
        [RuleRequiredField("crmStage.DateBegin.Required", "Save")]
        //[RuleRequiredField("crmStage.DateBegin.Required.Immediate", "Immediate")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set {
                if (!IsLoading) {
                    // Паша правильно написать условие 
                    if (DateEnd.Year > 1900 && DateEnd < value) {
                        value = DateEnd;
                    }
                    if (TopStage != null) {
                        if (TopStage.DateBegin > value)
                            value = TopStage.DateBegin;
                        if (TopStage.DateEnd < value)
                            value = TopStage.DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
            }
        }

        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateEnd;
        [RuleRequiredField("crmStage.DateEnd.Required", "Save")]
        //[RuleRequiredField("crmStage.DateEnd.Required.Immediate", "Immediate")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                if (!IsLoading) {
                    if (DateBegin > value) {
                        value = DateBegin;
                    }
                    if (TopStage != null) {
                        if (TopStage.DateBegin > value)
                            value = TopStage.DateBegin;
                        if (TopStage.DateEnd < value)
                            value = TopStage.DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
            }
        }
        /// <summary>
        /// Code
        /// </summary>
        private string _Code;
        [Size(10)]
        [RuleRequiredField("crmStage.Code.Required", "Save")]
        //[RuleRequiredField("crmStage.Code.Required.Immediate", "Immediate")]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Name - описание
        /// </summary>
        private string _Name;
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        //
        crmStage _TopStage;
        [Browsable(false)]
        [Association("crmStage-SubStages")]
        public virtual crmStage TopStage {
            get { return _TopStage; }
            set {
                SetPropertyValue<crmStage>("TopStage", ref _TopStage, value);
                if (!IsLoading) {
                    //                    this.TopWork = value;
                    if (value != null) {
                        this.StageStructure = value.StageStructure;
                        if (value.StageType == CRM.Contract.StageType.FINANCE)
                            this.StageType = CRM.Contract.StageType.TECHNICAL;
                        else
                            this.StageType = value.StageType;
                        if (value.CostModel != null)
                            this.CostModel = value.CostModel;
                        if (value.Valuta != null)
                            this.Valuta = value.Valuta;
                        this.DisplaySummMode = value.DisplaySummMode;
                        if (this.DateBegin < value.DateBegin || this.DateBegin > value.DateEnd)
                            this.DateBegin = value.DateBegin;
                        if (this.DateEnd > value.DateEnd || this.DateEnd < value.DateBegin)
                            this.DateEnd = value.DateEnd;
                    }
                }
            }
        }

        [Association("crmStage-SubStages", typeof(crmStage))]
        public virtual XPCollection<crmStage> SubStages {
            get { return GetCollection<crmStage>("SubStages"); }
        }


//        [Browsable(false)]
//        public override CS.Work.csWork TopWork {
//            get { return base.TopWork; }
//            set {
//                base.TopWork = value;
//            }
//        }
        /// <summary>
        /// Code
        /// </summary>
        public string FullCode {
            get {
                if (TopStage == null)
                    return Code;
                else
                    return String.Concat(TopStage.FullCode, ".", Code);
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        private crmCostModel _CostModel;
        public crmCostModel CostModel {
            get { return _CostModel; }
            set {
                if (!IsLoading) {
                    UpdateCurrentCost(value, this.Valuta);
                    foreach (crmStage ss in SubStages)
                        ss.CostModel = value;
                }
                SetPropertyValue<crmCostModel>("CostModel", ref _CostModel, value); 
            }
        }
        private crmCost _CurrentCost;
        [Browsable(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmCost CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCost>("CurrentCost", ref _CurrentCost, value); }
        }
        
        [Association("crmStage-crmCost"), Aggregated]
        public XPCollection<crmCost> StageCosts {
            get { return GetCollection<crmCost>("StageCosts"); }
        }
        /// <summary>
        /// ObligationTransfer Обязательство на данном этапе
        /// </summary>
        private crmDeliveryUnit _Delivery;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmDeliveryUnit Delivery {
            get { return _Delivery; }
            set { SetPropertyValue<crmDeliveryUnit>("Delivery", ref _Delivery, value); }
        }
        /// <summary>
        /// ObligationTransfer Обязательство на данном этапе
        /// </summary>
        [Association("crmStage-DeliveryUnits", typeof(crmDeliveryUnit))]
        public XPCollection<crmDeliveryUnit> DeliveryUnits {
            get { return GetCollection<crmDeliveryUnit>("DeliveryUnits"); }
        }
        /// <summary>
        /// ObligationTransfer Обязательство на данном этапе
        /// </summary>
        private crmPaymentUnit _Payment;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmPaymentUnit Payment {
            get { return _Payment; }
            set { SetPropertyValue<crmPaymentUnit>("Payment", ref _Payment, value); }
        }
        /// <summary>
        /// ObligationTransfer Обязательство на данном этапе
        /// </summary>
        [Association("crmStage-PaymentUnits", typeof(crmPaymentUnit))]
        public XPCollection<crmPaymentUnit> PaymentUnits {
            get { return GetCollection<crmPaymentUnit>("PaymentUnits"); }
        }
        //        [Browsable(false)]
        private DisplaySummMode _DisplaySummMode;
        [VisibleInListView(false)]
        public DisplaySummMode DisplaySummMode {
            get { return _DisplaySummMode; }
            set { 
                SetPropertyValue<DisplaySummMode>("DisplaySummMode", ref _DisplaySummMode, value); 
            }
        }
        private StageType _StageType;
        public StageType StageType {
            get { return _StageType; }
            set {
                if (_StageType == CRM.Contract.StageType.FINANCE) {
                    if (value != CRM.Contract.StageType.FINANCE) {
                        this.DeliveryUnits.Remove(this.Delivery);
                        this.Delivery.Delete();
                        this.Delivery = null;
                    }
                }
                SetPropertyValue<StageType>("StageType", ref _StageType, value);
                if (!IsLoading) {
                    if (_StageType == StageType.FINANCE || _StageType == StageType.TECHNICAL) {
                        foreach (crmStage st in SubStages)
                            st.StageType = CRM.Contract.StageType.TECHNICAL;
                        if (StageStructure.DeliveryPlan != null) {
                            this.Delivery = StageStructure.DeliveryPlan.DeliveryUnitCreate();
                            this.Delivery.Code = this.FullCode;
                            this.Delivery.DatePlane = this.DateEnd;
                            this.DeliveryUnits.Add(this.Delivery);
                        }
                    }
                }
            }
        }

        protected void UpdateCurrentCost(crmCostModel cm, Valuta vl) {
            CurrentCost = null;
            if (cm == null || vl == null) return;
            CurrentCost = CostGet(cm, vl);
        }
        protected crmCost CostGet(crmCostModel cm, Valuta vl) {
            crmCost cost = null;
            if (cm == null || vl == null) return null;
            foreach (crmCost sp in this.StageCosts) {
                if (sp.CostModel == cm && sp.Valuta == vl) {
                    cost = sp;
                    break;
                }
            }
            if (cost == null) {
                cost = new crmCost(this.Session);
                cost.CostModel = cm;
                cost.Valuta = vl;
                StageCosts.Add(cost);
            }
            return cost;
        }

        public virtual decimal Price { 
            get {
                if (CurrentCost == null)
                    return 0;
                if (DisplaySummMode == DisplaySummMode.DISPLAY_COST)
                    return CurrentCost.SummCost;
                else
                    if (DisplaySummMode == DisplaySummMode.DISPLAY_FULL)
                        return CurrentCost.SummFull;
                    else
                        return 0;
            }
        }

        /// <summary>
        /// Valuta
        /// </summary>
        private Valuta _Valuta;
        public Valuta Valuta {
            get { return _Valuta; }
            set {
                if (!IsLoading) {
                    UpdateCurrentCost(this.CostModel, this.Valuta);
                    foreach (crmStage ss in SubStages)
                        ss.Valuta = value;
                    foreach (crmObligationUnit ou in DeliveryUnits)
                        ou.Valuta = value;
                    foreach (crmObligationUnit ou in PaymentUnits)
                        ou.Valuta = value;
                }
                SetPropertyValue<Valuta>("Valuta", ref _Valuta, value);
                // Бага не обновляется текущая стоимость пока ХАК
                if (!IsLoading) {
                    UpdateCurrentCost(this.CostModel, this.Valuta);
                }
            }
        }

        #endregion
        

        #region МЕТОДЫ
        #region ITreeNode Members

        IBindingList ITreeNode.Children {
            get { return SubStages; }
        }

        string ITreeNode.Name {
            get { return Code; }
        }

        ITreeNode ITreeNode.Parent {
            get { return TopStage; }
        }

        #endregion

        protected void UpdateStageCostInt(crmCost sp, Boolean mode) {
            //if (CurrentStageCost != null)
            //    CurrentStageCost.UpdatePrice(sp, mode);
            //UpdateStageCost(sp, mode);
        }

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public void UpdateCost(crmCost sp, Boolean mode)
        {
            crmCost cost = this.CostGet(sp.CostModel, sp.Valuta);
            if (cost != null)
                cost.UpdateCost(sp, mode);
            if (TopStage != null)
                TopStage.UpdateCost(sp, mode);
        }

        #endregion

    }

}