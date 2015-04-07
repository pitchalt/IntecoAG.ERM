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

namespace IntecoAG.ERM.FM.Tax {

    [Persistent("FmTaxЛицо")]
    public class Лицо : BaseEntity {
        public Лицо(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}