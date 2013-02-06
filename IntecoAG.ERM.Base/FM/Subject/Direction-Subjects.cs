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
    public partial class Direction
    {
        [Association("crmDirection-Subjects"), Aggregated]
        public XPCollection<Subject> _Subjects {
            get {
                return GetCollection<Subject>("_Subjects");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Subject
    {
        private Direction _Direction;
        [Association("crmDirection-Subjects")]
        public Direction Direction {
            get { return _Direction; }
            set { SetPropertyValue("Direction", ref _Direction, value); }
        }
    }

}
