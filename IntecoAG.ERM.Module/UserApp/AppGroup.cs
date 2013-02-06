using DevExpress.Xpo;
using System.Security;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.Security;

namespace IntecoAG.ERM.Module {

    [ImageName("BO_Role")]
    [DefaultProperty("Name")]
    [RuleCriteria(null, "Delete", "AppUsers.Count == 0", "Cannot delete the role because there are users that reference it", SkipNullOrEmptyValues = true)]
    public class Group : BasePersistentObject, IRole, ICustomizableRole {

        public const string DefaultAdministratorsGroupName = "Administrators";
        public const string DefaultUsersGroupName = "Users";
        private string _Name;
        private List<IPermission> _Permissions = new List<IPermission>();

        public Group(Session session) : base(session) { }

        public ReadOnlyCollection<IPermission> GetPermissions(IList<IPersistentPermission>persistentPermissions) {
            _Permissions.Clear();
            foreach (IPersistentPermission persistentPermission in persistentPermissions)
                if (persistentPermission.Permission != null) _Permissions.Add(persistentPermission.Permission);
            return _Permissions.AsReadOnly();
        }

        [Association("AppUser-Group", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<AppUser> AppUsers {
            get { return GetCollection<AppUser>("AppUsers"); }
        }

        [Aggregated, Association("Group-PersistentPermissionObjects")]
        [DevExpress.Xpo.DisplayName("Permissions")]
        public XPCollection<PersistentPermissionObject> PersistentPermissions {
            get { return GetCollection<PersistentPermissionObject>("PersistentPermissions"); }
        }

        public PersistentPermissionObject AddPermission(IPermission permission) {
            PersistentPermissionObject result = new PersistentPermissionObject(Session, permission);
            PersistentPermissions.Add(result);
            return result;
        }

        IList<IUser> IRole.Users {
            get { return new ListConverter<IUser, AppUser>(AppUsers); }
        }


        #region IRole Members

        [RuleRequiredField(null, "Save", "The group name must not be empty")]
        [RuleUniqueValue(null, "Save", "The group with the entered Name was already registered within the system")]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        ReadOnlyCollection<IPermission> IRole.Permissions {
            get { return GetPermissions(new ListConverter<IPersistentPermission, PersistentPermissionObject>(PersistentPermissions)); }
        }

        #endregion


        #region ICustomizableRole Members

        void ICustomizableRole.AddPermission(IPermission permission) {
            AddPermission(permission);
        }

        #endregion
    }
}