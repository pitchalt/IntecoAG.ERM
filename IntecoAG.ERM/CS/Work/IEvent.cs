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

namespace IntecoAG.ERM.CS.Work
{
    /// <summary>
    /// ��������� Event, �������������� �������
    /// </summary>
    public interface IEvent
    {
        #region �������� ������

        /// <summary>
        /// EventDateTime - ����, �������� �������
        /// </summary>
        DateTime EventDateTime {
            get;
            set;
        }

        #endregion
    }

}