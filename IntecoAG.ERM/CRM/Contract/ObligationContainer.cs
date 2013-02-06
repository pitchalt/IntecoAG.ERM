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

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс ObligationContainer, представляющий пакеты обязательств по Договора
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmObligationContainer")]
    public partial class ObligationContainer : Obligation
    {
        public ObligationContainer() : base() { }
        public ObligationContainer(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion

    }

}