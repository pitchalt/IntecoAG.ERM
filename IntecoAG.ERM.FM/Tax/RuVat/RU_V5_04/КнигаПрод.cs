using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.XafExt.DC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {

    [MapInheritance(MapInheritanceType.OwnTable)]
    [Appearance(null, AppearanceItemType.ViewItem, null, 
        TargetItems = "СтПродБезНДС18;СтПродБезНДС10;СтПродБезНДС0;СумНДСВсКПр18;СумНДСВсКПр10;СтПродОсвВсКПр", Enabled = false)]
//        private Decimal _СтПродБезНДС18;
//        private Decimal _СтПродБезНДС10;
//        private Decimal _СтПродБезНДС0;
//        private Decimal _СумНДСВсКПр18;
//        private Decimal _СумНДСВсКПр10;
//        private Decimal _СтПродОсвВсКПр;
    /// <summary>
    /// Сведения из книги продаж об операциях, отражаемых за истекший налоговый период
    /// </summary>
    [Persistent("FmTaxRuVatV504КнигаПрод")]
    public class КнигаПрод : КнигаДокументОсновная {
        /// <summary>
        /// Сведения по строке из книги продаж об операциях, отражаемых за истекший налоговый период
        /// </summary>
        [Association("КнигаПрод-КнПродСтр"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнПродСтр> КнПродСтр { get { return GetCollection<КнПродСтр>("КнПродСтр"); } }
        private Decimal _СтПродБезНДС18;
        /// <summary>
        /// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 18%
        /// </summary>
        public Decimal СтПродБезНДС18 {
            get { return _СтПродБезНДС18; }
            set {
                if (!IsLoading) OnChanging("СтПродБезНДС18", value);
                SetPropertyValue<Decimal>("СтПродБезНДС18", ref _СтПродБезНДС18, value);
            }
        }
        private Decimal _СтПродБезНДС10;
        /// <summary>
        /// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 10%
        /// </summary>
        public Decimal СтПродБезНДС10 {
            get { return _СтПродБезНДС10; }
            set {
                if (!IsLoading) OnChanging("СтПродБезНДС10", value);
                SetPropertyValue<Decimal>("СтПродБезНДС10", ref _СтПродБезНДС10, value);
            }
        }
        private Decimal _СтПродБезНДС0;
        /// <summary>
        /// Всего стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 0%
        /// </summary>
        public Decimal СтПродБезНДС0 {
            get { return _СтПродБезНДС0; }
            set {
                if (!IsLoading) OnChanging("СтПродБезНДС0", value);
                SetPropertyValue<Decimal>("СтПродБезНДС0", ref _СтПродБезНДС0, value);
            }
        }
        private Decimal _СумНДСВсКПр18;
        /// <summary>
        /// Всего сумма налога по книге продаж в рублях и копейках, по ставке 18%
        /// </summary>
        public Decimal СумНДСВсКПр18 {
            get { return _СумНДСВсКПр18; }
            set {
                if (!IsLoading) OnChanging("СумНДСВсКПр18", value);
                SetPropertyValue<Decimal>("СумНДСВсКПр18", ref _СумНДСВсКПр18, value);
            }
        }
        private Decimal _СумНДСВсКПр10;
        /// <summary>
        /// Всего сумма налога по книге продаж в рублях и копейках, по ставке 10%
        /// </summary>
        public Decimal СумНДСВсКПр10 {
            get { return _СумНДСВсКПр10; }
            set {
                if (!IsLoading) OnChanging("СумНДСВсКПр10", value);
                SetPropertyValue<Decimal>("СумНДСВсКПр10", ref _СумНДСВсКПр10, value);
            }
        }
        private Decimal _СтПродОсвВсКПр;
        /// <summary>
        /// Всего стоимость продаж, освобождаемых от налога, по книге продаж в рублях и копейках
        /// </summary>
        public Decimal СтПродОсвВсКПр {
            get { return _СтПродОсвВсКПр; }
            set {
                if (!IsLoading) OnChanging("СтПродОсвВсКПр", value);
                SetPropertyValue<Decimal>("СтПродОсвВсКПр", ref _СтПродОсвВсКПр, value);
            }
        }
        public КнигаПрод(Session session) : base(session) { }

        public override void ToXml() {
            XmlDocument document = new XmlDocument();
            XmlNode file_node = document.CreateElement("Файл");
            document.CreateXmlDeclaration("1.0", "windows-1251", "");
            document.AppendChild(file_node);
            XmlAttribute attribute_f1 = document.CreateAttribute("ИдФайл");
            String fname = Книга.ПериодНДС.Код + ".КнигаПродаж.Основная.xml";
            attribute_f1.Value = fname;
            XmlAttribute attribute_f2 = document.CreateAttribute("ВерсПрог");
            attribute_f2.Value = "1.0";
            XmlAttribute attribute_f3 = document.CreateAttribute("ВерсФорм");
            attribute_f3.Value = "5.04";
            file_node.Attributes.Append(attribute_f1);
            file_node.Attributes.Append(attribute_f2);
            file_node.Attributes.Append(attribute_f3);
            XmlNode doc_node = document.CreateElement("Документ");
            XmlAttribute attribute_d1 = document.CreateAttribute("Индекс");
            attribute_d1.Value = "0000090";
            XmlAttribute attribute_d2 = document.CreateAttribute("НомКорр");
            attribute_d2.Value = "1";
            file_node.AppendChild(doc_node);
            doc_node.Attributes.Append(attribute_d1);
            doc_node.Attributes.Append(attribute_d2);
            XmlAttribute attribute_prizn_sved9 = document.CreateAttribute("ПризнСвед9");
            if (!(attribute_d2.Value == "0")) {
                attribute_prizn_sved9.Value = "1";
                doc_node.Attributes.Append(attribute_prizn_sved9);
            }
            if ((attribute_prizn_sved9.Value) == "1") {
                doc_node.AppendChild(ToXmlGenerated(document, doc_node));
            }
            String exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(exename);
            String folder = "KnigaProd\\" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "\\";
            String path = fi.Directory.FullName + "\\Export\\" + fname;
            document.Save(path);
        }

        /// <summary>
        /// Преобразует книгу покупок в неоходимый тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнигаПрод");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            foreach (КнПродСтр str in КнПродСтр) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }
            if (СтПродБезНДС18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродБезНДС18");
                attrib.Value = СтПродБезНДС18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродБезНДС10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродБезНДС10");
                attrib.Value = СтПродБезНДС10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродБезНДС0 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродБезНДС0");
                attrib.Value = СтПродБезНДС0.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСВсКПр18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСВсКПр18");
                attrib.Value = СумНДСВсКПр18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСВсКПр10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСВсКПр10");
                attrib.Value = СумНДСВсКПр10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродОсвВсКПр != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродОсвВсКПр");
                attrib.Value = СтПродОсвВсКПр.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            return result;
        }
    }
}