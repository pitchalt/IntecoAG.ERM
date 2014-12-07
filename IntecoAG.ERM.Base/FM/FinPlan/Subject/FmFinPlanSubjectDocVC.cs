using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.FinPlan.Subject {
    public partial class FmFinPlanSubjectDocVC : ViewController {
        public FmFinPlanSubjectDocVC() {
            InitializeComponent();
            RegisterActions(components);
//            TargetObjectType = typeof(FmFinPlanSubjectDoc);
//            TargetViewType = ViewType.DetailView;
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e) {

        }
    }
}
