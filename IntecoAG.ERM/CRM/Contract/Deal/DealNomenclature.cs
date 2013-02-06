using System;
using System.ComponentModel;

using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract.Deal {

    [Persistent("crmDealNomenclature")]
    [DefaultProperty("NomenclatureName")]
    public class crmDealNomenclature : VersionRecord {
        public crmDealNomenclature(Session session): base(session) {
        }

        public crmDealNomenclature(Session session, VersionStates state) : base(session, state) { 
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();

            this.CostCol = new crmCostCol(this.Session, this.VersionState);
        }

        private csNomenclature _Nomenclature;
        private String _NomenclatureName;
        private crmCostCol _CostCol;
        ///
        ///
        ///
        [Delayed]
        [Association("crmDealVersion-DealNomenclature")]
        public crmDealVersion DealVersion {
            get { return GetDelayedPropertyValue<crmDealVersion>("DealVersion"); }
            set { SetDelayedPropertyValue<crmDealVersion>("DealVersion", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public csNomenclature Nomenclature { 
            get { return _Nomenclature; }
            set { SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public String NomenclatureName {
            get { return _NomenclatureName; }
            set { SetPropertyValue<String>("NomenclatureName", ref _NomenclatureName, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        public crmCostCol CostCol {
            get { return _CostCol; }
            set { SetPropertyValue<crmCostCol>("CostCol", ref _CostCol, value); }
        }
        //
        CS.Measurement.csUnit _CountUnit;
        /// <summary>
        /// 
        /// </summary>
        public CS.Measurement.csUnit CountUnit {
            get { return this._CountUnit; }
            set { SetPropertyValue("CountUnit", ref _CountUnit, value); }
        }

    }

}