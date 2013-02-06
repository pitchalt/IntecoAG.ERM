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

namespace IntecoAG.ERM.CRM.Contract.Document
{
    /// <summary>
    /// ����� crmDocContract, �������������� ������ ��������
    /// </summary>
    public interface IDocContract
    {

        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Description - ��������
        /// </summary>
        string Description {
            get;
            set;
        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        string ToString();

        #endregion

    }

}