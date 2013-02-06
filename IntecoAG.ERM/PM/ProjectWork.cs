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

using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.PM
{
    /// <summary>
    /// Класс ProjectWork, представляющий стороны Договора
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("pmProjectWork")]
    public partial class ProjectWork : CS.Work.csWork
    {
        public ProjectWork(Session session) : base(session) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        #endregion

    }

}