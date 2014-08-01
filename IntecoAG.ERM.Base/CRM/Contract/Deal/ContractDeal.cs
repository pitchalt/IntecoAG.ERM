#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;
//
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Exchange;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Статусы сделки
    /// </summary>
    public enum DealStates {
        /// <summary>
        /// Проект
        /// </summary>
        DEAL_PROJECT = 1,
        /// <summary>
        /// Оформление
        /// </summary>
        DEAL_FORMATION = 2,
        /// <summary>
        /// Урегулирование
        /// </summary>
        DEAL_RESOLVED = 3,
        /// <summary>
        /// Заключен
        /// </summary>
        DEAL_CONCLUDED = 4,
        /// <summary>
        /// Исполнен
        /// </summary>
        DEAL_CLOSED = 5, 
        /// <summary>
        /// Удален
        /// </summary>
        DEAL_DELETED = 10,
        /// <summary>
        /// Отклонен
        /// </summary>
        DEAL_DECLINE = 11,
    }

    public enum KindOfDeal {
        DEAL_WITH_STAGE = 1,
        DEAL_WITHOUT_STAGE = 2
//        DEAL_LONG_SERVICE = 3
    }

    public enum ContractKind {
        CONTRACT = 1,
        ADDENDUM = 2
    }
    /// <summary>
    /// Класс crmContractDeal, представляющий объект Договора
    /// </summary>
    // Не позволяет редактировать - [RuleCombinationOfPropertiesIsUnique("Unique_Index_On_ContractDeal", DefaultContexts.Save, "Customer; Supplier; DateRegistration; ContractDocument.Number")]
    [LikeSearchPathList(new string[] { 
        "UserRegistrator.LastName",
        "Contract.ContractDocument.Number",
        "ContractDocument.Number", 
        "Current.Customer.Party.Name", 
        "Current.Customer.Party.INN", 
        "Current.Supplier.Party.Name",
        "Current.Supplier.Party.INN"
    })]
    [MiniNavigation("Project", "Редакция проектная", TargetWindow.Default, 1)]
    [MiniNavigation("Current", "Редакция текущая", TargetWindow.Default, 2)]
    [MiniNavigation("Contract", "Договор", TargetWindow.Default, 3)]
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [VisibleInReports]
    [Persistent("crmDeal")]
    public partial class crmContractDeal : csCComponent, TrwIContract, IStateMachineProvider
        //, ICategorizedItem
    {
        public crmContractDeal(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.State = DealStates.DEAL_PROJECT;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //protected crmContract _Contract;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        //[Aggregated]
        //public crmContract crmContract {
        //    get { return _Contract; }
        //    set { SetPropertyValue<crmContract>("Contract", ref _Contract, value); }
        //}
        // Тип договорного документа: Договор/Дополнительное соглашение

        //[Action(Caption = "Validate")]
        //public void Validate() {
        //    IList<crmContractDeal> deals = new List<crmContractDeal>( new XPCollection<crmContractDeal>(this.Session, new BinaryOperator("ContractDocument.Number", "Р/935612111785-413902")));
        //    foreach (crmContractDeal deal in deals.Where(x => x.Contract == null)) {
        //        System.Diagnostics.Debug.Print("Deal: " + deal.ContractDocument.Number);
        //        Session.Delete(deal);
        //    }
        //    if (deals.Count != 0) {
        //        this.Session.CommitTransaction();
        //    }
        //}

        [Browsable(false)]
        public crmDealVersion Project {
            get { 
                foreach (crmDealVersion dv in this.DealVersions) {
                    if (dv.VersionState == VersionStates.VERSION_NEW ||
                        dv.VersionState == VersionStates.VERSION_PROJECT)
                        return dv;
                }
                return this.Current;
            }
        }
        protected ContractKind _ContractKind;
        public ContractKind ContractKind {
            get { return _ContractKind; }
            set { SetPropertyValue<ContractKind>("ContractKind", ref _ContractKind, value); }
        }

        public String Name {
            get { 
                String ret;
                crmContractDocument cont_doc;
                if (this.Contract != null)
                    cont_doc = this.Contract.ContractDocument;
                else 
                    cont_doc = null;
                if (cont_doc != null) {
                    if (this.ContractDocument != null && cont_doc != this.ContractDocument)
                        ret = cont_doc.FullName + " " + this.ContractDocument.FullName;
                    else 
                        ret = cont_doc.FullName;
                } else {
                    if (this.ContractDocument != null )
                        ret = this.ContractDocument.FullName;
                    else
                        ret = "";
                }
                //if (this.ContractDocument != null) {
                //    if (this.Contract.ContractDocument == this.ContractDocument) {
                //        ret = this.ContractDocument.FullName;
                //    }
                //    else {
                //        ret = this.Contract.ContractDocument.FullName + this.ContractDocument.FullName;
                //    }
                //}
                //if (this.ContractDocument != null) { 
                //    ret = 
                //}
                //}
                return ret;
            }
        }
        // Папка
        private String _Delo;
        [Size(15)]
        public String Delo {
            get { return _Delo; }
            set { SetPropertyValue<String>("Delo", ref _Delo, value); }
        }

        [PersistentAlias("Current.Customer.Party")]
        public crmCParty Customer {
            get {
                if (Current != null)
                    if (Current.Customer != null)
                        return Current.Customer.Party;
                return null;
            }
            set { Current.Customer.Party = value;  }
        }

        [PersistentAlias("Current.Supplier.Party")]
        public crmCParty Supplier {
            get { 
                if (Current != null)
                    if (Current.Supplier != null)
                        return Current.Supplier.Party; 
                return null;
            }
            set { Current.Supplier.Party = value; }
        }

        /// <summary>
        /// Curator
        /// </summary>
        private hrmDepartment _CuratorDepartment;
        public hrmDepartment CuratorDepartment {
            get { return _CuratorDepartment; }
            set { SetPropertyValue<hrmDepartment>("CuratorDepartment", ref _CuratorDepartment, value); }
        }
        //
        private DateTime _DateRegistration;
        public DateTime DateRegistration {
            get { return _DateRegistration; }
            set { SetPropertyValue<DateTime>("DateRegistration", ref _DateRegistration, value); }
        }
        // Регистрирующий пользователь: Пользователь, осуществляющий регистрацию
        private hrmStaff _UserRegistrator;
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator;}
            set { SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value); }
        }

        // Регистрирующее подразделение: Подразделение, осуществляющее регистрацию договора. Определяется автоматически по регистрирующему пользователю
        protected hrmDepartment _DepartmentRegistrator;
        public hrmDepartment DepartmentRegistrator {
            get { return _DepartmentRegistrator; }
            set {
                SetPropertyValue<hrmDepartment>("DepartmentRegistrator", ref _DepartmentRegistrator, value);
            }
        }

        private DealStates _State;
        public DealStates State {
            get { return _State; }
            set { SetPropertyValue<DealStates>("State", ref _State, value); }
        }

        //
        protected crmContractCategory _Category;
        //[RuleRequiredField("crmDealRegistrationForm.Category.Required", "Next")]
        //[Appearance("crmDealRegistrationForm.Category.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
        public crmContractCategory Category {
            get { return _Category; }
            set { SetPropertyValue<crmContractCategory>("Category", ref _Category, value); }
        }

        private TrwContractType _TRVType;
        [RuleRequiredField(TargetCriteria = "State != 'DEAL_CLOSED' && State != 'DEAL_DELETED'")]
        public TrwContractType TRVType {
            get { return _TRVType; }
            set { SetPropertyValue<TrwContractType>("TRVType", ref _TRVType, value); }
        }

        private TrwContractMarket _TRVContractor;
        [RuleRequiredField(TargetCriteria = "State != 'DEAL_CLOSED' && State != 'DEAL_DELETED'")]
        public TrwContractMarket TRVContractor {
            get { return _TRVContractor; }
            set { SetPropertyValue<TrwContractMarket>("TRVContractor", ref _TRVContractor, value); }
        }

        private crmContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [DataSourceProperty("ContractDocuments")]
        public crmContractDocument ContractDocument {
            get { return _ContractDocument; }
            set { 
                SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value); 
            }
        }

        public XPCollection<crmContractDocument> ContractDocuments {
//        public IList<crmContractDocument> ContractDocuments {
            get {
                if (this.Contract != null)
                    return Contract.ContractDocuments;
                else
                    return null;
//                        BindingList<crmContractDocument>();
            }
        }

        private crmDealVersion _Current;
        //[Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        [ExplicitLoading(2)]
        public crmDealVersion Current {
            get { return _Current; }
            set {
                SetPropertyValue<crmDealVersion>("Current", ref _Current, value); }
        }

        [Association("fmSubjects-crmDeals")]
        public XPCollection<fmCSubject> Subjects {
            get {
                return GetCollection<fmCSubject>("Subjects");
            }
        }

        [Association("CrmContractDeal-CrmAct")]
        [Aggregated]
        public XPCollection<crmAct> Acts {
            get {
                return GetCollection<crmAct>("Acts");
            }
        }

        [RuleFromBoolProperty("ContractUnique", DefaultContexts.Save, "Contract with this <Customer, Supplier, DateRegistration, Number> already exists")]
        protected bool IsContractUnique {
            get {
                // Если объект страый
                if (!Session.IsNewObject(this))
                    return true;

                // Если объект новый
                CriteriaOperatorCollection criteriaAND = new CriteriaOperatorCollection();
                criteriaAND.Add(new BinaryOperator("ContractDocument.Number", this.ContractDocument.Number));
                criteriaAND.Add(new BinaryOperator("Customer", this.Customer, BinaryOperatorType.Equal));
                criteriaAND.Add(new BinaryOperator("Supplier", this.Supplier, BinaryOperatorType.Equal));
                criteriaAND.Add(new BinaryOperator("DateRegistration", this.DateRegistration.Date, BinaryOperatorType.GreaterOrEqual));
                criteriaAND.Add(new BinaryOperator("DateRegistration", this.DateRegistration.Date.AddDays(1), BinaryOperatorType.Less));

                crmContractDeal cd = Session.FindObject<crmContractDeal>(PersistentCriteriaEvaluationBehavior.BeforeTransaction, CriteriaOperator.And(criteriaAND));
                if (cd != null)
                    return false;

                /*
                // Работающий вариант через LINQ
                XPQuery<crmContractDeal> contractDeals = new XPQuery<crmContractDeal>(Session, false);
                var queryContractDeals = from contractDeal in contractDeals
                                         where contractDeal.Customer == this.Customer
                                            && contractDeal.Supplier == this.Supplier
                                            && contractDeal.DateRegistration.Date == this.DateRegistration.Date
                                            && contractDeal.ContractDocument.Number == this.ContractDocument.Number
                                         select contractDeal;
                if (queryContractDeals.Count() > 0) {
                    return false;
                }
                */
                return true;
            }
        }
        //
        [Persistent("TrwNumber")]
        [Size(57)]
        private String _TrwNumber;
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("_TrwNumber")]
        public String TrwNumber {
            get { return _TrwNumber; }
            //            set { SetPropertyValue<String>("TrwNumber", ref _TrwNumber, value); }
        }
        public void TrwNumberSet(String number) {
            String old = _TrwNumber;
            _TrwNumber = number;
            OnChanged("TrwNumber", old, number);
        }
        //
        [Persistent("TrwIntNumber")]
        [Indexed(Unique=true)]
        [Size(20)]
        private String _TrwInternalNumber;
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("_TrwInternalNumber")]
        public String TrwInternalNumber {
            get { return _TrwInternalNumber; }
            //            set { SetPropertyValue<String>("TrwNumber", ref _TrwNumber, value); }
        }
        public void TrwInternalNumberSet(String number) {
            String old = _TrwInternalNumber;
            _TrwInternalNumber = number;
            OnChanged("TrwIntNumber", old, number);
        }
        //
        private Int32 _IntNumber;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Int32 IntNumber {
            get { return _IntNumber; }
            set { SetPropertyValue<Int32>("IntNumber", ref _IntNumber, value); }
        }
        //
        private Int32 _FailNumber;
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Int32 FailNumber {
            get { return _FailNumber; }
            set { SetPropertyValue<Int32>("FailNumber", ref _FailNumber, value); }
        }

        [Aggregated]
        [Association("crmDeal-TrwOrders")]
        public XPCollection<TrwOrder> TrwOrders {
            get { return GetCollection<TrwOrder>("TrwOrders"); }
        }

        #endregion


        #region МЕТОДЫ
        public void UpdateTrwNumbers() {
            if (this.ContractKind == ContractKind.CONTRACT) {
                this.TrwNumberSet(this.Contract.ContractDocument.Number);
                this.TrwInternalNumberSet(this.Contract.TrwIntNumber);
            }
            else {
                if (this.ContractKind == ContractKind.ADDENDUM) {
                    if (this.ContractDocument.Number.Length > 10) {
                        this.TrwNumberSet(this.ContractDocument.Number);
                        this.TrwInternalNumberSet(this.Contract.TrwIntNumber + "/" + this.IntNumber);
                    }
                    else {
                        this.TrwNumberSet(this.Contract.ContractDocument.Number + "//" + this.ContractDocument.Number);
                        this.TrwInternalNumberSet(this.Contract.TrwIntNumber + "//" + this.ContractDocument.Number);
                    }
                }
                else
                    throw new InvalidDataException("Invalid deal type");
            }
        }

        #endregion

        /*
        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (crmContractCategory) value;
            }
        }
        */


        #region РАБОТА С РЕГИСТРАМИ

        protected virtual void RegisterClear(crmDealVersion scVersion) {
            if (scVersion == null) return;

            // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
            Guid token = ((crmContractDeal)scVersion.MainObject).Oid;

            // Чистим регистры 
            //FindAndDeletePFRegisterRecords(scVersion);
            //FindAndDeleteDCDRegisterRecords(scVersion);  // Очистка сразу двух родственных регистров
            FindAndDeletePFRegisterRecords(token);
            FindAndDeleteDCDRegisterRecords(token);
            FindAndDeleteCFRegisterRecords(token);
        }

        protected virtual void FindAndDeletePFRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmPlaneFactRegister> RegColl = new XPCollection<crmPlaneFactRegister>(this.Session, criteria, null);
            if (!RegColl.IsLoaded) RegColl.Load();
            RegColl.DeleteObjectOnRemove = true;

            // Удаление старого
            while (RegColl.Count > 0) RegColl.Remove(RegColl[0]);
        }

        protected virtual void FindAndDeleteDCDRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmDebtorCreditorDebtRegister> dcdRegColl = new XPCollection<crmDebtorCreditorDebtRegister>(this.Session, criteria, null);
            if (!dcdRegColl.IsLoaded) dcdRegColl.Load();
            dcdRegColl.DeleteObjectOnRemove = true;

            // Удаление старого
            while (dcdRegColl.Count > 0) dcdRegColl.Remove(dcdRegColl[0]);
        }

        protected virtual void FindAndDeleteCFRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmCashFlowRegister> cfRegColl = new XPCollection<crmCashFlowRegister>(this.Session, criteria, null);
            if (!cfRegColl.IsLoaded) cfRegColl.Load();
            cfRegColl.DeleteObjectOnRemove = true;

            // Удаление старого
            while (cfRegColl.Count > 0) cfRegColl.Remove(cfRegColl[0]);
        }

        #endregion


        public IList<IStateMachine> GetStateMachines() {
            IList<IStateMachine> sml = new List<IStateMachine>(1);
            sml.Add(new crmContractDealSM());
            return sml;
        }

        #region Trw

