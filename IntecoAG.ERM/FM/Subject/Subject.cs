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
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    ///  Î‡ÒÒ Subject
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("fmSubject")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class fmSubject : fmAbstractSubject
    {
        public fmSubject(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿
        [Association("fmSubject-Orders", typeof(Order.fmOrder))]
        public XPCollection<Order.fmOrder> Orders {
            get {
                return GetCollection<Order.fmOrder>("Orders");
            }
        }

        public override System.ComponentModel.IBindingList Children {
            get {
                return Orders;
                //return new BindingList<object>(); 
            }
        }

        public override ITreeNode Parent {
            get { return Direction; }
        }


        #endregion

    }

}