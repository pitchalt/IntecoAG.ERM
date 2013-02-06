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

namespace IntecoAG.ERM.CRM.Party.Ru
{
    /// <summary>
    /// ����� Office, �������������� ��������� (��� �������) ��������
    /// </summary>
    [DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyLegalPersonForeignRu")]
    public class crmLegalPersonForeignRu : crmLegalPerson
    {
        public crmLegalPersonForeignRu(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// ������������ �������
        /// </summary>
        private string _KPP;
        public string KPP {
            get { return _KPP; }
            set { if (_KPP != value) SetPropertyValue("KPP", ref _KPP, value); }
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
            Res = base.ToString() + ", ��� " + this.KPP;
            return Res;
        }

        #endregion

    }

}