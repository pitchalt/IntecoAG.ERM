using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt.DC {

    [NonPersistent]
    [DefaultProperty("Name")]
    public abstract class BaseEntity : XPCustomObject {

        private Int32 _Oid = -1;
        [Key(AutoGenerate = true)]
        [Persistent("OID")]
        [Browsable(false)]
        public Int32 Oid {
            get {
                return _Oid;
            }
            set {
                Int32 oldValue = Oid;
                if (oldValue == value)
                    return;
                _Oid = value;
                OnChanged("Oid", oldValue, value);
            }
        }

        public BaseEntity(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
        public virtual void OnChanging(String propertyName, object newValue) {
        }

        //protected override void OnChanged(string propertyName, object oldValue, object newValue) {
        //    base.OnChanged(propertyName, oldValue, newValue);
        //}
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public String Name {
            get { return ToString(); }
        }
    }

    [NonPersistent]
    public abstract class CodedEntity:BaseEntity {

        public CodedEntity(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private String _Code;
        public String Code {
            get { return _Code; }
            set {
                if (!IsLoading)
                    OnChanging("Code", value);
                SetPropertyValue<String>("Code", ref _Code, value);
            }
        }

        private String _NameShort;
        public String NameShort {
            get { return _NameShort; }
            set {
                if (!IsLoading)
                    OnChanging("NameShort", value);
                SetPropertyValue<String>("NameShort", ref _NameShort, value);
            }
        }

        [Persistent("CodeFull")]
        private String _CodeFull;
        [PersistentAlias("_CodeFull")]
        public String CodeFull {
            get { return _CodeFull; }
        }

        public void CodeFullSet(String value) {
                if (!IsLoading)
                    OnChanging("CodeFull", value);
                SetPropertyValue<String>("CodeFull", ref _CodeFull, value);
        }

        public override string ToString() {
            return '(' + Code + ") " + NameShort;
        }


    }
}
