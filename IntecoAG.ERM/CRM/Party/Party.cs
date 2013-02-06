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
    public abstract partial class crmCParty : csCComponent
    {
        public crmCParty(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            this.PartyName = String.Empty;
//            this.Description = String.Empty;
        }


        #region ПОЛЯ КЛАССА
        private Boolean _IsClosed;
        private string _Code;
        private string _Name;
        private string _Description;
        private string _NameFull;
        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// NameFull
        /// </summary>
        [Size(7)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        /// <summary>
        /// NameFull
        /// </summary>
        [Size(100)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String Description {
            get { return _Description; }
            set { SetPropertyValue<String>("Description", ref _Description, value); }
        }

        /// <summary>
        /// NameFull
        /// </summary>
        [Size(250)]
        public String NameFull {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}