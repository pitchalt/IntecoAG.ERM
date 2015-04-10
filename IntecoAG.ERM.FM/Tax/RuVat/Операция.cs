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
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    public enum СтавкаНДС { 
        ОБЛ_18 = 2,
        ОБЛ_10 = 3,
        ОБЛ_0 = 4,
        НЕОБЛ = 5
    }

    [Persistent("FmTaxRuVatОперация")]
    public class Операция: BaseEntity {
        public enum ТипОбъектаТип {
            РЕАЛИЗАЦИЯ = 1,
            ПОТРЕБЛЕНИЕ = 2,
            СМР = 3,
            ИМПОРТ = 4
        }

        public enum ТипКнигиТип {
            ПРОДАЖ = 1,
            ПОКУПОК = 2
        }

        public enum ТипОперВнутрТип {
            РЕАЛИЗАЦИЯ = 1,
            АВАНС = 2,
            АВАНС_ЗАЧЕТ = 3,
            БЕЗВОЗМЕЗДНАЯ = 4,
            ПОТРЕБЛЕНИЕ = 5,
            СМР = 6,
            ЕВРАЗЭС = 7,
            ИМПОРТ = 8,

            СРОК_180ДНЕЙ = 9,
            ВОСТАНОВЛЕН_0 = 10,
            НЕПОДТВ_РЕАЛ_0 = 11
        }

        public enum ТипОсновнойТип {
            НАЛ_БАЗА = 1,
            ВЫЧЕТ = 2
        }

        public enum ТипНапрОперТип {
            НОРМАЛЬНЫЙ = 1,
            ВОЗВРАТ = 2
        }

        public enum ТипДеятельностиТип {
            ОБЛ_18 = 2,
            ОБЛ_10 = 3,
            ЭКСПОРТ = 4,
            НЕОБЛ = 5,
            НДС_ОТЛОЖЕН = 64,
            ПОТРЕБЛЕНИЕ = 65
        }

        private ТипКнигиТип _ТипКниги;
        public ТипКнигиТип ТипКниги {
            get { return _ТипКниги; }
            set {
                if (!IsLoading) OnChanging("ТипКниги", value);
                SetPropertyValue<ТипКнигиТип>("ТипКниги", ref _ТипКниги, value);
            }
        }

        private ТипОбъектаТип _ТипОбъекта;
        public ТипОбъектаТип ТипОбъекта {
            get { return _ТипОбъекта; }
            set {
                if (!IsLoading) OnChanging("ТипОбъекта", value);
                SetPropertyValue<ТипОбъектаТип>("ТипОбъекта", ref _ТипОбъекта, value);
            }
        }

        private ТипОперВнутрТип _ТипОперВнутр;
        public ТипОперВнутрТип ТипОперВнутр {
            get { return _ТипОперВнутр; }
            set {
                if (!IsLoading) OnChanging("ТипОперВнутр", value);
                SetPropertyValue<ТипОперВнутрТип>("ТипОперВнутр", ref _ТипОперВнутр, value);
            }
        }
        private ТипОсновнойТип _ТипОсновной;
        public ТипОсновнойТип ТипОсновной {
            get { return _ТипОсновной; }
            set {
                if (!IsLoading) OnChanging("ТипОсновной", value);
                SetPropertyValue<ТипОсновнойТип>("ТипОсновной", ref _ТипОсновной, value);
            }
        }
        private ТипНапрОперТип _ТипНапрОпер;
        public ТипНапрОперТип ТипНапрОпер {
            get { return _ТипНапрОпер; }
            set {
                if (!IsLoading) OnChanging("ТипНапрОпер", value);
                SetPropertyValue<ТипНапрОперТип>("ТипНапрОпер", ref _ТипНапрОпер, value);
            }
        }
        private ТипДеятельностиТип _ТипДеятельности;
        public ТипДеятельностиТип ТипДеятельности {
            get { return _ТипДеятельности; }
            set {
                if (!IsLoading) OnChanging("ТипДеятельности", value);
                SetPropertyValue<ТипДеятельностиТип>("ТипДеятельности", ref _ТипДеятельности, value);
            }
        }

        private ВидОперации _ОфицВидОперации;
        public ВидОперации ОфицВидОперации {
            get { return _ОфицВидОперации; }
            set {
                if (!IsLoading) OnChanging("ОфицВидОперации", value);
                SetPropertyValue<ВидОперации>("ОфицВидОперации", ref _ОфицВидОперации, value);
            }
        }


        private DateTime _ДатаБУ;
        [RuleRequiredField]
        public DateTime ДатаБУ {
            get { return _ДатаБУ; }
            set {
                if (!IsLoading) OnChanging("ДатаБУ", value);
                SetPropertyValue<DateTime>("ДатаБУ", ref _ДатаБУ, value);
            }
        }
        //[Browsable(false)]
        //public Boolean ДатаБУКоррект { 
        //}

        private DateTime _ДатаНДС;
        [RuleRequiredField]
        public DateTime ДатаНДС {
            get { return _ДатаНДС; }
            set {
                if (!IsLoading) OnChanging("ДатаНДС", value);
                SetPropertyValue<DateTime>("ДатаНДС", ref _ДатаНДС, value);
            }
        }
        //[Browsable(false)]
        //public Boolean ДатаНДСКоррект {
        //}

        private ПериодБУ _ПериодБУ;
        [Association("ПериодБУ-Операция")]
        [RuleRequiredField]
        [VisibleInDetailView(true)]
//        [VisibleInListView(true)]
        public ПериодБУ ПериодБУ {
            get { return _ПериодБУ; }
            set {
                if (!IsLoading) OnChanging("ПериодБУ", value);
                SetPropertyValue<ПериодБУ>("ПериодБУ", ref _ПериодБУ, value);
            }
        }

        private ПериодНДС _ПериодНДС;
        [Association("ПериодНДС-Операция")]
        [RuleRequiredField]
        [VisibleInDetailView(true)]
//        [VisibleInListView(true)]
        public ПериодНДС ПериодНДС {
            get { return _ПериодНДС; }
            set {
                if (!IsLoading) OnChanging("ПериодНДС", value);
                SetPropertyValue<ПериодНДС>("ПериодНДС", ref _ПериодНДС, value);
            }
        }

        private ОперацияКонтейнер _Контейнер;
        [Association("ОперацияКонтейнер-Операция")]
        public ОперацияКонтейнер Контейнер {
            get { return _Контейнер; }
            set {
                if (!IsLoading) OnChanging("Контейнер", value);
                SetPropertyValue<ОперацияКонтейнер>("Контейнер", ref _Контейнер, value);
            }
        }

        private СтавкаНДС _Ставка;
        [RuleRequiredField]
        public СтавкаНДС Ставка {
            get { return _Ставка; }
            set {
                if (!IsLoading) OnChanging("Ставка", value);
                SetPropertyValue<СтавкаНДС>("Ставка", ref _Ставка, value);
            }
        }

        private String _Проводка;
        [Size(5)]
        public String Проводка {
            get { return _Проводка; }
            set {
                if (!IsLoading) OnChanging("Проводка", value);
                SetPropertyValue<String>("Проводка", ref _Проводка, value);
            }
        }

        private String _КодПартнера;
        [Size(5)]
        public String КодПартнера {
            get { return _КодПартнера; }
            set {
                if (!IsLoading) OnChanging("КодПартнера", value);
                SetPropertyValue<String>("КодПартнера", ref _КодПартнера, value);
            }
        }

        private String _ОснованиеРегНомер;
        [Size(20)]
        public String ОснованиеРегНомер {
            get { return _ОснованиеРегНомер; }
            set {
                if (!IsLoading) OnChanging("ОснованиеРегНомер", value);
                SetPropertyValue<String>("ОснованиеРегНомер", ref _ОснованиеРегНомер, value);
            }
        }

        private String _СФТип;
        [Size(3)]
        public String СФТип {
            get { return _СФТип; }
            set {
                if (!IsLoading) OnChanging("СФТип", value);
                SetPropertyValue<String>("СФТип", ref _СФТип, value);
            }
        }

        private String _СФНаим;
        [Size(33)]
        public String СФНаим {
            get { return _СФНаим; }
            set {
                if (!IsLoading) OnChanging("СФНаим", value);
                SetPropertyValue<String>("СФНаим", ref _СФНаим, value);
            }
        }

        private String _ПДНаим;
        [Size(33)]
        public String ПДНаим {
            get { return _ПДНаим; }
            set {
                if (!IsLoading) OnChanging("ПДНаим", value);
                SetPropertyValue<String>("ПДНаим", ref _ПДНаим, value);
            }
        }


        private КнигаСтрока _КнигаСтрока;
        [Association("КнигаСтрока-Операция")]
        public КнигаСтрока КнигаСтрока {
            get { return _КнигаСтрока; }
            set {
                if (!IsLoading) OnChanging("КнигаСтрока", value);
                SetPropertyValue<КнигаСтрока>("КнигаСтрока", ref _КнигаСтрока, value);
            }
        }

        private Основание _Основание;
        [Association("Основание-Операция")]
        [RuleRequiredField(TargetCriteria="Ставка != 'НЕОБЛ' && Ставка != 'ОБЛ_0'")]
        public Основание Основание {
            get { return _Основание; }
            set {
                if (!IsLoading) OnChanging("Основание", value);
                SetPropertyValue<Основание>("Основание", ref _Основание, value); }
        }

        private ОснованиеДокумент _ОснованиеДокумент;
        [Association("ОснованиеДокумент-Операция")]
        [RuleRequiredField(TargetCriteria = "Ставка != 'НЕОБЛ' && Ставка != 'ОБЛ_0'")]
        [DataSourceProperty("Основание.Документы")]
        public ОснованиеДокумент ОснованиеДокумент {
            get { return _ОснованиеДокумент; }
            set {
                if (!IsLoading) OnChanging("ОснованиеДокумент", value);
                SetPropertyValue<ОснованиеДокумент>("ОснованиеДокумент", ref _ОснованиеДокумент, value);
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

        private Decimal _СуммаСтоимость;
        public Decimal СуммаСтоимость {
            get { return _СуммаСтоимость; }
            set {
                if (!IsLoading) OnChanging("СуммаСтоимость", value);
                SetPropertyValue<Decimal>("СуммаСтоимость", ref _СуммаСтоимость, value);
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

        public Операция(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            Decimal old_summa;
            switch (propertyName) {
                case "ОснованиеРегНомер":
                    if (!String.IsNullOrEmpty(ОснованиеРегНомер)) {
                        if (ОснованиеДокумент == null || ОснованиеРегНомер != ОснованиеДокумент.РегНомер) {
                            ОснованиеДокумент = Session.FindObject<ОснованиеДокумент>(
                                PersistentCriteriaEvaluationBehavior.InTransaction,
                                new BinaryOperator("РегНомер", ОснованиеРегНомер));
                            if (ОснованиеДокумент == null)
                                Основание = null;
                        }
                    }
                    else 
                        Основание = null;
                    break;
                case "Контейнер":
                    if (Контейнер != null) {
                        ПериодБУ = Контейнер.ПериодБУ;
                        ПериодНДС = Контейнер.ПериодНДС;
                    }
                    break;
                case "ТипКниги":
                case "ПериодНДС":
                    ОбновитьСтрокуКниги();
                    break;
                case "КнигаСтрока":
                    КнигаСтрока old_str = (КнигаСтрока)oldValue;
//                    ОбновитьСтрокуКниги();
                    if (old_str != null)
                        ОбновитьСтрокуКнигиВычесть(old_str);
                    if (КнигаСтрока != null)
                        ОбновитьСтрокуКнигиДобавить(КнигаСтрока);
                    break;
                case "Основание":
                    if (Основание != null) {
                        if (ОснованиеДокумент == null || Основание != ОснованиеДокумент.Основание) {
                            ОснованиеДокумент = Основание.ДействующийДокумент;
                        }
                        ОбновитьСтрокуКниги();
                    }
                    else
                        ОснованиеДокумент = null;
                    break;
                case "ОснованиеДокумент":
                    if (ОснованиеДокумент != null)
                        Основание = ОснованиеДокумент.Основание;
                    break;
                case "ДатаБУ":
                    if (Контейнер != null && Контейнер.ПериодБУ != null) {
                        ПериодБУ = Контейнер.ПериодБУ;
                    }
                    else {
                        if (ДатаБУ < new DateTime(2015, 1, 1))
                            break;
                        ПериодБУ = Session.FindObject<ПериодБУ>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            new BinaryOperator("ДатаС", ДатаБУ, BinaryOperatorType.LessOrEqual) &
                            new BinaryOperator("ДатаПо", ДатаБУ, BinaryOperatorType.GreaterOrEqual));
                        if (ПериодБУ == null) {
                            ПериодБУ = new ПериодБУ(Session);
                            ПериодБУ.Налогоплательщик = Контейнер.Налогоплательщик;
                            ПериодБУ.ДатаПериода = ДатаБУ;
                        }
                    }
                    break;
                case "ДатаНДС":
                    if (Контейнер != null && Контейнер.ПериодНДС != null) {
                        ПериодНДС = Контейнер.ПериодНДС;
                    }
                    else {
                        if (ДатаНДС < new DateTime(2000, 1, 1))
                            break;
                        ПериодНДС = Session.FindObject<ПериодНДС>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            new BinaryOperator("ДатаС", ДатаНДС, BinaryOperatorType.LessOrEqual) &
                            new BinaryOperator("ДатаПо", ДатаНДС, BinaryOperatorType.GreaterOrEqual));
                        if (ПериодНДС == null) {
                            ПериодНДС = new ПериодНДС(Session);
                            ПериодНДС.Налогоплательщик = Контейнер.Налогоплательщик;
                            ПериодНДС.ДатаПериода = ДатаНДС;
                        }
                    }
                    break;
                case "ТипОперВнутр":
                    UpdateTypes();
                    break;
                case "ТипОсновной":
                    UpdateTypes();
                    break;
                case "ТипНапрОпер":
                    UpdateTypes();
                    break;
                case "ТипДеятельности":
                    UpdateTypes();
                    break;
                case "СуммаВсего":
                    old_summa = (Decimal)oldValue;
                    if (КнигаСтрока != null) {
                        КнигаСтрока.СуммаВсего -= old_summa;
                        КнигаСтрока.СуммаВсего += СуммаВсего;
                    }
                    break;
                case "СуммаСтоимость":
                    old_summa = (Decimal)oldValue;
                    if (КнигаСтрока != null) {
                        КнигаСтрока.СуммаСтоимость -= old_summa;
                        КнигаСтрока.СуммаСтоимость += СуммаВсего;
                    }
                    break;
                case "СуммаНДСБаза":
                    old_summa = (Decimal)oldValue;
                    if (КнигаСтрока != null) {
                        КнигаСтрока.СуммаНДСБаза -= old_summa;
                        КнигаСтрока.СуммаНДСБаза += СуммаВсего;
                    }
                    break;
            }
        }
        protected void ОбновитьСтрокуКниги() {
            if (ПериодНДС == null || Основание == null)
                return;
            if (ТипКниги == ТипКнигиТип.ПРОДАЖ) {
                ПериодНДС.КнигаПродаж.Основания.Add(Основание);
                КнигаСтрока = РазместитьОснованиеВКниге(ПериодНДС.КнигаПродаж, Основание);
            }
            if (ТипКниги == ТипКнигиТип.ПОКУПОК) {
                ПериодНДС.КнигаПродаж.Основания.Add(Основание);
                КнигаСтрока = РазместитьОснованиеВКниге(ПериодНДС.КнигаПокупок, Основание);
            }
        }
        protected КнигаСтрока РазместитьОснованиеВКниге(Книга книга, Основание основание ) {
            foreach (КнигаСтрока str in книга.СтрокиКниги) {
                if (str.Основание == основание)
                    return str;
            }
            return null;
        }
        protected void ОбновитьСтрокуКнигиВычесть(КнигаСтрока str) {
            ОбновитьСтрокуКнигиДобавить(str, -1);
        }
        protected void ОбновитьСтрокуКнигиДобавить(КнигаСтрока str) {
            ОбновитьСтрокуКнигиДобавить(str, 1);
        }
        private void ОбновитьСтрокуКнигиДобавить(КнигаСтрока str, Int32 k) {
            str.СуммаВсего += k * СуммаВсего;
            str.СуммаСтоимость += k * СуммаСтоимость;
            str.СуммаНДСБаза += k * СуммаНДСБаза;
        }

        protected void UpdateTypes() {
            if (ТипОбъекта == ТипОбъектаТип.РЕАЛИЗАЦИЯ && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.РЕАЛИЗАЦИЯ && ТипНапрОпер == ТипНапрОперТип.НОРМАЛЬНЫЙ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "01"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.РЕАЛИЗАЦИЯ && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.АВАНС && ТипНапрОпер == ТипНапрОперТип.НОРМАЛЬНЫЙ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "02"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.РЕАЛИЗАЦИЯ && ТипОсновной == Операция.ТипОсновнойТип.ВЫЧЕТ &&
                ТипОперВнутр == ТипОперВнутрТип.РЕАЛИЗАЦИЯ  && ТипНапрОпер == ТипНапрОперТип.ВОЗВРАТ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "03"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.ПОТРЕБЛЕНИЕ && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.ПОТРЕБЛЕНИЕ && ТипНапрОпер == ТипНапрОперТип.НОРМАЛЬНЫЙ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "07"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.СМР && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.СМР && ТипНапрОпер == ТипНапрОперТип.НОРМАЛЬНЫЙ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "08"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.РЕАЛИЗАЦИЯ && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.БЕЗВОЗМЕЗДНАЯ && ТипНапрОпер == ТипНапрОперТип.НОРМАЛЬНЫЙ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "10"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
            if (ТипОбъекта == ТипОбъектаТип.РЕАЛИЗАЦИЯ && ТипОсновной == Операция.ТипОсновнойТип.НАЛ_БАЗА &&
                ТипОперВнутр == ТипОперВнутрТип.РЕАЛИЗАЦИЯ && ТипНапрОпер == ТипНапрОперТип.ВОЗВРАТ) {
                ОфицВидОперации = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "16"));
                ТипКниги = ТипКнигиТип.ПРОДАЖ;
                return;
            }
        }

    }
}
