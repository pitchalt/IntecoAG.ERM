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
    //[DefaultClassOptions]
    [Persistent]
    public partial class GroupTaskReference : BaseUserTaskReference {
        public GroupTaskReference(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            UserAssociationType = Module.UserAssociationType.group;
        }

        public override List<AppUser> GetUserList(IList<BaseUserTaskReference> Butrs, IList<string> userNameList) { return null; }

    }

}
