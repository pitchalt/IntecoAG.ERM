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
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.Subject
{

    // См. также в классе fmAbstractSubject закомментаренный регион #region Разузлование дерева:  TopAbstractSubject - SubAbstractSubjects

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class fmAbstractSubject
    {
        [Association("fmAbstractSubject-fmAbstractSubjects"), Aggregated]
        public XPCollection<fmAbstractSubject> AbstractSubjects {
            get {
                return GetCollection<fmAbstractSubject>("AbstractSubjects");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class fmAbstractSubject
    {
        private fmAbstractSubject _AbstractSubject;
        [Association("fmAbstractSubject-fmAbstractSubjects")]
        public fmAbstractSubject AbstractSubject {
            get { return _AbstractSubject; }
            set { SetPropertyValue<fmAbstractSubject>("AbstractSubject", ref _AbstractSubject, value); }
        }
    }

}
