using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
//using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM {
    //
    public partial class fmComponentViewController : ViewController<ObjectView> {

        NewObjectViewController wovc = null;

        public fmComponentViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            wovc = Frame.GetController<NewObjectViewController>();
            if (wovc == null) return;
            //
            wovc.CollectCreatableItemTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

        }

        protected override void OnViewChanged() {
            base.OnViewChanged();
            //newview = null;
        }

        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
        }

        protected override void OnActivated() {
            base.OnActivated();
            //
            wovc.NewObjectAction.Executing += new CancelEventHandler(NewObjectAction_Executing);
            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(NewObjectAction_Execute);
            wovc.NewObjectAction.Executed += new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
            //
            wovc.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
            wovc.CustomAddObjectToCollection += new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
            wovc.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            //
            //
            BaseObject obj = View.CurrentObject as BaseObject;
            if (obj != null) {
                ViewReadOnlyStatusUpdate();
                obj.Changed += new DevExpress.Xpo.ObjectChangeEventHandler(CurrentObject_Changed);
            }
        }


        void ViewReadOnlyStatusUpdate() { 
            csIComponent comp = View.CurrentObject as csIComponent;
            if (comp != null) 
                View.AllowEdit.SetItemValue("fmComponentViewController.csIComponent.ReadOnly", !comp.ReadOnly);
        }

        void CurrentObject_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e) {
            if (e.PropertyName == "ReadOnly") { 
                ViewReadOnlyStatusUpdate();
            }
        }

        protected override void OnDeactivated() {
            if (wovc == null) {
                base.OnDeactivated();
                return;
            }
            wovc.CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes -= new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);
            //
            wovc.NewObjectAction.Execute -= new SingleChoiceActionExecuteEventHandler(NewObjectAction_Execute);
            //
            wovc.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
            wovc.CustomAddObjectToCollection -= new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
            wovc.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            //
            BaseObject obj = View.CurrentObject as BaseObject;
            if (obj != null) {
                obj.Changed -= new DevExpress.Xpo.ObjectChangeEventHandler(CurrentObject_Changed);
            }

            base.OnDeactivated();
        }


        #region События

//        private csCComponent comp;
//        private IObjectSpace os;

        private void CustomNewActionController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            if (e.ObjectType == typeof(fmIDirection)) {
                e.NewObject = e.ObjectSpace.CreateObject<fmCDirection>();
            }
            if (e.ObjectType == typeof(fmISubjectExt)) {
                e.NewObject = e.ObjectSpace.CreateObject<fmCSubjectExt>();
            }
            if (e.ObjectType == typeof(fmIOrderExt)) {
                e.NewObject = e.ObjectSpace.CreateObject<fmCOrderExt>();
            }
        }
        //
        private void CustomNewActionController_CustomAddObjectToCollection(object sender, ProcessNewObjectEventArgs e) {
        }
        //
        private void CustomNewActionController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {
        }
        //
        void NewObjectAction_Executing(object sender, CancelEventArgs e) {

        }
        //
        void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
//            if (e.ObjectType == typeof(fmCDirection))
            //if (comp != null) {
            //    e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, "fmIDirection_DetailView", true, comp);
            //    comp = null;
            //    os = null;
            //}

        }
        //
        void NewObjectAction_Executed(object sender, ActionBaseEventArgs e) {
        }
        // Сборка типов 
        private void CustomNewActionController_CollectCreatableItemTypes(object sender, CollectTypesEventArgs e) {

        }

        private void CustomNewActionController_CollectDescendantTypes(object sender, CollectTypesEventArgs e) {
            if (Frame.View.ObjectTypeInfo.Type == typeof(fmIDirection)) {
                e.Types.Clear();
                e.Types.Add(typeof(fmIDirection));
            }
            if (Frame.View.ObjectTypeInfo.Type == typeof(fmISubject)) {
                e.Types.Clear();
                e.Types.Add(typeof(fmISubjectExt));
            }
            if (Frame.View.ObjectTypeInfo.Type == typeof(fmIOrder)) {
                e.Types.Clear();
                e.Types.Add(typeof(fmIOrderExt));
            }
        }
        //
        #endregion

    }
}
