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

namespace IntecoAG.ERM.CS.Measurement
{
    /// <summary>
    /// Класс, отражающий сущность числового данного с единицей измерения
    /// </summary>
    //[Serializable]
    public struct Measurement
    {
        public Measurement(decimal count, csUnit unit) : this() {
            _Count = count;
            _Unit = unit;
        }


        #region ПОЛЯ

        #endregion


        #region СВОЙСТВА
        
        /// <summary>
        /// Дата начала события
        /// </summary>
        [Persistent("Count")]
        private decimal _Count;
        public decimal Count {
            get { return _Count; }
            set { _Count = value; }
        }

        /// <summary>
        /// Дата начала события
        /// </summary>
        private csUnit _Unit;
        [Persistent("Unit")]
        public csUnit Unit {
            get { return _Unit; }
            set { _Unit = value; }
        }
        
        #endregion
    }

}