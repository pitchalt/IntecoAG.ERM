using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

//
using IntecoAG.ERM.CS.Common;
using DevExpress.ExpressApp.Utils;

namespace IntecoAG.ERM.CS.Security {

    public class csCSecurityRoleActionPermission : BaseObject {

        public csCSecurityRoleActionPermission(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private csCSecurityActionExecutePermissionData FindExportPermissionData() {
            //foreach (PermissionData permissionData in PersistentPermissions) {
            //    ExportPermissionData actionPermissionData = permissionData as ExportPermissionData;
            //    if (actionPermissionData != null) {
            //        return actionPermissionData;
            //    }
            //}
            return null;
        }

        private IModelAction _ModelAction;
        /// <summary>
        /// Model's action
        /// </summary>
        [ValueConverter(typeof(TypeToStringConverter))]
        //[TypeConverter(typeof(StateMachineTypeConverter))]
        //[DataSourceProperty("Application.Model.ActionDesign.Actions")]
        [DataSourceProperty("AvailableModelActions")]
        public IModelAction ModelAction {
            get { return _ModelAction; }
            set {
                SetPropertyValue<IModelAction>("ModelAction", ref _ModelAction, value);
            }
        }

        [Browsable(false)]
        public IList<IModelAction> AvailableModelActions;

        [NonPersistent]
        public bool CanExecute {
            get {
                csCSecurityActionExecutePermissionData ActionExecutePermissionData = FindExportPermissionData();
                return ActionExecutePermissionData != null && ActionExecutePermissionData.CanExecute;
            }
            set {
                if (!IsLoading) {
                    csCSecurityActionExecutePermissionData actionPermissionData = FindExportPermissionData();
                    if (value) {
                        //if (actionPermissionData == null) {
                        //    actionPermissionData = new csCSecurityActionExecutePermissionData(Session);
                        //    PersistentPermissions.Add(actionPermissionData);
                        //}
                        actionPermissionData.CanExecute = true;
                    }
                    else {
                        if (actionPermissionData != null) {
                            actionPermissionData.CanExecute = false;

                        }
                    }
                }
            }
        }

        private csCSecurityRole _SecurityRole;
        [Association("csCSecurityRole-csCSecurityRoleActionPermission")]
        public csCSecurityRole SecurityRole {
            get { return _SecurityRole; }
            set { SetPropertyValue<csCSecurityRole>("SecurityRole", ref _SecurityRole, value); }
        }
    
    }

}