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
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Статусы сделки
    /// </summary>
    public enum DealStates {
        DEAL_PROJECT = 1,   // Проект
        DEAL_FORMATION = 2,  // Оформление
        DEAL_RESOLVED = 3,  // Урегулирование
        DEAL_CONCLUDED = 4,  // Заключён
        DEAL_CLOSED = 5  // Закрыт
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
    public partial class crmContractDeal : csCComponent
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

        #endregion


        #region МЕТОДЫ

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

    }

}