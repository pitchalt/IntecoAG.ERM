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

    [NavigationItem("Налоги")]
    [Persistent("FmTaxRuVatОснованиеДокумент")]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "РегНомер")]
    public class ОснованиеДокумент : BaseEntity {

        [PersistentAlias("Основание.Источник")]
        public Основание.ТипИсточника Источник {
            get { return Основание.Источник; }
        }

        [PersistentAlias("Основание.Корректировка")]
        public Основание.ТипПодчиненности Корректировка {
            get { return Основание.Корректировка; }
        }

        [PersistentAlias("Основание.Тип")]
        public Основание.ТипОснования Тип {
            get { return Основание.Тип; }
        }

        [PersistentAlias("Основание.Номер")]
        public String Номер {
            get { return Основание.Номер; }
        }

        [PersistentAlias("Основание.Дата")]
        public DateTime Дата {
            get { return Основание.Дата; }
        }

        [PersistentAlias("Основание.ЛицоТип")]
        public ЛицоТип ЛицоТип {
            get { return Основание.ЛицоТип; }
        }

        [PersistentAlias("Основание.ИНН")]
        public String ИНН {
            get { return Основание.ИНН; }
        }

        [PersistentAlias("Основание.КПП")]
        public String КПП {
            get { return Основание.КПП; }
        }

        [Association("ОснованиеДокумент-Операция")]
        public XPCollection<Операция> Операции {
            get { return GetCollection<Операция>("Операции"); }
        }

        private String _РегНомер;
        [Size(20)]
        [RuleRequiredField]
        public String РегНомер {
            get { return _РегНомер; }
            set {
                if (!IsLoading) OnChanging("РегНомер", value);
                SetPropertyValue<String>("РегНомер", ref _РегНомер, value);
            }
        }

        private String _КодПартнера;
        [Size(20)]
        public String КодПартнера {
            get { return _КодПартнера; }
            set {
                if (!IsLoading) OnChanging("КодПартнера", value);
                SetPropertyValue<String>("КодПартнера", ref _КодПартнера, value);
            }
        }

        private String _НаименКонтрагента;
        [Size(256)]
        public String НаименКонтрагента {
            get { return _НаименКонтрагента; }
            set {
                if (!IsLoading) OnChanging("НаименКонтрагента", value);
                SetPropertyValue<String>("НаименКонтрагента", ref _НаименКонтрагента, value);
            }
        }

        private UInt16 _НомерИсправления;
        public UInt16 НомерИсправления {
            get { return _НомерИсправления; }
            set {
                if (!IsLoading) OnChanging("НомерИсправления", value);
                SetPropertyValue<UInt16>("НомерИсправления", ref _НомерИсправления, value);
            }
        }

        private DateTime _ДатаИсправления;
        public DateTime ДатаИсправления {
            get { return _ДатаИсправления; }
            set {
                if (!IsLoading) OnChanging("ДатаИсправления", value);
                SetPropertyValue<DateTime>("ДатаИсправления", ref _ДатаИсправления, value);
            }
        }

        private DateTime _ДатаВыставления;
        public DateTime ДатаВыставления {
            get { return _ДатаВыставления; }
            set {
                if (!IsLoading) OnChanging("ДатаВыставления", value);
                SetPropertyValue<DateTime>("ДатаВыставления", ref _ДатаВыставления, value);
            }
        }

        private DateTime _ДатаПолучения;
        public DateTime ДатаПолучения {
            get { return _ДатаПолучения; }
            set {
                if (!IsLoading) OnChanging("ДатаПолучения", value);
                SetPropertyValue<DateTime>("ДатаПолучения", ref _ДатаПолучения, value);
            }
        }

        private ВалютаОКВ _Валюта;
        public ВалютаОКВ Валюта {
            get { return _Валюта; }
            set {
                if (!IsLoading) OnChanging("Валюта", value);
                SetPropertyValue<ВалютаОКВ>("Валюта", ref _Валюта, value);
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

        private Decimal _СуммаНДС;
        public Decimal СуммаНДС {
            get { return _СуммаНДС; }
            set {
                if (!IsLoading) OnChanging("СуммаНДС", value);
                SetPropertyValue<Decimal>("СуммаНДС", ref _СуммаНДС, value);
            }
        }

        private Decimal _СуммаАкциза;
        public Decimal СуммаАкциза {
            get { return _СуммаАкциза; }
            set {
                if (!IsLoading) OnChanging("СуммаАкциза", value);
                SetPropertyValue<Decimal>("СуммаАкциза", ref _СуммаАкциза, value);
            }
        }

        [Persistent("СуммаСтоимость")]
        private Decimal _СуммаСтоимость;
        [PersistentAlias("_СуммаСтоимость")]
        public Decimal СуммаСтоимость {
            get { return _СуммаСтоимость; }
        }
        public void СуммаСтоимостьSet(Decimal value) {
            SetPropertyValue<Decimal>("СуммаСтоимость", ref _СуммаСтоимость, value);
        }

        private Decimal _СуммаВсегоУвел;
        [VisibleInListView(false)]
        public Decimal СуммаВсегоУвел {
            get { return _СуммаВсегоУвел; }
            set {
                if (!IsLoading) OnChanging("СуммаВсегоУвел", value);
                SetPropertyValue<Decimal>("СуммаВсегоУвел", ref _СуммаВсегоУвел, value);
            }
        }

        private Decimal _СуммаНДСУвел;
        [VisibleInListView(false)]
        public Decimal СуммаНДСУвел {
            get { return _СуммаНДСУвел; }
            set {
                if (!IsLoading) OnChanging("СуммаНДСУвел", value);
                SetPropertyValue<Decimal>("СуммаНДСУвел", ref _СуммаНДСУвел, value);
            }
        }

        private Decimal _СуммаАкцизаУвел;
        [VisibleInListView(false)]
        public Decimal СуммаАкцизаУвел {
            get { return _СуммаАкцизаУвел; }
            set {
                if (!IsLoading) OnChanging("СуммаАкцизаУвел", value);
                SetPropertyValue<Decimal>("СуммаАкцизаУвел", ref _СуммаАкцизаУвел, value);
            }
        }

        [Persistent("СуммаСтоимостьУвел")]
        private Decimal _СуммаСтоимостьУвел;
        [PersistentAlias("_СуммаСтоимостьУвел")]
        [VisibleInListView(false)]
        public Decimal СуммаСтоимостьУвел {
            get { return _СуммаСтоимостьУвел; }
        }
        public void СуммаСтоимостьУвелSet(Decimal value) {
            SetPropertyValue<Decimal>("СуммаСтоимостьУвел", ref _СуммаСтоимостьУвел, value);
        }

        [Persistent("СуммаВсегоУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаВсегоУменьш {
            get { return Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ? СуммаВсегоУвел - СуммаВсего : 0; }
        }

        [Persistent("СуммаНДСУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаНДСУменьш {
            get { return Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ ? СуммаНДСУвел - СуммаНДС : 0; }
        }

        [Persistent("СуммаАкцизаУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаАкцизаУменьш {
            get { return Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ ? СуммаАкцизаУвел - СуммаАкциза : 0; }
        }

        [Persistent("СуммаСтоимостьУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаСтоимостьУменьш {
            get { return Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ ? СуммаСтоимостьУвел - СуммаСтоимость : 0; }
        }

        private Основание _Основание;
        [Association("Основание-ОснованиеДокумент")]
        [ExplicitLoading(1)]
        public Основание Основание {
            get { return _Основание; }
            set {
                if (!IsLoading) OnChanging("Основание", value);
                SetPropertyValue<Основание>("Основание", ref _Основание, value);
            }
        }

        private ОснованиеДокумент _БазовыйДокумент;
        [ExplicitLoading(1)]
        [RuleRequiredField(TargetCriteria="Корректировка=='КОРРЕКТИРОВОЧНЫЙ'")]
        [DataSourceProperty("Основание.БазовоеОснование.Документы")]
        public ОснованиеДокумент БазовыйДокумент {
            get { return _БазовыйДокумент; }
            set {
                if (!IsLoading) OnChanging("БазовыйДокумент", value);
                SetPropertyValue<ОснованиеДокумент>("БазовыйДокумент", ref _БазовыйДокумент, value);
            }
        }

        public ОснованиеДокумент(Session session) : base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction(); 
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "НомерИсправления":
                    if (НомерИсправления > Основание.ДействующийДокумент.НомерИсправления)
                        Основание.ДействующийДокумент = this;
                    break;
                case "Основание":
                        if (Корректировка == Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ)
                            БазовыйДокумент = Основание.БазовоеОснование.ДействующийДокумент;
                    break;
                case "СуммаВсего":
                    ОбновитьСтоимость();
                    break;
                case "СуммаАкциза":
                    ОбновитьСтоимость();
                    break;
                case "СуммаНДС":
                    ОбновитьСтоимость();
                    break;
                case "СуммаВсегоУвел":
                    ОбновитьСтоимость();
                    break;
                case "СуммаАкцизаУвел":
                    ОбновитьСтоимость();
                    break;
                case "СуммаНДСУвел":
                    ОбновитьСтоимость();
                    break;
            }
        }

        public override string ToString() {
            String result = String.Empty;
            if (Основание != null)
                result += Основание.ToString();
            if (НомерИсправления != 0)
                result += " ИСПР " + НомерИсправления.ToString() + ' ' + ДатаИсправления.ToString("dd.MM.yyyy");
            return result;
        }

        public void ОбновитьСтоимость() {
            СуммаСтоимостьSet(СуммаВсего - СуммаАкциза - СуммаНДС);
            СуммаСтоимостьУвелSet(СуммаВсегоУвел - СуммаАкцизаУвел - СуммаНДСУвел);
        }
    }
}