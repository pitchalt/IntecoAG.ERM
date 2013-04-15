using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.FM.Order {

    [Persistent("fmOrderAnalityc")]
    [DefaultProperty("Code")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public abstract class fmCOrderAnalityc : csCComponent {
        public fmCOrderAnalityc(Session session) 
            : base(session) {
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        private String _Code;
        private String _Name;
        private String _Description;
        private Int32 _SortOrder;

        [Size(14)]
        [RuleRequiredField]
        public virtual String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Size(60)]
        [RuleRequiredField]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public virtual String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        public virtual Int32 SortOrder {
            get { return _SortOrder; }
            set { SetPropertyValue<Int32>("SortOrder", ref _SortOrder, value); }
        }

        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public virtual String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }

    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycAccouterType : fmCOrderAnalityc {
        public fm—OrderAnalitycAccouterType(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycAVT : fmCOrderAnalityc {
        public fm—OrderAnalitycAVT(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycWorkType : fmCOrderAnalityc {
        public fm—OrderAnalitycWorkType(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycOrderSource : fmCOrderAnalityc {
        public fm—OrderAnalitycOrderSource(Session session)
            : base(session) {
        }
        Boolean _IsGOZ;
        Boolean _IsGZ;

        public Boolean IsGOZ {
            get { return _IsGOZ; }
            set { SetPropertyValue<Boolean>("IsGOZ", ref _IsGOZ, value); }
        }

        public Boolean IsGZ {
            get { return _IsGZ; }
            set { SetPropertyValue<Boolean>("IsGZ", ref _IsGZ, value); }
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycFinanceSource : fmCOrderAnalityc {
        public fm—OrderAnalitycFinanceSource(Session session)
            : base(session) {
        }
    }

    public enum fm—OrderAnalitycMilitaryProductType { 
        PRODUCT_OTHER = 0,
        PRODUCT_MILITARY = 1,
        PRODUCT_CIVIL = 2
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycMilitary : fmCOrderAnalityc {
        public fm—OrderAnalitycMilitary(Session session)
            : base(session) {
        }

        fm—OrderAnalitycMilitaryProductType _ProductType;
        public fm—OrderAnalitycMilitaryProductType ProductType {
            get { return _ProductType; }
            set { SetPropertyValue<fm—OrderAnalitycMilitaryProductType>("ProductType", ref _ProductType, value); }
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycOKVED : fmCOrderAnalityc {
        public fm—OrderAnalitycOKVED(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycFedProg : fmCOrderAnalityc {
        public fm—OrderAnalitycFedProg(Session session)
            : base(session) {
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycBigCustomer : fmCOrderAnalityc {
        public fm—OrderAnalitycBigCustomer(Session session)
            : base(session) {
        }

        String _Group;
        fm—OrderAnalitycRegion _Region;

        [Size(14)]
        public String Group {
            get { return _Group; }
            set { SetPropertyValue<String>("Group", ref _Group, value); }
        }

        [Association("fmOrderAnalitycRegion-fmOrderAnalitycBigCustomer")]
        public fm—OrderAnalitycRegion Region {
            get { return _Region; }
            set { SetPropertyValue<fm—OrderAnalitycRegion>("Region", ref _Region, value); }
        }
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fm—OrderAnalitycRegion : fmCOrderAnalityc {
        public fm—OrderAnalitycRegion(Session session)
            : base(session) {
        }

        Boolean _IsVED;
        Boolean _IsSNG;

        public Boolean IsVED {
            get { return _IsVED; }
            set { SetPropertyValue<Boolean>("IsVED", ref _IsVED, value); }
        }

        public Boolean IsSNG {
            get { return _IsSNG; }
            set { SetPropertyValue<Boolean>("IsSNG", ref _IsSNG, value); }
        }

        [Association("fmOrderAnalitycRegion-fmOrderAnalitycBigCustomer")]
        public XPCollection<fm—OrderAnalitycBigCustomer> BigCustomers {
            get { return GetCollection<fm—OrderAnalitycBigCustomer>("BigCustomers"); }
        }
    }
}