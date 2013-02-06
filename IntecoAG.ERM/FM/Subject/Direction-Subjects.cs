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

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class fmDirection
    {
        [Association("fmDirection-Subjects"), Aggregated]
        public XPCollection<fmSubject> Subjects {
            get {
                return GetCollection<fmSubject>("Subjects");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class fmSubject
    {
        private fmDirection _Direction;
        [Association("fmDirection-Subjects")]
        public fmDirection Direction {
            get { return _Direction; }
            set { SetPropertyValue("Direction", ref _Direction, value); }
        }
    }

}
