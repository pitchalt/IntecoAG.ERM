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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    ///  Î‡ÒÒ Direction
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("fmDirection")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class fmDirection : fmAbstractSubject
    {
        public fmDirection(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿
        public override System.ComponentModel.IBindingList Children {
            get { return Subjects; }
        }

        public override ITreeNode Parent {
            get { return null; }
        }

        #endregion

    }

}