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

    //[Persistent("FmTaxRuVatКнигаПродаж")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class КнигаПродаж : Книга {

        [Association("КнигаПродаж-КнигаПродажСтрока"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаПродажСтрока> СтрокиПродаж {
            get { return GetCollection<КнигаПродажСтрока>("СтрокиПродаж"); }
        }

        [ManyToManyAlias("СтрокиПродаж", "Основание2")]
        public IList<Основание> Основания {
            get { return GetList<Основание>("Основания"); }
        }

        public КнигаПродаж(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}