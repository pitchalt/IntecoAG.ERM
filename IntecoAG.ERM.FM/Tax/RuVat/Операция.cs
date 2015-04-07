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
            ВОСТАНОВЛЕН_0 = 10
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
        [RuleRequiredField]
        public Основание Основание {
            get { return _Основание; }
            set {
                if (!IsLoading) OnChanging("Основание", value);
                SetPropertyValue<Основание>("Основание", ref _Основание, value); }
        }

        private ОснованиеДокумент _ОснованиеДокумент;
        [Association("ОснованиеДокумент-Операция")]
        [RuleRequiredField]
        [DataSourceProperty("Основание.Документы")]
        public ОснованиеДокумент ОснованиеДокумент {
            get { return _ОснованиеДокумент; }
            set {
                if (!IsLoading) OnChanging("ОснованиеДокумент", value);
                SetPropertyValue<ОснованиеДокумент>("ОснованиеДокумент", ref _ОснованиеДокумент, value);
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
            switch (propertyName) { 
                case "Контейнер":
                    if (Контейнер != null) {
                        ПериодБУ = Контейнер.ПериодБУ;
                        ПериодНДС = Контейнер.ПериодНДС;
                    }
                    break;
                case "Основание":
                    if (Основание == null)
                        ОснованиеДокумент = null;
                    else
                        ОснованиеДокумент = Основание.ДействующийДокумент;
                    break;
                case "ДатаБУ":
                    if (Контейнер != null && Контейнер.ПериодБУ != null) {
                        ПериодБУ = Контейнер.ПериодБУ;
                    }
                    else {
                        ПериодБУ = Session.FindObject<ПериодБУ>(
                            new BinaryOperator("ДатаС", ДатаБУ, BinaryOperatorType.LessOrEqual) &
                            new BinaryOperator("ДатаПо", ДатаБУ, BinaryOperatorType.GreaterOrEqual), true);
                    }
                    break;
                case "ДатаНДС":
                    if (Контейнер != null && Контейнер.ПериодНДС != null) {
                        ПериодНДС = Контейнер.ПериодНДС;
                    }
                    else {
                        ПериодНДС = Session.FindObject<ПериодНДС>(
                            new BinaryOperator("ДатаС", ДатаНДС, BinaryOperatorType.LessOrEqual) &
                            new BinaryOperator("ДатаПо", ДатаНДС, BinaryOperatorType.GreaterOrEqual), true);
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
           }
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
