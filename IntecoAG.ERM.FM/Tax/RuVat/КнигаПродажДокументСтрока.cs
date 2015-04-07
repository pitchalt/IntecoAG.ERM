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

    //[Persistent("FmTaxRuVatКнигаПродажДокументСтрока")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class КнигаПродажДокументСтрока : КнигаДокументСтрока {

        private КнигаПродажСтрока _КнигаПродажСтрока;
        [Association("КнигаПродажСтрока-КнигаПродажДокументСтрока")]
        public КнигаПродажСтрока КнигаПродажСтрока {
            get { return _КнигаПродажСтрока; }
            set { SetPropertyValue<КнигаПродажСтрока>("КнигаПродажСтрока", ref _КнигаПродажСтрока, value); }
        }

        public КнигаПродажДокументСтрока(Session session)
            : base(session) {
        }

        //private ГенДокументПокупокПродаж _ГенДокумент;
        //[Association("ГенДокументПокупокПродаж-ГенСтрока")]
        //public ГенДокументПокупокПродаж ГенДокумент {
        //    get { return _ГенДокумент; }
        //    set {
        //        if (!IsLoading) OnChanging("ГенДокумент", value);
        //        SetPropertyValue<ГенДокументПокупокПродаж>("ГенДокумент", ref _ГенДокумент, value); }
        //}

        public override void AfterConstruction() {
            base.AfterConstruction();
            
        }

    }
}
