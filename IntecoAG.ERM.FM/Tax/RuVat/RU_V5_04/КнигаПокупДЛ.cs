using System;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using System.Xml;
//using System.Xml.Linq;
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
    /// <summary>
    /// Сведения из дополнительных листов книги покупок
    /// </summary>
    [Persistent("FmTaxRuVatV504КнигаПокупДЛ")]
    public class КнигаПокупДЛ : КнигаДокументДопЛист {
        public КнигаПокупДЛ(Session session) : base(session) { }
        /// <summary>
        /// Сведения по строке из дополнительных листов книги покупок
        /// </summary>
        [Association("КнигаПокупДЛ-КнПокДЛСтр"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнПокДЛСтр> КнПокДЛСтр { get { return GetCollection<КнПокДЛСтр>("КнПокДЛСтр"); } }
        private Decimal _СумНДСИтКПк;
        /// <summary>
        /// Итоговая сумма налога по книге покупок
        /// </summary>
        [RuleRequiredField]
        public Decimal СумНДСИтКПк {
            get { return _СумНДСИтКПк; }
            set {
                if (!IsLoading) OnChanging("СумНДСИтКПк", value);
                SetPropertyValue<Decimal>("СумНДСИтКПк", ref _СумНДСИтКПк, value);
            }
        }
        private Decimal _СумНДСИтП1Р8;
        /// <summary>
        /// Сумма налога всего по приложению 1 к разделу 8 в рублях и копейках
        /// </summary>
        [RuleRequiredField]
        public Decimal СумНДСИтП1Р8 {
            get { return _СумНДСИтП1Р8; }
            set {
                if (!IsLoading) OnChanging("СумНДСИтП1Р8", value);
                SetPropertyValue<Decimal>("СумНДСИтП1Р8", ref _СумНДСИтП1Р8, value);
            }
        }

        public override void ToXml() {
            //XmlDocument document = new XmlDocument();
            //XmlNode file_node = document.CreateElement("Файл");
            //document.CreateXmlDeclaration("1.0", "windows-1251", "");
            //document.AppendChild(file_node);
            //XmlAttribute attribute_f1 = document.CreateAttribute("ИдФайл");
            //attribute_f1.Value = "ИдФайл1";
            //XmlAttribute attribute_f2 = document.CreateAttribute("ВерсПрог");
            //attribute_f2.Value = "ВерсПрог1";
            //XmlAttribute attribute_f3 = document.CreateAttribute("ВерсФорм");
            //attribute_f1.Value = "ВерсФорм1";
            //file_node.Attributes.Append(attribute_f1);
            //file_node.Attributes.Append(attribute_f2);
            //file_node.Attributes.Append(attribute_f3);
            //XmlNode doc_node = document.CreateElement("Документ");
            //XmlAttribute attribute_d1 = document.CreateAttribute("Индекс");
            //attribute_d1.Value = "0000080";
            //XmlAttribute attribute_d2 = document.CreateAttribute("НомКорр");
            //attribute_d1.Value = "1";
            //XmlAttribute attribute_d3 = document.CreateAttribute("ПризнСвед8");
            //attribute_d1.Value = "0";
            //file_node.AppendChild(doc_node);
            //file_node.Attributes.Append(attribute_d1);
            //file_node.Attributes.Append(attribute_d2);
            //file_node.Attributes.Append(attribute_d3);
            //doc_node.AppendChild(ToXmlGenerated(document, doc_node));
            //String path = "C:\\XMLGenerated\\";
            //String file_name = "KnigaPokupDL.xml";
            //document.Save(path + file_name);

            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "windows-1251", "");
            XmlNode file_node = document.CreateElement("Файл");
            document.AppendChild(file_node);
            XmlAttribute attribute_id_file = document.CreateAttribute("ИдФайл");
            String fname = Книга.ПериодНДС.Код + ".КнигаПокупок.Дополнительная.xml";
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
            attribute_index.Value = "0000080";
            doc_node.Attributes.Append(attribute_index);
            XmlAttribute attribute_nom_korr = document.CreateAttribute("НомКорр");
            attribute_nom_korr.Value = "0";
            doc_node.Attributes.Append(attribute_nom_korr);
            XmlAttribute attribute_prizn_sved81 = document.CreateAttribute("ПризнСвед81");
            if (!(attribute_nom_korr.Value == "0")) {
                attribute_prizn_sved81.Value = "1";
                doc_node.Attributes.Append(attribute_prizn_sved81);
            }
            if ((attribute_prizn_sved81.Value) == "1") {
                doc_node.AppendChild(ToXmlGenerated(document, doc_node));
            }
            string exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(exename);
            String folder = "KnigaPokup\\" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "\\";
            String path = fi.Directory.FullName + "\\Export\\" + fname;
            document.Save(path);
        }

        /// <summary>
        /// Генерирует по дополнителному листу книги покупок (КнПокДЛСтр) правильный тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнигаПокупДЛ");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            foreach (КнПокДЛСтр str in КнПокДЛСтр) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }
            attrib = document.CreateAttribute("СумНДСИтКПк");
            attrib.Value = СумНДСИтКПк.ToString("F");
            result.Attributes.Append(attrib);
            attrib = document.CreateAttribute("СумНДСИтП1Р8");
            attrib.Value = СумНДСИтП1Р8.ToString("F");
            result.Attributes.Append(attrib);
            return result;
        }

    }

}
