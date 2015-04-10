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

    [Persistent("FmTaxRuVatКнигаСтрока")]
    public abstract class КнигаСтрока : BaseEntity {

        public Type ТипСтроки {
            get { return this.GetType(); }
        }

        protected Книга _Книга;
        [Association("Книга-КнигаСтрока")]
        public Книга Книга {
            get { return _Книга; }
            set {
                if (!IsLoading) OnChanging("Книга", value);
                SetPropertyValue<Книга>("Книга", ref _Книга, value); 
            }
        }

        [Persistent]
        public Основание СчетФактура {
            get {
                if (Основание == null)
                    return null;
                if (Основание.Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ)
                    return Основание.БазовоеОснование;
                return Основание;
            }
        }

        [Persistent]
        public Основание КорректировочныйСчетФактура {
            get {
                if (Основание == null)
                    return null;
                if (Основание.Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ)
                    return Основание;
                return null;
            }
        }

        private Decimal _СуммаВсего;
        public Decimal СуммаВсего {
            get { return _СуммаВсего; }
            set {
                if (!IsLoading) OnChanging("СуммаВсего", value);
                SetPropertyValue<Decimal>("СуммаВсего", ref _СуммаВсего, value);
            }
        }

        private Decimal _СуммаНДСБаза;
        public Decimal СуммаНДСБаза {
            get { return _СуммаНДСБаза; }
            set {
                if (!IsLoading) OnChanging("СуммаНДСБаза", value);
                SetPropertyValue<Decimal>("СуммаНДСБаза", ref _СуммаНДСБаза, value);
            }
        }

        private Decimal _СуммаСтоимость;
        public Decimal СуммаСтоимость {
            get { return _СуммаСтоимость; }
            set {
                if (!IsLoading) OnChanging("СуммаСтоимость", value);
                SetPropertyValue<Decimal>("СуммаСтоимость", ref _СуммаСтоимость, value);
            }
        }

        private КнигаДокументСтрока _ТекущаяСтрокаДокумента;
        [DataSourceProperty("СтрокиДокументов")]
        public КнигаДокументСтрока ТекущаяСтрокаДокумента {
            get { return _ТекущаяСтрокаДокумента; }
            set {
                if (!IsLoading) OnChanging("ТекущаяСтрокаДокумента", value);
                SetPropertyValue<КнигаДокументСтрока>("ТекущаяСтрокаДокумента", ref _ТекущаяСтрокаДокумента, value);
            }
        }

        [Association("КнигаСтрока-КнигаДокументСтрока")]
        public XPCollection<КнигаДокументСтрока> СтрокиДокументов {
            get { return GetCollection<КнигаДокументСтрока>("СтрокиДокументов"); }
        }

        private Основание _Основание;
        [Association("Основание-КнигаСтрока")]
        [ExplicitLoading(1)]
        [RuleRequiredField]
        public Основание Основание {
            get { return _Основание; }
            set {
                if (!IsLoading) OnChanging("Основание", value);
                SetPropertyValue<Основание>("Основание", ref _Основание, value);
            }
        }

        [Association("КнигаСтрока-Операция")]
        XPCollection<Операция> Операции { 
            get { return GetCollection<Операция>("Операции"); } 
        }

        public КнигаСтрока(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
