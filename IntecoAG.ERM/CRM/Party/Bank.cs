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
    /// Класс LegalPerson, представляющий юридическое лицо как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [Persistent("crmBank")]
    public partial class crmBank : BaseObject
    {
        public crmBank(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmPartyRu _Party;
        public crmPartyRu Party {
            get { return _Party; }
            set { SetPropertyValue<crmPartyRu>("Party", ref _Party, value); }
        }
        //
        private csCountry _Country;
        public csCountry Country {
            get { return _Country; }
            set { SetPropertyValue<csCountry>("Country", ref _Country, value); }
        }
        //
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value != null ? value.Trim() : null); }
        }
        private string _BIC;
        public string BIC {
            get { return _BIC; }
            set { SetPropertyValue("BIC", ref _BIC, value != null ? value.Trim(): null); }
        }
        private string _RCBIC;
        public string RCBIC {
            get { return _RCBIC; }
            set { SetPropertyValue("RCBIC", ref _RCBIC, value != null ? value.Trim() : null); }
        }
        private string _KorAcc;
        public string KorAcc {
            get { return _KorAcc; }
            set { SetPropertyValue("KorAcc", ref _KorAcc, value != null ? value.Trim() : null); }
        }

        public string FullName {
            get {
                return Name;
            }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}