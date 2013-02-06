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
        #region ���� ������ ��� ��������� ������������

        /// <summary>
        /// PrevVersion - ������ �� ���������� ������ (null � ������ ���������� �������)
        /// </summary>
        IVersionSupport PrevVersion { get; set; }

        Boolean IsProcessCloning { get; set; }

        /// <summary>
        /// VersionState - ������ ������
        /// </summary>
        VersionStates VersionState { get; set; }

        /// <summary>
        /// IsCurrent - ������� ������� ������ (�� �� �� �����, ��� CURRENT ��� VERSION_CURRENT)
        /// </summary>
        bool IsCurrent { get; set; }

        /// <summary>
        /// IsOfficial - ������� ���������� �������� ������ ������
        /// </summary>
        bool IsOfficial { get; set; }

        ///// <summary>
        ///// Current - ������ �� ������ � ��������� VersionState = CURRENT
        ///// </summary>
        //IVersionSupport Current { get; set; }

        /// <summary>
        /// LinkToEditor - ������ �� ��������, ������� � ��������� ��� ��������������� ������ ������������� ���������
        /// </summary>
        IVersionSupport LinkToEditor { get; set; }

        #endregion

        #region ������ ��� ��������� ������������

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