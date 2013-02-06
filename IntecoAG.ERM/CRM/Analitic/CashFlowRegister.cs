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
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// ����� crmCashFlowRegister
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmCashFlowRegister")]
    public class crmCashFlowRegister : crmBasePrimaryPartyContragentRegister
    {
        public crmCashFlowRegister(Session ses) : base(ses) { }

        #region ���� ������

        #endregion


        #region �������� ������

        private crmPaymentItem _PaymentItem;
        /// <summary>
        /// PaymentItem
        /// ����� ������������ ������
        /// </summary>
        public virtual crmPaymentItem PaymentItem {
            get { return _PaymentItem; }
            set { SetPropertyValue<crmPaymentItem>("PaymentItem", ref _PaymentItem, value); }
        }

        private decimal _PaymentCost;
        /// <summary>
        /// CalculateCost
        /// ��������� � ������ �������
        /// </summary>
        public decimal PaymentCost {
            get { return _PaymentCost; }
            set { SetPropertyValue<decimal>("PaymentCost", ref _PaymentCost, value); }
        }

        private csValuta _PaymentValuta;
        /// <summary>
        /// CalculateValuta
        /// ������ �������
        /// </summary>
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
            }
        }

        private decimal _Cost;
        /// <summary>
        /// Cost
        /// ��������� � ������ ��������
        /// </summary>
        public decimal Cost {
            get { return _Cost; }
            set { SetPropertyValue<decimal>("Cost", ref _Cost, value); }
        }

        private csValuta _Valuta;
        /// <summary>
        /// Valuta
        /// ������ ��������
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        private decimal _CostInRUR;
        /// <summary>
        /// CostInRUR
        /// ��������� � ������
        /// </summary>
        public decimal CostInRUR {
            get { return _CostInRUR; }
            set { SetPropertyValue<decimal>("CostInRUR", ref _CostInRUR, value); }
        }

        #endregion


        #region ������

        #endregion

    }
}