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

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс PaymentUnit, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmPaymentUnit : crmObligationUnit   //BaseObject, IVersionSupport
    {
        public crmPaymentUnit(Session session) : base(session) { }
        public crmPaymentUnit(Session session, VersionStates state) : base(session) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }

        #region ПОЛЯ КЛАССА

        #endregion

        #region СВОЙСТВА КЛАССА

        ///// <summary>
        ///// DatePlane - планируемая дата оплаты
        ///// <summary>
        //private DateTime _DatePlane;
        //public DateTime DatePlane {
        //    get { return _DatePlane; }
        //    set { SetPropertyValue<DateTime>("DatePlane", ref _DatePlane, value); }
        //}

        ///// <summary>
        ///// DateStart
        ///// <summary>
        //private DateTime _DateStart;
        //public DateTime DateStart {
        //    get { return _DateStart; }
        //    set { SetPropertyValue<DateTime>("DateStart", ref _DateStart, value); }
        //}

        ///// <summary>
        ///// DateStop
        ///// </summary>
        //private DateTime _DateStop;
        //public DateTime DateStop {
        //    get { return _DateStop; }
        //    set { SetPropertyValue<DateTime>("DateStop", ref _DateStop, value); }
        //}


        ///// <summary>
        ///// WorkParty
        ///// </summary>
        //private WorkParty _WorkParty;
        //public WorkParty WorkParty {
        //    get { return _WorkParty; }
        //    set { SetPropertyValue<WorkParty>("WorkParty", ref _WorkParty, value); }
        //}


        //// Плательщик, он же Заказчик/Полкупатель
        //private ContractParty _Payer;
        //public ContractParty Payer {
        //    get { return _Payer; }
        //    set { SetPropertyValue<ContractParty>("Payer", ref _Payer, value); }
        //}

        //// Получатель оплаты, он же Испольнитель/Поставщиу
        //private ContractParty _Recipient;
        //public ContractParty Recipient {
        //    get { return _Recipient; }
        //    set { SetPropertyValue<ContractParty>("Recipient", ref _Recipient, value); }
        //}
        public override crmStage Stage {
            get { return PaymentStage; }
        }
        private crmStage _PaymentStage;
        [Association("crmStage-PaymentUnits")]
        public crmStage PaymentStage {
            get { return this._PaymentStage; }
            set { SetPropertyValue<crmStage>("Stage", ref _PaymentStage, value); }
        }

        #endregion

        #region МЕТОДЫ

        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

    }

}