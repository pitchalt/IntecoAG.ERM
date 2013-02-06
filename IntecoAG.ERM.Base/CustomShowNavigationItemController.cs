using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;

namespace IntecoAG.ERM.Module {
    public partial class CustomShowNavigationItemController : ShowNavigationItemController {
        public CustomShowNavigationItemController() {
            InitializeComponent();
            RegisterActions(components);
        }

        string ViewId = "";
        //bool isPers = true;
        DetailView dv = null;

        
        protected override void OnActivated() {
            base.OnActivated();
            //Application.CustomProcessShortcut +=new EventHandler<CustomProcessShortcutEventArgs>(Application_CustomProcessShortcut);
        }

        private void Application_CustomProcessShortcut(object sender, CustomProcessShortcutEventArgs args) {
            //if (!isPers) {
            //    args.View = dv;
            //    args.Handled = true;
            //}
        }

        protected override void OnDeactivated() {
            //Application.CustomProcessShortcut += new EventHandler<CustomProcessShortcutEventArgs>(Application_CustomProcessShortcut);
            base.OnDeactivated();
        }
        
        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs args) {
            if (args.SelectedChoiceActionItem.Data == null) {
                base.ShowNavigationItem(args);
                return;
            }
            ViewId = ((DevExpress.ExpressApp.ViewShortcut)(args.SelectedChoiceActionItem.Data)).ViewId;

            if (string.IsNullOrEmpty(ViewId)) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не найдена форма для представления объекта");
                return;
            }

            IModelDetailView modelDetailView = Application.FindModelView(ViewId) as IModelDetailView;
            if (modelDetailView == null) {
                //isPers = true;
                base.ShowNavigationItem(args);
                return;
            }

            // Знание, является ли объект NonPersisten пока не нужно
            //isPers = ((IModelDetailView)modelDetailView).AsObjectView.ModelClass.TypeInfo.IsPersistent;


            /*
            // Случай, когда NonPersistent - объект создаём самостоятельно
            Frame frame = Frame;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            Type nonPersistentType = ((((((IModelNavigationItem)(args.SelectedChoiceActionItem.Model)).View).AsObjectView).ModelClass).TypeInfo).Type;
            object newObj = objectSpace.CreateObject(nonPersistentType);

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            dv = frame.Application.CreateDetailView(objectSpace, ViewId, true, newObj);
            //dv = frame.Application.CreateDetailView(objectSpace, newObj);
            //frame.SetView(dv);

            //DetailView dv = frame.Application.CreateDetailView(objectSpace, newObj, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            args.ShowViewParameters.Context = TemplateContext.View;
            args.ShowViewParameters.CreateAllControllers = true;
            args.ShowViewParameters.TargetWindow = openMode;
            args.ShowViewParameters.CreatedView = dv;
            base.ShowNavigationItem(args);
            */


            
            // Показываем NonPersistent объект - ошибка!!!
            Frame frame = Frame;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            Type nonPersistentType = ((((((IModelNavigationItem)(args.SelectedChoiceActionItem.Model)).View).AsObjectView).ModelClass).TypeInfo).Type;
            object newObj = objectSpace.CreateObject(nonPersistentType);

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            dv = frame.Application.CreateDetailView(objectSpace, ViewId, true, newObj);
            //dv = frame.Application.CreateDetailView(objectSpace, newObj);
            //frame.SetView(dv);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = openMode;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, this.ShowNavigationItemAction));
            

        
            /*
            // Показываем обычный Persistent объект - та же ошибка!!!
            Frame frame = Frame;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            Type nonPersistentType = typeof(IntecoAG.ERM.CS.Country.csCountry);
            object newObj = objectSpace.CreateObject(nonPersistentType);

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            dv = frame.Application.CreateDetailView(objectSpace, "csCountry_DetailView", true, newObj);
            //dv = frame.Application.CreateDetailView(objectSpace, newObj);
            //frame.SetView(dv);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = openMode;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;
            frame.Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(frame, null));
            */
        }

    }
}
