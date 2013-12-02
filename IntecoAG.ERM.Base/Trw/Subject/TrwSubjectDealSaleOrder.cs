using System;
using System.Linq;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.Trw.Nomenclature;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.Trw.Subject {

    [Persistent("TrwSubjectDealSaleOrder")]
    public class TrwSubjectDealSaleOrder : XPObject {

        private TrwSubjectDealSale _TrwSubjectDeal;
        [Association("TrwSubjectDealSale-TrwSubjectDealSaleOrder")]
        public TrwSubjectDealSale TrwSubjectDeal {
            get { return _TrwSubjectDeal; }
            set {
                SetPropertyValue<TrwSubjectDealSale>("TrwSubjectDeal", ref _TrwSubjectDeal, value);
            }
        }
        //
        [Persistent("TrwSaleNomenclature")]
        private TrwSaleNomenclature _TrwSaleNomenclature;
        [PersistentAlias("_TrwSaleNomenclature")]
        public TrwSaleNomenclature TrwSaleNomenclature {
            get { return _TrwSaleNomenclature; }
        }

        private fmCOrder _Order; 
        public fmCOrder Order {
            get { return _Order; }
            set { 
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
                if (!IsLoading) {
                    UpdateTrwNomenclature();
                }
            }
        }
        public void UpdateTrwNomenclature() {
            if (TrwSubjectDeal.Nomenclature == null || Order == null) return;
            TrwSaleNomenclature old = _TrwSaleNomenclature;
            if (old == null || old.Order != Order || old.Nomenclature != TrwSubjectDeal.Nomenclature) {
                IObjectSpace os = ObjectSpace.FindObjectSpaceByObject(this);
                _TrwSaleNomenclature = os.GetObjects<TrwSaleNomenclature>(
                        new OperandProperty("Order") == Order &
                        new OperandProperty("Nomenclature") == TrwSubjectDeal.Nomenclature
                    ).FirstOrDefault();
                if (_TrwSaleNomenclature == null) {
                    _TrwSaleNomenclature = os.CreateObject<TrwSaleNomenclature>();
                    _TrwSaleNomenclature.Order = Order;
                    _TrwSaleNomenclature.Nomenclature = TrwSubjectDeal.Nomenclature;
                }
                OnChanged("TrwSaleNomenclature", old, _TrwSaleNomenclature);
            }
        }

        public TrwSubjectDealSaleOrder(Session session)
            : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}
