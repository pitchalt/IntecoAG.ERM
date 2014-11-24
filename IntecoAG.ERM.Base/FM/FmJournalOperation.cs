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

namespace IntecoAG.ERM.FM {

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
