using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Budget {

    public partial class TrwBudgetPeriodVC : ViewController {
        public TrwBudgetPeriodVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportInContractBsrAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            TrwBudgetPeriod period = View.CurrentObject as TrwBudgetPeriod;
            if (period == null)
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                period = os.GetObject<TrwBudgetPeriod>(period);
                StreamReader reader = File.OpenText(dialog.FileName);
                TrwBudgetPeriodLogic.ImportInContractBSR(period, os, reader);
                os.CommitChanges();
            }
        }
    }
}
