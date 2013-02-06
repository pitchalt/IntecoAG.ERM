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
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class WorkPlan
    {
        [Association("crmWorkPlan-WorkPlanVersions"), Aggregated]
        public XPCollection<WorkPlanVersion> WorkPlanVersions {
            get {
                return GetCollection<WorkPlanVersion>("WorkPlanVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class WorkPlanVersion
    {
        private WorkPlan _WorkPlanLink;
        [Association("crmWorkPlan-WorkPlanVersions")]
        public WorkPlan WorkPlan {
            get { return _WorkPlanLink; }
            set { SetPropertyValue<WorkPlan>("WorkPlan", ref _WorkPlanLink, value); }
        }
    }

}
