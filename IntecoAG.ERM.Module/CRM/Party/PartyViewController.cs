using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;


using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Module.CRM.Party {
    //
    public partial class PartyViewController : ViewController {

        NewObjectViewController wovc = null;
//        Frame frame = null;
        ObjectView newview = null;

        public PartyViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            Frame.ViewChanging += new EventHandler<ViewChangingEventArgs>(Frame_ViewChanging);
//            frame = Frame;
//            if (frame == null) return;
            wovc = Frame.GetController<NewObjectViewController>();
            if (wovc == null) return;
            //
            wovc.CollectCreatableItemTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            wovc.CollectDescendantTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

        }

        void Frame_ViewChanging(object sender, ViewChangingEventArgs e) {
            //throw new NotImplementedException();
            newview = e.View as ObjectView;
        }

        protected override void OnViewChanged() {
            base.OnViewChanged();
            newview = null;
        }

        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
        }

        protected override void OnActivated() {
            base.OnActivated();
            //
            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(NewObjectAction_Execute);
            wovc.NewObjectAction.Executed += new EventHandler<ActionBaseEventArgs>(NewObjectAction_Executed);
            //
            wovc.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
            wovc.CustomAddObjectToCollection += new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
            wovc.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);
            //

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

            base.OnDeactivated();
        }


        #region События

//        private crmCLegalPerson comp = null;
//        private IObjectSpace compos = null;
        /// <summary>
        /// Этот метод позволяет создать другой объект, чем тот, что задаётся выбором пункта меню или подменю New.
        /// Однако, не удаётся пока передать этот объект дальше в метод CustomNewActionController_CustomAddObjectToCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomNewActionController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
//            if (e.ObjectType == typeof(crmLegalPerson)) {
//                compos = e.ObjectSpace;
//                comp = (crmLegalPerson) e.ObjectSpace.CreateObject(e.ObjectType);
//                e.NewObject = comp.Party;
//                e.NewObject = comp;
//            }
            //
        }

        private void CustomNewActionController_CustomAddObjectToCollection(object sender, ProcessNewObjectEventArgs e) {
        }

        private void CustomNewActionController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {
//            e.CreatedObject = comp;
        }
        //
        void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
//            IObjectSpace os = Application.CreateObjectSpace();
//            object obj = os.GetObject(comp);
//            IObjectSpace os2 = e.ShowViewParameters.CreatedView.ObjectSpace;
//            Boolean iseql = os2 == compos;
//              e.ShowViewParameters.CreatedView.CurrentObject = null;
//            os2.Owner = null;

//            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os2, comp);

//            Application.e.ShowViewParameters.CreatedView.
//            Application.Vi
        }
        //
        void NewObjectAction_Executed(object sender, ActionBaseEventArgs e) {
        }

        private void CustomNewActionController_CollectCreatableItemTypes(object sender, CollectTypesEventArgs e) {
            if (newview == null) return;
            if (newview.ObjectTypeInfo.Type != typeof(crmPartyRu)) return;
            e.Types.Add(typeof(crmCBusinessman));
            e.Types.Add(typeof(crmCLegalPerson));
            e.Types.Add(typeof(crmCLegalPersonUnit));
            e.Types.Add(typeof(crmCPhysicalParty));
        }

        private void CustomNewActionController_CollectDescendantTypes(object sender, CollectTypesEventArgs e) {
//            Frame.View.ObjectTypeInfo
//            if (view != null) {
//                if (Frame.View.ObjectTypeInfo.Type == typeof(crmIParty)) {
//                    e.Types.Clear();
//                    e.Types.Add(typeof(crmLegalPerson));
//                    e.Types.Add(typeof(crmLegalPersonUnit));
//                }
//            }
        }
        //
        #endregion

    }
}
