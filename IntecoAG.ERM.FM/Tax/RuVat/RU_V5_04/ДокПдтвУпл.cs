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
    /// Сведения о документе, подтверждающем уплату налога
    /// </summary>
    [Persistent("FmTaxRuVatV504ДокПдтвУпл")]
    public class ДокПдтвУпл: BaseEntity {
        public ДокПдтвУпл(Session session) : base(session) { }
        private String _НомДокПдтвУпл;
        /// <summary>
        /// Номер документа, подтверждающего уплату налога
        /// </summary>
        [RuleRequiredField]
        [Size(256)]
        public String НомДокПдтвУпл {
            get { return _НомДокПдтвУпл; }
            set {
                if (!IsLoading) OnChanging("НомДокПдтвУпл", value);
                SetPropertyValue<String>("НомДокПдтвУпл", ref _НомДокПдтвУпл, value);
            }
        }
        private DateTime _ДатаДокПдтвУпл;
        /// <summary>
        /// Дата документа, подтверждающего уплату налога
        /// </summary>
        [RuleRequiredField]
        public DateTime ДатаДокПдтвУпл {
            get { return _ДатаДокПдтвУпл; }
            set {
                if (!IsLoading) OnChanging("ДатаДокПдтвУпл", value);
                SetPropertyValue<DateTime>("ДатаДокПдтвУпл", ref _ДатаДокПдтвУпл, value);
            }
        }
        private КнПокСтрБаз _КнПокСтрБаз;
        /// <summary>
        /// Связь КнПокСтр-ДокПдтвУпл
        /// </summary>
        [Association("КнПокСтрБаз-ДокПдтвУпл")]
        public КнПокСтрБаз КнПокСтрБаз {
            get { return _КнПокСтрБаз; }
            set {
                if (!IsLoading) OnChanging("КнПокСтрБаз", value);
                SetPropertyValue<КнПокСтрБаз>("КнПокСтрБаз", ref _КнПокСтрБаз, value);
            }
        }

        //private КнПокСтр _КнПокСтр;
        ///// <summary>
        ///// Связь КнПокСтр-ДокПдтвУпл
        ///// </summary>
        //[Association("КнПокСтр-ДокПдтвУпл")]
        //public КнПокСтр КнПокСтр {
        //    get { return _КнПокСтр; }
        //    set {
        //        if (!IsLoading) OnChanging("КнПокСтр", value);
        //        SetPropertyValue<КнПокСтр>("КнПокСтр", ref _КнПокСтр, value);
        //    }
        //}
        //private КнПокДЛСтр _КнПокДЛСтр;
        ///// <summary>
        ///// Связь КнПокСтр-ДокПдтвУпл
        ///// </summary>
        //[Association("КнПокДЛСтр-ДокПдтвУпл")]
        //public КнПокДЛСтр КнПокДЛСтр {
        //    get { return _КнПокДЛСтр; }
        //    set {
        //        if (!IsLoading) OnChanging("КнПокДЛСтр", value);
        //        SetPropertyValue<КнПокДЛСтр>("КнПокДЛСтр", ref _КнПокДЛСтр, value);
        //    }
        //}
        /// <summary>
        /// Сгенерировать из документа, подтверждающего УПЛату (ДокПдтвУпл) правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("ДокПдтвУпл");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            attrib = document.CreateAttribute("НомДокПдтвУпл");
            attrib.Value = НомДокПдтвУпл.ToString();
            result.Attributes.Append(attrib);
            result.Attributes.Append(ValidationMethods.DateAttribute(document, ДатаДокПдтвУпл, "ДатаДокПдтвУпл"));
            //attrib = document.CreateAttribute("ДатаДокПдтвУпл");
            //attrib.Value = ДатаДокПдтвУпл.ToString();
            //result.Attributes.Append(attrib);
            return result;
        }
    }
}
