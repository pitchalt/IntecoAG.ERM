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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс Contract, представляющий объект Договора
    /// </summary>
    [NonPersistent]
    public abstract partial class ContractVersion : VersionRecord
    {
        public ContractVersion(Session ses) : base(ses) { }
        public ContractVersion(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            //            OidInitializationMode = DevExpress.Persistent.BaseImpl.OidInitializationMode.AfterConstruction;
            //            this.VersionState = VersionStates.VERSION_NEW;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private string _Number;
        [Size(30)]
        [RuleRequiredField("crmContractVersion.Number.Required", "Save")]
        //[RuleRequiredField("crmContractVersion.Number.Required.Immediate", "Immediate")]
        public virtual string Number {
            get { return _Number; }
            set {
                SetPropertyValue("Number", ref _Number, value);
            }
        }

        private DateTime _Date;
        [RuleRequiredField("crmContractVersion.Date.Required", "Save")]
        //[RuleRequiredField("crmContractVersion.Date.Required.Immediate", "Immediate")]
        public virtual DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }


        private crmContractDocumentCategory _DocumentCategory;
        [RuleRequiredField("crmContractVersion.DocumentCategory.Required", "Save")]
        //[RuleRequiredField("crmContractVersion.DocumentCategory.Immediate", "Immediate")]
        public virtual crmContractDocumentCategory DocumentCategory {
            get { return _DocumentCategory; }
            set {
                SetPropertyValue<crmContractDocumentCategory>("DocumentCategory", ref _DocumentCategory, value);
            }
        }

        protected crmContractCategory _Category;
        [RuleRequiredField("crmContractVersion.Category.Required", "Save")]
        //[RuleRequiredField("crmContractVersion.Category.Immediate", "Immediate")]
        public virtual crmContractCategory Category {
            get { return _Category; }
            set {
                SetPropertyValue<crmContractCategory>("Category", ref _Category, value);
            }
        }

        ///// <summary>
        ///// ContractDocument для версии
        ///// </summary>
        //private ContractDocument _VersionContractDocument;
        //[DataSourceProperty("")]
        //public ContractDocument VersionContractDocument {
        //    get { return _VersionContractDocument; }
        //    set { SetPropertyValue<ContractDocument>("VersionContractDocument", ref _VersionContractDocument, value); }
        //}
        private DateTime _DateBegin;
        [RuleRequiredField("ContractVersion.DateBegin.Required", "Save")]
        //[RuleRequiredField("crmStage.DateBegin.Required.Immediate", "Immediate")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set {
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
            }
        }

        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateEnd;
        [RuleRequiredField("ContractVersion.DateEnd.Required", "Save")]
        //[RuleRequiredField("crmStage.DateEnd.Required.Immediate", "Immediate")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
            }
        }

        #endregion

        
        #region МЕТОДЫ

        #endregion

    }

}