﻿using System;
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
namespace IntecoAG.ERM.FM.PaymentRequest {

    [NonPersistent]
    public class fmCPRRepaymentTaskFilter01 : ReportParametersObjectBase, ICustomFilter {

        public fmCPRRepaymentTaskFilter01(Session session)
            : base(session) {
        }

        public override CriteriaOperator GetCriteria() {
            return SearchCriteriaObjectBuilder();
        }

        public override SortingCollection GetSorting() {
            SortingCollection sorting = new SortingCollection();
            //                if (SortByName) {
            sorting.Add(new SortProperty("DocDate", SortingDirection.Descending));
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

        private RepaymentTaskStates _State;   // Статус
        [Custom("Caption", "Статус")]
        public RepaymentTaskStates State {
            get {
                return _State;
            }
            set {
                SetPropertyValue<RepaymentTaskStates>("State", ref _State, value);
            }
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

        //private fmCSAImportResult _ImportResult;
        //[Custom("Caption", "Результат импорта")]
        //public fmCSAImportResult ImportResult {
        //    get {
        //        return _ImportResult;
        //    }
        //    set {
        //        SetPropertyValue<fmCSAImportResult>("ImportResult", ref _ImportResult, value);
        //    }
        //}

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilder() {

            GroupOperator criteria = new GroupOperator(GroupOperatorType.And);

            // Конъюнктивные члены
            if (this.DateBegin != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("Date", this.DateBegin.Date, BinaryOperatorType.GreaterOrEqual));
            }
            if (this.DateEnd != System.DateTime.MinValue) {
                criteria.Operands.Add(new BinaryOperator("Date", this.DateEnd.Date.AddDays(1), BinaryOperatorType.Less));
            }
            if (this.Bank != null) {
                criteria.Operands.Add(new BinaryOperator("BankAccount.Bank", this.Bank, BinaryOperatorType.Equal));
            }
            if (this.BankAccount != null) {
                criteria.Operands.Add(new BinaryOperator("BankAccount", this.BankAccount, BinaryOperatorType.Equal));
            }
            //if (this.ImportResult != null) {
            //    criteria.Operands.Add(new BinaryOperator("ImportResult", this.ImportResult, BinaryOperatorType.Equal));
            //}
            if (!String.IsNullOrEmpty(this.Comment)) {
                criteria.Operands.Add(new BinaryOperator("Comment", this.Comment, BinaryOperatorType.Like));
            }
            //if (this.PaymentRequest != null) {
            //    criteria.Operands.Add(new BinaryOperator("PaymentRequest", this.PaymentRequest, BinaryOperatorType.Equal));
            //}
            if (this.State != 0) {
                criteria.Operands.Add(new BinaryOperator("State", this.State, BinaryOperatorType.Equal));
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
            Comment = "";
            State = 0;
                //this.ImportResult = null;

        }

        #endregion

    }
}
