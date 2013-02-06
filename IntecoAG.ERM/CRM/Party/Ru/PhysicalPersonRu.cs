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

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// ����� RussianIndividualPerson, �������������� �������������� ������� ����������� ��� ��������� (�������) ��������
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
//    [Persistent("crmPartyPhysicalPersonRu")]
    public partial class crmPhysicalPersonRu : crmPhysicalPerson
    {
        public crmPhysicalPersonRu(Session ses) : base(ses) { }

    }

}