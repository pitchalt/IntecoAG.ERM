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
using DevExpress.Xpo;

// === IntecoAG namespaces ===
//using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CRM
{
    /// <summary>
    /// Класс Side, представляющий стороны Договора
    /// </summary>
    [Persistent("crmSide")]
    public partial class Side : XPObject
    {
        public Side() : base() { }
        public Side(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Party, представляющий сторону
        /// </summary>
        private Party _Party;
        public Party Party {
            get { return _Party; }
            set { if (_Party != value) SetPropertyValue("Party", ref _Party, value); }
        }

        #endregion

    }

}