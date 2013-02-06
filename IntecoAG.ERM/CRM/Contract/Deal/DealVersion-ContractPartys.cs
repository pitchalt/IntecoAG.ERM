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
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmDealVersion
    {
        [Association("crmDealVersion-ContractPartys"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmContractParty> ContractParties {
            get {
                return GetCollection<crmContractParty>("ContractParties");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract {

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmContractParty
    {
        private crmDealVersion _DealVersion;
        [Association("crmDealVersion-ContractPartys")]
        [Browsable(false)]
        public crmDealVersion DealVersion {
            get { return _DealVersion; }
            set { SetPropertyValue<crmDealVersion>("DealVersion", ref _DealVersion, value); }
        }
    }

}
