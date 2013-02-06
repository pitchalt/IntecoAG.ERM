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
    /// Данный контроллер создаёт кнопку для печати отчёта "Перечень неоплаченных счетов".
    /// </summary>
    public partial class fmCRHUnpayedRequestViewController : ViewController
    {

        public fmCRHUnpayedRequestViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        /// <summary>
        /// Печать отчёта "Перечень неоплаченных счетов на ... г." в полном варианте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullPrintAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ShowReport("Full");
        }

        /// <summary>
        /// Печать отчёта "Перечень неоплаченных счетов на ... г." в коротком варианте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShortPrintAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ShowReport("Short");
        }

        private void ShowReport(string reportMode) {
            CriteriaOperator criteria = ((fmCRHUnpayedRequest)View.CurrentObject).GetCriteria();

            // Определяем тип объектов отчёта
            Type dsType = typeof(fmCRHUnpayedRequestContractLine);

            // Получаем название файла (без расширения, которое всегда .repx) разметки отчёта
            string reportFileName = "fmCRHUnpayedRequestLine";

            
            // Создаём для отчёта отдельный ObjectSpace
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            // Создаём объект, чтобы вызвать в нём метод фомирования коллекции
            fmCRHUnpayedRequest obj = View.CurrentObject as fmCRHUnpayedRequest;
            if (obj == null)
                return;

            // Находим сессию
            Session ssn = ((ObjectSpace)objectSpace).Session;

            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);

            string checkDir = fi.Directory.FullName + "\\Reports\\";


            // http://www.devexpress.com/Support/Center/p/Q135993.aspx
            //XafExtReport rep = new XafExtReport();
            XafReport rep = new XafReport();
            rep.LoadLayout(checkDir + reportFileName + ".repx");

            if (reportMode == "Short") {
                for (int i = 0; i < rep.Bands.Count; i++) {
                    if (rep.Bands[i].Name == "detailBand1") {
                        rep.Bands[i].Visible = false;
                    }
                }
            }

            rep.DataSource = obj.CreateReportListSource(ssn, criteria);

            //rep.ShowDesignerDialog();
            rep.ShowPreview();
        }

    }
}
