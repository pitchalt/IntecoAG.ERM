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
    /// Сведения по строке из дополнительных листов книги покупок
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save ,"КнигаПокупДЛ;НомерПор")]
    //[Persistent("FmTaxRuVatV504КнПокДЛСтр")]
    public class КнПокДЛСтр : КнПокСтрБаз {

        private Decimal _СумНДС;
        /// <summary>
        /// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре, принимаемая к вычету, в рублях и копейках
        /// </summary>
        [RuleRequiredField]
        public Decimal СумНДС {
            get { return _СумНДС; }
            set {
                if (!IsLoading) OnChanging("СумНДС", value);
                SetPropertyValue<Decimal>("СумНДС", ref _СумНДС, value);
            }
        }
        private КнигаПокупДЛ _КнигаПокупДЛ;
        /// <summary>
        /// Связь КнигаПокупДЛ-КнПокДЛСтр
        /// </summary>
        [Association("КнигаПокупДЛ-КнПокДЛСтр")]
        public КнигаПокупДЛ КнигаПокупДЛ {
            get { return _КнигаПокупДЛ; }
            set {
                if (!IsLoading) OnChanging("КнигаПокупДЛ", value);
                SetPropertyValue<КнигаПокупДЛ>("КнигаПокупДЛ", ref _КнигаПокупДЛ, value);
            }
        }

        public КнПокДЛСтр(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            СвПрод = new СвУчСделки(this.Session);
            СвПос = new СвУчСделки(this.Session);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "КнигаПокупДЛ":
                    КнигаДокумент = КнигаПокупДЛ;
                    break;
            }
        }

        /// <summary>
        /// Генерирует из строки дополнительного листа книги покупок (КнПокДлСтр) правильно заполненный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнПокДЛСтр");
            last_node.AppendChild(result);
            //XmlAttribute attrib;
            //if (КодВидОпер != null) {
            //    XmlNode oper = document.CreateElement("КодВидОпер");
            //    result.AppendChild(oper);
            //    oper.Value = КодВидОпер.Код;
            //}
            ////            foreach (КодВидОпер str in КодВидОпер) {
            ////                result.AppendChild(str.ToXmlGenerated(document, result));
            ////            }
            //foreach (ДокПдтвУпл str in ДокПдтвУпл) {
            //    result.AppendChild(str.ToXmlGenerated(document, result));
            //}
            //if (ДатаУчТов > new DateTime(1900, 01, 01)) {
            //    XmlNode date = document.CreateElement("ДатаУчТов");
            //    result.AppendChild(date);
            //    date.Value = ДатаУчТов.ToString("dd.MM.yyyy");
            //}
            ////            foreach (ДатаУчТов str in ДатаУчТов) {
            ////                result.AppendChild(str.ToXmlGenerated(document, result));
            ////            }
            //result.AppendChild(СвПрод.ToXmlGenerated(document, result));
            ////foreach (СвПрод str in СвПрод) {
            ////    result.AppendChild(str.ToXmlGenerated(document, result));
            ////}
            //if (СвПос != null) result.AppendChild(СвПос.ToXmlGenerated(document, result));
            //attrib = document.CreateAttribute("НомерПор");
            //attrib.Value = НомерПор.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("НомСчФПрод");
            //attrib.Value = НомСчФПрод.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("ДатаСчФПрод");
            //attrib.Value = ДатаСчФПрод.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("НомИспрСчФ");
            //attrib.Value = НомИспрСчФ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("ДатаИспрСчФ");
            //attrib.Value = ДатаИспрСчФ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("НомКСчФПрод");
            //attrib.Value = НомКСчФПрод.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("ДатаКСчФПрод");
            //attrib.Value = ДатаКСчФПрод.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("НомИспрКСчФ");
            //attrib.Value = НомИспрКСчФ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("ДатаИспрКСчФ");
            //attrib.Value = ДатаИспрКСчФ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("НомТД");
            //attrib.Value = НомТД.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("ОКВ");
            //attrib.Value = ОКВ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("СтоимПокупВ");
            //attrib.Value = СтоимПокупВ.ToString();
            //result.Attributes.Append(attrib);
            //attrib = document.CreateAttribute("СумНДС");
            //attrib.Value = СумНДС.ToString();
            //result.Attributes.Append(attrib);

            XmlAttribute attrib;
            if (КодВидОпер != null) {
                XmlNode oper = document.CreateElement("КодВидОпер");
                result.AppendChild(oper);
                oper.InnerText = КодВидОпер.Код;
            }
            foreach (ДокПдтвУпл str in ДокПдтвУпл) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }

            if (ДатаУчТов != ValidationMethods._DATE_NULL) {
                XmlNode date = document.CreateElement("ДатаУчТов");
                result.AppendChild(date);
                date.Value = ДатаУчТов.ToString("dd.MM.yyyy");
            }
            XmlNode node = null;
            XmlNode node2 = null;
            node = СвПрод.ToXmlGenerated(document, result);
            if (node != null) {
                node2 = document.CreateElement("СвПрод");
                node2.AppendChild(node);
                result.AppendChild(node2);
            }
            if (node != null)
                result.AppendChild(node);
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
            if (!String.IsNullOrEmpty(НомТД)) {
                attrib = document.CreateAttribute("НомТД");
                attrib.Value = НомТД.ToString();
                result.Attributes.Append(attrib);
            }
            if (!String.IsNullOrEmpty(НомТД)) {
                attrib = document.CreateAttribute("ОКВ");
                attrib.Value = ОКВ.ToString();
                result.Attributes.Append(attrib);
            }
            attrib = document.CreateAttribute("СтоимПокупВ");
            attrib.Value = СтоимПокупВ.ToString("F2", CultureInfo.InvariantCulture);
            result.Attributes.Append(attrib);
            attrib = document.CreateAttribute("СумНДС");
            attrib.Value = СумНДС.ToString("F2", CultureInfo.InvariantCulture);
            result.Attributes.Append(attrib);
            return result;
        }
    }
}