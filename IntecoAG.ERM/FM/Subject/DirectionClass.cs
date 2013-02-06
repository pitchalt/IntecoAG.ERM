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
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

using IntecoAG.ERM.GFM;

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    ///  Î‡ÒÒ Direction
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("fmDirection")]
    [FriendlyKeyProperty("Code")]
    [RuleCriteria(null, DefaultContexts.Save, "1==1")]
    public partial class DirectionClass : BaseObject, IDirection
    {
        public DirectionClass(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.AbstractAnalitic = new AbstractAnaliticClass(this.Session);
        }

        #region Generic

        [Browsable(false)]
        [Aggregated]
        public AbstractAnaliticClass AbstractAnalitic;

        #endregion

        #region œŒÀﬂ  À¿——¿

        #endregion

        #region Association

        [Browsable(false)]
        [Association("fmDirection-Subjects", typeof(SubjectClass))]
        public XPCollection<SubjectClass> SubjectClasss {
            get {
                return GetCollection<SubjectClass>("SubjectClasss");
            }
        }

        [Aggregated]
        public IList<ISubject> Subjects {
            get {
                return new ListConverter<ISubject, SubjectClass>(this.SubjectClasss);
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
//            get { return Subjects; }
//        }

//        public override ITreeNode Parent {
//            get { return null; }
//        }

        #endregion

    }

}