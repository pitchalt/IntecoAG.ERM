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
using System.ComponentModel;

using DevExpress.Xpo;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// ����� crmBaseRegister
    /// </summary>
    [NonPersistent]
    public class crmBaseRegister : DevExpress.Xpo.XPLiteObject
    {
        public crmBaseRegister(Session ses) : base(ses) { }

        #region ���� ������

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int Oid;

        private Guid _Token;
        /// <summary>
        /// Token
        /// ������������� ������ ��������� �������, �������������� ������������, ���� ������������� �����
        /// </summary>
        public Guid Token {
            get { return _Token; }
            set { SetPropertyValue<Guid>("Token", ref _Token, value); }
        }

        #endregion


        #region �������� ������

        #endregion


        #region ������

        #endregion

    }
}