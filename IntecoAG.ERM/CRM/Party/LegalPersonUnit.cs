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

namespace IntecoAG.ERM.CRM.Party {
    /// <summary>
    /// ����� Person, �������������� ��������� (��� �������) ��������
    /// </summary>
//    [DefaultClassOptions]
    [Persistent("crmPartyLegalPersonUnit")]
    public partial class crmLegalPersonUnit : BaseObject 
    {
        public crmLegalPersonUnit(Session ses) : base(ses) { }

        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// KPP
        /// </summary>
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        /// <summary>
        /// KPP
        /// </summary>
        private string _KPP;
        public string KPP {
            get { return _KPP; }
            set { SetPropertyValue("KPP", ref _KPP, value); }
        }

        #endregion


        #region ������

        /// <summary>
        /// ����������� ����� (XAF ��� ����������, ����� �������� ������ � ����������)
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name;
        }

        #endregion

    }

}