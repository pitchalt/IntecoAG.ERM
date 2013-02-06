using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.Module.ReportHelper;

namespace IntecoAG.ERM.Module
{
    public partial class CreateReportSourceDataController : ViewController
    {
        public CreateReportSourceDataController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void UpdateViewStateEventHandler(object sender, EventArgs e) {
            View.AllowEdit.SetItemValue("CurrentUser", false);
        }

        protected override void OnActivated() {
            base.OnActivated();
            
            if (View.GetType() == typeof(DashboardView)) {
                return;
            }

            object currentObject = View.CurrentObject;
        }

        private void FillSourceData_Execute(object sender, SimpleActionExecuteEventArgs e) {
            //crmGroupByStageAndObligationReportHelper obj = View.ObjectSpace.CreateObject<crmGroupByStageAndObligationReportHelper>();
            //Session ssn = obj.Session;

            //crmGroupByStageAndObligationReportHelper.CreateReportDataSource(ssn, null);

            //View.ObjectSpace.CommitChanges();
        }
 
    }
}
