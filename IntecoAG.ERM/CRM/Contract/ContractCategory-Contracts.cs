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
    public partial class crmContractCategory
    {
        [Association("crmContractCategory-Contracts")]
        public XPCollection<crmContract> Contracts {
            get {
                return GetCollection<crmContract>("Contracts");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmContract
    {
        private crmContractCategory _ContractCategory;
        [VisibleInListView(false)]
        [Association("crmContractCategory-Contracts")]
        public crmContractCategory ContractCategory {
            get { return _ContractCategory; }
            set { SetPropertyValue<crmContractCategory>("ContractCategory", ref _ContractCategory, value); }
        }
    }

}
