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
    public partial class Side
    {
        [Association("crmCreditor-Obligations")]
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
        private Side _Creditor;
        [Association("crmCreditor-Obligations")]
        public Side Creditor {
            get { return _Creditor; }
            set { SetPropertyValue("Creditor", ref _Creditor, value); }
        }
    }

}
