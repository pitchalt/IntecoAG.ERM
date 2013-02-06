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

namespace IntecoAG.ERM.CRM
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Contract
    {
        [Association("crmContract-Sides"), Aggregated]
        public XPCollection<Side> _Sides {
            get {
                return GetCollection<Side>("_Sides");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Side
    {
        private Contract _Contract;
        [Association("crmContract-Sides")]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue("Contract", ref _Contract, value); }
        }
    }

}
