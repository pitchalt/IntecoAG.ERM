using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
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
    /// Класс
    /// </summary>
    [Persistent("fmFJSaleDocLine")]
    public class fmCFJSaleDocLine : csCComponent {
        public fmCFJSaleDocLine(Session ses) : base(ses) { }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        //
        private fmCFJSaleDoc _SaleDoc;
        private fmCFJSaleJurnalLine _SaleJurnalLine;
        private fmCFJSaleOperation _SaleOperation;
        //
        private String _PartyCode;
        private String _PartyName;
        private crmCParty _Party;
        //
        private String _AVTInvoiceNumber;
        private DateTime _AVTInvoiceDate;
        private fmCAVTInvoiceBase _AVTInvoice;
        private fmCAVTInvoiceType _AVTInvoiceType;
        //
        private String _DealNumber;
        private DateTime _DealDate;
        private crmContractDeal _Deal;
        //
        private String _OrderNumber;
        private fmCOrderExt _Order;
        //
        private String _DocBaseType;
        private String _DocBaseNumber;
        private DateTime _DocBaseDate;
        private String _PayNumber;
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
        //
        private Int32 _SaleJurnalNumber;
        /// <summary>
        /// тип записи
        /// </summary>
        //[Browsable(false)]
        [Association("fmCFJSaleDoc-fmCFJSale")]
        public fmCFJSaleDoc SaleDoc {
            get { return _SaleDoc; }
            set { SetPropertyValue<fmCFJSaleDoc>("SaleDoc", ref _SaleDoc, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public fmCFJSaleJurnalLine SaleJurnalLine {
            get { return _SaleJurnalLine; }
            set { SetPropertyValue<fmCFJSaleJurnalLine>("SaleJurnalLine", ref _SaleJurnalLine, value); }
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
        [Appearance("", AppearanceItemType.ViewItem, "Party != Null", Enabled = false)]
        [Size(5)]
        public String PartyCode {
            get { return _PartyCode; }
            set {
                SetPropertyValue<String>("PartyCode", ref _PartyCode, value);
                if (!IsLoading && !String.IsNullOrEmpty(value) && Party == null) {
                    Party = Session.FindObject<crmCParty>(new BinaryOperator("Code", value));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "Party != Null", Enabled = false)]
        [Size(180)]
        public String PartyName {
            get { return _PartyName; }
            set { SetPropertyValue<String>("PartyName", ref _PartyName, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public crmCParty Party {
            get { return _Party; }
            set {
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
                if (!IsLoading) {
                    if (value != null) {
                        this.Deal = DealCheck(this.Party, this.DealNumber, this.DealDate);
                        PartyCode = value.Code;
                        PartyName = value.Name;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "AVTInvoice != Null or SaleOperation.AVTInvoiceType != Null", Enabled = false)]
        [RuleRequiredField]
        public fmCAVTInvoiceType AVTInvoiceType {
            get { return _AVTInvoiceType; }
            set {
                SetPropertyValue<fmCAVTInvoiceType>("AVTInvoiceType", ref _AVTInvoiceType, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(30)]
        [Appearance("", AppearanceItemType.ViewItem, "AVTInvoice != Null or AVTInvoiceType.AutoNumber", Enabled = false)]
        public String AVTInvoiceNumber {
            get { return _AVTInvoiceNumber; }
            set {
                SetPropertyValue<String>("AVTInvoiceNumber", ref _AVTInvoiceNumber, value);
                if (!IsLoading && AVTInvoice == null) {
                    AVTInvoice = AVTInvoiceCheck();
                }
            }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "AVTInvoice != Null", Enabled = false)]
        [RuleRequiredField]
        public DateTime AVTInvoiceDate {
            get { return _AVTInvoiceDate; }
            set {
                SetPropertyValue<DateTime>("AVTInvoiceDate", ref _AVTInvoiceDate, value);
                if (!IsLoading && AVTInvoice == null) {
                    AVTInvoice = AVTInvoiceCheck();
                }
            }
        }

        protected fmCAVTInvoiceBase AVTInvoiceCheck() {
            crmCParty supl = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    supl = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            fmCAVTInvoiceBase invoice = Session.FindObject<fmCAVTInvoiceBase>(
                CriteriaOperator.And(
                    new BinaryOperator("Number", AVTInvoiceNumber),
                    new BinaryOperator("Date", AVTInvoiceDate),
                    new BinaryOperator("Supplier", supl)
                    ));
            return invoice;
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTInvoiceBase AVTInvoice {
            get { return _AVTInvoice; }
            set {
                SetPropertyValue<fmCAVTInvoiceBase>("AVTInvoice", ref _AVTInvoice, value);
                if (!IsLoading && value != null) {
                    this.AVTInvoiceType = value.InvoiceType;
                    this.AVTInvoiceNumber = value.Number;
                    this.AVTInvoiceDate = value.Date;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(30)]
        [Appearance("", AppearanceItemType.ViewItem, "Deal != Null", Enabled = false)]
        public String DealNumber {
            get { return _DealNumber; }
            set { 
                SetPropertyValue<String>("DealNumber", ref _DealNumber, value);
                if (!IsLoading && Deal == null)
                    this.Deal = DealCheck(this.Party, this.DealNumber, this.DealDate);
            }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "Deal != Null", Enabled = false)]
        public DateTime DealDate {
            get { return _DealDate; }
            set {
                SetPropertyValue<DateTime>("DealDate", ref _DealDate, value);
                if (!IsLoading && Deal == null)
                    this.Deal = DealCheck(this.Party, this.DealNumber, this.DealDate);
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
                if (!IsLoading && value != null) {
                    this.DealNumber = value.ContractDocument.Number;
                    this.DealDate = value.ContractDocument.Date;
                }
            }
        }
        protected crmContractDeal DealCheck(crmCParty party, String cnt_number, DateTime cnt_date) {
            XPQuery<crmContractDeal> deals = new XPQuery<crmContractDeal>(this.Session);
            var qd = from deal in deals
                     where 
                        deal.Customer == party  &&
                        deal.ContractDocument.Number == cnt_number &&
                        deal.ContractDocument.Date.Date == cnt_date.Date
                    select deal;
            foreach (crmContractDeal deal in qd) {
                return deal;
            }
            return null;
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
        [Size(30)]
        public String PayNumber {
            get { return _PayNumber; }
            set { SetPropertyValue<String>("PayNumber", ref _PayNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(9)]
        [Appearance("", AppearanceItemType.ViewItem, "Order != Null", Enabled = false)]
        public String OrderNumber {
            get { return _OrderNumber; }
            set {
                SetPropertyValue<String>("OrderNumber", ref _OrderNumber, value);
                if (!IsLoading && !String.IsNullOrEmpty(value) && Order == null) {
                    Order = Session.FindObject<fmCOrderExt>(new BinaryOperator("Code", value));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCOrderExt Order {
            get { return _Order; }
            set {
                SetPropertyValue<fmCOrderExt>("Order", ref _Order, value);
                if (!IsLoading) {
                    if (value != null) {
                        OrderNumber = value.Code;
                    }
                }
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
        public Int32 SaleJurnalNumber {
            get { return _SaleJurnalNumber; }
            set { SetPropertyValue<Int32>("SaleJurnalNumber", ref _SaleJurnalNumber, value); }
        }

        private Boolean _IsApproved;
        private Boolean _IsSyncIBS;
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsApproved {
            get { return _IsApproved; }
            set { SetPropertyValue<Boolean>("IsApproved", ref _IsApproved, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsSyncIBS {
            get { return _IsSyncIBS; }
            set { SetPropertyValue<Boolean>("IsSyncIBS", ref _IsSyncIBS, value); }
        }

        public void Approve() {
            //            if (IsApproved) return;
            if (SaleJurnalNumber == 0) {
                if (AVTInvoiceType.AutoNumber) {
                    SaleJurnalNumber = fmCAVTInvoiceNumberGenerator.GenerateNumber(this.Session, this.Oid, this.AVTInvoiceType, this.AVTInvoiceDate, 0);
                    AVTInvoiceNumber = AVTInvoiceType.Prefix + AVTInvoiceDate.ToString("yyyyMMdd").Substring(4, 2) + SaleJurnalNumber.ToString("00000");
                }
                else {
                    if (AVTInvoiceNumber == null) return;
                    if (AVTInvoiceNumber.Substring(0, 1) != "4") {
                        if (AVTInvoiceNumber.Length != 8) return;
                        if (AVTInvoiceNumber.Substring(0, this.AVTInvoiceType.Prefix.Length) != this.AVTInvoiceType.Prefix) return;
                        if (AVTInvoiceNumber.Substring(1, 2) != this.AVTInvoiceDate.ToString("yyyyMMdd").Substring(4, 2)) return;
                        Int32 num = 0;
                        if (Int32.TryParse(AVTInvoiceNumber.Substring(3, 5), out num)) {
                            if (num < 1) return;
                            SaleJurnalNumber = num;
                            fmCAVTInvoiceNumberGenerator.GenerateNumber(this.Session, this.Oid, this.AVTInvoiceType, this.AVTInvoiceDate, this.SaleJurnalNumber);
                        }
                    }
                    else {
                        if (AVTInvoiceNumber.Substring(0, this.AVTInvoiceType.Prefix.Length) != this.AVTInvoiceType.Prefix) return;
                        if (AVTInvoiceNumber.Substring(2, 2) != this.AVTInvoiceDate.ToString("yyyyMMdd").Substring(4, 2)) return;
                        Int32 num = 0;
                        if (Int32.TryParse(AVTInvoiceNumber.Substring(5, 5), out num)) {
                            if (num < 1) return;
                            SaleJurnalNumber = num;
                            fmCAVTInvoiceNumberGenerator.GenerateNumber(this.Session, this.Oid, this.AVTInvoiceType, this.AVTInvoiceDate, this.SaleJurnalNumber);
                        }
                    }
                }
            }
            if (!this.SaleOperation.IsNotAVTInvoice && AVTInvoice == null) {
                AVTInvoice = AVTInvoiceCheck();
                if (AVTInvoice == null) {
                    fmCAVTInvoiceBase invoice = new fmCAVTInvoiceBase(this.Session);
                    invoice.InvoiceType = this.AVTInvoiceType;
                    invoice.Date = this.AVTInvoiceDate;
                    invoice.IntNumber = SaleJurnalNumber;
                    invoice.Number = this.AVTInvoiceNumber;
                    if (crmUserParty.CurrentUserPartyGet(this.Session) != null)
                        invoice.Supplier = crmUserParty.CurrentUserPartyGet(this.Session).Party;
                    invoice.UseCounter++;
                    invoice.IsApproved = true;
                    this.AVTInvoice = invoice;
                }
            }
            if (AVTInvoice != null) {
                this.AVTInvoice.Customer = this.Party;
                this.AVTInvoice.Valuta = this.Valuta;
                this.AVTInvoice.SummCost = this.SummCost;
                this.AVTInvoice.SummAVT = this.SummAVT;
            }
            this.DocBuhNumber = this.SaleJurnalNumber;
            this.DocBuhDate = this.AVTInvoiceDate;
            if (SaleJurnalLine == null) {
                SaleJurnalLine = new fmCFJSaleJurnalLine(this.Session);
            }
            SaleJurnalLine.JurnalLineType = fmJurnalLineType.WorkLine;
            SaleJurnalLine.SaleOperation = this.SaleOperation;
            SaleJurnalLine.IntNumber = this.SaleJurnalNumber;
            SaleJurnalLine.Party = this.Party;
            SaleJurnalLine.AVTInvoiceType = this.AVTInvoiceType;
            SaleJurnalLine.AVTInvoiceNumber = this.AVTInvoiceNumber;
            SaleJurnalLine.AVTInvoiceDate = this.AVTInvoiceDate;
            SaleJurnalLine.AVTInvoice = this.AVTInvoice;
            SaleJurnalLine.Deal = this.Deal;
            SaleJurnalLine.Order = this.Order;
            SaleJurnalLine.SummCost = this.SummCost;
            SaleJurnalLine.AVTRate = this.AVTRate;
            SaleJurnalLine.SummAVT = this.SummAVT;
            SaleJurnalLine.Valuta = this.Valuta;
            SaleJurnalLine.SummValuta = this.SummValuta;
            //
            SaleJurnalLine.DocBuhProv = this.DocBuhProv;
            SaleJurnalLine.DocBuhPck = this.DocBuhPck;
            SaleJurnalLine.DocBuhNumber = this.DocBuhNumber;
            SaleJurnalLine.DocBuhDate = this.DocBuhDate;
            SaleJurnalLine.AccRealDebet = this.AccRealDebet;
            SaleJurnalLine.AccRealCredit = this.AccRealCredit;
            SaleJurnalLine.AccAVTDebet = this.AccAVTDebet;
            SaleJurnalLine.AccAVTCredit = this.AccAVTCredit;
            //
            UseCounter = 1;
            IsApproved = true;
        }

        public override bool ReadOnlyGet() {
            return IsApproved;
        }
    }
}
