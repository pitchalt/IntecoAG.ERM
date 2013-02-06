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
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;

using IntecoAG.ERM.CRM.Party;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class ComplexContractVersion
    {
        [Association("crmComplexContractVersion-ContractPartys"), Aggregated]
        public XPCollection<crmContractParty> ContractPartys {
            get {
                return GetCollection<crmContractParty>("ContractPartys");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmContractParty
    {
        private ComplexContractVersion _ComplexContractVersion;
        [Association("crmComplexContractVersion-ContractPartys")]
        public ComplexContractVersion ComplexContractVersion {
            get { return _ComplexContractVersion; }
            set { SetPropertyValue<ComplexContractVersion>("ComplexContractVersion", ref _ComplexContractVersion, value); }
        }
    }

}
