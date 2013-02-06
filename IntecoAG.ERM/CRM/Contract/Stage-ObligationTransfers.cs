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
    public partial class crmStage
    {
        [Association("crmStage-ObligationTransfers")]
        public XPCollection<ObligationTransfer> ObligationTransfers {
            get {
                return GetCollection<ObligationTransfer>("ObligationTransfers");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ObligationTransfer
    {
        protected crmStage _Stage;
        [Association("crmStage-ObligationTransfers")]
        public crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue("Stage", ref _Stage, value); }
        }
    }

}
