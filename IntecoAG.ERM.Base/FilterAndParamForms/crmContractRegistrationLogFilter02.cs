using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.Module {
    [NonPersistent]
    public class crmContractRegistrationLogFilter02 : ReportParametersObjectBase, ICustomFilter
    {
        public crmContractRegistrationLogFilter02(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            return SearchCriteriaObjectBuilder();
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            if (SortByName) {
                sorting.Add(new SortProperty("SortNumber", SortingDirection.Ascending));
                //sorting.Add(new SortProperty("Category.Name", SortingDirection.Ascending));
                //sorting.Add(new SortProperty("crmWorkPlan.Current.Supplier.Name", SortingDirection.Ascending));
            }
            return sorting;
        }

        #region Для фильтра в списках ListView

        private ListView _LV;
        [Browsable(false)]
        public ListView LV {
            get { return _LV; }
            set { SetPropertyValue<ListView>("LV", ref _LV, value); }
        }

        private DetailView _DV;
        [Browsable(false)]
        public DetailView DV {
            get { return _DV; }
            set { SetPropertyValue<DetailView>("DV", ref _DV, value); }
        }

        private DevExpress.ExpressApp.DC.ITypeInfo _objectTypeInfo;
        [Browsable(false)]
        public DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo {
            get { return _objectTypeInfo; }
            set { SetPropertyValue<DevExpress.ExpressApp.DC.ITypeInfo>("objectTypeInfo", ref _objectTypeInfo, value); }
        }

        private ViewController _criteriaController;
        [Browsable(false)]
        public ViewController CriteriaController {
            get { return _criteriaController; }
            set { SetPropertyValue<ViewController>("CriteriaController", ref _criteriaController, value); }
        }

        [Browsable(false)]
        public Type ObjectType { get { return objectTypeInfo.Type; } }

        [Browsable(false)]
        public string CriterionString {
            get { return SearchCriteriaBuilder(); } // Criterion; }
        }

        private string _AdditionalCriterionString;
        [Browsable(false)]
        public string AdditionalCriterionString {
            get { return _AdditionalCriterionString; }
            set { SetPropertyValue<string>("AdditionalCriterionString", ref _AdditionalCriterionString, value); }
        }

        #endregion

        #region ПАРАМЕТРЫ НА ФОРМЕ

        protected DateTime _DateBegin;
        [Custom("Caption", "Дата с")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }

        protected DateTime _DateEnd;
        [Custom("Caption", "Дата по")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }


        // Сортировать ли
        private bool sortByName;
        [Browsable(false)]   // Временно прячем за ненадобностью
        [Custom("Caption", "Сортировать по возрастанию")]
        public bool SortByName {
            get { return sortByName; }
            set { sortByName = value; }
        }

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilder() {
            
            CriteriaOperator criteria = null;
            CriteriaOperator criteriaAND = null;
            CriteriaOperator criteriaOR = null;

            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            criteriaAND = new GroupOperator();
            ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

            criteriaOR = new GroupOperator();
            ((GroupOperator)criteriaOR).OperatorType = GroupOperatorType.Or;


            // Конъюнктивные члены

            if (!string.IsNullOrEmpty(AdditionalCriterionString)) {
                CriteriaOperator AdditionalCriterionOperator = CriteriaOperator.Parse(AdditionalCriterionString);
                ((GroupOperator)criteriaAND).Operands.Add(AdditionalCriterionOperator);
            }


            if (this.DateBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateBegin = new BinaryOperator("Date", this.DateBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateBegin);
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("Date", this.DateEnd.AddDays(1), BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateEnd);
            }

            // Совокупность критериев OR добавляем через AND к общей совокупности поисковых членов
            if (((GroupOperator)criteriaAND).Operands.Count > 0) {
                ((GroupOperator)criteria).Operands.Add(criteriaAND);
            }


            if (((GroupOperator)criteria).Operands.Count > 0) {
                return criteria;
            } else {
                return null;
            }
        }

        private string SearchCriteriaBuilder() {
            CriteriaOperator searchCriteriaObjectBuilder = SearchCriteriaObjectBuilder();
            return (object.ReferenceEquals(searchCriteriaObjectBuilder, null)) ? "" : searchCriteriaObjectBuilder.LegacyToString();
        }


        //[Action(ToolTip = "Apply Filter")]
        public void ApplyFilter() {
            if (LV == null) return;

            Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

            LV.CollectionSource.Criteria.Clear();
            LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] =
                CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

            if ((CriteriaController as ListViewFilterPanelController) != null) (CriteriaController as ListViewFilterPanelController).criteriaFormSearchString = SearchCriteriaBuilder();

        }

        public void ClearFilter() {

            this.DateBegin = System.DateTime.MinValue;
            this.DateEnd = System.DateTime.MinValue;

            if (LV != null) LV.CollectionSource.Criteria.Clear();
        }

        #endregion


    }

}
