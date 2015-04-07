using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.ERM.FM.Tax;
using IntecoAG.XafExt.DC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {
    /// <summary>
    /// Сведения по строке из дополнительных листов книги продаж
    /// </summary>
    [Persistent("FmTaxRuVatV504КнПродДЛСтр")]
    public class КнПродДЛСтр : КнПродСтрБаз {
        ///// <summary>
        ///// Сведения о документе, подтверждающем уплату налога
        ///// </summary>
        //[Association("КнПродДЛСтр-ДокПдтвОпл"), DevExpress.Xpo.Aggregated]
        //public XPCollection<ДокПдтвОпл> ДокПдтвОпл { get { return GetCollection<ДокПдтвОпл>("ДокПдтвОпл"); } }

        private КнигаПродДЛ _КнигаПродДЛ;
        /// <summary>
        /// Связь КнигаПродДЛ-КнПродДЛСтр
        /// </summary>
        [Association("КнигаПродДЛ-КнПродДЛСтр")]
        public КнигаПродДЛ КнигаПродДЛ {
            get { return _КнигаПродДЛ; }
            set {
                if (!IsLoading) OnChanging("КнигаПродДЛ", value);
                SetPropertyValue<КнигаПродДЛ>("КнигаПродДЛ", ref _КнигаПродДЛ, value);
            }
        }

        public КнПродДЛСтр(Session session) : base(session) { }
        
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "КнигаПрод":
                    КнигаДокумент = КнигаПродДЛ;
                    break;
            }
        }

        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнПродДЛСтр");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            if (КодВидОпер != null) {
                XmlNode oper = document.CreateElement("КодВидОпер");
                result.AppendChild(oper);
                oper.InnerText = КодВидОпер.Код;
            }
            foreach (ДокПдтвОпл str in ДокПдтвОпл) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }
            result.AppendChild(СвПокуп.ToXmlGenerated(document, result));

            if (СвПос != null) result.AppendChild(СвПос.ToXmlGenerated(document, result));
            attrib = document.CreateAttribute("НомерПор");
            attrib.Value = НомерПор.ToString();
            result.Attributes.Append(attrib);
            attrib = document.CreateAttribute("НомСчФПрод");
            attrib.Value = НомСчФПрод.ToString();
            result.Attributes.Append(attrib);
            if (ДатаСчФПрод != ValidationMethods._DATE_NULL) {
                attrib = document.CreateAttribute("ДатаСчФПрод");
                attrib.Value = ДатаСчФПрод.ToString("dd.MM.yyyy");
                result.Attributes.Append(attrib);
            }
            if (НомИспрСчФ != 0) {
                attrib = document.CreateAttribute("НомИспрСчФ");
                attrib.Value = НомИспрСчФ.ToString();
                result.Attributes.Append(attrib);
            }
            if (ДатаИспрСчФ != ValidationMethods._DATE_NULL) {
                attrib = document.CreateAttribute("ДатаИспрСчФ");
                attrib.Value = ДатаИспрСчФ.ToString("dd.MM.yyyy");
                result.Attributes.Append(attrib);
            }
            if (!String.IsNullOrEmpty(НомКСчФПрод)) {
                attrib = document.CreateAttribute("НомКСчФПрод");
                attrib.Value = НомКСчФПрод.ToString();
                result.Attributes.Append(attrib);
            }
            if (ДатаКСчФПрод != ValidationMethods._DATE_NULL) {
                attrib = document.CreateAttribute("ДатаКСчФПрод");
                attrib.Value = ДатаКСчФПрод.ToString("dd.MM.yyyy");
                result.Attributes.Append(attrib);
            }
            if (НомИспрКСчФ != 0) {
                attrib = document.CreateAttribute("НомИспрКСчФ");
                attrib.Value = НомИспрКСчФ.ToString();
                result.Attributes.Append(attrib);
            }
            if (ДатаИспрКСчФ != ValidationMethods._DATE_NULL) {
                attrib = document.CreateAttribute("ДатаИспрКСчФ");
                attrib.Value = ДатаИспрКСчФ.ToString("dd.MM.yyyy");
                result.Attributes.Append(attrib);
            }
            if (!String.IsNullOrEmpty(ОКВ)) {
                attrib = document.CreateAttribute("ОКВ");
                attrib.Value = ОКВ.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФВ != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФВ");
                attrib.Value = СтоимПродСФВ.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ");
                attrib.Value = СтоимПродСФ.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ18");
                attrib.Value = СтоимПродСФ18.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ10");
                attrib.Value = СтоимПродСФ10.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ0 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ0");
                attrib.Value = СтоимПродСФ0.ToString();
                result.Attributes.Append(attrib);
            }
            if (СумНДССФ18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДССФ18");
                attrib.Value = СумНДССФ18.ToString();
                result.Attributes.Append(attrib);
            }
            if (СумНДССФ10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДССФ10");
                attrib.Value = СумНДССФ10.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродОсв != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродОсв");
                attrib.Value = СтоимПродОсв.ToString();
                result.Attributes.Append(attrib);
            }
            return result;
        }
    }
}