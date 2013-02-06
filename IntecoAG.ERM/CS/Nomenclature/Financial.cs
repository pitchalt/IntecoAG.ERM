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

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// Класс, отражающий сущность Финансовых типов (например, валюты)
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csFinancial")]
    public class csFinancial : csNomenclature
    {
        public csFinancial(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion

    }

}