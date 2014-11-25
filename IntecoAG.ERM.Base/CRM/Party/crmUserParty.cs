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
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Accounting;
using IntecoAG.ERM.FM.FinPlan;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Party, представляющий участника как сторону во взаимоотношениях
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [Persistent("crmUserParty")]
    public partial class crmUserParty : BaseObject, TrwICfr
    {
        public crmUserParty(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА
        #endregion

        public static IValueManager<crmUserParty> CurrentUserParty;
        public static crmUserParty CurrentUserPartyGet(Session ses) {
            if (CurrentUserParty == null) return null;
            if (CurrentUserParty.Value == null) return null;
            return ses.GetObjectByKey<crmUserParty>(CurrentUserParty.Value.Oid);
        }
        public static crmUserParty CurrentUserPartyGet(IObjectSpace os) {
            if (CurrentUserParty == null) return null;
            if (CurrentUserParty.Value == null) return null;
            return os.GetObject<crmUserParty>(CurrentUserParty.Value);
        }
        public static void CurrentUserPartySet(crmUserParty party) {
            CurrentUserParty.Value = party;
        }


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// 
        /// </summary>
        private crmCParty _Party;
        [RuleRequiredField("crmUserParty.Party.Required", "Save")]
        [VisibleInListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [ImmediatePostData]
        public crmCParty Party {
            get { return _Party; }
            set { 
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
                if (!IsLoading) {
                    OnChanged();
                }
            }
        }
        public String Name {
            get { return Party == null ? String.Empty : Party.Name; }
        }
        public String INN {
            get { return Party == null ? String.Empty : Party.INN; }
        }
        public String KPP {
            get { return Party == null ? String.Empty : Party.KPP; }
        }
        //
        [Association("crmUserParty-crmDealPartys")]
        public XPCollection<crmContractParty> DealPartys {
            get { return GetCollection<crmContractParty>("DealPartys"); }
        }

        [Persistent("AccountingContract")]
        protected FmAccountingContract _AccountingContract;
        [PersistentAlias("_AccountingContract")]
        public FmAccountingContract AccountingContract {
            get { return _AccountingContract; }
        }
        public void AccountingContractSet(FmAccountingContract value) {
            SetPropertyValue<FmAccountingContract>("AccountingContract", ref _AccountingContract, value);
        }

        [Persistent("AccountingFact")]
        protected FmAccountingFinancial _AccountingFact;
        [PersistentAlias("_AccountingFact")]
        public FmAccountingFinancial AccountingFact {
            get { return _AccountingFact; }
        }
        public void AccountingFactSet(FmAccountingFinancial value) {
            SetPropertyValue<FmAccountingFinancial>("AccountingFact", ref _AccountingFact, value);
        }

        #endregion


        #region МЕТОДЫ

        #endregion

        #region Trw

        private String _TrwCode;
        public String TrwCode {
            get { return _TrwCode; }
            set { SetPropertyValue<String>("TrwCode", ref _TrwCode, value); }
        }

        [PersistentAlias("Party.Person")]
        public TrwIPerson Person {
            get {
                return Party.Person;
            }
        }

        public IList<TrwIContractParty> TrwContractPartys {
            get {
                return new ListConverter<TrwIContractParty, crmContractParty>(DealPartys);
            }
        }

        #endregion Trw
    }

}