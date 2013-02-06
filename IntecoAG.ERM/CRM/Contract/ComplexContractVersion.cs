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
using System.Collections;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using System.Windows.Forms;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract {
    /// <summary>
    /// Класс ComplexContractVersion, представляющий объект Договора
    /// </summary>
    [Appearance("ComplexContractVersionApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "ComplexContractVersionApprove", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    //[DefaultClassOptions]
    [Persistent("crmComplexContractVersion")]
    public partial class ComplexContractVersion : ContractVersion
    {
        public ComplexContractVersion(Session session) : base(session) { }
        public ComplexContractVersion(Session session, VersionStates state) : base(session, state) { }

        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
//            OidInitializationMode = DevExpress.Persistent.BaseImpl.OidInitializationMode.AfterConstruction;
//            this.VersionState = VersionStates.VERSION_NEW;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        
        /// <summary>
        /// ContractDocument для версии
        /// </summary>
        private ContractDocument _VersionContractDocument;
        [DataSourceProperty("ComplexContract.Contract.ContractDocuments")]
        public ContractDocument VersionContractDocument {
            get { return _VersionContractDocument; }
            set { SetPropertyValue<ContractDocument>("VersionContractDocument", ref _VersionContractDocument, value); }
        }
        //
        public override crmContractCategory Category {
            get { return base.Category; }
            set {
                base.Category = value;
                if (!IsLoading) {
                    if (this.ComplexContract != null) {
                        if (this.ComplexContract.Category == null)
                            this.ComplexContract.Category = value;
                    }
                }
            }
        }

        public override string Number {
            get { return base.Number; }
            set {
                base.Number = value;
                if (!IsLoading) {
                    if (this.ComplexContract != null) {
                        if (this.ComplexContract.Contract.ContractDocument != null)
                            this.ComplexContract.Contract.ContractDocument.Number = value;
                    }
                }
            }
        }

        public override DateTime Date {
            get { return base.Date; }
            set {
                base.Date = value;
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (this.ComplexContract != null) {
                            if (this.ComplexContract.Contract.ContractDocument != null)
                                this.ComplexContract.Contract.ContractDocument.Date = value;
                        }
                    }
                }
            }
        }

        public override crmContractDocumentCategory DocumentCategory {
            get { return base.DocumentCategory; }
            set {
                base.DocumentCategory = value;
                if (!IsLoading) {
                    if (!IsLoading) {
                        if (this.ComplexContract != null) {
                            if (this.ComplexContract.Contract.ContractDocument != null)
                                this.ComplexContract.Contract.ContractDocument.DocumentCategory = value;
                        }
                    }
                }
            }
        }


        public override object MainObject {
            get { return this.ComplexContract; }
        }

        #endregion


        #region МЕТОДЫ

        #endregion


        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        #endregion

        
        #region КНОПКИ

        public void ApproveVersion() {
            this.ComplexContract.ApproveVersion(this);
        }

        protected override void TriggerObjectChanged(ObjectChangeEventArgs args) {
            if (this.VersionContractDocument == null) {
                base.TriggerObjectChanged(args);
                return;
            }
            //if (args.NewValue != null & (args.PropertyName == "Number" | args.PropertyName == "Date" | args.PropertyName == "DocumentCategory" | args.PropertyName == "VersionContractDocument")) {
            if (args.PropertyName == "Number" | args.PropertyName == "Date" | args.PropertyName == "DocumentCategory" | args.PropertyName == "VersionContractDocument") {
                if (this.VersionContractDocument == null) {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Перед редактированием полей 'Номер', 'Дата', 'Категория документа' необходимо указать документ!");
                } else {
                    this.VersionContractDocument.Number = this.Number;
                    this.VersionContractDocument.Date = this.Date;
                    this.VersionContractDocument.DocumentCategory = this.DocumentCategory;
                }
            }
            base.TriggerObjectChanged(args);
        }

        #endregion

    }

}