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
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmFinancialStructure
    {
        [Association("crmFinancialStructure-crmFinancialDealVersion"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmFinancialDealVersion> FinancialDeals {
            get {
                return GetCollection<crmFinancialDealVersion>("FinancialDeals");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmFinancialDealVersion
    {
        private crmFinancialStructure _FinancialStructure;
        [Association("crmFinancialStructure-crmFinancialDealVersion")]
        [Browsable(false)]
        public crmFinancialStructure FinancialStructure {
            get { return _FinancialStructure; }
            set { SetPropertyValue<crmFinancialStructure>("FinancialStructure", ref _FinancialStructure, value); }
        }
    }

}
