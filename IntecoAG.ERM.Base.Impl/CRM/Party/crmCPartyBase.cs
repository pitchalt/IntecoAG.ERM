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
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

using System.ComponentModel;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Party, представляющий участника как сторону во взаимоотношениях
    /// </summary>
    //[DefaultClassOptions]
//    [Persistent("crmParty")]
    [NonPersistent]
    public abstract partial class crmCPartyBase : csCComponent
    {
        public crmCPartyBase(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            this.PartyName = String.Empty;
//            this.Description = String.Empty;
        }


        #region ПОЛЯ КЛАССА
        #endregion


        #region СВОЙСТВА КЛАССА
        #endregion


        #region МЕТОДЫ

        #endregion

    }

}