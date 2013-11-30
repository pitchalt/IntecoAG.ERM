using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
//
namespace IntecoAG.ERM.Trw.Subject {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwSubjectDealSale : TrwSubjectDealBase {

        private TrwSubject _TrwSubject;
        [Association("TrwSubject-TrwSubjectDealSale")]
        public TrwSubject TrwSubject {
            get { return _TrwSubject; }
            set { SetPropertyValue<TrwSubject>("TrwSubject", ref _TrwSubject, value); }
        }

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

        public override crmCParty Party {
            get { return Deal != null ? Deal.Customer : null; }
        }

        public TrwSubjectDealSale(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
