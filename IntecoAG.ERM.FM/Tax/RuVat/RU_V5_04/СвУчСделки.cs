using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.XafExt.DC;
using System.Xml;

namespace IntecoAG.ERM.FM.Tax.RuVat.RU_V5_04 {

    [Persistent("FmTaxRuV504СвУчСд")]
    public class СвУчСделки : BaseEntity {
        private const String _ИНН_ЮЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}";
        private const String _ИНН_ФЛ_Рег = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{10}";
        private const String _КПП_Рек = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-9A-Z]{2})([0-9]{3})";
        
        private ЛицоТип _Тип;
        public ЛицоТип Тип {
            get { return _Тип; }
            set { SetPropertyValue<ЛицоТип>("Тип", ref _Тип, value); }
        }
        private String _ИНН;
        [Size(12)]
        [RuleRequiredField(TargetCriteria = "Тип == 'ТИП_ЮЛ' || Тип == 'ТИП_ИП'")]
        [RuleRegularExpression("FmTaxRuV504СвУчСд_ИНН1", DefaultContexts.Save, _ИНН_ЮЛ_Рег, TargetCriteria = "Тип == 'ТИП_ЮЛ'",CustomMessageTemplate="Некорректный ИНН")]
        [RuleRegularExpression("FmTaxRuV504СвУчСд_ИНН2",DefaultContexts.Save, _ИНН_ФЛ_Рег, TargetCriteria = "Тип == 'ТИП_ИП'",CustomMessageTemplate="Некорректный ИНН")]
        public String ИНН {
            get { return _ИНН; }
            set { SetPropertyValue<String>("ИНН", ref _ИНН, value); }
        }

        private String _КПП;
        [Size(9)]
        [RuleRequiredField(TargetCriteria = "Тип == 'ТИП_ЮЛ'")]
        [RuleRegularExpression(null, DefaultContexts.Save, _КПП_Рек, CustomMessageTemplate = "Некорректный КПП")]
        public String КПП {
            get { return _КПП; }
            set { SetPropertyValue<String>("КПП", ref _КПП, value); }
        }

        private String _Наименование;
        [Size(256)]
        public String Наименование {
            get { return _Наименование; }
            set { SetPropertyValue<String>("Наименование", ref _Наименование, value); }
        }

        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = null;

            if (Тип == ЛицоТип.ЮР_ЛИЦО) {
                result = document.CreateElement("СведЮЛ");
                last_node.AppendChild(result);
                
                XmlAttribute attrib = null;
                attrib = document.CreateAttribute("ИННЮЛ");
                attrib.Value = ИНН;
                result.Attributes.Append(attrib);
                attrib = document.CreateAttribute("КПП");
                attrib.Value = КПП;
                result.Attributes.Append(attrib);
            }
            if (Тип == ЛицоТип.ПРЕДПРИНИМАТЕЛЬ) {
                result = document.CreateElement("СведИП");
                last_node.AppendChild(result);

                XmlAttribute attrib = null;
                attrib = document.CreateAttribute("ИННФЛ");
                attrib.Value = ИНН;
                result.Attributes.Append(attrib);
            }
            return result;
        }

        public СвУчСделки(Session session) : base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction();
            Тип = ЛицоТип.НЕЗАДАН;
        }

        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
            //switch (propertyName) {
            //    case "ИНН":
            //        if (Тип != ЛицоТип.ТИП_ЮЛ && Тип != ЛицоТип.ТИП_ИП)
            //            throw new ArgumentException("Нельзя задать ИНН, сначала задайте тип лица ЮЛ или ФЛ");
            //        String inn = (String) newValue;
            //        if (Тип == ЛицоТип.ТИП_ЮЛ && !_ИНН_ЮЛ_Рег.IsMatch(inn))
            //            throw new ArgumentException("Неверный формат ИНН для ЮЛ");
            //        if (Тип == ЛицоТип.ТИП_ИП && !_ИНН_ФЛ_Рег.IsMatch(inn))
            //            throw new ArgumentException("Неверный формат ИНН для ИП");
            //        break;
            //    case "КПП":
            //        if (Тип != ЛицоТип.ТИП_ЮЛ && Тип != ЛицоТип.ТИП_ФЛ)
            //            throw new ArgumentException("Нельзя задать КПП, сначала задайте тип лица ЮЛ");
            //        String kpp = (String) newValue;
            //        if (!_КПП_Рег.IsMatch(kpp))
            //            throw new ArgumentException("Неверный формат КПП");
            //        break;
            //}
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "Тип":
                    if (Тип != ЛицоТип.ЮР_ЛИЦО && Тип != ЛицоТип.ФИЗ_ЛИЦО)
                        ИНН = null;
                    if (Тип != ЛицоТип.ЮР_ЛИЦО)
                        КПП = null;
                    break;
            }
        }

        protected override void OnDeleting() {
            base.OnDeleting();
        }
    }
}
