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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс Side, представляющий стороны Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmSide")]
    public partial class Side : BaseObject
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
        private Party.crmParty _Party;
        public Party.crmParty Party {
            get { return _Party; }
            set { if (_Party != value) SetPropertyValue("Party", ref _Party, value); }
        }

        /// <summary>
        /// Обязательства (Debitor)
        /// </summary>
        private CRM.Contract.ObligationTransfer _Debitor;
        public CRM.Contract.ObligationTransfer Debitor {
            get { return _Debitor; }
            set { if (_Debitor != value) SetPropertyValue("Debitor", ref _Debitor, value); }
        }

        #endregion

    }

}