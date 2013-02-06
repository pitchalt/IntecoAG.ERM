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
    public partial class ContractImplementation
    {
        [Association("crmVersionedContract-ContractVersions"), Aggregated]
        public XPCollection<ContractVersion> ContractVersions {
            get {
                return GetCollection<ContractVersion>("ContractVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ContractVersion
    {
        private ContractImplementation _VersionedContract;
        [Association("crmVersionedContract-ContractVersions")]
        public ContractImplementation VersionedContract {
            get { return _VersionedContract; }
            set { SetPropertyValue<VersionedContract>("ContractImplementation", ref _VersionedContract, value); }
        }
    }

}
