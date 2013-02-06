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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс DeliveryUnit, представляющий план работ по Договору
    /// </summary>
    //[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDeliveryUnit : crmObligationUnit //BaseObject, IVersionSupport
    {
        public crmDeliveryUnit(Session session) : base(session) { }
        public crmDeliveryUnit(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        public override fmOrder fmOrder {
            get { return base.fmOrder; }
            set {
                base.fmOrder = value;
                if (!IsLoading) {
                    foreach (crmDeliveryItem di in this.DeliveryItems)
                        di.fmOrder = value;
                }
            }
        }

        //private decimal _Summ;
        //public decimal Summ {
        //    get { return _Summ; }
        //    set { SetPropertyValue<decimal>("Summ", ref _Summ, value); }
        //}

        //private decimal _SummNDS;
        //public decimal SummNDS {
        //    get { return _SummNDS; }
        //    set { SetPropertyValue<decimal>("SummNDS", ref _SummNDS, value); }
        //}

        //public decimal SummFull {
        //    get { return Summ + SummNDS; }
        //}

        ///// <summary>
        ///// Description - описание
        ///// </summary>
        //private string _Description;
        //public string Description {
        //    get { return _Description; }
        //    set { SetPropertyValue<string>("Description", ref _Description, value); }
        //}
        ///// <summary>
        ///// Stage Привязка к этапу
        ///// </summary>
        //private crmStage _Stage;
        //public crmStage Stage {
        //    get { return _Stage; }
        //    set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        //}

        /// <summary>
        /// Valuta
        /// </summary>
        public override Valuta Valuta {
            get { return base.Valuta; }
            set {
                base.Valuta = value;
                if (!IsLoading) {
                    foreach (crmDeliveryItem di in DeliveryItems)
                        di.Valuta = value;
                }
            }
        }
        /// <summary>
        /// CostModel - по смыслу - принятый на текущее время вариант цены для оперирования расчётами (фактическая цена, окончательная цена и т.п.) 
        /// </summary>
        public override crmCostModel CostModel {
            get { return base.CostModel; }
            set {
                base.CostModel = value;
                if (!IsLoading) {
                    foreach (crmDeliveryItem di in DeliveryItems)
                        di.CostModel = value;
                }
            }
        }

        public override crmStage Stage {
            get { return DeliveryStage; }
        }

        private crmStage _DeliveryStage;
        [Association("crmStage-DeliveryUnits")]
        [DataSourceProperty("DeliveryPlan.Stages")]
        public crmStage DeliveryStage {
            get { return this._DeliveryStage; }
            set {
                SetPropertyValue<crmStage>("DeliveryStage", ref _DeliveryStage, value); 
                if (!IsLoading) {
                    if (value != null) {
                        this.CostModel = value.CostModel;
                        this.Valuta = value.Valuta;
                    }
                }
            }
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