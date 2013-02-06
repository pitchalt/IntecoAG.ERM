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

using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal {

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmDealVersion
    {
        [Association("crmDealVersion-ObligationUnits")]
        public XPCollection<crmObligationUnit> ObligationUnits {
            get {
                return GetCollection<crmObligationUnit>("ObligationUnits");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract.Obligation {

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmObligationUnit
    {
        private XPDelayedProperty _DealVersion = new XPDelayedProperty();
        [VisibleInListView(false)]
        [Association("crmDealVersion-ObligationUnits")]
        [Delayed("_DealVersion", true)]
        public virtual crmDealVersion DealVersion {
            get { return (crmDealVersion) _DealVersion.Value; }
            set { _DealVersion.Value = value; }
        }
//        private crmDealVersion _DealVersion;
//        [VisibleInListView(false)]
//        [Association("crmDealVersion-ObligationUnits")]
//        public virtual crmDealVersion DealVersion {
//            get { return _DealVersion; }
//            set { SetPropertyValue<crmDealVersion>("DealVersion", ref _DealVersion, value); }
//        }
    }

}
