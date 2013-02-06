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

        #region ���� ������ ��� ��������� �������� ����� ������ �������

        /// <summary>
        /// VersionState - ������ ������
        /// </summary>
        //VersionStates VersionState { get; set; }

        #endregion

        #region ������ ��� ��������� �������� ����� ������ �������

        IVersionSupport CreateNewVersion();

        void Approve(IVersionSupport obj);

        #endregion
    }

}