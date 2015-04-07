using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using IntecoAG.XafExt.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Tax {
    [Persistent("FmTaxДекларацияДокумент")]
    public abstract class ДекларацияДокумент : BaseEntity {

        private Декларация _Декларация;
        [Association("Декларация-ДекларацияДокумент")]
        public Декларация Декларация {
            get { return _Декларация; }
            set {
                if (!IsLoading) OnChanging("Декларация", value);
                SetPropertyValue<Декларация>("Декларация", ref _Декларация, value);
            }
        }

        private String _Код;
        [Size(16)]
        public String Код {
            get { return _Код; }
            set {
                if (!IsLoading) OnChanging("Код", value);
                SetPropertyValue<String>("Код", ref _Код, value);
            }
        }

        public ДекларацияДокумент(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}