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

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    ///  Î‡ÒÒ Subject
    /// </summary>
    [Persistent("crmSubject")]
    public partial class Subject : XPObject
    {
        public Subject() : base() { }
        public Subject(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        #endregion

    }

}