using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;

namespace IntecoAG.ERM.Module {
    public partial class CustomNewActionViewController : ViewController {

        NewObjectViewController wovc = null;
        Frame frame = null;
        View view = null;

        public CustomNewActionViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            frame = Frame;
            if (frame == null) return;
            view = View;
            if (view == null) return;

            wovc = Frame.GetController<NewObjectViewController>();
            if (wovc == null) return;

            //wovc.CollectCreatableItemTypes += new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            //wovc.CollectDescendantTypes += new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

            wovc.NewObjectAction.Execute += new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);

            wovc.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
            wovc.CustomAddObjectToCollection += new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
            wovc.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);

            // Настройка способа реакции на кнопке
            wovc.NewObjectAction.ShowItemsOnClick = !(wovc.NewObjectAction.Items.Count < 2);
        }

        protected override void OnDeactivated() {
            if (wovc == null) {
                base.OnDeactivated();
                return;
            }
            //wovc.CollectCreatableItemTypes -= new EventHandler<CollectTypesEventArgs>(CustomNewActionController_CollectCreatableItemTypes);
            //wovc.CollectDescendantTypes -= new EventHandler<DevExpress.ExpressApp.SystemModule.CollectTypesEventArgs>(CustomNewActionController_CollectDescendantTypes);

            wovc.NewObjectAction.Execute -= new SingleChoiceActionExecuteEventHandler(CustomNewActionController_Execute);

            wovc.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(CustomNewActionController_ObjectCreating);
            wovc.CustomAddObjectToCollection -= new EventHandler<ProcessNewObjectEventArgs>(CustomNewActionController_CustomAddObjectToCollection);
            wovc.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(CustomNewActionController_ObjectCreated);

            base.OnDeactivated();
        }


        #region События

        /// <summary>
        /// Этот метод позволяет создать другой объект, чем тот, что задаётся выбором пункта меню или подменю New.
        /// Однако, не удаётся пока передать этот объект дальше в метод CustomNewActionController_CustomAddObjectToCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomNewActionController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            foreach (Type type in e.ObjectType.GetInterfaces()) {
                if (type == typeof(IVersionSupport)) {
                    e.NewObject = e.ObjectSpace.CreateObject(e.ObjectType);
                    IVersionSupport vobj = e.NewObject as IVersionSupport;
                    if (vobj != null) {
                        //!!!Паша нужно правильно определить vobj.VersionState сейчас заглушка
                        vobj.VersionState = VersionStates.VERSION_NEW;
                        vobj.VersionAfterConstruction();
                    }
                }
            }
        }

        private void CustomNewActionController_CustomAddObjectToCollection(object sender, ProcessNewObjectEventArgs e) {

        }

        private void CustomNewActionController_ObjectCreated(object sender, ObjectCreatedEventArgs e) {

        }

        void CustomNewActionController_Execute(object sender, ActionBaseEventArgs e) {
            if (!View.IsRoot) { 
                if (View is ListView) {
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                }
            }
        }

        #endregion

    }
}
