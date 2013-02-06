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
    /// Класс ObligationContainer, представляющий пакеты обязательств по Договора
    /// </summary>
    [Persistent("crmObligationContainer")]
    public partial class ObligationContainer : Obligation
    {
        public ObligationContainer() : base() { }
        public ObligationContainer(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}