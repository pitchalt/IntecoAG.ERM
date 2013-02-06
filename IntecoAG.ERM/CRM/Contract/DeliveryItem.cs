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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;

using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс DeliveryItem, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    //[Persistent("crmDeliveryItem")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract partial class crmDeliveryItem : crmObligationTransfer
    {
        public crmDeliveryItem(Session session) : base(session) { }
        public crmDeliveryItem(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Date
        /// <summary>
        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }
        /// <summary>
        /// CountCode
        /// </summary>
        private decimal _CountValue;
        public decimal CountValue {
            get { return _CountValue; }
            set { SetPropertyValue("CountValue", ref _CountValue, value); }
        }

        /// <summary>
        /// CountUnit
        /// </summary>
        private CS.Measurement.csUnit _CountUnit;
        public CS.Measurement.csUnit CountUnit {
            get { return _CountUnit; }
            set { SetPropertyValue("CountUnit", ref _CountUnit, value); }
        }

        #endregion


        #region МЕТОДЫ

        public override void UpdateCost(crmCost sp, Boolean mode) {
            if (DeliveryUnit != null)
                DeliveryUnit.UpdateCost(sp, mode);
        }
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}