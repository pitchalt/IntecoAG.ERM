using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.HRM.Organization {

    public enum HrmStructItemType {
        HRM_STRUCT_UNKNOW = 0,
        HRM_STRUCT_KB = 1,
        HRM_STRUCT_OZM = 2,
        HRM_STRUCT_ORION = 3,
        HRM_STRUCT_CONTRACT = 4
    }

    [Persistent("HrmDepartmentStructItem")]
    [DefaultProperty("StructType")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems="New;Delete", Enabled=false)]
    public class HrmDepartmentStructItem : XPObject {
        public HrmDepartmentStructItem(Session session): base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
            _StructType = HrmStructItemType.HRM_STRUCT_UNKNOW;
        }

        private HrmDepartmentStruct _DepartmentStruct;
        [Association("HrmDepartmentStruct-HrmDepartmentStructItem")]
        public HrmDepartmentStruct DepartmentStruct {
            get { return _DepartmentStruct; }
            set { SetPropertyValue<HrmDepartmentStruct>("DepartmentStruct", ref _DepartmentStruct, value); }
        }

        [Persistent("StructType")]
        [Browsable(false)]
        public HrmStructItemType _StructType;
        [PersistentAlias("_StructType")]
        public HrmStructItemType StructType {
            get { return _StructType; }
        }

        protected override void OnDeleting() {
            new InvalidOperationException("Delete not Support");
        }

    }

}
