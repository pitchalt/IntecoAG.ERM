using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// INewVersionSupport
    /// </summary>
    public interface INewVersionSupport {

        #region ���� ������ ��� ��������� �������� ����� ������ �������

        /// <summary>
        /// VersionState - ������ ������
        /// </summary>
        //VersionStates VersionState { get; set; }

        #endregion

        #region ������ ��� ��������� �������� ����� ������ �������

        IVersionSupport CreateNewVersion();

        #endregion
    }

}