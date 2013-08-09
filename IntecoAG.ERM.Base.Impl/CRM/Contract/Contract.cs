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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp;

using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс crmContract, представляющий объект Договора
    /// </summary>
    //[DefaultClassOptions]
    [MiniNavigation("This", "Карточка договора", TargetWindow.Current, 1)]
    //[RepresentativeProperty("This")]
    [Persistent("crmContract")]
    public partial class crmContract : BaseObject
    {
        public crmContract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            this.ContractDocument = new crmContractDocument(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private string _Description;
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        private crmContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmContractDocument ContractDocument {
            get { return _ContractDocument;}
            set { 
                SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value);
                if (!IsLoading) {
                    this.ContractDocuments.Add(this.ContractDocument);
                }
            }
        }

        //protected crmContractCategory _ContractCategory;
        //public crmContractCategory ContractCategory {
        //    get { return _ContractCategory; }
        //    set { SetPropertyValue<crmContractCategory>("ContractCategory", ref _ContractCategory, value); }
        //}

        // Регистрирующий пользователь: Пользователь, осуществляющий регистрацию
        private hrmStaff _UserRegistrator;
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator; }
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
        //
        private Int32 _IntCurDocNumber;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Int32 IntCurDocNumber {
            get { return _IntCurDocNumber; }
            set { SetPropertyValue<Int32>("IntCurDocNumber", ref _IntCurDocNumber, value); }
        }

        //
        private Int32 _IntNumber;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [Indexed(Unique = true)]
        public Int32 IntNumber {
            get { return _IntNumber; }
            set { 
                SetPropertyValue<Int32>("IntNumber", ref _IntNumber, value);
                if (!IsLoading && value != null) {
                    TrwIntNumberSet("D" + value.ToString("D6"));
                }
            }
        }
        //
        [Persistent("TrwIntNumber")]
        [Size(20)]
        [Indexed(Unique=true)]
        private String _TrwIntNumber;
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("_TrwIntNumber")]
        public String TrwIntNumber {
            get { return _TrwIntNumber; }
            //            set { SetPropertyValue<String>("TrwNumber", ref _TrwNumber, value); }
        }
        public void TrwIntNumberSet(String number) {
            String old = _TrwIntNumber;
            _TrwIntNumber = number;
            OnChanged("TrwIntNumber", old, number);
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

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}