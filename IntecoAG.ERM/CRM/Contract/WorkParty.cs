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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� ContractParty, �������������� ���������� ��������
    /// </summary>
    //[DefaultClassOptions]
    //[Persistent("crmCommonParty")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class WorkParty : ContractParty
    {
        public WorkParty(Session session) : base(session) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        #endregion


        #region ������

        public override string ToString()
        {
            string Res = "";
//            Res = Person.ToString();
            return Res;
        }

        public void CopyPartyToContract() {
        }


        #endregion

    }

}