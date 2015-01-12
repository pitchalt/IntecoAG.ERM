using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

//    [VisibleInReports]
    [Persistent("fmAVTBookBuhStructRecord")]
    public class fmCAVTBookBuhStructRecord : XPObject {

        public fmCAVTBookBuhStructRecord(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private fmCAVTBookBuhStruct _InInvoiceStructRecord;
        [Association("fmAVTBookBuhStruct-InInvoiceStructRecords")]
        [Browsable(false)]
        public fmCAVTBookBuhStruct InInvoiceStructRecord {
            get { return _InInvoiceStructRecord; }
            set { SetPropertyValue<fmCAVTBookBuhStruct>("InInvoiceStructRecord", ref _InInvoiceStructRecord, value); }
        }
        private fmCAVTBookBuhStruct _OutInvoiceStructRecord;
        [Association("fmAVTBookBuhStruct-OutInvoiceStructRecords")]
        [Browsable(false)]
        public fmCAVTBookBuhStruct OutInvoiceStructRecord {
            get { return _OutInvoiceStructRecord; }
            set { SetPropertyValue<fmCAVTBookBuhStruct>("OutInvoiceStructRecord", ref _OutInvoiceStructRecord, value); }
        }

        public Int32 RowNumber;
        [Size(20)]
        public String InvoiceRegNumber;
        [Size(3)]
        public String InvoiceType;
        [Size(1)]
        public String TransferType;
        [Size(2)]
        public String OperationType;
        [Size(20)]
        public String InvoiceNumber;
        public DateTime InvoiceDate;
        [Size(20)]
        public String InvoiceChangeNumber;
        public DateTime InvoiceChangeDate;
        [Size(20)]
        public String InvoiceCorrectNumber;
        public DateTime InvoiceCorrectDate;
        [Size(20)]
        public String InvoiceCorrectChangeNumber;
        public DateTime InvoiceCorrectChangeDate;

        public DateTime TransferDate;

        [Size(12)]
        public String PartnerInn;
        [Size(9)]
        public String PartnerKpp;
        [Size(150)]
        public String PartnerName;
        [Size(30)]
        public String PartnerCountry;
        [Size(150)]
        public String PartnerSity;
        [Size(150)]
        public String PartnerAddress;

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVAT;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll;

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummIncCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummIncVAT;

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummDecCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummDecVAT;

        public DateTime SaleDate;
        public csNDSRate SaleVATRate;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SaleSummAll;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SaleSummVAT;

        private String _SaleAccCode;
        [Size(5)]
        public String SaleAccCode {
            get { return _SaleAccCode; }
            set { SetPropertyValue<String>("SaleAccCode", ref _SaleAccCode, value); }
        }

        public DateTime BayDate;
        public csNDSRate BayVATRate;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal BaySummAll;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal BaySummVAT;

        private String _BayAccCode;
        [Size(5)]
        public String BayAccCode {
            get { return _BayAccCode; }
            set { SetPropertyValue<String>("BayAccCode", ref _BayAccCode, value); }
        }

    }

}