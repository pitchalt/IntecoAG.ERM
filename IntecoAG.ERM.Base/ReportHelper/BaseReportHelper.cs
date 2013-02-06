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
using System.Data;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

namespace IntecoAG.ERM.Module.ReportHelper
{

    /// <summary>
    /// Класс BaseReportHelper базовый класс для отчётов
    /// </summary>
    //[NonPersistent]
    [Persistent("BaseReportHelper")]
    //[NonPersistent]
    public class BaseReportHelper : BaseObject
    {
        public BaseReportHelper(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion


        #region МЕТОДЫ

        // Формирование набора данных с таблицей для отчёта
        public virtual List<BaseReportHelper> CreateReportListSource(Session ssn, CriteriaOperator criteria) {
            return null;
        }

       #endregion
    }
}