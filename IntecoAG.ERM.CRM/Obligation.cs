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

// === IntecoAG namespaces ===
//using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CRM
{
    /// <summary>
    /// Класс Obligation, представляющий обязательства по Договора
    /// </summary>
    [Persistent("crmObligation")]
    public abstract partial class Obligation : XPObject
    {
        public Obligation() : base() { }
        public Obligation(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}