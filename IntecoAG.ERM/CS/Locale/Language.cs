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

namespace IntecoAG.ERM.CS.Language
{
    /// <summary>
    /// ����� csLanguage, �������������� ������� ��������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csLanguage")]
    public partial class csLanguage : BaseObject
    {
        public csLanguage(Session ses) : base(ses) { }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res = Description;
            return Res;
        }

        #endregion

    }

}