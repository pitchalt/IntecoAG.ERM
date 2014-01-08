using System;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Nomenclature;
using IntecoAG.ERM.Trw.Exchange;
//
namespace IntecoAG.ERM.Trw.Contract {

    [Persistent("TrwOrder")]
    [DefaultProperty("TrwCode")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public class TrwOrder : csCComponent, TrwIOrder, TrwExchangeIExportableObject {
        public TrwOrder(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
            TrwExportStateSet(TrwExchangeExportStates.CREATED);
            TrwOrderWorkType = TrwOrderWorkType.WORK_TYPE_UNKNOW;
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        private fmCSubject _Subject;
        [Association("fmSubject-TrwOrders")]
        [VisibleInListView(true)]
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                if (IsLoading) {
                    _Subject = value;
                } else {
                    fmCSubject old = _Subject;
                    if (old != value) {
                        _Subject = value;
                        OnChanged("Subject", old, value);
                        if (value != null) {
                            TrwInternalNumberSet(value.GetNextOrderNumber());
                            UpdatePropertys();
                        }
                    }
                }
            }
        }

        private crmContractDeal _Deal;
        [Association("crmDeal-TrwOrders")]
        [VisibleInListView(true)]
        [RuleRequiredField("", "Confirm;Save", TargetCriteria = "This.TrwContractInt == Null")]
        [Indexed(new String[]{"Subject", "TrwContractInt"}, Name = "Subject-Deal-Contract", Unique = true)]
        public crmContractDeal Deal {
            get { return _Deal; }
            set {
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
                if (!IsLoading && value != null) {
                    UpdatePropertys();
                }
            }
        }
        //
        [Persistent("TrwInternalNumber")]
        [Indexed("Subject", Name = "Subject-InternalNumber", Unique = true)]
        private Int32 _TrwInternalNumber;
        //
        [PersistentAlias("_TrwInternalNumber")]
        public Int32 TrwInternalNumber {
            get { return _TrwInternalNumber; }
        }
        public void TrwInternalNumberSet(Int32 value) {
            if (_TrwInternalNumber != value) {
                Int32 old = _TrwInternalNumber;
                _TrwInternalNumber = value;
                OnChanged("TrwInternalNumber", old, value);
            }
        }
        //
        [Persistent("TrwCode")]
        private String _TrwCode;
        //
        [PersistentAlias("_TrwCode")]
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public String TrwCode {
            get { return _TrwCode; }
        }
        [PersistentAlias("_TrwCode")]
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public String TrwInternalCode {
            get { return _TrwCode; }
        }
        public void TrwCodeSet(String code) {
            if (_TrwCode != code) {
                String old = _TrwCode;
                _TrwCode = code;
                OnChanged("TrwCode", old, code);
            }
        }

        private TrwContract _TrwContract;
        [Persistent("TrwContract")]
        [Association("TrwContract-TrwOrder")]
        public TrwContract TrwContractInt {
            get { return _TrwContract; }
            set { 
                SetPropertyValue<TrwContract>("TrwContractInt", ref _TrwContract, value);
                if (!IsLoading && value != null) {
                    UpdatePropertys();
                }
            }
        }

        //[PersistentAlias("Deal")]
        public TrwIContract TrwContract {
            get {
                if (TrwContractInt == null)
                    return Deal;
                else
                    return TrwContractInt;
            }
        }

        private TrwOrderWorkType _TrwOrderWorkType;
        [RuleValueComparison("", "Confirm", ValueComparisonType.NotEquals, TrwOrderWorkType.WORK_TYPE_UNKNOW)]
        public TrwOrderWorkType TrwOrderWorkType {
            get { return _TrwOrderWorkType; }
            set { SetPropertyValue<TrwOrderWorkType>("TrwOrderWorkType", ref _TrwOrderWorkType, value); }
        }
        public String TrwOrderWorkTypeCode {
            get { return TrwOrderWorkTypeLogic.GetOrderWorkTypeCode(TrwOrderWorkType); }
        }
        public String TrwFinWorkTypeCode {
            get { return TrwOrderWorkTypeLogic.GetFinWorkTypeCode(TrwOrderWorkType); }
        }

        [Association("TrwOrder-TrwSaleNomenclature")]
        public XPCollection<TrwSaleNomenclature> TrwSaleNomenclatures {
            get { return GetCollection<TrwSaleNomenclature>("TrwSaleNomenclatures"); }
        }

        private DateTime _TrwDateFrom;
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public DateTime TrwDateFrom {
            get { return _TrwDateFrom; }
            set { SetPropertyValue("TrwDateFrom", ref _TrwDateFrom, value); }
        }

        private DateTime _TrwDateToPlan;
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public DateTime TrwDateToPlan {
            get { return _TrwDateToPlan; }
            set { SetPropertyValue("TrwDateToPlan", ref _TrwDateToPlan, value); }
        }

        private DateTime _TrwDateToFact;
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public DateTime TrwDateToFact {
            get { return _TrwDateToFact; }
            set { SetPropertyValue("TrwDateToFact", ref _TrwDateToFact, value); }
        }

        [Persistent("TrwExportState")]
        private TrwExchangeExportStates _TrwExportState;
        [PersistentAlias("_TrwExportState")]
        public TrwExchangeExportStates TrwExportState {
            get { return _TrwExportState; }
            //            set { SetPropertyValue<TrwExchangeExportStates>("TrwExportState", ref _TrwExportState, value); }
        }

        public void TrwExportStateSet(TrwExchangeExportStates state) {
            if (_TrwExportState != state) {
                TrwExchangeExportStates old = _TrwExportState;
                _TrwExportState = state;
                OnChanged("TrwExportState", old, state);
            }
        }

        public void UpdatePropertys() {
            if (Subject != null) {
                TrwCodeSet(Subject.TrwCode + "/" + TrwInternalNumber);
                if (Subject.AnalitycWorkType != null)
                    TrwOrderWorkType = Subject.AnalitycWorkType.TrwOrderWorkType;
                else
                    TrwOrderWorkType = TrwOrderWorkType.WORK_TYPE_UNKNOW;
            }
            if (TrwContract != null) {
                TrwDateFrom = TrwContract.TrwDateValidFrom;
                TrwDateToPlan = TrwContract.TrwDateValidToPlan;
                TrwDateToFact = TrwContract.TrwDateValidToFact;
            }
        }

        [Action(PredefinedCategory.RecordEdit, TargetObjectsCriteria = "TrwExportState == 'CREATED' || TrwExportState == 'REJECTED' || TrwExportState == 'PREPARED' ")]
        public void Refresh() {
            UpdatePropertys();
        }
        [Action(PredefinedCategory.RecordEdit, TargetObjectsCriteria = "TrwExportState == 'CREATED' || TrwExportState == 'REJECTED' || TrwExportState == 'PREPARED' ")]
        public void Confirm() {
            switch (TrwExportState) {
                case TrwExchangeExportStates.CREATED:
                case TrwExchangeExportStates.REJECTED:
                    Validator.RuleSet.Validate(this, "Confirm");
                    TrwExportStateSet(TrwExchangeExportStates.PREPARED);
                    break;
                case TrwExchangeExportStates.PREPARED:
                    TrwExportStateSet(TrwExchangeExportStates.CONFIRMED);
                    break;
                default:
                    break;
            }
        }
    }
}
