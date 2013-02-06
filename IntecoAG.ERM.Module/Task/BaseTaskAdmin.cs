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

    public abstract partial class BaseTaskAdmin : BaseObject {

        public BaseTaskAdmin(Session session)
            : base(session) {
        }

        #region ��������

        public abstract bool CanCallFromUI {
            get;
            set;
        }

        #endregion


        #region ������

        public virtual void create() { }

        // ������ ������ � ��������������

        public virtual List<AppUser> MergeUserLists(List<AppUser> userList1, List<AppUser> userList2) {
            return null;
        }

        public virtual List<AppUser> GetUserList111(List<AppUser> userList1, List<AppUser> userList2) {
            return null;
        }

        #endregion

    }

}