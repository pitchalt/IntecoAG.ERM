using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.FinAccount {

    [Persistent("fmFAAccount")]
    [DefaultProperty("Code")]
    public class fmCFAAccount : csCCodedComponent, ITreeNode {
        public fmCFAAccount(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private fmCFAAccountSystem _AccountSystem;
        private fmCFAAccount _TopAccount;

        private String _NameFull;
        private String _BuhCode;
        private Boolean _IsClosed;
        private Boolean _IsSelectabled;

        public Boolean IsSelectabled {
            get { return _IsSelectabled; }
            set { SetPropertyValue<Boolean>("IsSelectabled", ref _IsSelectabled, value); }
        }

        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }

        [Size(240)]
        public String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }

        [Size(9)]
        public String BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<String>("BuhCode", ref _BuhCode, value); }
        }

        [Association("fmCFAAccountSystem-fmCFAAccount")]
        public fmCFAAccountSystem AccountSystem {
            get { return _AccountSystem; }
            set { SetPropertyValue<fmCFAAccountSystem>("AccountSystem", ref _AccountSystem, value); }
        }

        [Association("fmFATopAccount-SubAccounts")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public fmCFAAccount TopAccount {
            get { return _TopAccount; }
            set { 
                SetPropertyValue<fmCFAAccount>("TopAccount", ref _TopAccount, value);
                if (!IsLoading && value != null) {
                    AccountSystem = value.AccountSystem;
                }
            }
        }

        [Association("fmFATopAccount-SubAccounts")]
        [Aggregated]
        public XPCollection<fmCFAAccount> SubAccounts {
            get { return GetCollection<fmCFAAccount>("SubAccounts"); }
        }

        public IBindingList Children {
            get { return SubAccounts; }
        }

        public ITreeNode Parent {
            get { return TopAccount; }
        }
    }

}
