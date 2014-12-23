using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;

using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.FinAccount;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Subject;

using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;

using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM {

    public enum FinAccountType {
        ACC_A_CASH              = 101,
        ACC_A_PREPAY_SUPPLIER   = 102,
        ACC_A_PAY_CUSTOMER      = 103,
        ACC_A_BAY = 121,
        ACC_O_PAY_SUPPLIER      = 201,
        ACC_O_PREPAY_CUSTOMER   = 202
//        ACC_O_PAY_SUPPLIER   = 201,
    }

    public enum FinOperationType { 
        DEBET   = 1,
        CREDIT  = 2
    }

    /// <summary>
    /// 
    /// </summary>
    [Persistent("FmJournalOperation")]
    public class FmJournalOperation : XPObject {
        public FmJournalOperation(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private FmJournal _Journal;
        [Association("FmJournal-FmJournalOperation")]
        public FmJournal Journal {
            get { return _Journal; }
            set { 
                SetPropertyValue<FmJournal>("Journal", ref _Journal, value);
                if (!IsLoading) { 
                    if (value != null) {
                        Subject = value.Subject;
                        Order = value.Order;
                    }
                }
            }
        }

        [PersistentAlias("Journal.Code")]
        public String JournalCode {
            get { return Journal.Code; }
        }

        [PersistentAlias("Journal.JournalTypeSource")]
        public JournalTypeSource JournalTypeSource {
            get { return Journal.JournalTypeSource; }
        }

        [PersistentAlias("Journal.JournalTypeLegal")]
        public JournalTypeLegal JournalTypeLegal {
            get { return Journal.JournalTypeLegal; }
        }

        [PersistentAlias("Journal.JournalTypePeriod")]
        public JournalTypePeriod JournalTypePeriod {
            get { return Journal.JournalTypePeriod; }
        }

        [PersistentAlias("Journal.JournalTypeObject")]
        public JournalTypeObject JournalTypeObject {
            get { return Journal.JournalTypeObject; }
        }

        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }

        public Int32 Year {
            get { return Date.Year;  }
        }

        public Int32 YearMonth {
            get { return Year * 100 + Date.Month; }
        }

        public Int32 Month {
            get { return Date.Month; }
        }

        private fmCSubject _Subject;
        [ExplicitLoading(1)]
        [Appearance("", AppearanceItemType.ViewItem, "Journal.Subject != Null", Enabled=false)]
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                if (!IsLoading && Journal.Subject != null)
                    new InvalidOperationException("Ќе может быт изменено, поскольку задано дл€ журнала");
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); 
            }
        }

        private fmCOrder _Order;
        [ExplicitLoading(1)]
        [Appearance("", AppearanceItemType.ViewItem, "Journal.Order != Null", Enabled = false)]
        public fmCOrder Order {
            get { return _Order; }
            set {
                if (!IsLoading && Journal.Subject != null)
                    new InvalidOperationException("Ќе может быт изменено, поскольку задано дл€ журнала");
                SetPropertyValue<fmCOrder>("Order", ref _Order, value); 
            }
        }

        [Persistent("FinIndex")]
        protected fmCFinIndex _FinIndex;
        [PersistentAlias("_FinIndex")]
        [Appearance("", AppearanceItemType.ViewItem, "CostItem != Null", Enabled=false)]
        public fmCFinIndex FinIndex {
            get { return _FinIndex; }
            set {
                if (CostItem != null)
                    new InvalidOperationException("”же задана стать€");
                SetPropertyValue<fmCFinIndex>("FinIndex", ref _FinIndex, value);
            }
        }

        private fmCostItem _CostItem;
        [ExplicitLoading(1)]
        public fmCostItem CostItem {
            get { return _CostItem; }
            set { 
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
                if (!IsLoading && value != null) {
                    fmCFinIndex old = _FinIndex;
                    _FinIndex = fmCFinIndex.FinIndexGet(this.Session, value, Date);
                    OnChanged("FinIndex", old, _FinIndex);
                }
            }
        }

        private hrmDepartment _Department;
        [ExplicitLoading(1)]
        public hrmDepartment Department {
            get { return _Department; }
            set { SetPropertyValue<hrmDepartment>("Department", ref _Department, value); }
        }

        private HrmDepartmentStructItem _DepartmentStructItem;
        public HrmDepartmentStructItem  DepartmentStructItem {
            get { return _DepartmentStructItem; }
            set { SetPropertyValue<HrmDepartmentStructItem>("DepartmentStructItem", ref _DepartmentStructItem, value); }
        }

        private crmCParty _Party;
        [ExplicitLoading(1)]
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }

        [PersistentAlias("Party.Person")]
        public crmCPerson Person {
            get { return Party != null ? Party.Person : null; }
        }

        private crmContractDeal _Deal;
        [ExplicitLoading(1)]
        public crmContractDeal Deal {
            get { return _Deal; }
            set { SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value); }
        }

        private fmCFAAccount _BuhAccount;
        public fmCFAAccount BuhAccount {
            get { return _BuhAccount; }
            set { SetPropertyValue<fmCFAAccount>("BuhAccount", ref _BuhAccount, value); }
        }

        private fmCFAAccount _FinAccount;
        public fmCFAAccount FinAccount {
            get { return _FinAccount; }
            set { SetPropertyValue<fmCFAAccount>("FinAccount", ref _FinAccount, value); }
        }

        public FinAccountType FinAccountType;

        public FinOperationType FinOperationType;
        public FinOperationType FinAccountBalanceType;
        public fmPRPayType PayType;


        private Decimal _BalanceSumma;
        public Decimal BalanceSumma {
            get { return _BalanceSumma; }
            set { SetPropertyValue<Decimal>("BalanceSumma", ref _BalanceSumma, value); }
        }

        private csValuta _BalanceValuta;
        public csValuta BalanceValuta {
            get { return _BalanceValuta; }
            set { SetPropertyValue<csValuta>("BalanceValuta", ref _BalanceValuta, value); }
        }

        private Decimal _ObligationSumma;
        public Decimal ObligationSumma {
            get { return _ObligationSumma; }
            set { SetPropertyValue<Decimal>("ObligationSumma", ref _ObligationSumma, value); }
        }

        private csValuta _ObligationValuta;
        public csValuta ObligationValuta {
            get { return _ObligationValuta; }
            set { SetPropertyValue<csValuta>("ObligationValuta", ref _ObligationValuta, value); }
        }
    }

}
