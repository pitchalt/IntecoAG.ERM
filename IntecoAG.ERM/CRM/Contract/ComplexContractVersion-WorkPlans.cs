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
        [Association("crmComplexContractVersion-WorkPlans"), Aggregated]
        public XPCollection<WorkPlan> WorkPlans {
            get {
                return GetCollection<WorkPlan>("WorkPlans");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class WorkPlan
    {
        private ComplexContractVersion _ComplexContractVersion;
        [Association("crmComplexContractVersion-WorkPlans")]
        public ComplexContractVersion ComplexContractVersion {
            get { return _ComplexContractVersion; }
            set { SetPropertyValue<ComplexContractVersion>("ComplexContractVersion", ref _ComplexContractVersion, value); }
        }
    }

}
