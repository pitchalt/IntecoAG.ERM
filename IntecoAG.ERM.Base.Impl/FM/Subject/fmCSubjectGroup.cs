using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM.Subject {

    [Persistent("fmSubjectGroup")]
    [DefaultProperty("Code")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public class fmCSubjectGroup : csCComponent {
        public fmCSubjectGroup(Session session)
            : base(session) {
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        private String _Code;
        private String _Name;

        [Size(14)]
        [RuleRequiredField]
        public virtual String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Size(60)]
        [RuleRequiredField]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public virtual String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        [Association("fmCSubject-fmCSubjectGroup")]
        public XPCollection<fmCSubject> Subjects {
            get {
                return GetCollection<fmCSubject>("Subjects");
            }
        }
    }
}
