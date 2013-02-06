using System;
using System.ComponentModel;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Reports;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Module;

namespace IntecoAG.ERM.FM.PaymentRequest {
    [NonPersistent]
    public class fmCPRPaymentRequestObligationFilter01 : ReportParametersObjectBase, ICustomFilter {
        public fmCPRPaymentRequestObligationFilter01(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            return SearchCriteriaObjectBuilder();
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            //                if (SortByName) {
            sorting.Add(new SortProperty("PaymentDate", SortingDirection.Descending));
            //sorting.Add(new SortProperty("Category.Name", SortingDirection.Ascending));
            //sorting.Add(new SortProperty("crmWorkPlan.Current.Supplier.Name", SortingDirection.Ascending));
            //                }
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

        // BEGIN
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
        // END

//
        private String _OrderCode;
        [Custom("Caption", "Код Заказа")]
        public String OrderCode {
            get { return _OrderCode; }
            set {
                String old = _OrderCode;
                if (old != value) {
                    _OrderCode = value;
                    if (!IsLoading) {
                        if (value != null)
                            Order = this.Session.FindObject<fmCOrderExt>(new BinaryOperator("Code", value));
                        if (String.IsNullOrEmpty(value))
                            Order = null;
                        OnChanged("OrderCode", old, value);
                    }
                }

            }
        }
        //
        private fmCOrderExt _Order;
        [Custom("Caption", "Заказ")]
        public fmCOrderExt Order {
            get { return _Order; }
            set {
                fmCOrderExt old = _Order;
                if (old != value) {
                    _Order = value;
                    if (!IsLoading) {
                        if (value != null)
                            this.OrderCode = value.Code;
                        else
                            this.OrderCode = null;
                    }
                    OnChanged("Order", old, value);
                }
            }
        }

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilder() {

            GroupOperator criteria = new GroupOperator(GroupOperatorType.And);

            // Конъюнктивные члены
            if (this.DateBegin != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("PaymentRequestBase.Date", this.DateBegin.Date, BinaryOperatorType.GreaterOrEqual));
            }
            if (this.DateEnd != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("PaymentRequestBase.Date", this.DateEnd.Date.AddDays(1), BinaryOperatorType.Less));
            }
            if (this.Order != null) {
                criteria.Operands.Add(new BinaryOperator("Order.Code", this.Order.Code, BinaryOperatorType.Equal));
            }
            if (criteria.Operands.Count > 0) {
                return criteria;
            }
            else {
                return null;
            }
        }

        private string SearchCriteriaBuilder() {
                CriteriaOperator criteria = SearchCriteriaObjectBuilder();
                return ((object)criteria) == null ? "" : criteria.LegacyToString();
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
            this.Order = null;
            this.OrderCode = null;
        }

        #endregion

    }

}
