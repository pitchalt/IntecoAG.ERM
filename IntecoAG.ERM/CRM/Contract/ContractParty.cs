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
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

using IntecoAG.ERM.CRM.Party;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс ContractParty, представляющий участников договора
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmContractParty")]
    public partial class crmContractParty : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmContractParty(Session ses) : base(ses) { }
        public crmContractParty(Session ses, VersionStates state) : base(ses, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// crmPartyRu - описание
        /// </summary>
        private crmPartyRu _Party;
        [RuleRequiredField("crmContractParty.RequiredParty", "Save")]
        //[RuleRequiredField("crmContractParty.RequiredParty.Immediate", "Immediate")]
        public crmPartyRu Party {
            get { 
                return _Party;
            }
            set { 
                SetPropertyValue<crmPartyRu>("Party", ref _Party, value);
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

    }

}