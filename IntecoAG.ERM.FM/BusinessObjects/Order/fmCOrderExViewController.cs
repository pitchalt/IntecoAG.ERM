using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.Order {
    public partial class fmCOrderExViewController : ViewController {
        public fmCOrderExViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            Frame.GetController<StateMachineController>().TransitionExecuted += OnTransitionExecuted;
        }
        void OnTransitionExecuted(object sender, ExecuteTransitionEventArgs e) {
            View.ObjectSpace.CommitChanges();
        }
        protected override void OnDeactivated() {
            Frame.GetController<StateMachineController>().TransitionExecuted -= OnTransitionExecuted;
            base.OnDeactivated();
        }

    }
}
