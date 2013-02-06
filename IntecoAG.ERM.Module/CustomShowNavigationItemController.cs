using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    public partial class CustomShowNavigationItemController : ShowNavigationItemController {
        public CustomShowNavigationItemController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs args) {

            // Определяем, является объект NonPersistent и каким образом он показывается - списком или формой
            
            // View Id:
            string ViewId = ((DevExpress.ExpressApp.ViewShortcut)(args.SelectedChoiceActionItem.Data)).ViewId;

            if (string.IsNullOrEmpty(ViewId)) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не найдена форма для представления объекта");
                return;
            }

            IModelDetailView modelDetailView = Application.FindModelView(ViewId) as IModelDetailView;
            //IModelListView modelListView = Application.FindModelView(ViewId) as IModelListView;

            // Если не DetailView
            if (modelDetailView == null) {
                base.ShowNavigationItem(args);
                return;
            }

            // Если DatailView, проверяем на Persistent
            bool isPers = (((((IModelDetailView)modelDetailView).AsObjectView).ModelClass).TypeInfo).IsPersistent;

            if (isPers) {
                base.ShowNavigationItem(args);
                return;
            }


            // Случай, когда NonPersistent - объект создаём самостоятельно
            Frame frame = Frame;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            crmContractNewForm newObj = objectSpace.CreateObject<crmContractNewForm>();

            // Определяем DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newObj.GetType());

            // Показываем:
            TargetWindow openMode = TargetWindow.Current;
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);


            /*
            // "@1a9219ab-d0f9-41e3-ad69-4d887bfa934c"
            if (args.SelectedChoiceActionItem.Id.ToLower() == "@1a9219ab-d0f9-41e3-ad69-4d887bfa934c".ToLower()) {
                Frame frame = Frame;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                crmContractNewForm newObj = objectSpace.CreateObject<crmContractNewForm>();

                // Определяем DetailView
                string DetailViewId = frame.Application.FindDetailViewId(newObj.GetType());
                
            
                // Показываем:
                TargetWindow openMode = TargetWindow.Current;
                CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);
            } else {
                base.ShowNavigationItem(args);
            }
            */
        }

    }
}
