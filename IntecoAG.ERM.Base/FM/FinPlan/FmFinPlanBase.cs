using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan {

    [NonPersistent]
    [DefaultProperty("Code")]
    public abstract class FmFinPlanBase : BaseObject {
        public FmFinPlanBase(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Size(64)]
        [Persistent]
        private String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }
        public void CodeSet(String code) {
            SetPropertyValue<String>("Code", ref _Code, code);
        }

        [Persistent("Journal")]
        [Aggregated]
        protected FmJournal _Journal;
        [PersistentAlias("_Journal")]
//        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public FmJournal Journal {
            get { return _Journal; }
        }

        private XPCollection<FmJournalOperation> _Operations;
        public XPCollection<FmJournalOperation> Operations {
            get {
                if (_Operations == null) {
                    _Operations = new XPCollection<FmJournalOperation>(PersistentCriteriaEvaluationBehavior.InTransaction, this.Session, OperationsCriteria);
                    _Operations.BindingBehavior = CollectionBindingBehavior.AllowNone;
                }
                return _Operations;
            }
        }
        protected abstract CriteriaOperator OperationsCriteria { get; }

    }

}
