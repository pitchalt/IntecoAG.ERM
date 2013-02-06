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
using IntecoAG.ERM.Module;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.PaymentRequest {
    [NonPersistent]
    public class fmCPRRegistratorFilter01 : ReportParametersObjectBase, ICustomFilter {
        public fmCPRRegistratorFilter01(Session session) : base(session) {
        }

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
        protected DateTime _DateRegBegin;
        [Custom("Caption", "Дата рег. с")]
        public DateTime DateRegBegin {
            get { return _DateRegBegin; }
            set { 
                DateTime old = _DateRegBegin;
                if (old != value) {
                    _DateRegBegin = value;
                    if (!IsLoading) {
                        if (DateRegEnd == old || DateRegEnd == default(DateTime))
                            DateRegEnd = value;
                        OnChanged("DateRegBegin", old, value);
                    }
                }
            }
        }

        protected DateTime _DateRegEnd;
        [Custom("Caption", "Дата рег. по")]
        public DateTime DateRegEnd {
            get { return _DateRegEnd; }
            set { SetPropertyValue<DateTime>("DateRegEnd", ref _DateRegEnd, value); }
        }

        protected DateTime _DateInvoiceBegin;
        [Custom("Caption", "Дата док. с")]
        public DateTime DateInvoiceBegin {
            get { return _DateInvoiceBegin; }
            set {
                DateTime old = _DateInvoiceBegin;
                if (old != value) {
                    _DateInvoiceBegin = value;
                    if (!IsLoading) {
                        if (DateInvoiceEnd == old || DateInvoiceEnd == default(DateTime))
                            DateInvoiceEnd = value;
                        OnChanged("DateInvoiceBegin", old, value);
                    }
                }
            }
        }

        protected DateTime _DateInvoiceEnd;
        [Custom("Caption", "Дата док. по")]
        public DateTime DateInvoiceEnd {
            get { return _DateInvoiceEnd; }
            set { SetPropertyValue<DateTime>("DateInvoiceEnd", ref _DateInvoiceEnd, value); }
        }
        // END
        private String _InvoiceNumber; //
        [Custom("Caption", "Номер док.")]
        public String InvoiceNumber {
            get { return _InvoiceNumber; }
            set { SetPropertyValue<String>("InvoiceNumber", ref _InvoiceNumber, value); }
        }
        // 
        private PaymentRequestStates _State; // Состояние заявки
        [Custom("Caption", "Статус")]
        public PaymentRequestStates State {
            get { return _State; }
            set { SetPropertyValue<PaymentRequestStates>("State", ref _State, value); }
        }


        private String _Comment; //
        [Custom("Caption", "Примечание")]
        public String Comment {
            get {
                return _Comment;
            }
            set {
                SetPropertyValue<String>("Comment", ref _Comment, value);
            }
        }

        private fmCPRPaymentRequest _PaymentRequest;
        [Custom("Caption", "Заявка")]
        public fmCPRPaymentRequest PaymentRequest {
            get {
                return _PaymentRequest;
            }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
            }
        }

        private crmCParty _Receiver;
        [Custom("Caption", "Получатель")]
        public crmCParty Receiver {
            get {
                return _Receiver;
            }
            set {
                SetPropertyValue<crmCParty>("Receiver", ref _Receiver, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilder() {
            GroupOperator criteria = new GroupOperator(GroupOperatorType.And);
            // Конъюнктивные члены
            if (this.DateRegBegin != default(DateTime)) {
                criteria.Operands.Add(new BinaryOperator("Date", this.DateRegBegin.Date, BinaryOperatorType.GreaterOrEqual));
            }
            if (this.DateRegEnd != default(DateTime)) {
                criteria.Operands.Add(new BinaryOperator("Date", this.DateRegEnd.Date.AddDays(1), BinaryOperatorType.Less));
            }
            if (this.DateInvoiceBegin != default(DateTime)) {
                criteria.Operands.Add(new BinaryOperator("InvoiceDate", this.DateInvoiceBegin.Date, BinaryOperatorType.GreaterOrEqual));
            }
            if (this.DateInvoiceEnd != default(DateTime)) {
                criteria.Operands.Add(new BinaryOperator("InvoiceDate", this.DateInvoiceEnd.Date.AddDays(1), BinaryOperatorType.Less));
            }
            if (this.State != default(PaymentRequestStates)) {
                criteria.Operands.Add(new BinaryOperator("State", this.State, BinaryOperatorType.Equal));
            }
            if (!String.IsNullOrEmpty(this.InvoiceNumber)) {
                criteria.Operands.Add(new BinaryOperator("InvoiceNumber", this.InvoiceNumber, BinaryOperatorType.Like));
            }
            if (!String.IsNullOrEmpty(this.Comment)) {
                criteria.Operands.Add(new BinaryOperator("Comment", this.Comment, BinaryOperatorType.Like));
            }
            if (this.PaymentRequest != null) {
                criteria.Operands.Add(new BinaryOperator("PaymentRequest", this.PaymentRequest, BinaryOperatorType.Equal));
            }
            if (this.Receiver != null) {
                criteria.Operands.Add(new BinaryOperator("Receiver", this.Receiver, BinaryOperatorType.Equal));
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
            this.DateRegBegin = default(DateTime);
            this.DateRegEnd = default(DateTime);
            this.DateInvoiceBegin = default(DateTime);
            this.DateInvoiceEnd = default(DateTime);
            this.InvoiceNumber = "";
            this.State = 0;
            Receiver = null;
            PaymentRequest = null;
            Comment = "";
        }

        #endregion

    }

}
