﻿#region Copyright (c) 2011 INTECOAG.
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

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
//using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.HRM.Organization;
//using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CRM.Contract.Deal;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

using IntecoAG.ERM.CRM.Counters;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс crmContractRegistrationLog - журнал регистрации входящих договоров
    /// Договора бывают внешние и внутренние, внутренним номер присваивается вручную, внешним - автоматически.
    /// Автоматически присваиваемый номер зависит от года и подразделения. С наступлением очередного года нумерация начинается с 1.
    /// Следовательно, номер имеет три компоненты: год, номер подразделения, очередной номер, букву модификации (А - Я) - нужна для
    /// вставки договоров в журнал как бы задним числом
    /// </summary>
    /// Атрибуты регистратора договоров.
    /// Номер, дата, Заказчик, Исполнитель, Валюта, Сумма, Краткий комментарий (DescriptionShort), 
    /// Срок до какого действует, Куратор подразделение, Регистрирующий сотрудник, Регистрирующее подразделение.
    //[DefaultClassOptions]
    [LikeSearchPathList(new string[] { 
        "ContractDeal.ContractDocument.Number",
        "ContractDeal.Current.Customer.Party.Name",
        "ContractDeal.Current.Customer.Party.INN",
        "ContractDeal.Current.Supplier.Party.Name",
        "ContractDeal.Current.Supplier.Party.INN",
        "UserRegistrator.LastName"
    })]
    [MiniNavigation("This", "Карточка записи журнала", TargetWindow.Default, 1)]
    [MiniNavigation("ContractDeal.Contract", "Договор", TargetWindow.Default, 2)]
    [MiniNavigation("ContractDeal.Current", "Простой договор", TargetWindow.Default, 3)]
    //[RepresentativeProperty("DemonstratedObject")]
    [Persistent("crmContractRegistrationLog")]
    public class crmContractRegistrationLog : BaseObject  //, ILog
    {
        public crmContractRegistrationLog(Session ses) : base(ses) { }

        public crmContractRegistrationLog(Session ses, int prmDocNumberYear, DateTime prmDate, bool NewNumberRequired, crmContractDeal prmContractDeal)
            : base(ses) {
            
            //DealVersion = prmContractDeal;
            ContractDeal = prmContractDeal;

            DocNumberYear = prmDocNumberYear;
            Date = prmDate;   // prmContractRegistrationForm.Date;

            // Присвоение DocNumber
            if (NewNumberRequired) {
                setAllNumbers();
            } else {
                // Регистрационный номер берётся из prmContractRegistrationForm
                setAllNumbers(ContractDeal.ContractDocument.Number);
            }
        }

        #region ПОЛЯ КЛАССА

        private crmContractRegistrationLog ContractRegistrationLogLastRecord = null;
        private crmContractRegistrationLog ContractRegistrationLogConditionalLastRecord = null;

        #endregion

        #region СВОЙСТВА КЛАССА


        private crmContractRegistrationForm _ContractRegistrationForm;
        [Browsable(false)]
        [NonPersistent]
        public crmContractRegistrationForm ContractRegistrationForm {
            get { return _ContractRegistrationForm; }
            set { SetPropertyValue<crmContractRegistrationForm>("ContractRegistrationForm", ref _ContractRegistrationForm, value); }
        }

        //private crmDealVersion _DealVersion;
        //public crmDealVersion DealVersion {
        //    get { return _DealVersion; }
        //    set { SetPropertyValue<crmDealVersion>("DealVersion", ref _DealVersion, value); }
        //}


        private crmContractDeal _ContractDeal;
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }

        #region Служебные поля

        // ISN - inner system number внутрисистемный номер (ВСН)
        private int _ISN;
        [Indexed(Name="ISN", Unique = true)]
        [Browsable(false)]
        public int ISN {
            get { return _ISN; }
            set { SetPropertyValue<int>("ISN", ref _ISN, value); }
        }


        // SortNumber - номер для выполнения операции сортировки order by SortNUmber
        private int _SortNumber;
        public int SortNumber {
            get { return _SortNumber; }
            set { SetPropertyValue<int>("SortNumber", ref _SortNumber, value); }
        }


        // CurrentUser - тот пользователь, в чъём сеансе произошла эта регистрация
        private User _CurrentUser;
        public User CurrentUser {
            get { return _CurrentUser; }
            set { SetPropertyValue<User>("CurrentUser", ref _CurrentUser, value); }
        }

        #endregion Служебные поля


        private int _DocNumberYear;
        //[Indexed(new string[] {"DocNumber", "Department"}, Name = "crmContractRegistrationForm_GroupIndex", Unique = true)]
        public int DocNumberYear {
            get { return _DocNumberYear; }
            set { SetPropertyValue<int>("DocNumberYear", ref _DocNumberYear, value); }
        }

        private int _DocNumber;
        public int DocNumber {
            get { return _DocNumber; }
            set { SetPropertyValue<int>("DocNumber", ref _DocNumber, value); }
        }

        ////private string _DocNumberDepartment;
        //public string DocNumberDepartment {
        //    //get { return _DocNumberDepartment; }
        //    //set { SetPropertyValue<string>("DocNumberDepartment", ref _DocNumberDepartment, value); }
        //    get { return DealVersion.Registrator.Code; }
        //}

        //private string _DocNumberModificator;
        //[Size(1)]
        //public string DocNumberModificator {
        //    get { return _DocNumberModificator; }
        //    set { SetPropertyValue<string>("DocNumberModificator", ref _DocNumberModificator, value); }
        //}


        // ---
        //private DealStates _DealState;
        //public DealStates DealState {
        //    get { return _DealState; }
        //    set { SetPropertyValue<DealStates>("DealState", ref _DealState, value); }
        //}

        //////[Persistent("DealState")]
        //////private DealStates dealState;

        [PersistentAlias("ContractDeal.Current.DealState")]
        public DealStates DealState {
            get { return ContractDeal.Current.DealState; }
        }
        // ---
              
  
        // ---
        //private crmContractDocument _ContractDocument;
        //public crmContractDocument ContractDocument {
        //    get { return _ContractDocument; }
        //    set { SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value); }
        //}

        //////[Persistent("ContractDocument")]
        //////private crmContractDocument contractDocument;

        [PersistentAlias("ContractDeal.ContractDocument")]
        public crmContractDocument ContractDocument {
            get { return ContractDeal.ContractDocument; }
        }
        // ---

        //public string DocumentNumber {
        //    get { return DealVersion.ContractDeal.Current.ContractDocument.Number; }
        //}


        private DateTime _Date;
        //[VisibleInListView(false)]
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }


        // ---
        //private crmContractParty _Customer;
        ////[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public crmContractParty Customer {
        //    get { return _Customer; }
        //    set {
        //        SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
        //    }
        //    //get { return DealVersion.ContractDeal.Current.Customer; }
        //}

        //////[Persistent("Customer")]
        //////private crmContractParty customer;

        [PersistentAlias("ContractDeal.Current.Customer")]
        public crmContractParty Customer {
            get { return ContractDeal.Current.Customer; }
        }
        // ---


        // ---
        //private crmContractParty _Supplier;
        ////[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public crmContractParty Supplier {
        //    get { return _Supplier; }
        //    set {
        //        SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
        //    }
        //    //get { return DealVersion.ContractDeal.Current.Supplier; }
        //}

        //////[Persistent("Supplier")]
        //////private crmContractParty supplier;

        [PersistentAlias("ContractDeal.Current.Supplier")]
        public crmContractParty Supplier {
            get { return ContractDeal.Current.Supplier; }
        }
        // ---

        // ---
        ///// <summary>
        ///// csValuta
        ///// </summary>
        //private csValuta _Valuta;
        //public csValuta Valuta {
        //    get { return _Valuta; }
        //    set {
        //        SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
        //    }
        //    //get { return DealVersion.Valuta; }
        //}

        //////[Persistent("Valuta")]
        //////private csValuta valuta;

        [PersistentAlias("ContractDeal.Current.Valuta")]
        public csValuta Valuta {
            get { return ContractDeal.Current.Valuta; }
        }
        // ---



        // ---
        //private decimal _Price;
        //public decimal Price {
        //    get { return _Price; }
        //    set { SetPropertyValue<decimal>("Price", ref _Price, value); }
        //    //get { return DealVersion.Price; }
        //}

        //////[Persistent("Price")]
        //////private decimal price;

        [PersistentAlias("ContractDeal.Current.Price")]
        public decimal Price {
            get { return ContractDeal.Current.Price; }
        }
        // ---



        // ---
        //private string _DescriptionShort;
        //public string DescriptionShort {
        //    get { return _DescriptionShort; }
        //    set { SetPropertyValue<string>("DescriptionShort", ref _DescriptionShort, value); }
        //    //get { return DealVersion.DescriptionShort; }
        //}

        //////[Persistent("DescriptionShort")]
        //////private string descriptionShort;

        [PersistentAlias("ContractDeal.Current.DescriptionShort")]
        public string DescriptionShort {
            get { return ContractDeal.Current.DescriptionShort; }
        }
        // ---



        // ---
        //private DateTime _DateEnd;
        //public DateTime DateEnd {
        //    get { return _DateEnd; }
        //    set { SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value); }
        //    //get { return DealVersion.DateEnd; }
        //}

        ////[Persistent("DateEnd")]
        ////private DateTime dateEnd;

        [PersistentAlias("ContractDeal.Current.DateEnd")]
        public DateTime DateEnd {
            get { return ContractDeal.Current.DateEnd; }
        }
        // ---


        // ---
        ///// <summary>
        ///// Curator
        ///// </summary>
        //private hrmDepartment _Curator;
        //public hrmDepartment Curator {
        //    get { return _Curator; }
        //    set { SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value); }
        //    //get { return DealVersion.Curator; }
        //}

        //////[Persistent("Curator")]
        //////private hrmDepartment curator;

        [PersistentAlias("ContractDeal.CuratorDepartment")]
        public hrmDepartment Curator {
            get { return ContractDeal.CuratorDepartment; }
        }
        // ---


        // ---
        //// Регистрирующий пользователь: Пользователь, осуществляющий регистрацию
        //private hrmStaff _UserRegistrator;
        //public hrmStaff UserRegistrator {
        //    get { return _UserRegistrator; }
        //    set {
        //        SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value);
        //    }
        //    //get { return DealVersion.ContractDeal.Contract.UserRegistrator; }
        //}

        //////[Persistent("UserRegistrator")]
        //////private hrmStaff userRegistrator;

        [PersistentAlias("ContractDeal.UserRegistrator")]
        public hrmStaff UserRegistrator {
            get { return ContractDeal.UserRegistrator; }
        }
        // ---



        // ---
        //// Регистрирующее подразделение: Подразделение, осуществляющее регистрацию договора. Определяется автоматически по регистрирующему пользователю
        //protected hrmDepartment _DepartmentRegistrator;
        //public hrmDepartment DepartmentRegistrator {
        //    get { return _DepartmentRegistrator; }
        //    set {
        //        SetPropertyValue<hrmDepartment>("DepartmentRegistrator", ref _DepartmentRegistrator, value);
        //    }
        //    //get { return DealVersion.ContractDeal.Contract.DepartmentRegistrator; }
        //}

        //////[Persistent("DepartmentRegistrator")]
        //////private hrmDepartment departmentRegistrator;

        [PersistentAlias("ContractDeal.DepartmentRegistrator")]
        public hrmDepartment DepartmentRegistrator {
            get { return ContractDeal.DepartmentRegistrator; }
        }

        private hrmDepartment _Department;
        public hrmDepartment Department {
            get { return _Department; }
            set {
//                _Department = ContractDeal.Current.ContractDeal.Contract.DepartmentRegistrator;
//                OnChanged("Department");
                SetPropertyValue<hrmDepartment>("Department", ref _Department, value); 
            }
        }

        // Из-за того, что DepartmentRegistrator вычислимый - он в базе не хранится
        // ---

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Создание записи о новом документе и выдача ему номера по журналу регистрации
        /// Номер теоретически не обязательно последовательный, может например, идти с шагом 10 и т.п.
        /// </summary>
        /// <returns></returns>
        public void setAllNumbers() {
            this.ISN = getISN();
            this.DocNumber = getDocNumber();
            this.SortNumber = this.ISN;   // getSortNumber();
            //this.DocNumber = getDocumentNumber();
            ContractDeal.ContractDocument.Number = this.DocNumber.ToString();   // getDocumentNumber();
        }

        public void setAllNumbers(string docNum) {
            //this.DocumentNumber = docNum;
            this.ISN = getISN();
            this.SortNumber = this.ISN;   // getSortNumber();
            ContractDeal.ContractDocument.Number = docNum;
        }

        /// <summary>
        /// Получение очередного абсолютного номера записи в базе
        /// </summary>
        public int getISN() {

            // Вариант с XPCollection

            Session ssn = this.Session;   // new Session(this.Session.Dictionary);

            XPCollection<RegistrationLogISNCounter> xpCounterCol = new XPCollection<RegistrationLogISNCounter>(ssn);
            if (!xpCounterCol.IsLoaded) xpCounterCol.Load();


            if (xpCounterCol.Count == 0) {
                
                // SHU 2011-11-03 ПЕРЕДЕЛАТЬ НА Session!
                using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer)) {
                    RegistrationLogISNCounter newCounter = new RegistrationLogISNCounter(uow);
                    newCounter.Save();
                    uow.CommitChanges();
                    xpCounterCol.Reload();
                    if (!xpCounterCol.IsLoaded) xpCounterCol.Load();
                }

            }

            // Номер ISN
            return xpCounterCol[0].ReserveNumber();

        }

        /// <summary>
        /// Получение очередного номера для сортировки Пока алгоритм не отличается от алгоритма получения ISN
        /// </summary>
        public int getDocNumber() {

            // Вариант 2 (с XPCollection)

            Session ssn = this.Session;   // new Session(this.Session.Dictionary);

            XPCollection<RegistrationLogDocNumberCounter> xpCounterCol = new XPCollection<RegistrationLogDocNumberCounter>(ssn);

            // Условия
            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            CriteriaOperator criteriaYear = new BinaryOperator("year", DocNumberYear, BinaryOperatorType.Equal);
            ((GroupOperator)criteria).Operands.Add(criteriaYear);

            CriteriaOperator criteriaDepartment = null;
            if (DepartmentRegistrator != null) {
                criteriaDepartment = new BinaryOperator("Department", DepartmentRegistrator, BinaryOperatorType.Equal);
                ((GroupOperator)criteria).Operands.Add(criteriaDepartment);
            }

            xpCounterCol.Criteria = criteria;

            if (!xpCounterCol.IsLoaded) xpCounterCol.Load();


            if (xpCounterCol.Count == 0) {

                // SHU 2011-11-03 ПЕРЕДЕЛАТЬ НА Session!
                // Другой работающий вариант. Здесь плохо с Connection (надо явно прописывать). 
                // ssn.BeginNestedUnitOfWork не даёт желаемого результата
                using (UnitOfWork uow = new UnitOfWork(this.Session.Dictionary)) {
                    uow.Connection = ssn.Connection;
                    //if (uow.Connection.State == System.Data.ConnectionState.Closed) uow.Connect();
                    RegistrationLogDocNumberCounter newCounter = new RegistrationLogDocNumberCounter(uow);
                    newCounter.Year = this.DocNumberYear;

                    hrmDepartment dep = uow.GetObjectByKey(this.DepartmentRegistrator.GetType(), this.DepartmentRegistrator.ClassInfo.GetId(this.DepartmentRegistrator)) as hrmDepartment;

                    newCounter.Department = dep;   // this.DepartmentRegistrator;
                    newCounter.Save();
                    uow.FlushChanges();
                    //if (uow.Connection.State == System.Data.ConnectionState.Open) uow.Connection.Close();
                    xpCounterCol.Reload();
                    if (!xpCounterCol.IsLoaded) xpCounterCol.Load();
                }

            }

            // Номер документа
            return xpCounterCol[0].ReserveDocNumber();
        }


        /// <summary>
        /// Получение очередного номера для сортировки Пока алгоритм не отличается от алгоритма получения ISN
        /// </summary>
        public int getSortNumber() {
            //// Смотрим, какой наибольший номер есть в базе
            //CriteriaOperator criteriaSortNumber = CriteriaOperator.Parse("SortNumber = Max(SortNumber)");

            //XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteriaSortNumber, null);
            //if (!crl.IsLoaded) crl.Load();

            //if (crl.Count > 0) {
            //    this.SortNumber = crl[0].SortNumber + 1;
            //} else {
            //    this.SortNumber = 1;
            //}

            if (getConditionalLastISN() != 0) {
                return ContractRegistrationLogConditionalLastRecord.SortNumber + 1;
            } else {
                return 1;
            }
        }

        public int getLastISN() {
            if (ContractRegistrationLogLastRecord != null) return ContractRegistrationLogLastRecord.ISN;

            // Смотрим, какой наибольший номер есть в базе
            //CriteriaOperator criteriaISN = CriteriaOperator.Parse("ISN = Max(ISN)");

            //SortingCollection sorting = new SortingCollection();
            ////sorting.Add(new SortProperty("", SortingDirection.Ascending)); 
            //sorting.Add(new SortProperty("ISN", DevExpress.Xpo.DB.SortingDirection.Descending));
            SortProperty[] sorting = { new SortProperty("ISN", DevExpress.Xpo.DB.SortingDirection.Descending) };

            //XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteriaISN, sorting);
            XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, null, sorting);
            crl.TopReturnedObjects = 1;
            if (!crl.IsLoaded) crl.Load();

            if (crl.Count > 0) {
                ContractRegistrationLogLastRecord = crl[0];
                return crl[0].ISN;
            } else {
                return 0;
            }
        }

        public int getConditionalLastISN() {
            if (ContractRegistrationLogConditionalLastRecord != null) return ContractRegistrationLogConditionalLastRecord.ISN;

            // Смотрим, какой наибольший номер есть в базе
            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            CriteriaOperator criteriaDocNumberYear = new BinaryOperator("DocNumberYear", DocNumberYear, BinaryOperatorType.Equal);
            ((GroupOperator)criteria).Operands.Add(criteriaDocNumberYear);

            //CriteriaOperator criteriaDocNumberDepartment = null;
            //if (!string.IsNullOrEmpty(DocNumberDepartment)) {
            //    criteriaDocNumberDepartment = new BinaryOperator("DocNumberDepartment", DocNumberDepartment, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteria).Operands.Add(criteriaDocNumberDepartment);
            //}

            CriteriaOperator criteriaDocNumberDepartment = null;
            if (DepartmentRegistrator != null) {
                criteriaDocNumberDepartment = new BinaryOperator("DepartmentRegistrator", DepartmentRegistrator, BinaryOperatorType.Equal);
                ((GroupOperator)criteria).Operands.Add(criteriaDocNumberDepartment);
            }

            SortProperty[] sorting = { new SortProperty("ISN", DevExpress.Xpo.DB.SortingDirection.Descending) };

            //XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteriaISN, sorting);
            XPCollection<crmContractRegistrationLog> crl = new XPCollection<crmContractRegistrationLog>(this.Session, criteria, sorting);
            crl.TopReturnedObjects = 1;
            if (!crl.IsLoaded) crl.Load();

            if (crl.Count > 0) {
                ContractRegistrationLogConditionalLastRecord = crl[0];
                return crl[0].ISN;
            } else {
                return 0;
            }
        }

        public string getDocumentNumber() {
            //string documentNumber = "№ " + DepartmentRegistrator.Name + "-" + DocNumberYear.ToString() + "/" + DocNumber.ToString();   // +DocNumberModificator;

            string documentNumber;
            if (String.IsNullOrEmpty(DepartmentRegistrator.PostCode)) {
                documentNumber = DocNumber.ToString() + "/" + DepartmentRegistrator.Code + "-" + DocNumberYear.ToString().Substring(2,2);   // +DocNumberModificator;
            }
            else {
                documentNumber = DocNumber.ToString() + "/" + DepartmentRegistrator.PostCode + "-" + DocNumberYear.ToString().Substring(2, 2);   // +DocNumberModificator;
            }
            return documentNumber;
        }


