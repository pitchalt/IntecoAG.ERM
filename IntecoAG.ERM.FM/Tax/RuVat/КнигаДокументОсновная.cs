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

//    [Persistent("FmTaxRuVatКнигаДокументОсновная")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class КнигаДокументОсновная : КнигаДокумент {

        [Association("КнигаДокументОсновная-КнигаДокументДопЛист"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаДокументДопЛист> ДопЛисты {
            get { return GetCollection<КнигаДокументДопЛист>("ДопЛисты"); }
        }

        public КнигаДокументОсновная(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override string ToString() {
            return "КнигаГлавная";
        }
    }
}