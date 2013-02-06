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
    /// ����� Side, �������������� ������� ��������
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmSide")]
    public partial class Side : BaseObject
    {
        public Side() : base() { }
        public Side(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Party, �������������� �������
        /// </summary>
        private Party.crmParty _Party;
        public Party.crmParty Party {
            get { return _Party; }
            set { if (_Party != value) SetPropertyValue("Party", ref _Party, value); }
        }

        /// <summary>
        /// ������������� (Debitor)
        /// </summary>
        private CRM.Contract.ObligationTransfer _Debitor;
        public CRM.Contract.ObligationTransfer Debitor {
            get { return _Debitor; }
            set { if (_Debitor != value) SetPropertyValue("Debitor", ref _Debitor, value); }
        }

        #endregion

    }

}