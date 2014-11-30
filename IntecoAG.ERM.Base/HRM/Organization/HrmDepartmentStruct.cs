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

    [Persistent("HrmDepartmentStruct")]
    [DefaultProperty("Code")]
    [NavigationItem("FinPlan")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "New;Delete", Enabled = false)]
    public class HrmDepartmentStruct : XPObject {
        public HrmDepartmentStruct(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Size(64)]
        [Browsable(false)]
        [Persistent("Code")]
        public String _Code;
        [PersistentAlias("_Code")]
        public String Code {
            get { return _Code; }
        }


        [Association("HrmDepartmentStruct-HrmDepartmentStructItem"), Aggregated]
        public XPCollection<HrmDepartmentStructItem> Items {
            get { return GetCollection<HrmDepartmentStructItem>("Items"); }
        }

        protected override void OnDeleting() {
            new InvalidOperationException("Delete not Support");
        }
    }

}
