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

namespace IntecoAG.ERM.Module {
    [NonPersistent]
    public class crmContractRegistrationLogFilter01 : ReportParametersObjectBase, ICustomFilter
    {
        public crmContractRegistrationLogFilter01(Session session) : base(session) { }

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

        // === Добавлены 2011-08-28 ===

        private DealStates _DealState;
        [Custom("Caption", "Статус")]
        public DealStates DealState {
            get { return _DealState; }
            set {
                SetPropertyValue<DealStates>("DealState", ref _DealState, value);
            }
        }

        // BEGIN
        private crmPartyRu _Customer;
        [Custom("Caption", "Заказчик")]
        public crmPartyRu Customer {
            get { return _Customer; }
            set {
                SetPropertyValue<crmPartyRu>("Customer", ref _Customer, value);
            }
        }

        private crmCPerson _PersonOfCustomer;
        [Custom("Caption", "или его персона (з-чик)")]
        public crmCPerson PersonOfCustomer {
            get { return _PersonOfCustomer; }
            set { SetPropertyValue<crmCPerson>("PersonOfCustomer", ref _PersonOfCustomer, value); }
        }
        // END

        //BEGIN
        private crmPartyRu _Supplier;
        [Custom("Caption", "Исполнитель")]
        public crmPartyRu Supplier {
            get { return _Supplier; }
            set {
                SetPropertyValue<crmPartyRu>("Supplier", ref _Supplier, value);
            }
        }

        private crmCPerson _PersonOfSupplier;
        [Custom("Caption", "или его персона (исп-ль)")]
        public crmCPerson PersonOfSupplier {
            get { return _PersonOfSupplier; }
            set { SetPropertyValue<crmCPerson>("PersonOfSupplier", ref _PersonOfSupplier, value); }
        }
        // END

        protected hrmDepartment _Curator;
        [Custom("Caption", "Куратор")]
        public hrmDepartment Curator {
            get { return _Curator; }
            set {
                SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value);
            }
        }

        protected hrmStaff _UserRegistrator;
        [Custom("Caption", "Регистрир. сотр.")]
        //[Custom("Caption", (false) ? "Регистрир. сотр." : "WWWWWWWWWWWWWWWW")]
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator; }
            set {
                SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value);
            }
        }

        // Подразделение
        protected hrmDepartment _Department;
        [Custom("Caption", "Регистр. подразделение")]
        public hrmDepartment Department {
            get { return _Department; }
            set {
                SetPropertyValue<hrmDepartment>("Department", ref _Department, value);
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
                CriteriaOperator criteriathisDateEnd = new BinaryOperator("Date", this.DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateEnd);
            }


            CriteriaOperator criteriaDepartment = null;
            if (this.Department != null) {
                criteriaDepartment = new BinaryOperator("DepartmentRegistrator.Oid", this.Department.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaDepartment);
            }

            CriteriaOperator criteriaDealState = null;
            if (this.DealState != 0) {
                criteriaDealState = new BinaryOperator("DealState", DealState, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaDealState);
            }

            // BEGIN
            CriteriaOperator criteriaCustomerOR = new GroupOperator();
            ((GroupOperator)criteriaCustomerOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaCustomer = null;
            if (this.Customer != null) {
                criteriaCustomer = new BinaryOperator("Customer.Party.Oid", Customer.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerOR).Operands.Add(criteriaCustomer);
            }

            CriteriaOperator criteriaPersonOfCustomer = null;
            if (this.PersonOfCustomer != null) {
                criteriaPersonOfCustomer = new BinaryOperator("Customer.Party.Person.Oid", PersonOfCustomer.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerOR).Operands.Add(criteriaPersonOfCustomer);
            }

            if (((GroupOperator)criteriaCustomerOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaCustomerOR);
            // END

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

            CriteriaOperator criteriaValuta = null;
            if (this.Valuta != null) {
                criteriaValuta = new BinaryOperator("Valuta.Oid", Valuta.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaValuta);
            }

            if (this.PriceFrom != 0) {
                CriteriaOperator criteriathisPriceFrom = new BinaryOperator("Price", this.PriceFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisPriceFrom);
            }

            if (this.PriceTo != 0) {
                CriteriaOperator criteriathisPriceTo = new BinaryOperator("Price", this.PriceTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisPriceTo);
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

            this.DateBegin = System.DateTime.MinValue;
            this.DateEnd = System.DateTime.MinValue;
            this.Curator = null;
            this.Customer = null;
            this.DealState = 0;
            this.Description = "";
            this.DocumentNumber = "";
            this.Department = null;
            this.PersonOfCustomer = null;
            this.PriceFrom = 0;
            this.PriceTo = 0;
            this.PersonOfSupplier = null;
            this.Supplier = null;
            this.UserRegistrator = null;
            this.Valuta = null;

            if (LV != null) LV.CollectionSource.Criteria.Clear();
        }

        #endregion


    }

}
