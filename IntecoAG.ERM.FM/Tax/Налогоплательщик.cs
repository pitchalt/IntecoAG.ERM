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
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax {

    [NavigationItem("Налоги")]
    [Persistent("FmTaxНалогоплательщик")]
    public class Налогоплательщик : BaseEntity {

        private String _ИНН;
        [Size(12)]
        [RuleRequiredField]
        public String ИНН {
            get { return _ИНН; }
            set {
                SetPropertyValue<String>("ИНН", ref _ИНН, value);
            }
        }

        private String _КПП;
        [Size(9)]
        [RuleRequiredField]
        public String КПП {
            get { return _КПП; }
            set {
                SetPropertyValue<String>("КПП", ref _КПП, value);
            }
        }

        private String _Наименование;
        [Size(256)]
        [RuleRequiredField]
        public String Наименование {
            get { return _Наименование; }
            set {
                SetPropertyValue<String>("Наименование", ref _Наименование, value);
            }
        }

        [Association("Налогоплательщик-Период")]
        public XPCollection<Период> Периоды {
            get { return GetCollection<Период>("Периоды"); }
        }

        public Налогоплательщик(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return Наименование;
        }
    }
}