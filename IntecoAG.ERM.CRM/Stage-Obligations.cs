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

namespace IntecoAG.ERM.CRM
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Stage
    {
        [Association("crmStage-Obligations"), Aggregated]
        public XPCollection<Obligation> _Obligations {
            get {
                return GetCollection<Obligation>("_Obligations");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Obligation
    {
        private Stage _Stage;
        [Association("crmStage-Obligations")]
        public Stage Stage {
            get { return _Stage; }
            set { SetPropertyValue("Stage", ref _Stage, value); }
        }
    }

}
