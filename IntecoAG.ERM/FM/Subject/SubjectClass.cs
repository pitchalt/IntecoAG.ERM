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

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

using IntecoAG.ERM.GFM;

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    ///  Î‡ÒÒ Subject
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("fmSubject")]
    public partial class SubjectClass : BaseObject, ISubject
    {
        public SubjectClass(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.AbstractAnalitic = new AbstractAnaliticClass(this.Session);
        }


        #region œŒÀﬂ  À¿——¿
        private DirectionClass _Direction;
        #endregion

        #region Generic fmAbstractAnalitic

        [Browsable(false)]
        public AbstractAnaliticClass AbstractAnalitic;

        #endregion

        #region Associations

        [Association("fmDirection-Subjects")]
        public DirectionClass Direction {
            get { return _Direction; }
            set { SetPropertyValue<DirectionClass>("Direction", ref _Direction, value); }
        }

        [Association("fmSubject-Orders", typeof(Order.fmOrder)), Aggregated]
        public XPCollection<Order.fmOrder> Orders {
            get {
                return GetCollection<Order.fmOrder>("Orders");
            }
        }

        #endregion

        #region —¬Œ…—“¬¿  À¿——¿

        [PersistentAlias("AbstractAnalitic.Code")]
        //        [Size(15)]
        //        [RuleRequiredField]
        public String Code {
            get { return AbstractAnalitic.Code; }
            set {
                String old = AbstractAnalitic.Code;
                if (old == value) return;
                AbstractAnalitic.Code = value;
                OnChanged("Code", old, value);
            }
        }
        //
        [VisibleInLookupListView(true)]
        [PersistentAlias("AbstractAnalitic.Name")]
        //        [Size(70)]
        public string Name {
            get { return AbstractAnalitic.Name; }
            set {
                String old = AbstractAnalitic.Name;
                if (old == value) return;
                AbstractAnalitic.Name = value;
                OnChanged("Name", old, value);
            }
        }
        //        [RuleRequiredField]
        [PersistentAlias("AbstractAnalitic.DateBegin")]
        public DateTime DateBegin {
            get { return AbstractAnalitic.DateBegin; }
            set {
                DateTime old = AbstractAnalitic.DateBegin;
                if (old == value) return;
                AbstractAnalitic.DateBegin = value;
                OnChanged("DateBegin", old, value);
            }
        }
        [PersistentAlias("AbstractAnalitic.DateEnd")]
        public DateTime DateEnd {
            get { return AbstractAnalitic.DateEnd; }
            set {
                DateTime old = AbstractAnalitic.DateEnd;
                if (old == value) return;
                AbstractAnalitic.DateEnd = value;
                OnChanged("DateEnd", old, value);
            }
        }
        [PersistentAlias("AbstractAnalitic.IsClosed")]
        public Boolean IsClosed {
            get { return AbstractAnalitic.IsClosed; }
        }

        [Action]
        public void ReOpen() {
            AbstractAnalitic.ReOpen();
        }


//        public override System.ComponentModel.IBindingList Children {
//            get {
//                return Orders;
                //return new BindingList<object>(); 
//            }
//        }

//        public override ITreeNode Parent {
//            get { return Direction; }
//        }


        #endregion

    }

}