using System;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using System.Xml;
//using System.Xml.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
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
    /// <summary>
    /// Сведения о документе, подтверждающем оплату
    /// </summary>
    [Persistent("FmTaxRuVatV504ДокПдтвОпл")]
    public class ДокПдтвОпл: BaseEntity {
        public ДокПдтвОпл(Session session) : base(session) { }
        private String _НомДокПдтвОпл;
        /// <summary>
        /// Номер документа, подтверждающего оплату
        /// </summary>
        [RuleRequiredField]
        [Size(20)]
        public String НомДокПдтвОпл {
            get { return _НомДокПдтвОпл; }
            set {
                if (!IsLoading) OnChanging("НомДокПдтвОпл", value);
                SetPropertyValue<String>("НомДокПдтвОпл", ref _НомДокПдтвОпл, value);
            }
        }
        private DateTime _ДатаДокПдтвОпл;
        /// <summary>
        /// Дата документа, подтверждающего оплату
        /// </summary>
        [RuleRequiredField]
        public DateTime ДатаДокПдтвОпл {
            get { return _ДатаДокПдтвОпл; }
            set {
                if (!IsLoading) OnChanging("ДатаДокПдтвОпл", value);
                SetPropertyValue<DateTime>("ДатаДокПдтвОпл", ref _ДатаДокПдтвОпл, value);
            }
        }
        private КнПродСтрБаз _КнПродСтр;
        /// <summary>
        /// Связь КнПродСтр-ДокПдтвОпл
        /// </summary>
        [Association("КнПродСтр-ДокПдтвОпл")]
        public КнПродСтрБаз КнПродСтр {
            get { return _КнПродСтр; }
            set {
                if (!IsLoading) OnChanging("КнПродСтр", value);
                SetPropertyValue<КнПродСтрБаз>("КнПродСтр", ref _КнПродСтр, value);
            }
        }
        //private КнПродДЛСтр _КнПродДЛСтр;
        ///// <summary>
        ///// Связь КнПродСтр-ДокПдтвОпл
        ///// </summary>
        //[Association("КнПродДЛСтр-ДокПдтвОпл")]
        //public КнПродДЛСтр КнПродДЛСтр {
        //    get { return _КнПродДЛСтр; }
        //    set {
        //        if (!IsLoading) OnChanging("КнПродДЛСтр", value);
        //        SetPropertyValue<КнПродДЛСтр>("КнПродДЛСтр", ref _КнПродДЛСтр, value);
        //    }
        //}
        /// <summary>
        /// Сгенерировать из документа, подтверждающего ОПЛату (ДокПдтвОпл) правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("ДокПдтвОпл");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            attrib = document.CreateAttribute("НомДокПдтвОпл");
            attrib.Value = НомДокПдтвОпл.ToString();
            result.Attributes.Append(attrib);
            result.Attributes.Append(ValidationMethods.DateAttribute(document, ДатаДокПдтвОпл, "ДатаДокПдтвОпл"));
            //attrib = document.CreateAttribute("ДатаДокПдтвОпл");
            //attrib.Value = ДатаДокПдтвОпл.ToString();
            //result.Attributes.Append(attrib);
            return result;
        }
    }
}
