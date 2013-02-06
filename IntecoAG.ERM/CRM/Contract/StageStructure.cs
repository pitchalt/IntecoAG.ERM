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
    /// ����� StageStructure, �������������� ��������� ������ ��������
    /// </summary>
    [Persistent("crmStageStructure")]
    public partial class crmStageStructure : VersionRecord   //BaseObject, IVersionSupport
    {
        public crmStageStructure(Session session) : base(session) {
            if (!IsLoading) {
                this.VersionState = VersionStates.VERSION_NEW;
                //VersionAfterConstruction();
            }
        }
        public crmStageStructure(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
            this.FirstStage = new crmStage(this.Session, this.VersionState);
            this.FirstStage.StageType = StageType.AGREGATE;
            this.Stages.Add(this.FirstStage);
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// DeliveryPlan
        /// </summary>
        private DeliveryPlan _DeliveryPlan;
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }

        /// <summary>
        /// PaymentPlan
        /// </summary>
        private PaymentPlan _PaymentPlan;
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }

        /// <summary>
        /// Stage - �����
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

        /// <summary>
        /// Description - ��������
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


        #region ������

        #endregion

    }

}