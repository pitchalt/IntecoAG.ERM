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

    [Persistent("FmTaxRuV504������")]
    public class ���������� : BaseEntity {
        private const String _���_��_��� = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}";
        private const String _���_��_��� = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{10}";
        private const String _���_��� = "([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-9A-Z]{2})([0-9]{3})";
        
        private ������� _���;
        public ������� ��� {
            get { return _���; }
            set { SetPropertyValue<�������>("���", ref _���, value); }
        }
        private String _���;
        [Size(12)]
        [RuleRequiredField(TargetCriteria = "��� == '���_��' || ��� == '���_��'")]
        [RuleRegularExpression("FmTaxRuV504������_���1", DefaultContexts.Save, _���_��_���, TargetCriteria = "��� == '���_��'",CustomMessageTemplate="������������ ���")]
        [RuleRegularExpression("FmTaxRuV504������_���2",DefaultContexts.Save, _���_��_���, TargetCriteria = "��� == '���_��'",CustomMessageTemplate="������������ ���")]
        public String ��� {
            get { return _���; }
            set { SetPropertyValue<String>("���", ref _���, value); }
        }

        private String _���;
        [Size(9)]
        [RuleRequiredField(TargetCriteria = "��� == '���_��'")]
        [RuleRegularExpression(null, DefaultContexts.Save, _���_���, CustomMessageTemplate = "������������ ���")]
        public String ��� {
            get { return _���; }
            set { SetPropertyValue<String>("���", ref _���, value); }
        }

        private String _������������;
        [Size(256)]
        public String ������������ {
            get { return _������������; }
            set { SetPropertyValue<String>("������������", ref _������������, value); }
        }

        public XmlNode ToXmlGenerated(XmlDocument document, XmlNode last_node) {
            XmlNode result = null;

            if (��� == �������.��_����) {
                result = document.CreateElement("������");
                last_node.AppendChild(result);
                
                XmlAttribute attrib = null;
                attrib = document.CreateAttribute("�����");
                attrib.Value = ���;
                result.Attributes.Append(attrib);
                attrib = document.CreateAttribute("���");
                attrib.Value = ���;
                result.Attributes.Append(attrib);
            }
            if (��� == �������.���������������) {
                result = document.CreateElement("������");
                last_node.AppendChild(result);

                XmlAttribute attrib = null;
                attrib = document.CreateAttribute("�����");
                attrib.Value = ���;
                result.Attributes.Append(attrib);
            }
            return result;
        }

        public ����������(Session session) : base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction();
            ��� = �������.�������;
        }

        public override void OnChanging(string propertyName, object newValue) {
            base.OnChanging(propertyName, newValue);
            if (IsLoading)
                return;
            //switch (propertyName) {
            //    case "���":
            //        if (��� != �������.���_�� && ��� != �������.���_��)
            //            throw new ArgumentException("������ ������ ���, ������� ������� ��� ���� �� ��� ��");
            //        String inn = (String) newValue;
            //        if (��� == �������.���_�� && !_���_��_���.IsMatch(inn))
            //            throw new ArgumentException("�������� ������ ��� ��� ��");
            //        if (��� == �������.���_�� && !_���_��_���.IsMatch(inn))
            //            throw new ArgumentException("�������� ������ ��� ��� ��");
            //        break;
            //    case "���":
            //        if (��� != �������.���_�� && ��� != �������.���_��)
            //            throw new ArgumentException("������ ������ ���, ������� ������� ��� ���� ��");
            //        String kpp = (String) newValue;
            //        if (!_���_���.IsMatch(kpp))
            //            throw new ArgumentException("�������� ������ ���");
            //        break;
            //}
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading)
                return;
            switch (propertyName) {
                case "���":
                    if (��� != �������.��_���� && ��� != �������.���_����)
                        ��� = null;
                    if (��� != �������.��_����)
                        ��� = null;
                    break;
            }
        }

        protected override void OnDeleting() {
            base.OnDeleting();
        }
    }
}
