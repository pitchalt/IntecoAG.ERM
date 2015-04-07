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

    [Persistent("FmTaxДекларация")]
    public abstract class Декларация : BaseEntity {

        private Период _Период;
        [Association("Период-Декларация")]
        public Период Период {
            get { return _Период; }
            set {
                if (!IsLoading) OnChanging("Период", value);
                SetPropertyValue<Период>("Период", ref _Период, value); }
        }

        private String _Код;
        [Size(16)]
//        [Custom("Caption", "Код")]
        public String Код {
            get { return _Код; }
            set {
                if (!IsLoading) OnChanging("Код", value);
                SetPropertyValue<String>("Код", ref _Код, value);
            }
        }

        //private Налогоплательщик _Налогоплательщик;
        //[Association("Налогоплательщик-Декларация")]
        //public Налогоплательщик Налогоплательщик {
        //    get { return _Налогоплательщик; }
        //    set {
        //        if (!IsLoading) OnChanging("Налогоплательщик",value);
        //        SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value); }
        //}

        //private ДокументДекларации _ДокументДекларации;
        //[Association("Декларация-ДокументДекларации"), DevExpress.Xpo.Aggregated]
        //public ДокументДекларации ДокументДекларации {
        //    get { return _ДокументДекларации; }
        //    set {
        //        if (!IsLoading) OnChanging("ДокументДекларации", value);
        //        SetPropertyValue<ДокументДекларации>("ДокументДекларации", ref _ДокументДекларации, value); }
        //}

        [Association("Декларация-ДекларацияДокумент"), DevExpress.Xpo.Aggregated]
        public XPCollection<ДекларацияДокумент> Документы { 
            get { return GetCollection<ДекларацияДокумент>("Документы"); } 
        }

        public Декларация(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}