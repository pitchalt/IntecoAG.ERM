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
    /// Класс crmContract, представляющий объект Договора
    /// </summary>
    [NonPersistent]
    public abstract partial class crmContractVersion : VersionRecord
    {
        public crmContractVersion(Session ses) : base(ses) { }
        public crmContractVersion(Session session, VersionStates state) : base(session, state) { }

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


        private crmContractDocumentType _DocumentCategory;
        [RuleRequiredField("crmContractVersion.DocumentCategory.Required", "Save")]
        //[RuleRequiredField("crmContractVersion.DocumentCategory.Immediate", "Immediate")]
        public virtual crmContractDocumentType DocumentCategory {
            get { return _DocumentCategory; }
            set {
                SetPropertyValue<crmContractDocumentType>("DocumentCategory", ref _DocumentCategory, value);
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
        ///// crmContractDocument для версии
        ///// </summary>
        //private crmContractDocument _VersionContractDocument;
        //[DataSourceProperty("")]
        //public crmContractDocument VersionContractDocument {
        //    get { return _VersionContractDocument; }
        //    set { SetPropertyValue<crmContractDocument>("VersionContractDocument", ref _VersionContractDocument, value); }
        //}
        private DateTime _DateBegin;
        [RuleRequiredField("crmContractVersion.DateBegin.Required", "Save")]
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
        [RuleRequiredField("crmContractVersion.DateEnd.Required", "Save")]
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