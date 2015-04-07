using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.Tax.RuVat {
    public partial class ОснованиеVC : ViewController {
        public ОснованиеVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                ОснованиеЛогика.ImportInvoices(os, dialog.FileName);
                os.CommitChanges();
            }
        }
    }
}
