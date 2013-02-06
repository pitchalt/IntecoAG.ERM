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
using System.ComponentModel;

using DevExpress.Xpo;


namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmObligationUnitMain
    {
        [Association("crmObligationUnitMain-ObligationUnits"), Aggregated]
        [Browsable(false)]
        public XPCollection<crmObligationUnit> ObligationUnits {
            get {
                return GetCollection<crmObligationUnit>("ObligationUnits");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmObligationUnit
    {
        private crmObligationUnitMain _ObligationUnitMain;
        [Association("crmObligationUnitMain-ObligationUnits")]
        [Browsable(false)]
        public crmObligationUnitMain ObligationUnitMain {
            get { return _ObligationUnitMain; }
            set { SetPropertyValue<crmObligationUnitMain>("ObligationUnitMain", ref _ObligationUnitMain, value); }
        }
    }

}
