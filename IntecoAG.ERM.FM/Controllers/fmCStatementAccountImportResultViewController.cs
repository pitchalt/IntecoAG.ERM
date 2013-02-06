using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.FM.StatementAccount;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCStatementAccountImportResultViewController : ViewController {
        public fmCStatementAccountImportResultViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        // !!!!!!!!!!!!!!!!! Контроллер не нужен. УДАЛИТЬ ЕГО ПОСЛЕ ПОКАЗА !!!!!!!!!!!!!!!!!!!!!

        protected override void OnActivated() {
            base.OnActivated();
            ProcessAction.Active["IT_HIDE"] = false;
        }

        private void ProcessAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || View.CurrentObject == null) return;
            fmCSAImportResult result = View.CurrentObject as fmCSAImportResult;
            if (result == null) return;
            if (result.TaskImporter == null) return;
            fmCSAStatementAccountImportLogic.PostProcess(this.ObjectSpace, result.TaskImporter, result);
            this.ObjectSpace.CommitChanges();
        }
    }
}
