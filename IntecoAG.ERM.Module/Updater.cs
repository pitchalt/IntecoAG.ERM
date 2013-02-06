using System;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Reports;

using CS=IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.Module
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            // Добавление отчётов
            CheckDirectoryOnReports();
            //CreateReport("Раб. план 2");
            //CreateReport("crmContract Versions");

            // Добавление аналитических данных
            CreateDataToBeAnalysed();


            #region ДАННЫЕ ДЛЯ ОБЪЕКТА "csTimeUnit"

            CS.Measurement.csTimeUnit TimeUnit1 = ObjectSpace.FindObject<CS.Measurement.csTimeUnit>(new BinaryOperator("Name", "day"));
            if (TimeUnit1 == null) {
                TimeUnit1 = ObjectSpace.CreateObject<CS.Measurement.csTimeUnit>();
                TimeUnit1.Name = "day";
                TimeUnit1.Save();
            }

            CS.Measurement.csTimeUnit TimeUnit2 = ObjectSpace.FindObject<CS.Measurement.csTimeUnit>(new BinaryOperator("Name", "month"));
            if (TimeUnit2 == null) {
                TimeUnit2 = ObjectSpace.CreateObject<CS.Measurement.csTimeUnit>();
                TimeUnit2.Name = "month";
                TimeUnit2.Save();
            }

            CS.Measurement.csTimeUnit TimeUnit3 = ObjectSpace.FindObject<CS.Measurement.csTimeUnit>(new BinaryOperator("Name", "year"));
            if (TimeUnit3 == null) {
                TimeUnit3 = ObjectSpace.CreateObject<CS.Measurement.csTimeUnit>();
                TimeUnit3.Name = "year";
                TimeUnit3.Save();
            }

            #endregion

            #region ДАННЫЕ ДЛЯ ОБЪЕКТА "Stage"
            /*
            CRM.crmContract.DateTimeExt dteStd1 = ObjectSpace.CreateObject<CRM.crmContract.DateTimeExt>();
            dteStd1.DateTime = System.DateTime.Now;
            CRM.crmContract.Time tm1 = ObjectSpace.CreateObject<CRM.crmContract.Time>();
            tm1.AbsoluteDateTime = dteStd1;


            CRM.crmContract.Stage Stage1 = ObjectSpace.FindObject<CRM.crmContract.Stage>(new BinaryOperator("Oid", 1));
            if (Stage1 == null) {
                Stage1 = ObjectSpace.CreateObject<CRM.crmContract.Stage>();
                Stage1.DateBegin = tm1;
                Stage1.Save();
            }

            CRM.crmContract.DateTimeExt dteStd2 = ObjectSpace.CreateObject<CRM.crmContract.DateTimeExt>();
            dteStd1.TimeSingularity = CS.TimeSingularity.NegativeInfinity;
            CRM.crmContract.Time tm2 = ObjectSpace.CreateObject<CRM.crmContract.Time>();
            tm1.AbsoluteDateTime = dteStd2;

            CRM.crmContract.Stage Stage2 = ObjectSpace.FindObject<CRM.crmContract.Stage>(new BinaryOperator("Oid", 2));
            if (Stage2 == null) {
                Stage2 = ObjectSpace.CreateObject<CRM.crmContract.Stage>();
                Stage2.DateBegin = tm2;
                Stage2.Save();
            }
*/
            #endregion

        }

        /// <summary>
        /// Проверяется директория запуска и её поддиректория Reports, если таковая существует
        /// </summary>
        private void CheckDirectoryOnReports() {
            // Директория запуска
            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);
            
            string checkDir1 = fi.Directory.FullName;
            string checkDir2 = checkDir1 + "\\Reports";

            GetAllReportsFromDirectory(checkDir1);
            GetAllReportsFromDirectory(checkDir2);
        }

        /// <summary>
        /// http://www.devexpress.com/Support/Center/e/E1160.aspx How to import additional reports into the database
        /// </summary>
        /// <param name="Path"></param>
         private void GetAllReportsFromDirectory(string Path) {

            DirectoryInfo di = new DirectoryInfo(Path);
            try {
                if (!di.Exists) return;

                foreach (FileInfo fi in di.GetFiles("*.repx", SearchOption.TopDirectoryOnly)) {
                    //using (UnitOfWork uow = new UnitOfWork()) {   //XpoDefault.DataLayer)) {
                        //using (ObjectSpace os = new ObjectSpace(uow, XafTypesInfo.Instance)) {
                        IObjectSpace os = ObjectSpace;
                            try {
                                CreateReport((ObjectSpace)os, fi);
                                if (os.IsModified) {
                                    os.CommitChanges();
                                }
                            } catch { // (Exception e) {
                                os.Rollback();
                                //Console.WriteLine("{0} report cannot be imported because of an error: {1} \nStackTrace: {2} \n",
                                //fi.Name, e.Message, e.StackTrace);
                            }
                        //}
                    //}
                }
            } catch { // (Exception e) {
                //Console.WriteLine("The process failed: {0}", e.ToString());
            } finally { }
        }


        private static void CreateReport(ObjectSpace os, FileInfo fi) {
            string reportName = fi.Name.TrimEnd(fi.Extension.ToCharArray());
            ReportData reportdata = os.Session.FindObject<ReportData>(new BinaryOperator("Name", reportName));
            if (reportdata == null) {
                using (FileStream fs = fi.OpenRead()) {
                    reportdata = new ReportData(os.Session);
                    XafReport rep = new XafReport();
                    rep.ReportName = reportName;
                    rep.ObjectSpace = os;
                    rep.LoadLayout(fs);
                    reportdata.SaveXtraReport(rep);
                    reportdata.Save();
                }
            }
        }


        private void CreateReport(string reportName) {
            ReportData reportdata = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", reportName));
            if (reportdata == null) {
                reportdata = ObjectSpace.CreateObject<ReportData>();
                XafReport rep = new XafReport();
                rep.ObjectSpace = ObjectSpace;
//                rep.LoadLayout(GetType().Assembly.GetManifestResourceStream("IntecoAG.ERM.Module.Reports." + reportName + ".repx"));
                rep.ReportName = reportName;
                reportdata.SaveXtraReport(rep);
                reportdata.Save();
            }
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
