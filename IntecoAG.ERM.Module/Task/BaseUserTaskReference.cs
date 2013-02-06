using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module  {
    //[DefaultClassOptions]
    [Persistent]
    public abstract partial class BaseUserTaskReference : BaseObject {
        public BaseUserTaskReference(Session session)
            : base(session) {
        }

        #region ��������

        private string _UserName;
        /// <summary>
        /// �������� �������� ��� ������������ (Login) (��� �� ActiveDirectory), ���������� ��� ����� � �����
        /// </summary>
        public virtual string UserName {
            get { return _UserName; }
            set { SetPropertyValue<string>("UserName", ref _UserName, value); }
        }

        private string _Name;
        /// <summary>
        /// �������� ������������ ������������ (���), ��� ����� � ����� - �� �����-�� ������������
        /// </summary>
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        private UserAssociationType _userAssociationType;
        /// <summary>
        /// ��������� ������
        /// </summary>
        public virtual UserAssociationType UserAssociationType
        {
            get { return _userAssociationType; }
            set { SetPropertyValue<UserAssociationType>("UserAssociationType", ref _userAssociationType, value); }
        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ������ ������������� �� ��� �������������� (������ �� 1-�� ������������), �� ����, �� ������
        /// </summary>
        /// <returns></returns>
        public abstract List<AppUser> GetUserList(IList<BaseUserTaskReference> Butrs, IList<string> userNameList);

        #endregion
    }

}
