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
    public partial class crmStageMain
    {
        [Association("crmStageMain-Stages"), Aggregated]
        public XPCollection<crmStage> Stages {
            get {
                return GetCollection<crmStage>("Stages");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmStage
    {
        private crmStageMain _StageMain;
        [Association("crmStageMain-Stages")]
        public crmStageMain StageMain {
            get { return _StageMain; }
            set {
                SetPropertyValue<crmStageMain>("StageMain", ref _StageMain, value);
            }
        }
    }

}
