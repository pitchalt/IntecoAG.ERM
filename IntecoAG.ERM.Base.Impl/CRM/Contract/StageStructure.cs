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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс StageStructure, представляющий структуру этапов договора
    /// </summary>
    [Persistent("crmStageStructure")]
    public partial class crmStageStructure : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmStageStructure(Session session) : base(session) {
            if (!IsLoading) {
                this.VersionState = VersionStates.VERSION_NEW;
                //VersionAfterConstruction();
            }
        }
        public crmStageStructure(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.FirstStage = new crmStage(this.Session, this.VersionState);
            this.Stages.Add(this.FirstStage);
            this.FirstStage.StageType = StageType.AGREGATE;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Stage - этапы
        /// </summary>
        private crmStage _FirstStage;
        [Aggregated]
        public crmStage FirstStage {
            get { return _FirstStage; }
            set { SetPropertyValue<crmStage>("FirstStage", ref _FirstStage, value); }
        }

        private crmDealVersion _DealVersion;
        [Browsable(false)]
        public crmDealVersion DealVersion {
            get { return _DealVersion; }
            set {
                SetPropertyValue<crmDealVersion>("DealVersion", ref _DealVersion, value);
                if (!IsLoading) {
                    foreach (crmStage stage in this.Stages) {
                        stage.DealVersion = value;
                    }
                }
            }
        }

        private crmContractParty _Customer;
        [Browsable(false)]
        public crmContractParty Customer {
            get { return _Customer; }
            set { 
                SetPropertyValue<crmContractParty>("Customer", ref _Customer, value);
                if (!IsLoading) {
                    foreach (crmStage stage in this.Stages) {
                        stage.Customer = value;
                    }
                }
            }
        }

        private crmContractParty _Supplier;
        [Browsable(false)]
        public crmContractParty Supplier {
            get { return _Supplier; }
            set { 
                SetPropertyValue<crmContractParty>("Supplier", ref _Supplier, value);
                if (!IsLoading) {
                    foreach (crmStage stage in this.Stages) {
                        stage.Supplier = value;
                    }
                }
            }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}