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
    public partial class ComplexContract
    {
        [Association("crmComplexContract-WorkPlans"), Aggregated]
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
        private ComplexContract _ComplexContract;
        [Association("crmComplexContract-WorkPlans")]
        public ComplexContract ComplexContract {
            get { return _ComplexContract; }
            set { SetPropertyValue<ComplexContract>("ComplexContract", ref _ComplexContract, value); }
        }
    }

}
