using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Order {
    public partial class fmCOrderExViewController : ViewController {
        public fmCOrderExViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            Frame.GetController<StateMachineController>().TransitionExecuted += OnTransitionExecuted;
//            Frame.GetController<StateMachineController>().TransitionExecuting += OnTransitionExecuting;
        }
        void OnTransitionExecuting(object sender, ExecuteTransitionEventArgs e) {
            fmCOrderExt order = e.TargetObject as fmCOrderExt;
            if (order != null) {

            }
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
