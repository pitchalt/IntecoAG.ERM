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

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// Класс crmBaseDebitCreditRegister
    /// </summary>
    [NonPersistent]
    public abstract class crmBaseDebitCreditRegister : crmCommonBaseRegister
    {
        public crmBaseDebitCreditRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmCParty _Creditor;
        /// <summary>
        /// Creditor
        /// Получатель оплаты, он же Испольнитель/Поставщиу
        /// </summary>
        public crmCParty Creditor {
            get { return _Creditor; }
            set { SetPropertyValue<crmCParty>("Creditor", ref _Creditor, value); }
        }

        private crmCParty _Debitor;
        /// <summary>
        /// Debitor
        /// Плательщик, он же Заказчик/Полкупатель
        /// </summary>
        public crmCParty Debitor {
            get { return _Debitor; }
            set { SetPropertyValue<crmCParty>("Debitor", ref _Debitor, value); }
        }

        private csNomenclature _Nomenclature;
        /// <summary>
        /// Nomenclature
        /// Номенклатура обязательств
        /// </summary>
        public csNomenclature Nomenclature {
            get { return _Nomenclature; }
            set { SetPropertyValue<csNomenclature>("Nomenclature", ref _Nomenclature, value); }
        }

        /// <summary>
        /// MeasureUnit
        /// Единица измерения
        /// </summary>
        private csUnit _MeasureUnit;
        public csUnit MeasureUnit {
            get { return _MeasureUnit; }
            set { SetPropertyValue<csUnit>("MeasureUnit", ref _MeasureUnit, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }
}