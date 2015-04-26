using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {

    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("FmTaxRuVatV504КнПродСтрБаз")]
    [Appearance(null, AppearanceItemType.ViewItem, "not ИзменитьВручную", Enabled=false,  
         TargetItems="НомСчФПрод; ДатаСчФПрод; НомИспрСчФ; ДатаИспрСчФ;СвПокупТип;СвПокупИНН;СвПокупКПП;СвПокупНаименование;НомКСчФПрод;ДатаКСчФПрод;НомИспрКСчФ;ДатаИспрКСчФ")]
    public class КнПродСтрБаз : КнигаПродажДокументСтрока {

        private СвУчСделки _СвПокуп;
        /// <summary>
        /// Сведения о продавце
        /// </summary>
        [DevExpress.Xpo.Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public СвУчСделки СвПокуп {
            get { return _СвПокуп; }
            set {
                if (!IsLoading) OnChanging("СвПокуп", value);
                SetPropertyValue<СвУчСделки>("СвПокуп", ref _СвПокуп, value);
            }
        }

        [PersistentAlias("СвПокуп.Тип")]
        public ЛицоТип СвПокупТип {
            get { return СвПокуп.Тип; }
            set { 
                СвПокуп.Тип = value;
                OnChanged("СвПокупТип");
            }
        }

        [PersistentAlias("СвПокуп.ИНН")]
        public String СвПокупИНН {
            get { return СвПокуп.ИНН; }
            set { 
                СвПокуп.ИНН = value;
                OnChanged("СвПокупИНН");
            }
        }

        [PersistentAlias("СвПокуп.КПП")]
        public String СвПокупКПП {
            get { return СвПокуп.КПП; }
            set { 
                СвПокуп.КПП = value;
                OnChanged("СвПокупКПП");
            }
        }

        [PersistentAlias("СвПокуп.Наименование")]
        public String СвПокупНаименование {
            get { return СвПокуп.Наименование; }
            set { 
                СвПокуп.Наименование = value;
                OnChanged("СвПокупНаименование");
            }
        } 

        /// <summary>
        /// Сведения о документе, подтверждающем уплату налога
        /// </summary>
        [Association("КнПродСтр-ДокПдтвОпл"), DevExpress.Xpo.Aggregated]
        public XPCollection<ДокПдтвОпл> ДокПдтвОпл { get { return GetCollection<ДокПдтвОпл>("ДокПдтвОпл"); } }

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

        private Decimal _СтоимПродОсв;
        /// <summary>
        /// Стоимость продаж, освобождаемых от налога, по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках
        /// </summary>
        public Decimal СтоимПродОсв {
            get { return _СтоимПродОсв; }
            set {
                if (!IsLoading) OnChanging("СтоимПродОсв", value);
                SetPropertyValue<Decimal>("СтоимПродОсв", ref _СтоимПродОсв, value);
            }
        }

        private Decimal _СумНДССФ10;
        /// <summary>
        /// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках, по ставке 10
        /// </summary>
        public Decimal СумНДССФ10 {
            get { return _СумНДССФ10; }
            set {
                if (!IsLoading) OnChanging("СумНДССФ10", value);
                SetPropertyValue<Decimal>("СумНДССФ10", ref _СумНДССФ10, value);
            }
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
        /// <summary>
        /// Сведения о покупателе
        /// </summary>
        // [Association("КнПродСтр-СвПокуп")]
        // public XPCollection<СвПокуп> СвПокуп { get { return GetCollection<СвПокуп>("СвПокуп"); } }
        private Int32 _НомерПор;
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [RuleRequiredField]
        [RuleRange(null, DefaultContexts.Save, 1, Int32.MaxValue)]
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
        [Size(1000)]
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
        [RuleRequiredField]
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
                foreach (var плат in ДокПдтвОпл) {
                    if (result == String.Empty)
                        result = плат.НомДокПдтвОпл + " " + плат.ДатаДокПдтвОпл.ToString("dd.MM.yyyy"); 
                    else
                        result = result + "; " + плат.НомДокПдтвОпл + " " + плат.ДатаДокПдтвОпл.ToString("dd.MM.yyyy"); 
                }
                return result;
            }
        }
        private String _ОКВ;
        /// <summary>
        /// Код валюты по ОКВ
        /// </summary>
        [Size(3)]
        [RuleRegularExpression(null, DefaultContexts.Save, "[0-9]{3}")]
        public String ОКВ {
            get { return _ОКВ; }
            set {
                if (!IsLoading) OnChanging("ОКВ", value);
                SetPropertyValue<String>("ОКВ", ref _ОКВ, value);
            }
        }
        private Decimal _СтоимПродСФВ;
        /// <summary>
        /// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог), в валюте счета-фактуры
        /// </summary>
        public Decimal СтоимПродСФВ {
            get { return _СтоимПродСФВ; }
            set {
                if (!IsLoading) OnChanging("СтоимПродСФВ", value);
                SetPropertyValue<Decimal>("СтоимПродСФВ", ref _СтоимПродСФВ, value);
            }
        }
        private Decimal _СтоимПродСФ;
        /// <summary>
        /// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая налог) в рублях и копейках
        /// </summary>
        public Decimal СтоимПродСФ {
            get { return _СтоимПродСФ; }
            set {
                if (!IsLoading) OnChanging("СтоимПродСФ", value);
                SetPropertyValue<Decimal>("СтоимПродСФ", ref _СтоимПродСФ, value);
            }
        }
        private Decimal _СтоимПродСФ18;
        /// <summary>
        /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 18 %
        /// </summary>
        public Decimal СтоимПродСФ18 {
            get { return _СтоимПродСФ18; }
            set {
                if (!IsLoading) OnChanging("СтоимПродСФ18", value);
                SetPropertyValue<Decimal>("СтоимПродСФ18", ref _СтоимПродСФ18, value);
            }
        }
        private Decimal _СтоимПродСФ10;
        /// <summary>
        /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 10
        /// </summary>
        public Decimal СтоимПродСФ10 {
            get { return _СтоимПродСФ10; }
            set {
                if (!IsLoading) OnChanging("СтоимПродСФ10", value);
                SetPropertyValue<Decimal>("СтоимПродСФ10", ref _СтоимПродСФ10, value);
            }
        }
        private Decimal _СтоимПродСФ0;
        /// <summary>
        /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре (без налога) в рублях и копейках, по ставке 0
        /// </summary>
        public Decimal СтоимПродСФ0 {
            get { return _СтоимПродСФ0; }
            set {
                if (!IsLoading) OnChanging("СтоимПродСФ0", value);
                SetPropertyValue<Decimal>("СтоимПродСФ0", ref _СтоимПродСФ0, value);
            }
        }
        private Decimal _СумНДССФ18;
        /// <summary>
        /// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре в рублях и копейках, по ставке 18
        /// </summary>
        public Decimal СумНДССФ18 {
            get { return _СумНДССФ18; }
            set {
                if (!IsLoading) OnChanging("СумНДССФ18", value);
                SetPropertyValue<Decimal>("СумНДССФ18", ref _СумНДССФ18, value);
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомИспрСчФ/ДатаИспрСчФ при наличии элемента ДатаИспрСчФ/НомИспрСчФ", UsedProperties = "НомИспрСчФ,ДатаИспрСчФ")]
        public Boolean НомИспрСчФ_ДатаИспрСчФ {
            get {
                return true;
                //                return (НомИспрСчФ == ValidationMethods._UINT16_NULL) == (ДатаИспрСчФ == ValidationMethods._DATE_NULL); 
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомИспрКСчФ при наличии элемента ДатаИспрКСчФ", UsedProperties = "НомИспрКСчФ,ДатаКСчФПрод")]
        public Boolean НомИспрКСчФ_ДатаИспрКСчФ {
            get {
                return true;
//                return (НомИспрКСчФ == ValidationMethods._UINT16_NULL) == (ДатаИспрКСчФ == ValidationMethods._DATE_NULL); 
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента НомКСчФПрод при наличии одного из элементов: ДатаКСчФПрод или НомИспрКСчФ или ДатаИспрКСчФ", UsedProperties = "НомКСчФПрод,ДатаИспрКСчФ,НомИспрКСчФ")]
        public Boolean НомКСчФПрод_ДатаКСчфПрод_ДатаИспрКСчФ {
            get {
                return true;
                //return (!String.IsNullOrEmpty(НомКСчФПрод)) == (ДатаИспрКСчФ != ValidationMethods._DATE_NULL || НомИспрКСчФ != ValidationMethods._UINT16_NULL || ДатаИспрКСчФ != ValidationMethods._DATE_NULL); 
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента ДатаКСчФПрод при наличии одного из элементов: НомКСчФПрод или НомИспрКСчФ или ДатаИспрКСчФ", UsedProperties = "ДатаКСчФПрод,НомКСчФПрод,ДатаИспрКСчФ")]
        public Boolean НомИспрКСчФ_ДатаКСчфПрод_ДатаИспрКСчФ {
            get {
                return true;
//                return (ДатаКСчФПрод != ValidationMethods._DATE_NULL) == (!String.IsNullOrEmpty(НомКСчФПрод) || НомИспрКСчФ != ValidationMethods._UINT16_NULL || ДатаИспрКСчФ != ValidationMethods._DATE_NULL); 
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента СтоимПродСФВ при наличии элемента ОКВ", UsedProperties = "ОКВ,СтоимПродСФВ")]
        public Boolean СтоимПродСФВ_ОКВ {
            get { return (ОКВ != null && String.Compare(ОКВ, "643", true) != 0) == (СтоимПродСФВ != ValidationMethods._DECIMAL_NULL); }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента СтоимПродСФ при наличии (отсутствии) элемента СтоимПродОсв", UsedProperties = "СтоимПродСФ,СтоимПродОсв")]
        public Boolean СтоимПродСФ_СтоимПродОсв {
            get { return СтоимПродСФ != ValidationMethods._DECIMAL_NULL || СтоимПродОсв != ValidationMethods._DECIMAL_NULL; }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента СумНДССФ18 при наличии (отсутствии) элемента СтоимПродСФ18", UsedProperties = "СумНДССФ18,СтоимПродСФ18")]
        public Boolean СумНДССФ18_СтоимПродСФ18 {
            get { return СтоимПродСФ18 == ValidationMethods._DECIMAL_NULL || СумНДССФ18 != ValidationMethods._DECIMAL_NULL; }
        }

        [Browsable(false)]
        [RuleFromBoolProperty(null, DefaultContexts.Save, CustomMessageTemplate = "Не выполнено условие присутствия (отсутствия) элемента СумНДССФ10 при наличии (отсутствии) элемента СтоимПродСФ10", UsedProperties = "СумНДССФ10,СтоимПродСФ10")]
        public Boolean СумНДССФ10_СтоимПродСФ10 {
            get {
                return СтоимПродСФ10 == ValidationMethods._DECIMAL_NULL || СумНДССФ10 != ValidationMethods._DECIMAL_NULL; }
        }

        public КнПродСтрБаз(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            СвПос = new СвУчСделки(this.Session);
            СвПокуп = new СвУчСделки(this.Session);
            _ДатаСчФПрод = ValidationMethods._DATE_NULL;
            _ДатаИспрСчФ = ValidationMethods._DATE_NULL;
            _ДатаКСчФПрод = ValidationMethods._DATE_NULL;
            _ДатаИспрКСчФ = ValidationMethods._DATE_NULL;
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
                if (НомИспрСчФ > 0 && СчетФактура.ДатаИсправления > ValidationMethods._DATE_NULL)
                    ДатаИспрСчФ = СчетФактура.ДатаИсправления;
                else
                    ДатаИспрСчФ = ValidationMethods._DATE_NULL;
                СвПокупТип = Основание.ЛицоТип;
                СвПокупИНН = Основание.ИНН;
                СвПокупКПП = Основание.КПП;
                СвПокупНаименование = Основание.НаименКонтрагента;
                if ( КорректировочныйСчетФактура != null) {
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
                this.СвПокупНаименование = ОснованиеДокумент.НаименКонтрагента;
                this.СтоимПродСФ = ОснованиеДокумент.СуммаВсего;
            }
        }
    }
}