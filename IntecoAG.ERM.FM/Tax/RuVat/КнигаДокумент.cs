using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.XafExt.DC;
//
namespace IntecoAG.ERM.FM.Tax.RuVat {

    [Persistent("FmTaxRuVatКнигаДокумент")]
    public abstract class КнигаДокумент : BaseEntity {
        private Книга _Книга;
        [Association("Книга-КнигаДокумент")]
        public Книга Книга {
            get { return _Книга; }
            set {
                if (!IsLoading) OnChanging("Книга", value);
                SetPropertyValue<Книга>("Книга", ref _Книга, value); }
        }

        [PersistentAlias("Книга.ПериодНДС")]
        public ПериодНДС ПериодНДС {
            get {
                return Книга.ПериодНДС;
            }
        }

        [PersistentAlias("Книга.ТипКниги")]
        public Type ТипКниги {
            get {
                return Книга.ТипКниги;
            }
        }

        private UInt16 _НомерЛиста;
        public UInt16 НомерЛиста {
            get { return _НомерЛиста; }
            set {
                if (!IsLoading) OnChanging("НомерЛиста", value);
                SetPropertyValue<UInt16>("НомерЛиста", ref _НомерЛиста, value);
            }
        }

        public abstract void ToXml();

        [Association("КнигаДокумент-КнигаДокументСтрока"), DevExpress.Xpo.Aggregated]
        [VisibleInDetailView(false)]
        public XPCollection<КнигаДокументСтрока> Строки { 
            get { return GetCollection<КнигаДокументСтрока>("Строки"); } 
        }

        public КнигаДокумент(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}