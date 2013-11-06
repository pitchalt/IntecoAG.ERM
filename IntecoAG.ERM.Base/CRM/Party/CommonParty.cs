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
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Party, представляющий участника (как сторону) Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmCommonParty")]
    public partial class CommonParty : Party
    {
        public CommonParty() : base() { }
        public CommonParty(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА


        /// <summary>
        /// Person - описание
        /// </summary>
        private Person _Person;
        public Person Person {
            get { return _Person; }
            set { SetPropertyValue<Person>("Person", ref _Person, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}