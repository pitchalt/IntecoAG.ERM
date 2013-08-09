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
using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmContract
    {
        [Association("crmContract-ContractDeals"), Aggregated]
        public XPCollection<crmContractDeal> ContractDeals {
            get {
                return GetCollection<crmContractDeal>("ContractDeals");
            }
        }
    }
}


namespace IntecoAG.ERM.CRM.Contract.Deal {
    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmContractDeal
    {
        private crmContract _Contract;
        [Association("crmContract-ContractDeals")]
        //[Browsable(false)]
        public crmContract Contract {
            get { return _Contract; }
            set { 
                SetPropertyValue<crmContract>("Contract", ref _Contract, value);
                if (!IsLoading && value != null) {
                    value.IntCurDocNumber++;
                    this.IntNumber = value.IntCurDocNumber;
                }
            }
        }
    }

}
