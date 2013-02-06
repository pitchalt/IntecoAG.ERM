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
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

using IntecoAG.ERM.CS.Country;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Party, представляющий участника как сторону во взаимоотношениях
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [Persistent("crmUserParty")]
    public partial class crmUserParty : BaseObject
    {
        public crmUserParty(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        public static IValueManager<crmUserParty> CurrentUserParty;
        public static crmUserParty CurrentUserPartyGet(Session ses) {
            if (CurrentUserParty == null) return null;
            if (CurrentUserParty.Value == null) return null;
            return ses.GetObjectByKey<crmUserParty>(CurrentUserParty.Value.Oid);
        }
        public static void CurrentUserPartySet(crmUserParty party) {
            CurrentUserParty.Value = party;
        }
        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// 
        /// </summary>
        private crmPartyRu _Party;
        [RuleRequiredField("crmUserParty.Party.Required", "Save")]
        public crmPartyRu Party {
            get { return _Party; }
            set { 
                SetPropertyValue<crmPartyRu>("Party", ref _Party, value);
                if (!IsLoading) {
                    OnChanged("PartyView");
                    OnChanged("Name");
                }
            }
        }

        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmPartyRu PartyView {
            get { return Party; }
        }

        public String Name {
            get { return Party == null ? String.Empty : Party.Name; }
        }
        #endregion


        #region МЕТОДЫ

        #endregion

    }

}