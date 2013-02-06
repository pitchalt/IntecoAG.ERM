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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс ObligationTransfer, представляющий трансферы обязательствам Договора
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract partial class crmObligationTransfer : crmObligation   //, IVersionSupport
    {
        public crmObligationTransfer(Session session) : base(session) { }
        public crmObligationTransfer(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            //Count = new CS.Measurement.Measurement(0, new CS.Measurement.Unit(this.Session));
            //Price = new CS.Measurement.Measurement(0, new CS.Measurement.Unit(this.Session));

            //Count = new CS.Measurement.Measurement(123);
            //Price = new CS.Measurement.Measurement(0);
            //Summ = new CS.Measurement.Measurement(0);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #region Count

        ///// <summary>
        ///// Count - именованное количество
        ///// </summary>
        //private CS.Measurement.Measurement _Count;
        //[Persistent("Count")]
        //public CS.Measurement.Measurement Count {
        //    get { return _Count; }
        //    set { SetPropertyValue("Count", ref _Count, value); }
        //}

        
        #endregion



        #region Summ

        ///// <summary>
        ///// Summ - именованное количество
        ///// </summary>
        //private CS.Measurement.Measurement _Summ;
        //[Persistent("Summ")]
        //public CS.Measurement.Measurement Summ {
        //    get { return _Summ; }
        //    set { SetPropertyValue("Summ", ref _Summ, value); }
        //}

        ///// <summary>
        ///// SummCode
        ///// </summary>
        //private decimal _SummCode;
        //public decimal SummCode {
        //    get { return _SummCode; }
        //    set { SetPropertyValue("SummCode", ref _SummCode, value); }
        //}

        ///// <summary>
        ///// SummUnit
        ///// </summary>
        //private CS.Measurement.Unit _SummUnit;
        //public CS.Measurement.Unit SummUnit {
        //    get { return _SummUnit; }
        //    set { SetPropertyValue("SummUnit", ref _SummUnit, value); }
        //}

        #endregion



        /// <summary>
        /// Номентклатура
        /// </summary>
//        private CS.Nomenclature.Nomenclature _Nomenclature;
        [NonPersistent]
        public abstract CS.Nomenclature.Nomenclature Nomenclature {
            get;
        }
/*
        /// <summary>
        /// Contragent Ссылка на сторону договора - Исполнитель/Поставщик (вычисляется?)
        /// </summary>
        private crmContractParty _Debitor;
        public crmContractParty Debitor {
            get { return _Debitor; }
            set {
                SetPropertyValue<crmContractParty>("Debitor", ref _Debitor, value);
            }
        }

        /// <summary>
        /// Customer Ссылка на сторону договора - Заказчик/Покупатель (вычисляется?)
        /// </summary>
        private crmContractParty _Creditor;
        public crmContractParty Creditor {
            get { return _Creditor; }
            set { SetPropertyValue<crmContractParty>("Creditor", ref _Creditor, value); }
        }
*/

        #endregion

    }

}