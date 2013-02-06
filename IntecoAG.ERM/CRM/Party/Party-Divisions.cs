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
using DevExpress.Xpo;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;

namespace IntecoAG.ERM.CRM.Party
{
    // Party-Divisions.cs
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Party
    {
        [Association("crmParty-Divisions"), Aggregated]
        public XPCollection<LegalPersonUnit> _Divisions {
            get {
                return GetCollection<LegalPersonUnit>("_Divisions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class LegalPersonUnit
    {
        private Party _Party;
        [Association("crmParty-Divisions")]
        public Party Party {
            get { return _Party; }
            set { SetPropertyValue<Party>("Party", ref _Party, value); }
        }
    }

}
