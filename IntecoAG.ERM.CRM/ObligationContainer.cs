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
    /// Класс ObligationTransfer, представляющий трансферы обязательствам Договора
    /// </summary>
    [Persistent("crmObligationTransfer")]
    public class ObligationTransfer : Obligation
    {
        public ObligationTransfer() : base() { }
        public ObligationTransfer(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}