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

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// �����, ���������� �������� ������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csValuta")]
    public class Valuta : Financial
    {
        public Valuta() : base() { }
        public Valuta(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        #endregion

    }

}