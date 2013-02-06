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

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Contract
    {
        [Association("crmContract-crmEvents"), Aggregated]
        public XPCollection<crmEvent> crmEvents {
            get {
                return GetCollection<crmEvent>("crmEvents");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmEvent
    {
        private Contract _Contract;
        [Association("crmContract-crmEvents")]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue("Contract", ref _Contract, value); }
        }
    }

}
