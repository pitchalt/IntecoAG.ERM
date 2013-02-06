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

namespace IntecoAG.ERM.Module {
    [NonPersistent]
    public class crmCashFlowRegisterFilter01 : ReportParametersObjectBase, ICustomFilter
    {
        public crmCashFlowRegisterFilter01(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            //return CriteriaOperator.Parse("Current.Category = ? AND Current.DocumentCategory = ?", Category, DocumentCategory);
            //return CriteriaOperator.Parse("Date >= ? AND Date <= ? AND DepartmentRegistrator.Oid = ?", DateBegin, DateEnd, Department.Oid);
            
            //return CriteriaOperator.Parse("Date >= ? AND Date <= ? AND DepartmentRegistrator = ?", DateBegin, DateEnd, Department);
            //return CriteriaOperator.Parse("1 = 1");

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

        //private PivotGrid _PG;
        //[Browsable(false)]
        //public ListView PG {
        //    get { return _PG; }
        //    set { SetPropertyValue<ListView>("PG", ref _PG, value); }
        //}

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

        //private DealStates _DealState;
        //[Custom("Caption", "Статус")]
        //public DealStates DealState {
        //    get { return _DealState; }
        //    set {
        //        SetPropertyValue<DealStates>("DealState", ref _DealState, value);
        //    }
        //}

        // BEGIN
        private crmPartyRu _PrimaryParty;
        [Custom("Caption", "Первичная организация")]
        public crmPartyRu PrimaryParty {
            get { return _PrimaryParty; }
            set {
                SetPropertyValue<crmPartyRu>("PrimaryParty", ref _PrimaryParty, value);
            }
        }

        private crmPartyRu _ContragentParty;
        [Custom("Caption", "Контрагент")]
        public crmPartyRu ContragentParty {
            get { return _ContragentParty; }
            set {
                SetPropertyValue<crmPartyRu>("ContragentParty", ref _ContragentParty, value);
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

        //BEGIN
        private fmCostItem _CostItem;
        [Custom("Caption", "Исполнитель")]
        public fmCostItem CostItem {
            get { return _CostItem; }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }

        private SubjectClass _Subject;
        [Custom("Caption", "Тема")]
        public SubjectClass Subject {
            get { return _Subject; }
            set { SetPropertyValue<SubjectClass>("Subject", ref _Subject, value); }
        }
        // END

        protected crmObligationUnit _ObligationUnit;
        [Custom("Caption", "Обязательство")]
        public crmObligationUnit ObligationUnit {
            get { return _ObligationUnit; }
            set {
                SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value);
            }
        }

        protected fmOrder _fmOrder;
        [Custom("Caption", "Заказ")]
        //[Custom("Caption", (false) ? "Регистрир. сотр." : "WWWWWWWWWWWWWWWW")]
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value);
            }
        }

        // Этап (Финансовый)
        protected crmStage _Stage;
        [Custom("Caption", "Финансовый этап")]
        public crmStage Stage {
            get { return _Stage; }
            set {
                SetPropertyValue<crmStage>("Stage", ref _Stage, value);
            }
        }

        // Этап (технический)
        protected crmStage _StageTech;
        [Custom("Caption", "Технический этап")]
        public crmStage StageTech {
            get { return _StageTech; }
            set {
                SetPropertyValue<crmStage>("StageTech", ref _StageTech, value);
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

        private decimal _PriceFrom;
        [Custom("Caption", "Цена от")]
        public decimal PriceFrom {
            get { return _PriceFrom; }
            set {
                SetPropertyValue<decimal>("PriceFrom", ref _PriceFrom, value);
            }
        }

        private decimal _PriceTo;
        [Custom("Caption", "Цена до")]
        public decimal PriceTo {
            get { return _PriceTo; }
            set {
                SetPropertyValue<decimal>("PriceTo", ref _PriceTo, value);
            }
        }

        // Учесть также PaymentCost (decimal), PaymentItem (crmPaymentItem), PaymentValuta (csValuta)

        private csValuta _PaymentValuta;
        [Custom("Caption", "Валюта обязательства оплаты")]
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
            }
        }

        private decimal _PaymentPriceFrom;
        [Custom("Caption", "Цена обязательства оплаты от")]
        public decimal PaymentPriceFrom {
            get { return _PaymentPriceFrom; }
            set {
                SetPropertyValue<decimal>("PaymentPriceFrom", ref _PaymentPriceFrom, value);
            }
        }

        private decimal _PaymentPriceTo;
        [Custom("Caption", "Цена обязательства оплаты до")]
        public decimal PaymentPriceTo {
            get { return _PaymentPriceTo; }
            set {
                SetPropertyValue<decimal>("PaymentPriceTo", ref _PaymentPriceTo, value);
            }
        }


        private PlaneFact _PlaneFact;
        [Custom("Caption", "План/Факт")]
        public PlaneFact PlaneFact {
            get { return _PlaneFact; }
            set {
                SetPropertyValue<PlaneFact>("PlaneFact", ref _PlaneFact, value);
            }
        }
        

