using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.AVT;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.FinJurnal {
    /// <summary>
    /// Класс счета фактуры 
    /// </summary>
    [Persistent("fmFJSaleOperation")]
    [DefaultProperty("Code")]
    [FriendlyKeyProperty("Code")]
    [NavigationItem("Sale")]
    public class fmCFJSaleOperation : csCComponent {
        public fmCFJSaleOperation(Session ses) : base(ses) { }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        //
        private String _Code;
        private String _Name;
        //
        //private fmCAVTInvoiceBase _AVTInvoice;
        private fmCAVTInvoiceType _AVTInvoiceType;
        //
        private crmContractDeal _Deal;
        private fmCOrderExt _Order;
        private csNDSRate _AVTRate;
        private csValuta _Valuta;
        //
        private Int32 _DocBuhProv;
        private Int32 _DocBuhPck;
        private Int32 _DocBuhNumber;
        private Int32 _AccRealDebet;
        private Int32 _AccRealCredit;
        private Int32 _AccAVTDebet;
        private Int32 _AccAVTCredit;
        private Boolean _IsNotAVTInvoice;
        /// <summary>
        /// 
        /// </summary>
        [Size(30)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(70)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public fmCAVTInvoiceBase AVTInvoice {
        //    get { return _AVTInvoice; }
        //    set {
        //        SetPropertyValue<fmCAVTInvoiceBase>("AVTInvoice", ref _AVTInvoice, value);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTInvoiceType AVTInvoiceType {
            get { return _AVTInvoiceType; }
            set {
                SetPropertyValue<fmCAVTInvoiceType>("AVTInvoiceType", ref _AVTInvoiceType, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public crmContractDeal Deal {
            get { return _Deal; }
            set {
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCOrderExt Order {
            get { return _Order; }
            set {
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public csNDSRate AVTRate {
            get { return _AVTRate; }
            set {
                SetPropertyValue<csNDSRate>("AVTRate", ref _AVTRate, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 DocBuhProv {
            get { return _DocBuhProv; }
            set { SetPropertyValue<Int32>("DocBuhProv", ref _DocBuhProv, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 DocBuhPck {
            get { return _DocBuhPck; }
            set { SetPropertyValue<Int32>("DocBuhPck", ref _DocBuhPck, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 DocBuhNumber {
            get { return _DocBuhNumber; }
            set { SetPropertyValue<Int32>("DocBuhNumber", ref _DocBuhNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AccRealDebet {
            get { return _AccRealDebet; }
            set { SetPropertyValue<Int32>("AccRealDebet", ref _AccRealDebet, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AccRealCredit {
            get { return _AccRealCredit; }
            set { SetPropertyValue<Int32>("AccRealCredit", ref _AccRealCredit, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AccAVTDebet {
            get { return _AccAVTDebet; }
            set { SetPropertyValue<Int32>("AccAVTDebet", ref _AccAVTDebet, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AccAVTCredit {
            get { return _AccAVTCredit; }
            set { SetPropertyValue<Int32>("AccAVTCredit", ref _AccAVTCredit, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsNotAVTInvoice {
            get { return _IsNotAVTInvoice; }
            set { SetPropertyValue<Boolean>("IsNotAVTInvoice", ref _IsNotAVTInvoice, value); }
        }

    }
}
