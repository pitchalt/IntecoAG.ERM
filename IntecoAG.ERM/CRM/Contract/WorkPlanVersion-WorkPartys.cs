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
    public partial class WorkPlanVersion
    {
        [Association("crmWorkPlanVersion-WorkPartys"), Aggregated]
        public XPCollection<WorkParty> WorkPlanVersionPartys {
            get {
                return GetCollection<WorkParty>("WorkPlanVersionPartys");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class WorkParty
    {
        private WorkPlanVersion _WorkPlanVersion;
        [Association("crmWorkPlanVersion-WorkPartys")]
        public WorkPlanVersion WorkPlanVersion {
            get { return _WorkPlanVersion; }
            set {
                if (!IsLoading) {
                    if (value != null) value.AddContractParty(this);
                    //value.RemoveContractParty(this);
                }
                SetPropertyValue<WorkPlanVersion>("WorkPlanVersion", ref _WorkPlanVersion, value);
            }
        }
    }

}
