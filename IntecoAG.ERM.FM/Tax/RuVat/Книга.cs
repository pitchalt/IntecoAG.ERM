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

    [NavigationItem("Налоги")]
    [Persistent("FmTaxRuVatКнига")]
    public abstract class Книга : Регистр {

        public Type ТипКниги {
            get {
                return this.GetType();
            }
        }

        private ПериодНДС _ПериодНДС;
        [Association("ПериодНДС-Книга")]
        public ПериодНДС ПериодНДС {
            get { return _ПериодНДС; }
            set {
                if (!IsLoading) OnChanging("ПериодНДС", value);
                SetPropertyValue<ПериодНДС>("ПериодНДС", ref _ПериодНДС, value);
            }
        }
        
        [Association("Книга-КнигаДокумент"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаДокумент> КнигаДокументы {
            get { return GetCollection<КнигаДокумент>("КнигаДокументы"); } 
        }

        [Association("Книга-КнигаДокументСтрока"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаДокументСтрока> СтрокиДокументов {
            get { return GetCollection<КнигаДокументСтрока>("СтрокиДокументов"); } 
        }

        [Association("Книга-КнигаСтрока"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаСтрока> СтрокиКниги {
            get { return GetCollection<КнигаСтрока>("СтрокиКниги"); } 
        }

        public Книга(Session session) : base(session) { }
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