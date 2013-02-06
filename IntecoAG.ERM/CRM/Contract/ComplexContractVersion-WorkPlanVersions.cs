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
    public partial class ComplexContractVersion
    {
        [Association("crmComplexContractVersion-WorkPlanVersions"), Aggregated]
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
        private ComplexContractVersion _ComplexContractVersion;
        [Association("crmComplexContractVersion-WorkPlanVersions")]
        public ComplexContractVersion ComplexContractVersion {
            get { return _ComplexContractVersion; }
            set {
                if (!IsLoading && this.WorkPlan == null) {
                    // Создаём WorkPlan
                    WorkPlan wp = new WorkPlan(this.Session);
                    this.WorkPlan = wp;
                    wp.Current = this;
                    wp.WorkPlanVersions.Add(this);
                }
                SetPropertyValue<ComplexContractVersion>("ComplexContractVersion", ref _ComplexContractVersion, value); 
            }
        }
    }

}
