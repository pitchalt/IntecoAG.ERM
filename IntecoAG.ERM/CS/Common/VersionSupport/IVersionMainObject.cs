using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// IMainObjectVersionSupport: наличие у класса данного интерфейса означает, что он является головным объектом для
    /// версионной структуры, например, это crmContract и у него есть версионная структура crmContractVersion. crmContract помечается
    /// интерфейсом IMainObjectVersionSupport для работы контроллеров ReadOnlyController и т.д.
    /// </summary>
    public interface IVersionMainObject {

        #region ПОЛЯ

        /// <summary>
        /// VersionRecord - версия записи
        /// </summary>
        //VersionRecord Current { get; set; }

        #endregion

        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ СОЗДАНИЯ НОВОЙ ВЕРСИИ ОБЪЕКТА

        VersionRecord GetCurrent();

        #endregion
    }

}