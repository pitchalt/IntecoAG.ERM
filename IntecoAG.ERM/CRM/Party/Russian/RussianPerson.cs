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
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party.Russian
{
    /// <summary>
    /// ����� Person, �������������� ��������� (��� �������) ��������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmRussianPerson")]
    public class RussianPerson : Party
    {
        public RussianPerson() : base() { }
        public RussianPerson(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// ������������ �������
        /// </summary>
        private string _INN;
        public string INN {
            get { return _INN; }
            set { if (_INN != value) SetPropertyValue("INN", ref _INN, value); }
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
            Res = base.ToString() + ", ��� " + this.INN;
            return Res;
        }

        #endregion

    }

}