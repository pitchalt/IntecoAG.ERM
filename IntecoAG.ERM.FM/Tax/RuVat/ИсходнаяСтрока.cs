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

    [Persistent("FmTaxRuVatИсходнаяСтрока")]
    public class ИсходнаяСтрока : BaseEntity {
        private ИсходныйДокумент _ИсхДокумент;
        [Association("ИсходныйДокумент-ИсходнаяСтрока")]
        public ИсходныйДокумент ИсхДокумент {
            get { return _ИсхДокумент; }
            set {
                if (!IsLoading) OnChanging("ИсхДокумент", value);
                SetPropertyValue<ИсходныйДокумент>("ИсхДокумент", ref _ИсхДокумент, value); }
        }

        public ИсходнаяСтрока(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
