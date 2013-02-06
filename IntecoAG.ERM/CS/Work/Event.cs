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
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

namespace IntecoAG.ERM.CS.Work
{
    /// <summary>
    /// ����� csEvent, �������������� �������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csEvent")]
    [DefaultProperty("EventDateTime")]
    public partial class csEvent : BaseObject, IEvent
    {
        public csEvent(Session ses) : base(ses) { }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// EventDateTime - ����, �������� �������
        /// </summary>
        private DateTime _EventDateTime;
        public DateTime EventDateTime {
            get { return _EventDateTime; }
            set { if (_EventDateTime != value) SetPropertyValue("EventDateTime", ref _EventDateTime, value); }
        }

        #endregion

    }

}