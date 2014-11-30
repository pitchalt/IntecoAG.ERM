using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM {

    [NavigationItem("FinPlan")]
    [Persistent("FmAccounting")]
    [DefaultProperty("Code")]
    public abstract class FmAccounting : XPObject {
        public FmAccounting(Session session): base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Size(64)]
        [Persistent]
        protected String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }
        public void CodeSet(String code) {
            SetPropertyValue<String>("Code", ref _Code, code);
            _Journal.CodeSet(code);
        }


        [Persistent("Journal")]
        [Aggregated]
        protected FmJournal _Journal;
        [PersistentAlias("_Journal")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public FmJournal Journal {
            get { return _Journal; }
        }

        private crmCPerson _Person;
//        [ExplicitLoading(1)]
        public crmCPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmCPerson>("Person", ref _Person, value); }
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
