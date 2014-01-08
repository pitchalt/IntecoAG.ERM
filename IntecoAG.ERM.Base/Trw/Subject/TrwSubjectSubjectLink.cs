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
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Budget;
//

namespace IntecoAG.ERM.Trw.Subject {
    public class TrwSubjectSubjectLink : XPObject {

        private TrwSubject _TrwSubject;
        [Association("TrwSubject-TrwSubjectSubjectLink")]
        public TrwSubject TrwSubject {
            get { return _TrwSubject; }
            set { SetPropertyValue<TrwSubject>("TrwSubject", ref _TrwSubject, value); }
        }

        private fmCSubject _FmSubject;
        public fmCSubject FmSubject {
            get { return _FmSubject; }
            set { 
                SetPropertyValue<fmCSubject>("FmSubject", ref _FmSubject, value);
                if (!IsLoading && TrwSubject != null && value != null) {
                    TrwSubject.UpdateDeals();               
                }
            }
        }

        public TrwSubjectSubjectLink(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
