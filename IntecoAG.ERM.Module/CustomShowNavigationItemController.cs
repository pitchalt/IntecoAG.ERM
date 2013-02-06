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

            // ����������, �������� ������ NonPersistent � ����� ������� �� ������������ - ������� ��� ������
            
            // View Id:
            string ViewId = ((DevExpress.ExpressApp.ViewShortcut)(args.SelectedChoiceActionItem.Data)).ViewId;

            if (string.IsNullOrEmpty(ViewId)) {
                DevExpress.XtraEditors.XtraMessageBox.Show("�� ������� ����� ��� ������������� �������");
                return;
            }

            IModelDetailView modelDetailView = Application.FindModelView(ViewId) as IModelDetailView;
            //IModelListView modelListView = Application.FindModelView(ViewId) as IModelListView;

            // ���� �� DetailView
            if (modelDetailView == null) {
                base.ShowNavigationItem(args);
                return;
            }

            // ���� DatailView, ��������� �� Persistent
            bool isPers = ((IModelDetailView)modelDetailView).AsObjectView.ModelClass.TypeInfo.IsPersistent;

            if (isPers) {
                base.ShowNavigationItem(args);
                return;
            }


            // ������, ����� NonPersistent - ������ ������ ��������������
            Frame frame = Frame;
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            // ���������� ���
            Type nonPersistentType = ((((((IModelNavigationItem)(args.SelectedChoiceActionItem.Model)).View).AsObjectView).ModelClass).TypeInfo).Type;
            object newObj = objectSpace.CreateObject(nonPersistentType);

            // ���������� DetailView
            string DetailViewId = frame.Application.FindDetailViewId(nonPersistentType);   //newObj.GetType());

            // ����������:
            TargetWindow openMode = TargetWindow.Current;
            CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newObj, openMode);
        }

    }
}
