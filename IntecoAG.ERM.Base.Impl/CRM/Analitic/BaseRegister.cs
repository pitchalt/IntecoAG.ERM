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
using System.ComponentModel;

using DevExpress.Xpo;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Класс crmBaseRegister
    /// </summary>
    [NonPersistent]
    public abstract class crmBaseRegister : DevExpress.Xpo.XPLiteObject
    {
        public crmBaseRegister(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            DateRecord = DateTime.Now;
        }

        #region ПОЛЯ КЛАССА

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int Oid;

        private Guid _Token;
        /// <summary>
        /// Token
        /// Идентификатор группы связанных записей, сосуществующих одновременно, либо несущесвующих вовсе
        /// </summary>
        public Guid Token {
            get { return _Token; }
            set { SetPropertyValue<Guid>("Token", ref _Token, value); }
        }

        private DateTime _DateRecord;   // Время создания текущей записи регистра
        /// <summary>
        /// Время создания текущей записи регистра
        /// </summary>
        public DateTime DateRecord {
            get {
                return _DateRecord;
            }
            set {
                SetPropertyValue<DateTime>("DateRecord", ref _DateRecord, value);
            }
        }

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}