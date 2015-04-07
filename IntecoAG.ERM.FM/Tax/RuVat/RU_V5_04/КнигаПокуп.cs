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

    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("FmTaxRuVatV504КнигаПокуп")]
    [Appearance(null, AppearanceItemType.ViewItem, null, TargetItems = "СумНДСВсКПк", Enabled = false)]
    /// <summary>
    /// Сведения из книги покупок об операциях, отражаемых за истекший налоговый период
    /// </summary>
    public class КнигаПокуп : КнигаДокументОсновная {

        /// <summary>
        /// Сведения по строке из книги покупок об операциях, отражаемых за истекший налоговый период
        /// </summary>
        [Association("КнигаПокуп-КнПокСтр"), DevExpress.Xpo.Aggregated]
        public XPCollection<КнПокСтр> КнПокСтр { get { return GetCollection<КнПокСтр>("КнПокСтр"); } }
        private Decimal _СумНДСВсКПк;
        /// <summary>
        /// Сумма налога всего по книге покупок в рублях и копейках
        /// </summary>
        [RuleRequiredField]
        public Decimal СумНДСВсКПк {
            get { return _СумНДСВсКПк; }
            set {
                if (!IsLoading) OnChanging("СумНДСВсКПк", value);
                SetPropertyValue<Decimal>("СумНДСВсКПк", ref _СумНДСВсКПк, value);
            }
        }

        public КнигаПокуп(Session session) : base(session) { }

        public override void ToXml() {            
            XmlDocument document = new XmlDocument();
            document.CreateXmlDeclaration("1.0", "windows-1251", "");
            XmlNode file_node = document.CreateElement("Файл");
            document.AppendChild(file_node);
            XmlAttribute attribute_id_file = document.CreateAttribute("ИдФайл");
            String fname = Книга.ПериодНДС.Код + ".КнигаПокупок.Основная.xml";
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
            attribute_nom_korr.Value = "1";
            doc_node.Attributes.Append(attribute_nom_korr);
            XmlAttribute attribute_prizn_sved8 = document.CreateAttribute("ПризнСвед8");
            if(!(attribute_nom_korr.Value == "0")){
                attribute_prizn_sved8.Value = "1";
                doc_node.Attributes.Append(attribute_prizn_sved8);
            }
            if ((attribute_prizn_sved8.Value) == "1") {
                doc_node.AppendChild(ToXmlGenerated(document, doc_node));
            }
            string exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(exename);
            String folder = "KnigaPokup\\" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "\\";
            String path = fi.Directory.FullName + "\\Export\\" + fname;
            document.Save(path);
        }

        /// <summary>
        /// Преобразует книгу покупок в неоходимый тег
        /// </summary>
        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = document.CreateElement("КнигаПокуп");
            last_node.AppendChild(result);
            XmlAttribute attrib;
            foreach (КнПокСтр str in КнПокСтр) {
                result.AppendChild(str.ToXmlGenerated(document, result));
            }
            if (СумНДСВсКПк != ValidationMethods._DECIMAL_NULL) {
                attrib = document.CreateAttribute("СумНДСВсКПк");
                attrib.Value = СумНДСВсКПк.ToString("F");
                result.Attributes.Append(attrib);
            }
            return result;
        }
    }
}