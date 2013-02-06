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
    /// Класс crmBaseDebitCreditRegister
    /// </summary>
    [NonPersistent]
    public class crmBaseDebitCreditRegister : crmCommonBaseRegister
    {
        public crmBaseDebitCreditRegister(Session ses) : base(ses) { }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmPartyRu _Creditor;
        /// <summary>
        /// Creditor
        /// Получатель оплаты, он же Испольнитель/Поставщиу
        /// </summary>
        public crmPartyRu Creditor {
            get { return _Creditor; }
            set { SetPropertyValue<crmPartyRu>("Creditor", ref _Creditor, value); }
        }

        private crmPartyRu _Debitor;
        /// <summary>
        /// Debitor
        /// Плательщик, он же Заказчик/Полкупатель
        /// </summary>
        public crmPartyRu Debitor {
            get { return _Debitor; }
            set { SetPropertyValue<crmPartyRu>("Debitor", ref _Debitor, value); }
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