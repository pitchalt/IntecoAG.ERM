using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {

    // Контроллер, обслуживающий процессы работы с версиями: создание версий, утверждение версий и т.п.

    public partial class VersionServiceController : ViewController {

        static string HideVersionServiceControllerReason = "HideVersionServiceControllerReason";
        static string HideActionReason = "HideActionReason";
        static string ApproveActionShowReason = "ApproveActionShowReason";

        public VersionServiceController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnAfterConstruction() {
            base.OnAfterConstruction();

            //this.VersionApprove.TargetObjectsCriteria = "IsInstanceOfType(This, \'IntecoAG.ERM.CS.IVersionSupport\')";
            //this.CreateNewVersionAction.TargetObjectsCriteria = "IsInstanceOfType(This, \'IntecoAG.ERM.CS.IVersionSupport\')";
        }

        protected override void OnActivated() {
            base.OnActivated();

            Frame frame = Frame;
            View view = View;
            if (view == null) return;

            object currentObject = view.CurrentObject;
            VersionRecord currentVR = currentObject as VersionRecord;

            // <<< Проверка, когда контроллер должен работать
            this.Active[HideVersionServiceControllerReason] = true;

            if (currentObject == null || currentVR == null) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }

            if (Frame.Context.Name == TemplateContext.LookupControl || Frame.Context.Name == TemplateContext.LookupWindow) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }

            if ((frame as NestedFrame) != null | !view.IsRoot) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }

            //if (currentObject as IntecoAG.ERM.CS.IVersionBusinessLogicSupport == null) {
            //    this.Active[HideVersionServiceControllerReason] = false;
            //    return;
            //}

            //object currentObj = view.CurrentObject;

            // >>> Проверка, когда контроллер должен работать
            

            // Кнопка Новая версия
            //this.CreateNewVersionAction.Active.Clear();
            this.CreateNewVersionAction.Active[HideActionReason] = false;

            //if (currentVR.MainObject == null) {
            //    CreateNewVersionAction.Active[HideActionReason] = false;
            //}

            if (currentObject is IVersionBusinessLogicSupport) {
                this.CreateNewVersionAction.Active[HideActionReason] = true;

                if (currentVR.VersionState == VersionStates.VERSION_NEW) {
                    CreateNewVersionAction.Active[HideActionReason] = false;
                }

                //if (currentObj as IntecoAG.ERM.CS.IVersionSupport == null) {
                //    CreateNewVersionAction.Active[HideActionReason] = false;
                //}

            //} else {
            //    this.CreateNewVersionAction.Active[HideActionReason] = false;
            }
            


            // Кнопка Подвердить версию

            // Разбор, когда показывать кнопку
            // Если объект является главным для системы версий и имеется версия, которую можно утвердить

            VersionApprove.Active.Clear();

            if (currentVR.MainObject == null) {
                VersionApprove.Active[ApproveActionShowReason] = false;
                //return;
            }

            if (currentVR.MainObject as IVersionBusinessLogicSupport == null) {
                VersionApprove.Active[ApproveActionShowReason] = false;
                //return;
            }

            if (currentVR.VersionState != VersionStates.VERSION_PROJECT & currentVR.VersionState != VersionStates.VERSION_NEW) {
                VersionApprove.Active[ApproveActionShowReason] = false;
                //return;
            }

        }


        private void VersionApprove_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            // Запускаем метод Approve текущего объекта
            VersionRecord currentObj = View.CurrentObject as VersionRecord;
            if (currentObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не поддерживает операцию утверждения");
                return;
            }

            IVersionBusinessLogicSupport mainObj = currentObj.MainObject as IVersionBusinessLogicSupport;
            if (mainObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не поддерживает операцию утверждения");
                return;
            }

            // Паша: добавил сохранение объекта перед Approve, иначе только что созданная версия не находиться в списке версий:(
            view.ObjectSpace.CommitChanges();
            mainObj.Approve(currentObj);
            view.ObjectSpace.CommitChanges();

            // Прежняя концепция (с SimpleContract и т.д.) предполагала, что главный объект показывается пользователю со списком 
            // его версий. Новая концепция предполагает вместо показа главного объекта всегда показывать его Current версию, а если
            // таковой нет, то версию со статусом NEW.
            // Замечание. Наличие записи сос статусом NEW означает, что Currentу главного объекта и есть эта запись со статусом NEW


            //Type objType = (System.Type)((System.Reflection.MemberInfo)((mainObj).GetType()));
            //string DetailViewID = Application.FindDetailViewId(objType);

            //IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //object passedDisplayedObject = objectSpace.GetObject(mainObj);


            // Основной объект:
            //object DisplayedObject = currentObj.MainObject;
            object DisplayedObject = (mainObj as crmContractDeal).Current;

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(DisplayedObject.GetType());

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            object passedDisplayedObject = objectSpace.GetObject(DisplayedObject);

            // Показываем
            //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, passedDisplayedObject, TargetWindow.Current);

            TargetWindow openMode = TargetWindow.Current;
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedDisplayedObject);

            ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };

            e.ShowViewParameters.Assign(svp);
        }

        private void CreateNewVersionAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            object currentObj = View.CurrentObject;
            IVersionBusinessLogicSupport currentVersObj = View.CurrentObject as IVersionBusinessLogicSupport;
            if (currentVersObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не поддерживает создание версий", "Система версионирования документов");
                return;
            }

            this.ObjectSpace.CommitChanges();

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            IVersionBusinessLogicSupport passedCurrentObj = objectSpace.GetObject(currentObj) as IVersionBusinessLogicSupport;

            // Новая версия:
            IVersionSupport newVers = passedCurrentObj.CreateNewVersion();
            //IVersionSupport newVers = ((IVersionBusinessLogicSupport)currentObj).CreateNewVersion();
            //objectSpace.CommitChanges();


            // Показ новой версии
            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newVers.GetType());

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newVers, openMode);

            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newVers);
            //DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedCurrentObj);
            //DetailView dv = frame.Application.CreateDetailView(objectSpace, newVers, true);
            //DetailView dv = frame.Application.CreateDetailView(objectSpace, newVers, true);

            ShowViewParameters svp = new ShowViewParameters() { CreatedView = dv, TargetWindow = openMode, Context = TemplateContext.View, CreateAllControllers = true };

            e.ShowViewParameters.Assign(svp);

            ////object passedNewObj = this.ObjectSpace.GetObject(newVers);
            //DetailView dv = frame.Application.CreateDetailView(this.ObjectSpace, DetailViewId, true, newVers);
            //e.ShowViewParameters.CreatedView = dv;
            ////e.ShowViewParameters.CreatedView.CurrentObject = newVers;  // passedNewObj;

            
        }
    }
}
