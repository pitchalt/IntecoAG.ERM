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
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.Xpo;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmContractDeal {
        [Association("crmContractDeal-ObligationUnitMains"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmObligationUnitMain> ObligationUnitMains {
            get {
                return GetCollection<crmObligationUnitMain>("ObligationUnitMains");
            }
        }
    }
}


namespace IntecoAG.ERM.CRM.Contract.Obligation {
    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmObligationUnitMain {
        private crmContractDeal _ContractDeal;
        [Association("crmContractDeal-ObligationUnitMains")]
        [Browsable(false)]
        public crmContractDeal ContractDeal {
            get { return _ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref _ContractDeal, value); }
        }
    }
}
