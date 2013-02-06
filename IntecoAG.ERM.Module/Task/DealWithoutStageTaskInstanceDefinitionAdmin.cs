using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

using System.Linq;

using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// ����� ��� ����������� � �������������� ���������� ����������� ���� ����� ��� ����������� ���� ������-��������
    /// � �������� ��� ���������������� ��������� � ������� ������
    /// ���� ����� ������ ��������������� ������ � ��������� � ���������.
    /// </summary>
    //[DefaultClassOptions]
    [Persistent]
    public partial class DealWithoutStageTaskInstanceDefinitionAdmin : UserTaskAdmin {
        public DealWithoutStageTaskInstanceDefinitionAdmin(Session session)
            : base(session) {
        }


        #region ������������� ��������

        //public TIn _MsgIn;
        /// <summary>
        /// ���������� � ���� ����� MsgIn � MsgOut � ������� ����� ������������ XML
        /// ������������� �� ��������� �� ����.
        /// </summary>
        [Delayed(true)]
        public string MsgInXML {
            get { return GetDelayedPropertyValue<string>("MsgInXML"); }
            set { SetDelayedPropertyValue<string>("MsgInXML", value); }
        }

        //[PersistentAlias("MsgInXML")]
        [NonPersistent]
        public TaskParameters MsgIn {
            get {
                try {
                    return CommonMethods.DeserializeObject<TaskParameters>(MsgInXML);
                } catch {
                    return new TaskParameters();
                }
            }
            set {
                MsgInXML = CommonMethods.SerializeObject<TaskParameters>(value);
            }
        }

        //public TOut _MsgOut;
        /// <summary>
        /// ���������� � ���� ����� MsgIn � MsgOut � ������� ����� ������������ XML
        /// ������������� �� ��������� �� ����.
        /// </summary>
        [Delayed(true)]
        public string MsgOutXML {
            get { return GetDelayedPropertyValue<string>("MsgOutXML"); }
            set { SetDelayedPropertyValue<string>("MsgOutXML", value); }
        }

        //[PersistentAlias("MsgOutXML")]
        [NonPersistent]
        public ApproveResult MsgOut {
            get {
                try {
                    return CommonMethods.DeserializeObject<ApproveResult>(MsgOutXML);
                } catch {
                    return ApproveResult.Undefined;
                }
            }
            set {
                MsgOutXML = CommonMethods.SerializeObject<ApproveResult>(value);
            }
        }

        #endregion


        #region ������

        /// <summary>
        /// ��������� � ������ new ������ UserTask ��� �������� ���������� ������ ��� ���������� ������-������
        /// �������� ���������� ������, � ������� ���������� ���������� ������ ������ ���������������� ��������
        /// �������� ������ �����. �������� ��������� ������� ���� UserTask. �����, � ���� ��������� ����������� ��������� �� 
        /// ������� ���������������� ������� (�� this), ��������, ����������� ������ �������������, ������ ���������.
        /// </summary>
        [Action(ToolTip = "Create concrete user task")]
        public override void create() {
            // ��������� ����������� ����� ������������� ��� ������������ ������������� ��������� ������� � ������ SetTaskParameters
            TaskParameters tp = GetTaskParams();

            // �������� ������ (����� new)
            DealWithoutStageTaskInstanceDefinition tid = DealWithoutStageTaskInstanceDefinition.create(this.Session, tp);

            SetTaskParameters(ref tid, this, tp);

            // �������� activate() ��� ��������� ������
            tid.activate();
        }

        //[DebuggerHidden]
        private TaskParameters GetTaskParams() {
            TaskParameters taskParams = new TaskParameters();
            taskParams.Priority = Priority;

            taskParams.ExcludedOwnerString = "";
            taskParams.PotentialOwnerString = "user: VB_ANGARA\\HomoSapiens; role: ; group: ;";
            taskParams.BusinessAdministratorString = "user: VB_ANGARA\\HomoSapiens; role: ; group: ;";

            taskParams.PossibleDelegateString = "";
            taskParams.TaskStakeholderString = "";
            taskParams.NotificationRecipientString = "";

            taskParams.CallBackObject = this.Oid;
            return taskParams;
        }

        /// <summary>
        /// ������� ���������� � ������.
        /// ������ ���������:
        /// 1. ��������� ������������� �� ��������������� ������� ������� ������� �����������������
        /// 2. ��������� ������������� �� �������, ��������� ����� �������� ���� TaskParameters
        /// 3. ������� ������ ��������������� ������ �� �� 1 � 2
        /// 4. ��������� ��������� � ��������������� ������
        /// 5. ����� ��������� ������ � ���������� � �.�.
        /// </summary>
        /// <param name="task"></param>
        private void SetTaskParameters(ref DealWithoutStageTaskInstanceDefinition task, DealWithoutStageTaskInstanceDefinitionAdmin adminTask, TaskParameters tp) {

            FunctionalTaskReference.ProcessElements(tp, ref task, this.Session);
            FunctionalTaskReference.FillUsersFromAdminTaskCollection(this, ref task);

            task.Priority = this.Priority;
            //task.CallBackObject = this.CallBackObject;
            task.TaskAdminGuid = this.Oid;

        }

        #endregion

    }

}
