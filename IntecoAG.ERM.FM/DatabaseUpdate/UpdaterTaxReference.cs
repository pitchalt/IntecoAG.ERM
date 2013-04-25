using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CS.Settings;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.FM {
    /// <summary>
    /// Disable
    /// </summary>
    public class UpdaterTaxReference : ModuleUpdater {
        public UpdaterTaxReference(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion != new Version("0.0.0.0"))
                return;
            //
            IObjectSpace os = ObjectSpace;

            // Справочник Оснований платежа
            CreatePaymentBase(os);
            os.CommitChanges();

            // Справочник КБК
            CreatePaymentKBK(os);
            os.CommitChanges();

            // Справочник Типов платежа
            CreatePaymentKind(os);
            os.CommitChanges();

            // Справочник "Периоды уплаты налога (сбора)"
            CreatePaymenTaxPeriod(os);
            os.CommitChanges();
        }

        private void CreatePaymentBase(IObjectSpace os) {
            XPQuery<fmCPaymentBase> Queries = new XPQuery<fmCPaymentBase>(((ObjectSpace)os).Session);
            int count = (from elem in Queries
                         select elem).Count();
            if (count > 0)
                return;
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ТП", "ТП", "платежи текущего года");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ТП", "ТП", "платежи текущего года");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ЗД", "ЗД", "добровольное погашение задолженности по истекшим налоговым периодам при отсутствии требования об уплате налогов (сборов) от налогового органа");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "БФ", "БФ", "текущие платежи физических лиц - клиентов банка (владельцев счета), уплачиваемые со своего банковского счета");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ТР", "ТР", "погашение задолженности по требованию налогового органа об уплате налогов (сборов)");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "PC", "PC", "погашение рассроченной задолженности");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ОТ", "ОТ", "погашение отсроченной задолженности");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "РТ", "РТ", "погашение реструктурируемой задолженности");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ВУ", "ВУ", "погашение отсроченной задолженности в связи с введением внешнего управления");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "ПР", "ПР", "погашение задолженности, приостановленной к взысканию");
            CreatePaymentRefRecord<fmCPaymentBase>(os, "АП", "АП", "погашение задолженности по акту проверки");
        }

        private void CreatePaymentKBK(IObjectSpace os) {
            XPQuery<fmCPaymentKBK> Queries = new XPQuery<fmCPaymentKBK>(((ObjectSpace)os).Session);
            int count = (from elem in Queries
                         select elem).Count();
            if (count > 0)
                return;
            CreatePaymentRefRecord<fmCPaymentKBK>(os, "01010", "182 1 05 01010 01 0000 110", "единый налог, взимаемый при применении упрощенной системы налогообложения с налогоплательщиков, выбравших в качестве объекта налогообложения доходы");
            CreatePaymentRefRecord<fmCPaymentKBK>(os, "01020", "182 1 05 01020 01 0000 110", "единый налог, взимаемый при применении упрощенной системы налогообложения с налогоплательщиков, выбравших в качестве объекта налогообложения доходы, уменьшенные на величину расходов");
            CreatePaymentRefRecord<fmCPaymentKBK>(os, "01030", "182 1 05 01030 01 0000 110", "единый минимальный налог, зачисляемый в бюджеты государственных внебюджетных фондов");
            CreatePaymentRefRecord<fmCPaymentKBK>(os, "01040", "182 1 05 01040 02 0000 110", "доходы от выдачи патентов на осуществление предпринимательской деятельности при применении упрощенной системы налогообложения");
        }

        private void CreatePaymentKind(IObjectSpace os) {
            XPQuery<fmCPaymentKind> Queries = new XPQuery<fmCPaymentKind>(((ObjectSpace)os).Session);
            int count = (from elem in Queries
                         select elem).Count();
            if (count > 0)
                return;
            CreatePaymentRefRecord<fmCPaymentKind>(os, "НС", "НС", "Налог или сбор");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "АВ", "АВ", "Аванс или предоплата");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ПЛ", "ПЛ", "Платеж");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ГП", "ГП", "Пошлина");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ВЗ", "ВЗ", "Взнос");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ПЕ", "ПЕ", "Пеня");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ПЦ", "ПЦ", "Проценты");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "СА", "СА", "Налоговые санкции, установленные НК РФ");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "АШ", "АШ", "Административные штрафы");
            CreatePaymentRefRecord<fmCPaymentKind>(os, "ИШ", "ИШ", "Иные штрафы, установленные соответствующими законодательными или иными нормативными актами");
        }

        private void CreatePaymenTaxPeriod(IObjectSpace os) {
            XPQuery<fmCPaymentTaxPeriod> Queries = new XPQuery<fmCPaymentTaxPeriod>(((ObjectSpace)os).Session);
            int count = (from elem in Queries
                         select elem).Count();
            if (count > 0)
                return;
            CreatePaymentRefRecord<fmCPaymentTaxPeriod>(os, "МС", "МС", "месячные платежи");
            CreatePaymentRefRecord<fmCPaymentTaxPeriod>(os, "KB", "KB", "квартальные платежи");
            CreatePaymentRefRecord<fmCPaymentTaxPeriod>(os, "ПЛ", "ПЛ", "полугодовые платежи");
            CreatePaymentRefRecord<fmCPaymentTaxPeriod>(os, "ГД", "ГД", "годовые платежи");
        }

        private void CreatePaymentRefRecord<T>(IObjectSpace os, String code, String name, String description) where T : csCCodedComponent {
            T elem = os.CreateObject<T>();
            elem.Code = code;
            elem.Name = name;
            elem.Description = description;
            elem.Save();
        }
    }
}
