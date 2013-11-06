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
    /// Тип числового данного с единицей измерения
    /// </summary>
    public struct csMeasurement
    {
        public csMeasurement(Decimal count, csUnit unit) {
            _Count = count;
            _Unit = unit;
        }


        #region ПОЛЯ
        private csUnit _Unit;
        private Decimal _Count;

        #endregion


        #region СВОЙСТВА
        
        /// <summary>
        /// Кол-во
        /// </summary>
        public Decimal Count {
            get { return _Count; }
            set { _Count = value; }
        }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public csUnit Unit {
            get { return _Unit; }
            set { _Unit = value; }
        }
        
        #endregion
    }

}