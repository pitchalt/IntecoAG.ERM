using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Reports;
//using DevExpress.ExpressApp.Reports.XafReport;
using DevExpress.XtraReports.UI;
//
using IntecoAG.ERM.FM.ReportHelper;
using IntecoAG.ERM.Module.ReportHelper;
//

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// Данный контроллер создаёт кнопку для подтверждения формирования данных для отчёта "Перечень неоплаченных счетов".
    /// </summary>
    public partial class fmCRHPayedReqReportParamsViewController : ViewController
    {

        public fmCRHPayedReqReportParamsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            if (View != null && (View.CurrentObject as fmCRHPayedRequestReportParameters) != null) {
                fmCRHPayedRequestReportParameters payedRequestReportParameters = View.CurrentObject as fmCRHPayedRequestReportParameters;
                if (payedRequestReportParameters != null && payedRequestReportParameters.ReportDate == DateTime.MinValue) {
                    payedRequestReportParameters.ReportDateStart = DateTime.Now.Date.AddDays(-1);
                    payedRequestReportParameters.ReportDate = DateTime.Now.Date.AddDays(-1);   // DateTime.Now.Date;
                }
            }
        }

        private void NextRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCRHPayedRequestReportParameters current = View.CurrentObject as fmCRHPayedRequestReportParameters;
            if (current == null)
                return;

            if (current.ReportMode) {
                ShowReport("Short");
            } else {
                ShowReport("Full");
            }
        }


        private void ShowReport(string reportMode) {
            fmCRHPayedRequestReportParameters current = View.CurrentObject as fmCRHPayedRequestReportParameters;
            if (current == null)
                return;

            // Создаём для отчёта отдельный ObjectSpace
            IObjectSpace objectSpace = ObjectSpace;   //ObjectSpace;   // Application.CreateObjectSpace();

            /*
            // Определяем тип объектов отчёта
            Type dsType = typeof(fmCRHPayedRequestNonPersistent);

            // Получаем название файла (без расширения, которое всегда .repx) разметки отчёта
            string reportFileName = "fmCRHPayedRequestReport";

            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);

            string checkDir = fi.Directory.FullName + "\\Reports\\";

            // http://www.devexpress.com/Support/Center/p/Q135993.aspx
            //XafExtReport rep = new XafExtReport();
            XafReport rep = new XafReport();
            rep.LoadLayout(checkDir + reportFileName + ".repx");
            */

            /*
            if (reportMode == "Short") {
                for (int i = 0; i < rep.Bands.Count; i++) {
                    if (rep.Bands[i].Name == "detailBand1") {
                        rep.Bands[i].Visible = false;
                    }
                }
            }
            */

            //IReportData reportData = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", "fmCRHPayedRequestReport"));
            IReportData reportData = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", "Список оплаченных счетов"));
            XafReport rep = reportData.LoadXtraReport(objectSpace);

            rep.DataSource = current.GenerateReportContent();

            //rep.ShowDesignerDialog();
            rep.ShowPreview();
        }

    }
}
