using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    [NavigationItem("Налоги")]
//    [NavigationItem("Настройки")]
    [Persistent("FmTaxRuVatВалютаОКВ")]
    public class ВалютаОКВ : BaseEntity {

        private String _Код;
        [Size(3)]
        public String Код {
            get { return _Код; }
            set {
                if (!IsLoading) OnChanging("Код", value);
                SetPropertyValue<String>("Код", ref _Код, value);
            }
        }

        private String _Наименование;
        [Size(32)]
        public String Наименование {
            get { return _Наименование; }
            set {
                if (!IsLoading) OnChanging("Наименование", value);
                SetPropertyValue<String>("Наименование", ref _Наименование, value);
            }
        }

        public ВалютаОКВ(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return '(' + Код + ") " + Наименование;
        }
    }
}
