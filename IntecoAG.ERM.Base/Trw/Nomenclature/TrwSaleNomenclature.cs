using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Validation;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Exchange;
using IntecoAG.XafExt.Bpmn;
//
namespace IntecoAG.ERM.Trw.Nomenclature {

    [Persistent("TrwNomenclature")]
    [DefaultProperty("TrwCode")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public class TrwSaleNomenclature : csCComponent, TrwISaleNomenclature, TrwExchangeIExportableObject, XafExtBpmnIAcceptableObject {
        public TrwSaleNomenclature(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TrwCodeSet("");
            TrwName = "";
            TrwDescription = "";
            TrwExportStateSet(TrwExchangeExportStates.CREATED);
        }

        protected override void OnSaving() {
            base.OnSaving();
            //if (TrwExportState == TrwExchangeExportStates.INIT) {
            //    TrwExportStateSet(TrwExchangeExportStates.CREATED);
            //}
        }

        protected override void OnDeleting() {
            if (!this.Session.IsNewObject(this)) {
                throw new InvalidOperationException("Delete is not allowed");
            }
        }

        private fmCOrder _Order;
        [RuleRequiredField(TargetContextIDs = "Confirm;Save")]
        public fmCOrder Order {
            get { return _Order; }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
                if (!IsLoading)
                    UpdatePropertys();
            }
        }

        private csNomenclature _Nomenclature;
        [Indexed("Order", Unique = true)]
        [RuleRequiredField(TargetContextIDs = "Confirm;Save")]
        public csNomenclature Nomenclature {
            get { return _Nomenclature; }
            set {
                SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value);
                if (!IsLoading)
                    UpdatePropertys();
            }
        }
        [Association("TrwOrder-TrwSaleNomenclature")]
        public XPCollection<TrwOrder> TrwOrders {
            get { return GetCollection<TrwOrder>("TrwOrders"); }
        }

        #region Trw

        [Persistent("TrwCode")]
        private String _TrwCode;
        [RuleRequiredField(TargetContextIDs="Confirm")]
        [PersistentAlias("_TrwCode")]
        public String TrwCode {
            get { return _TrwCode; }
//            set { SetPropertyValue<String>("TrwCode", ref _TrwCode, value); }
        }
        public void TrwCodeSet(String code) {
            if (_TrwCode != code) {
                String old = _TrwCode;
                _TrwCode = code;
                OnChanged("TrwCode", old, code);
            }
        }

        private String _TrwName;
        [RuleRequiredField(TargetContextIDs = "Confirm")]
        public String TrwName {
            get { return _TrwName; }
            set { SetPropertyValue<String>("TrwName", ref _TrwName, value); }
        }

        private TrwSaleNomenclatureMilitaryType _TrwSaleNomenclatureMilitaryType;
        [RuleValueComparison("", "Confirm", ValueComparisonType.NotEquals, TrwSaleNomenclatureMilitaryType.SALE_NOMENCLATURE_MILITARY_TYPE_UNKNOW)]
        public TrwSaleNomenclatureMilitaryType TrwSaleNomenclatureMilitaryType {
            get { return _TrwSaleNomenclatureMilitaryType; }
            set { SetPropertyValue<TrwSaleNomenclatureMilitaryType>("TrwSaleNomenclatureMilitaryType", ref _TrwSaleNomenclatureMilitaryType, value); }
        }

        private TrwSaleNomenclatureType _TrwSaleNomenclatureType;
        [RuleValueComparison("", "Confirm", ValueComparisonType.NotEquals, TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_UNKNOW)]
        public TrwSaleNomenclatureType TrwSaleNomenclatureType {
            get { return _TrwSaleNomenclatureType; }
            set { SetPropertyValue<TrwSaleNomenclatureType>("TrwSaleNomenclatureType", ref _TrwSaleNomenclatureType, value); }
        }

        private TrwSaleNomenclatureMeasurementUnit _TrwMeasurementUnit;
        [RuleValueComparison("", "Confirm", ValueComparisonType.NotEquals, TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_UNKNOW)]
        public TrwSaleNomenclatureMeasurementUnit TrwMeasurementUnit {
            get { return _TrwMeasurementUnit; }
            set { SetPropertyValue<TrwSaleNomenclatureMeasurementUnit>("TrwMeasurementUnit", ref _TrwMeasurementUnit, value); }
        }

        private String _TrwDescription;
        [Size(SizeAttribute.Unlimited)]
        public String TrwDescription {
            get { return _TrwName; }
            set { SetPropertyValue<String>("TrwDescription", ref _TrwDescription, value); }
        }
        //
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
                if (StateChangedEvent != null)
                    StateChangedEvent(this, new StateChangedEventArgs(IsAcceptable, IsRejectable));
            }
        }

        #endregion

        public void UpdatePropertys() {
            if (Order == null || Nomenclature == null) return;
            //
            TrwCodeSet(Order.Code + "." + Nomenclature.Code);
            TrwName = Nomenclature.NameShort;
            if (String.IsNullOrEmpty(TrwDescription))
                TrwDescription = TrwName;
            TrwSaleNomenclatureType = Nomenclature.TrwSaleNomenclatureType;
            TrwMeasurementUnit = Nomenclature.TrwMeasurementUnit;
            if (Order.AnalitycMilitary != null) {
                if (Order.AnalitycMilitary.ProductType == fm—OrderAnalitycMilitaryProductType.PRODUCT_MILITARY)
                    TrwSaleNomenclatureMilitaryType = Trw.TrwSaleNomenclatureMilitaryType.SALE_NOMENCLATURE_MILITARY_TYPE_MILITARY;
                else
                    TrwSaleNomenclatureMilitaryType = Trw.TrwSaleNomenclatureMilitaryType.SALE_NOMENCLATURE_MILITARY_TYPE_CIVIL;
            }
            else
                TrwSaleNomenclatureMilitaryType = TrwSaleNomenclatureMilitaryType.SALE_NOMENCLATURE_MILITARY_TYPE_UNKNOW;

        }

        [Action(PredefinedCategory.RecordEdit, TargetObjectsCriteria = "TrwExportState == 'CREATED' || TrwExportState == 'REJECTED' || TrwExportState == 'PREPARED' ")]
        public void Refresh() {
            UpdatePropertys();
        }
//        [Action(PredefinedCategory.RecordEdit, TargetObjectsCriteria = "TrwExportState == 'CREATED' || TrwExportState == 'REJECTED' || TrwExportState == 'PREPARED' ")]
        public event StateChangedEventHandler StateChangedEvent;

        public bool IsAcceptable {
            get {
                switch (TrwExportState) {
                    case TrwExchangeExportStates.CREATED:
                    case TrwExchangeExportStates.REJECTED:
                    case TrwExchangeExportStates.CHANGED:
                    case TrwExchangeExportStates.PREPARED:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsRejectable {
            get {
                switch (TrwExportState) {
                    case TrwExchangeExportStates.PREPARED:
                    case TrwExchangeExportStates.EXPORTED:
                    case TrwExchangeExportStates.CONFIRMED:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public void Accept(IObjectSpace os) {
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

        public void Reject(IObjectSpace os) {
            switch (TrwExportState) {
                case TrwExchangeExportStates.PREPARED:
                case TrwExchangeExportStates.EXPORTED:
                    TrwExportStateSet(TrwExchangeExportStates.REJECTED);
                    break;
                case TrwExchangeExportStates.CONFIRMED:
                    TrwExportStateSet(TrwExchangeExportStates.REJECTED);
                    break;
                default:
                    break;
            }
        }
    }
}
