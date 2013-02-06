using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.AVT {
    public partial class fmCAVTBookVATViewController : ViewController {
        public fmCAVTBookVATViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmCAVTBookVAT book = e.CurrentObject as fmCAVTBookVAT;
            if (book == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                fmCAVTBookVATLogic.ImportBuhData(os, os.GetObject<fmCAVTBookVAT>(book));
                os.CommitChanges();
            }
        }
    }
}
