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

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}