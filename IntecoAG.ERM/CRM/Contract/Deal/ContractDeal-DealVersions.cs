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

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmContractDeal
    {
        [Association("crmContractDeal-DealVersions"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmDealVersion> DealVersions {
            get {
                return GetCollection<crmDealVersion>("DealVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmDealVersion
    {
        private crmContractDeal _ContractDeal;
        [Association("crmContractDeal-DealVersions")]
        [Browsable(false)]
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("crmContractDeal", ref _ContractDeal, value); }
        }
    }

}
