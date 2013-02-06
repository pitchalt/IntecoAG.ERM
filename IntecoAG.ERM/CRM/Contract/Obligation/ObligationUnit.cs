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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;


namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// 
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Code")]
    [Persistent("crmObligationUnit")]
    public abstract partial class crmObligationUnit : VersionRecord, IVersionSupport
    {
        public crmObligationUnit(Session session) : base(session) { }
        public crmObligationUnit(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.CurrentCost = new crmCostCol(this.Session);
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.CurrentCost.VersionState = this.VersionState;
        }

        #region ���� ������

        #endregion

        #region �������� ������

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Code;
        [Size(20)]
        [RuleRequiredField]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        /// <summary>
        /// Name - ������������
        /// </summary>
        private string _Name;
        [Size(50)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
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

        /// <summary>
        /// DatePlane - ����������� ���� ������
        /// <summary>
        private DateTime _DatePlane;
        [RuleRequiredField("crmObligationUnit.DatePlan.Required", "Save")]
        public DateTime DatePlane {
            get { return _DatePlane; }
            set { SetPropertyValue<DateTime>("DatePlane", ref _DatePlane, value); }
        }

        /// <summary>
        /// crmOrder ������ �� �����
        /// </summary>
        private fmOrder _Order;
        public virtual fmOrder Order {
            get { return _Order; }
            set { 
                SetPropertyValue<fmOrder>("Order", ref _Order, value);
            }
        }
        // ------
        /// <summary>
        /// CostItem
        /// </summary>
        private fmCostItem _CostItem;
        public virtual fmCostItem CostItem {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        // ����������, �� �� ��������/�����������
        // private crmContractParty _Debitor;
        public abstract crmContractParty Debitor {
            get;
        }

        // ���������� ������, �� �� ������������/���������
        // private crmContractParty _Creditor;
        public abstract crmContractParty Creditor {
            get;
        }

        // ------

        // ����������, �� �� ��������/�����������
        //private crmPhysicalPerson _PersonSender;
        [Delayed]
        [DataSourceProperty("DealVersion.Partys")]
        public crmContractParty Sender {
            //get { return _PersonSender; }
            //set { SetPropertyValue<crmPhysicalPerson>("PersonSender", ref _PersonSender, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Sender"); }
            set { SetDelayedPropertyValue<crmContractParty>("Sender", value); }
        }

        // ���������� ������, �� �� ������������/���������
        //private crmPhysicalPerson _PersonReceiver;
        [Delayed]
        [DataSourceProperty("DealVersion.Partys")]
        public crmContractParty Receiver {
            //get { return _PersonReceiver; }
            //set { SetPropertyValue<crmPhysicalPerson>("PersonReceiver", ref _PersonReceiver, value); }
            get { return GetDelayedPropertyValue<crmContractParty>("Receiver"); }
            set { SetDelayedPropertyValue<crmContractParty>("Receiver", value); }
        }

        public abstract crmStage Stage {
            get;
        }
        // ///<summary>
        // ///Stage �������� � �����
        // ///</summary>
        //private crmStage _Stage;
        //public virtual crmStage Stage {
        //    get { return _Stage; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        //}

        [NonPersistent]
        public virtual csNDSRate NDSRate {
            get { return this.CurrentCost.NDSRate; }
            set {
                this.CurrentCost.NDSRate = value;
                OnChanged("NDSRate");
            }
        }
        [NonPersistent]
        public virtual csValuta Valuta {
            get { return this.CurrentCost.Valuta; }
            set {
                this.CurrentCost.Valuta = value;
                OnChanged("Valuta");
            }
        }
        //
        [NonPersistent]
        public virtual crmCostModel CostModel {
            get { return this.CurrentCost.CostModel; }
            set {
                this.CurrentCost.CostModel = value;
                OnChanged("CostModel");
            }
        }
        //
        [NonPersistent]
        public virtual decimal SummCost {
            get { return this.CurrentCost.SummCost; }
            //set {
            //    this.CurrentCost.SummCost = value;
            //    OnChanged("SummCost");
            //}
        }
        //
        [NonPersistent]
        public virtual decimal SummNDS {
            get { return this.CurrentCost.SummNDS; }
            //set {
            //    this.CurrentCost.SummNDS = value;
            //    OnChanged("SummCost");
            //}
        }
        //
        [NonPersistent]
        public virtual decimal SummFull {
            get { return this.CurrentCost.SummFull; }
            set { }
        }
        //
        private crmCostCol _CurrentCost;
        [Aggregated]
        [Browsable(false)]
//        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public crmCostCol CurrentCost {
            get { return _CurrentCost; }
            set { SetPropertyValue<crmCostCol>("CurrentCost", ref _CurrentCost, value); }
        }
        #endregion


        #region ������ ������
        //void UpdateCurrentCost(crmCostModel cm, csValuta vl) {
        //    CurrentCost = null;
        //    if (cm == null || vl == null) return;
        //    foreach (crmCostItem sp in ObligationUnitCosts) {
        //        if (sp.CostModel == cm && sp.Valuta == vl) {
        //            CurrentCost = sp;
        //            break;
        //        }
        //    }
        //    if (CurrentCost == null) {
        //        CurrentCost = new crmCostItem(this.Session);
        //        CurrentCost.CostModel = cm;
        //        CurrentCost.Valuta = vl;
        //        ObligationUnitCosts.Add(CurrentCost);
        //    }
        //}

        //public void UpdateCost(crmCostItem sp, Boolean mode) {
        //    if (sp == null) return;
        //    crmCostItem old_cost = this.CurrentCost;
        //    UpdateCurrentCost(sp.CostModel, sp.Valuta);
        //    if (this.CurrentCost != null) {
        //        this.CurrentCost.UpdateCost(sp, mode);
        //        this.CurrentCost = old_cost;
        //    }
        //    if (Stage != null)
        //        Stage.UpdateCost(sp, mode);

        //}


        ///// <summary>
        ///// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}


        public crmObligationUnitMain GetObligationUnitMain() {
            if (this.ObligationUnitMain == null) {
                this.ObligationUnitMain = new crmObligationUnitMain(this.Session);
                this.ObligationUnitMain.Current = this;
                this.ObligationUnitMain.ContractDeal = this.DealVersion.ContractDeal;
            }
            return this.ObligationUnitMain;
        }

        #endregion

        #region IVersionMainObject

        public VersionRecord GetCurrent() {
            return this;
        }

        #endregion

    }

}