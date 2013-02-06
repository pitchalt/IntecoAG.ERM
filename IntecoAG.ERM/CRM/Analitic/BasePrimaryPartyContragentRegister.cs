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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Класс crmBasePrimaryPartyContragentRegister
    /// </summary>
    [NonPersistent]
    public class crmBasePrimaryPartyContragentRegister : crmCommonBaseRegister
    {
        public crmBasePrimaryPartyContragentRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        // SHU 2011-11-01
        private DateTime _ObligationUnitDateTime;
        /// <summary>
        /// DateTime
        /// Дата и время обязательства
        /// </summary>
        public DateTime ObligationUnitDateTime {
            get { return _ObligationUnitDateTime; }
            set { SetPropertyValue<DateTime>("ObligationUnitDateTime", ref _ObligationUnitDateTime, value); }
        }

        // SHU 2011-11-01
        private crmObligationUnit _ObligationUnit;
        /// <summary>
        /// ObligationUnit
        /// Обязательство
        /// </summary>
        public crmObligationUnit ObligationUnit {
            get { return _ObligationUnit; }
            set { SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value); }
        }

        // SHU 2011-11-01 crmContractParty заменены на crmPartyRu
        private crmPartyRu _PrimaryParty;
        /// <summary>
        /// PrimaryParty
        /// Получатель оплаты, он же Исполнитель/Поставщиу
        /// </summary>
        public crmPartyRu PrimaryParty {
            get { return _PrimaryParty; }
            set { SetPropertyValue<crmPartyRu>("PrimaryParty", ref _PrimaryParty, value); }
        }

        private crmPartyRu _ContragentParty;
        /// <summary>
        /// ContragentParty
        /// Плательщик, он же Заказчик/Полкупатель
        /// </summary>
        public crmPartyRu ContragentParty {
            get { return _ContragentParty; }
            set { SetPropertyValue<crmPartyRu>("ContragentParty", ref _ContragentParty, value); }
        }


        // SHU 2011-11-01 Две ссылки на Stage с типом Технический
        private crmStage _StageTech;
        /// <summary>
        /// StageTech
        /// Технический этап 1
        /// </summary>
        public crmStage StageTech {
            get { return _StageTech; }
            set { SetPropertyValue<crmStage>("StageTech", ref _StageTech, value); }
        }



        #endregion


        #region МЕТОДЫ

        #endregion

    }
}