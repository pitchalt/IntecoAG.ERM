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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Xml.Linq;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {
    /// <summary>
    /// Сведения из дополнительных листов книги продаж
    /// </summary>
    [Persistent("FmTaxRuVatV504КнигаПродДЛ")]
    public class КнигаПродДЛ : КнигаДокументДопЛист {
        public КнигаПродДЛ(Session session) : base(session) { }
        /// <summary>
        /// Сведения по строке из дополнительных листов книги продаж
        /// </summary>
        [Association("КнигаПродДЛ-КнПродДЛСтр"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнПродДЛСтр> КнПродДЛСтр { get { return GetCollection<КнПродДЛСтр>("КнПродДЛСтр"); } }
        private Decimal _ИтСтПродКПр18;
        /// <summary>
        /// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 18%
        /// </summary>
        public Decimal ИтСтПродКПр18 {
            get { return _ИтСтПродКПр18; }
            set {
                if (!IsLoading) OnChanging("ИтСтПродКПр18", value);
                SetPropertyValue<Decimal>("ИтСтПродКПр18", ref _ИтСтПродКПр18, value);
            }
        }
        private Decimal _ИтСтПродКПр10;
        /// <summary>
        /// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 10%
        /// </summary>
        public Decimal ИтСтПродКПр10 {
            get { return _ИтСтПродКПр10; }
            set {
                if (!IsLoading) OnChanging("ИтСтПродКПр10", value);
                SetPropertyValue<Decimal>("ИтСтПродКПр10", ref _ИтСтПродКПр10, value);
            }
        }
        private Decimal _ИтСтПродКПр0;
        /// <summary>
        /// Итоговая стоимость продаж по книге продаж (без налога) в рублях и копейках, по ставке 0%
        /// </summary>
        public Decimal ИтСтПродКПр0 {
            get { return _ИтСтПродКПр0; }
            set {
                if (!IsLoading) OnChanging("ИтСтПродКПр0", value);
                SetPropertyValue<Decimal>("ИтСтПродКПр0", ref _ИтСтПродКПр0, value);
            }
        }
        private Decimal _СумНДСИтКПр18;
        /// <summary>
        /// Итоговая сумма налога по книге продаж в рублях и копейках,  по ставке 18%
        /// </summary>
        public Decimal СумНДСИтКПр18 {
            get { return _СумНДСИтКПр18; }
            set {
                if (!IsLoading) OnChanging("СумНДСИтКПр18", value);
                SetPropertyValue<Decimal>("СумНДСИтКПр18", ref _СумНДСИтКПр18, value);
            }
        }
        private Decimal _СумНДСИтКПр10;
        /// <summary>
        /// Итоговая сумма налога по книге продаж в рублях и копейках,  по ставке 10%
        /// </summary>
        public Decimal СумНДСИтКПр10 {
            get { return _СумНДСИтКПр10; }
            set {
                if (!IsLoading) OnChanging("СумНДСИтКПр10", value);
                SetPropertyValue<Decimal>("СумНДСИтКПр10", ref _СумНДСИтКПр10, value);
            }
        }
        private Decimal _ИтСтПродОсвКПр;
        /// <summary>
        /// Итоговая стоимость продаж, освобождаемых от налога, по книге продаж в рублях и копейках
        /// </summary>
        public Decimal ИтСтПродОсвКПр {
            get { return _ИтСтПродОсвКПр; }
            set {
                if (!IsLoading) OnChanging("ИтСтПродОсвКПр", value);
                SetPropertyValue<Decimal>("ИтСтПродОсвКПр", ref _ИтСтПродОсвКПр, value);
            }
        }
        private Decimal _СтПродВсП1Р9_18;
        /// <summary>
        /// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 18%
        /// </summary>
        public Decimal СтПродВсП1Р9_18 {
            get { return _СтПродВсП1Р9_18; }
            set {
                if (!IsLoading) OnChanging("СтПродВсП1Р9_18", value);
                SetPropertyValue<Decimal>("СтПродВсП1Р9_18", ref _СтПродВсП1Р9_18, value);
            }
        }
        private Decimal _СтПродВсП1Р9_10;
        /// <summary>
        /// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 10%
        /// </summary>
        public Decimal СтПродВсП1Р9_10 {
            get { return _СтПродВсП1Р9_10; }
            set {
                if (!IsLoading) OnChanging("СтПродВсП1Р9_10", value);
                SetPropertyValue<Decimal>("СтПродВсП1Р9_10", ref _СтПродВсП1Р9_10, value);
            }
        }
        private Decimal _СтПродВсП1Р9_0;
        /// <summary>
        /// Всего стоимость продаж по Приложению 1 к Разделу 9 (без налога) в рублях и копейках, по ставке 0%
        /// </summary>
        public Decimal СтПродВсП1Р9_0 {
            get { return _СтПродВсП1Р9_0; }
            set {
                if (!IsLoading) OnChanging("СтПродВсП1Р9_0", value);
                SetPropertyValue<Decimal>("СтПродВсП1Р9_0", ref _СтПродВсП1Р9_0, value);
            }
        }
        private Decimal _СумНДСВсП1Р9_18;
        /// <summary>
        /// Всего сумма налога по Приложению 1 к Разделу 9 в рублях и копейках, по ставке  18%
        /// </summary>
        public Decimal СумНДСВсП1Р9_18 {
            get { return _СумНДСВсП1Р9_18; }
            set {
                if (!IsLoading) OnChanging("СумНДСВсП1Р9_18", value);
                SetPropertyValue<Decimal>("СумНДСВсП1Р9_18", ref _СумНДСВсП1Р9_18, value);
            }
        }
        private Decimal _СумНДСВсП1Р9_10;
        /// <summary>
        /// Всего сумма налога по Приложению 1 к Разделу 9 в рублях и копейках, по ставке 10%
        /// </summary>
        public Decimal СумНДСВсП1Р9_10 {
            get { return _СумНДСВсП1Р9_10; }
            set {
                if (!IsLoading) OnChanging("СумНДСВсП1Р9_10", value);
                SetPropertyValue<Decimal>("СумНДСВсП1Р9_10", ref _СумНДСВсП1Р9_10, value);
            }
        }
        private Decimal _СтПродОсвП1Р9Вс;
        /// <summary>
        /// Всего стоимость продаж, освобождаемых от налога, по приложению 1 к Разделу 9 в рублях и копейках
        /// </summary>
        public Decimal СтПродОсвП1Р9Вс {
            get { return _СтПродОсвП1Р9Вс; }
            set {
                if (!IsLoading) OnChanging("СтПродОсвП1Р9Вс", value);
                SetPropertyValue<Decimal>("СтПродОсвП1Р9Вс", ref _СтПродОсвП1Р9Вс, value);
            }
        }

        //public override void ToXml() {
        //    XmlDocument document = new XmlDocument();
        //    XmlNode file_node = document.CreateElement("Файл");
        //    document.CreateXmlDeclaration("1.0", "windows-1251", "");
        //    document.AppendChild(file_node);
        //    XmlAttribute attribute_f1 = document.CreateAttribute("ИдФайл");
        //    attribute_f1.Value = "ИдФайл1";
        //    XmlAttribute attribute_f2 = document.CreateAttribute("ВерсПрог");
        //    attribute_f2.Value = "ВерсПрог1";
        //    XmlAttribute attribute_f3 = document.CreateAttribute("ВерсФорм");
        //    attribute_f1.Value = "ВерсФорм1";
        //    file_node.Attributes.Append(attribute_f1);
        //    file_node.Attributes.Append(attribute_f2);
        //    file_node.Attributes.Append(attribute_f3);
        //    XmlNode doc_node = document.CreateElement("Документ");
        //    XmlAttribute attribute_d1 = document.CreateAttribute("Индекс");
        //    attribute_d1.Value = "0000080";
        //    XmlAttribute attribute_d2 = document.CreateAttribute("НомКорр");
        //    attribute_d1.Value = "1";
        //    XmlAttribute attribute_d3 = document.CreateAttribute("ПризнСвед8");
        //    attribute_d1.Value = "0";
        //    file_node.AppendChild(doc_node);
        //    file_node.Attributes.Append(attribute_d1);
        //    file_node.Attributes.Append(attribute_d2);
        //    file_node.Attributes.Append(attribute_d3);
        //    doc_node.AppendChild(ToXmlGenerated(document, doc_node));
        //    String path = "C:\\XMLGenerated\\";
        //    String file_name = "KnigaProdDL.xml";
        //    document.Save(path + file_name);
        //}

        public override void ToXml() {
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "windows-1251", "");
            XmlNode file_node = document.CreateElement("Файл");
            document.AppendChild(file_node);
            XmlAttribute attribute_id_file = document.CreateAttribute("ИдФайл");
            String fname = Книга.ПериодНДС.Код + ".КнигаПродаж.Дополнительная.xml";
            attribute_id_file.Value = fname;
            XmlAttribute attribute_vers_prog = document.CreateAttribute("ВерсПрог");
            attribute_vers_prog.Value = "1.0";
            XmlAttribute attribute_vers_form = document.CreateAttribute("ВерсФорм");
            attribute_vers_form.Value = "5.04";
            file_node.Attributes.Append(attribute_id_file);
            file_node.Attributes.Append(attribute_vers_prog);
            file_node.Attributes.Append(attribute_vers_form);
            XmlNode doc_node = document.CreateElement("Документ");
            file_node.AppendChild(doc_node);
            XmlAttribute attribute_index = document.CreateAttribute("Индекс");
            attribute_index.Value = "0000090";
            doc_node.Attributes.Append(attribute_index);
            XmlAttribute attribute_nom_korr = document.CreateAttribute("НомКорр");
            attribute_nom_korr.Value = "0";
            doc_node.Attributes.Append(attribute_nom_korr);
            XmlAttribute attribute_prizn_sved91 = document.CreateAttribute("ПризнСвед91");
            if (!(attribute_nom_korr.Value == "0")) {
                attribute_prizn_sved91.Value = "1";
                doc_node.Attributes.Append(attribute_prizn_sved91);
            }
            if ((attribute_prizn_sved91.Value) == "1") {
                doc_node.AppendChild(ToXmlGenerated(document, doc_node));
            }
            string exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(exename);
            String folder = "KnigaProd\\" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "\\";
            String path = fi.Directory.FullName + "\\Export\\" + fname;
            document.Save(path);
        }

        /// <summary>
        /// Генерирует из дополнительного листа книги продаж (КнигаПродДЛ) правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнигаПродДЛ");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            foreach (КнПродДЛСтр str in КнПродДЛСтр) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }
            if (ИтСтПродКПр18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("ИтСтПродКПр18");
                attrib.Value = ИтСтПродКПр18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (ИтСтПродКПр10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("ИтСтПродКПр10");
                attrib.Value = ИтСтПродКПр10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (ИтСтПродКПр0 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("ИтСтПродКПр0");
                attrib.Value = ИтСтПродКПр0.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСИтКПр18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСИтКПр18");
                attrib.Value = СумНДСИтКПр18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСИтКПр10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСИтКПр10");
                attrib.Value = СумНДСИтКПр10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (ИтСтПродОсвКПр != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("ИтСтПродОсвКПр");
                attrib.Value = ИтСтПродОсвКПр.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродВсП1Р9_18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродВсП1Р9_18");
                attrib.Value = СтПродВсП1Р9_18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродВсП1Р9_10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродВсП1Р9_10");
                attrib.Value = СтПродВсП1Р9_10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродВсП1Р9_0 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродВсП1Р9_0");
                attrib.Value = СтПродВсП1Р9_0.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСВсП1Р9_18 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСВсП1Р9_18");
                attrib.Value = СумНДСВсП1Р9_18.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СумНДСВсП1Р9_10 != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСВсП1Р9_10");
                attrib.Value = СумНДСВсП1Р9_10.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            if (СтПродОсвП1Р9Вс != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СтПродОсвП1Р9Вс");
                attrib.Value = СтПродОсвП1Р9Вс.ToString("F2", CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            return result;
        }
    }
}