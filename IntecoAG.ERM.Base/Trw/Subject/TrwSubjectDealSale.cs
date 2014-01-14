using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
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

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectDealSale : TrwSubjectDealBase {

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

        //public void TrwContractOrdersUpdate() {
        //    if (DealType == TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL || TrwSubject == null || DealBudget == null)
        //        return;
        //    foreach (fmCSubject subj in TrwSubject.Subjects) {
        //        TrwOrder trw_order = DealBudget.TrwOrders.FirstOrDefault(x => x.Subject == subj);
        //        if (trw_order == null) {
        //            trw_order = new TrwOrder(this.Session);
        //            trw_order.Subject = subj;
        //            trw_order.TrwContractInt = DealBudget;
        //        }
        //    }
        //}

        public override XPCollection<TrwContract> DealBudgetSource {
            get { 
                return new XPCollection<TrwContract>(this.Session, 
                    new BinaryOperator("Subject", TrwSubject.Subject));
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

        private csNomenclature _Nomenclature;
        public csNomenclature Nomenclature {
            get { return _Nomenclature; }
            set { SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value); }
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