/*
        /// <summary>
        /// Конструктор фильтра
        /// </summary>
        /// <returns></returns>
        private CriteriaOperator BuidFilterCriteria(DetailView dv) {
            CriteriaOperator criteria = null;


            //// Находим, если существует запись CURRENT. В противном случае - создаём.
            //// Работающий вариант (три строки)
            //OperandProperty prop = new OperandProperty("Contract");
            //CriteriaOperator op = prop == this;
            //ContractVers.Filter = op;

            ////CriteriaOperator criteria = new NotOperator(new NullOperator("Contract")); // Проверка на not null - работает
            //// Ошибка из-за this : CriteriaOperator criteria = new BinaryOperator(new OperandProperty("Contract"), this, BinaryOperatorType.Equal);
            ////ContractVers.Filter = criteria;

            //return ContractVers; // new XPCollection<Contract>(this.Session); 



            //CriteriaOperator criteria = new GroupOperator(GroupOperatorType.And,
            //    new BinaryOperator(new OperandProperty("VersionState"), 1, BinaryOperatorType.Equal),
            //    new NotOperator(new NullOperator("Contract")));

            ////CriteriaOperator FindCurrentCriteria = new BinaryOperator(new OperandProperty("VersionState"), 1, BinaryOperatorType.Equal);

            ////Contract CurrentCOntract = ObjectSpace.FindObject<Contract>(FindCurrentCriteria);

            ////this.ContractSemantic = objContractSemantic;



            if (dv.CurrentObject != null) {
                criteria = ((ContractSemanticFilter)dv.CurrentObject).BuidFilterCriteria();
            } else {
                criteria = CriteriaOperator.Parse("1=1");
            }

            return criteria;
        }
        
        /// <summary>
        /// Конструктор фильтра
        /// </summary>
        /// <returns></returns>
        private CriteriaOperator BuidFilterCriteria(DetailView dv) {
            CriteriaOperator criteria = null;
            //CriteriaOperator criteria1 = new InOperator("Name", new string[] { "John", "Mike", "Nick" });

            Session ss = null;
        }
                    
        #region ПОСТРОЕНИЕ ФИЛЬТРА

        public CriteriaOperator BuidFilterCriteria() {
            CriteriaOperator criteria = null;
            //criteria = new BinaryOperator("Name", this.Name);   //, BinaryOperatorType.Like);
            //criteria = CriteriaOperator.Parse(
            //    "Number like '%" + this.Number + "%'" +
            //    " AND (NOT IsNull(RegNumber) OR RegNumber='" + this.RegNumber + "')" +
            //    " AND (NOT IsNull(Description) OR Description='" + this.Description + "')" //+
            //    //" AND (NOT IsNull(Date) OR Date='" + this.Date + "')" +
            //    //" AND (NOT IsNull(INN) OR INN='" + this.INN + "')" +
            //    //" AND (NOT IsNull(KPP) OR KPP='" + this.KPP + "')" +
            //    //" AND (NOT IsNull(IsHolding) OR IsHolding='" + this.IsHolding + "')"
            //    );
            ////criteria = CriteriaOperator.Parse(
            ////    " (NOT IsNull(RegNumber) OR RegNumber='" + this.RegNumber + "')" //+
            ////    );

            //criteria = CriteriaOperator.Parse(
            //    " (RegNumber='" + this.RegNumber + "')" //+
            //    );


            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;


            CriteriaOperator criteriaNumber = null;
            if (!string.IsNullOrEmpty(Number)) {
                criteriaNumber = new BinaryOperator("Number", "%" + Number + "%", BinaryOperatorType.Like);
                ((GroupOperator)criteria).Operands.Add(criteriaNumber);
            }


            CriteriaOperator criteriaRegNumber = null;
            if (!string.IsNullOrEmpty(RegNumber)) {
                criteriaRegNumber = new BinaryOperator("RegNumber", "%" + RegNumber + "%", BinaryOperatorType.Like);
                ((GroupOperator)criteria).Operands.Add(criteriaRegNumber);
            }


            CriteriaOperator criteriaDescription = null;
            if (!string.IsNullOrEmpty(Description)) {
                criteriaDescription = new BinaryOperator("Description", "%" + Description + "%", BinaryOperatorType.Like);
                ((GroupOperator)criteria).Operands.Add(criteriaDescription);
            }


            CriteriaOperator criteriaDateStart = null;
            //if (DateStart.ToString() != "01.01.0001 0:00:00") {
            if (DateStart != DateTime.MinValue) {
                criteriaDateStart = new BinaryOperator("Date", DateStart, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriaDateStart);
            }


            CriteriaOperator criteriaDateEnd = null;
            if (DateEnd != DateTime.MinValue) {
                criteriaDateEnd = new BinaryOperator("Date", DateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriaDateEnd);
            }

            CriteriaOperator criteriaRegDateStart = null;
            if (RegDateStart != DateTime.MinValue) {
                criteriaRegDateStart = new BinaryOperator("RegDate", RegDateStart, BinaryOperatorType.GreaterOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriaRegDateStart);
            }


            CriteriaOperator criteriaRegDateEnd = null;
            if (RegDateEnd != DateTime.MinValue) {
                criteriaRegDateEnd = new BinaryOperator("RegDate", RegDateEnd, BinaryOperatorType.LessOrEqual);
                ((GroupOperator)criteria).Operands.Add(criteriaRegDateEnd);
            }



            //    GroupOperatorType.And,
            //    new BinaryOperator(new OperandProperty("Contract.Contract"), this.Oid, BinaryOperatorType.Equal),
            //    new NotOperator(new NullOperator("Contract"))
            //);


            //CriteriaOperator criteria = new GroupOperator(GroupOperatorType.And,
            //    new BinaryOperator(new OperandProperty("Contract.Contract"), this.Oid, BinaryOperatorType.Equal),
            //    new NotOperator(new NullOperator("Contract")));


            //OperandProperty propLinkToCurrent = new OperandProperty("LinkToCurrent");
            //CriteriaOperator opLinkToCurrent = propLinkToCurrent == this.LinkToCurrent;

            //OperandProperty propVersion = new OperandProperty("VersionState");
            //CriteriaOperator opVersion = new GroupOperator(GroupOperatorType.And,
            //    new NotOperator(new BinaryOperator(propVersion, (int)VersionRecord.VersionStates.CURRENT, BinaryOperatorType.Equal)),
            //    new NotOperator(new BinaryOperator(propVersion, (int)VersionRecord.VersionStates.VERSION_CURRENT, BinaryOperatorType.Equal)),
            //    new NotOperator(new BinaryOperator(propVersion, (int)VersionRecord.VersionStates.VERSION_DECLINE, BinaryOperatorType.Equal)),
            //    new NotOperator(new BinaryOperator(propVersion, (int)VersionRecord.VersionStates.VERSION_OLD, BinaryOperatorType.Equal))
            //);

            //CriteriaOperator criteria = new GroupOperator(GroupOperatorType.And,
            //    opPrevVersion,
            //    opLinkToCurrent,
            //    opVersion
            //);





            return criteria;
        }

        #endregion
*/
        #endregion


        //#region Ilog

        //[NonPersistent]
        //public object DemonstratedObject {
        //    get {
        //        if (ContractDeal == null) return null;
        //        if (ContractDeal.Current == null) return null;
        //        return ContractDeal.Current;
        //    }
        //}

        //public object GetDemonstratedObject() {
        //    //if (ContractDeal == null) return null;
        //    //if (ContractDeal.Current == null) return null;
        //    //return ContractDeal.Current;
        //    return DemonstratedObject;
        //}

        //#endregion

    }
}