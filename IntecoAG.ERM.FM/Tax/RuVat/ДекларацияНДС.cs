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

namespace IntecoAG.ERM.FM.Tax.RuVat {

    [Persistent("FmTaxRuVatДекларацияНДС")]
    public class ДекларацияНДС : Декларация {
        //private String _Код;
        //[Size(16)]
        ////        [Custom("Caption", "Код")]
        //public String Код {
        //    get { return _Код; }
        //    set {
        //        if (!IsLoading) OnChanging("Код", value);
        //        SetPropertyValue<String>("Код", ref _Код, value);
        //    }
        //}
        private ПериодНДС _ПериодНДС;
        [Association("ПериодНДС-ДекларацияНДС")]
        public ПериодНДС ПериодНДС {
            get { return _ПериодНДС; }
            set {
                if (!IsLoading) OnChanging("ПериодНДС", value);
                SetPropertyValue<ПериодНДС>("ПериодНДС", ref _ПериодНДС, value);
            }
        }

        [Association("ДекларацияНДС-ДекларацияНДСДокумент"), DevExpress.Xpo.Aggregated]
        public XPCollection<ДекларацияНДСДокумент> ДекларацияНДСДокументы {
            get { return GetCollection<ДекларацияНДСДокумент>("ДекларацияНДСДокументы"); }
        }


        public ДекларацияНДС(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "ПериодНДС":
                    Период = (Период)newValue;
                    break;
            }
        }
    }
}