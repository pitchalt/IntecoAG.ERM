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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {
    /// <summary>
    /// Сведения по строке из книги продаж об операциях, отражаемых за истекший налоговый период
    /// </summary>
    
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("FmTaxRuVatV504КнПродСтр")]
    public class КнПродСтр : КнПродСтрБаз {

        private КнигаПрод _КнигаПрод;
        /// <summary>
        /// Связь КнигаПрод-КнПродСтр
        /// </summary>
        [Association("КнигаПрод-КнПродСтр")]
        public КнигаПрод КнигаПрод {
            get { return _КнигаПрод; }
            set {
                if (!IsLoading) OnChanging("КнигаПрод", value);
                SetPropertyValue<КнигаПрод>("КнигаПрод", ref _КнигаПрод, value);
            }
        }

        public КнПродСтр(Session session) : base(session) { }
        //        private Decimal СтПродБезНДС18;
        //        private Decimal СтПродБезНДС10;
        //        private Decimal СтПродБезНДС0;
        //        private Decimal СумНДСВсКПр18;
        //        private Decimal СумНДСВсКПр10;
        //        private Decimal СтПродОсвВсКПр;
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            Decimal old_sum, new_sum;
            switch (propertyName) {
                case "КнигаПрод":
                    КнигаДокумент = КнигаПрод;
                    break;
                case "СтоимПродСФ18":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СтПродБезНДС18 = КнигаПрод.СтПродБезНДС18 - old_sum + new_sum;
                    break;
                case "СтоимПродСФ10":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СтПродБезНДС10 = КнигаПрод.СтПродБезНДС10 - old_sum + new_sum;
                    break;
                case "СтоимПродСФ0":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СтПродБезНДС0 = КнигаПрод.СтПродБезНДС0 - old_sum + new_sum;
                    break;
                case "СумНДССФ18":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СумНДСВсКПр18 = КнигаПрод.СумНДСВсКПр18 - old_sum + new_sum;
                    break;
                case "СумНДССФ10":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СумНДСВсКПр10 = КнигаПрод.СумНДСВсКПр10 - old_sum + new_sum;
                    break;
                case "СтоимПродОсв":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПрод.СтПродОсвВсКПр = КнигаПрод.СтПродОсвВсКПр - old_sum + new_sum;
                    break;
            }
        }
        /// <summary>
        /// Генерирует из строки книги продаж (КнПродСтр) правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнПродСтр");
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
            XmlNode node = null;
            XmlNode node2 = null;
            node = СвПокуп.ToXmlGenerated(document, result);
            if (node != null) {
                node2 = document.CreateElement("СвПокуп");
                node2.AppendChild(node);
                result.AppendChild(node2);
            }
            if (СвПос != null) {
                node = СвПос.ToXmlGenerated(document, result);
                if (node != null) {
                    node2 = document.CreateElement("СвПос");
                    node2.AppendChild(node);
                    result.AppendChild(node2);
                }
            }
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
            if (НомИспрСчФ != ValidationMethods._UINT16_NULL) {
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
            if (НомИспрКСчФ != ValidationMethods._UINT16_NULL) {
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
            if (!String.IsNullOrEmpty(ОКВ) && ОКВ != "643") {
                attrib = document.CreateAttribute("СтоимПродСФВ");
                attrib.Value = СтоимПродСФВ.ToString();
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ != ValidationMethods._DECIMAL_NULL || СтоимПродОсв != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ");
                attrib.Value = СтоимПродСФ.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ18 != ValidationMethods._DECIMAL_NULL || СумНДССФ18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ18");
                attrib.Value = СтоимПродСФ18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ10 != ValidationMethods._DECIMAL_NULL || СумНДССФ10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ10");
                attrib.Value = СтоимПродСФ10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ0 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродСФ0");
                attrib.Value = СтоимПродСФ0.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ18 != ValidationMethods._DECIMAL_NULL || СумНДССФ18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДССФ18");
                attrib.Value = СумНДССФ18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ10 != ValidationMethods._DECIMAL_NULL || СумНДССФ10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДССФ10");
                attrib.Value = СумНДССФ10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтоимПродСФ != ValidationMethods._DECIMAL_NULL || СтоимПродОсв != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтоимПродОсв");
                attrib.Value = СтоимПродОсв.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            return result;
        }
    }
}