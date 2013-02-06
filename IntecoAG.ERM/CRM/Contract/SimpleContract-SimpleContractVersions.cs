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

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class SimpleContract
    {
        [Association("crmSimpleContract-SimpleContractVersions"), Aggregated]
        public XPCollection<SimpleContractVersion> SimpleContractVersions {
            get {
                return GetCollection<SimpleContractVersion>("SimpleContractVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class SimpleContractVersion
    {
        private SimpleContract _SimpleContract;
        [Association("crmSimpleContract-SimpleContractVersions")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public SimpleContract SimpleContract {
            get { return _SimpleContract; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.Category = value.Category;
                        this.Number = value.Contract.ContractDocument.Number;
                        this.Date = value.Contract.ContractDocument.Date;
                        this.DocumentCategory = value.Contract.ContractDocument.DocumentCategory;
                    }
                }
                SetPropertyValue<SimpleContract>("SimpleContract", ref _SimpleContract, value); 
            }
        }
    }

}
