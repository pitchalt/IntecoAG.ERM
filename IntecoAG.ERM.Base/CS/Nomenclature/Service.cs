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
//
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.Trw;

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// Класс, отражающий сущность Услуги
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csService")]
    public class csService : csNomenclature
    {
        public csService(Session ses) : base(ses) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TrwMeasurementUnit = TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_WORK;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion

    }

}