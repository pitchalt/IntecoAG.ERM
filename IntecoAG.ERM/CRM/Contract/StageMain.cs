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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    ///  Î‡ÒÒ crmStageMain
    /// </summary>
    [Persistent("crmStageMain")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmStageMain : BaseObject //, IContractDealFactory, IVersionBusinessLogicSupport, IVersionMainObject
    {
        public crmStageMain(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        private crmStage _Current;
        //[Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmStage Current {
            get { return _Current; }
            set { SetPropertyValue<crmStage>("Current", ref _Current, value); }
        }

        private bool _Closed;
        public bool Closed {
            get { return _Closed; }
            set { SetPropertyValue<bool>("Closed", ref _Closed, value); }
        }

        private DateTime _DateClosed;
        public DateTime DateClosed {
            get { return _DateClosed; }
            set { SetPropertyValue<DateTime>("DateClosed", ref _DateClosed, value); }
        }

        #endregion


        #region Ã≈“Œƒ€

        #endregion
    }
}