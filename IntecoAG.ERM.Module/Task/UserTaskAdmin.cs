using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// ����� (� ����� ������) ��� ���������� ���������������� �������� ������������� ������.
    /// ���� ����� ����������� � ������������� � ���� �������� ����������������, ������� ����� ������� � ������� �����
    /// � �������� ���������������� �������� ����� ����, � �������, �����:
    /// - ���������
    /// - ������ �������������
    /// </summary>
    /// <typeparam name="TIn">��� (��������) ������-������� (��������, Invoice)</typeparam>
    /// <typeparam name="TOut">��� (���������) ������-�������</typeparam>
    public abstract partial class UserTaskAdmin : BaseTaskAdmin {
        public UserTaskAdmin(Session session)
            : base(session) {
        }


        #region ��������

        private bool _CanCallFromUI;
        public override bool CanCallFromUI {
            get { return _CanCallFromUI; }
            set { SetPropertyValue<Boolean>("CanCallFromUI", ref _CanCallFromUI, value); }
        }


        private bool _MustMergeUsersFromBusinessObject;
        public bool MustMergeUsersFromBusinessObject {
            get { return _MustMergeUsersFromBusinessObject; }
            set { SetPropertyValue<bool>("MustMergeUsersFromBusinessObject", ref _MustMergeUsersFromBusinessObject, value); }
        }

        private uint _Priority;
        public uint Priority {
            get { return _Priority; }
            set { SetPropertyValue<uint>("Priority", ref _Priority, value); }
        }


        /// <summary>
        /// Guid ��� ������, ������� ����������� ������ ���������������� ��������
        /// </summary>
        private Guid _TaskGuid;
        public Guid TaskGuid {
            get { return _TaskGuid; }
            set { SetPropertyValue<Guid>("TaskGuid", ref _TaskGuid, value); }
        }


        /// <summary>
        /// Guid ��� ���������� ��������� ������. ��� Guid ���������������� ������������ ������� ���� BPInvoiceProcess
        /// </summary>
        private Guid _CallBackObject;
        public Guid CallBackObject {
            get { return _CallBackObject; }
            set { SetPropertyValue<Guid>("CallBackObject", ref _CallBackObject, value); }
        }


        private string _DetailView;
        /// <summary>
        /// ������������ ���� DetailView, ������� ������ ���� ������� ��� ������ ������ (UserTask)
        /// ������� �����. ���� DetailView ������������ ������ ����� ����, ��� � UsertaskAdmin ��������
        /// ����� create() � ������ � ������, ����� ������ �� ��������� � ��������� suspend. � ��������� ������,
        /// ������������ ����������� DetailView ��� ������� BaseUserTask
        /// </summary>
        public string DetailView {
            get { return _DetailView; }
            set { SetPropertyValue<string>("DetailView", ref _DetailView, value); }
        }

        #endregion

        
        #region ������

        // ������ ������ � ��������������: 
        // ������� ������� �������������;
        // ������������ ������������� �� ������ ��������� (Windows, ActiveDirectory � �.�.)
        // ������������ ������ ������������� �� ������ �����
        // ������������ ������ ������������� �� ������ ����� (����� ���� ���)
        // ������������ ������ ������������� �� ������ ������������� �� ������ ������ (�����������), ����� ���� ������ ���������� � �������������

        public override List<AppUser> MergeUserLists(List<AppUser> userList1, List<AppUser> userList2) {
            List<AppUser> resultList = new List<AppUser>();
            resultList = userList1;
            foreach (AppUser au in userList2) {
                if (!userList1.Contains(au)) userList1.Add(au);
            }
            return resultList;
        }

        #endregion

    }

}
