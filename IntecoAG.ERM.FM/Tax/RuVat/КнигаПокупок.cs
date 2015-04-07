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

//    [Persistent("FmTaxRuVatКнигаПокупок")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class КнигаПокупок : Книга {

        [Association("КнигаПокупок-КнигаПокупокСтрока"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаПокупокСтрока> СтрокиПокупок {
            get { return GetCollection<КнигаПокупокСтрока>("СтрокиПокупок"); }
        }

        [ManyToManyAlias("СтрокиПокупок", "Основание2")]
        public IList<Основание> Основания {
            get { return GetList<Основание>("Основания"); }
        }

        public КнигаПокупок(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}