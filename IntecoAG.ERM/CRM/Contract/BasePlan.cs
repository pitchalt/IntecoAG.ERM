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

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� BasePlan, �������������� ���� ����� �� ��������
    /// </summary>
    public abstract partial class BasePlan : BaseObject
    {
        public BasePlan() : base() { }
        public BasePlan(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Number
        /// </summary>
        [Size(30)]
        private string _Number;
        public string Number {
            get { return _Number; }
            set { SetPropertyValue("Number", ref _Number, value); }
        }

        /// <summary>
        /// Date
        /// </summary>
        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }

        /// <summary>
        /// EventDate
        /// </summary>
        private DateTime _EventDate;
        public DateTime EventDate {
            get { return _EventDate; }
            set { SetPropertyValue<DateTime>("EventDate", ref _EventDate, value); }
        }

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        #endregion


        #region ������

        ///// <summary>
        ///// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}