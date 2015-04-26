using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.AVT;
using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public class ОперацияКонтИмпортСтруктур : ОперацияКонтейнер { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        private const String _ИНН_ЮЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}";
        private const String _ИНН_ФЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{10}";
        private const String _КПП_Рек = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-9A-Z]{2})([0-9]{3})";

        [Appearance(null, AppearanceItemType.ViewItem, null, TargetItems = "*;ТипЛица;ИНН;КПП")]
        [Persistent("FmTaxRuVatОперКонтОснов")]
        public class Документ : BaseEntity {

            private ОперацияКонтИмпортСтруктур _Контейнер;
            [Association("FmTaxRuVatКонтИмпортСтруктур-Документы")]
            public ОперацияКонтИмпортСтруктур Контейнер {
                get { return _Контейнер; }
                set {
                    if (!IsLoading) OnChanging("Контейнер", value);
                    SetPropertyValue<ОперацияКонтИмпортСтруктур>("Контейнер", ref _Контейнер, value);
                }
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
            [RuleRegularExpression("FmTaxRuV504СвУчСд3_ИНН1", "ПРОВЕРКА", _ИНН_ЮЛ_Рег, TargetCriteria = "ТипЛица == 'ТИП_ЮЛ'", CustomMessageTemplate = "Некорректный ИНН")]
            [RuleRegularExpression("FmTaxRuV504СвУчСд3_ИНН2", "ПРОВЕРКА", _ИНН_ФЛ_Рег, TargetCriteria = "ТипЛица == 'ТИП_ИП'", CustomMessageTemplate = "Некорректный ИНН")]
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
            private UInt16 _НомерИсправления;
            public UInt16 НомерИсправления {
                get { return _НомерИсправления; }
                set {
                    if (!IsLoading) OnChanging("НомерИсправления", value);
                    SetPropertyValue<UInt16>("НомерИсправления", ref _НомерИсправления, value);
                }
            }

            private DateTime _ДатаИсправления;
            public DateTime ДатаИсправления {
                get { return _ДатаИсправления; }
                set {
                    if (!IsLoading) OnChanging("ДатаИсправления", value);
                    SetPropertyValue<DateTime>("ДатаИсправления", ref _ДатаИсправления, value);
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

            //[Persistent("СчетФактура")]
            //private fmCAVTInvoiceBase _СчетФактура;
            //[PersistentAlias("_СчетФактура")]
            //public fmCAVTInvoiceBase СчетФактура {
            //    get { return _СчетФактура; }
            //}
            //public void СчетФактураУст(fmCAVTInvoiceBase value) {
            //    SetPropertyValue<fmCAVTInvoiceBase>("СчетФактура", ref _СчетФактура, value);
            //}


            //private fmCAVTInvoiceBase _СчетФактура;
            //public fmCAVTInvoiceBase СчетФактура {
            //    get { return _СчетФактура; }
            //    set { SetPropertyValue<fmCAVTInvoiceBase>("СчетФактура", ref _СчетФактура, value); }
            //}

            //private Основание _Основание;
            //[RuleRequiredField]
            //public Основание Основание {
            //    get { return _Основание; }
            //    set {
            //        if (!IsLoading) OnChanging("Основание", value);
            //        SetPropertyValue<Основание>("Основание", ref _Основание, value);
            //    }
            //}

            //private ОснованиеДокумент _ОснованиеДокумент;
            //[DataSourceProperty("Основание.Документы")]
            //[RuleRequiredField]
            //public ОснованиеДокумент ОснованиеДокумент {
            //    get { return _ОснованиеДокумент; }
            //    set {
            //        if (!IsLoading) OnChanging("ОснованиеДокумент", value);
            //        SetPropertyValue<ОснованиеДокумент>("ОснованиеДокумент", ref _ОснованиеДокумент, value);
            //    }
            //}

            public Документ(Session session)
                : base(session) {
            }
            public override void AfterConstruction() {
                base.AfterConstruction();
                ТипОснования = RuVat.Основание.ТипОснования.Неопределен;
            }

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
                            ИННПродавцаУст(Контейнер.Налогоплательщик.ИНН);
//                        ОбновитьСчетФактуру();
                        break;
                    case "Номер":
                    case "Дата":
//                        ОбновитьСчетФактуру();
                        break;
                    case "Контрагент":
                        ОбновитьКонтрагента();
                        break;
                    //case "СчетФактура":
                    //    if (СчетФактура != null) {
                    //        if (СчетФактура.Supplier.INN == Контейнер.Налогоплательщик.ИНН) {
                    //            Контрагент = СчетФактура.Customer;
                    //            ТипИсточника = RuVat.Основание.ТипИсточника.ИСХОДЯЩИЙ;
                    //        } else {
                    //            Контрагент = СчетФактура.Supplier;
                    //            ТипИсточника = RuVat.Основание.ТипИсточника.ВХОДЯЩИЙ;
                    //        }
                    //        Номер = СчетФактура.Number;
                    //        Дата = СчетФактура.Date;
                    //        if (ТипИсточника == RuVat.Основание.ТипИсточника.ИСХОДЯЩИЙ) { 
                    //            String dates = СчетФактура.Date.ToString("yyyyMMdd");
                    //            РегНомер = СчетФактура.RegNumber[0] + dates.Substring(2,2) + 
                    //                //+ dates[2] + dates[3] +
                    //                СчетФактура.RegNumber.Substring(1);
                    //        }
                    //        else
                    //            РегНомер = СчетФактура.RegNumber;
                    //        ТипОснования = Основание.ТипОснования.Неопределен;
                    //    }
                    //    break;
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

            //protected void ОбновитьСчетФактуру() {
            //    if (ТипИсточника == Основание.ТипИсточника.ВХОДЯЩИЙ || String.IsNullOrEmpty(Номер) || Дата < new DateTime(2000, 1, 1) ||
            //        СчетФактура != null && СчетФактура.Number == Номер &&
            //        СчетФактура.Date >= Дата && СчетФактура.Date < Дата.AddDays(1))
            //        return;
            //    СчетФактураУст(Session.FindObject<fmCAVTInvoiceBase>(
            //                            XPQuery<fmCAVTInvoiceBase>.TransformExpression(Session,
            //        rec =>
            //            rec.Supplier.INN == ИННПродавца &&
            //            rec.Number == Номер &&
            //            rec.Date >= Дата &&
            //            rec.Date < Дата.AddDays(1)
            //        ))
            //    );
            //}

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
            public void ЗаполнитьКонтр() {
                IObjectSpace osbase = CommonMethods.FindObjectSpaceByObject(this);
                using (IObjectSpace os = osbase.CreateNestedObjectSpace()) {
                    Документ строка = os.GetObject<Документ>(this);
                    строка.Контейнер.ЗаполнитьКонтрагента(строка);
                    os.CommitChanges();
                }
            }
        }

        private DateTime _ДатаБУ;
        [RuleRequiredField]
        public DateTime ДатаБУ {
            get { return _ДатаБУ; }
            set {
                if (!IsLoading) OnChanging("ДатаБУ", value);
                SetPropertyValue<DateTime>("ДатаБУ", ref _ДатаБУ, value);
            }
        }

        private fmCAVTBookBuhStruct _ДанныеСтруктур;
        public fmCAVTBookBuhStruct ДанныеСтруктур {
            get { return _ДанныеСтруктур; }
            set {
                if (!IsLoading) OnChanging("ДанныеСтруктур", value);
                SetPropertyValue<fmCAVTBookBuhStruct>("ДанныеСтруктур", ref _ДанныеСтруктур, value);
            }
        }

        private Decimal _ДоляСтоимость;
        [Custom("DisplayFormat", "### ### ### ##0.000000")]
        [Custom("EditMask", "### ### ### ##0.000000")]
        public Decimal ДоляСтоимость {
            get { return _ДоляСтоимость; }
            set {
                if (!IsLoading) OnChanging("ДоляСтоимость", value);
                SetPropertyValue<Decimal>("ДоляСтоимость", ref _ДоляСтоимость, value);
            }
        }

        [Association("FmTaxRuVatКонтИмпортСтруктур-Документы"), DevExpress.Xpo.Aggregated]
        public XPCollection<Документ> Документы {
            get { return GetCollection<Документ>("Документы"); }
        }

        public ОперацияКонтИмпортСтруктур(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "ДатаБУ":
                       if (ДатаБУ < new DateTime(2015, 1, 1))
                            break;
                        ПериодБУ = Session.FindObject<ПериодБУ>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            new BinaryOperator("ДатаС", ДатаБУ, BinaryOperatorType.LessOrEqual) &
                            new BinaryOperator("ДатаПо", ДатаБУ, BinaryOperatorType.GreaterOrEqual));
                        if (ПериодБУ == null) {
                            ПериодБУ = new ПериодБУ(Session);
                            ПериодБУ.Налогоплательщик = Налогоплательщик;
                            ПериодБУ.ДатаПериода = ДатаБУ;
                        }
                     break;
            }
        }


        protected void ЗаполнитьКонтрагента(Документ строка_образец) {
            if (строка_образец.Контрагент != null) {
                foreach (Документ строка in Документы) {
                    if (строка_образец.Контрагент == строка.Контрагент) {
                        строка.ТипЛица = строка_образец.ТипЛица;
                        строка.ИНН = строка_образец.ИНН;
                        строка.КПП = строка_образец.КПП;
                    }
                }
            }
        }

        [Action]
        public void Обновить() {
            IObjectSpace os_cur = CommonMethods.FindObjectSpaceByObject(this);
            using (IObjectSpace os = os_cur.CreateNestedObjectSpace()) {
                ОперацияКонтИмпортСтруктур конт = os.GetObject<ОперацияКонтИмпортСтруктур>(this);
                if (конт.ДанныеСтруктур == null)
                    return;
                Обновить(os, конт);
                //                Перенумеровать(os, book);
                os.CommitChanges();
            }
        }

        public static void Обновить(IObjectSpace os, ОперацияКонтИмпортСтруктур конт) {
            os.Delete(конт.Операции);
            foreach (var запись in конт.ДанныеСтруктур.InInvoiceRecords) {
                Обновить(os, конт, Основание.ТипИсточника.ВХОДЯЩИЙ, запись);
            }
            foreach (var запись in конт.ДанныеСтруктур.OutInvoiceRecords) {
                Обновить(os, конт, Основание.ТипИсточника.ИСХОДЯЩИЙ, запись);
            }
            //        public static void Обновить(IObjectSpace os, XPCollection<fmCAVTBookBuhStructRecord> записи) { 
        }
        public static void Обновить(IObjectSpace os, ОперацияКонтИмпортСтруктур конт, Основание.ТипИсточника источник, fmCAVTBookBuhStructRecord запись) {
            Документ cur_doc = null;
            UInt16 change_number = 0;
                if (!String.IsNullOrEmpty(запись.InvoiceChangeNumber))
                    UInt16.TryParse(запись.InvoiceChangeNumber, out change_number);
            foreach (var doc in конт.Документы) {
                if (doc.ТипИсточника == источник && 
                    doc.Контрагент == запись.PartnerParty &&
                    doc.Номер == запись.InvoiceNumber && doc.Дата == запись.InvoiceDate &&
                    doc.НомерИсправления == change_number && doc.ДатаИсправления == запись.InvoiceChangeDate) {
                    cur_doc = doc;
                    break;
                }
            }
            if (cur_doc == null) {
                cur_doc = os.CreateObject<Документ>();
                конт.Документы.Add(cur_doc);
                cur_doc.ТипИсточника = источник;
                cur_doc.Контрагент = запись.PartnerParty;
                cur_doc.Номер = запись.InvoiceNumber;
                cur_doc.Дата = запись.InvoiceDate;
                cur_doc.НомерИсправления = change_number;
                cur_doc.ДатаИсправления = запись.InvoiceChangeDate; //                cur_doc.СчетФактураУст(запись.Invoice);
                cur_doc.ТипОснования = Основание.String2ТипОснования(запись.InvoiceType);
                if (источник == Основание.ТипИсточника.ВХОДЯЩИЙ || cur_doc.ТипОснования == Основание.ТипОснования.ГТД ||
                    cur_doc.ТипОснования == Основание.ТипОснования.СФЗ ) {
                    cur_doc.РегНомер = запись.InvoiceRegNumber;
                }
                else {
                    String dates = запись.InvoiceDate.ToString("yyyyMMdd");
                    // !!!!!!!!!!!!!!!!
                    // Ошибочные номера счетов-фактур выставленных
                    if (запись.InvoiceNumber[0] == '0')
                        cur_doc.РегНомер = запись.InvoiceRegNumber[0] + dates.Substring(2, 2) + запись.InvoiceRegNumber.Substring(1);
                    else
                        cur_doc.РегНомер = запись.InvoiceNumber[0] + dates.Substring(2, 2) + запись.InvoiceNumber.Substring(1);
//                    cur_doc.РегНомер = запись.InvoiceRegNumber;
                }
            }
            ОбновитьДокумент(os, cur_doc, источник, запись);
            if (источник == Основание.ТипИсточника.ИСХОДЯЩИЙ)
                ОбновитьОперациюПродаж(os, конт, cur_doc, источник, запись);
            if (источник == Основание.ТипИсточника.ВХОДЯЩИЙ)
                ОбновитьОперациюПокупок(os, конт, cur_doc, источник, запись);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="строка"></param>
        /// <param name="invoice"></param>
        ///// <param name="sf_sfz_type"></param>
        //static public void ProcessLine(IObjectSpace os, Документ строка, fmCAVTInvoiceType sf_sfz_type) {
        static public void ОбновитьДокумент(IObjectSpace os, Документ строка, Основание.ТипИсточника источник, fmCAVTBookBuhStructRecord запись) {

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
                new BinaryOperator("Дата", строка.Дата.AddDays(1), BinaryOperatorType.Less), true);
            if (sf == null) {
                sf = os.CreateObject<Основание>();
                sf.Налогоплательщик = строка.Контейнер.Налогоплательщик;
                sf.Подразделение = строка.Контейнер.Подразделение;
                sf.Источник = строка.ТипИсточника;
                sf.ИНН = строка.ИНН;
                sf.Номер = строка.Номер;
                sf.Дата = строка.Дата;
                sf.КПП = строка.КПП;
//                sf.Налогоплательщик = строка.Контейнер.Налогоплательщик;
//                sf.Подразделение = строка.Контейнер.Подразделение;
            }
            строка.Основание = sf;
            sf.Корректировка = Основание.ТипПодчиненности.ОСНОВНОЙ;
            //                sf.Источник = ts;
            sf.Тип = строка.ТипОснования;
            sf.ЛицоТип = строка.ТипЛица;
            ОснованиеДокумент sfdoc = null;
            //            String sfdoc_sver = invoice.SF_PRAV_NUMBER.Trim();
            DateTime sfdoc_date;
            if (строка.НомерИсправления != 0) {
                sfdoc_date = строка.ДатаИсправления;
            }
            else {
                sfdoc_date = строка.Дата;
            }
            //            DateTime sfdoc_date = default(DateTime);
            //            DateTime.TryParseExact(invoice.SF_DATE.Trim(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out sfdoc_date);
            //Decimal summ_cost = Decimal.Parse(invoice.SUMM_COST.Trim().Replace('.', ','));
            //Decimal summ_nds = Decimal.Parse(invoice.SUMM_NDS.Trim().Replace('.', ','));
            //Decimal summ_sub_cost = Decimal.Parse(invoice.SUMM_SUB_COST.Trim().Replace('.', ','));
            //Decimal summ_sub_nds = Decimal.Parse(invoice.SUMM_SUB_NDS.Trim().Replace('.', ','));
            ////Decimal summ_cost = строка.СчетФактура.SummCost;
            ////Decimal summ_nds = строка.СчетФактура.SummAVT;
            ////Decimal summ_sub_cost = строка.СчетФактура.DeltaSummAllSub;
            ////Decimal summ_sub_nds = строка.СчетФактура.DeltaSummAVTSub;
            //if (sf.Источник == Основание.ТипИсточника.ИСХОДЯЩИЙ &&
            //    sf.Тип != Основание.ТипОснования.Неопределен &&
            //    sf.Тип != Основание.ТипОснования.СЧГ &&
            //    sf.Тип != Основание.ТипОснования.БЖД &&
            //    sf.Тип != Основание.ТипОснования.СФЗ) {
            //    if (строка.СчетФактура != null) {
            //        if (sfdoc_sver == "0" && !String.IsNullOrEmpty(строка.СчетФактура.Current.VersionNumber)) {
            //            sfdoc_sver = строка.СчетФактура.Current.VersionNumber;
            //            UInt16.TryParse(sfdoc_sver, out sfdoc_ver);
            //            sfdoc_date = строка.СчетФактура.Current.VersionDate;
            //        }
            //        if (summ_cost == 0 && summ_nds == 0 && summ_sub_cost == 0 && summ_sub_nds == 0) {
            //            summ_cost = строка.СчетФактура.SummAll - строка.СчетФактура.SummAVT;
            //            summ_nds = строка.СчетФактура.SummAVT;
            //        }
            //    }
            //}
            foreach (ОснованиеДокумент doc in sf.Документы) {
                if (doc.НомерИсправления == строка.НомерИсправления) {
                    sfdoc = doc;
                    break;
                }
            }
            if (sfdoc == null) {
                sfdoc = os.CreateObject<ОснованиеДокумент>();
                sf.Документы.Add(sfdoc);
                sfdoc.НомерИсправления = строка.НомерИсправления;
                if (sf.ДействующийДокумент.НомерИсправления < sfdoc.НомерИсправления) {
                    sf.ДействующийДокумент = sfdoc;
                }
            }
            строка.ОснованиеДокумент = sfdoc;
            sfdoc.ДатаИсправления = sfdoc_date;
            sfdoc.РегНомер = строка.РегНомер;
            //            if (sf.Тип == Основание.ТипОснования.СФЗ && String.IsNullOrEmpty(sfdoc.РегНомер)) {
            //                Int32 IntNumber = fmCAVTInvoiceNumberGenerator.GenerateNumber(((ObjectSpace)os).Session, sf.ДействующийДокумент.CID, sf_sfz_type, sf.Дата, 0);
            //                sfdoc.РегНомер = sf_sfz_type.Prefix + sf.Дата.ToString("yyyyMM").Substring(2, 4) + IntNumber.ToString("00000");
            //                строка.РегНомер = sfdoc.РегНомер;
            //            }
            sfdoc.КодПартнера = строка.Контрагент.Code;
            sfdoc.НаименКонтрагента = строка.Контрагент.Name;
            sfdoc.СуммаВсего = запись.SummAll;
            sfdoc.СуммаНДС = запись.SummVAT;
            sfdoc.СуммаВсегоУвел = запись.SummIncCost + запись.SummIncVAT;
            sfdoc.СуммаНДСУвел = запись.SummIncVAT;
//            sfdoc.СуммаВсегоУвел = sfdoc.СуммаВсего + summ_sub_cost;
//            sfdoc.СуммаНДСУвел = sfdoc.СуммаНДС + summ_sub_nds;
        }

        public static void ОбновитьОперациюПродаж(IObjectSpace os, ОперацияКонтИмпортСтруктур конт, Документ док, Основание.ТипИсточника источник, fmCAVTBookBuhStructRecord запись) {
            if (док.Основание == null || запись.SummAll == 0)
                return;
            Операция опер = os.CreateObject<Операция>();
            конт.Операции.Add(опер);
            опер.ОснованиеДокумент = док.ОснованиеДокумент;
            опер.ТипКниги = Операция.ТипКнигиТип.ПРОДАЖ;
            опер.ТипДеятельности = Операция.ТипДеятельностиТип.ОБЛ_18;
            опер.ТипОбъекта = Операция.ТипОбъектаТип.РЕАЛИЗАЦИЯ;
            опер.ТипНапрОпер = Операция.ТипНапрОперТип.НОРМАЛЬНЫЙ;
            if (запись.OperationType.Code == "01" || запись.OperationType.Code == "1")
                опер.ТипОперВнутр = Операция.ТипОперВнутрТип.РЕАЛИЗАЦИЯ;
            if (запись.OperationType.Code == "02" || запись.OperationType.Code == "2")
                опер.ТипОперВнутр = Операция.ТипОперВнутрТип.АВАНС;
            опер.ТипОсновной = Операция.ТипОсновнойТип.НАЛ_БАЗА;
            опер.ОснованиеРегНомер = док.РегНомер;
            опер.СФТип = запись.InvoiceType;
            опер.СФНомер = запись.InvoiceNumber;
            опер.СФДата = запись.InvoiceDate;
            CS.Finance.csNDSRate rate = запись.SaleVATRate;
            if (rate == null)
                rate = запись.BayVATRate;
            if (rate != null) {
                if (rate.Code == "18%")
                    опер.Ставка = СтавкаНДС.ОБЛ_18;
                if (rate.Code == "10%")
                    опер.Ставка = СтавкаНДС.ОБЛ_10;
                if (rate.Code == "0%")
                    опер.Ставка = СтавкаНДС.ОБЛ_0;
                if (rate.Code == "БЕЗ НДС")
                    опер.Ставка = СтавкаНДС.НЕОБЛ;
            }
            else
                опер.Ставка = СтавкаНДС.ОБЛ_18;

            if (запись.SaleDate > new DateTime(2000, 1, 1))
                опер.ДатаНДС = запись.SaleDate;
            else
                опер.ДатаНДС = запись.InvoiceDate;
            опер.ПериодБУ = конт.ПериодБУ;
            опер.ДатаБУ = конт.ДатаБУ;
            if (запись.SaleSummAll != 0)
                опер.СуммаВсего = запись.SaleSummAll;
            else
                опер.СуммаВсего = запись.SummAll;
            if (запись.SaleSummVAT != 0)
                опер.СуммаНДСБаза = запись.SaleSummVAT;
            else
                опер.СуммаНДСБаза = запись.SummVAT;
            опер.СуммаСтоимость = опер.СуммаВсего - опер.СуммаНДСБаза;
        }
        public static void ОбновитьОперациюПокупок(IObjectSpace os, ОперацияКонтИмпортСтруктур конт, Документ док, Основание.ТипИсточника источник, fmCAVTBookBuhStructRecord запись) {
            if (док.Основание == null || запись.SummAll == 0)
                return;
            Операция опер = os.CreateObject<Операция>();
            конт.Операции.Add(опер);
            опер.ОснованиеДокумент = док.ОснованиеДокумент;
            опер.ТипКниги = Операция.ТипКнигиТип.ПОКУПОК;
            опер.ТипДеятельности = Операция.ТипДеятельностиТип.ОБЛ_18;
            опер.ТипОбъекта = Операция.ТипОбъектаТип.РЕАЛИЗАЦИЯ;
            опер.ТипНапрОпер = Операция.ТипНапрОперТип.НОРМАЛЬНЫЙ;
            if (запись.OperationType == null)
                throw new ArgumentNullException("Незадан Тип операции для " + опер.ОснованиеДокумент);
            if (запись.OperationType.Code == "01" || запись.OperationType.Code == "1")
                опер.ТипОперВнутр = Операция.ТипОперВнутрТип.РЕАЛИЗАЦИЯ;
            if (запись.OperationType.Code == "02" || запись.OperationType.Code == "2")
                опер.ТипОперВнутр = Операция.ТипОперВнутрТип.АВАНС_ЗАЧЕТ;
            опер.ТипОсновной = Операция.ТипОсновнойТип.ВЫЧЕТ;
            опер.ОснованиеРегНомер = док.РегНомер;
            опер.СФТип = запись.InvoiceType;
            опер.СФНомер = запись.InvoiceNumber;
            опер.СФДата = запись.InvoiceDate;
            CS.Finance.csNDSRate rate = запись.BayVATRate;
            if (rate == null)
                rate = запись.SaleVATRate;
            if (rate != null) {
                if (rate.Code == "18%")
                    опер.Ставка = СтавкаНДС.ОБЛ_18;
                if (rate.Code == "10%")
                    опер.Ставка = СтавкаНДС.ОБЛ_10;
                if (rate.Code == "0%")
                    опер.Ставка = СтавкаНДС.ОБЛ_0;
                if (rate.Code == "БЕЗ НДС")
                    опер.Ставка = СтавкаНДС.НЕОБЛ;
            }
            else
                опер.Ставка = СтавкаНДС.ОБЛ_18;
            опер.ПериодБУ = конт.ПериодБУ;
            опер.ДатаБУ = конт.ДатаБУ;
            опер.ДатаНДС = запись.BayDate;
            if (запись.BayDate > new DateTime(2000, 1, 1))
                опер.ДатаНДС = запись.BayDate;
            else
                if (запись.BaySummVAT != 0)
                    throw new ArgumentOutOfRangeException("Незадана дата отнесения для покупок ");
//            опер.ДатаНДС = опер.ДатаБУ;
            if (запись.SaleSummAll != 0)
                опер.СуммаВсего = запись.BaySummAll;
            else
                опер.СуммаВсего = запись.SummAll;
            if (запись.SaleSummVAT != 0)
                опер.СуммаНДСВычет = запись.BaySummVAT;
            else
                опер.СуммаНДСВычет = запись.SummVAT;
            опер.СуммаСтоимость = опер.СуммаВсего - опер.СуммаНДСВычет;
            if (опер.ТипОперВнутр == Операция.ТипОперВнутрТип.РЕАЛИЗАЦИЯ) {
                опер.СуммаНДС19Входящий = опер.СуммаНДСВычет;
                опер.СуммаНДС19Списано = опер.СуммаНДСВычет;
            }
            if (док.Контейнер != null && док.Контейнер.ДоляСтоимость > 0) {
                опер.СуммаНДССтоимость = Decimal.Round(опер.СуммаНДСВычет * док.Контейнер.ДоляСтоимость, 2);
                опер.СуммаНДСВычет = опер.СуммаНДСВычет - опер.СуммаНДССтоимость;
            }
        }

    }
}