/*
        String TrwNumber {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
*/
        [PersistentAlias("ContractDocument.Date")]
        public DateTime TrwDate {
            get {
                return ContractDocument != null ? ContractDocument.Date : default(DateTime);
            }
 //           set {
 //               throw new NotImplementedException();
 //           }
        }
        [PersistentAlias("Current.Customer")]
        public TrwIContractParty TrwCustomerParty {
            get { return Current != null? Current.Customer : null; }
        }

        [PersistentAlias("Current.Supplier")]
        public TrwIContractParty TrwSupplierParty {
            get { return Current != null ? Current.Supplier : null; }
        }

        [PersistentAlias("TRVContractor")]
        public TrwContractMarket TrwContractMarket {
            get {
                return TRVContractor;
            }
        }

        public IList<TrwIOrder> TrwSaleOrders {
            get { 
                return new ListConverter<TrwIOrder, TrwOrder>(TrwOrders); 
            }
        }

        [PersistentAlias("Current.DescriptionShort")]
        public String TrwSubject {
            get {
                return Current != null ? Current.DescriptionShort : null;
            }
 //           set {
 //               throw new NotImplementedException();
 //           }
        }

        [PersistentAlias("ContractDocument.Date")]
        public DateTime TrwDateSigning {
            get {
                return ContractDocument != null ? ContractDocument.Date : default(DateTime);
            }
//            set {
//                throw new NotImplementedException();
//            }
        }

        [PersistentAlias("Current.DateBegin")]
        public DateTime TrwDateValidFrom {
            get {
                return Current != null ? Current.DateBegin : default(DateTime);
//                throw new NotImplementedException();
            }
//            set {
//                throw new NotImplementedException();
//            }
        }

        [PersistentAlias("Current.DateEnd")]
        public DateTime TrwDateValidToPlan {
            get {
                return Current != null ? Current.DateEnd : default(DateTime);
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }

        public DateTime TrwDateValidToFact {
            get {
                return default(DateTime);
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }

        [PersistentAlias("Current.Valuta")]
        public csValuta TrwObligationCurrency {
            get {
                return Current != null ? Current.Valuta : null;
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }

        [PersistentAlias("Current.Price")]
        public Decimal TrwObligationSumma {
            get {
                return Current != null ? Current.Price : default(Decimal);
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }

        [PersistentAlias("Current.PaymentValuta")]
        public csValuta TrwPaymentCurrency {
            get {
                return Current != null ? Current.PaymentValuta : null;
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }

        [PersistentAlias("Current.NDSRate")]
        public csNDSRate TrwVATRate {
            get {
                return Current != null ? Current.NDSRate : null;
            }
            //set {
            //    throw new NotImplementedException();
            //}
        }


        #endregion

        //public IBindingList Children {
        //    get { return new BindingList<TrwIOrder>( TrwSaleOrders); }
        //}

        //public ITreeNode Parent {
        //    get { return null; }
        //}
    }

}