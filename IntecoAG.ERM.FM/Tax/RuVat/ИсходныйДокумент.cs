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

namespace IntecoAG.ERM.FM.Tax.RuVat {

    [Persistent("FmTaxRuVatИсходныйДокумент")]
    public class ИсходныйДокумент : BaseEntity {
        [Association("ИсходныйДокумент-ИсходнаяСтрока"),DevExpress.Xpo.Aggregated]
        XPCollection<ИсходнаяСтрока> ИсходныеСтроки { get { return GetCollection<ИсходнаяСтрока>("ИсходныеСтроки"); } }

        [Association("ИсходныйДокумент-Операция"), DevExpress.Xpo.Aggregated]
        XPCollection<Операция> Операции { get { return GetCollection<Операция>("Операции"); } }

        public ИсходныйДокумент(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}
