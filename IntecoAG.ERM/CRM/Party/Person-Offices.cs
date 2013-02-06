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

namespace IntecoAG.ERM.CRM.Party
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class Person
    {
        [Association("crmPerson-Offices"), Aggregated]
        public XPCollection<Office> _Offices {
            get {
                return GetCollection<Office>("_Offices");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Office
    {
        private Person _Person;
        [Association("crmPerson-Offices")]
        public Person Person {
            get { return _Person; }
            set { SetPropertyValue("Person", ref _Person, value); }
        }
    }

}
