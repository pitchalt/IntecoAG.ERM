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
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// ����� LegalPerson, �������������� ����������� ���� ��� ��������� (�������) ��������
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmSupplier")]
    public partial class crmSupplier : BaseObject
    {
        public crmSupplier(Session ses) : base(ses) { }


        #region ���� ������

        #endregion


        #region �������� ������

        #endregion


        #region ������

        #endregion

    }

}