using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.ERM.FM.Tax;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {

    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("FmTaxRuVatV504КнПокСтрБаз")]
    [Appearance(null, AppearanceItemType.ViewItem, "not ИзменитьВручную", Enabled = false,
         TargetItems = "НомСчФПрод; ДатаСчФПрод; НомИспрСчФ; ДатаИспрСчФ;СвПродТип;СвПродИНН;СвПродКПП;СвПродНаименование;НомКСчФПрод;ДатаКСчФПрод;НомИспрКСчФ;ДатаИспрКСчФ")]
    public class КнПокСтрБаз : КнигаПокупокДокументСтрока {
        private ВидОперации _КодВидОпер;
        /// <summary>
        /// Код вида операции
        /// </summary>
        //[Association("КнПокСтр-КодВидОпер")]
        [RuleRequiredField]
        public ВидОперации КодВидОпер {
            get { return _КодВидОпер; }
            set {
                if (!IsLoading) OnChanging("КодВидОпер", value);
                SetPropertyValue<ВидОперации>("КодВидОпер", ref _КодВидОпер, value);
            }
        }
        private DateTime _ДатаУчТов;
        /// <summary>
        /// Дата принятия на учет товаров (работ, услуг), имущественных прав
        /// </summary>
        //[Association("КнПокСтр-ДатаУчТов")]
        //public XPCollection<ДатаУчТов> ДатаУчТов { get { return GetCollection<ДатаУчТов>("ДатаУчТов"); } }
        //[DevExpress.Xpo.Aggregated]
        public DateTime ДатаУчТов {
            get { return _ДатаУчТов; }
            set {
                if (!IsLoading) OnChanging("ДатаУчТов", value);
                SetPropertyValue<DateTime>("ДатаУчТов", ref _ДатаУчТов, value);
            }
        }
        private СвУчСделки _СвПрод;
        /// <summary>
        /// Сведения о продавце
        /// </summary>
        [DevExpress.Xpo.Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public СвУчСделки СвПрод {
            get { return _СвПрод; }
            set {
                if (!IsLoading) OnChanging("СвПрод", value);
                SetPropertyValue<СвУчСделки>("СвПрод", ref _СвПрод, value);
            }
        }
        [PersistentAlias("СвПрод.Тип")]
        public ЛицоТип СвПродТип {
            get { return СвПрод.Тип; }
            set {
                СвПрод.Тип = value;
                OnChanged("СвПродТип");
            }
        }

        [PersistentAlias("СвПрод.ИНН")]
        public String СвПродИНН {
            get { return СвПрод.ИНН; }
            set {
                СвПрод.ИНН = value;
                OnChanged("СвПрод");
            }
        }

        [PersistentAlias("СвПрод.КПП")]
        public String СвПродКПП {
            get { return СвПрод.КПП; }
            set {
                СвПрод.КПП = value;
                OnChanged("СвПрод");
            }
        }

        [PersistentAlias("СвПрод.Наименование")]
        public String СвПродНаименование {
            get { return СвПрод.Наименование; }
            set {
                СвПрод.Наименование = value;
                OnChanged("СвПродНаименование");
            }
        }
        /// <summary>
        /// Сведения о документе, подтверждающем уплату налога
        /// </summary>
        [Association("КнПокСтрБаз-ДокПдтвУпл"), DevExpress.Xpo.Aggregated]
        public XPCollection<ДокПдтвУпл> ДокПдтвУпл { 
            get { return GetCollection<ДокПдтвУпл>("ДокПдтвУпл"); } 
        }
        private СвУчСделки _СвПос;
        /// <summary>
        /// Сведения о посреднике (комиссионере, агенте, экспедиторе, застройщике)
        /// </summary>
        [DevExpress.Xpo.Aggregated]
        public СвУчСделки СвПос {
            get { return _СвПос; }
            set {
                if (!IsLoading) OnChanging("СвПос", value);
                SetPropertyValue<СвУчСделки>("СвПос", ref _СвПос, value);
            }
        }
        private Int32 _НомерПор;
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [RuleRequiredField]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThan,0)]
        public Int32 НомерПор {
            get { return _НомерПор; }
            set {
                if (!IsLoading) OnChanging("НомерПор", value);
                SetPropertyValue<Int32>("НомерПор", ref _НомерПор, value);
            }
        }
        private String _НомСчФПрод;
        /// <summary>
        /// Номер счета-фактуры продавца
        /// </summary>
        [RuleRequiredField]
        public String НомСчФПрод {
            get { return _НомСчФПрод; }
            set {
                if (!IsLoading) OnChanging("НомСчФПрод", value);
                SetPropertyValue<String>("НомСчФПрод", ref _НомСчФПрод, value);
            }
        }
        private DateTime _ДатаСчФПрод;
        /// <summary>
        /// Дата счета-фактуры продавца
        /// </summary>
        public DateTime ДатаСчФПрод {
            get { return _ДатаСчФПрод; }
            set {
                if (!IsLoading) OnChanging("ДатаСчФПрод", value);
                SetPropertyValue<DateTime>("ДатаСчФПрод", ref _ДатаСчФПрод, value);
            }
        }
        private UInt16 _НомИспрСчФ;
        /// <summary>
        /// Номер исправления счета-фактуры продавца
        /// </summary>
        //[RuleRegularExpression("[1-9]{1}[0-9]{0,2}")]
        public UInt16 НомИспрСчФ {
            get { return _НомИспрСчФ; }
            set {
                if (!IsLoading) OnChanging("НомИспрСчФ", value);
                SetPropertyValue<UInt16>("НомИспрСчФ", ref _НомИспрСчФ, value);
            }
        }
        private DateTime _ДатаИспрСчФ;
        /// <summary>
        /// Дата исправления счета-фактуры продавца
        /// </summary>
        public DateTime ДатаИспрСчФ {
            get { return _ДатаИспрСчФ; }
            set {
                if (!IsLoading) OnChanging("ДатаИспрСчФ", value);
                SetPropertyValue<DateTime>("ДатаИспрСчФ", ref _ДатаИспрСчФ, value);
            }
        }
        private String _НомКСчФПрод;
        /// <summary>
        /// Номер корректировочного счета-фактуры продавца
        /// </summary>
        [Size(256)]
        public String НомКСчФПрод {
            get { return _НомКСчФПрод; }
            set {
                if (!IsLoading) OnChanging("НомКСчФПрод", value);
                SetPropertyValue<String>("НомКСчФПрод", ref _НомКСчФПрод, value);
            }
        }
        private DateTime _ДатаКСчФПрод;
        /// <summary>
        /// Дата корректировочного счета-фактуры продавца
        /// </summary>
        public DateTime ДатаКСчФПрод {
            get { return _ДатаКСчФПрод; }
            set {
                if (!IsLoading) OnChanging("ДатаКСчФПрод", value);
                SetPropertyValue<DateTime>("ДатаКСчФПрод", ref _ДатаКСчФПрод, value);
            }
        }
        private UInt16 _НомИспрКСчФ;
        /// <summary>
        /// Номер исправления корректировочного счета-фактуры продавца
        /// </summary>
        [RuleRange(null, DefaultContexts.Save, 0, 999)]
        public UInt16 НомИспрКСчФ {
            get { return _НомИспрКСчФ; }
            set {
                if (!IsLoading) OnChanging("НомИспрКСчФ", value);
                SetPropertyValue<UInt16>("НомИспрКСчФ", ref _НомИспрКСчФ, value);
            }
        }
        private DateTime _ДатаИспрКСчФ;
        /// <summary>
        /// Дата исправления корректировочного счета-фактуры продавца
        /// </summary>
        public DateTime ДатаИспрКСчФ {
            get { return _ДатаИспрКСчФ; }
            set {
                if (!IsLoading) OnChanging("ДатаИспрКСчФ", value);
                SetPropertyValue<DateTime>("ДатаИспрКСчФ", ref _ДатаИспрКСчФ, value);
            }
        }
        public String ПлатежиСтрока {
            get {
                String result = String.Empty;
                foreach (var плат in ДокПдтвУпл) {
                    if (result == String.Empty)
                        result = плат.НомДокПдтвУпл + " " + плат.ДатаДокПдтвУпл.ToString("dd.MM.yyyy");
                    else
                        result = result + "; " + плат.НомДокПдтвУпл + " " + плат.ДатаДокПдтвУпл.ToString("dd.MM.yyyy");
                }
                return result;
            }
        }
        private String _НомТД;
        /// <summary>
        /// Номер таможенной декларации
        /// </summary>
        [Size(1000)]
        public String НомТД {
            get { return _НомТД; }
            set {
                if (!IsLoading) OnChanging("НомТД", value);
                SetPropertyValue<String>("НомТД", ref _НомТД, value);
            }
        }
        private String _ОКВ;
        /// <summary>
        /// Код валюты по ОКВ
        /// </summary>
        [Size(3)]
        [RuleRegularExpression(null, DefaultContexts.Save,"[0-9]{3}")]
        public String ОКВ {
            get { return _ОКВ; }
            set {
                if (!IsLoading) OnChanging("ОКВ", value);
                SetPropertyValue<String>("ОКВ", ref _ОКВ, value);
            }
        }

        private Decimal _СтоимПокупВ;
        /// <summary>
        /// Стоимость покупок по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
        /// </summary>
        [RuleRequiredField]
        public Decimal СтоимПокупВ {
            get { return _СтоимПокупВ; }
            set {
                if (!IsLoading) OnChanging("СтоимПокупВ", value);
                SetPropertyValue<Decimal>("СтоимПокупВ", ref _СтоимПокупВ, value);
            }
        }

        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомСчФПрод/ДатаСчФПрод при наличии элемента ДатаСчФПрод/НомСчФПрод", UsedProperties = "ДатаСчФПрод,НомСчФПрод")]
        public Boolean НомСчФПрод_ДатаСчФПрод {
            get { return (String.IsNullOrEmpty(НомСчФПрод)) == (ДатаСчФПрод == ValidationMethods._DATE_NULL); }
        }

        /// <summary>
        /// Контроль  обязательности  присутствия  НомИспрСчФ в  зависимости  от наличия  ДатаИспрСчФ
        /// Контроль  обязательности  присутствия  ДатаИспрСчФ в  зависимости  от наличия  НомИспрСчФ
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомИспрСчФ/ДатаИспрСчФ при наличии элемента ДатаИспрСчФ/НомИспрСчФ", UsedProperties = "ДатаИспрСчФ,НомИспрСчФ")]
        public Boolean НомИспрСчФ_ДатаИспрСчФ {
            get { return (НомИспрСчФ == ValidationMethods._UINT16_NULL) == (ДатаИспрСчФ == ValidationMethods._DATE_NULL); }
        }

        /// <summary>
        /// Контроль  обязательности  присутствия  НомКСчФПрод в  зависимости  от наличия  ДатаКСчФПрод или НомИспрКСчФ или ДатаИспрКСчФ
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомКСчФПрод при наличии одного из элементов: ДатаКСчФПрод/НомКСчФПрод", UsedProperties = "ДатаКСчФПрод,НомКСчФПрод")]
        public Boolean НомКСчФПрод_ДатаКСчФПрод_НомИспрКСчФ_ДатаИспрКСчФ {
//            get { return (!String.IsNullOrEmpty(НомКСчФПрод)) == (ДатаИспрКСчФ != ValidationMethods._DATE_NULL || НомИспрКСчФ != ValidationMethods._UINT16_NULL || ДатаИспрКСчФ != ValidationMethods._DATE_NULL); }
            get { return String.IsNullOrEmpty(НомКСчФПрод) == (ДатаКСчФПрод <= ValidationMethods._DATE_NULL); }
        }

        /// <summary>
        /// Контроль  обязательности  присутствия  НомИспрКСчФ в  зависимости  от наличия  ДатаИспрКСчФ
        /// Контроль  обязательности  присутствия  ДатаИспрКСчФ в  зависимости  от наличия  НомИспрКСчФ
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомИспрКСчФ/ДатаИспрКСчФ при наличии элемента ДатаИспрКСчФ/НомИспрКСчФ", UsedProperties = "ДатаИспрКСчФ,НомИспрКСчФ")]
        public Boolean НомИспрКСчФ_ДатаИспрКСчФ {
            get { return (НомИспрКСчФ == ValidationMethods._UINT16_NULL) == (ДатаИспрКСчФ <= ValidationMethods._DATE_NULL); }
        }


