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

        private DateTime _ObligationUnitDateTime;
        private crmObligationUnit _ObligationUnit;
        private crmCParty _PrimaryParty;
        private crmCParty _ContragentParty;
        private crmStage _StageTech;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Дата и время обязательства
        /// </summary>
        public DateTime ObligationUnitDateTime {
            get { return _ObligationUnitDateTime; }
            set { SetPropertyValue<DateTime>("ObligationUnitDateTime", ref _ObligationUnitDateTime, value); }
        }


        /// <summary>
        /// Месяц обязательства
        /// </summary>
        public int ObligationUnitMonth {
            get { return ObligationUnitDateTime.Month; }
        }

        /// <summary>
        /// Квартал обязательства
        /// </summary>
        public int ObligationUnitQuarter {
            get {
                if (ObligationUnitMonth <= 3) return 1;
                else if (ObligationUnitMonth <= 6) return 2;
                else if (ObligationUnitMonth <= 9) return 3;
                return 4;
            }
        }

        /// <summary>
        /// Год обязательства
        /// </summary>
        public int ObligationUnitYear {
            get { return ObligationUnitDateTime.Year; }
        }

        /// <summary>
        /// Обязательство
        /// </summary>
        public crmObligationUnit ObligationUnit {
            get { return _ObligationUnit; }
            set { SetPropertyValue<crmObligationUnit>("ObligationUnit", ref _ObligationUnit, value); }
        }

        /// <summary>
        /// Получатель оплаты, он же Исполнитель/Поставщиу
        /// </summary>
        public crmCParty PrimaryParty {
            get { return _PrimaryParty; }
            set { SetPropertyValue<crmCParty>("PrimaryParty", ref _PrimaryParty, value); }
        }

        /// <summary>
        /// Плательщик, он же Заказчик/Полкупатель
        /// </summary>
        public crmCParty ContragentParty {
            get { return _ContragentParty; }
            set { SetPropertyValue<crmCParty>("ContragentParty", ref _ContragentParty, value); }
        }


        /// <summary>
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