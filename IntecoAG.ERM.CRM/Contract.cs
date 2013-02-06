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
    /// Класс Contract, представляющий объект Договора
    /// </summary>
    [Persistent("crmContract")]
    public partial class Contract : XPObject
    {
        public Contract() : base() { }
        public Contract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}