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
using System.ComponentModel;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс Office, представляющий участника (как сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyPartyRu")]
    public class crmPartyRu : crmParty
    {
        public crmPartyRu(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Инициирующая персона
        /// </summary>
        private string _CodeFA;
        public string CodeFA {
            get { return _CodeFA; }
            set { if (_CodeFA != value) SetPropertyValue("CodeFA", ref _CodeFA, value); }
        }
        /// <summary>
        /// Инициирующая персона
        /// </summary>
        private string _KPP;
        public string KPP {
            get { return _KPP; }
            set { if (_KPP != value) SetPropertyValue("KPP", ref _KPP, value); }
        }

        public string INN {
            get { return Person == null ? null : Person.INN; }
        }

        public string RegNumber {
            get { 
                crmLegalPerson lp = Person as crmLegalPerson;
                if (lp == null)
                    return null;
                else
                    return lp.RegNumber;
            }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string Res = "";
            Res = base.ToString() + ", КПП " + this.KPP;
            return Res;
        }

        #endregion

    }

}