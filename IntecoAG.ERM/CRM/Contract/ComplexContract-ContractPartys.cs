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
        [Association("crmComplexContract-ComplexContractPartys"), Aggregated]
        public XPCollection<ContractParty> ComplexContractPartys {
            get {
                return GetCollection<ContractParty>("ComplexContractPartys");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ContractParty
    {
        private ComplexContract _ComplexContract;
        [Association("crmComplexContract-ComplexContractPartys")]
        public ComplexContract ComplexContract {
            get { return _ComplexContract; }
            set { SetPropertyValue<ComplexContract>("ComplexContract", ref _ComplexContract, value); }
        }
    }

}
