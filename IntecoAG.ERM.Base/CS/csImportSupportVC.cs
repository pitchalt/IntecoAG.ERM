using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS {
    public partial class csImportSupportVC : ObjectViewController {
        public csImportSupportVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            ImportAction.Active.SetItemValue(typeof(csImportSupportVC).FullName, typeof(csIImportSupport).IsAssignableFrom(View.ObjectTypeInfo.Type));

        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            csIImportSupport obj = View.CurrentObject as csIImportSupport;
            if (obj == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    obj = os.GetObject(obj);
                    obj.Import(os, dialog.FileName);
                    os.CommitChanges();
                }
            }

        }
    }
}
