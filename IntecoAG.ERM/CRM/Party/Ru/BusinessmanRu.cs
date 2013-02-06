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
using System.ComponentModel;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Party
{
    /// <summary>
    /// Класс RussianIndividualPerson, представляющий индивидуальное частное предприятие как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Persistent("crmPartyBusinessmanRu")]
    public partial class crmBusinessmanRu : crmLegalPerson
    {
        public crmBusinessmanRu(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА



        /// <summary>
        /// PhysicalPerson
        /// </summary>
        private crmPhysicalPerson _PhysicalPerson;
        [RuleRequiredField("crmBusinessmanRu.RequiredPerson", "Save")]
        public crmPhysicalPerson PhysicalPerson {
            get { return _PhysicalPerson; }
            set { SetPropertyValue("PhysicalPerson", ref _PhysicalPerson, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
//        public override string ToString()
//        {
//            return Name + ". " + ((_PhysicalPerson == null) ? "" : _PhysicalPerson.Name);
//        }

        #endregion

    }

}