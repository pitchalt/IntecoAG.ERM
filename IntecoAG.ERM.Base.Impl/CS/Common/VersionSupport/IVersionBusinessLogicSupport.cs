using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// IVersionBusinessLogicSupport
    /// </summary>
    public interface IVersionBusinessLogicSupport {

        #region ПОЛЯ КЛАССА ДЛЯ ПОДДЕРЖКИ СОЗДАНИЯ НОВОЙ ВЕРСИИ ОБЪЕКТА

        /// <summary>
        /// VersionState - версия записи
        /// </summary>
        //VersionStates VersionState { get; set; }

        #endregion

        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ СОЗДАНИЯ НОВОЙ ВЕРСИИ ОБЪЕКТА

        IVersionSupport CreateNewVersion();

        void Approve(IVersionSupport obj);

        #endregion
    }

}