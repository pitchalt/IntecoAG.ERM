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
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// ����� Party, �������������� ��������� ��� ������� �� ����������������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmParty")]
    public abstract partial class crmParty : BaseObject
    {
        public crmParty(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.PartyName = String.Empty;
            this.Description = String.Empty;
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Name - ��������
        /// </summary>
        private crmPerson _Person;
        [Association("crmPerson-crmParty")]
        [RuleRequiredField("crmParty.RequiredPerson", "Save")]
        public crmPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmPerson>("Person", ref _Person, value); }
        }
        /// <summary>
        /// Name - ��������
        /// </summary>
        public string Name {
            get {
                if (String.IsNullOrEmpty(PartyName))
                    return Person == null ? String.Empty : Person.FullName;
                else
                    return PartyName;
            }
        }
        /// <summary>
        /// Name - ��������
        /// </summary>
        private string _PartyName;
        public string PartyName {
            get { return _PartyName; }
            set { SetPropertyValue<string>("PartyName", ref _PartyName, value == null ? String.Empty : value.Trim()); }
        }
        /// <summary>
        /// Name - ��������
        /// </summary>
        public csAddress AddressLegal {
            get {
                if (this.AddressLegalParty == null)
                    if (Person != null) return Person.Address;
                    else return null;
                else
                    return this.AddressLegalParty;
            }
        }
        /// <summary>
        /// Name - ��������
        /// </summary>
        public XPCollection<crmBankAccount> BankAccounts {
            get {
                if (Person != null) return Person.BankAccounts;
                else return null;
            }
        }
        /// <summary>
        /// AddressFact - ��������
        /// </summary>
        private csAddress _AddressLegalParty;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressLegalParty {
            get { return _AddressLegalParty; }
            set { SetPropertyValue<csAddress>("AddressLegalParty", ref _AddressLegalParty, value); }
        }
        /// <summary>
        /// AddressFact - ��������
        /// </summary>
        private csAddress _AddressFact;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public csAddress AddressFact {
            get { return _AddressFact; }
            set { SetPropertyValue<csAddress>("AddressFact", ref _AddressFact, value); }
        }

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ". " + _Description;
        }
        /// <summary>
        /// AddressFact - ��������
        /// </summary>
        [Action(PredefinedCategory.Edit, Caption = "AddresLegal Clear")]
        public void AddressLegalPartyClear() {
            if (this.AddressLegalParty != null) {
                this.AddressLegalParty.Delete();
                this.AddressLegalParty = null;
            }
        }
        #endregion

    }

}