        // Поиск по LIKE, присоединяется через OR
        private string _DocumentNumber;
        [Custom("Caption", "Номер документа")]
        public string DocumentNumber {
            get { return _DocumentNumber; }
            set {
                SetPropertyValue<string>("DocumentNumber", ref _DocumentNumber, value);
            }
        }

        // Поиск по LIKE, присоединяется через OR
        private string _Description;
        [Custom("Caption", "Описание")]
        public string Description {
            get { return _Description; }
            set {
                SetPropertyValue<string>("Description", ref _Description, value);
            }
        }



        /*
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
        */

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

        private CriteriaOperator SearchCriteriaObjectBuilderOLD() {
            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            if (this.DateBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateBegin = new BinaryOperator("Date", this.DateBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriathisDateBegin);
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("Date", this.DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriathisDateEnd);
            }


            CriteriaOperator criteriaContract = null;
            if (this.Contract != null) {
                criteriaContract = new BinaryOperator("Contract.Oid", this.Contract.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteria).Operands.Add(criteriaContract);
            }

            if (((GroupOperator)criteria).Operands.Count > 0) {
                return criteria;
            } else {
                return null;
            }
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

            if (!string.IsNullOrEmpty(AdditionalCriterionString)) {
                CriteriaOperator AdditionalCriterionOperator = CriteriaOperator.Parse(AdditionalCriterionString);
                ((GroupOperator)criteriaAND).Operands.Add(AdditionalCriterionOperator);
            }


            if (this.DateBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateBegin = new BinaryOperator("ObligationUnitDateTime", this.DateBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateBegin);
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("ObligationUnitDateTime", this.DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateEnd);
            }


            //CriteriaOperator criteriaDepartment = null;
            //if (this.Department != null) {
            //    criteriaDepartment = new BinaryOperator("DepartmentRegistrator.Oid", this.Department.Oid, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteriaAND).Operands.Add(criteriaDepartment);
            //}

            //CriteriaOperator criteriaDealState = null;
            //if (this.DealState != 0) {
            //    criteriaDealState = new BinaryOperator("DealState", DealState, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteriaAND).Operands.Add(criteriaDealState);
            //}

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

            CriteriaOperator criteriaCostItem = null;
            if (this.CostItem != null) {
                criteriaCostItem = new BinaryOperator("CostItem.Oid", CostItem.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaCostItem);
            }

            CriteriaOperator criteriaSubject = null;
            if (this.Subject != null) {
                criteriaSubject = new BinaryOperator("Subject.Oid", Subject.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaSubject);
            }

            CriteriaOperator criteriafmOrder = null;
            if (this.fmOrder != null) {
                criteriafmOrder = new BinaryOperator("fmOrder.Oid", fmOrder.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriafmOrder);
            }

            CriteriaOperator criteriaStage = null;
            if (this.Stage != null) {
                criteriaStage = new BinaryOperator("Stage.Oid", Stage.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaStage);
            }

            CriteriaOperator criteriaStageTech = null;
            if (this.StageTech != null) {
                criteriaStageTech = new BinaryOperator("StageTech.Oid", StageTech.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaStageTech);
            }

            // END
            
            /*
            // BEGIN
            CriteriaOperator criteriaSupplierOR = new GroupOperator();
            ((GroupOperator)criteriaSupplierOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaSupplier = null;
            if (this.Supplier != null) {
                criteriaSupplier = new BinaryOperator("Supplier.Party.Oid", Supplier.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaSupplierOR).Operands.Add(criteriaSupplier);
            }

            CriteriaOperator criteriaPersonOfSupplier = null;
            if (this.PersonOfSupplier != null) {
                criteriaPersonOfSupplier = new BinaryOperator("Supplier.Party.Person.Oid", PersonOfSupplier.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaSupplierOR).Operands.Add(criteriaPersonOfSupplier);
            }

            if (((GroupOperator)criteriaSupplierOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaSupplierOR);
            // END

            CriteriaOperator criteriaCurator = null;
            if (this.Curator != null) {
                criteriaCurator = new BinaryOperator("Curator.Oid", Curator.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCurator);
            }

            CriteriaOperator criteriaUserRegistrator = null;
            if (this.UserRegistrator != null) {
                criteriaUserRegistrator = new BinaryOperator("UserRegistrator.Oid", UserRegistrator.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaUserRegistrator);
            }
            */

            CriteriaOperator criteriaValuta = null;
            if (this.Valuta != null) {
                criteriaValuta = new BinaryOperator("Valuta.Oid", Valuta.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaValuta);
            }

            if (this.PriceFrom != 0) {
                CriteriaOperator criteriathisPriceFrom = new BinaryOperator("CostInRUR", this.PriceFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisPriceFrom);
            }

            if (this.PriceTo != 0) {
                CriteriaOperator criteriathisPriceTo = new BinaryOperator("CostInRUR", this.PriceTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisPriceTo);
            }

            CriteriaOperator criteriaPaymentValuta = null;
            if (this.PaymentValuta != null) {
                criteriaPaymentValuta = new BinaryOperator("PaymentValuta.Oid", PaymentValuta.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaPaymentValuta);
            }

            if (this.PaymentPriceFrom != 0) {
                CriteriaOperator criteriaPaymentPriceFrom = new BinaryOperator("PaymentCost", this.PaymentPriceFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaPaymentPriceFrom);
            }

            if (this.PaymentPriceTo != 0) {
                CriteriaOperator criteriaPaymentPriceTo = new BinaryOperator("PaymentCost", this.PaymentPriceTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaPaymentPriceTo);
            }

            if (this.PlaneFact != 0) {
                CriteriaOperator criteriaPlaneFact = new BinaryOperator("PlaneFact ", this.PlaneFact, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaPlaneFact);
            }

            // Совокупность критериев OR добавляем через AND к общей совокупности поисковых членов
            if (((GroupOperator)criteriaAND).Operands.Count > 0) {
                ((GroupOperator)criteria).Operands.Add(criteriaAND);
            }


            // Дизъюнктивные члены
            CriteriaOperator criteriaDocumentNumber = null;
            if (!string.IsNullOrEmpty(this.DocumentNumber)) {
                //criteriaDocumentNumber = new BinaryOperator("DealVersion.ContractDocument.Number", "%" + this.DocumentNumber + "%", BinaryOperatorType.Like);
                criteriaDocumentNumber = new BinaryOperator("ContractDocument.Number", "%" + this.DocumentNumber + "%", BinaryOperatorType.Like);
                ((GroupOperator)criteriaOR).Operands.Add(criteriaDocumentNumber);
            }

            CriteriaOperator criteriaDescriptionShort = null;
            if (!string.IsNullOrEmpty(this.Description)) {
                //criteriaDescriptionShort = new BinaryOperator("DealVersion.DescriptionShort", "%" + this.Description + "%", BinaryOperatorType.Like);
                criteriaDescriptionShort = new BinaryOperator("DescriptionShort", "%" + this.Description + "%", BinaryOperatorType.Like);
                ((GroupOperator)criteriaOR).Operands.Add(criteriaDescriptionShort);
            }

            // Совокупность критериев OR добавляем через AND к общей совокупности поисковых членов
            if (((GroupOperator)criteriaOR).Operands.Count > 0) {
                ((GroupOperator)criteria).Operands.Add(criteriaOR);
            }



            if (((GroupOperator)criteria).Operands.Count > 0) {
                return criteria;
            } else {
                return null;
            }
        }

