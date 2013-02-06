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

namespace IntecoAG.ERM.FM.Subject
{
    /// <summary>
    /// Класс Subject
    /// </summary>
    [DefaultClassOptions]
    [Persistent("fmSubjectWork")]
    public partial class SubjectWork : fmAbstractSubject
    {
        public SubjectWork() : base() { }
        public SubjectWork(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        #endregion

    }

}