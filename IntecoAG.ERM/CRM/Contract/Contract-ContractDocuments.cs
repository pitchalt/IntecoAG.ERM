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
    public partial class crmContract
    {
        [Association("crmContract-ContractDocuments"), Aggregated]
        public XPCollection<crmContractDocument> ContractDocuments {
            get {
                return GetCollection<crmContractDocument>("ContractDocuments");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmContractDocument
    {
        private crmContract _Contract;
        [VisibleInListView(false)]
        [Association("crmContract-ContractDocuments")]
        public crmContract Contract {
            get { return _Contract; }
            set { SetPropertyValue<crmContract>("Contract", ref _Contract, value); }
        }
    }

}
