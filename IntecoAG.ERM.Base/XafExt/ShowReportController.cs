using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

using System.Data;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Reports;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.Module.ReportHelper;

namespace IntecoAG.ERM.Module
{
    /// <summary>
    /// Контроллер предназначен для размещения кнопки выполнения отчёта на DetailView для объекта,
    /// унаследованного от класса BaseReportHelper
    /// </summary>
    public partial class ShowReportController : ViewController
    {
        private Frame frame = null;
        private DetailView detailView = null;
        private object currentObject = null;

        public ShowReportController() {
            InitializeComponent();
            RegisterActions(components);
        }

        //private void UpdateViewStateEventHandler(object sender, EventArgs e) {
        //    View.AllowEdit.SetItemValue("CurrentUser", false);
        //}

        protected override void OnActivated() {
            base.OnActivated();

            //this.Active.Clear();

            frame = this.Frame;
            if (frame == null) {
                this.Active["Enabled"] = false;
                return;
            }

            detailView = this.View as DetailView;
            if (detailView == null) {
                this.Active["Enabled"] = false;
                return;
            }

            //if (View.GetType() == typeof(DashboardView)) {
            //    return;
            //}

            currentObject = View.CurrentObject;
            if (currentObject == null) {
                this.Active["Enabled"] = false;
                return;
            }

            if (!(currentObject is BaseReportFilter)) {
                this.Active["Enabled"] = false;
                return;
            }

        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            this.Active.Clear();
        }

        private void ShowReportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            CriteriaOperator criteria = ((BaseReportFilter)currentObject).GetCriteria();

            // Определяем тип объектов отчёта
            Type dsType = ((BaseReportFilter)currentObject).GetReportDataSourceType();

            // Получаем название файла (без расширения, которое всегда .repx) разметки отчёта
            string reportFileName = ((BaseReportFilter)currentObject).GetReportFileName();

            //PreShowReport(dsType, criteria);
            //ShowReport<dsType>(criteria);
            ShowReport(reportFileName, dsType, criteria);
        }

        //private void PreShowReport(Type dsType, CriteriaOperator criteria) {
        //    ShowReport<dsType>(criteria);
        //}

        private void ShowReport(string reportFileName, Type dsType, CriteriaOperator criteria) {
            // Создаём для отчёта отдельный ObjectSpace
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            // Создаём объект, чтобы вызвать в нём метод фомирования коллекции
            BaseReportHelper obj = objectSpace.CreateObject(dsType) as BaseReportHelper;

            // Находим сессию
            Session ssn = obj.Session;

            string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(fname);

            string checkDir = fi.Directory.FullName + "\\Reports\\";


            // http://www.devexpress.com/Support/Center/p/Q135993.aspx
            XafExtReport rep = new XafExtReport();
            rep.LoadLayout(checkDir + reportFileName + ".repx");

            /*
            // Источник данных - DataTable
            //rep.DataAdapter = ;
            //rep.DataSource = obj.CreateReportDataSource(ssn, criteria);   //new XPCollection<DomainObject1>(View.ObjectSpace.Session);
            rep.DataSource = (DataSet)obj.CreateReportDataSet(ssn, criteria);
            rep.DataMember = ((DataSet)rep.DataSource).Tables[0].TableName;
            //rep.FillDataSource();
            */

            // Источник данных XPCollection
            //XafExtReport crep = rep as XafExtReport;

            rep.DataSource = obj.CreateReportListSource(ssn, criteria);
            //rep.DataSource = obj.CreateReportListFromLinqSource(ssn, criteria);

            //rep.ShowDesignerDialog();
            rep.ShowPreview();
        }

        //private void ShowReport<dsType> (CriteriaOperator criteria) {
        //    Session ssn = ((BaseReportFilter)currentObject).Session.BeginNestedUnitOfWork();

        //    // Формируем источник данных
        //    // Источник данных формируется как результат работы метода в классе с типом dsType
        //    //XPCollection col = new XPCollection(dsType);

        //    // Создаём объект типа dsType
        //    dsType obj = View.ObjectSpace.CreateObject<dsType>();
        //    //BaseReportHelper.CreateReportDataSource(ssn, criteria);
        //    //((BaseReportHelper)obj).CreateReportDataSource(ssn, criteria);

        //    // Считываем отчёт с диска, связываем с источником данных и выполняем
        //    string reportName = "";
        //    ReportData reportdata = ssn.FindObject<ReportData>(new BinaryOperator("Name", reportName));
        //    //if (reportdata == null) {
        //    //    using (FileStream fs = fi.OpenRead()) {
        //    //        reportdata = new ReportData(os.Session);
        //    //        XafReport rep = new XafReport();
        //    //        rep.ReportName = reportName;
        //    //        rep.ObjectSpace = os;
        //    //        rep.LoadLayout(fs);
        //    //        reportdata.SaveXtraReport(rep);
        //    //        reportdata.Save();
        //    //    }
        //    //}
        //    /*
        //    if (reportdata != null) {
        //        //reportdata = new ReportData(os.Session);
        //        XafReport rep = new XafReport();
        //        rep.ReportName = reportName;
        //        //rep.FillDataSource
        //        //rep.ApplyFiltering();
        //        rep.ObjectSpace = View.ObjectSpace.CreateNestedObjectSpace();
        //        //rep.DataSource = BaseReportHelper.CreateReportDataSource(ssn, criteria);
        //    }
        //    */

        //    string fname = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        //    FileInfo fi = new FileInfo(fname);

        //    string checkDir1 = fi.Directory.FullName;
        //    string checkDir2 = checkDir1 + "\\Reports";


        //    // http://www.devexpress.com/Support/Center/p/Q135993.aspx
        //    XafReport rep = new XafReport();
        //    rep.LoadLayout(checkDir2 + "Spravka1.repx");
        //    rep.DataSource = ((BaseReportHelper)obj).CreateReportDataSource(ssn, criteria);   //new XPCollection<DomainObject1>(View.ObjectSpace.Session);
        //    //rep.ShowDesignerDialog();
        //    rep.ShowPreview();
        //}

        private void ClearReportFilterAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ((BaseReportFilter)currentObject).ClearCriteria();
        }
 
    }
}
