using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM {

    /// <summary>
    /// 
    /// </summary>
    [Persistent("FmJSBase")]
    public abstract class FmJournalSet : XPObject {
        public FmJournalSet(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [Association("FmJournalSet-FmJournal"), Aggregated]
        [ReadOnly(true)]
        public XPCollection<FmJournal> Journals {
            get { return GetCollection<FmJournal>("Journals"); }
        }

        [Size(64)]
        [Persistent]
        private String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }
        public virtual void CodeSet(String code) {
            SetPropertyValue<String>("Code", ref _Code, code);
        }

        public abstract XPCollection<FmJournalOperation> Operations { get; }

    }
}
