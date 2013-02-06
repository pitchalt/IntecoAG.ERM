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

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmContractDeal {
        [Association("crmContractDeal-StageMains"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmStageMain> StageMains {
            get {
                return GetCollection<crmStageMain>("StageMains");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract {
    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmStageMain {
        private crmContractDeal __ContractDeal;
        [Association("crmContractDeal-StageMains")]
        [Browsable(false)]
        public crmContractDeal ContractDeal {
            get { return __ContractDeal; }
            set { SetPropertyValue<crmContractDeal>("ContractDeal", ref __ContractDeal, value); }
        }
    }
}
