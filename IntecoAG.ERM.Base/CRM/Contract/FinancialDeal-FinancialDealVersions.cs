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

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmFinancialDeal
    {
        [Association("crmFinancialDeal-FinancialDealVersions"), Aggregated]
        public XPCollection<crmFinancialDealVersion> FinancialDealVersions {
            get {
                return GetCollection<crmFinancialDealVersion>("FinancialDealVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmFinancialDealVersion
    {
        private crmFinancialDeal _FinancialDeal;
        [Association("crmFinancialDeal-FinancialDealVersions")]
        public crmFinancialDeal FinancialDeal {
            get { return _FinancialDeal; }
            set {
                //if (!IsLoading) {
                //    if (value != null) {
                //        this.Category = value.Category;
                //        this.Number = value.Contract.ContractDocument.Number;
                //        this.Date = value.Contract.ContractDocument.Date;
                //        this.DocumentCategory = value.Contract.ContractDocument.DocumentCategory;
                //    }
                //}
                SetPropertyValue<crmFinancialDeal>("FinancialDeal", ref _FinancialDeal, value);
            }
        }
    }

}
