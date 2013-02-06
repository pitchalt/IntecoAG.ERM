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
    /// Класс для поддержки структуры этапов проекта
    /// </summary>
    [Persistent("crmStage")]
    public partial class Stage : ProjectWork
    {
        public Stage() : base() { }
        public Stage(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }
    }

}