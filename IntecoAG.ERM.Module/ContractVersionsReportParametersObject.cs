using System;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {
    [NonPersistent]
    public class ContractVersionsReportParametersObject : ReportParametersObjectBase {
        public ContractVersionsReportParametersObject(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            return CriteriaOperator.Parse("Current.Category = ? AND Current.DocumentCategory = ?", Category, DocumentCategory);
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            if (SortByName) {
                sorting.Add(new SortProperty("Category.Name", SortingDirection.Ascending));
                //sorting.Add(new SortProperty("crmWorkPlan.Current.Supplier.Name", SortingDirection.Ascending));
            }
            return sorting;
        }


        #region ПАРАМЕТРЫ НА ФОРМЕ

        // Заказчик
        private crmContractCategory _Category;
        [Custom("Caption", "Категория контракта")]
        public crmContractCategory Category {
            get { return _Category; }
            set { _Category = value; }
        }
 
        // Исполнитель
        private crmContractDocumentType _DocumentCategory;
        [Custom("Caption", "Категория документа")]
        public crmContractDocumentType DocumentCategory {
            get { return _DocumentCategory; }
            set { _DocumentCategory = value; }
        }


        // Сортировать ли
        private bool sortByName;
        [Custom("Caption", "Сортировать по имени")]
        public bool SortByName {
            get { return sortByName; }
            set { sortByName = value; }
        }

        #endregion

    }

}
