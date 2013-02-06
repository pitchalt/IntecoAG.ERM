using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.AVT {
    public partial class fmCAVTInvoiceRegisterViewController : ObjectViewController {
        public fmCAVTInvoiceRegisterViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            NewObjectViewController ctrl = Frame.GetController<NewObjectViewController>();
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTInvoiceRegister register = e.CurrentObject as fmCAVTInvoiceRegister;
            if (register == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                register = os.GetObject<fmCAVTInvoiceRegister>(register);
                fmCAVTInvoiceRegisterLogic.ImportBuhData(os, register);
                os.CommitChanges();
            }
        }

        private void ImportAvansAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTInvoiceRegister register = e.CurrentObject as fmCAVTInvoiceRegister;
            if (register == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                fmCAVTInvoiceRegisterLogic.ImportAvansData(os, dialog.FileName);
                os.CommitChanges();
            }
        }

        private void ReNumberAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTInvoiceRegister register = e.CurrentObject as fmCAVTInvoiceRegister;
            if (register == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                register = os.GetObject<fmCAVTInvoiceRegister>(register);
                fmCAVTInvoiceRegisterLogic.RegisterLineReNumber(os, register.InLines);
                fmCAVTInvoiceRegisterLogic.RegisterLineReNumber(os, register.OutLines);
                os.CommitChanges();
            }
        }

        private void InvoiceImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTInvoiceRegister register = e.CurrentObject as fmCAVTInvoiceRegister;
            if (register == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                register = os.GetObject<fmCAVTInvoiceRegister>(register);
                fmCAVTInvoiceRegisterLogic.ImportInvoices(os, register, dialog.FileName);
                os.CommitChanges();
            }

        }

    }
}
