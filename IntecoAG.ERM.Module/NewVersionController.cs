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
    public partial class NewVersionController : ViewController {

        string HideActionReason = "HideActionReason";

        public NewVersionController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            
            View view = View;
            if (view == null) return;
            
            this.CreateNewVersionAction.Active.Clear();
            if (view.CurrentObject is INewVersionSupport) {
                this.CreateNewVersionAction.Active[HideActionReason] = true;
            } else {
                this.CreateNewVersionAction.Active[HideActionReason] = false;
            }
        }

        
        private void CreateNewVersionAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            object currentObj = View.CurrentObject;
            INewVersionSupport currentVersObj = View.CurrentObject as INewVersionSupport;
            if (currentVersObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не поддерживает создание версий", "Система версионирования документов");
                return;
            }

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            INewVersionSupport passedCurrentObj = objectSpace.GetObject(currentObj) as INewVersionSupport; 

            // Новая версия:
            IVersionSupport newVers = passedCurrentObj.CreateNewVersion();

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newVers.GetType());

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newVers, openMode);
        }
        
/*
        private void CreateNewVersionAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            object currentObj = View.CurrentObject;
            INewVersionSupport currentVersObj = View.CurrentObject as INewVersionSupport;
            if (currentVersObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не поддерживает создание версий", "Система версионирования документов");
                return;
            }

            // Новая версия:
            IVersionSupport newVers = ((INewVersionSupport)currentObj).CreateNewVersion();

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newVers.GetType());
            TargetWindow openMode = TargetWindow.Current;

            View.SetInfo(
            frame.SetView(frame.Application.CreateDetailView(view.ObjectSpace, DetailViewId, true, newVers));
            //e.ShowViewParameters.CreatedView = frame.Application.CreateDetailView(view.ObjectSpace, DetailViewId, true, newVers);
            e.ShowViewParameters.TargetWindow = openMode;

            // Показываем:
            //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newVers, openMode);
        }
        */
    }
}
