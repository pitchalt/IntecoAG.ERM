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
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmStageMain
    {
        [Association("crmStageMain-ObligationUnitMains"), Aggregated]
        public XPCollection<crmStage> ObligationUnitMains {
            get {
                return GetCollection<crmStage>("ObligationUnitMains");
            }
        }
    }
}

namespace IntecoAG.ERM.CRM.Contract.Obligation {
    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmObligationUnitMain
    {
        private crmStageMain _StageMain;
        [Association("crmStageMain-ObligationUnitMains")]
        public crmStageMain StageMain {
            get { return _StageMain; }
            set {
                SetPropertyValue<crmStageMain>("StageMain", ref _StageMain, value);
            }
        }
    }

}
