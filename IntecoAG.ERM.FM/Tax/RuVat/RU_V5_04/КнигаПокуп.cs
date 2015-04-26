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
            document.AppendChild(document.CreateXmlDeclaration("1.0", "windows-1251", ""));
            XmlNode file_node = document.CreateElement("Файл");
            document.AppendChild(file_node);
            XmlAttribute attribute_f1 = document.CreateAttribute("ИдФайл");
            String fname = "NO_NDS.8" + '_' + "5099" + '_' + "5099" + '_' + Книга.ПериодНДС.Налогоплательщик.ИНН +
                    Книга.ПериодНДС.Налогоплательщик.КПП + '_' + DateTime.Now.ToString("yyyyMMdd") + '_' + "000001"
                    ;
            //XmlAttribute attribute_id_file = document.CreateAttribute("ИдФайл");
            //String fname = Книга.ПериодНДС.Код + ".КнигаПокупок.Основная.xml";
            attribute_f1.Value = fname;
            XmlAttribute attribute_vers_prog = document.CreateAttribute("ВерсПрог");
            attribute_vers_prog.Value = "1.0";
            XmlAttribute attribute_vers_form = document.CreateAttribute("ВерсФорм");
            attribute_vers_form.Value = "5.04";
            file_node.Attributes.Append(attribute_f1);
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
            XmlAttribute attribute_prizn_sved8 = document.CreateAttribute("ПризнСвед8");
            if(!(attribute_nom_korr.Value == "0")){
                attribute_prizn_sved8.Value = "1";
                doc_node.Attributes.Append(attribute_prizn_sved8);
            }
//            if ((attribute_prizn_sved8.Value) == "1") {
                doc_node.AppendChild(ToXmlGenerated(document, doc_node));
//            }
//            string exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
//            FileInfo fi = new FileInfo(exename);
//            String folder = "KnigaPokup\\" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "\\";
//            String path = fi.Directory.FullName + "\\Export\\" + fname;
            document.Save("S:\\" + fname + ".xml");
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
                attrib.Value = СумНДСВсКПк.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                result.Attributes.Append(attrib);
            }
            return result;
        }

        [Action]
        public void Обновить() {
            IObjectSpace os_cur = CommonMethods.FindObjectSpaceByObject(this);
            using (IObjectSpace os = os_cur.CreateNestedObjectSpace()) {
                КнигаПокуп book = os.GetObject<КнигаПокуп>(this);
                Обновить(os, book);
                os.CommitChanges();
            }
        }

        public static void Обновить(IObjectSpace os, КнигаПокуп book) {
            os.Delete(book.КнПокСтр);
            Int32 cur_num = 1;
            book.Книга.СтрокиКниги.Sorting.Add(new SortProperty("Основание.Дата", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (var line in book.Книга.СтрокиКниги) {
                КнПокСтр doc_line = os.CreateObject<КнПокСтр>();
                book.КнПокСтр.Add(doc_line);
                doc_line.КнигаСтрока = line;
                doc_line.НомерПор = cur_num++;
                DateTime дата_учета = default(DateTime);
//                DateTime дата_учета = default(DateTime);
                foreach (Операция oper in line.Операции) {
                    if (oper.Основание.Тип == Основание.ТипОснования.СФА) {
                        ДокПдтвУпл pay_doc = null;
                        foreach (ДокПдтвУпл doc in doc_line.ДокПдтвУпл) {
                            if (doc.НомДокПдтвУпл == oper.ПДНомер && doc.ДатаДокПдтвУпл == oper.ПДДата) {
                                pay_doc = doc;
                                break;
                            }
                        }
                        if (pay_doc == null) {
                            pay_doc = os.CreateObject<ДокПдтвУпл>();
                            doc_line.ДокПдтвУпл.Add(pay_doc);
                            pay_doc.НомДокПдтвУпл = oper.ПДНомер;
                            pay_doc.ДатаДокПдтвУпл = oper.ПДДата;
//                            дата_учета = oper.ПДДата;
                        }
                    }
                    else {
                        if (oper.СуммаСтоимость > 0) {
                            if (дата_учета < oper.ДатаБУ)
                                дата_учета = oper.ДатаБУ;
                        }
                    }
                    if (oper.СуммаНДСВычет != 0) {
                        doc_line.КодВидОпер = oper.ОфицВидОперации;
                        if (oper.Ставка != СтавкаНДС.НЕОБЛ) {
                            doc_line.СумНДСВыч += oper.СуммаНДСВычет;
                        }
                    }
                    if (дата_учета < oper.ПДДата)
                        дата_учета = oper.ПДДата;
                }
                if (doc_line.Основание.Тип != Основание.ТипОснования.СФА) {
                    if (дата_учета > ValidationMethods._DATE_NULL)
                        doc_line.ДатаУчТов = дата_учета;
                    else
                        doc_line.ДатаУчТов = doc_line.ДатаСчФПрод;
                }
                if (doc_line.СумНДСВыч == 0 ) {
                    os.Delete(doc_line);
                    cur_num--;
                }
            }
        }

        [Action]
        public void Перенумеровать() {
            IObjectSpace os_cur = CommonMethods.FindObjectSpaceByObject(this);
            using (IObjectSpace os = os_cur.CreateNestedObjectSpace()) {
                КнигаПокуп book = os.GetObject<КнигаПокуп>(this);
                Перенумеровать(os, book);
                os.CommitChanges();
            }
        }

        public static void Перенумеровать(IObjectSpace os, КнигаПокуп book) {
            Int32 cur_num = 1;
            foreach (var line in book.КнПокСтр.OrderBy(x => x.ДатаСчФПрод)) {
                line.НомерПор = cur_num++;
            }
        }

        [Action]
        public void БСО() {
            foreach (var line in КнПокСтр) {
                if (line.Основание.Тип == Основание.ТипОснования.БЖД || 
                    line.Основание.Тип == Основание.ТипОснования.СЧГ)
                    line.КодВидОпер = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "23"));
            }
        }

        [Action]
        public void СФАПолуч() {
            foreach (var line in КнПокСтр) {
                if (line.Основание.Тип == Основание.ТипОснования.СФА &&
                    line.Основание.Источник == Основание.ТипИсточника.ИСХОДЯЩИЙ)
                    line.КодВидОпер = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "22"));
            }
        }

        [Action]
        public void ФизЛица26() {
            foreach (var line in КнПокСтр) {
                if (line.Основание.ЛицоТип == ЛицоТип.ФИЗ_ЛИЦО ||
                    line.Основание.ЛицоТип == ЛицоТип.РОЗНИЦА) {
                    line.КодВидОпер = Session.FindObject<ВидОперации>(new BinaryOperator("Код", "26"));
                }
            }
        }

        [Action]
        public void СФПродИНН() {
            foreach (var line in КнПокСтр) {
                if (line.Основание.Источник == Основание.ТипИсточника.ИСХОДЯЩИЙ) {
                    line.СвПродИНН = line.Основание.Налогоплательщик.ИНН;
                    line.СвПродКПП = line.Основание.Налогоплательщик.КПП;
                }
            }
        }
    }
}