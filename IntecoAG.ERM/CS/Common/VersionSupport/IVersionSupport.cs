using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// IVersionSupport
    /// </summary>
    public partial interface IVersionSupport
    {
        #region ПОЛЯ КЛАССА ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        /// <summary>
        /// PrevVersion - ссылка на предыдущую версию (null в случае отсутствия таковой)
        /// </summary>
        IVersionSupport PrevVersion { get; set; }

        Boolean IsProcessCloning { get; set; }

        /// <summary>
        /// VersionState - версия записи
        /// </summary>
        VersionStates VersionState { get; set; }

        /// <summary>
        /// IsCurrent - признак текущей записи (не то же самое, что CURRENT или VERSION_CURRENT)
        /// </summary>
        bool IsCurrent { get; set; }

        /// <summary>
        /// IsOfficial - признак официально принятой версии записи
        /// </summary>
        bool IsOfficial { get; set; }

        ///// <summary>
        ///// Current - Ссылка на версию с признаком VersionState = CURRENT
        ///// </summary>
        //IVersionSupport Current { get; set; }

        /// <summary>
        /// LinkToEditor - Ссылка на редактор, которым в последний раз редактировалась данная семантическая структура
        /// </summary>
        IVersionSupport LinkToEditor { get; set; }

        #endregion

        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        List<IVersionSupport> GetVersionedStrate(IVersionSupport sourceObj, VersionHelper vHelper);

        List<IVersionSupport> GetFirstDependentList(IVersionSupport sourceObj, List<IVersionSupport> dependentObjectList, VersionHelper vHelper);

        Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> list, Session ssn, IVersionSupport sourceObj);

        IVersionSupport CreateNewVersion(IVersionSupport sourceObj, VersionHelper vHelper);

        void SetVersionState(Dictionary<IVersionSupport, IVersionSupport> dict, VersionStates versionState);

        void SetVersionStateExt(IVersionSupport obj, VersionStates vs);

        void AddObjectToList(IVersionSupport obj, List<IVersionSupport> dependentObjectList);

        void VersionAfterConstruction();

        #endregion
    }

}