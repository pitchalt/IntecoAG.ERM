using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Budget {
    public partial class TrwBudgetVC : ViewController {
        public TrwBudgetVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void CalculateAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            TrwBudgetBase budget = e.CurrentObject as TrwBudgetBase;
            if (budget == null)
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                budget = os.GetObject<TrwBudgetBase>(budget);
                budget.Calculate(os);
                os.CommitChanges();
            }
        }
    }
}
