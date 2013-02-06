using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.FM.Account {

    // Счёт по договору

    //[NavigationItem("Finance")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmAccountContract : fmAccountBase
    {
        public fmAccountContract(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmAccountContract);
            this.CID = Guid.NewGuid();

            //Curator = new hrmDepartment(this.Session);
            //Contract = new crmContract(this.Session);
            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        //private bool _AllowEditProperty;   // Разрешение редактирования

        private hrmDepartment _Curator; // Подразделение-заявитель (курирующее подразделение)
        private crmContract _Contract;     // Ссылка на договор, обязательна, если в PaymentRequestType указано договор

        #endregion

        #region СВОЙСТВА КЛАССА

        ///// <summary>
        ///// AllowEditProperty - Разрешение редактирования
        ///// </summary>
        //[ImmediatePostData]
        //[Browsable(false)]
        //public bool AllowEditProperty {
        //    get { return _AllowEditProperty; }
        //    set {
        //        SetPropertyValue<bool>("AllowEditProperty", ref _AllowEditProperty, value);
        //    }
        //}

        /// <summary>
        /// Подразделение-заявитель
        /// </summary>
        public hrmDepartment Curator {
            get { return _Curator; }
            set { SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value); }
        }

        /// <summary>
        /// Contract - Ссылка на договор, обязательна, если в PaymentRequestType указано договор
        /// </summary>
        public crmContract Contract {
            get { return _Contract; }
            set {
                SetPropertyValue<crmContract>("Contract", ref _Contract, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        //public void SetAllowEdit() {
        //    AllowEditProperty = true;
        //}

        //public void SetDisAllowEdit() {
        //    AllowEditProperty = false;
        //}

        //public bool GetAllowEdit() {
        //    return AllowEditProperty && !ReadOnly;
        //}

        protected override void OnSaving() {
            AllowEditProperty = false;
            base.OnSaving();
        }

        #endregion

    }

}
