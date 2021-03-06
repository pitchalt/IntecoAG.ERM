using System;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.Budget.Period;
//
namespace IntecoAG.ERM.Trw.Budget {

    [NavigationItem("Trw")]
    [Persistent("TrwBudgetPeriod")]
    [DefaultProperty("Name")]
    public class TrwBudgetPeriod : BaseObject {

        private Int16 _Year;
        public Int16 Year {
            get { return _Year; }
            set { SetPropertyValue<Int16>("Year", ref _Year, value); }
        }

        public String Code {
            get { return Year.ToString("D4"); }
        }

        public String Name {
            get { return "������� ��� �� " + Year.ToString() + "�."; }
        }

        private Int16 _SubjectNumberCurrent;
        [Browsable(false)]
        public Int16 SubjectNumberCurrent {
            get { return _SubjectNumberCurrent; }
            set { SetPropertyValue<Int16>("SubjectNumberCurrent", ref _SubjectNumberCurrent, value); }
        }

        public Int16 SubjectNumberNextGet() {
            SubjectNumberCurrent++;
            return SubjectNumberCurrent;
        }

        [Association("TrwBudgetPeriod-TrwSubject"), Aggregated]
        public XPCollection<TrwSubject> TrwSubjects {
            get { return GetCollection<TrwSubject>("TrwSubjects"); }
        }

        [Association("TrwBudgetPeriod-TrwBudgetPeriodValue"), Aggregated]
        public XPCollection<TrwBudgetPeriodValue> PeriodValues {
            get { return GetCollection<TrwBudgetPeriodValue>("PeriodValues"); }
        }

        [Association("TrwBudgetPeriod-TrwBudgetMaster"), Aggregated]
        public XPCollection<TrwBudgetMaster> BudgetMasters {
            get { return GetCollection<TrwBudgetMaster>("BudgetMasters"); }
        }

        [Association("TrwBudgetPeriod-TrwBudgetPeriodCurrencyExchange"), Aggregated]
        public XPCollection<TrwBudgetPeriodCurrencyExchange> CurrencyExchanges {
            get { return GetCollection<TrwBudgetPeriodCurrencyExchange>("CurrencyExchanges"); }
        }

        [Association("TrwBudgetPeriod-TrwBudgetPeriodInContractBSR"), Aggregated]
        public XPCollection<TrwBudgetPeriodInContractBSR> InContractBSR {
            get { return GetCollection<TrwBudgetPeriodInContractBSR>("InContractBSR"); }
        }
        [Association("TrwBudgetPeriod-TrwBudgetPeriodDoc"), Aggregated]
        public XPCollection<TrwBudgetPeriodDoc> BudgetPeriodDocs {
            get { return GetCollection<TrwBudgetPeriodDoc>("BudgetPeriodDocs"); }
        }

        [Association("TrwBudgetPeriod-crmParty")]
        public XPCollection<crmCParty> CorporationPartys {
            get { return GetCollection<crmCParty>("CorporationPartys"); }
        }

        private csValuta _Valuta;
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }

        public TrwBudgetPeriod(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            InitPeriodValues();
        }

        public void InitPeriodValues() {
            for (short i = 0; i < 14; i++) {
                TrwBudgetPeriodValue period_value = PeriodValues.FirstOrDefault(x => x.Month == i);
                if (period_value == null) {
                    period_value = new TrwBudgetPeriodValue(this.Session);
                    period_value.Month = i;
                    PeriodValues.Add(period_value);
                }
            }
        }

        public TrwBudgetPeriodValue ValueGet(DateTime date) {
            Int16 period_month = -1;
            if (date.Year == Year) {
                period_month = (Int16) date.Month;
            }
            else {
                if (date.Year < Year)
                    period_month = 0;
                else
                    period_month = 13;
            }
            return PeriodValues.FirstOrDefault(x => x.Month == period_month);
        }

        public XPCollection<TrwSubjectDealBay> PeriodBayDeals {
            get { 
                return new XPCollection<TrwSubjectDealBay>(this.Session, 
                    new BinaryOperator("TrwSubject.Period", this));
            }
        }

        public XPCollection<TrwSubjectDealSale> PeriodSaleDeals {
            get {
                return new XPCollection<TrwSubjectDealSale>(this.Session,
                    new BinaryOperator("TrwSubject.Period", this));
            }
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

}
