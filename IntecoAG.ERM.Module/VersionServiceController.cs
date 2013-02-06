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

    // ����������, ������������� �������� ������ � ��������: �������� ������, ����������� ������ � �.�.

    public partial class VersionServiceController : ViewController {

        static string HideVersionServiceControllerReason = "HideVersionServiceControllerReason";
        static string HideActionReason = "HideActionReason";
        static string ApproveActionShowReason = "ApproveActionShowReason";

        public VersionServiceController() {
            InitializeComponent();
            RegisterActions(components);
        }


        protected override void OnActivated() {
            base.OnActivated();

            Frame frame = Frame;
            View view = View;
            if (view == null) return;

            // <<< ��������, ����� ���������� ������ ��������
            this.Active[HideVersionServiceControllerReason] = true;

            if (Frame.Context.Name == TemplateContext.LookupControl || Frame.Context.Name == TemplateContext.LookupWindow) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }

            if ((frame as NestedFrame) != null | !view.IsRoot) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }

            object currentObj = view.CurrentObject;
            VersionRecord currentVR = currentObj as VersionRecord;

            if (currentObj == null || currentVR == null) {
                this.Active[HideVersionServiceControllerReason] = false;
                return;
            }
            // >>> ��������, ����� ���������� ������ ��������

            // ������ ����� ������
            this.CreateNewVersionAction.Active.Clear();
            if (view.CurrentObject is IVersionBusinessLogicSupport) {
                this.CreateNewVersionAction.Active[HideActionReason] = true;

                if (currentVR.VersionState == VersionStates.VERSION_NEW) {
                    CreateNewVersionAction.Active[HideActionReason] = false;
                }

            } else {
                this.CreateNewVersionAction.Active[HideActionReason] = false;
            }


            // ������ ���������� ������

            // ������, ����� ���������� ������
            // ���� ������ �������� ������� ��� ������� ������ � ������� ������, ������� ����� ���������

            VersionApprove.Active.Clear();

            if (currentObj == null) {
                VersionApprove.Active[ApproveActionShowReason] = false;
                //return;
            }

            if (currentVR == null) return;

            if (currentVR == null) {
                VersionApprove.Active[ApproveActionShowReason] = false;
                //return;
            }

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

            // ��������� ����� Approve �������� �������
            VersionRecord currentObj = View.CurrentObject as VersionRecord;
            if (currentObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("�������� �� ������������ �������� �����������");
                return;
            }

            IVersionBusinessLogicSupport mainObj = currentObj.MainObject as IVersionBusinessLogicSupport;
            if (mainObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("�������� �� ������������ �������� �����������");
                return;
            }

            // ����: ������� ���������� ������� ����� Approve, ����� ������ ��� ��������� ������ �� ���������� � ������ ������:(
            view.ObjectSpace.CommitChanges();
            mainObj.Approve(currentObj);
            view.ObjectSpace.CommitChanges();

            // ������� ��������� (� SimpleContract � �.�.) ������������, ��� ������� ������ ������������ ������������ �� ������� 
            // ��� ������. ����� ��������� ������������ ������ ������ �������� ������� ������ ���������� ��� Current ������, � ����
            // ������� ���, �� ������ �� �������� NEW.
            // ���������. ������� ������ ��� �������� NEW ��������, ��� Current� �������� ������� � ���� ��� ������ �� �������� NEW


            //Type objType = (System.Type)((System.Reflection.MemberInfo)((mainObj).GetType()));
            //string DetailViewID = Application.FindDetailViewId(objType);

            //IObjectSpace objectSpace = frame.Application.CreateObjectSpace();
            //object passedDisplayedObject = objectSpace.GetObject(mainObj);


            // �������� ������:
            //object DisplayedObject = currentObj.MainObject;
            object DisplayedObject = (mainObj as crmContractDeal).Current;

            // ���������� DetailView
            string DetailViewId = frame.Application.FindDetailViewId(DisplayedObject.GetType());

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            object passedDisplayedObject = objectSpace.GetObject(DisplayedObject);

            // ����������
            //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, passedDisplayedObject, TargetWindow.Current);

            TargetWindow openMode = TargetWindow.Current;
            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, passedDisplayedObject);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = openMode;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;

            e.ShowViewParameters.Assign(svp);
        }

        private void CreateNewVersionAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View view = View;
            Frame frame = Frame;

            object currentObj = View.CurrentObject;
            IVersionBusinessLogicSupport currentVersObj = View.CurrentObject as IVersionBusinessLogicSupport;
            if (currentVersObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("�������� �� ������������ �������� ������", "������� ��������������� ����������");
                return;
            }

            this.ObjectSpace.CommitChanges();

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            IVersionBusinessLogicSupport passedCurrentObj = objectSpace.GetObject(currentObj) as IVersionBusinessLogicSupport;

            // ����� ������:
            IVersionSupport newVers = passedCurrentObj.CreateNewVersion();

            // ���������� DetailView
            string DetailViewId = frame.Application.FindDetailViewId(newVers.GetType());

            // ����������:
            TargetWindow openMode = TargetWindow.Current;
            //CommonMethods.ShowConcreteDetailViewInWindow(frame, objectSpace, DetailViewId, newVers, openMode);


            DetailView dv = frame.Application.CreateDetailView(objectSpace, DetailViewId, true, newVers);

            ShowViewParameters svp = new ShowViewParameters();
            svp.CreatedView = dv;
            svp.TargetWindow = openMode;
            svp.Context = TemplateContext.View;
            svp.CreateAllControllers = true;

            e.ShowViewParameters.Assign(svp);
        }
    }
}
