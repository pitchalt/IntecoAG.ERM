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

using IntecoAG.ERM.FM.Subject;

namespace IntecoAG.ERM.FM.Order
{
    /// <summary>
    /// ����� Order
    /// </summary>
    //[DefaultClassOptions]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("fmOrder")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class fmOrder : fmAbstractSubject
    {
        public fmOrder(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region ���� ������
        private string _Description;
        private Subject.fmSubject _Subject;
        #endregion

        #region �������� ������
        /// <summary>
        /// Description - ��������
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        [Association("fmSubject-Orders")]
        [RuleRequiredField("fmOrder.RequiredSubject", "Save")]
        public Subject.fmSubject Subject {
            get { return _Subject; }
            set { 
                SetPropertyValue("Subject", ref _Subject, value);
                OnChanged("Category");
            }
        }

        #endregion

        public override System.ComponentModel.IBindingList Children {
            get { return new BindingList<object>(); }
        }

        public override ITreeNode Parent {
            get { return Subject; }
        }
    }

}