using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Nomenclature;

using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;

using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.FM.FinPlan {

    /// <summary>
    /// 
    /// </summary>
    public enum JournalSubType {
        JOURNAL_SUB_TYPE_PLAN0 = 1,
        JOURNAL_SUB_TYPE_FACT = 2,
        JOURNAL_SUB_TYPE_COST_STRUCTURE = 3, 
        JOURNAL_SUB_TYPE_CONTRACT = 4,
        JOURNAL_SUB_TYPE_YEAR = 5
    }

    /// <summary>
    /// 
    /// </summary>
    [Persistent("FmFinPlanOperation")]
    public class FmFinPlanOperation : XPObject {
        public FmFinPlanOperation(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private FmFinPlanJournal _Journal;
        [Association("fmFinPlanJournal-fmFinPlanOperation")]
        public FmFinPlanJournal Journal {
            get { return _Journal; }
            set { SetPropertyValue<FmFinPlanJournal>("Journal", ref _Journal, value); }
        }

        private JournalSubType _JournalSubType;
        public JournalSubType JournalSubType {
            get { return _JournalSubType; }
            set { SetPropertyValue<JournalSubType>("JournalSubType", ref _JournalSubType, value); }
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
        public fmCSubject Subject {
            get { return _Subject; }
            set { SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); }
        }

        private fmCOrder _Order;
        [ExplicitLoading(1)]
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private crmCPerson _Person;
        [ExplicitLoading(1)]
        public crmCPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmCPerson>("Person", ref _Person, value); }
        }

        private crmContractDeal _Deal;
        [ExplicitLoading(1)]
        public crmContractDeal Deal {
            get { return _Deal; }
            set { SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value); }
        }

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
