using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Budget;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.References;

namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_224 : ModuleUpdater {
        public Updater_1_1_1_224(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.224"))
                return;
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetValue\";", true);
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudgetKey\";", true);
            ExecuteNonQueryCommand("DROP TABLE \"TrwBudget\";", true);
            //            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudget\" DROP CONSTRAINT \"FK_TrwBudget_TrwPeriod\";", false);
//            ExecuteNonQueryCommand("ALTER TABLE \"TrwBudgetValue\" DROP CONSTRAINT \"FK_TrwBudgetValue_PeriodValue\";", false);
        }


        public void UpdateTrwSubjects(IObjectSpace os) {
            foreach (TrwSubject trw_subj in os.GetObjects<TrwSubject>()) {
                trw_subj.Period = trw_subj.Period;
                trw_subj.Subjects.Clear();
                trw_subj.Subjects.Add(trw_subj.Subject);
                //
                IList<TrwSubjectDealSale> deal_sales = new List<TrwSubjectDealSale>(trw_subj.DealsSale);
                foreach (TrwSubjectDealSale deal_sale in deal_sales) {
                    if (deal_sale.DealBudget == null) {
                        deal_sale.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL;
                        if (deal_sale.Deal != null)
                            deal_sale.CrmContractDeals.Add(deal_sale.Deal);
                    }
                    else {
                        os.Delete(deal_sale);
                    }
                }
                TrwSubjectDealSale deal_sale_other = os.CreateObject<TrwSubjectDealSale>();
                trw_subj.DealsSale.Add(deal_sale_other);
                deal_sale_other.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER;
                os.Delete(trw_subj.DealOtherSale);
                trw_subj.DealOtherSale = deal_sale_other.DealBudget;
                deal_sale_other.PersonInternal = TrwSettings.GetInstance(os).PersonOtherSale;
                //                deal_sale_other.UpdateConsolidateDeal(true);
                //
                IList<TrwSubjectDealBay> deal_bays = new List<TrwSubjectDealBay>(trw_subj.DealsBay);
                foreach (TrwSubjectDealBay deal_bay in deal_bays) {
                    if (deal_bay.DealBudget == null) {
                        deal_bay.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL;
                        if (deal_bay.Deal != null)
                            deal_bay.CrmContractDeals.Add(deal_bay.Deal);
                    }
                    else {
                        os.Delete(deal_bay);
                    }
                }
                TrwSubjectDealBay deal_bay_other = os.CreateObject<TrwSubjectDealBay>();
                trw_subj.DealsBay.Add(deal_bay_other);
                deal_bay_other.DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER;
                os.Delete(trw_subj.DealOtherBay);
                trw_subj.DealOtherBay = deal_bay_other.DealBudget;
                deal_bay_other.PersonInternal = TrwSettings.GetInstance(os).PersonOtherBay;
                //                deal_bay_other.UpdateConsolidateDeal(true);
                //
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.224"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                TrwSettings instance = TrwSettings.GetInstance(os);
                crmCLegalPerson other_sale = os.FindObject<crmCLegalPerson>(new BinaryOperator("Code", "12582"));
                other_sale.Name = "Прочие заказчики";
                instance.PersonOtherSale = other_sale.Person;
                crmCLegalPerson other_bay = os.FindObject<crmCLegalPerson>(new BinaryOperator("Code", "12581"));
                other_bay.Name = "Прочие поставщики";
                instance.PersonOtherBay = other_bay.Person;
                //
                UpdateTrwSubjects(os);
                os.CommitChanges();
            }
        }

    }
}
