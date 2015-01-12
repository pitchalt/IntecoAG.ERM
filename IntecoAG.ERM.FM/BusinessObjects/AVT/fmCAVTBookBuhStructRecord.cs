using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    [VisibleInReports]
    [Persistent("fmCAVTBookBuhStructRecord")]
    public class fmCAVTBookBuhStructRecord : XPObject {

        public fmCAVTBookBuhStructRecord(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private fmCAVTBookBuhImport _BookBuhImport;
        [Association("fmAVTBookBuhImport-fmAVTBookBuhRecords")]
        public fmCAVTBookBuhImport BookBuhImport {
            get { return _BookBuhImport; }
            set { SetPropertyValue<fmCAVTBookBuhImport>("BookBuhImport", ref _BookBuhImport, value); }
        }

        [Size(6)]
        public String PeriodBuhgal;
        [Size(6)]
        public String PeriodOtchet;
        [Size(5)]
        public String BuhProvCode;
        [Size(5)]
        public String BuhProvOrigCode;
        [Size(5)]
        public String BuhPckCode;
        [Size(6)]
        public String BuhDocNumber;
        public DateTime BuhDocDate;
        //
        private String _AccSubDebetCode;
        [Size(5)]
        public String AccSubDebetCode {
            get { return _AccSubDebetCode; }
            set { SetPropertyValue<String>("AccSubDebetCode", ref _AccSubDebetCode, value); }
        }
        private String _AccSubCreditCode;
        [Size(5)]
        public String AccSubCreditCode {
            get { return _AccSubCreditCode; }
            set { SetPropertyValue<String>("AccSubCreditCode", ref _AccSubCreditCode, value); }
        }
        [Size(5)]
        public String AVTInvoicePartyCode;
        [Size(3)]
        public String AVTInvoiceType;
        [Size(20)]
        public String AVTInvoiceNumber;
        [Size(20)]
        public String AVTInvoiceRegNumber;
        public DateTime AVTInvoiceDate;

        [Size(3)]
        public String PayDocType;
        [Size(10)]
        public String PayDocNumber;
        public DateTime PayDocDate;

        [Size(3)]
        public String RecordSummType;

        private Decimal _RecordSumm;

        [Custom("DisplayFormat", "### ### ### ###.00")]
        public Decimal RecordSumm {
            get { return _RecordSumm; }
            set { SetPropertyValue<Decimal>("RecordSumm", ref _RecordSumm, Decimal.Round(value, 2)); }
        }

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATIn;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVAT;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATExp;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATNoInvoice;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATCrdOther;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVATControl;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll;

        [Size(1)]
        public String BookType;
        [Size(3)]
        public String RecordType;
        [Size(8)]
        public String FiscalLetLine;

        [Size(6)]
        public String NDSRate;
    }

}