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
using DevExpress.Xpo;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class ContractVersion
    {
        [Association("crmContractVersion-ContractPartys"), Aggregated]
        public XPCollection<ContractParty> ContractPartys {
            get {
                return GetCollection<ContractParty>("ContractPartys");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ContractParty
    {
        private ContractVersion _ContractVersion;
        [Association("crmContractVersion-ContractPartys")]
        public ContractVersion ContractVersion {
            get { return _ContractVersion; }
            set { SetPropertyValue<ContractVersion>("ContractVersion", ref _ContractVersion, value); }
        }
    }

}
