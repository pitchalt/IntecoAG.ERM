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

    [NavigationItem("Налоги")]
    [Persistent("FmTaxПериод")]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "Код")]
    public abstract class Период : BaseEntity {

        private Налог _Налог;
        [Association("Налог-Период")]
        [RuleRequiredField]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налог Налог {
            get { return _Налог; }
            set {
                if (!IsLoading) OnChanging("Налог", value);
                SetPropertyValue<Налог>("Налог", ref _Налог, value); }
        }

        private Налогоплательщик _Налогоплательщик;
        [Association("Налогоплательщик-Период")]
        [RuleRequiredField]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налогоплательщик Налогоплательщик {
            get { return _Налогоплательщик; }
            set {
                if (!IsLoading) OnChanging("Налогоплательщик", value);
                SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value);
            }
        }

        [Association("Период-Декларация"), DevExpress.Xpo.Aggregated]
        [VisibleInDetailView(false)]
        public XPCollection<Декларация> Декларации { 
            get { return GetCollection<Декларация>("Декларации"); } 
        }

        [Association("Период-Регистр"), DevExpress.Xpo.Aggregated]
        [VisibleInDetailView(false)]
        public XPCollection<Регистр> Регистры { 
            get { return GetCollection<Регистр>("Регистры"); } 
        }

        [Persistent("Код")]
        private String _Код;
        [PersistentAlias("_Код")]
        [Size(16)]
        public String Код {
            get { return _Код; }
        }
        public void КодУст(String value) {
           SetPropertyValue<String>("Код", ref _Код, value);
        }

        [Persistent("ДатаС")]
        private DateTime _ДатаС;
        [PersistentAlias("_ДатаС")]
        public DateTime ДатаС {
            get { return _ДатаС; }
        }
        public void ДатаСSet(DateTime value) {
            SetPropertyValue<DateTime>("ДатаС", ref _ДатаС, value);
        }

        [Persistent("ДатаПо")]
        private DateTime _ДатаПо;
        [PersistentAlias("_ДатаПо")]
        public DateTime ДатаПо {
            get { return _ДатаПо; }
        }
        public void ДатаПоSet(DateTime value) {
            SetPropertyValue<DateTime>("ДатаПо", ref _ДатаПо, value);
        }

        public Период(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}