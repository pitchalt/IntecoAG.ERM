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

namespace IntecoAG.ERM.CRM.Contract.Analitic
{

    /// <summary>
    /// ����� crmPlaneFactRegister
    /// </summary>
    //[DefaultClassOptions]
    [Persistent("crmPlaneFactRegister")]
    public class crmPlaneFactRegister : crmBaseDebitCreditRegister
    {
        public crmPlaneFactRegister(Session ses) : base(ses) { }

        #region ���� ������

        #endregion


        #region �������� ������

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

        private decimal _Volume;
        /// <summary>
        /// Volume
        /// ����� � ����������� ���������
        /// </summary>
        public virtual decimal Volume {
            get { return _Volume; }
            set { SetPropertyValue<decimal>("Volume", ref _Volume, value); }
        }

        #endregion


        #region ������

        #endregion

    }
}