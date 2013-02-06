using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Reports;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.Module.ReportHelper;

namespace IntecoAG.ERM.Module {
    public partial class ImportReportsViewController : ViewController {

        //const String DO_ACTIVE = "DO_ACTIVE";

        public ImportReportsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            EnableButton();
        }

        private void ImportReports_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace objSpace = this.ObjectSpace;

            // Добавление отчётов
            CheckDirectoryOnReports();
            //CreateReport("Раб. план 2");
            //CreateReport("crmContract Versions");

            // Добавление аналитических данных
            CreateDataToBeAnalysed();
        }

        private void EnableButton() {
            // Скорее всего импорт отчётов придётся убрать совсем. А пока скрываем эту action.
            // Поправил работу кнопки (2012-08-22)
            //this.ImportReports.Active[DO_ACTIVE] = false;
        }

        private static Int32 GetRandomIntegerFromInterval(int minVal, int maxVal) {
            System.Random rand = new Random(DateTime.Now.Millisecond);
            return rand.Next(minVal, maxVal);
        }

        /// <summary>
        /// Проверяется директория запуска и её поддиректория Reports, если таковая существует
        /// </summary>
        private void CheckDirectoryOnReports() {
            // Директория запуска
            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);

            string checkDir1 = fi.Directory.FullName;
            string checkDir2 = checkDir1 + "\\ReportLayouts";

            ReportHelper.ReportHelper.GetAllReportsFromDirectory(checkDir1, ObjectSpace);
            ReportHelper.ReportHelper.GetAllReportsFromDirectory(checkDir2, ObjectSpace);
        }

        // ANALYSIS & PIVOT
        // http://documentation.devexpress.com/#Xaf/CustomDocument3050   "Distribute the Created Analysis with the Application"
        // По указанному адресу находится описание приёмов как сохранить Layout (располжение полей и т.п.) и восстанавливать его при последующих запусках
        private void CreateDataToBeAnalysed() {
            Analysis taskAnalysis1 = ObjectSpace.FindObject<Analysis>(CriteriaOperator.Parse("Name='Анализ движения денег'"));
            if (taskAnalysis1 == null) {
                taskAnalysis1 = ObjectSpace.CreateObject<Analysis>();
                taskAnalysis1.Name = "Анализ движения денег";
                taskAnalysis1.ObjectTypeName = typeof(crmPaymentPlan).FullName;

                // DataType не будет отображаться в выпадающем списке в дизайнере, возможная причина - этот тип исключён из пунктов навигатора.
                taskAnalysis1.DataType = typeof(crmPaymentPlan);

                taskAnalysis1.Criteria = null;   // "[DueDate] < '@CurrentDate' and [Status] = 'Completed'";
                taskAnalysis1.Save();
            }
        }

    }
}
