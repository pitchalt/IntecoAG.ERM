using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{

    /// <summary>
    /// IMainObjectVersionSupport: ������� � ������ ������� ���������� ��������, ��� �� �������� �������� �������� ���
    /// ���������� ���������, ��������, ��� crmContract � � ���� ���� ���������� ��������� crmContractVersion. crmContract ����������
    /// ����������� IMainObjectVersionSupport ��� ������ ������������ ReadOnlyController � �.�.
    /// </summary>
    public interface IVersionMainObject {

        #region ����

        /// <summary>
        /// VersionRecord - ������ ������
        /// </summary>
        //VersionRecord Current { get; set; }

        #endregion

        #region ������ ��� ��������� �������� ����� ������ �������

        VersionRecord GetCurrent();

        #endregion
    }

}