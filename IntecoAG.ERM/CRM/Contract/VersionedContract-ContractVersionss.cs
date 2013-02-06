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
    public partial class VersionedContract
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
        private VersionedContract _VersionedContract;
        [Association("crmVersionedContract-ContractVersions")]
        public VersionedContract VersionedContract {
            get { return _VersionedContract; }
            set { SetPropertyValue<VersionedContract>("VersionedContract", ref _VersionedContract, value); }
        }
    }

}