        private string SearchCriteriaBuilder() {
            //string res = "";

            /*
            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            if (this.DateBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateBegin = new BinaryOperator("Date", this.DateBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriathisDateBegin);
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("Date", this.DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriathisDateEnd);
            }


            CriteriaOperator criteriaDepartment = null;
            if (this.Department != null) {
                criteriaDepartment = new BinaryOperator("DepartmentRegistrator.Oid", this.Department.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteria).Operands.Add(criteriaDepartment);
            }

            if (((GroupOperator)criteria).Operands.Count > 0) res = criteria.LegacyToString();
            */


            /*
            if (this.DateBegin != System.DateTime.MinValue) {
                res += ((res == "") ? "" : " AND ") + "DateBegin >= " + this.DateBegin;
            }

            if (this.DateEnd != System.DateTime.MinValue) {
                res += ((res == "") ? "" : " AND ") + "DateBegin  " + this.DateBEgin;
            }

            if (this.Department != null) {
                res += ((res == "") ? "" : " AND ") + "DepartmentRegistrator.Oid = " + this.Department.Oid.ToString();
            }
            //if (!(string.IsNullOrEmpty(this.SupplierName))) {
            //    res += ((res == "") ? "" : " AND ") + "Current.Supplier.Name" + " like '%" + this.SupplierName + "%'";
            //    res += ((res == "") ? "" : " AND ") + "Current.crmWorkPlan.Current.Supplier.Name" + " like '%" + this.SupplierName + "%'";
            //}
            */

            CriteriaOperator searchCriteriaObjectBuilder = SearchCriteriaObjectBuilder();
            return (object.ReferenceEquals(searchCriteriaObjectBuilder, null)) ? "" : searchCriteriaObjectBuilder.LegacyToString();
        }


        //[Action(ToolTip = "Apply Filter")]
        public void ApplyFilter() {

            Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

            LV.CollectionSource.Criteria.Clear();
            LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] =
                CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

            if ((CriteriaController as ListViewFilterPanelController) != null) (CriteriaController as ListViewFilterPanelController).criteriaFormSearchString = SearchCriteriaBuilder();

        }

        public void ClearFilter() {

            // Очистка полей пока бессмысленна, т.к. они всё равно нигде не запоминаются
            this.DateBegin = System.DateTime.MinValue;
            this.DateEnd = System.DateTime.MinValue;
            //this.Department = null;

            LV.CollectionSource.Criteria.Clear();
        }

        #endregion


    }

}
