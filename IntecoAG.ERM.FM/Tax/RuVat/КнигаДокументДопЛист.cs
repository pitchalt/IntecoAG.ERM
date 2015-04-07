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

    [MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("FmTaxRuVatКнигаДокументДопЛист")]
    public abstract class КнигаДокументДопЛист : КнигаДокумент {

        private КнигаДокументОсновная _КнигаДокументОсновная;
        [Association("КнигаДокументОсновная-КнигаДокументДопЛист"), DevExpress.Xpo.Aggregated]
        public КнигаДокументОсновная КнигаДокументОсновная {
            get { return _КнигаДокументОсновная; }
            set { SetPropertyValue<КнигаДокументОсновная>("КнигаДокументОсновная", ref _КнигаДокументОсновная, value); }
        }

        public КнигаДокументДопЛист(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return "КнигаДопЛист" + НомерЛиста.ToString("000");
        }
    }
}