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
    public class crmPlaneFactRegisterFilter01 : ReportParametersObjectBase, ICustomFilter
    {
        public crmPlaneFactRegisterFilter01(Session session) : base(session) { }

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

        #region ��� ������� � ������� ListView

        private ListView _LV;
        [Browsable(false)]
        public ListView LV {
            get { return _LV; }
            set { SetPropertyValue<ListView>("LV", ref _LV, value); }
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

        #region ��������� �� �����

        protected DateTime _DateBegin;
        [Custom("Caption", "���� �")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value); }
        }

        protected DateTime _DateEnd;
        [Custom("Caption", "���� ��")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        }

        //private DealStates _DealState;
        //[Custom("Caption", "������")]
        //public DealStates DealState {
        //    get { return _DealState; }
        //    set {
        //        SetPropertyValue<DealStates>("DealState", ref _DealState, value);
        //    }
        //}

        // BEGIN

        private crmContract _Contract;
        [Custom("Caption", "��������")]
        public crmContract Contract {
            get { return _Contract; }
            set { SetPropertyValue<crmContract>("Contract", ref _Contract, value); }
        }

        private crmContractDeal _ContractDeal;
        [Custom("Caption", "������� �������")]
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }
        // END

        //BEGIN
        private fmCostItem _CostItem;
        [Custom("Caption", "�����������")]
        public fmCostItem CostItem {
            get { return _CostItem; }
            set {
                SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value);
            }
        }




        private crmPartyRu _Creditor;
        [Custom("Caption", "��������� �����������")]
        public crmPartyRu Creditor {
            get { return _Creditor; }
            set {
                SetPropertyValue<crmPartyRu>("Creditor", ref _Creditor, value);
            }
        }

        private crmPartyRu _Debitor;
        [Custom("Caption", "����������")]
        public crmPartyRu Debitor {
            get { return _Debitor; }
            set {
                SetPropertyValue<crmPartyRu>("Debitor", ref _Debitor, value);
            }
        }

        private csNomenclature _Nomenclature;
        [Custom("Caption", "������������")]
        public csNomenclature Nomenclature {
            get { return _Nomenclature; }
            set { SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value); }
        }

        private csUnit _MeasureUnit;
        [Custom("Caption", "������� ���������")]
        public csUnit MeasureUnit {
            get { return _MeasureUnit; }
            set { SetPropertyValue<csUnit>("MeasureUnit", ref _MeasureUnit, value); }
        }
        // END

        //BEGIN
        private SubjectClass _Subject;
        [Custom("Caption", "����")]
        public SubjectClass Subject {
            get { return _Subject; }
            set { SetPropertyValue<SubjectClass>("Subject", ref _Subject, value); }
        }
        // END

        protected crmObligationUnit _ObligationUnit;
        [Custom("Caption", "�������������")]
        public crmObligationUnit ObligationUnit {
            get { return _ObligationUnit; }
            set {
                SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value);
            }
        }

        protected fmOrder _fmOrder;
        [Custom("Caption", "�����")]
        //[Custom("Caption", (false) ? "���������. ����." : "WWWWWWWWWWWWWWWW")]
        public fmOrder fmOrder {
            get { return _fmOrder; }
            set {
                SetPropertyValue<fmOrder>("fmOrder", ref _fmOrder, value);
            }
        }

        // ���� (����������)
        protected crmStage _Stage;
        [Custom("Caption", "���������� ����")]
        public crmStage Stage {
            get { return _Stage; }
            set {
                SetPropertyValue<crmStage>("Stage", ref _Stage, value);
            }
        }

        // ���� (�����������)
        protected crmStage _StageTech;
        [Custom("Caption", "����������� ����")]
        public crmStage StageTech {
            get { return _StageTech; }
            set {
                SetPropertyValue<crmStage>("StageTech", ref _StageTech, value);
            }
        }

        private csValuta _Valuta;
        [Custom("Caption", "������")]
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        private decimal _CostFrom;
        [Custom("Caption", "��������� � ������ �������� ��")]
        public decimal CostFrom {
            get { return _CostFrom; }
            set {
                SetPropertyValue<decimal>("CostFrom", ref _CostFrom, value);
            }
        }

        private decimal _CostTo;
        [Custom("Caption", "��������� � ������ �������� ��")]
        public decimal CostTo {
            get { return _CostTo; }
            set {
                SetPropertyValue<decimal>("CostTo", ref _CostTo, value);
            }
        }

        private decimal _CostInRURFrom;
        [Custom("Caption", "��������� �������� ���. ��")]
        public decimal CostInRURFrom {
            get { return _CostInRURFrom; }
            set {
                SetPropertyValue<decimal>("CostInRURFrom", ref _CostInRURFrom, value);
            }
        }

        private decimal _CostInRURTo;
        [Custom("Caption", "��������� �������� ���. ��")]
        public decimal CostInRURTo {
            get { return _CostInRURTo; }
            set {
                SetPropertyValue<decimal>("CostInRURTo", ref _CostInRURTo, value);
            }
        }

        private decimal _VolumeFrom;
        [Custom("Caption", "����� � ����������� ��������� ��")]
        public decimal VolumeFrom {
            get { return _VolumeFrom; }
            set {
                SetPropertyValue<decimal>("VolumeFrom", ref _VolumeFrom, value);
            }
        }

        private decimal _VolumeTo;
        [Custom("Caption", "����� � ����������� ��������� ��")]
        public decimal VolumeTo {
            get { return _VolumeTo; }
            set {
                SetPropertyValue<decimal>("VolumeTo", ref _VolumeTo, value);
            }
        }

        // ������ ����� PaymentCost (decimal), PaymentItem (crmPaymentItem), PaymentValuta (csValuta)


        private PlaneFact _PlaneFact;
        [Custom("Caption", "����/����")]
        public PlaneFact PlaneFact {
            get { return _PlaneFact; }
            set {
                SetPropertyValue<PlaneFact>("PlaneFact", ref _PlaneFact, value);
            }
        }
        

        // ����� �� LIKE, �������������� ����� OR
        private string _DocumentNumber;
        [Custom("Caption", "����� ���������")]
        public string DocumentNumber {
            get { return _DocumentNumber; }
            set {
                SetPropertyValue<string>("DocumentNumber", ref _DocumentNumber, value);
            }
        }

        // ����� �� LIKE, �������������� ����� OR
        private string _Description;
        [Custom("Caption", "��������")]
        public string Description {
            get { return _Description; }
            set {
                SetPropertyValue<string>("Description", ref _Description, value);
            }
        }



        /*
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
        */

        // ����������� ��
        private bool sortByName;
        [Browsable(false)]   // �������� ������ �� �������������
        [Custom("Caption", "����������� �� �����������")]
        public bool SortByName {
            get { return sortByName; }
            set { sortByName = value; }
        }

        #endregion


        #region ������

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


            //CriteriaOperator criteriaContract = null;
            //if (this.Contract != null) {
            //    criteriaContract = new BinaryOperator("Contract.Oid", this.Contract.Oid, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteria).Operands.Add(criteriaContract);
            //}

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


            // ������������� �����

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



            CriteriaOperator criteriaContract = null;
            if (this.Contract != null) {
                criteriaContract = new BinaryOperator("Contract.Oid", Contract.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaContract);
            }

            CriteriaOperator criteriaContractDeal = null;
            if (this.ContractDeal != null) {
                criteriaContractDeal = new BinaryOperator("ContractDeal.Oid", ContractDeal.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaContractDeal);
            }

            CriteriaOperator criteriaCostItem = null;
            if (this.CostItem != null) {
                criteriaCostItem = new BinaryOperator("CostItem.Oid", CostItem.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCostItem);
            }

            CriteriaOperator criteriaSubject = null;
            if (this.Subject != null) {
                criteriaSubject = new BinaryOperator("Subject.Oid", Subject.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaSubject);
            }

            CriteriaOperator criteriafmOrder = null;
            if (this.fmOrder != null) {
                criteriafmOrder = new BinaryOperator("fmOrder.Oid", fmOrder.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriafmOrder);
            }

            CriteriaOperator criteriaStage = null;
            if (this.Stage != null) {
                criteriaStage = new BinaryOperator("Stage.Oid", Stage.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaStage);
            }

            //CriteriaOperator criteriaStageTech = null;
            //if (this.StageTech != null) {
            //    criteriaStageTech = new BinaryOperator("StageTech.Oid", StageTech.Oid, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteriaAND).Operands.Add(criteriaStageTech);
            //}

            CriteriaOperator criteriaNomenclature = null;
            if (this.Nomenclature != null) {
                criteriaNomenclature = new BinaryOperator("Nomenclature.Oid", this.Nomenclature.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaNomenclature);
            }

            CriteriaOperator criteriaMeasureUnit = null;
            if (this.MeasureUnit != null) {
                criteriaMeasureUnit = new BinaryOperator("MeasureUnit", MeasureUnit, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaMeasureUnit);
            }

            // BEGIN
            CriteriaOperator criteriaCustomerAND = new GroupOperator();
            ((GroupOperator)criteriaCustomerAND).OperatorType = GroupOperatorType.And;

            CriteriaOperator criteriaCreditor = null;
            if (this.Creditor != null) {
                criteriaCreditor = new BinaryOperator("Creditor.Oid", Creditor.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaCreditor);
            }

            CriteriaOperator criteriaDebitor = null;
            if (this.Debitor != null) {
                criteriaDebitor = new BinaryOperator("Debitor.Oid", Debitor.Oid, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaCustomerAND).Operands.Add(criteriaDebitor);
            }

            if (((GroupOperator)criteriaCustomerAND).Operands.Count > 0) ((GroupOperator)criteriaAND).Operands.Add(criteriaCustomerAND);
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

            if (this.CostFrom != 0) {
                CriteriaOperator criteriaCostFrom = new BinaryOperator("Cost", this.CostFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCostFrom);
            }

            if (this.CostTo != 0) {
                CriteriaOperator criteriaCostTo = new BinaryOperator("Cost", this.CostTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCostTo);
            }

            if (this.CostInRURFrom != 0) {
                CriteriaOperator criteriaCostInRURFrom = new BinaryOperator("CostInRUR", this.CostInRURFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCostInRURFrom);
            }

            if (this.CostInRURTo != 0) {
                CriteriaOperator criteriaCostInRURTo = new BinaryOperator("CostInRUR", this.CostInRURTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaCostInRURTo);
            }

            if (this.VolumeFrom != 0) {
                CriteriaOperator criteriaVolumeFrom = new BinaryOperator("Volume", this.VolumeFrom, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaVolumeFrom);
            }

            if (this.VolumeTo != 0) {
                CriteriaOperator criteriaVolumeTo = new BinaryOperator("Volume", this.VolumeTo, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaVolumeTo);
            }

            if (this.PlaneFact != 0) {
                CriteriaOperator criteriaPlaneFact = new BinaryOperator("PlaneFact ", this.PlaneFact, BinaryOperatorType.Equal);
                ((GroupOperator)criteriaAND).Operands.Add(criteriaPlaneFact);
            }

            // ������������ ��������� OR ��������� ����� AND � ����� ������������ ��������� ������
            if (((GroupOperator)criteriaAND).Operands.Count > 0) {
                ((GroupOperator)criteria).Operands.Add(criteriaAND);
            }


            // ������������� �����
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

            // ������������ ��������� OR ��������� ����� AND � ����� ������������ ��������� ������
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

            // ������� ����� ���� ������������, �.�. ��� �� ����� ����� �� ������������
            this.DateBegin = System.DateTime.MinValue;
            this.DateEnd = System.DateTime.MinValue;
            //this.Department = null;

            LV.CollectionSource.Criteria.Clear();
        }

        #endregion


    }

}
