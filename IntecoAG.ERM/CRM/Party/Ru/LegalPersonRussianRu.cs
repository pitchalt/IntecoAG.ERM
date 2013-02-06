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

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс RussianLegalPerson, представляющий (не ИЧП) предприятие как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[Persistent("crmPartyLegalPersonRussianRu")]
    public partial class crmLegalPersonRussianRu : crmLegalPerson
    {
        public crmLegalPersonRussianRu(Session ses) : base(ses) { }

    }

}