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

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.Trw.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultProperty("Name")]
    [Persistent("crmBankAccount")]
    public partial class crmBankAccount : BaseObject
    {
        public crmBankAccount(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            TrwAccountType = TrwAccountType.ACCOUNT_CURRENT;
        }

        #region ПОЛЯ КЛАССА

        private crmCParty _PrefferedParty;
        private TrwAccountType _TrwAccountType;
        private crmBankAccount _TrwIntermediaAccount;

        #endregion


        #region СВОЙСТВА КЛАССА

        public TrwAccountType TrwAccountType {
            get { return _TrwAccountType; }
            set { SetPropertyValue<TrwAccountType>("TrwAccountType", ref _TrwAccountType, value); }
        }
        public Int32 TrwAccountTypeCode {
            get { return (Int32) TrwAccountType; }
        }

        [DataSourceCriteria("Person.TrwParty.PartyType == 'PARTY_INTERMEDIA_TREASURE' || Person.TrwParty.PartyType == 'PARTY_INTERMEDIA_BANK'")]
        public crmBankAccount TrwIntermediaAccount {
            get { return _TrwIntermediaAccount; }
            set { SetPropertyValue<crmBankAccount>("TrwIntermediaAccount", ref _TrwIntermediaAccount, value); }
        }

        private crmBank _Bank;
        public crmBank Bank {
            get { return _Bank; }
            set { SetPropertyValue<crmBank>("Bank", ref _Bank, value); }
        }

        private crmCPerson _Person;
        [Association("crmPerson-crmBankAccount")]
        public crmCPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmCPerson>("Person", ref _Person, value); }
        }
        
        private string _Number;
        [Size(30)]
        public string Number {
            get { return _Number; }
            set { SetPropertyValue("Number", ref _Number, (value == null) ?  null  : value.Trim()); }
        }

        [Browsable(false)]
        public string Name {
            get {
                if (Bank != null)
                    return string.Concat(Number, " ", Bank.FullName);
                else
                    return this.Number;
            }
        }

        /// <summary>
        /// Предпочтительная сторона
        /// </summary>
        [DataSourceProperty("Person.Partys")]
        public crmCParty PrefferedParty {
            get { return _PrefferedParty; }
            set { SetPropertyValue<crmCParty>("PrefferedParty", ref _PrefferedParty, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ses"></param>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public static csValuta GetValutaByBankAccount(Session ses, crmBankAccount bankAccount) {
            if (bankAccount == null || bankAccount.Number == null || bankAccount.Number.Length < 8)
                return null;
            if (string.IsNullOrEmpty(bankAccount.Bank.RCBIC)) {
                return null;   // !!!!!!! ПЕРЕСМОТРЕТЬ !!!!!!!!!!!!
            }
            XPQuery<csValuta> valutas = new XPQuery<csValuta>(ses, true);
            var queryRes = from valuta in valutas
                    where valuta.CodeCurrencyValue == bankAccount.Number.Substring(5, 3)
                    select valuta;
            foreach (var valuta in queryRes) {
                return valuta;
            }
            return null;
            //return (new XPQuery<csValuta>(ses)).Where(p => p.CodeCurrencyValue == bankAccount.Number.Substring(5, 3)).First();
        }

        #endregion

    }

}