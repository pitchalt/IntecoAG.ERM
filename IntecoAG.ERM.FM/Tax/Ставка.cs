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
    
//    [NavigationItem("Настройки")]
    [Persistent("FmTaxСтавка")]
    public class Ставка : BaseEntity {

        private Налог _Налог;
        [Association("Налог-Ставка")]
        [RuleRequiredField]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налог Налог {
            get { return _Налог; }
            set {
                if (!IsLoading) OnChanging("Налог", value);
                SetPropertyValue<Налог>("Налог", ref _Налог, value);
            }
        }

        private String _Код;
        [Size(16)]
        [RuleRequiredField]
        public String Код {
            get { return _Код; }
            set {
                SetPropertyValue<String>("Код", ref _Код, value);
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

        public Decimal Числитель;
        public Decimal Знаменатель;

        public Ставка(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return Наименование;
        }
    }
}