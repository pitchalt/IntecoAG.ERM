using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Module;
//
namespace IntecoAG.ERM.FM.StatementAccount {

    [NonPersistent]
    public class fmCSAOperationJournalFilter01 : ReportParametersObjectBase, ICustomFilter {

        public fmCSAOperationJournalFilter01(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            return SearchCriteriaObjectBuilder();
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            //                if (SortByName) {
            sorting.Add(new SortProperty("PaymentRequestBase.Date", SortingDirection.Descending));
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

        // BEGIN
        private crmBank _Bank;
        [Custom("Caption", "Банк")]
        public crmBank Bank {
            get { return _Bank; }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }

        private crmBankAccount _BankAccount;
        [Custom("Caption", "Банковский Счет")]
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
                if (!IsLoading && value != null) {
                    Bank = value.Bank;
                }
            }
        }
        // END

        private fmCDocRCB _PaymentDoc;
        [Custom("Caption", "Платежный документ")]
        public fmCDocRCB PaymentDoc {
            get { return _PaymentDoc; }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDoc", ref _PaymentDoc, value);
            }
        }

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilder() {

            GroupOperator criteria = new GroupOperator(GroupOperatorType.And);

            // Конъюнктивные члены
            if (this.DateBegin != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("OperationDate", this.DateBegin.Date, BinaryOperatorType.GreaterOrEqual));
            }
            if (this.DateEnd != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("OperationDate", this.DateEnd.Date.AddDays(1), BinaryOperatorType.Less));
            }
            if (this.Bank != null) {
                criteria.Operands.Add(new BinaryOperator("Bank", this.Bank, BinaryOperatorType.Equal));
            }
            if (this.BankAccount != null) {
                criteria.Operands.Add(new BinaryOperator("BankAccount", this.BankAccount, BinaryOperatorType.Equal));
            }
            if (this.PaymentDoc != null) {
                criteria.Operands.Add(new BinaryOperator("PaymentDoc", this.PaymentDoc, BinaryOperatorType.Equal));
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
            this.Bank = null;
            this.BankAccount = null;
            this.PaymentDoc = null;

        }

        #endregion

    }
}
