using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using FileHelpers;
using FileHelpers.DataLink;
//
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.AVT;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {

    [NavigationItem("Налоги")]
    [Persistent("FmTaxRuVatДокИмпОсн")]
    public class ДокИмпортОснований : BaseEntity {
        [FixedLengthRecord]
        public class InvoiceImport {
            [FieldFixedLength(1)]
            public String SF_IO_TYPE;
            [FieldFixedLength(3)]
            public String SF_TYPE;
            [FieldFixedLength(3)]
            public String SF_TYPE_ORIG;
            [FieldFixedLength(16)]
            public String SF_INT_NUMBER;
            [FieldFixedLength(9)]
            public String SF_CREATE_DATE;
            [FieldFixedLength(9)]
            public String SF_CHANGE_DATE;
            [FieldFixedLength(2)]
            public String SF_OPER_TYPE;
            [FieldFixedLength(2)]
            public String SF_TRANS_TYPE;
            [FieldFixedLength(9)]
            public String SF_TRANS_DATE;
            [FieldFixedLength(6)]
            public String SF_VO_CODE;
            [FieldFixedLength(7)]
            public String SF_PERIOD;
            [FieldFixedLength(20)]
            public String SF_REGNUM;
            [FieldFixedLength(250)]
            public String SF_NUMBER;
            [FieldFixedLength(8)]
            public String SF_DATE;
            [FieldFixedLength(20)]
            public String SF_PRAV_NUMBER;
            [FieldFixedLength(9)]
            public String SF_PRAV_DATE;
            [FieldFixedLength(3)]
            public String SF_VA_CODE;
            [FieldFixedLength(1)]
            public String IS_COR;
            [FieldFixedLength(6)]
            public String COR_VO_CODE;
            [FieldFixedLength(20)]
            public String COR_SF_NUMBER;
            [FieldFixedLength(9)]
            public String COR_SF_DATE;
            [FieldFixedLength(20)]
            public String COR_SF_PRAV_NUMBER;
            [FieldFixedLength(9)]
            public String COR_SF_PRAV_DATE;
            [FieldFixedLength(17)]
            public String SUMM_COST;
            [FieldFixedLength(17)]
            public String SUMM_NDS;
            [FieldFixedLength(17)]
            public String SUMM_ALL;
            [FieldFixedLength(17)]
            public String SUMM_ADD_COST;
            [FieldFixedLength(17)]
            public String SUMM_ADD_NDS;
            [FieldFixedLength(17)]
            public String SUMM_ADD_ALL;
            [FieldFixedLength(17)]
            public String SUMM_SUB_COST;
            [FieldFixedLength(17)]
            public String SUMM_SUB_NDS;
            [FieldFixedLength(17)]
            public String SUMM_SUB_ALL;
        }

        [Persistent("FmTaxRuVatДокИмпОснСтр")]
        public class СтрокаОснов : BaseEntity {
            private const String _ИНН_ЮЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}";
            private const String _ИНН_ФЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{10}";
            private const String _КПП_Рек = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-9A-Z]{2})([0-9]{3})";

            private ДокИмпортОснований _ИмпортОснований;
            [Association("FmTaxRuVatДокИмпОснов-СтрокаОснов")]
            public ДокИмпортОснований ИмпортОснований {
                get { return _ИмпортОснований; }
                set {
                    if (!IsLoading) OnChanging("ИмпортОснований", value);
                    SetPropertyValue<ДокИмпортОснований>("ИмпортОснований", ref _ИмпортОснований, value);
                }
            }

            [Size(1024)]
            [Persistent("Буфер")]
            private String _Буфер;
            [Size(1024)]
            [PersistentAlias("_Буфер")]
            public String Буфер {
                get { return _Буфер; }
            }
            public void БуферУст(String value) {
                SetPropertyValue<String>("Буфер", ref _Буфер, value);
            }

            private Основание.ТипИсточника _ТипИсточника;
            public Основание.ТипИсточника ТипИсточника {
                get { return _ТипИсточника; }
                set {
                    if (!IsLoading) OnChanging("ТипИсточника", value);
                    SetPropertyValue<Основание.ТипИсточника>("ТипИсточника", ref _ТипИсточника, value);
                }
            }

            private Основание.ТипОснования _ТипОснования;
            public Основание.ТипОснования ТипОснования {
                get { return _ТипОснования; }
                set {
                    if (!IsLoading) OnChanging("ТипОснования", value);
                    SetPropertyValue<Основание.ТипОснования>("ТипОснования", ref _ТипОснования, value);
                }
            }

            private crmCParty _Контрагент;
            public crmCParty Контрагент {
                get { return _Контрагент; }
                set {
                    if (!IsLoading) OnChanging("Контрагент", value);
                    SetPropertyValue<crmCParty>("Контрагент", ref _Контрагент, value);
                }
            }
            private ЛицоТип _ТипЛица;
            public ЛицоТип ТипЛица {
                get { return _ТипЛица; }
                set {
                    if (!IsLoading) OnChanging("ТипЛица", value);
                    SetPropertyValue<ЛицоТип>("ТипЛица", ref _ТипЛица, value);
                }
            }
            private String _ИНН;
            [RuleRequiredField(null, "ПРОВЕРКА", TargetCriteria = "ТипЛица == 'ТИП_ЮЛ' || ТипЛица == 'ТИП_ИП'")]
            [RuleRegularExpression("FmTaxRuV504СвУчСд2_ИНН1", "ПРОВЕРКА", _ИНН_ЮЛ_Рег, TargetCriteria = "ТипЛица == 'ТИП_ЮЛ'", CustomMessageTemplate = "Некорректный ИНН")]
            [RuleRegularExpression("FmTaxRuV504СвУчСд2_ИНН2", "ПРОВЕРКА", _ИНН_ФЛ_Рег, TargetCriteria = "ТипЛица == 'ТИП_ИП'", CustomMessageTemplate = "Некорректный ИНН")]
            [Size(20)]
            public String ИНН {
                get { return _ИНН; }
                set {
                    if (!IsLoading) OnChanging("ИНН", value);
                    SetPropertyValue<String>("ИНН", ref _ИНН, value);
                }
            }
            private String _КПП;
            [RuleRequiredField(null, "ПРОВЕРКА", TargetCriteria = "ТипЛица == 'ТИП_ЮЛ'")]
            [RuleRegularExpression(null, "ПРОВЕРКА", _КПП_Рек, CustomMessageTemplate = "Некорректный КПП")]
            [Size(20)]
            public String КПП {
                get { return _КПП; }
                set {
                    if (!IsLoading) OnChanging("КПП", value);
                    SetPropertyValue<String>("КПП", ref _КПП, value);
                }
            }

            [Persistent("ИННПродавца")]
            [Size(20)]
            private String _ИННПродавца;
            [Size(20)]
            [PersistentAlias("_ИННПродавца")]
            public String ИННПродавца {
                get { return _ИННПродавца; }
            }
            public void ИННПродавцаУст(String value) {
                SetPropertyValue<String>("ИННПродавца", ref _ИННПродавца, value);
            }

            private String _РегНомер;
            [Size(20)]
            public String РегНомер {
                get { return _РегНомер; }
                set {
                    if (!IsLoading) OnChanging("РегНомер", value);
                    SetPropertyValue<String>("РегНомер", ref _РегНомер, value);
                }
            }
            private String _Номер;
            [Size(256)]
            [VisibleInListView(true)]
            public String Номер {
                get { return _Номер; }
                set {
                    if (!IsLoading) OnChanging("Номер", value);
                    SetPropertyValue<String>("Номер", ref _Номер, value);
                }
            }
            private DateTime _Дата;
            public DateTime Дата {
                get { return _Дата; }
                set {
                    if (!IsLoading) OnChanging("Дата", value);
                    SetPropertyValue<DateTime>("Дата", ref _Дата, value);
                }
            }
            //private DateTime _ДатаПолучения;
            [PersistentAlias("ОснованиеДокумент.ДатаПолучения")]
            public DateTime ДатаПолучения {
                get { return ОснованиеДокумент != null ? ОснованиеДокумент.ДатаПолучения : default(DateTime); }
                set {
                    if (!IsLoading)
                        ОснованиеДокумент.ДатаПолучения = value;
                }
            }
            [PersistentAlias("ОснованиеДокумент.ДатаВыставления")]
            public DateTime ДатаВыставления {
                get { return ОснованиеДокумент != null ? ОснованиеДокумент.ДатаВыставления : default(DateTime); }
                set {
                    if (!IsLoading)
                        ОснованиеДокумент.ДатаВыставления = value;
                }
            }
            [PersistentAlias("ОснованиеДокумент.СуммаВсего")]
            public Decimal СуммаВсего {
                get { return ОснованиеДокумент != null ? ОснованиеДокумент.СуммаВсего : default(Decimal); }
                set {
                    if (!IsLoading)
                        ОснованиеДокумент.СуммаВсего = value;
                }
            }
            [PersistentAlias("ОснованиеДокумент.СуммаНДС")]
            public Decimal СуммаНДС {
                get { return ОснованиеДокумент != null ? ОснованиеДокумент.СуммаНДС : default(Decimal); }
                set {
                    if (!IsLoading)
                        ОснованиеДокумент.СуммаНДС = value;
                }
            }

            private Основание _Основание;
            public Основание Основание {
                get { return _Основание; }
                set {
                    if (!IsLoading) OnChanging("Основание", value);
                    SetPropertyValue<Основание>("Основание", ref _Основание, value);
                }
            }

            private ОснованиеДокумент _ОснованиеДокумент;
            [DataSourceProperty("Основание.Документы")]
            public ОснованиеДокумент ОснованиеДокумент {
                get { return _ОснованиеДокумент; }
                set {
                    if (!IsLoading) OnChanging("ОснованиеДокумент", value);
                    SetPropertyValue<ОснованиеДокумент>("ОснованиеДокумент", ref _ОснованиеДокумент, value);
                }
            }

            [Persistent("СчетФактура")]
            private fmCAVTInvoiceBase _СчетФактура;
            [PersistentAlias("_СчетФактура")]
            public fmCAVTInvoiceBase СчетФактура {
                get { return _СчетФактура; }
            }
            public void СчетФактураУст(fmCAVTInvoiceBase value) {
                SetPropertyValue<fmCAVTInvoiceBase>("СчетФактура", ref _СчетФактура, value);
            }

            public СтрокаОснов(Session session) : base(session) { }
            public override void AfterConstruction() { base.AfterConstruction(); }

            public override void OnChanging(string propertyName, object newValue) {
                base.OnChanging(propertyName, newValue);
                if (IsLoading)
                    return;
            }
            protected override void OnChanged(string propertyName, object oldValue, object newValue) {
                base.OnChanged(propertyName, oldValue, newValue);
                if (IsLoading)
                    return;
                switch (propertyName) {
                    case "ТипИсточника":
                    case "ИНН":
                        if (ТипИсточника == Основание.ТипИсточника.ВХОДЯЩИЙ)
                            ИННПродавцаУст(ИНН);
                        else
                            ИННПродавцаУст(ИмпортОснований.Налогоплательщик.ИНН);
                        ОбновитьСчетФактуру();
                        break;
                    case "Номер":
                    case "Дата":
                        ОбновитьСчетФактуру();
                        break;
                    case "Контрагент":
                        if (Контрагент != null)
                            ОбновитьКонтрагента();
                        break;
                    case "СчетФактура":
                        break;
                    case "Основание":
                        if (Основание != null) {
                            if (ОснованиеДокумент == null || ОснованиеДокумент.Основание != Основание)
                                ОснованиеДокумент = Основание.ДействующийДокумент;
                        }
                        else
                            ОснованиеДокумент = null;
                        break;
                    case "ОснованиеДокумент":
                        if (ОснованиеДокумент != null && ОснованиеДокумент.Основание != Основание)
                            Основание = ОснованиеДокумент.Основание;
                        break;
                }
            }

            protected void ОбновитьСчетФактуру() {
                if (ТипИсточника == Основание.ТипИсточника.ВХОДЯЩИЙ || String.IsNullOrEmpty(Номер) || Дата < new DateTime(2000, 1, 1) ||
                    СчетФактура != null && СчетФактура.Number == Номер &&
                    СчетФактура.Date >= Дата && СчетФактура.Date < Дата.AddDays(1))
                    return;
                СчетФактураУст(Session.FindObject<fmCAVTInvoiceBase>(
                                        XPQuery<fmCAVTInvoiceBase>.TransformExpression(Session,
                    rec =>
                        rec.Supplier.INN == ИННПродавца &&
                        rec.Number == Номер &&
                        rec.Date >= Дата &&
                        rec.Date < Дата.AddDays(1)
                    ))
                );
            }

            protected void ОбновитьКонтрагента() {
                if (Контрагент == null)
                    return;
                ТипЛица = ЛицоТип.НЕЗАДАН;
                ИНН = String.Empty;
                КПП = String.Empty;
                if (Контрагент.Person != null) {
                    if (Контрагент.Person.Address.Country.CodeAlfa2 == "RU") {
                        //                        Type party.ComponentTypeComponentObject.GetType();
                        if (Контрагент.ComponentType == typeof(crmCLegalPerson) ||
                            Контрагент.ComponentType == typeof(crmCLegalPersonUnit)) {
                            ТипЛица = ЛицоТип.ЮР_ЛИЦО;
                            ИНН = Контрагент.INN;
                            if (ИНН.Length == 9)
                                ИНН = "0" + ИНН;
                            КПП = Контрагент.KPP;
                            if (КПП.Length == 8)
                                КПП = "0" + КПП;
                            if (ИНН.Length != 10) {
                                System.Console.WriteLine("Party: " + Контрагент.Code + " fail INN (" + ИНН + ")");
                                //                                    continue;
                            }
                            if (КПП.Length != 9) {
                                System.Console.WriteLine("Party: " + Контрагент.Code + " fail KPP (" + КПП + ")");
                                //                                    continue;
                            }
                        }
                        else {
                            if (Контрагент.ComponentType == typeof(crmCBusinessman)) {
                                ТипЛица = ЛицоТип.ПРЕДПРИНИМАТЕЛЬ;
                                ИНН = Контрагент.INN;
                                if (ИНН.Length == 11)
                                    ИНН = "0" + ИНН;
                                if (ИНН.Length != 12) {
                                    System.Console.WriteLine("Party: " + Контрагент.Code + " fail INN (" + ИНН + ")");
                                    //                                        continue;
                                }
                            }
                            else
                                if (Контрагент.ComponentType == typeof(crmCPhysicalParty)) {
                                    ТипЛица = ЛицоТип.ФИЗ_ЛИЦО;
                                }
                        }
                    }
                    else {
                        ТипЛица = ЛицоТип.ИНО_ПАРТНЕР;
                        System.Console.WriteLine("Party: " + Контрагент.Code + " инопартнер ");
                    }
                }
                if (Контрагент.Code == "2706") {
                    ТипЛица = ЛицоТип.РОЗНИЦА;
                }

            }
            [Action]
            public void Обработать() {
                FixedFileEngine engine = new FixedFileEngine(typeof(InvoiceImport));
                InvoiceImport data = (InvoiceImport)engine.ReadString(Буфер)[0];
                IObjectSpace os = CommonMethods.FindObjectSpaceByObject(this);
                fmCAVTInvoiceType sf_sfz_type = os.GetObjects<fmCAVTInvoiceType>().First(x => x.Prefix == "Z");
                СтрокаОснов.ProcessLine(os, this, data, sf_sfz_type);
            }
            [Action]
            public void ЗаполнитьКонтр() {
                IObjectSpace osbase = CommonMethods.FindObjectSpaceByObject(this);
                using (IObjectSpace os = osbase.CreateNestedObjectSpace()) {
                    СтрокаОснов строка = os.GetObject<СтрокаОснов>(this);
                    строка.ИмпортОснований.ЗаполнитьКонтрагента(строка);
                    os.CommitChanges();
                }
            }

            static public void ImportLine(IObjectSpace os, СтрокаОснов строка, InvoiceImport invoice) {
                //            OpenFileDialog dialog = new OpenFileDialog();
                //            if (dialog.ShowDialog() == DialogResult.OK) {
                //                FixedFileEngine engine = new FixedFileEngine(typeof(InvoiceImport));
                //                InvoiceImport[] imp_res = (InvoiceImport[])engine.ReadFile(file_name);
                //                IList<fmCAVTInvoiceType> inv_types = os.GetObjects<fmCAVTInvoiceType>();
                //                IList<fmCAVTInvoiceTransferType> inv_transfer_types = os.GetObjects<fmCAVTInvoiceTransferType>();
                //                IList<fmCAVTInvoiceOperationType> inv_oper_types = os.GetObjects<fmCAVTInvoiceOperationType>();
                // Int32 count = 0;
                // 
                //                foreach (InvoiceImport imp_rec in imp_res) {
                invoice.SF_VO_CODE = invoice.SF_VO_CODE.Trim();
                invoice.SF_INT_NUMBER = invoice.SF_INT_NUMBER.Trim();
                invoice.SF_NUMBER = invoice.SF_NUMBER.Trim();
                //                    crmCParty party = 
                //                    Основание.ТипИсточника ts;
                if (invoice.SF_IO_TYPE == "I")
                    строка.ТипИсточника = Основание.ТипИсточника.ВХОДЯЩИЙ;
                else if (invoice.SF_IO_TYPE == "O")
                    строка.ТипИсточника = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                else
//                    continue;
                    throw new ArgumentOutOfRangeException("SF " + invoice.SF_NUMBER + " неопределен тип входящий/исходящий");
                строка.РегНомер = invoice.SF_REGNUM.Trim();
                строка.Номер = invoice.SF_NUMBER.Trim();
                DateTime date = default(DateTime);
                DateTime.TryParseExact(invoice.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date);
                строка.Дата = date;
                //                    Основание.ТипОснования tsf;
                строка.ТипОснования = Основание.ТипОснования.Неопределен;
                switch (invoice.SF_TYPE) {
                    case "СЧФ":
                        строка.ТипОснования = Основание.ТипОснования.СЧФ;
                        break;
                    case "УПД":
                        строка.ТипОснования = Основание.ТипОснования.УПД;
                        break;
                    case "СФА":
                        строка.ТипОснования = Основание.ТипОснования.СФА;
                        break;
                    case "СФЗ":
                        строка.ТипОснования = Основание.ТипОснования.СФЗ;
                        break;
                    case "СЧГ":
                        строка.ТипОснования = Основание.ТипОснования.СЧГ;
                        break;
                    case "БЖД":
                        строка.ТипОснования = Основание.ТипОснования.БЖД;
                        break;
                    case "СФВ":
                        строка.ТипОснования = Основание.ТипОснования.СФВ;
                        break;
                    case "БСО":
                        строка.ТипОснования = Основание.ТипОснования.БСО;
                        break;
                    case "ЧЕК":
                        строка.ТипОснования = Основание.ТипОснования.ЧЕК;
                        break;
                    default:
                        System.Console.WriteLine("SF: " + invoice.SF_NUMBER + " странный тип (" + invoice.SF_TYPE + ")");
                        break;
                    //                            continue;
                }
                //                    String inn = "";
                //                    String kpp = "";
                строка.Контрагент = os.GetObjects<crmCParty>(new BinaryOperator("Code", invoice.SF_VO_CODE)).FirstOrDefault();
                if (строка.Контрагент == null) {
                    System.Console.WriteLine("SF " + invoice.SF_NUMBER + " party not found (" + invoice.SF_VO_CODE + ")");
                    return;
                    //continue;
                }
                //                ЛицоТип party_type = ЛицоТип.НЕЗАДАН;
                //
                //                    String sale_inn = "5012039795";
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="os"></param>
            /// <param name="строка"></param>
            /// <param name="invoice"></param>
            /// <param name="sf_sfz_type"></param>
            static public void ProcessLine(IObjectSpace os, СтрокаОснов строка, InvoiceImport invoice, fmCAVTInvoiceType sf_sfz_type) {

                // !!!!!!!!!!!!!!!!!!!!!!!!!!
                // Выбраковываем СФ
                if (строка.ТипОснования == Основание.ТипОснования.Неопределен ||
                    строка.ТипЛица == ЛицоТип.НЕЗАДАН ||
                    String.IsNullOrEmpty(строка.ИННПродавца) ||
                    String.IsNullOrEmpty(строка.РегНомер) && строка.ТипОснования != Основание.ТипОснования.СФЗ ||
                    String.IsNullOrEmpty(строка.Номер) ||
                    строка.Дата < new DateTime(2000, 1, 1) ||
                    строка.ТипЛица == ЛицоТип.ЮР_ЛИЦО &&
                        (строка.ИНН.Length != 10 || !Regex.IsMatch(строка.ИНН, _ИНН_ЮЛ_Рег) ||
                         строка.КПП.Length != 9 || !Regex.IsMatch(строка.КПП, _КПП_Рек)) ||
                    строка.ТипЛица == ЛицоТип.ПРЕДПРИНИМАТЕЛЬ &&
                        (строка.ИНН.Length != 12 || !Regex.IsMatch(строка.ИНН, _ИНН_ФЛ_Рег))
                    )
                    return;
                // !!!!!!!!!!!!!!!!!!!!!!!!!!

                Основание sf = os.FindObject<Основание>(
                    new BinaryOperator("ИннПродавца", строка.ИННПродавца) &
                    new BinaryOperator("Номер", строка.Номер) &
                    new BinaryOperator("Дата", строка.Дата, BinaryOperatorType.GreaterOrEqual) &
                    new BinaryOperator("Дата", строка.Дата.AddDays(1), BinaryOperatorType.Less));
                if (sf == null) {
                    sf = os.CreateObject<Основание>();
                    sf.Источник = строка.ТипИсточника;
                    sf.Налогоплательщик = строка.ИмпортОснований.Налогоплательщик;
                    sf.Подразделение = строка.ИмпортОснований.Подразделение;
                    sf.ИНН = строка.ИНН;
                    sf.Номер = строка.Номер;
                    sf.Дата = строка.Дата;
                    sf.КПП = строка.КПП;
                }
                строка.Основание = sf;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // Временно для обновления имеющихся оснований
                sf.Налогоплательщик = строка.ИмпортОснований.Налогоплательщик;
                sf.Подразделение = строка.ИмпортОснований.Подразделение;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                sf.Корректировка = Основание.ТипПодчиненности.ОСНОВНОЙ;
                //                sf.Источник = ts;
                sf.Тип = строка.ТипОснования;
                sf.ЛицоТип = строка.ТипЛица;
                ОснованиеДокумент sfdoc = null;
                String sfdoc_sver = invoice.SF_PRAV_NUMBER.Trim();
                if (String.IsNullOrEmpty(sfdoc_sver))
                    sfdoc_sver = "0";
                UInt16 sfdoc_ver = 0;
                UInt16.TryParse(sfdoc_sver, out sfdoc_ver);
                DateTime sfdoc_date = default(DateTime);
                DateTime.TryParseExact(invoice.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sfdoc_date);
                Decimal summ_cost = Decimal.Parse(invoice.SUMM_COST.Trim().Replace('.', ','));
                Decimal summ_nds = Decimal.Parse(invoice.SUMM_NDS.Trim().Replace('.', ','));
                Decimal summ_sub_cost = Decimal.Parse(invoice.SUMM_SUB_COST.Trim().Replace('.', ','));
                Decimal summ_sub_nds = Decimal.Parse(invoice.SUMM_SUB_NDS.Trim().Replace('.', ','));
                if (sf.Источник == Основание.ТипИсточника.ИСХОДЯЩИЙ &&
                    sf.Тип != Основание.ТипОснования.Неопределен &&
                    sf.Тип != Основание.ТипОснования.СЧГ &&
                    sf.Тип != Основание.ТипОснования.БЖД &&
                    sf.Тип != Основание.ТипОснования.СФЗ) {
                    if (строка.СчетФактура != null) {
                        if (sfdoc_sver == "0" && !String.IsNullOrEmpty(строка.СчетФактура.Current.VersionNumber)) {
                            sfdoc_sver = строка.СчетФактура.Current.VersionNumber;
                            UInt16.TryParse(sfdoc_sver, out sfdoc_ver);
                            sfdoc_date = строка.СчетФактура.Current.VersionDate;
                        }
                        if (summ_cost == 0 && summ_nds == 0 && summ_sub_cost == 0 && summ_sub_nds == 0) {
                            summ_cost = строка.СчетФактура.SummAll - строка.СчетФактура.SummAVT;
                            summ_nds = строка.СчетФактура.SummAVT;
                        }
                    }
                }
                foreach (ОснованиеДокумент doc in sf.Документы) {
                    if (doc.НомерИсправления == sfdoc_ver) {
                        sfdoc = doc;
                        break;
                    }
                }
                if (sfdoc == null) {
                    sfdoc = os.CreateObject<ОснованиеДокумент>();
                    sf.Документы.Add(sfdoc);
                    sfdoc.НомерИсправления = sfdoc_ver;
                    if (sf.ДействующийДокумент.НомерИсправления < sfdoc.НомерИсправления) {
                        sf.ДействующийДокумент = sfdoc;
                    }
                }
                строка.ОснованиеДокумент = sfdoc;
                sfdoc.ДатаИсправления = sfdoc_date;
                sfdoc.РегНомер = строка.РегНомер;
                if (sf.Тип == Основание.ТипОснования.СФЗ && String.IsNullOrEmpty(sfdoc.РегНомер)) {
                    Int32 IntNumber = fmCAVTInvoiceNumberGenerator.GenerateNumber(((ObjectSpace)os).Session, sf.ДействующийДокумент.CID, sf_sfz_type, sf.Дата, 0);
                    sfdoc.РегНомер = sf_sfz_type.Prefix + sf.Дата.ToString("yyyyMM").Substring(2, 4) + IntNumber.ToString("00000");
                    строка.РегНомер = sfdoc.РегНомер;
                }
                sfdoc.КодПартнера = строка.Контрагент.Code;
                sfdoc.НаименКонтрагента = строка.Контрагент.Name;
                sfdoc.СуммаВсего = summ_cost + summ_nds;
                sfdoc.СуммаНДС = summ_nds;
                sfdoc.СуммаВсегоУвел = sfdoc.СуммаВсего + summ_sub_cost;
                sfdoc.СуммаНДСУвел = sfdoc.СуммаНДС + summ_sub_nds;
            }

        }

        private Налогоплательщик _Налогоплательщик;
        [RuleRequiredField]
        //        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public Налогоплательщик Налогоплательщик {
            get { return _Налогоплательщик; }
            set {
                if (!IsLoading) OnChanging("Налогоплательщик", value);
                SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value);
            }
        }

        private СтруктурноеПодразделение _Подразделение;
        //        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [DataSourceProperty("Налогоплательщик.Подразделения")]
        public СтруктурноеПодразделение Подразделение {
            get { return _Подразделение; }
            set {
                if (!IsLoading) OnChanging("Подразделение", value);
                SetPropertyValue<СтруктурноеПодразделение>("Подразделение", ref _Подразделение, value);
            }
        }

        private String _Код;
        //        [RuleRequiredField(TargetCriteria = "ПериодБУ == null")]
        [RuleRequiredField]
        [Size(32)]
        public String Код {
            get { return _Код; }
            set {
                if (!IsLoading) OnChanging("Код", value);
                SetPropertyValue<String>("Код", ref _Код, value);
            }
        }

        [Association("FmTaxRuVatДокИмпОснов-СтрокаОснов"), DevExpress.Xpo.Aggregated]
        public XPCollection<СтрокаОснов> Основания {
            get { return GetCollection<СтрокаОснов>("Основания"); }
        }
        public ДокИмпортОснований(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
        }

        protected void ЗаполнитьКонтрагента(СтрокаОснов строка_образец) {
            if (строка_образец.Контрагент != null) {
                foreach (СтрокаОснов строка in Основания) {
                    if (строка_образец.Контрагент == строка.Контрагент) {
                        строка.ТипЛица = строка_образец.ТипЛица;
                        строка.ИНН = строка_образец.ИНН;
                        строка.КПП = строка_образец.КПП;
                    }
                }
            }
        }

        static public void ImportInvoices(IObjectSpace os, ДокИмпортОснований док, TextReader reader) {
            FixedFileEngine engine = new FixedFileEngine(typeof(InvoiceImport));
            String line;
            os.Delete(док.Основания);
            fmCAVTInvoiceType sf_sfz_type = os.GetObjects<fmCAVTInvoiceType>().First(x => x.Prefix == "Z");
            while ((line = reader.ReadLine()) != null) {
                InvoiceImport data = (InvoiceImport)engine.ReadString(line)[0];
                if (data.SF_IO_TYPE.Trim() == String.Empty)
                    continue;
                СтрокаОснов строка = os.CreateObject<СтрокаОснов>();
                док.Основания.Add(строка);
                строка.БуферУст(line);
                СтрокаОснов.ImportLine(os, строка, data);
                СтрокаОснов.ProcessLine(os, строка, data, sf_sfz_type);
            }
            reader.Close();
        }

    }
}
