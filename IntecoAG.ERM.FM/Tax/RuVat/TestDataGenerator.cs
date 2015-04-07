using System;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppWindowControllertopic.
    public partial class TestDataGenerator : WindowController {
        public TestDataGenerator() {
            InitializeComponent();
            RegisterActions(components);
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void GenerateTestDataAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            using (IObjectSpace os = Application.ObjectSpaceProvider.CreateObjectSpace()) {
                ���������������� nlg = os.CreateObject<����������������>(); 
                nlg.������������ = "�� ��� \"��� ��������������\"";
                nlg.��� = "1234567890";
                nlg.��� = "123456789";

//                ����������� vid1 = os.CreateObject<�����������>();
//                vid1.��� = "01";
//                vid1.������������ = "�������/�������";
//                ����������� vid2 = os.CreateObject<�����������>();
//                vid2.��� = "02";
//                vid2.������������ = "������ ��������/����������";

                ��������� val1 = os.CreateObject<���������>();
                val1.��� = "���";
                ��������� val2 = os.CreateObject<���������>();
                val2.��� = "���";

                ��������� period = os.CreateObject<���������>();
                period.����������� = new DateTime(2015,1,1);
                period.���������������� = nlg;

                ��������� ����1 = os.CreateObject<���������>();
                ����1.�������� = ���������.������������.���������;
                ����1.��� = ���������.������������.���;
                ����1.����� = "10100001";
                ����1.���� = new DateTime(2015,01,10);
                ����1.������� = �������.���_��;
                ����1.��� = "1234567890";
                ����1.��� = "123456789";
                ����1.�������������������.����������������� = "��� �����";
                ����1.�������������������.���������� = 11800;
                ����1.�������������������.�������� = 1800;

                ��������� ����2 = os.CreateObject<���������>();
                ����2.�������� = ���������.������������.���������;
                ����2.��� = ���������.������������.���;
                ����2.����� = "10100002";
                ����2.���� = new DateTime(2015, 01, 11);
                ����2.������� = �������.���_��;
                ����2.��� = "1234567899";
                ����2.��� = "123456789";
                ����2.�������������������.����������������� = "��� ������";
                ����2.�������������������.���������� = 118000;
                ����2.�������������������.�������� = 18000;
                ����������������� ����2���2 = os.CreateObject<�����������������>();
                ����2.���������.Add(����2���2);
                ����2���2.���������������� = 1;
                ����2���2.��������������� = new DateTime(2015, 01, 20);
                ����2���2.����������������� = "�� ������";
                ����2���2.���������� = 118118;
                ����2���2.�������� = 18018;

                ��������� ����3 = os.CreateObject<���������>();
                ����3.�������� = ���������.������������.���������;
                ����3.��� = ���������.������������.���;
                ����3.������������� = ���������.����������������.����������������;
                ����3.���������������� = ����1;
                ����3.����� = "10100003";
                ����3.���� = new DateTime(2015, 01, 12);
                ����3.������� = �������.���_��;
                ����3.��� = "1234567899";
                ����3.��� = "123456789";
                ����3.�������������������.����������������� = "��� �����";
                ����3.�������������������.���������� = 1180;
                ����3.�������������������.�������� = 180;
                ����3.�������������������.�������������� = 1770;
                ����3.�������������������.������������ = 90;
                ����3.�������������������.���������������� = 2;
                ����3.�������������������.��������������� = new DateTime(2015, 01, 21);

                ��������� ����4 = os.CreateObject<���������>();
                ����4.�������� = ���������.������������.���������;
                ����4.��� = ���������.������������.���;
                ����4.������������� = ���������.����������������.����������������;
                ����4.���������������� = ����2;
                ����4.����� = "10100004";
                ����4.���� = new DateTime(2015, 01, 12);
                ����4.������� = �������.���_��;
                ����4.��� = "1234567899";
                ����4.��� = "123456789";
                ����4.�������������������.���������������� = 1;
                ����4.�������������������.��������������� = new DateTime(2015, 01, 13);
                ����4.�������������������.����������������� = "��� ������";
                ����4.�������������������.���������� = -1180;
                ����4.�������������������.�������� = -180;
                ����������������� ����4���2 = os.CreateObject<�����������������>();
                ����4.���������.Add(����4���2);
                ����4���2.����������������� = "�� ������";
                ����4���2.���������� = -1185;
                ����4���2.�������� = -181;
                ����4���2.���������������� = 3;
                ����4���2.��������������� = new DateTime(2015, 01, 21);
                ����������������� ����4���3 = os.CreateObject<�����������������>();
                ����4.���������.Add(����4���3);
                ����4���3.���������������� = 2;
                ����4���3.��������������� = new DateTime(2015, 01, 20);
                ����4���3.����������������� = "�� ������";
                ����4���3.���������� = -1185;
                ����4���3.�������� = -181;

//                ��������� ����1 = os.CreateObject<���������>();
//                ����1.�������� = ���������.������������.��������;
//                ����1.��� = ���������.������������.���;
//                ����1.����� = "10100001";
//                ����1.���� = new DateTime(2015, 01, 10);
//                ����1.������� = �������.���_��;
//                ����1.��� = "2345678901";
//                ����1.��� = "234567891";
                
                os.CommitChanges();
            }
        }

    }
}
