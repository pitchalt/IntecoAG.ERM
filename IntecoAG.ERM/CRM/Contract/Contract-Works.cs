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
        [Association("crmContract-crmWorks"), Aggregated]
        public XPCollection<crmWork> crmWorks {
            get {
                return GetCollection<crmWork>("crmWorks");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmWork
    {
        private Contract _Contract;
        [Association("crmContract-crmWorks")]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue("Contract", ref _Contract, value); }
        }
    }

}
