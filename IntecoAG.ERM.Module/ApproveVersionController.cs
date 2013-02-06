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
    public partial class ApproveVersionController : ViewController {
        public ApproveVersionController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void SimpleContractApproveVersion_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;
            // Запускаем метод текущего объекта, который создаёт новую версию
            SimpleContractVersion currentObj = (SimpleContractVersion)View.CurrentObject;
            currentObj.SimpleContract.ApproveVersion(currentObj);
            view.ObjectSpace.CommitChanges();
            //currentObj.Session.FlushChanges();
            //view.ObjectSpace.ReloadObject(currentObj);
            //view.ObjectSpace.Refresh();
            //view.Refresh();
            
            // Показываем этот новый объект
            //ShowConcreteDetailView<SimpleContract>("SimpleContract_DetailView", currentObj.SimpleContract);
            //ShowConcreteDetailView<SimpleContract>(frame, "SimpleContract_DetailView", currentObj.SimpleContract, TargetWindow.Current);
            //ShowConcreteShortCutDetailView<SimpleContract>(frame, currentObj.SimpleContract, TargetWindow.Current);

            SetConcreteDetailView<SimpleContract>(frame, currentObj.SimpleContract, "SimpleContract_DetailView");

        }


        private void ComplexContractApproveVersion_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;
            // Запускаем метод текущего объекта, который создаёт новую версию
            ComplexContractVersion currentObj = (ComplexContractVersion)View.CurrentObject;
            currentObj.ComplexContract.ApproveVersion(currentObj);
            View.ObjectSpace.CommitChanges();

            // Показываем этот новый объект
            ////ShowConcreteDetailView<ComplexContract>("ComplexContract_DetailView", currentObj.ComplexContract);
            //ShowConcreteDetailView<ComplexContract>(frame, "ComplexContract_DetailView", currentObj.ComplexContract, TargetWindow.Current);
            //view.ObjectSpace.CommitChanges();

            SetConcreteDetailView<ComplexContract>(frame, currentObj.ComplexContract, "ComplexContract_DetailView");

        }


        private void WorkPlanApproveVersion_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;
            // Запускаем метод текущего объекта, который создаёт новую версию
            WorkPlanVersion currentObj = (WorkPlanVersion)View.CurrentObject;
            currentObj.WorkPlan.ApproveVersion(currentObj);
            View.ObjectSpace.CommitChanges();

            // Показываем этот новый объект
            ////ShowConcreteDetailView<WorkPlan>("WorkPlan_DetailView", currentObj.WorkPlan);
            //ShowConcreteDetailView<WorkPlan>(frame, "WorkPlan_DetailView", currentObj.WorkPlan, TargetWindow.Current);
            //view.ObjectSpace.CommitChanges();

            SetConcreteDetailView<WorkPlan>(frame, currentObj.WorkPlan, "WorkPlan_DetailView");

        }


        #region Методы показа View

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteListView<classType>(Frame frame) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //SimpleContract objCurrent1 = objectSpace1.GetObject<SimpleContract>((SimpleContract)currentObject);
            ListView lv = frame.Application.CreateListView(objectSpace, typeof(classType), true);
            frame.SetView(lv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void SetConcreteDetailView<classType>(Frame frame, classType Obj, string DeatilViewId) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DeatilViewId, true, objCurrent);
            frame.SetView(dv, frame);
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteShortCutDetailView<classType>(Frame frame, classType Obj, TargetWindow tw) {
            BaseObject obj = Obj as BaseObject;
            if (obj == null) return;

            ShowViewParameters svp = new ShowViewParameters();
            ViewShortcut shortcut = new ViewShortcut(frame.Application.FindDetailViewId(typeof(classType)), obj.Oid);
            svp.CreatedView = frame.Application.ProcessShortcut(shortcut);

            //Specify various display settings.
            svp.TargetWindow = tw;   // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }

        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(Frame frame, string DetailViewID, classType Obj, TargetWindow tw) {
            IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = frame.View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(Obj);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;

            //Specify various display settings.
            svp.TargetWindow = tw;  // TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
        }


        /// <summary>
        /// Загрузка указанного параметром типа ListView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteListView<classType>(Frame frame, TargetWindow tw) {
            if (frame.Application == null) return;
            IObjectSpace objectSpaceList = frame.Application.CreateObjectSpace();
            ListView lv = frame.Application.CreateListView(objectSpaceList, typeof(classType), true);
            
            ShowViewParameters svpList = new ShowViewParameters();
            svpList.CreatedView = lv;

            //Specify various display settings.
            svpList.TargetWindow = tw;   // TargetWindow.Current;
            svpList.Context = TemplateContext.View;
            svpList.CreateAllControllers = true;
            Application.ShowViewStrategy.ShowView(svpList, new ShowViewSource(frame, null));
        }

        #endregion

    }
}
