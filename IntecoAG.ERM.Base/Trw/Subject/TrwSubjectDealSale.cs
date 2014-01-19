using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.Trw.Subject {

//    [Appearance(null, AppearanceItemType.ViewItem, "DealType != 'TRW_SUBJECT_DEAL_REAL'", TargetItems = "IsNomPlan", Enabled = false)]
    [Appearance(null, AppearanceItemType.ViewItem, "!IsNomPlan", TargetItems = "DealSaleOrders,TrwOrderWorkType,Nomenclature", Enabled = false)]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectDealSale : TrwSubjectDealBase {

        public override void UpdateConsolidateDeal(Boolean force) {
            base.UpdateConsolidateDeal(force);
            IsNomPlan = true;
        }

        private TrwSubject _TrwSubject;
        [Association("TrwSubject-TrwSubjectDealSale")]
        public TrwSubject TrwSubject {
            get { return _TrwSubject; }
            set { 
                SetPropertyValue<TrwSubject>("TrwSubject", ref _TrwSubject, value);
            }
        }

        public override TrwSubject TrwSubjectBase {
            get { return TrwSubject; }
        }

        public override XPCollection<TrwContract> DealBudgetSource {
            get { 
                return new XPCollection<TrwContract>(this.Session, 
                    new BinaryOperator("Subject", TrwSubject.Subject));
            }
        }
        public override void DealUpdated() {
            using (IObjectSpace os = ObjectSpace.FindObjectSpaceByObject(this).CreateNestedObjectSpace()) {
                TrwSubjectDealSale subj_deal = os.GetObject<TrwSubjectDealSale>(this);
                subj_deal.CrmContractDeals.Clear();
                subj_deal.CrmContractDeals.Add(subj_deal.Deal);
                os.Delete(subj_deal.DealSaleOrders);
                TrwSubjectDealLogic.RefreshDeal(os, subj_deal);
                os.CommitChanges();
            }
        }

        public override XPCollection<crmContractDeal> DealSource {
            get {
                return new XPCollection<crmContractDeal>(TrwSubject.Subject.Deals,
                        new BinaryOperator("TRVType.TrwContractSuperType", Contract.TrwContractSuperType.DEAL_SALE, BinaryOperatorType.Equal));
            }
        }

        public override crmCPerson Person {
            get { return Deal != null ? (Deal.Customer != null ?  Deal.Customer.Person : null)  : PersonInternal; }
        }

        private Boolean _IsNomPlan; 
        [ImmediatePostData]
        public Boolean IsNomPlan {
            get { return _IsNomPlan; }
            set { SetPropertyValue<Boolean>("IsNomPlan", ref _IsNomPlan, value); } 
        }

        private Decimal _Count;
        public Decimal Count {
            get { return _Count; }
            set {SetPropertyValue<Decimal>("Count", ref _Count, value); }
        }

        private csNomenclature _Nomenclature;
        [RuleRequiredField(TargetCriteria="IsNomPlan")]
        public csNomenclature Nomenclature {
            get { return _Nomenclature; }
            set { 
                SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value);
                if (!IsLoading && value != null) {
                    UpdateDeal();
                }
            }
        }

        void UpdateDeal() { 
            using (IObjectSpace os = ObjectSpace.FindObjectSpaceByObject(this).CreateNestedObjectSpace()) {
                TrwSubjectDealSale subj_deal = os.GetObject<TrwSubjectDealSale>(this);
                TrwSubjectDealLogic.RefreshDeal(os, subj_deal);
                os.CommitChanges();
            }
        }

        private TrwOrderWorkType _TrwOrderWorkType;
        public TrwOrderWorkType TrwOrderWorkType {
            get { return _TrwOrderWorkType; }
            set { SetPropertyValue<TrwOrderWorkType>("TrwOrderWorkType", ref _TrwOrderWorkType, value); }
        }

        [Association("TrwSubjectDealSale-TrwSubjectDealSaleOrder"), Aggregated]
        public XPCollection<TrwSubjectDealSaleOrder> DealSaleOrders {
            get { return GetCollection<TrwSubjectDealSaleOrder>("DealSaleOrders"); }
        }

        public TrwSubjectDealSale(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        //public override IBindingList Children {
        //    get { return TrwContract != null ? new BindingList<TrwIOrder>(TrwContract.TrwSaleOrders) : null; }
        //}
    }
}
