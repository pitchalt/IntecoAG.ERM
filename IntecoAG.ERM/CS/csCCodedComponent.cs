using System;
using System.ComponentModel;

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

//using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CS {

    [NonPersistent]
    [DefaultProperty("Name")]
    [FriendlyKeyProperty("Code")]
    public abstract class csCCodedComponent : csCComponent, csICodedComponent {

        public csCCodedComponent(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private String _Code;
        private String _Name;
        private String _Description;

        [Size(7)]
        [RuleRequiredField]
        public virtual String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Size(100)]
        [RuleRequiredField]
        public virtual String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public virtual String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }

    }

}