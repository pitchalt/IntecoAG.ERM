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
    /// ����� Event, �������������� �������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csEvent")]
    [DefaultProperty("EventDateTime")]
    public partial class Event : BaseObject, IEvent
    {
        public Event() : base() { }
        public Event(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


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