using System;
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
namespace IntecoAG.ERM.Trw.Budget {

    [Persistent("TrwBudget")]
    [DefaultProperty("Name")]
    public abstract class TrwBudgetBase : BaseObject {

        [Persistent("Code")]
        protected String _Code;
        [PersistentAlias("_Code")]
        [VisibleInLookupListView(true)]
        public String Code {
            get { return _Code; }
        }

        [Persistent("Name")]
        protected String _Name;
        [PersistentAlias("_Name")]
        public String Name {
            get { return _Name; }
//            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        protected abstract void NameUpdate();

//        [Persistent("TrwPeriod")]

        public abstract void Calculate(IObjectSpace os);

        public TrwBudgetBase(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            NameUpdate();
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

}
