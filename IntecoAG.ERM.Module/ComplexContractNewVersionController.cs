using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class ComplexContractNewVersionController : ViewController {
        public ComplexContractNewVersionController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void SimpleContractNewVersion_Execute(object sender, SimpleActionExecuteEventArgs e) {
            // Запускаем метод текущего объекта, который создаёт новую версию
            ComplexContract currentObj = (ComplexContract)View.CurrentObject;
            ComplexContractVersion newVers = currentObj.CreateNewVersion();
            View.ObjectSpace.CommitChanges();

            // Показываем этот новый объект
            ShowConcreteDetailView<ComplexContractVersion>("ComplexContractVersion_DetailView", newVers);
        }


        /// <summary>
        /// Загрузка указанного параметром DetailView
        /// </summary>
        /// <param name="DetailViewID"></param>
        private void ShowConcreteDetailView<classType>(string DetailViewID, classType but) {
            IObjectSpace objectSpace = Frame.Application.CreateObjectSpace();
            //IObjectSpace objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            classType objCurrent = objectSpace.GetObject<classType>(but);
            if (objCurrent == null) objCurrent = objectSpace.CreateObject<classType>();

            //TaskInvoiceInstanceDefinition objTaskInvoiceInstanceDefinition = objectSpace.CreateObject<TaskInvoiceInstanceDefinition>();
            DetailView dv = Frame.Application.CreateDetailView(objectSpace, DetailViewID, true, objCurrent);
            //ListView dv = Frame.Application.CreateListView(objectSpace, typeof(TaskInvoiceInstanceDefinition), true);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            //Specify various display settings.
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            // Here we show our detail view.
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
        }

    }
}
