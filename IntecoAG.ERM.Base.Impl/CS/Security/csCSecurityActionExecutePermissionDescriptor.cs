using System;
using System.Collections.Generic;
using System.Security;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;

namespace IntecoAG.ERM.CS.Security {

    [DevExpress.ExpressApp.DC.DomainComponent]
    [NonPersistent]
    [DefaultListViewOptions(true, NewItemRowPosition.None)]
#pragma warning disable 0618
    [NonPersistentEditable]
#pragma warning restore 0618
    public class ActionExecutePermissionDescriptor : PermissionDescriptorBase {
        private ActionExecutePermissionDescriptorsList owner;
        private Type type;
        private String targetAction;

        public ActionExecutePermissionDescriptor(ActionExecutePermissionDescriptorsList owner, Type type, String targetAction, bool defaultExecute, string inheritedFrom) {
            InheritedFrom = inheritedFrom;
            Initialize(owner, type, targetAction);
            this.executeGrantedInOtherPermission = defaultExecute;
        }

        internal void Initialize(ActionExecutePermissionDescriptorsList owner, Type type, String targetAction) {
            Guard.ArgumentNotNull(owner, "owner");
            Guard.ArgumentNotNull(type, "type");
            this.owner = owner;
            this.type = type;
            this.targetAction = targetAction;
        }

        [Browsable(false)]
        public Type ObjectType {
            get {
                return type;
            }
        }

        [Browsable(false)]
        public String TargetAction {
            get {
                return targetAction;
            }
        }

        public string Caption {
            get {
                return CaptionHelper.GetClassCaption(type.FullName) + ", " + targetAction;
            }
        }

        [CriteriaOptions("ObjectType")]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criteria {
            get;
            set;
        }

        public ActionOperationPermissionData CreateActionOperationPermissionData(Session session) {
            if (IsEmpty()) {
                return null;
            }
            ActionOperationPermissionData data = new ActionOperationPermissionData(session);
            data.TargetType = owner.ObjectType;
            data.Criteria = Criteria;
            data.TargetAction = TargetAction;
            data.AllowExecute = execute.HasValue ? execute.Value : false;
            data.Save();
            return data;
        }

        public bool IsEmpty() {
            return string.IsNullOrEmpty(Criteria) && (!execute.HasValue || string.IsNullOrEmpty(targetAction));
        }

        private bool executeGrantedInOtherPermission;
        private bool? execute;
        public bool? Execute {
            get {
                if (execute.HasValue && execute.Value) {
                    return true;
                } else {
                    return executeGrantedInOtherPermission ? (bool?)null : false;
                }
            }
            set {
                execute = value.HasValue && value.Value ? true : (bool?)null;
            }
        }
        public string InheritedFrom {
            get;
            private set;
        }
    }

    public class ActionExecutePermissionDescriptorsList : BindingList<ActionExecutePermissionDescriptor> {
        private readonly bool executeFromType;
        private Type type;
        private String targetAction;

        private ActionExecutePermissionDescriptor CreateNewActionOperationPermissionDescriptor(Type type, String targetAction, bool executeFromType) {
            string reason = "";
            if (executeFromType) {
                reason = string.Concat(reason, executeFromType ? string.Format(CaptionHelper.GetLocalizedText("Messages", "Execute") + CaptionHelper.GetLocalizedText("Messages", "IsInheritedFrom") + CaptionHelper.GetLocalizedText("Messages", "TargetAction"), CaptionHelper.GetClassCaption(type.FullName)) : "");
            }
            ActionExecutePermissionDescriptor newPermissionDescriptor = new ActionExecutePermissionDescriptor(this, type, targetAction, executeFromType, reason);
            return newPermissionDescriptor;
        }

        protected override void OnListChanged(ListChangedEventArgs e) {
            IsModified = true;
            base.OnListChanged(e);
        }

        protected override object AddNewCore() {
            ActionExecutePermissionDescriptor newPermissionDescriptor = CreateNewActionOperationPermissionDescriptor(type, targetAction, executeFromType);
            Add(newPermissionDescriptor);
            return newPermissionDescriptor;
        }

        public ActionExecutePermissionDescriptorsList(Type type, String targetAction, IList<IPermissionData> permissionData, bool executeFromType) {
            this.executeFromType = executeFromType;
            this.type = type;
            this.targetAction = targetAction;
            AllowNew = true;
            AllowRemove = true;
            RaiseListChangedEvents = false;
            ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(type);
            if (permissionData != null) {
                foreach (PermissionData info in permissionData) {
                    ActionOperationPermissionData data = info as ActionOperationPermissionData;
                    if (data != null && data.TargetType == type) {
                        ActionExecutePermissionDescriptor newPermissionDescriptor = CreateNewActionOperationPermissionDescriptor(type, targetAction, executeFromType);
                        newPermissionDescriptor.Criteria = data.Criteria;
                        newPermissionDescriptor.Execute = data.AllowExecute;
                        //newPermissionDescriptor.TargetAction = data.TargetAction; // Свойство только для чтения
                        Add(newPermissionDescriptor);
                    }
                }
            }
            RaiseListChangedEvents = true;
            IsModified = false;
        }
        public Type ObjectType {
            get {
                return type;
            }
        }
        public bool IsModified {
            get;
            private set;
        }
    }

    public class DisableProcessCurrentObjectForObjectPermissionDescriptorsList : DisableProcessCurrentObjectForTypeController<ActionExecutePermissionDescriptor> {
        private SimpleAction deleteAction;
        private SimpleAction newAction;
        private void deleteAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            foreach (object objToRemove in e.SelectedObjects) {
                ((IBindingList)View.CollectionSource.Collection).Remove(objToRemove);
            }
        }
        private void newAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ((IBindingList)View.CollectionSource.Collection).AddNew();
            View.Editor.Refresh();
        }
        private void newObjectViewController_CollectDescendantTypes(object sender, CollectTypesEventArgs e) {
            e.Types.Add(typeof(ActionExecutePermissionDescriptor));
        }
        protected override void OnActivated() {
            base.OnActivated();
            newAction.Active["ViewAllowNew"] = View.AllowNew;
            deleteAction.Active["ViewAllowDelete"] = View.AllowDelete;
        }
        public DisableProcessCurrentObjectForObjectPermissionDescriptorsList() {
            newAction = new SimpleAction(this, "NewPermissionDescriptor", PredefinedCategory.Edit);
            newAction.Caption = "New";
            newAction.ImageName = "MenuBar_New";
            newAction.Execute += new SimpleActionExecuteEventHandler(newAction_Execute);
            deleteAction = new SimpleAction(this, "DeletePermissionDescriptor", PredefinedCategory.Edit);
            deleteAction.Caption = "Delete";
            deleteAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            deleteAction.ImageName = "MenuBar_Delete";
            deleteAction.Execute += new SimpleActionExecuteEventHandler(deleteAction_Execute);
            deleteAction.ConfirmationMessage = "You are about to delete the selected item(s). Do you want to proceed?";
        }
    }
}
