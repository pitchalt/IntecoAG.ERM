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

using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.PM
{
    /// <summary>
    /// ����� pmProjectWork, �������������� ������� ��������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("pmProjectWork")]
    public partial class pmProjectWork : CS.Work.csWork
    {
        public pmProjectWork(Session session) : base(session) { }


        #region ���� ������

        #endregion


        #region �������� ������
        #endregion

    }

}