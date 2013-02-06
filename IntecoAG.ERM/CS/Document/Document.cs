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

namespace IntecoAG.ERM.CS.Document
{
    /// <summary>
    /// ����� csDocument, �������������� ������� ��������
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("csDocument")]
    [DefaultProperty("Number")]
    public partial class csDocument : BaseObject
    {
        public csDocument(Session ses) : base(ses) { }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Number - ����� ���������
        /// </summary>
        private string _Number;
        public string Number {
            get { return _Number; }
            set { if (_Number != value) SetPropertyValue("Number", ref _Number, value); }
        }

        /// <summary>
        /// Date - ���� ���������
        /// </summary>
        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { if (_Date != value) SetPropertyValue("Date", ref _Date, value); }
        }

        /// <summary>
        /// RegNumber - ����� ���������
        /// </summary>
        private string _RegNumber;
        public string RegNumber {
            get { return _RegNumber; }
            set { if (_RegNumber != value) SetPropertyValue("RegNumber", ref _RegNumber, value); }
        }

        /// <summary>
        /// RegDate - ���� ���������
        /// </summary>
        private DateTime _RegDate;
        public DateTime RegDate {
            get { return _RegDate; }
            set { if (_RegDate != value) SetPropertyValue("RegDate", ref _RegDate, value); }
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
            Res = Number + " " + Date.ToString() + " " + RegNumber + " " + RegDate.ToString();
            return Res;
        }

        #endregion

    }

}