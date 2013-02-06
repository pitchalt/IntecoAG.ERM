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

using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultProperty("Name")]
    [Persistent("crmBankAccount")]
    public partial class crmBankAccount : BaseObject
    {
        public crmBankAccount(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        private crmBank _Bank;
        public crmBank Bank {
            get { return _Bank; }
            set { SetPropertyValue<crmBank>("Bank", ref _Bank, value); }
        }

        private crmPerson _Person;
        [Association("crmPerson-crmBankAccount")]
        public crmPerson Person {
            get { return _Person; }
            set { SetPropertyValue<crmPerson>("Person", ref _Person, value); }
        }
        
        private string _Number;
        [Size(30)]
        public string Number {
            get { return _Number; }
            set { SetPropertyValue("Number", ref _Number, (value == null) ?  null  : value.Trim()); }
        }
        [Browsable(false)]
        public string Name {
            get {
                if (Bank != null)
                    return string.Concat(Number, " ", Bank.FullName);
                else
                    return this.Number;
            }
        }

        #endregion


        #region Ã≈“Œƒ€

        #endregion

    }

}