using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Data.Filtering;
//
using FileHelpers;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Budget;
using IntecoAG.ERM.Trw.Budget.Period;
//
namespace IntecoAG.ERM.CS {

    public class Updater_1_1_1_233 : ModuleUpdater {
        public Updater_1_1_1_233(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        //public override void UpdateDatabaseBeforeUpdateSchema() {
        //    base.UpdateDatabaseBeforeUpdateSchema();
        //    if (this.CurrentDBVersion != new Version("1.1.1.233"))
        //        return;
        //}

        public void UpdateBSR(IObjectSpace os) {
            foreach (TrwBudgetPeriod period in os.GetObjects<TrwBudgetPeriod>()) {
                TrwBudgetPeriodDocBSR doc = os.CreateObject<TrwBudgetPeriodDocBSR>();
                period.BudgetPeriodDocs.Add(doc);
                doc.Number = "ÁÑÐ";
                doc.Name = "ÁÑÐ";
                foreach (TrwBudgetPeriodInContractBSR bsr in period.InContractBSR) {
                    TrwBudgetPeriodDocBSR.LineBSR line = os.CreateObject<TrwBudgetPeriodDocBSR.LineBSR>();
                    doc.DocBSRLines.Add(line);
                    line.SaleNomCode = bsr.SaleNomCode;
                    line.SaleNomenclature = bsr.SaleNomenclature;
                    line.Period00 = bsr.Period00;
                    line.Period01 = bsr.Period01;
                    line.Period02 = bsr.Period02;
                    line.Period03 = bsr.Period03;
                    line.Period04 = bsr.Period04;
                    line.Period05 = bsr.Period05;
                    line.Period06 = bsr.Period06;
                    line.Period07 = bsr.Period07;
                    line.Period08 = bsr.Period08;
                    line.Period09 = bsr.Period09;
                    line.Period10 = bsr.Period10;
                    line.Period11 = bsr.Period11;
                    line.Period12 = bsr.Period12;
                    line.Period13 = bsr.Period13;
                }
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.233"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //
                UpdateBSR(os);
                os.CommitChanges();
            }
        }

    }
}
