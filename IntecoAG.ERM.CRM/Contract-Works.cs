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
        [Association("crmContract-Works"), Aggregated]
        public XPCollection<Work> _Works {
            get {
                return GetCollection<Work>("_Works");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Work
    {
        private Contract _Contract;
        [Association("crmContract-Works")]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue("Contract", ref _Contract, value); }
        }
    }

}
