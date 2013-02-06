using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Module {
    public partial class CheckChoiceActionForPivotGridController : ViewController {
        public CheckChoiceActionForPivotGridController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void singleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            throw new Exception(System.DateTime.Now.ToString());
        }
    }
}
