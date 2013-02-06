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
    public partial class fmCRHUnpayedReqReportParamsViewController : ViewController {

        public fmCRHUnpayedReqReportParamsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            if (View != null && (View.CurrentObject as fmCRHUnpayedRequestReportParameters) != null) {
                fmCRHUnpayedRequestReportParameters unpayedRequestReportParameters = View.CurrentObject as fmCRHUnpayedRequestReportParameters;
                if (unpayedRequestReportParameters != null && unpayedRequestReportParameters.ReportDate == DateTime.MinValue) {
                    unpayedRequestReportParameters.ReportDateStart = DateTime.MinValue;   //DateTime.Now.Date.AddDays(-1);
                    unpayedRequestReportParameters.ReportDate = DateTime.Now.Date;
                    DateTime beginDate = DateTime.Now.Date.AddMonths(-1);
                    beginDate = beginDate.AddDays(1-beginDate.Day);
                    unpayedRequestReportParameters.ReportDateStart = beginDate;
                }
            }
        }

        private void NextRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCRHUnpayedRequestReportParameters current = View.CurrentObject as fmCRHUnpayedRequestReportParameters;
            if (current == null)
                return;

            if (current.ReportMode) {
                ShowReport("Short");
            } else {
                ShowReport("Full");
            }
        }


        private void ShowReport(string reportMode) {
            fmCRHUnpayedRequestReportParameters current = View.CurrentObject as fmCRHUnpayedRequestReportParameters;
            if (current == null)
                return;

            // Создаём для отчёта отдельный ObjectSpace
            IObjectSpace objectSpace = ObjectSpace;   //ObjectSpace;   // Application.CreateObjectSpace();

            /*
            // Определяем тип объектов отчёта
            Type dsType = typeof(fmCRHUnpayedRequestContractList);

            // Получаем название файла (без расширения, которое всегда .repx) разметки отчёта
            string reportFileName = "fmCRHUnpayedRequestLine";

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

            IReportData reportData = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", "fmCRHUnpayedRequestLine"));
            XafReport rep = reportData.LoadXtraReport(objectSpace);

            rep.DataSource = current.GenerateReportContent();

            //rep.ShowDesignerDialog();
            rep.ShowPreview();
        }

    }
}
