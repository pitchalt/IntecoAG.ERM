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
    /// Класс Party, представляющий участника (как сторону) Договора
    /// </summary>
    [Persistent("crmParty")]
    public class Party : XPObject
    {
        public Party() : base() { }
        public Party(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}