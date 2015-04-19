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
    [Persistent("FmTaxRuVatОснование")]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "ИннПродавца;Номер;Дата")]
    public class Основание : BaseEntity {
        /// <summary>
        /// Тип документа основания для книги
        /// </summary>
        public enum ТипОснования {
            Неопределен = 0,
            /// <summary>
            /// Счет-фактура
            /// </summary>
            СЧФ = 1,
            /// <summary>
            /// Универсальный передаточный документ
            /// </summary>
            УПД = 2,
            /// <summary>
            /// Z-отчет по кассе, розничная продажа
            /// </summary>
            СФЗ = 3,
            /// <summary>
            /// Счет-фактура авансовая
            /// </summary>
            СФА = 4,
            /// <summary>
            /// Счет-фактура, внутреннее потребление
            /// </summary>
            СФВ = 5,
            /// <summary>
            /// Счет-гостинницы
            /// </summary>
            СЧГ = 6,
            /// <summary>
            /// Билет на транспорт
            /// </summary>
            БЖД = 7,
            /// <summary>
            /// Бланк строгой-отчетности
            /// </summary>
            БСО = 8,
            /// <summary>
            /// Чек покупки в розницу
            /// </summary>
            ЧЕК = 9,
            /// <summary>
            /// Государственная таможенная декларация
            /// </summary>
            ГТД = 10
        }

        public static String ТипОснования2String(ТипОснования value) {
            switch (value) { 
                case ТипОснования.БЖД:
                    return "БЖД";
                case ТипОснования.БСО:
                    return "БСО";
                case ТипОснования.СФА:
                    return "СФА";
                case ТипОснования.СФВ:
                    return "СФВ";
                case ТипОснования.СФЗ:
                    return "СФЗ";
                case ТипОснования.СЧГ:
                    return "СЧГ";
                case ТипОснования.СЧФ:
                    return "СЧФ";
                case ТипОснования.УПД:
                    return "УПД";
                case ТипОснования.ЧЕК:
                    return "ЧЕК";
                case ТипОснования.ГТД:
                    return "ГТД";
                default:
                    return "";
            }
        }

        public static ТипОснования String2ТипОснования(String value) {
            switch (value) {
                case "БЖД":
                    return ТипОснования.БЖД;
                case "БСО":
                    return ТипОснования.БСО;
                case "СФА":
                    return ТипОснования.СФА;
                case "СФВ":
                    return ТипОснования.СФВ;
                case "СФЗ":
                    return ТипОснования.СФЗ;
                case "СЧГ":
                    return ТипОснования.СЧГ;
                case "СЧФ":
                    return ТипОснования.СЧФ;
                case "УПД":
                    return ТипОснования.УПД;
                case "ЧЕК":
                    return ТипОснования.ЧЕК;
                case "ГТД":
                    return ТипОснования.ГТД;
                default:
                    return ТипОснования.Неопределен;
            }
        }

        public enum ТипИсточника {
            ВХОДЯЩИЙ = 1,
            ИСХОДЯЩИЙ = 2
        }

        public enum ТипПодчиненности {
            ОСНОВНОЙ = 1,
            КОРРЕКТИРОВОЧНЫЙ = 2
        }

        private Налогоплательщик _Налогоплательщик;
        [RuleRequiredField]
        //        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налогоплательщик Налогоплательщик {
            get { return _Налогоплательщик; }
            set {
                if (!IsLoading) OnChanging("Налогоплательщик", value);
                SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value);
            }
        }

        private СтруктурноеПодразделение _Подразделение;
        [VisibleInListView(true)]
        [DataSourceProperty("Налогоплательщик.Подразделения")]
        public СтруктурноеПодразделение Подразделение {
            get { return _Подразделение; }
            set {
                if (!IsLoading) OnChanging("Подразделение", value);
                SetPropertyValue<СтруктурноеПодразделение>("Подразделение", ref _Подразделение, value);
            }
        }

        private Основание.ТипИсточника _Источник;
        [RuleRequiredField]
        public Основание.ТипИсточника Источник {
            get { return _Источник; }
            set {
                if (!IsLoading) OnChanging("Источник", value);
                SetPropertyValue<Основание.ТипИсточника>("Источник", ref _Источник, value);
            }
        }

        private Основание.ТипПодчиненности _Корректировка;
        [RuleRequiredField]
        public Основание.ТипПодчиненности Корректировка {
            get { return _Корректировка; }
            set {
                if (!IsLoading) OnChanging("Корректировка", value);
                SetPropertyValue<Основание.ТипПодчиненности>("Корректировка", ref _Корректировка, value);
            }
        }

        private Основание.ТипОснования _Тип;
        [RuleRequiredField]
        public Основание.ТипОснования Тип {
            get { return _Тип; }
            set {
                if (!IsLoading) OnChanging("Тип", value);
                SetPropertyValue<Основание.ТипОснования>("Тип", ref _Тип, value);
            }
        }

        private String _ТипСтр;
        //        [VisibleInDetailView(false)]
        //        [VisibleInListView(false)]
        public String ТипСтр {
            get { return _ТипСтр; }
            set {
                if (!IsLoading) OnChanging("ТипСтр", value);
                SetPropertyValue<String>("ТипСтр", ref _ТипСтр, value);
            }
        }

        private String _Номер;
        [Size(1000)]
        [RuleRequiredField]
        [VisibleInListView(true)]
        public String Номер {
            get { return _Номер; }
            set {
                if (!IsLoading) OnChanging("Номер", value);
                SetPropertyValue<String>("Номер", ref _Номер, value);
            }
        }

        private DateTime _Дата;
        [Size(1000)]
        public DateTime Дата {
            get { return _Дата; }
            set {
                if (!IsLoading) OnChanging("Дата", value);
                SetPropertyValue<DateTime>("Дата", ref _Дата, value);
            }
        }

        private ЛицоТип _ЛицоТип;
        [RuleRequiredField]
        [VisibleInLookupListView(true)]
        [Index(2)]
        public ЛицоТип ЛицоТип {
            get { return _ЛицоТип; }
            set {
                if (!IsLoading) OnChanging("ЛицоТип", value);
                SetPropertyValue<ЛицоТип>("ЛицоТип", ref _ЛицоТип, value);
            }
        }
        [Persistent("ИннПродавца")]
        private String _ИннПродавца;
        [Size(12)]
        [VisibleInListView(false)]
        [PersistentAlias("_ИннПродавца")]
        [RuleRequiredField]
        public String ИннПродавца {
            get { return _ИннПродавца; }
        }
        public void ИннПродавцаУст(String value) {
            if (!IsLoading) OnChanging("ИннПродавца", value);
            SetPropertyValue<String>("ИннПродавца", ref _ИннПродавца, value);
        }


        private String _ИНН;
        [Size(12)]
        [VisibleInLookupListView(true)]
        [Index(3)]
        public String ИНН {
            get { return _ИНН; }
            set {
                if (!IsLoading) OnChanging("ИНН", value);
                SetPropertyValue<String>("ИНН", ref _ИНН, value);
            }
        }

        private String _КПП;
        [VisibleInLookupListView(true)]
        [Index(4)]
        [Size(9)]
        [RuleRequiredField(TargetCriteria = "ЛицоТип == 'ТИП_ЮЛ'")]
        public String КПП {
            get { return _КПП; }
            set {
                if (!IsLoading) OnChanging("КПП", value);
                SetPropertyValue<String>("КПП", ref _КПП, value);
            }
        }

        private Основание _БазовоеОснование;
        [Association("БазовоеОснование-Основание")]
        [RuleRequiredField(TargetCriteria = "Корректировка == 'КОРРЕКТИРОВОЧНЫЙ'")]
        public Основание БазовоеОснование {
            get { return _БазовоеОснование; }
            set {
                if (!IsLoading) OnChanging("БазовоеОснование", value);
                SetPropertyValue<Основание>("БазовоеОснование", ref _БазовоеОснование, value);
            }
        }

        private ОснованиеДокумент _ДействующийДокумент;
        [RuleRequiredField]
        [ExplicitLoading(1)]
        [DataSourceProperty("Документы")]
        public ОснованиеДокумент ДействующийДокумент {
            get { return _ДействующийДокумент; }
            set {
                if (!IsLoading) OnChanging("ДействующийДокумент", value);
                SetPropertyValue<ОснованиеДокумент>("ДействующийДокумент", ref _ДействующийДокумент, value);
            }
        }

        [Association("БазовоеОснование-Основание"), DevExpress.Xpo.Aggregated]
        public XPCollection<Основание> Корректировки {
            get { return GetCollection<Основание>("Корректировки"); }
        }

        [Association("Основание-ОснованиеДокумент"), DevExpress.Xpo.Aggregated]
        public XPCollection<ОснованиеДокумент> Документы { 
            get { return GetCollection<ОснованиеДокумент>("Документы"); } 
        }

        [Association("Основание-КнигаСтрока"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнигаСтрока> СтрокиКниги {
            get { return GetCollection<КнигаСтрока>("СтрокиКниги"); } 
        }

        [Association("Основание-Операция")]
        public XPCollection<Операция> Операции { 
            get { return GetCollection<Операция>("Операции"); } 
        }

        [VisibleInLookupListView(true)]
        [Index(5)]
        [PersistentAlias("ДействующийДокумент.НаименКонтрагента")]
        public String НаименКонтрагента {
            get { return ДействующийДокумент != null ? ДействующийДокумент.НаименКонтрагента : null; }
        }

        [PersistentAlias("ДействующийДокумент.НомерИсправления")]
        public UInt16 НомерИсправления {
            get { return ДействующийДокумент != null ? ДействующийДокумент.НомерИсправления : (UInt16) 0; }
        }

        [PersistentAlias("ДействующийДокумент.ДатаИсправления")]
        public DateTime ДатаИсправления {
            get { return ДействующийДокумент != null ? ДействующийДокумент.ДатаИсправления : default(DateTime); }
        }

        [PersistentAlias("ДействующийДокумент.ДатаВыставления")]
        public DateTime ДатаВыставления {
            get { return ДействующийДокумент != null ? ДействующийДокумент.ДатаВыставления : default(DateTime); }
        }

        [PersistentAlias("ДействующийДокумент.ДатаПолучения")]
        public DateTime ДатаПолучения {
            get { return ДействующийДокумент != null ? ДействующийДокумент.ДатаПолучения : default(DateTime); }
        }

        [PersistentAlias("ДействующийДокумент.Валюта")]
        public ВалютаОКВ Валюта {
            get { return ДействующийДокумент != null ? ДействующийДокумент.Валюта : null; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаВсего")]
        public Decimal СуммаВсего {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаВсего : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаНДС")]
        public Decimal СуммаНДС {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаНДС : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаАкциза")]
        public Decimal СуммаАкциза {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаАкциза : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаСтоимость")]
        public Decimal СуммаСтоимость {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаСтоимость : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаВсегоУвел")]
        [VisibleInListView(false)]
        public Decimal СуммаВсегоУвел {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаВсегоУвел : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаНДСУвел")]
        [VisibleInListView(false)]
        public Decimal СуммаНДСУвел {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаНДСУвел : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаАкцизаУвел")]
        [VisibleInListView(false)]
        public Decimal СуммаАкцизаУвел {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаАкцизаУвел : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаСтоимостьУвел")]
        [VisibleInListView(false)]
        public Decimal СуммаСтоимостьУвел {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаСтоимостьУвел : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаВсегоУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаВсегоУменьш {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаВсегоУменьш : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаНДСУменьш ")]
        [VisibleInListView(false)]
        public Decimal СуммаНДСУменьш {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаНДСУменьш : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаАкцизаУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаАкцизаУменьш {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаАкцизаУменьш : 0; }
        }

        [PersistentAlias("ДействующийДокумент.СуммаСтоимостьУменьш")]
        [VisibleInListView(false)]
        public Decimal СуммаСтоимостьУменьш {
            get { return ДействующийДокумент != null ? ДействующийДокумент.СуммаСтоимостьУменьш : 0; }
        }

        public Основание(Session session) : base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction();
            Корректировка = Основание.ТипПодчиненности.ОСНОВНОЙ;
            ОснованиеДокумент документ = new ОснованиеДокумент(this.Session);
            Документы.Add(документ);
            ДействующийДокумент = документ;
        }

        public override string ToString() {
            return ТипОснования2String(Тип) + ' ' + Номер + ' ' + Дата.ToString("dd.MM.yyyy");
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "БазовоеОснование":
                case "Корректировка":
                    UpdateBaseDocument();
                    break;
                case "Источник":
                case "ИНН":
                    if (Источник == ТипИсточника.ВХОДЯЩИЙ)
                        ИннПродавцаУст(ИНН);
                    else
                        ИннПродавцаУст("5012039795");
                    break;
            }
        }
        protected void UpdateBaseDocument() {
            foreach (var doc in Документы) {
                if (Корректировка == ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ && БазовоеОснование != null) {
                    doc.БазовыйДокумент = БазовоеОснование.ДействующийДокумент;
                }
                else {
                    doc.БазовыйДокумент = null;
                }
            }
        }
    }
}