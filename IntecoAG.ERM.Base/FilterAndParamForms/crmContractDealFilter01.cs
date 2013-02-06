using System;
using System.ComponentModel;
using System.Configuration;

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
    public class crmContractDealFilter01 : ReportParametersObjectBase, ICustomFilter {
        public crmContractDealFilter01(Session session) : base(session) { }

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

        protected DealStates _ContractStatus;
        [Custom("Caption", "Статус")]
        public DealStates ContractStatus {
            get { return _ContractStatus; }
            set { SetPropertyValue<DealStates>("ContractStatus", ref _ContractStatus, value); }
        }

        // BEGIN
        protected DateTime _DateRegistrationBegin;
        [Custom("Caption", "Дата регистрации с")]
        public DateTime DateRegistrationBegin {
            get { return _DateRegistrationBegin; }
            set { SetPropertyValue<DateTime>("DateRegistrationBegin", ref _DateRegistrationBegin, value); }
        }

        protected DateTime _DateRegistrationEnd;
        [Custom("Caption", "Дата регистрации по")]
        public DateTime DateRegistrationEnd {
            get { return _DateRegistrationEnd; }
            set { SetPropertyValue<DateTime>("DateRegistrationEnd", ref _DateRegistrationEnd, value); }
        }
        // END

        // BEGIN
        protected DateTime _DateFinishBegin;
        [Custom("Caption", "Срок завершения с")]
        public DateTime DateFinishBegin {
            get { return _DateFinishBegin; }
            set { SetPropertyValue<DateTime>("DateFinishBegin", ref _DateFinishBegin, value); }
        }

        protected DateTime _DateFinishEnd;
        [Custom("Caption", "Срок завершения по")]
        public DateTime DateFinishEnd {
            get { return _DateFinishEnd; }
            set { SetPropertyValue<DateTime>("DateFinishEnd", ref _DateFinishEnd, value); }
        }
        // END

        // BEGIN
        private crmCParty _Customer;
        [Custom("Caption", "Заказчик")]
        public crmCParty Customer {
            get { return _Customer; }
            set {
                SetPropertyValue<crmCParty>("Customer", ref _Customer, value);
            }
        }

        private crmCPerson _PersonOfCustomer;
        [Custom("Caption", "Заказчик юр. лицо")]
        public crmCPerson PersonOfCustomer {
            get { return _PersonOfCustomer; }
            set { SetPropertyValue<crmCPerson>("PersonOfCustomer", ref _PersonOfCustomer, value); }
        }
        // END

        //BEGIN
        private crmCParty _Supplier;
        [Custom("Caption", "Исполнитель")]
        public crmCParty Supplier {
            get { return _Supplier; }
            set {
                SetPropertyValue<crmCParty>("Supplier", ref _Supplier, value);
            }
        }

        private crmCPerson _PersonOfSupplier;
        [Custom("Caption", "Исполнитель юр. лицо")]
        public crmCPerson PersonOfSupplier {
            get { return _PersonOfSupplier; }
            set { SetPropertyValue<crmCPerson>("PersonOfSupplier", ref _PersonOfSupplier, value); }
        }
        // END

        //protected hrmDepartment _Curator;
        //[Custom("Caption", "Куратор")]
        //public hrmDepartment Curator {
        //    get { return _Curator; }
        //    set {
        //        SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value);
        //    }
        //}
        // Регистрирующий пользователь: Пользователь, осуществляющий регистрацию
        private hrmStaff _UserRegistrator;
        [Custom("Caption", "Регистр. пользователь")]
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator; }
            set { SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value); }
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
            if (this.DateRegistrationBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateRegistrationBegin = new BinaryOperator("DateRegistration", this.DateRegistrationBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateRegistrationBegin);
            }

            if (this.DateRegistrationEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateRegistrationEnd = new BinaryOperator("DateRegistration", this.DateRegistrationEnd.AddDays(1), BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateRegistrationEnd);
            }


            if (this.DateFinishBegin != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateFinishBegin = new BinaryOperator("Current.DateFinish", this.DateFinishBegin, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateFinishBegin);
            }

            if (this.DateFinishEnd != System.DateTime.MinValue) {
                CriteriaOperator criteriathisDateFinishEnd = new BinaryOperator("Current.DateFinish", this.DateFinishEnd.AddDays(1), BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriathisDateFinishEnd);
            }



            CriteriaOperator criteriaContractStatus = null;
            if (this.ContractStatus != 0) {
                criteriaContractStatus = new BinaryOperator("State", ContractStatus, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaContractStatus);
            }


            /*
            // BEGIN
            CriteriaOperator criteriaCustomerOR = new GroupOperator();
            ((GroupOperator)criteriaCustomerOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaPersonOfCustomer = null;
            if (this.PersonOfCustomer != null) {
                //criteriaPersonOfCustomer = new BinaryOperator("Current.Customer.Party.Person.Oid", PersonOfCustomer.Oid, BinaryOperatorType.Equal);
                criteriaPersonOfCustomer = new BinaryOperator("Customer.Person.Oid", PersonOfCustomer.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerOR).Operands.Add(criteriaPersonOfCustomer);
            }

            if (((GroupOperator)criteriaCustomerOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaCustomerOR);
            // END

            // BEGIN
            CriteriaOperator criteriaSupplierOR = new GroupOperator();
            ((GroupOperator)criteriaSupplierOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaPersonOfSupplier = null;
            if (this.PersonOfSupplier != null) {
                //criteriaPersonOfSupplier = new BinaryOperator("Current.Supplier.Party.Person.Oid", PersonOfSupplier.Oid, BinaryOperatorType.Equal);
                criteriaPersonOfSupplier = new BinaryOperator("Supplier.Person.Oid", PersonOfSupplier.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaSupplierOR).Operands.Add(criteriaPersonOfSupplier);
            }

            if (((GroupOperator)criteriaSupplierOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaSupplierOR);
            // END
            */


            // BEGIN
            CriteriaOperator criteriaCustomerOR = new GroupOperator();
            ((GroupOperator)criteriaCustomerOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaCustomer = null;
            if (this.Customer != null) {
                //criteriaCustomer = new BinaryOperator("Customer.Party.Oid", Customer.Oid, BinaryOperatorType.Equal);
                criteriaCustomer = new BinaryOperator("Current.Customer.Party.Oid", Customer.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerOR).Operands.Add(criteriaCustomer);
            }

            CriteriaOperator criteriaPersonOfCustomer = null;
            if (this.PersonOfCustomer != null) {
                //criteriaPersonOfCustomer = new BinaryOperator("Current.Customer.Party.Person.Oid", PersonOfCustomer.Oid, BinaryOperatorType.Equal);
                criteriaPersonOfCustomer = new BinaryOperator("Customer.Person.Oid", PersonOfCustomer.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerOR).Operands.Add(criteriaPersonOfCustomer);
            }

            if (((GroupOperator)criteriaCustomerOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaCustomerOR);
            // END

            // BEGIN
            CriteriaOperator criteriaSupplierOR = new GroupOperator();
            ((GroupOperator)criteriaSupplierOR).OperatorType = GroupOperatorType.Or;

            CriteriaOperator criteriaSupplier = null;
            if (this.Supplier != null) {
                criteriaSupplier = new BinaryOperator("Current.Supplier.Party.Oid", Supplier.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaSupplierOR).Operands.Add(criteriaSupplier);
            }

            CriteriaOperator criteriaPersonOfSupplier = null;
            if (this.PersonOfSupplier != null) {
                //criteriaPersonOfSupplier = new BinaryOperator("Current.Supplier.Party.Person.Oid", PersonOfSupplier.Oid, BinaryOperatorType.Equal);
                criteriaPersonOfSupplier = new BinaryOperator("Supplier.Person.Oid", PersonOfSupplier.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaSupplierOR).Operands.Add(criteriaPersonOfSupplier);
            }

            if (((GroupOperator)criteriaSupplierOR).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaSupplierOR);
            // END

            //CriteriaOperator criteriaCurator = null;
            //if (this.Curator != null) {
            //    criteriaCurator = new BinaryOperator("CuratorDepartment.Oid", Curator.Oid, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteriaAND).Operands.Add(criteriaCurator);
            //}

            CriteriaOperator criteriaUserRegistrator = null;
            if (this.UserRegistrator != null) {
                criteriaUserRegistrator = new BinaryOperator("UserRegistrator.Oid", UserRegistrator.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaUserRegistrator);
            }


            // Совокупность критериев OR добавляем через AND к общей совокупности поисковых членов
            if (((GroupOperator)criteriaAND).Operands.Count > 0) {
                ((GroupOperator)criteria).Operands.Add(criteriaAND);
            }


            // Дизъюнктивные члены
            CriteriaOperator criteriaDescriptionShort = null;
            if (!string.IsNullOrEmpty(this.Description)) {
                //criteriaDescriptionShort = new BinaryOperator("DealVersion.DescriptionShort", "%" + this.Description + "%", BinaryOperatorType.Like);
                criteriaDescriptionShort = new BinaryOperator("Current.DescriptionShort", "%" + this.Description + "%", BinaryOperatorType.Like);
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

            this.DateRegistrationBegin = System.DateTime.MinValue;
            this.DateRegistrationEnd = System.DateTime.MinValue;
            this.DateFinishBegin = System.DateTime.MinValue;
            this.DateFinishEnd = System.DateTime.MinValue;
            //this.Curator = null;
            this.UserRegistrator = null;
            this.PersonOfCustomer = null;
            this.Customer = null;
            this.PersonOfSupplier = null;
            this.Supplier = null;
            this.Description = "";

            // Устарело 2011-12-11
            //LV.CollectionSource.Criteria.Clear();
        }

        #endregion


    }

}
