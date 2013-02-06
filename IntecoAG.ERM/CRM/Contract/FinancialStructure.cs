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
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс FinancialStructure, представляющий структуру финансирования договора
    /// </summary>
    [Persistent("crmFinancialStructure")]
    public partial class crmFinancialStructure : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmFinancialStructure(Session session) : base(session) {
            if (!IsLoading) {
                this.VersionState = VersionStates.VERSION_NEW;
                //VersionAfterConstruction();
            }
        }

        public crmFinancialStructure(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            //this.FirstStage = new crmStage(this.Session, this.VersionState);
            //this.FirstStage.StageType = StageType.AGREGATE;
            //this.Stages.Add(this.FirstStage);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

/*
        /// <summary>
        /// crmDeliveryPlan
        /// </summary>
        private crmDeliveryPlan _DeliveryPlan;
        public crmDeliveryPlan crmDeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<crmDeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }

        /// <summary>
        /// crmPaymentPlan
        /// </summary>
        private crmPaymentPlan _PaymentPlan;
        public crmPaymentPlan crmPaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<crmPaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }

        /// <summary>
        /// Stage - этапы
        /// </summary>
        private crmStage _FirstStage;
        [Aggregated]
        public crmStage FirstStage {
            get { return _FirstStage; }
            set { SetPropertyValue<crmStage>("FirstStage", ref _FirstStage, value); }
        }

        private DisplaySummMode _DisplaySummMode;
        public DisplaySummMode DisplaySummMode {
            get { return _DisplaySummMode; }
            set {
                if (!IsLoading) {
                    foreach (crmStage ss in Stages)
                        ss.DisplaySummMode = value;
                }
                SetPropertyValue<DisplaySummMode>("DisplaySummMode", ref _DisplaySummMode, value);
            }
        }
*/
        /// <summary>
        /// Description - описание
        /// </summary>
//        [VisibleInListView(false)]
//        [Size(SizeAttribute.Unlimited)]
//        [NonPersistent]
//        public string Description {
//            get { return FirstStage.Description; }
//            set { 
//                FirstStage.Description = value;
//                OnChanged(Description);
//            }
//        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}