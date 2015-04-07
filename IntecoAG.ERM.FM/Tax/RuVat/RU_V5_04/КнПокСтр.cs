using System;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using IntecoAG.XafExt.DC;
using IntecoAG.ERM.FM.Tax;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {
    [MapInheritance(MapInheritanceType.ParentTable)]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "КнигаПокуп;НомерПор")]
    //[Persistent("FmTaxRuVatV504КнПокСтр")]
    /// <summary>
    /// Сведения по строке из книги покупок об операциях, отражаемых за истекший налоговый период
    /// </summary>
    public class КнПокСтр : КнПокСтрБаз {

        private Decimal _СумНДСВыч;
        /// <summary>
        /// Сумма налога по счету-фактуре, разница суммы налога по корректировочному счету-фактуре, принимаемая к вычету, в рублях и копейках
        /// </summary>
        [RuleRequiredField]
        public Decimal СумНДСВыч {
            get { return _СумНДСВыч; }
            set {
                if (!IsLoading) OnChanging("СумНДСВыч", value);
                SetPropertyValue<Decimal>("СумНДСВыч", ref _СумНДСВыч, value);
            }
        }
        private КнигаПокуп _КнигаПокуп;
        /// <summary>
        /// Связь КнигаПокуп-КнПокСтр
        /// </summary>
        [Association("КнигаПокуп-КнПокСтр")]
        public КнигаПокуп КнигаПокуп {
            get { return _КнигаПокуп; }
            set {
                if (!IsLoading) OnChanging("КнигаПокуп", value);
                SetPropertyValue<КнигаПокуп>("КнигаПокуп", ref _КнигаПокуп, value);
            }
        }

        public КнПокСтр(Session session) : base(session) { }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            Decimal old_sum, new_sum;
            switch (propertyName) {
                case "КнигаПокуп":
                    КнигаДокумент = КнигаПокуп;
                    break;
                case "СумНДСВыч":
                    old_sum = (Decimal)oldValue;
                    new_sum = (Decimal)newValue;
                    КнигаПокуп.СумНДСВсКПк = КнигаПокуп.СумНДСВсКПк - old_sum + new_sum;
                    break;
            }
        }
        /// <summary>
        /// Преобразует строку книги покупок (КнПокСтр) в правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнПокСтр");
            last_node.AppendChild(result);
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
            attrib.Value = НомСчФПрод;
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
                attrib.Value = НомКСчФПрод;
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
            if (!String.IsNullOrEmpty(НомТД)) {
                attrib = document.CreateAttribute("НомТД");
                attrib.Value = НомТД;
                result.Attributes.Append(attrib);
            }
            if (!String.IsNullOrEmpty(НомТД)) {
                attrib = document.CreateAttribute("ОКВ");
                attrib.Value = ОКВ;
                result.Attributes.Append(attrib);
            }
            attrib = document.CreateAttribute("СтоимПокупВ");
            attrib.Value = СтоимПокупВ.ToString("F");
            result.Attributes.Append(attrib);
            attrib = document.CreateAttribute("СумНДСВыч");
            attrib.Value = СумНДСВыч.ToString("F");
            result.Attributes.Append(attrib);
            return result;
        }
    }
}
