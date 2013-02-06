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

// === IntecoAG namespaces ===
//using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CRM
{
    /// <summary>
    /// ����� Side, �������������� ������� ��������
    /// </summary>
    [Persistent("crmSide")]
    public partial class Side : XPObject
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
        private Party _Party;
        public Party Party {
            get { return _Party; }
            set { if (_Party != value) SetPropertyValue("Party", ref _Party, value); }
        }

        #endregion

    }

}