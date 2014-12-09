using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.FinPlan.Subject {
    public partial class FmFinPlanSubjectDocVC : ViewController {
        public FmFinPlanSubjectDocVC() {
            InitializeComponent();
            RegisterActions(components);
            TargetObjectType = typeof(FmFinPlanSubjectDoc);
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }

        private NewObjectViewController NewObjectController;

        protected override void OnActivated() {
            base.OnActivated();
            NewObjectController = Frame.GetController<NewObjectViewController>();
            if (NewObjectController != null) {
                //            NewObjectController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(NewObjectController_ObjectCreated);
                NewObjectController.NewObjectAction.Executed += new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
                NewObjectController.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(NewObjectAction_Execute);
            }
        }

        void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
//            e.ShowViewParameters.CreatedView
//            throw new NotImplementedException();
        }

        void NewObjectAction_Executed(object sender, ActionBaseEventArgs e) {
//            e.ShowViewParameters.CreatedView.
            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
//            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
        }

        //void NewObjectController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {
        //}

        protected override void OnDeactivated() {
            if (NewObjectController != null) {
                //            NewObjectController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(NewObjectController_ObjectCreated);
                NewObjectController.NewObjectAction.Executed -= new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
            }
            NewObjectController = null;
            base.OnDeactivated();
        }
    }
}
