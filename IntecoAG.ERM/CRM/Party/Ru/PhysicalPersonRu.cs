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

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс RussianIndividualPerson, представляющий индивидуальное частное предприятие как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyPhysicalPersonRu")]
    public partial class crmPhysicalPersonRu : crmPhysicalPerson
    {
        public crmPhysicalPersonRu(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// INN
        /// </summary>
//        private string _INN;
//        public string INN {
//            get { return _INN; }
//            set { SetPropertyValue("INN", ref _INN, value); }
//        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
//        public override string ToString()
//        {
//            return Name; //+ ". " + ((_PhysicalPerson == null) ? "" : _PhysicalPerson.Name);
//        }

        #endregion

    }

}