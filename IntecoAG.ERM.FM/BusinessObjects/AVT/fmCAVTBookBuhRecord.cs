using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    [VisibleInReports]
    [Persistent("fmAVTBookBuhRecord")]
    public class fmCAVTBookBuhRecord : XPLiteObject {

        public enum SummType {
            COST = 1,
            NDS = 2,
            COST_NDS = 4
        };

        public fmCAVTBookBuhRecord(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

#if MediumTrust
        private Guid oid = Guid.Empty;
        [Browsable(false), Key(true), NonCloneable]
        public Guid Oid {
            get { return oid; }
            set { oid = value; }
        }
#else
        [Persistent("Oid"), Key(true), Browsable(false), MemberDesignTimeVisibility(false)]
        private Guid oid = Guid.Empty;
        [PersistentAlias("_Oid"), Browsable(false)]
        public Guid Oid { get { return oid; } }
#endif 
        protected override void OnSaving() {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork) && Session.IsNewObject(this)) {
                if (oid.Equals(Guid.Empty)) {
                    oid = XpoDefault.NewGuid();
                }
            }
        }

        private fmCAVTBookBuhImport _BookBuhImport;
        [Association("fmAVTBookBuhImport-fmAVTBookBuhRecords")]
        [Browsable(false)]
        public fmCAVTBookBuhImport BookBuhImport {
            get { return _BookBuhImport; }
            set { SetPropertyValue<fmCAVTBookBuhImport>("BookBuhImport", ref _BookBuhImport, value); }
        }

        private fmCAVTBookBuhStruct _BookBuhStruct;
        [Association("fmAVTBookBuhStruct-fmAVTBookBuhRecords")]
        [Browsable(false)]
        public fmCAVTBookBuhStruct BookBuhStruct {
            get { return _BookBuhStruct; }
            set { SetPropertyValue<fmCAVTBookBuhStruct>("BookBuhStruct", ref _BookBuhStruct, value); }
        }

        public csCCodedComponent BookBuh {
            get { 
                if (BookBuhImport != null) return BookBuhImport;
                if (BookBuhStruct != null) return BookBuhStruct;
                return null; 
            }
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