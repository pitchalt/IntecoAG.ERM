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

    [Persistent("FmTaxНалог")]
    public class Налог : BaseEntity {

        [Association("Налог-Период"), DevExpress.Xpo.Aggregated]
        public XPCollection<Период> Периоды { 
            get { return GetCollection<Период>("Периоды"); } 
        }

        [Persistent("Код")]
        private String _Код;
        [PersistentAlias("_Код")]
        [Size(16)]
        [RuleRequiredField]
        public String Код {
            get { return _Код; }
        }
        public void КодSet(String value) {
            SetPropertyValue<String>("Код", ref _Код, value);
        }

        [Association("Налог-Ставка"), DevExpress.Xpo.Aggregated]
        public XPCollection<Ставка> Ставки {
            get { return GetCollection<Ставка>("Ставки"); }
        }


        public Налог(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return Код;
        }
    }
}