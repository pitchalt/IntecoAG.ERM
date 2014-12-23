using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Import;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    public static class FmFinPlanSubjectLogic {

        private static FmJournalOperation MakeOperationPlan0(IObjectSpace os, FmFinPlanSubject fin_plan_subject) {
            FmJournalOperation oper = os.CreateObject<FmJournalOperation>();
            fin_plan_subject.PlanFullOperations.Add(oper);
            return oper;
        }

        public static void TransactPlan0(IObjectSpace os, FmFinPlanSubject fin_plan_subject, FmFinPlanSubjectDocFull doc) {
            if (doc.Order == null)
                throw new NullReferenceException();
            foreach (var oper in new List<FmJournalOperation>(fin_plan_subject.PlanFullOperations)) {
                if (oper.Order == doc.Order)
                    os.Delete(oper);
//                os.Delete(fin_plan_subject.PlanFullOperations.Where(x => x.Order == doc.Order));
            }
            foreach (var doc_oper in doc.DocOperations) {
                FmJournalOperation oper = MakeOperationPlan0(os, fin_plan_subject);
                oper.BalanceSumma = doc_oper.BalanceSumma;
                oper.BalanceValuta = doc_oper.BalanceValuta;
                oper.BuhAccount = doc_oper.BuhAccount;
                oper.CostItem = doc_oper.CostItem;
                oper.Date = doc_oper.Date;
                oper.Deal = doc_oper.Deal;
                oper.Department = doc_oper.Department;
                oper.DepartmentStructItem = doc_oper.DepartmentStructItem;
                oper.FinAccount = doc_oper.FinAccount;
                oper.FinAccountBalanceType = doc_oper.FinAccountBalanceType;
                oper.FinAccountType = doc_oper.FinAccountType;
                oper.FinIndex = doc_oper.FinIndex;
                oper.FinOperationType = doc_oper.FinOperationType;
                oper.ObligationSumma = doc_oper.ObligationSumma;
                oper.ObligationValuta = doc_oper.ObligationValuta;
                oper.Order = doc_oper.Order;
                oper.Party = doc_oper.Party;
                oper.PayType = doc_oper.PayType;
//                oper.Person = doc_oper.Person;
            }
        }

    }
}
