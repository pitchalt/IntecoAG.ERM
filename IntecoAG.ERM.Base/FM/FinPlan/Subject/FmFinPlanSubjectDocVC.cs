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

    public partial class FmFinPlanSubjectDocVC : ObjectViewController {
    
        public FmFinPlanSubjectDocVC() {
            InitializeComponent();
            RegisterActions(components);
//            TargetObjectType = typeof(FmFinPlanSubjectDoc);
//            TargetViewType = ViewType.ListView;
//            TargetViewNesting = Nesting.Nested;
        }

        //private NewObjectViewController NewObjectController;
        //private ListViewProcessCurrentObjectController ListViewProcessCurrentObjectController;

        protected override void OnActivated() {
            base.OnActivated();
//            NewObjectController = Frame.GetController<NewObjectViewController>();
//            ListViewProcessCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
            //if (ListViewProcessCurrentObjectController != null) {
            //    ListViewProcessCurrentObjectController.ProcessCurrentObjectAction.Executed += new EventHandler<ActionBaseEventArgs>(ProcessCurrentObjectAction_Executed);
            //}
            //if (NewObjectController != null) {
            //    //            NewObjectController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(NewObjectController_ObjectCreated);
            //    NewObjectController.NewObjectAction.Executed += new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
            //    NewObjectController.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(NewObjectAction_Execute);
            //}
        }

//        void ProcessCurrentObjectAction_Executed(object sender, ActionBaseEventArgs e) {
//            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
//            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
//        }

//        void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
//            View View = e.ShowViewParameters.CreatedView;
////            throw new NotImplementedException();
//        }

//        void NewObjectAction_Executed(object sender, ActionBaseEventArgs e) {
////            e.ShowViewParameters.CreatedView.
//            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
//            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
//            View View = e.ShowViewParameters.CreatedView;
////            e.ShowViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
//        }

        //void NewObjectController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {
        //}

        protected override void OnDeactivated() {
            //if (NewObjectController != null) {
            //    NewObjectController.NewObjectAction.Executed -= new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
            //}
            //if (ListViewProcessCurrentObjectController != null) {
            //    ListViewProcessCurrentObjectController.ProcessCurrentObjectAction.Executed -= new EventHandler<ActionBaseEventArgs>(ProcessCurrentObjectAction_Executed);
            //}
            //NewObjectController = null;
            base.OnDeactivated();
        }

        private void TransactAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            FmFinPlanSubjectDocFull doc = e.CurrentObject as FmFinPlanSubjectDocFull;
            if (doc == null)
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                doc = os.GetObject<FmFinPlanSubjectDocFull>(doc);
                FmFinPlanSubjectDocFullLogic.ReMakeOperations(os, doc);
                os.CommitChanges();
            }
        }

        private void ConfirmAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            FmFinPlanSubjectDocFull doc = e.CurrentObject as FmFinPlanSubjectDocFull;
            if (doc == null)
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                doc = os.GetObject<FmFinPlanSubjectDocFull>(doc);
                FmFinPlanSubjectDocFullLogic.TransactToSubject(os, doc);
                os.CommitChanges();
            }
        }

    }
}
