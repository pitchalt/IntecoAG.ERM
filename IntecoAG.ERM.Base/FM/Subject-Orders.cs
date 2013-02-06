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

// === IntecoAG namespaces ===
//using IntecoAG.ERM.FM;
//using IntecoAG.ERM.FM;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.FM.Subject
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Subject
    {
        [Association("crmSubject-Orders"), Aggregated]
        public XPCollection<Order.Order> _Orders {
            get {
                return GetCollection<Order.Order>("_Orders");
            }
        }
    }

}

namespace IntecoAG.ERM.FM.Order
{
    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Order
    {
        private Subject.Subject _Subject;
        [Association("crmSubject-Orders")]
        public Subject.Subject Subject {
            get { return _Subject; }
            set { SetPropertyValue("Subject", ref _Subject, value); }
        }
    }

}
