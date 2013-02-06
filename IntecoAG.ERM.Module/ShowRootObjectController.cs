using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class ShowRootObjectController : ViewController {

        string HideActionReason = "HideGotoMainObjectActionReason";

        public ShowRootObjectController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();

            View view = View;
            if (view == null) return;

            VersionRecord currentObject = view.CurrentObject as VersionRecord;

            this.GotoMainAction.Active.Clear();
            if (currentObject != null && currentObject.MainObject != null) {
                this.GotoMainAction.Active[HideActionReason] = true;
            } else {
                this.GotoMainAction.Active[HideActionReason] = false;
            }
        }


        private void GotoMainAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            VersionRecord currentObj = View.CurrentObject as VersionRecord;
            if (currentObj == null || currentObj.MainObject == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не найден основной объект для версии", "Система версионирования документов");
                return;
            }

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            VersionRecord passedCurrentObj = objectSpace.GetObject(currentObj) as VersionRecord;

            // Основной объект:
            object mainObject = passedCurrentObj.MainObject;

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(mainObject.GetType());

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, mainObject, openMode);
        }

    }
}
