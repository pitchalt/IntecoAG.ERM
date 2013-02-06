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

namespace IntecoAG.ERM.Module.ReportHelper
{

    /// <summary>
    /// Класс BaseReportFilter базовый класс для фильтров отчётов
    /// В его реализация должны присутствовать параметры фильтрации и метод получения критерия фильтрации
    /// из этих параметров.
    /// Также на DetailView конкретных реализаций этого класса должна быть кнопка, запускающая метод создания критерия
    /// отбора. Результат работы этого метода используется контроллером ShowReportController для вызова отчёта,
    /// формирования коллекции для DataSource и подключения отчёта к этому источнику данных
    /// </summary>
    [NonPersistent]
    public class BaseReportFilter : BaseObject
    {
        public BaseReportFilter(Session ses) : base(ses) { }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Зашиваем значение типа объектов для отчёта
        //    GetReportDataSourceType();
        //}

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //protected Type _ReportDataSourceType;
        //public Type ReportDataSourceType {
        //    get { return _ReportDataSourceType; }
        //    set { SetPropertyValue<Type>("ReportDataSourceType", ref _ReportDataSourceType, value); }
        //}

        #endregion


        #region МЕТОДЫ

        // Формирование критерия фильтрации
        public virtual CriteriaOperator GetCriteria() {
            return null;
        }

        // Очистка критерия фильтрации
        public virtual void ClearCriteria() {
        }

        // Тип объекта для отчёта
        public virtual Type GetReportDataSourceType() {
            //return typeof(object);
            return null;
        }

        // Название файла отчёта без расширения (которое всегда .repx)
        public virtual string GetReportFileName() {
            return null;
        }

       #endregion
    }
}