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


        #region ��������� �� �����

        // ��������
        private crmContractCategory _Category;
        [Custom("Caption", "��������� ���������")]
        public crmContractCategory Category {
            get { return _Category; }
            set { _Category = value; }
        }
 
        // �����������
        private crmContractDocumentType _DocumentCategory;
        [Custom("Caption", "��������� ���������")]
        public crmContractDocumentType DocumentCategory {
            get { return _DocumentCategory; }
            set { _DocumentCategory = value; }
        }


        // ����������� ��
        private bool sortByName;
        [Custom("Caption", "����������� �� �����")]
        public bool SortByName {
            get { return sortByName; }
            set { sortByName = value; }
        }

        #endregion

    }

}
