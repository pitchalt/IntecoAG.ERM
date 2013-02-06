using System;
using System.ComponentModel;
using System.Drawing;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Contract.Forms {

    public enum RegistrationKind {
        NEW = 1,
        PROJECT = 2
    }

    // Регистрация проекта договора

    //[NavigationItem(true, GroupName = "crmContract")]

    [Appearance("crmContractRegistrationForm.GroupProperty.Caption.Bold", AppearanceItemType = "LayoutItem", TargetItems = "Category; Date; DateBegin; DateEnd; DateFinish; Department; DescriptionShort; Price; Valuta; OurParty; OurRole; PartnerParty", Criteria = "not ApplyTopAppearance", FontStyle = FontStyle.Bold)]
    [Appearance("crmContractRegistrationForm.GroupProperty.Enable", AppearanceItemType = "ViewItem", Criteria = "ApplyTopAppearance", TargetItems = "Category; Date; DateBegin; DateEnd; DateFinish; Department; DescriptionShort; Price; Valuta; OurParty; OurRole; PartnerParty", Enabled = false)] //, BackColor = "15527155")]
    [Appearance("crmContractRegistrationForm.GroupProperty.Enable.Color", AppearanceItemType = "ViewItem", Criteria = "not ApplyTopAppearance", TargetItems = "Category; Date; DateBegin; DateEnd; DateFinish; Department; DescriptionShort; Price; Valuta; OurParty; OurRole; PartnerParty", Enabled = true)] //, BackColor = "16116735")]

    [Appearance("crmContractRegistrationForm.ContractDeal.Enable", AppearanceItemType = "ViewItem", Criteria = "RegistrationKind = 1", TargetItems = "ContractDeal", Enabled = false)]

    [Appearance("crmContractRegistrationForm.CuratorDepartmentAndKindOfDeal.Caption", AppearanceItemType.LayoutItem, "", TargetItems = "CuratorDepartment; KindOfDeal", FontStyle = FontStyle.Bold)]

    [NonPersistent]
    public class crmContractRegistrationForm : crmDealRegistrationForm, IWizardSupport {

        public crmContractRegistrationForm(Session session): base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.RegistrationKind = RegistrationKind.NEW;
            this.ContractKind = ContractKind.CONTRACT;
            //this.Document = new crmContractDocument(this.Session);
            this.ContractDocument = new crmContractDocument(this.Session);
            this.Date = DateTime.Now;
            this.IsRegRequired = true;
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            user = this.Session.GetObjectByKey<csCSecurityUser>(user.Oid);
            if (user != null) {
                this.UserRegistrator = user.Staff;
            }

        }

        // Тип регистрации. По-умолчани: REGISTRATION_NEW
        protected RegistrationKind _RegistrationKind;
        [RuleRequiredField("crmContractRegistrationForm.RegistrationKind.Required", "Next")]
        [Appearance("crmContractRegistrationForm.RegistrationKind.Caption", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [ImmediatePostData]
        public RegistrationKind RegistrationKind {
            get { return _RegistrationKind; }
            set { SetPropertyValue<RegistrationKind>("RegistrationKind", ref _RegistrationKind, value); }
        }

        public Boolean IsRegRequired;

        // Простой договор (ведомость): Ссылка на существующий простой договор (ведомость). Обязателен к заполнению, если тип договорного документа дополнительное соглашение.
        protected crmContractDeal _ContractDeal;
        //[RuleRequiredField("crmContractRegistrationForm.ContractDeal.Required", "Next")]
        [Appearance("crmContractRegistrationForm.ContractDeal.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        //[Appearance("crmContractRegistrationForm.ContractDeal.Caption", AppearanceItemType = "LayoutItem", TargetItems = "Category, DateBegin, DateEnd, DateFinish, DescriptionShort, OurRole, OurParty, PartnerParty, Price, Valuta", Criteria = "IsNull(ContractDeal)", FontStyle = FontStyle.Bold)]
        //[Appearance("crmContractRegistrationForm.ContractDeal.Enable", AppearanceItemType = "PropertyEditor", TargetItems = "Category, DateBegin, DateEnd, DateFinish, DescriptionShort, OurRole, OurParty, PartnerParty, Price, Valuta", Criteria = "not IsNull(ContractDeal)", Enabled = false)]
        [ImmediatePostData]
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set {
                SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value);
                if (!IsLoading)
                    if (this.ContractDeal != null) {
                        //this.ContractCategory = this.ContractDeal.Category;
                        this.Category = this.ContractDeal.Category;

                        //this.ContractDocument.Date = this.ContractDeal.ContractDocument.Date;
                        //this.ContractDocument.Number = this.ContractDeal.ContractDocument.Number;
                        //this.ContractDocument.DocumentCategory = this.ContractDeal.ContractDocument.DocumentCategory;

                        this.Date = this.ContractDeal.ContractDocument.Date;
                        this.Number = this.ContractDeal.ContractDocument.Number;
                        this.DocumentCategory = this.ContractDeal.ContractDocument.DocumentCategory;


                        // Заполнение DealRegistrationForm
                        // SHU!!! ??? this.Category = this.ContractDeal.Current.Category;
                        //this.Document.Date = this.ContractDeal.Current.ContractDocument.Date;
                        //this.Document.Number = this.ContractDeal.Current.ContractDocument.Number;
                        //this.Document.DocumentCategory = this.ContractDeal.Current.ContractDocument.DocumentCategory;
                        this.DateBegin = this.ContractDeal.Current.DateBegin;
                        this.DateEnd = this.ContractDeal.Current.DateEnd;
                        this.DateFinish = this.ContractDeal.Current.DateFinish;
                        //this.DateRegistration = this.ContractDeal.DateRegistration;
                        //this.Department = this.ContractDeal.Current.;
                        this.DescriptionShort = this.ContractDeal.Current.DescriptionShort;
                        this.Delo = this.Delo;
                        //this.Document = this.ContractDeal.;
                        UpdateRole(this.ContractDeal);
                        // Номер и дата служебной записки
                        // SHU!!! ??? this.Number = this.ContractDeal.Current.ContractDocument.Number;
                        // SHU!!! ???  this.Date = this.ContractDeal.Current.ContractDocument.Date;

                        //this.OurRole = ;
                        //this.PartnerRole = ;
                        this.Price = this.ContractDeal.Current.Price;
                        this.Valuta = this.ContractDeal.Current.Valuta;

                        this.NewNumberRequired = false;
                    } else {
                        this.Date = System.DateTime.MinValue;
                        this.Number = "";
                        this.DocumentCategory = null;
                    }
            }
        }
        protected void UpdateRole(crmContractDeal deal) {
            if (crmUserParty.CurrentUserPartyGet(this.Session) != null) {
                if (crmUserParty.CurrentUserPartyGet(this.Session).Party == deal.Current.Customer.Party) {
                    this.OurRole = IntecoAG.ERM.CS.PartyRole.CUSTOMER;
                    this.OurParty = deal.Current.Customer.Party;
                    this.PartnerParty = deal.Current.Supplier.Party;
                }
                else {
                    this.OurRole = IntecoAG.ERM.CS.PartyRole.SUPPLIER;
                    this.OurParty = deal.Current.Supplier.Party;
                    this.PartnerParty = deal.Current.Customer.Party;
                }
            }
        }
        // Папка
        private String _Delo;
        [Size(15)]
        public String Delo {
            get { return _Delo; }
            set { SetPropertyValue<String>("Delo", ref _Delo, value); }
        }

        // Тип договорного документа: Договор/Дополнительное соглашение
        protected ContractKind _ContractKind;
        [RuleRequiredField("crmContractRegistrationForm.ContractKind.Required", "Next")]
        //[Appearance("crmContractRegistrationForm.ContractKind.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [ImmediatePostData]
        public ContractKind ContractKind {
            get { return _ContractKind; }
            set { SetPropertyValue<ContractKind>("ContractKind", ref _ContractKind, value); }
        }

        // Выбор догоовра в случае ContractDocumentKind = ADDENDUM
        protected crmContractDeal _Contract;
        //[RuleRequiredField("crmContractRegistrationForm.ContractDoc.Required", "Next")]
        [Appearance("crmContractRegistrationForm.Contract.Caption.Bold", AppearanceItemType = "LayoutItem", Criteria = "ContractKind = 1", FontStyle = FontStyle.Bold)]
        [Appearance("crmContractRegistrationForm.Contract.Enable", Criteria = "ContractKind = 1", Enabled = false)]
        public crmContractDeal Contract {
            get { return _Contract; }
            //set { SetPropertyValue<crmContractDeal>("Contract", ref _Contract, value); }
            set {
                _Contract = value;
                if (!IsLoading) {
                    UpdateRole(this._Contract);
                }
                OnChanged("Contract");
            }
        }

        // То же самое. что и this.Category
        //// Категория договора. Заполняется автоматически, если выбран простой договор (ведомость).
        //private crmContractCategory _ContractCategory;
        //[RuleRequiredField("crmContractRegistrationForm.ContractCategory.Required", "Next")]
        //[Appearance("crmContractRegistrationForm.ContractCategory.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        //public crmContractCategory ContractCategory {
        //    get { return _ContractCategory; }
        //    set { SetPropertyValue<crmContractCategory>("ContractCategory", ref _ContractCategory, value); }
        //}

        // Требуется новый номер. Признак, что система должна сформировать номер договорного документа из регистратора	Если выбран простой договор (ведомость), автоматически значение не требуется.
        private bool _NewNumberRequired;
        //[RuleRequiredField("crmContractRegistrationForm.NewNumberRequired.Required", "Next")]
        //[Appearance("crmContractRegistrationForm.NewNumberRequired.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [ImmediatePostData]
        public bool NewNumberRequired {
            get { return _NewNumberRequired; }
            set { SetPropertyValue<bool>("NewNumberRequired", ref _NewNumberRequired, value); }
        }

        //// Если не требуется автоматическое формирование регистрационного номера, он вводится вручную
        //private string _RegistrationNumber;
        //public string RegistrationNumber {
        //    get { return _RegistrationNumber; }
        //    set { SetPropertyValue<string>("RegistrationNumber", ref _RegistrationNumber, value); }
        //}
        // Регистрирующий пользователь: Пользователь, осуществляющий регистрацию
        private hrmStaff _UserRegistrator;
        [RuleRequiredField("crmContractRegistrationForm.UserRegistrator.Required", "Next")]
        [Appearance("crmContractRegistrationForm.UserRegistrator.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [DataSourceProperty("ManagerContractDepartment")]
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator; }
            set {
                SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value);
                if (!IsLoading)
                    if (this.UserRegistrator != null) {
                        this.DepartmentRegistrator = this.UserRegistrator.Department;
                    }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerContractDepartment {
            get { return crmCSettingsContract.GetInstance(this.Session).ManagerGroupOfContractStaffs; }
        }

        // Регистрирующее подразделение: Подразделение, осуществляющее регистрацию договора. Определяется автоматически по регистрирующему пользователю
        protected hrmDepartment _DepartmentRegistrator;
        [RuleRequiredField("crmContractRegistrationForm.DepartmentRegistrator.Required", "Next")]
        [Appearance("crmContractRegistrationForm.DepartmentRegistrator.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        public hrmDepartment DepartmentRegistrator {
            //get { return if (UserRegistrator != null) UserRegistrator.; }
            get { return _DepartmentRegistrator; }
            set {
                SetPropertyValue<hrmDepartment>("DepartmentRegistrator", ref _DepartmentRegistrator, value);
            }
        }

        ////
        //private crmContractDocument _ContractDocument;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        //[Appearance("crmContractRegistrationForm.ContractDocument.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        //public crmContractDocument ContractDocument {
        //    get { return _ContractDocument; }
        //    set { SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value); }
        //}

        ////
        //private crmDealRegistrationForm _DealRegistrationForm;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        //[Appearance("crmContractRegistrationForm.DealRegistrationForm.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        //public crmDealRegistrationForm DealRegistrationForm {
        //    get { return _DealRegistrationForm; }
        //    set { SetPropertyValue<crmDealRegistrationForm>("DealRegistrationForm", ref _DealRegistrationForm, value); }
        //}

        #region VALIDATION & APPEARANCE

        //[Appearance("crmContractRegistrationForm.ContractDeal.GroupPropertiesEnable", TargetItems = "DescriptionShort", Enabled = false)]
        //[ImmediatePostData]
        [Browsable(false)]
        public override bool ApplyTopAppearance {
            get { return (this.ContractDeal != null); }
        }

        // Проверяем, что если RegistrationKind = REGISTRATION_PROJECT
        //[RuleFromBoolProperty("crmContractRegistrationForm.ContractDeal.ConditionalRequired", DefaultContexts.Save, "Необходимо указать проект договора!")]
        [RuleFromBoolProperty("crmContractRegistrationForm.ContractDeal.ConditionalRequired", "Next", "Необходимо указать проект договора!")]
        [NonPersistent]
        //[ImmediatePostData]
        [Browsable(false)]
        private bool ContractDeal_ConditionalRequired {
            get { return (!(this.RegistrationKind == RegistrationKind.PROJECT) | this.ContractDeal == null); }
        }

        /// <summary>
        /// ContractDealDisable - вычисление, когда поле недоступно для редактирования
        /// </summary>
        [NonPersistent]
        //[ImmediatePostData]
        [Browsable(false)]
        public bool ContractDealDisable {
            get { return (this.RegistrationKind == RegistrationKind.NEW); }
        }

        // Проверяем, что если RegistrationKind = REGISTRATION_PROJECT
        //[RuleFromBoolProperty("crmContractRegistrationForm.ContractDoc.ConditionalRequired", DefaultContexts.Save, "Необходимо указать договор!")]
        [RuleFromBoolProperty("crmContractRegistrationForm.ContractDoc.ConditionalRequired", "Next", "Необходимо указать договор!")]
        //[NonPersistent]
        //[ImmediatePostData]
        [Browsable(false)]
        private bool ContractDoc_ConditionalRequired {
            //get { return ((this.ContractDocumentKind != Contract.ContractKind.ADDENDUM & this.ContractDocumentKind != 0) | this.ContractDoc == null); }
            get { return !(this.ContractKind == ContractKind.ADDENDUM && this.Contract == null); }
        }

        /// <summary>
        /// ContractDisable - вычисление, когда поле недоступно для редактирования
        /// </summary>
        //[NonPersistent]
        //[ImmediatePostData]
        [Browsable(false)]
        public bool ContractDisable {
            get { return (this.ContractKind == ContractKind.CONTRACT | this.ContractKind == 0); }
        }

        // Проверяем, что если RegistrationNumber != null
        //[RuleFromBoolProperty("crmContractRegistrationForm.RegistrationNumber.ConditionalRequired", "Next", "Необходимо указать регистрационный номер!")]
        //[Browsable(false)]
        [ImmediatePostData]
        private bool NumberRequired {
            get { return this.ContractKind == ContractKind.CONTRACT | !this.NewNumberRequired ; }
        }

        // Номер документа
        [RuleRequiredField("crmContractRegistrationForm.Number.Required", "Next", TargetCriteria = "not NewNumberRequired")]
        [Appearance("crmContractRegistrationForm.Number.Caption.Bold", AppearanceItemType = "LayoutItem", Criteria = "not NewNumberRequired AND not ApplyTopAppearance", FontStyle = FontStyle.Bold)]
        [Appearance("crmContractRegistrationForm.Number.Caption.Regular", AppearanceItemType = "LayoutItem", Criteria = "NewNumberRequired OR ApplyTopAppearance", FontStyle = FontStyle.Regular)]
        [Appearance("crmContractRegistrationForm.Number.Desable", Criteria = "NewNumberRequired OR ApplyTopAppearance", Enabled = false)]
        [Appearance("crmContractRegistrationForm.Number.Enable", Criteria = "not NewNumberRequired AND not ApplyTopAppearance", Enabled = true)]  //, BackColor = "16772085")]
        [Size(30)]
        //[ImmediatePostData]
        public string Number {
            get { return this.ContractDocument.Number; }
            set {
                this.ContractDocument.Number = value;
                OnChanged("Number");
            }
        }

        // Дата документа
        //private DateTime _Date;
        [RuleRequiredField("crmContractRegistrationForm.Date.Required", "Next")]
        //[Appearance("crmContractRegistrationForm.Date.Caption.Bold", AppearanceItemType = "LayoutItem", Criteria = "IsNull(ContractDeal)", FontStyle = FontStyle.Bold)]
        //[Appearance("crmContractRegistrationForm.Date.Enable", Criteria = "not IsNull(ContractDeal)", Enabled = false)]
        //[Appearance("crmContractRegistrationForm.Date.Enable.Color", Criteria = "not ApplyTopAppearance", Enabled = true, BackColor = "16772085")]
        ////[RuleRequiredField("crmContractDocument.Date.Required.Immediate", "Immediate")]
        [ImmediatePostData]
        public DateTime Date {
            get { return this.ContractDocument.Date; }
            set {
                this.ContractDocument.Date = value;
                OnChanged("Date");
            }
        }

        //private crmContractDocumentType _DocumentCategory;
        [RuleRequiredField("crmContractRegistrationForm.DocumentCategory.Required", "Next")]
        //[Appearance("crmContractRegistrationForm.DocumentCategory.Caption.Bold", AppearanceItemType = "LayoutItem", Criteria = "IsNull(ContractDeal)", FontStyle = FontStyle.Bold)]
        //[Appearance("crmContractRegistrationForm.DocumentCategory.Enable", Criteria = "not IsNull(ContractDeal)", Enabled = false, BackColor = "15527155")]
        //[Appearance("crmContractRegistrationForm.DocumentCategory.Enable.Color", Criteria = "IsNull(ContractDeal)", Enabled = true, BackColor = "16116735")]
        [ImmediatePostData]
        public crmContractDocumentType DocumentCategory {
            get { return this.ContractDocument.DocumentCategory; }
            set {
                this.ContractDocument.DocumentCategory = value;
                OnChanged("DocumentCategory");
            }
        }

        [Browsable(false)]
        public crmContractDocument ContractDocument;

        #endregion


        #region IWizardSupport Members

        BaseObject IWizardSupport.Complete() {

            // Пояснение к алгоритму регистрации договора.
            // Для того, чтобы зарегистрировать договор, необходимо помимо его создания в базе ещё создать для него запись в журнале регистрации.
            // Создание записи в журнале регистрации приводит к выдаче номера регистрации для договора. Этот номер
            // создаётся при заведении записи в журнале регистрации и эта запись должна быть немедленной для исключения
            // возможности присвоения такого же номера в другом сеансе. Но заводить запись для несуществующего в базе договора
            // неправильно, следовательно, необходимо сначала сохранить договор в базе, затем сделать запись в журнале регистрации,
            // затем присвоить полученный номер договору и снова сохранить договор уже с номером в базе.
            // Указанные действия осуществляются в рамках одной транзакции

            // Считаем, что транзакции в сессии сопряжена с транзакцией в БД.

            crmContractDeal deal;
            crmContract contract;
            if (RegistrationKind == RegistrationKind.NEW) {
                // В этом случае создаётся новый ContractDeal подходящего подтипа
                deal = RegisterDeal();
            } else { // RegistrationKind == RegistrationKind.PROJECT
                deal = this.ContractDeal;
            }
            deal.ContractKind = this.ContractKind;
            deal.State = DealStates.DEAL_FORMATION;
            deal.DateRegistration = this.DateRegistration;
            deal.UserRegistrator = this.UserRegistrator;
            deal.Delo = this.Delo;
            deal.DepartmentRegistrator = this.DepartmentRegistrator;
            deal.ContractDocument = this.ContractDocument;
            deal.Current.ContractDocument = this.ContractDocument;
            // Разбор другого выпадающего списка
            if (ContractKind == ContractKind.CONTRACT) {
                // Создаём контракт (объект crmContract) 
                contract = new crmContract(this.Session);
                //this.ContractDeal.Contract = contract;
                //deal.Contract = contract;

                contract.ContractCategory = this.Category;   // this.ContractCategory;
                contract.UserRegistrator = this.UserRegistrator;
                contract.DepartmentRegistrator = this.DepartmentRegistrator;

                contract.ContractDocument = this.ContractDocument;
            } else { // if (ContractKind == ContractKind.ADDENDUM)
                contract = this.Contract.Contract; 
            }
            contract.ContractDeals.Add(deal); // одно и то же
            try {
                if (this.IsRegRequired) {
                    crmContractRegistrationLog crl = new crmContractRegistrationLog(this.Session, System.DateTime.Now.Year, System.DateTime.Now, this.NewNumberRequired, deal);
                    crl.Save();
                    if (NewNumberRequired) {
                        this.ContractDocument.Number = crl.getDocumentNumber();
                    }
                }
            }
            catch (Exception ex) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Contract saving Error: " + ex.ToString());
            }
            contract.ContractDocuments.Add(this.ContractDocument);
            // SHU 2011-10-03 this.Session.CommitTransaction();
            return deal.Current;
        }

        #endregion
    }

}
