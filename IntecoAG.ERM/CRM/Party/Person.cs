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
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// ����� Person, �������������� ����������� ����
    /// </summary>
    [DefaultProperty("Name")]
    [Persistent("crmPartyPerson")]
    public partial class crmCPerson : csCComponent, crmIPerson
    {
        public crmCPerson(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Address = new csAddress(this.Session);
            this.ComponentType = typeof(crmCPerson);
            this.CID = Guid.NewGuid();
        }

        #region ���� ������
        //
        private crmPersonType _PersonType;
        private csAddress _Address;
        private String _Name;
        private String _NameFull;
        private String _RegCode;
        private String _INN;
        //
        #endregion


        #region �������� ������
        /// <summary>
        /// 
        /// </summary>
        public crmPersonType PersonType {
            get { return _PersonType; }
            set { SetPropertyValue<crmPersonType>("PersonType", ref _PersonType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Address.Country")]
        public csCountry Country {
            get { return Address == null ? null : Address.Country; }
            set {
                csCountry old = this.Country;
                if (old != value && Address != null) {
                    this.Address.Country = value;
                    OnChanged("Country", old, value);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        // SHU!!! 2011-08-24 �������� ����� �������� [RuleRequiredField("crmPerson.RequiredAddress", "Save")]
        public csAddress Address {
            get { return _Address; }
            set { SetPropertyValue<csAddress>("Address", ref _Address, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [Browsable(false)]
        [Association("crmPerson-crmBankAccount",typeof(crmBankAccount))]
        public XPCollection<crmBankAccount> BankAccounts {
            get { return GetCollection<crmBankAccount>("BankAccounts"); }
        }
        //
        IList<crmBankAccount> crmIPerson.BankAccounts {
            get { return BankAccounts; }
            //get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Association("crmPerson-crmParty", typeof(crmPartyRu))]
        [Browsable(false)]
        public XPCollection<crmPartyRu> Partys {
            get { return GetCollection<crmPartyRu>("Partys"); }
        }
        //
        IList<crmIParty> crmIPerson.Partys {
            get { return new ListConverter<crmIParty, crmPartyRu>(Partys); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(15)]
        public String INN {
            get { return _INN; }
            set { SetPropertyValue<String>("INN", ref _INN, value == null ? String.Empty : value.Trim()); }
        }
        /// <summary>
        /// Name
        /// </summary>
        [Size(100)]
        public String Name {
            get { return _Name; }
            set {
                SetPropertyValue<String>("Name", ref _Name, value);
            }
        }
        /// <summary>
        /// Full Name
        /// </summary>
        [Size(250)]
        public String NameFull {
            get { return _NameFull; }
            set {
                SetPropertyValue<String>("NameFull", ref _NameFull, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(20)]
        public String RegCode {
            get { return _RegCode; }
            set {
                SetPropertyValue<String>("RegCode", ref _RegCode, value);
            }
        }

        #endregion


        #region ������

        #endregion
    }

}