//        /// <summary>
//        /// Контроль  обязательности  присутствия  ДатаКСчФПрод в  зависимости  от наличия  НомКСчФПрод или НомИспрКСчФ или ДатаИспрКСчФ
//        /// </summary>
//        [VisibleInLookupListView(false)]
//        [VisibleInDetailView(false)]
//        [VisibleInListView(false)]
//        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента ДатаКСчФПрод при наличии одного из элементов: НомКСчФПрод или НомИспрКСчФ или ДатаИспрКСчФ", UsedProperties = "ДатаКСчФПрод")]
//        public Boolean ДатаКСчФПрод_НомКСчФПрод_НомИспрКСчФ_ДатаИспрКСчФ {
////            get { return (ДатаКСчФПрод != ValidationMethods._DATE_NULL) == (!String.IsNullOrEmpty(НомКСчФПрод) || НомИспрКСчФ != ValidationMethods._UINT16_NULL || ДатаИспрКСчФ != ValidationMethods._DATE_NULL); }
//            get { return (String.IsNullOrEmpty(НомСчФПрод)) == (ДатаСчФПрод == ValidationMethods._DATE_NULL); }
//        }

        /// <summary>
        /// Контроль  обязательности  присутствия  СвПрод в зависимости  от значения КодВидОпер
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента СвПрод при наличии элемента КодВидОпер со значением из перечня: 01, 02, 03, 04, 05, 07, 08, 09, 10, 11, 12, 13", UsedProperties = "СвПрод")]
        public Boolean СвПрод_КодВидОпер {
            get { return (КодВидОпер != null && (КодВидОпер.Код == "01" || КодВидОпер.Код == "02" || КодВидОпер.Код == "03" ||
                КодВидОпер.Код == "04" || КодВидОпер.Код == "05" || КодВидОпер.Код == "07" ||
                КодВидОпер.Код == "08" || КодВидОпер.Код == "09" || КодВидОпер.Код == "10" ||
                КодВидОпер.Код == "11" || КодВидОпер.Код == "12" || КодВидОпер.Код == "13")) ? СвПрод != null : true;
            }
        }
        public КнПокСтрБаз(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            СвПрод = new СвУчСделки(this.Session);
            СвПос = new СвУчСделки(this.Session);
            _ДатаСчФПрод = ValidationMethods._DATE_NULL;
            _ДатаКСчФПрод = ValidationMethods._DATE_NULL;
            _ДатаИспрСчФ = ValidationMethods._DATE_NULL;
            _ДатаИспрКСчФ = ValidationMethods._DATE_NULL;
            _ДатаУчТов = ValidationMethods._DATE_NULL;
        }

        protected override void UpdateDocumentInfo() {
            base.UpdateDocumentInfo();
            if (СчетФактура != null) {
                НомСчФПрод = СчетФактура.Номер;
                if (СчетФактура.Дата > ValidationMethods._DATE_NULL)
                    ДатаСчФПрод = СчетФактура.Дата;
                else
                    ДатаСчФПрод = ValidationMethods._DATE_NULL;
                НомИспрСчФ = СчетФактура.НомерИсправления;
                if (НомИспрСчФ > 0 && СчетФактура.ДатаИсправления > ValidationMethods._DATE_NULL )
                    ДатаИспрСчФ = СчетФактура.ДатаИсправления;
                else
                    ДатаИспрСчФ = ValidationMethods._DATE_NULL;
                СвПродТип = Основание.ЛицоТип;
                СвПродИНН = Основание.ИНН;
                СвПродКПП = Основание.КПП;
                СвПродНаименование = Основание.НаименКонтрагента;
                if (КорректировочныйСчетФактура != null) {
                    НомКСчФПрод = КорректировочныйСчетФактура.Номер;
                    if (СчетФактура.Дата > ValidationMethods._DATE_NULL)
                        ДатаКСчФПрод = КорректировочныйСчетФактура.Дата;
                    else
                        ДатаКСчФПрод = ValidationMethods._DATE_NULL;
                    НомИспрКСчФ = КорректировочныйСчетФактура.НомерИсправления;
                    if (НомИспрКСчФ > 0 && СчетФактура.ДатаИсправления > ValidationMethods._DATE_NULL)
                        ДатаИспрКСчФ = КорректировочныйСчетФактура.ДатаИсправления;
                    else
                        ДатаИспрКСчФ = ValidationMethods._DATE_NULL;
                }
                this.СвПродНаименование = ОснованиеДокумент.НаименКонтрагента;
                this.СтоимПокупВ = ОснованиеДокумент.СуммаВсего;
            }
        }

    }
}