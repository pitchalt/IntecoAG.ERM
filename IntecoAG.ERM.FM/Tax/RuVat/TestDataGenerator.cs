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
                Налогоплательщик nlg = os.CreateObject<Налогоплательщик>(); 
                nlg.Наименование = "АО ВПК \"НПО машиностроения\"";
                nlg.ИНН = "1234567890";
                nlg.КПП = "123456789";

//                ВидОперации vid1 = os.CreateObject<ВидОперации>();
//                vid1.Код = "01";
//                vid1.Наименование = "Покупки/Продажи";
//                ВидОперации vid2 = os.CreateObject<ВидОперации>();
//                vid2.Код = "02";
//                vid2.Наименование = "Авансы выданные/полученные";

                ВалютаОКВ val1 = os.CreateObject<ВалютаОКВ>();
                val1.Код = "РУБ";
                ВалютаОКВ val2 = os.CreateObject<ВалютаОКВ>();
                val2.Код = "ДОЛ";

                ПериодНДС period = os.CreateObject<ПериодНДС>();
                period.ДатаПериода = new DateTime(2015,1,1);
                period.Налогоплательщик = nlg;

                Основание осни1 = os.CreateObject<Основание>();
                осни1.Источник = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                осни1.Тип = Основание.ТипОснования.СЧФ;
                осни1.Номер = "10100001";
                осни1.Дата = new DateTime(2015,01,10);
                осни1.ЛицоТип = ЛицоТип.ТИП_ЮЛ;
                осни1.ИНН = "1234567890";
                осни1.КПП = "123456789";
                осни1.ДействующийДокумент.НаименКонтрагента = "ОАО Пермь";
                осни1.ДействующийДокумент.СуммаВсего = 11800;
                осни1.ДействующийДокумент.СуммаНДС = 1800;

                Основание осни2 = os.CreateObject<Основание>();
                осни2.Источник = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                осни2.Тип = Основание.ТипОснования.СЧФ;
                осни2.Номер = "10100002";
                осни2.Дата = new DateTime(2015, 01, 11);
                осни2.ЛицоТип = ЛицоТип.ТИП_ЮЛ;
                осни2.ИНН = "1234567899";
                осни2.КПП = "123456789";
                осни2.ДействующийДокумент.НаименКонтрагента = "ОАО Стрела";
                осни2.ДействующийДокумент.СуммаВсего = 118000;
                осни2.ДействующийДокумент.СуммаНДС = 18000;
                ОснованиеДокумент осни2док2 = os.CreateObject<ОснованиеДокумент>();
                осни2.Документы.Add(осни2док2);
                осни2док2.НомерИсправления = 1;
                осни2док2.ДатаИсправления = new DateTime(2015, 01, 20);
                осни2док2.НаименКонтрагента = "АО Стрела";
                осни2док2.СуммаВсего = 118118;
                осни2док2.СуммаНДС = 18018;

                Основание осни3 = os.CreateObject<Основание>();
                осни3.Источник = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                осни3.Тип = Основание.ТипОснования.СЧФ;
                осни3.Корректировка = Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ;
                осни3.БазовоеОснование = осни1;
                осни3.Номер = "10100003";
                осни3.Дата = new DateTime(2015, 01, 12);
                осни3.ЛицоТип = ЛицоТип.ТИП_ЮЛ;
                осни3.ИНН = "1234567899";
                осни3.КПП = "123456789";
                осни3.ДействующийДокумент.НаименКонтрагента = "ОАО Пермь";
                осни3.ДействующийДокумент.СуммаВсего = 1180;
                осни3.ДействующийДокумент.СуммаНДС = 180;
                осни3.ДействующийДокумент.СуммаВсегоУвел = 1770;
                осни3.ДействующийДокумент.СуммаНДСУвел = 90;
                осни3.ДействующийДокумент.НомерИсправления = 2;
                осни3.ДействующийДокумент.ДатаИсправления = new DateTime(2015, 01, 21);

                Основание осни4 = os.CreateObject<Основание>();
                осни4.Источник = Основание.ТипИсточника.ИСХОДЯЩИЙ;
                осни4.Тип = Основание.ТипОснования.СЧФ;
                осни4.Корректировка = Основание.ТипПодчиненности.КОРРЕКТИРОВОЧНЫЙ;
                осни4.БазовоеОснование = осни2;
                осни4.Номер = "10100004";
                осни4.Дата = new DateTime(2015, 01, 12);
                осни4.ЛицоТип = ЛицоТип.ТИП_ЮЛ;
                осни4.ИНН = "1234567899";
                осни4.КПП = "123456789";
                осни4.ДействующийДокумент.НомерИсправления = 1;
                осни4.ДействующийДокумент.ДатаИсправления = new DateTime(2015, 01, 13);
                осни4.ДействующийДокумент.НаименКонтрагента = "ОАО Стрела";
                осни4.ДействующийДокумент.СуммаВсего = -1180;
                осни4.ДействующийДокумент.СуммаНДС = -180;
                ОснованиеДокумент осни4док2 = os.CreateObject<ОснованиеДокумент>();
                осни4.Документы.Add(осни4док2);
                осни4док2.НаименКонтрагента = "АО Стрела";
                осни4док2.СуммаВсего = -1185;
                осни4док2.СуммаНДС = -181;
                осни4док2.НомерИсправления = 3;
                осни4док2.ДатаИсправления = new DateTime(2015, 01, 21);
                ОснованиеДокумент осни4док3 = os.CreateObject<ОснованиеДокумент>();
                осни4.Документы.Add(осни4док3);
                осни4док3.НомерИсправления = 2;
                осни4док3.ДатаИсправления = new DateTime(2015, 01, 20);
                осни4док3.НаименКонтрагента = "АО Стрела";
                осни4док3.СуммаВсего = -1185;
                осни4док3.СуммаНДС = -181;

//                Основание оснв1 = os.CreateObject<Основание>();
//                оснв1.Источник = Основание.ТипИсточника.ВХОДЯЩИЙ;
//                оснв1.Тип = Основание.ТипОснования.СЧФ;
//                оснв1.Номер = "10100001";
//                оснв1.Дата = new DateTime(2015, 01, 10);
//                оснв1.ЛицоТип = ЛицоТип.ТИП_ЮЛ;
//                оснв1.ИНН = "2345678901";
//                оснв1.ИНН = "234567891";
                
                os.CommitChanges();
            }
        }

    }
}
