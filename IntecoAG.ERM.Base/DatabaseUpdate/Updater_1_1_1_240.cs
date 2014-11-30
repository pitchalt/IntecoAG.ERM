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
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Accounting;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.CS {
    public class Updater_1_1_1_240 : ModuleUpdater {
        public Updater_1_1_1_240(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

//        public override void UpdateDatabaseBeforeUpdateSchema() {
//            base.UpdateDatabaseBeforeUpdateSchema();
//            if (this.CurrentDBVersion != new Version("1.1.1.240"))
//                return;
////            ExecuteNonQueryCommand("DROP TABLE \"FmFinPlanDoc\", \"FmFinPlanPlan\", \"FmFinPlanOperation\", \"FmFinPlanJournal\" CASCADE;", false);
//        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (this.CurrentDBVersion != new Version("1.1.1.240"))
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                HrmDepartmentStruct dep_struct = os.CreateObject<HrmDepartmentStruct>();
                dep_struct._Code = "УУ";
                HrmDepartmentStructItem item = null;
                item = os.CreateObject<HrmDepartmentStructItem>();
                item._StructType = HrmStructItemType.HRM_STRUCT_KB;
                dep_struct.Items.Add(item);
                item = os.CreateObject<HrmDepartmentStructItem>();
                item._StructType = HrmStructItemType.HRM_STRUCT_ORION;
                dep_struct.Items.Add(item);
                item = os.CreateObject<HrmDepartmentStructItem>();
                item._StructType = HrmStructItemType.HRM_STRUCT_OZM;
                dep_struct.Items.Add(item);
                item = os.CreateObject<HrmDepartmentStructItem>();
                item._StructType = HrmStructItemType.HRM_STRUCT_CONTRACT;
                dep_struct.Items.Add(item);

                IList<fmCostItem> cost_items = os.GetObjects<fmCostItem>();
                fmCostItem cost_item = null;
                fmCFinIndex fin_index = null;
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "ФОТ"));
                fin_index.CodeBuh = 4;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 2;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2000");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2001");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2002");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2003");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2004");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2005");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2006");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2007");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2008");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2011");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2021");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Командировки"));
                fin_index.CodeBuh = 9;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 2;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2100");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2101");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2102");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2103");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Накладные"));
                fin_index.CodeBuh = 8;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 2;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "3001");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "4001");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "4010");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "3002");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "4002");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "4011");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Соц.Страх."));
                fin_index.CodeBuh = 8;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 2;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5003");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2012");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "2022");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Материалы"));
                fin_index.CodeBuh = 1;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6000");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6001");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6002");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6004");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6005");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6007");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6009");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Оборудование"));
                fin_index.CodeBuh = 1;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6003");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Оборудование"));
                fin_index.CodeBuh = 1;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6006");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Смежники"));
                fin_index.CodeBuh = 1;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "6003");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7001");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Прочие прямые"));
                fin_index.CodeBuh = 24;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5005");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5006");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5007");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5008");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5009");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5010");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "5011");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7002");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7003");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7004");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7005");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7006");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7007");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7008");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7009");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7010");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7011");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7012");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7013");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7014");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7015");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7016");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                cost_item = cost_items.FirstOrDefault(x => x.Code == "7025");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Выручка"));
                fin_index.CodeBuh = 94;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "1000");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //                
                //
                fin_index = os.FindObject<fmCFinIndex>(new BinaryOperator("Name", "Выручка"));
                fin_index.CodeBuh = 94;
                fin_index.Code = fin_index.Name;
                fin_index.SortOrder = 1;
                cost_item = cost_items.FirstOrDefault(x => x.Code == "1000");
                if (cost_item != null)
                    fin_index.CostItems.Add(cost_item);
                //
                os.CommitChanges();
            }
        }

    }
}
