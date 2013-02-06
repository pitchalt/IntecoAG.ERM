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
using DevExpress.Data.Filtering;

using DevExpress.Persistent.Validation;
//using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS.Finance;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс PaymentItem, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract partial class crmPaymentItem : crmObligationTransfer
    {
        public crmPaymentItem(Session session) : base(session) { }
        public crmPaymentItem(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Date
        /// <summary>
        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }

        // Суммы

        /// <summary>
        /// AccountSumma - сумма платежа
        /// </summary>
        private decimal _AccountSumma;
        public decimal AccountSumma {
            get { return _AccountSumma; }
            set { SetPropertyValue<decimal>("AccountSumma", ref _AccountSumma, value); }
        }

        /// <summary>
        /// AccountValuta - валюта платежа
        /// </summary>
        private csValuta _AccountValuta;
        public csValuta AccountValuta {
            get { return _AccountValuta; }
            set { SetPropertyValue<csValuta>("AccountValuta", ref _AccountValuta, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public override csNomenclature Nomenclature {
            get {
                return this.AccountValuta;
            }
        }

        public override crmStage Stage {
            get {
                return this.PaymentUnit.Stage;
            }
        }

        //// Сумма c НДС
        //protected decimal _TotalSumm;
        //public decimal TotalSumm {
        //    get { return _TotalSumm; }
        //    set {
        //        SetPropertyValue<decimal>("TotalSumm", ref _TotalSumm, value);
        //    }
        //}

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}