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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.GFM;
using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.Order
{
    /// <summary>
    /// Класс Order
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("fmOrder")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class fmOrder : BaseObject
    {
        public fmOrder(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА
        private string _Description;
        private Subject.SubjectClass _Subject;
        #endregion

        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        [Association("fmSubject-Orders")]
        [RuleRequiredField]
        public Subject.SubjectClass Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue("Subject", ref _Subject, value);
                OnChanged("Category");
            }
        }

        #endregion

        //public override System.ComponentModel.IBindingList Children {
        //    get { return new BindingList<object>(); }
        //}

        //public override ITreeNode Parent {
        //    get { return Subject; }
        //}
    }

}