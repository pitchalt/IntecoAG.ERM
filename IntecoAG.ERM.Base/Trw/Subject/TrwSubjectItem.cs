using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Subject {

    [NonPersistent]
    [DefaultProperty("Code")]
    [MiniNavigation("Item", "Объект", TargetWindow.Default, 0)]
    public abstract class TrwSubjectItem : BaseObject, ITreeNode {

        protected static IBindingList EmptyList = new BindingList<Object>();

        [Browsable(false)]
        public abstract ITreeNode Parent { get; }
        protected IBindingList _Children;
        public abstract IBindingList Children { get; }

        public abstract String Code { get; }
        public abstract String Name { get; }

        public virtual fmCOrder FmOrder {
            get { return null; }
        }
        public virtual fmCSubject FmSubject {
            get { return null; }
        }
        public virtual crmContractDeal CrmDeal {
            get { return null; }
        }
        [Browsable(false)]
        public abstract Object Item { get; }

        public TrwSubjectItem(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }


    }

    [NonPersistent]
    public class TrwSubjectItemSaleDeals : TrwSubjectItem {
        [Browsable(false)]
        public TrwSubject Subject;

        public override String Code {
            get { return Subject.Code + " продажи"; }
        }
        public override String Name {
            get { return Subject.Name + " договора продажи"; }
        }

        public override Object Item {
            get { return Subject; }
        }

        public override ITreeNode Parent {
            get { return null; }
        }
        public override IBindingList Children {
            get {
                if (_Children == null) {
                    _Children = new BindingList<TrwSubjectItemSaleDeal>();
                    foreach (TrwSubjectDealSale deal in Subject.DealsSale) {
                        TrwSubjectItemSaleDeal item = new TrwSubjectItemSaleDeal(this.Session);
                        item.SaleDeals = this;
                        item.SaleDeal = deal;
                        _Children.Add(item);
                    }
                }
                return _Children;
            }
        }

        public TrwSubjectItemSaleDeals(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

    [NonPersistent]
    public class TrwSubjectItemSaleDeal : TrwSubjectItem {
        [Browsable(false)]
        public TrwSubjectItemSaleDeals SaleDeals;
        [Browsable(false)]
        public TrwSubjectDealSale SaleDeal;

        public override String Code {
            get { return SaleDeal.TrwContract != null ? SaleDeal.TrwContract.TrwNumber : null; }
        }
        public override String Name {
            get { return SaleDeal.TrwContract != null ? SaleDeal.TrwContract.Name : null; }
        }

        public override Object Item {
            get {
                if (SaleDeal.Deal != null)
                    return SaleDeal.Deal.Current;
                else
                    return SaleDeal.DealBudget;
            }
        }

        public override crmContractDeal CrmDeal {
            get {
                return SaleDeal.Deal;
            }
        }

        public override ITreeNode Parent {
            get { return SaleDeals; }
        }
        public override IBindingList Children {
            get {
                if (_Children == null) {
                    _Children = new BindingList<TrwSubjectItemSaleOrder>();
                    if (SaleDeal.TrwContract != null) {
                        foreach (TrwOrder order in SaleDeal.TrwOrders) {
                            TrwSubjectItemSaleOrder item = new TrwSubjectItemSaleOrder(this.Session);
                            item.SaleDeal = this;
                            item.SaleOrder = order;
                            _Children.Add(item);
                        }
                    }
                }
                return _Children;
            }
        }

        public TrwSubjectItemSaleDeal(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

    [NonPersistent]
    public class TrwSubjectItemSaleOrder : TrwSubjectItem {
        [Browsable(false)]
        internal TrwSubjectItemSaleDeal SaleDeal;
        [Browsable(false)]
        internal TrwOrder SaleOrder;

        public override String Code {
            get { return SaleOrder.TrwCode; }
        }
        public override String Name {
            get { return null; }
        }
        public override Object Item {
            get { return SaleOrder; }
        }

        public override ITreeNode Parent {
            get { return SaleDeal; }
        }
        public override IBindingList Children {
            get {
                if (_Children == null) {
                    _Children = new BindingList<TrwSubjectItemSaleNomenclature>();
                    foreach (TrwSaleNomenclature nom in SaleOrder.TrwSaleNomenclatures) {
                        TrwSubjectItemSaleNomenclature item = new TrwSubjectItemSaleNomenclature(this.Session);
                        item.SaleOrder = this;
                        item.SaleNomenclature = nom;
                        _Children.Add(item);
                    }
                }
                return _Children;
            }
        }

        public override fmCSubject FmSubject {
            get {
                return SaleOrder.Subject;
            }
        }

        public TrwSubjectItemSaleOrder(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

    [NonPersistent]
    public class TrwSubjectItemSaleNomenclature : TrwSubjectItem {
        [Browsable(false)]
        public TrwSubjectItemSaleOrder SaleOrder;
        [Browsable(false)]
        public TrwSaleNomenclature SaleNomenclature;

        public override String Code {
            get { return SaleNomenclature.TrwCode; }
        }
        public override String Name {
            get { return SaleNomenclature.TrwName; }
        }
        public override Object Item {
            get { return SaleNomenclature; }
        }

        public override ITreeNode Parent {
            get { return SaleOrder; }
        }
        public override IBindingList Children {
            get {
                return EmptyList;
            }
        }

        public override fmCSubject FmSubject {
            get {
                return SaleNomenclature.Order.Subject;
            }
        }
        public override fmCOrder FmOrder {
            get {
                return SaleNomenclature.Order;
            }
        }

        public TrwSubjectItemSaleNomenclature(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

    [NonPersistent]
    public class TrwSubjectItemBayDeals : TrwSubjectItem {
        [Browsable(false)]
        public TrwSubject Subject;

        public override String Code {
            get { return Subject.Code + " покупки"; }
        }
        public override String Name {
            get { return Subject.Name + " договора покупки"; }
        }

        public override Object Item {
            get { return Subject; }
        }

        public override ITreeNode Parent {
            get { return null; }
        }
        public override IBindingList Children {
            get {
                if (_Children == null) {
                    _Children = new BindingList<TrwSubjectItemBayDeal>();
                    foreach (TrwSubjectDealBay deal in Subject.DealsBay) {
                        TrwSubjectItemBayDeal item = new TrwSubjectItemBayDeal(this.Session);
                        item.BayDeals = this;
                        item.BayDeal = deal;
                        _Children.Add(item);
                    }
                }
                return _Children;
            }
        }

        public TrwSubjectItemBayDeals(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

    [NonPersistent]
    public class TrwSubjectItemBayDeal : TrwSubjectItem {
        [Browsable(false)]
        public TrwSubjectItemBayDeals BayDeals;
        [Browsable(false)]
        public TrwSubjectDealBay BayDeal;

        public override String Code {
            get { return BayDeal.TrwContract != null ? BayDeal.TrwContract.TrwNumber : null; }
        }
        public override String Name {
            get { return BayDeal.TrwContract != null ? BayDeal.TrwContract.Name : null; }
        }

        public override Object Item {
            get {
                if (BayDeal.Deal != null)
                    return BayDeal.Deal.Current;
                else
                    return BayDeal.DealBudget;
            }
        }

        public override crmContractDeal CrmDeal {
            get {
                return BayDeal.Deal;
            }
        }

        public override ITreeNode Parent {
            get { return BayDeals; }
        }
        public override IBindingList Children {
            get {
                //if (_Children == null) {
                //    _Children = new BindingList<TrwSubjectItemSaleOrder>();
                //    if (BayDeal.TrwContract != null) {
                //        foreach (TrwOrder order in BayDeal.TrwOrders) {
                //            TrwSubjectItemSaleOrder item = new TrwSubjectItemSaleOrder(this.Session);
                //            item.SaleDeal = this;
                //            item.SaleOrder = order;
                //            _Children.Add(item);
                //        }
                //    }
                //}
                //return _Children;
                return EmptyList;
            }
        }

        public TrwSubjectItemBayDeal(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
