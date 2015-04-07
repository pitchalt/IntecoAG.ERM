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

    [Persistent("FmTaxРегистр")]
    public abstract class Регистр : BaseEntity {

        private Период _Период;
        [Association("Период-Регистр")]
        public Период Период {
            get { return _Период; }
            set {
                if (!IsLoading) OnChanging("Период", value);
                SetPropertyValue<Период>("Период", ref _Период, value); }
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

        public Регистр(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}