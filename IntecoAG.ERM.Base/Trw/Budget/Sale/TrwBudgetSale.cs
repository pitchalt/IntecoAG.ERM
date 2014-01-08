using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.Trw.Subject;

namespace IntecoAG.ERM.Trw.Budget {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwBudgetSale : TrwBudgetMaster {

        [Association("TrwBudgetSale-TrwBudgetSubjectSale"), Aggregated]
        public XPCollection<TrwBudgetSubjectSale> TrwBudgetSubjectSales {
            get { return GetCollection<TrwBudgetSubjectSale>("TrwBudgetSubjectSales"); }
        }

        public TrwBudgetSale(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void Calculate(IObjectSpace os) {
            foreach (var trw_subject in BudgetPeriod.TrwSubjects) {
                TrwBudgetSubjectSale subject_budget_sale = TrwBudgetSubjectSales.FirstOrDefault(x => x.TrwSubject == trw_subject); 
                if (subject_budget_sale == null) {
                    subject_budget_sale = os.CreateObject<TrwBudgetSubjectSale>();
                    TrwBudgetSubjectSales.Add(subject_budget_sale);
                    subject_budget_sale.TrwSubject = trw_subject;
                }
            }
        }

        protected override void NameUpdate() {
            if (BudgetPeriod != null)
                _Name = "Ѕюджет выручки за " + BudgetPeriod.Year + "г.";
        }
    }

}
