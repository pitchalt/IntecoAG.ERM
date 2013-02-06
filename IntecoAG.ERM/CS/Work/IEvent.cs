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
    /// Интерфейс Event, представляющий Событие
    /// </summary>
    public interface IEvent
    {
        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// EventDateTime - дата, присущая событию
        /// </summary>
        DateTime EventDateTime {
            get;
            set;
        }

        #endregion
    }

}