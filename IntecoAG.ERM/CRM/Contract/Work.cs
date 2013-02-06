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


//*****************************************************************************************************//
// �� ������������.
//*****************************************************************************************************//
// Stage � ���� ����� 
// - DateBegin, DateEnd � �������� �������� ���� ������ � ��������� ����� ����������� 
// CS.CRM.Contract..Time ���� ������� ���������� �� DateStart � DateStop, ������� ������ 
// ���������� ���� ��� �������� ������ ������������� ����������� ����� ���� DateBegin � DateEnd
//
// ������ Stage ������ ��� Event (������ � ���������� �����) ��� Contract. ��� ���� �������������� 
// �������������
//*****************************************************************************************************//


namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� ��� ��������� ��������� ����� � ������ �������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmWork")]
    public partial class crmWork : Stage
    {
        public crmWork() : base() { }
        public crmWork(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {

            // �������� ������� ��� ���������
            _EventBegin = new crmEvent(this.Session);
            _EventBegin.Contract = this.Contract;

            _EventEnd = new crmEvent(this.Session);
            _EventEnd.Contract = this.Contract;
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// ������� ������ ������
        /// </summary>
        private CRM.Contract.crmEvent _EventBegin;
        public CRM.Contract.crmEvent EventBegin {
            get { return _EventBegin; }
            set {
                if (_EventBegin != value) {
                    SetPropertyValue("EventBegin", ref _EventBegin, value);
                }
            }
        }

        /// <summary>
        /// ������� ����� ������
        /// </summary>
        private CRM.Contract.crmEvent _EventEnd;
        public CRM.Contract.crmEvent EventEnd {
            get { return _EventEnd; }
            set { if (_EventEnd != value) SetPropertyValue("EventEnd", ref _EventEnd, value); }
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
            Res = base.ToString() + ", ������: " + this.EventBegin.ToString() + " - " + this.EventEnd.ToString();
            return Res;
        }

        #endregion

    }

}