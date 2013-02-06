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
    public partial class ObligationContainer
    {
        [Association("crmObligationContainer-Obligations"), Aggregated]
        public XPCollection<Obligation> Obligations {
            get {
                return GetCollection<Obligation>("Obligations");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Obligation
    {
        private ObligationContainer _ObligationContainer;
        [Association("crmObligationContainer-Obligations")]
        public ObligationContainer ObligationContainer {
            get { return _ObligationContainer; }
            set { SetPropertyValue("ObligationContainer", ref _ObligationContainer, value); }
        }
    }

}
