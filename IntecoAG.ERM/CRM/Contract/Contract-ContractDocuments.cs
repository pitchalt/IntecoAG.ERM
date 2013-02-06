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
    public partial class Contract
    {
        [Association("crmContract-ContractDocuments"), Aggregated]
        public XPCollection<ContractDocument> ContractDocuments {
            get {
                return GetCollection<ContractDocument>("ContractDocuments");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ContractDocument
    {
        private Contract _Contract;
        [VisibleInListView(false)]
        [Association("crmContract-ContractDocuments")]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue<Contract>("Contract", ref _Contract, value); }
        }
    }

}
