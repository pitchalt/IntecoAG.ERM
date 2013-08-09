using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.Trw.Contract {

    [Persistent("TrwOrder")]
    public class TrwOrder : csCComponent {
        public TrwOrder(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
        }

        private crmContractDeal _Deal;
        private fmCSubject _Subject;
        [Persistent("TrwCode")]
        private String _TrwCode;

        [Association("fmSubject-TrwOrders")]
        public fmCSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading && value != null) {
                    UpdateCode();
                }
            }
        }

        [Association("crmDeal-TrwOrders")]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public crmContractDeal Deal {
            get { return _Deal; }
            set {
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
                if (!IsLoading && value != null) {
                    UpdateCode();
                }
            }
        }

        [PersistentAlias("_TrwCode")]
        public String TrwCode {
            get { return _TrwCode; }
        }
        public void UpdateCode() {
            if (Subject != null && Deal != null) {
                String old = _TrwCode;
                _TrwCode = Subject.GetNextOrderNumber();
                OnChanged("TrwCode", old, _TrwCode);
            }
        }

    }
}
