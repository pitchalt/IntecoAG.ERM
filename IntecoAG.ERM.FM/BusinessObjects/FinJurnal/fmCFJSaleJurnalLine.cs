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
    //
    public enum fmJurnalLineType{
        WorkLine = 1,
        TemplateLine = 2
    }
    /// <summary>
    /// Класс
    /// </summary>
    [Persistent("fmFJSaleJurnalLine")]
    [VisibleInReports]
    [NavigationItem("Sale")]
    public class fmCFJSaleJurnalLine : csCComponent {
        public fmCFJSaleJurnalLine(Session ses) : base(ses) { }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
            JurnalLineType = fmJurnalLineType.WorkLine;
        }
        //
        private fmJurnalLineType _JurnalLineType;
        private fmCFJSaleOperation _SaleOperation;
        private Int32 _IntNumber;
        //
        private crmCParty _Party;
        //
        private fmCAVTInvoiceBase _AVTInvoice;
        private String _AVTInvoiceNumber;
        private DateTime _AVTInvoiceDate;
        private fmCAVTInvoiceType _AVTInvoiceType;
        //
        private crmContractDeal _Deal;
        //
        private fmCOrderExt _Order;
        //
        private String _DocBaseType;
        private String _DocBaseNumber;
        private DateTime _DocBaseDate;
        //
        private Decimal _SummCost;
        private csNDSRate _AVTRate;
        private Decimal _SummAVT;
        private csValuta _Valuta;
        private Decimal _SummValuta;
        //
        private Int32 _DocBuhProv;
        private Int32 _DocBuhPck;
        private Int32 _DocBuhNumber;
        private DateTime _DocBuhDate;
        private Int32 _AccRealDebet;
        private Int32 _AccRealCredit;
        private Int32 _AccAVTDebet;
        private Int32 _AccAVTCredit;
        /// <summary>
        /// тип записи
        /// </summary>
        public fmJurnalLineType JurnalLineType {
            get { return _JurnalLineType; }
            set { SetPropertyValue<fmJurnalLineType>("JurnalLineType", ref _JurnalLineType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [RuleRequiredField]
        public fmCFJSaleOperation SaleOperation {
            get { return _SaleOperation; }
            set {
                SetPropertyValue<fmCFJSaleOperation>("JurnalLineType", ref _SaleOperation, value);
                if (!IsLoading && value != null) {
                    SaleOperationSet(value);
                }
            }
        }
        //
        protected void SaleOperationSet(fmCFJSaleOperation oper) {
            if (oper.AVTInvoiceType != null)
                AVTInvoiceType = oper.AVTInvoiceType;
            //
            if (oper.Deal != null)
                Deal = oper.Deal;
            if (oper.Order != null)
                Order = oper.Order;
            if (oper.AVTRate != null)
                AVTRate = oper.AVTRate;
            if (oper.Valuta != null)
                Valuta = oper.Valuta;
            //
            if (oper.DocBuhProv != 0)
                DocBuhProv = oper.DocBuhProv;
            if (oper.DocBuhPck != 0)
                DocBuhPck = oper.DocBuhPck;
            if (oper.DocBuhNumber != 0)
                DocBuhNumber = oper.DocBuhNumber;
            if (oper.AccRealDebet != 0)
                AccRealDebet = oper.AccRealDebet;
            if (oper.AccRealCredit != 0)
                AccRealCredit = oper.AccRealCredit;
            if (oper.AccAVTDebet != 0)
                AccAVTDebet = oper.AccAVTDebet;
            if (oper.AccAVTCredit != 0)
                AccAVTCredit = oper.AccAVTCredit;
        }
        /// <summary>
        /// 
        /// </summary>
        public crmCParty Party {
            get { return _Party; }
            set {
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(30)]
        public String AVTInvoiceNumber {
            get { return _AVTInvoiceNumber; }
            set { SetPropertyValue<String>("AVTInvoiceNumber", ref _AVTInvoiceNumber, value); }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        public DateTime AVTInvoiceDate {
            get { return _AVTInvoiceDate; }
            set {
                SetPropertyValue<DateTime>("AVTInvoiceDate", ref _AVTInvoiceDate, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTInvoiceBase AVTInvoice {
            get { return _AVTInvoice; }
            set {
                SetPropertyValue<fmCAVTInvoiceBase>("AVTInvoice", ref _AVTInvoice, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "SaleOperation.AVTInvoiceType != Null", Enabled = false)]
        public fmCAVTInvoiceType AVTInvoiceType {
            get { return _AVTInvoiceType; }
            set {
                SetPropertyValue<fmCAVTInvoiceType>("AVTInvoiceType", ref _AVTInvoiceType, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "SaleOperation.Deal != Null", Enabled = false)]
        public crmContractDeal Deal {
            get { return _Deal; }
            set {
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(3)]
        public String DocBaseType {
            get { return _DocBaseType; }
            set { SetPropertyValue<String>("DocBaseType", ref _DocBaseType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(30)]
        public String DocBaseNumber {
            get { return _DocBaseNumber; }
            set { SetPropertyValue<String>("DocBaseNumber", ref _DocBaseNumber, value); }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        public DateTime DocBaseDate {
            get { return _DocBaseDate; }
            set {
                SetPropertyValue<DateTime>("DocBaseDate", ref _DocBaseDate, value);
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
        public Decimal SummCost {
            get { return _SummCost; }
            set { 
                SetPropertyValue<Decimal>("SummCost", ref _SummCost, value);
                if (!IsLoading) {
                    OnChanged("SummAll");
                }
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
        public Decimal SummAVT {
            get { return _SummAVT; }
            set { 
                SetPropertyValue<Decimal>("SummAVT", ref _SummAVT, value);
                if (!IsLoading) {
                    OnChanged("SummAll");
                }
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
        public Decimal SummValuta {
            get { return _SummValuta; }
            set { SetPropertyValue<Decimal>("SummValuta", ref _SummValuta, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal SummAll {
            get { return SummCost + SummAVT; }
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
        public DateTime DocBuhDate {
            get { return _DocBuhDate; }
            set { SetPropertyValue<DateTime>("DocBuhDate", ref _DocBuhDate, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        public Int32 DocBuhDateYYYY {
            get { return DocBuhDate.Year; }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        public Int32 DocBuhDateMM {
            get { return DocBuhDate.Month; }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        public Int32 DocBuhDateDD {
            get { return DocBuhDate.Day; }
        }
        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        public String DocBuhDateMonth {
            get { return DocBuhDate.ToString("MMMM"); }
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
        [RuleRequiredField]
        public Int32 IntNumber {
            get { return _IntNumber; }
            set { SetPropertyValue<Int32>("IntNumber", ref _IntNumber, value); }
        }

        public void SyncIBS() {

        }
    }
}
