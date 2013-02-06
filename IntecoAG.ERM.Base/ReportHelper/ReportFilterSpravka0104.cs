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
//using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.Module.ReportHelper;

namespace IntecoAG.ERM.Module.ReportHelper {
    [NonPersistent]
    public class ReportFilterSpravka0104 : BaseReportFilter //ReportParametersObjectBase, ICustomFilter
    {
        public ReportFilterSpravka0104(Session session) : base(session) { }

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
        
        // BEGIN
        private crmCParty _PrimaryParty;
        [Custom("Caption", "Первичная организация")]
        public crmCParty PrimaryParty {
            get { return _PrimaryParty; }
            set {
                SetPropertyValue<crmCParty>("PrimaryParty", ref _PrimaryParty, value);
            }
        }

        private crmCParty _ContragentParty;
        [Custom("Caption", "Контрагент")]
        public crmCParty ContragentParty {
            get { return _ContragentParty; }
            set {
                SetPropertyValue<crmCParty>("ContragentParty", ref _ContragentParty, value);
            }
        }

        
        private crmContract _Contract;
        [Custom("Caption", "Контракт")]
        public crmContract Contract {
            get { return _Contract; }
            set { SetPropertyValue<crmContract>("Contract", ref _Contract, value); }
        }

        private crmContractDeal _ContractDeal;
        [Custom("Caption", "Простой договор")]
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }
        // END

        // Этап (Финансовый)
        protected crmStage _Stage;
        [Custom("Caption", "Финансовый этап")]
        public crmStage Stage {
            get { return _Stage; }
            set {
                SetPropertyValue<crmStage>("Stage", ref _Stage, value);
            }
        }

        private csValuta _Valuta;
        [Custom("Caption", "Валюта")]
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
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

        public override CriteriaOperator GetCriteria() {
            return SearchCriteriaObjectBuilder();
        }

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

            if (this.DateBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateBegin = new BinaryOperator("ObligationUnitDateTime", this.DateBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateBegin);
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("ObligationUnitDateTime", this.DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateEnd);
            }

            // BEGIN
            CriteriaOperator criteriaCustomerAND = new GroupOperator();
            ((GroupOperator)criteriaCustomerAND).OperatorType = GroupOperatorType.And;

            
            CriteriaOperator criteriaPrimaryParty = null;
            if (this.PrimaryParty != null) {
                criteriaPrimaryParty = new BinaryOperator("PrimaryParty.Oid", PrimaryParty.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaPrimaryParty);
            }

            CriteriaOperator criteriaContragentParty = null;
            if (this.ContragentParty != null) {
                criteriaContragentParty = new BinaryOperator("ContragentParty.Oid", ContragentParty.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaContragentParty);
            }
            
            
            if (((GroupOperator)criteriaCustomerAND).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaCustomerAND);


            CriteriaOperator criteriaContract = null;
            if (this.Contract != null) {
                criteriaContract = new BinaryOperator("Contract.Oid", Contract.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaContract);
            }

            CriteriaOperator criteriaContractDeal = null;
            if (this.ContractDeal != null) {
                criteriaContractDeal = new BinaryOperator("ContractDeal.Oid", ContractDeal.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaContractDeal);
            }

            CriteriaOperator criteriaStage = null;
            if (this.Stage != null) {
                criteriaStage = new BinaryOperator("Stage.Oid", Stage.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaStage);
            }

            CriteriaOperator criteriaValuta = null;
            if (this.Valuta != null) {
                criteriaValuta = new BinaryOperator("Valuta.Oid", Valuta.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaValuta);
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

/*
        //[Action(ToolTip = "Apply Filter")]
        public void ApplyFilter() {

            Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

            LV.CollectionSource.Criteria.Clear();
            LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] =
                CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

            if ((CriteriaController as ListViewFilterPanelController) != null) (CriteriaController as ListViewFilterPanelController).criteriaFormSearchString = SearchCriteriaBuilder();

        }
*/

        //[Action(ToolTip = "Clear filter fields", Caption = "Clear filter fields")]
        public override void ClearCriteria() {

            this.DateBegin = System.DateTime.MinValue;
            this.DateEnd = System.DateTime.MinValue;

            this.Contract = null;
            this.ContractDeal = null;
            this.ContragentParty = null;
            this.PrimaryParty = null;
            this.Stage = null;
            this.Valuta = null;
        }


        // Тип объекта для отчёта
        public override Type GetReportDataSourceType() {
            return typeof(Spravka0104);
        }

        public override string GetReportFileName() {
            return "Spravka0104";
        }
        #endregion


    }

}
