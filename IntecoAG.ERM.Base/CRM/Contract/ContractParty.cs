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
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Exchange;
//
namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс ContractParty, представляющий участников договора
    /// </summary>
    //[DefaultClassOptions]
    [LikeSearchPathList(new string[] { 
        "TrwInternalNumber",
        "ContractDeal.TrwNumber",
        "Party.Person.TrwParty.TrwName",
        "Party.Person.TrwParty.TrwINN",
        "Party.Person.TrwParty.TrwKPP"
    })]
    [Persistent("crmContractParty")]
    [Appearance("", AppearanceItemType.Action, "", TargetItems = "Delete", Enabled = false)]
    public partial class crmContractParty : VersionRecord, TrwIContractParty//, IPersistentInterfaceData<TrwIContractParty>   //BaseObject, IVersionSupport
    {
        public crmContractParty(Session ses) : base(ses) { }
        public crmContractParty(Session ses, VersionStates state) : base(ses, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmUserParty _CfrUserParty;
        [Association("crmUserParty-crmDealPartys")]
        public crmUserParty CfrUserParty {
            get { return _CfrUserParty; }
            set { SetPropertyValue<crmUserParty>("CfrUserParty", ref _CfrUserParty, value); }
        }

        private crmContractDeal _ContractDeal;
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }

        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        private crmCParty _Party;
        [RuleRequiredField("crmContractParty.RequiredParty", "Save")]
        //[RuleRequiredField("crmContractParty.RequiredParty.Immediate", "Immediate")]
        public crmCParty Party {
            get { 
                return _Party;
            }
            set { 
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
                if (!this.IsLoading) {
                    if (value != null) {
                        this.Name = value.Name;
                        this.INN = value.INN;
                        this.KPP = value.KPP;
//                        this.RegNumber = value.RegNumber;
                        if (this.AddressLegal != null)
                            this.AddressLegal.Delete();
                        if (value.AddressLegal != null)
                            this.AddressLegal = value.AddressLegal.Copy();
                        if (this.AddressFact != null)
                            this.AddressFact.Delete();
                        if (value.AddressFact != null)
                            this.AddressFact = value.AddressFact.Copy();
                    }
                }
            }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        private string _Name;
        //[Appearance("crmContractParty.Name.Require.Caption", AppearanceItemType = "LayoutItem", BackColor = "Red", FontColor = "Black", FontStyle = System.Drawing.FontStyle.Bold, Criteria = "isnull(Number)")]
        //[Appearance("crmContractParty.Name.Require.Field", BackColor = "Red", FontColor = "Black", Criteria = "isnull(Name)")]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, (value == null) ? "" : value.Trim()); }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        private string _INN;
        public string INN {
            get { return _INN; }
            set { SetPropertyValue<string>("INN", ref _INN, (value == null) ? "" : value.Trim()); }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        private string _KPP;
        public string KPP {
            get { return _KPP; }
            set { SetPropertyValue<string>("KPP", ref _KPP, (value == null) ? "" : value.Trim()); }
        }
        /// <summary>
        /// Рег.Номер	Регистрационный номер
        /// Заполняется по регистрационному номеру в регистраторе (для юридических лиц РФ – ОГРНЮЛ)
        /// </summary>
        private string _RegNumber;
        public string RegNumber {
            get { return _RegNumber; }
            set { SetPropertyValue<string>("RegNumber", ref _RegNumber, (value == null) ? "" : value.Trim()); }
        }
        /// <summary>
        /// AddressLegal - юридический адрес
        /// </summary>
        private csAddress _AddressLegal;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressLegal {
            get { return _AddressLegal; }
            set { SetPropertyValue<csAddress>("AddressLegal", ref _AddressLegal, value); }
        }

        /// <summary>
        /// AddressFact - фактический адрес
        /// </summary>
        private csAddress _AddressFact;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressFact {
            get { return _AddressFact; }
            set { SetPropertyValue<csAddress>("AddressFact", ref _AddressFact, value); }
        }

        /// <summary>
        /// AddressPost - почтовый адрес
        /// </summary>
        private csAddress _AddressPost;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressPost {
            get { return _AddressPost; }
            set { SetPropertyValue<csAddress>("AddressPost", ref _AddressPost, value); }
        }

        /// <summary>
        /// crmBankAccount
        /// </summary>
        private crmBankAccount _BankAccount;
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set { 
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
                if (!IsLoading) {
                    OnChanged("Bank");
                    OnChanged("CurrentAcc");
                    OnChanged("BIC");
                    OnChanged("RCBIC");
                    OnChanged("KorAcc");
                }
            }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        public crmBank Bank {
            get { return BankAccount != null ? BankAccount.Bank : null; }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        public string CurrentAcc {
            get { return BankAccount != null ? BankAccount.Number : String.Empty; }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        public string BIC {
            get { return Bank != null ? Bank.BIC : String.Empty; }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        public string RCBIC {
            get { return Bank != null ? Bank.RCBIC : String.Empty; }
        }
        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        public string KorAcc {
            get { return Bank != null ? Bank.KorAcc : String.Empty; }
        }
        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
//            Res = Person.ToString();
            return Res;
        }

        #endregion

        #region Trw 
        //
        [PersistentAlias("ContractDeal")]
        public TrwIContract TrwContract {
            get { return ContractDeal; }
        }

        private TrwContractPartyType _TrwContractPartyType;
        public TrwContractPartyType TrwContractPartyType {
            get { return _TrwContractPartyType; }
            set { SetPropertyValue<TrwContractPartyType>("TrwContractPartyType", ref _TrwContractPartyType, value); }
        }
        [PersistentAlias("ContractDeal.TrwInternalNumber")]
        public String TrwInternalNumber {
            get { return ContractDeal != null ? ContractDeal.TrwInternalNumber : null; }
        }

        [PersistentAlias("ContractDeal.TRVType")]
        public TrwContractType TrwContractType {
            get { return ContractDeal != null ? ContractDeal.TRVType : null; }
            set {
                if (ContractDeal != null)
                    ContractDeal.TRVType = value;
            }
        }

        [PersistentAlias("CfrUserParty")]
        public TrwICfr TrwCfr {
            get { return CfrUserParty; }
        }
        [PersistentAlias("CfrUserParty.Party.Person")]
        public TrwIPerson TrwCfrPerson {
            get { return CfrUserParty != null ? CfrUserParty.Party.Person : null; }
        }
        [PersistentAlias("Party.Person")]
        public TrwIPerson TrwPartyPerson {
            get {
                return Party != null ? Party.Person : null;
            }
        }
        //
        private TrwExchangeExportStates _TrwExportState;
        public TrwExchangeExportStates TrwExportState {
            get { return _TrwExportState; }
            set { SetPropertyValue<TrwExchangeExportStates>("TrwExportState", ref _TrwExportState, value); }
        }

        #endregion Trw

        //public TrwIContractParty Instance {
        //    get { return this; }
        //}


        public void Refresh() {
            throw new NotImplementedException();
        }
    }

}