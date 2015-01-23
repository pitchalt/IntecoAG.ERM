using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
//
namespace IntecoAG.ERM.FM.AVT {

    [DomainComponent]
    public interface IBookPay20144Record {
        UInt32 C1_SequenceNumber { get; }
        fmCAVTInvoiceOperationType C2_OperationType { get; }
        String C3_InvoceNumberDate { get; }
        String C4_InvoiceChangeNumberDate { get; }
        String C5_CorrectionNumberDate { get; }
        String C6_CorrectionChangeNumberDate { get; }
        String C7_PartyName { get; }
        String C8_PartyInnKpp { get; }

    }


    [VisibleInReports]
    [Persistent("fmAVTBookVATRecord")]
//    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "BookVAT;SequenceNumber")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "BookVAT;Invoice;RecordType;BuhRecordType")]
    public class fmCAVTBookVATRecord : XPLiteObject, IBookPay20144Record {

        public enum fmCAVTBookVATRecordType {
            MAIN = 1,
            STORNO = 2
        };

        public fmCAVTBookVATRecord(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            RecordType = fmCAVTBookVATRecordType.MAIN;
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

        private UInt32 _SequenceNumber;
        private fmCAVTBookVAT _BookVAT;
        private fmCAVTBookVATRecordType _RecordType;
        private fmCAVTInvoiceBase _Invoice;
        private fmCAVTInvoiceVersion _InvoiceVersion;
        /// <summary>
        /// 
        /// </summary>
        [Association("fmAVTBookVAT-fmAVTBookVATRecords")]
        [Indexed("Invoice;RecordType;BuhRecordType", Unique = true)]
        public fmCAVTBookVAT BookVAT {
            get { return _BookVAT; }
            set { SetPropertyValue<fmCAVTBookVAT>("BookVAT", ref _BookVAT, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTBookVATRecordType RecordType {
            get { return _RecordType; }
            set { SetPropertyValue<fmCAVTBookVATRecordType>("RecordType", ref _RecordType, value); }
        }

        private fmCAVTInvoiceOperationType _OperationType;
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTInvoiceOperationType OperationType {
            get { return _OperationType; }
            set { SetPropertyValue<fmCAVTInvoiceOperationType>("OperationType", ref _OperationType, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 SequenceNumber {
            get { return _SequenceNumber; }
            set { SetPropertyValue<UInt32>("SequenceNumber", ref _SequenceNumber, value); }
        }
        [Size(10)]
        public String BuhRecordType;
        [Size(3)]
        public String VATInvoiceType;
        [Size(20)]
        public String VATInvoiceNumber;
        [Size(20)]
        public String VATInvoiceRegNumber;
        public DateTime VATInvoiceDate;

        public crmCParty Party;
        public DateTime PayDate;
        public DateTime BuhDate;


        /// <summary>
        /// 
        /// </summary>
        [VisibleInListView(false)]
        public fmCAVTInvoiceBase Invoice {
            get { return _Invoice; }
            set {
                fmCAVTInvoiceBase old = _Invoice;
                if (old != value) {
                    _Invoice = value;
                    if (!IsLoading) {
                        if (value != null)
                            InvoiceVersion = value.Current;
                        OnChanged("Invoice", old, value);
                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [DataSourceProperty("Invoice.AVTInvoiceVersions")]
        //[RuleUniqueValue("", DefaultContexts.Save)]
        [VisibleInListView(false)]
        public fmCAVTInvoiceVersion InvoiceVersion {
            get { return _InvoiceVersion; }
            set {
                SetPropertyValue<fmCAVTInvoiceVersion>("InvoiceVersion", ref _InvoiceVersion, value);
                if (!IsLoading && value != null) {
                    Invoice = value.AVTInvoice;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String InvoiceNumber {
            get {
                if (Invoice != null) {
                    if (Invoice.InvoiceIntType == fmCAVTInvoiceIntType.NORMAL)
                        return Invoice.Number;
                    else if (Invoice.InvoiceCorrection != null)
                        return Invoice.InvoiceCorrection.Number;
                }
                return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime InvoiceDate {
            get {
                if (Invoice != null) {
                    if (Invoice.InvoiceIntType == fmCAVTInvoiceIntType.NORMAL)
                        return Invoice.Date;
                    else if (Invoice.InvoiceCorrection != null)
                        return Invoice.InvoiceCorrection.Date;
                }
                return default(DateTime);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String InvoiceText {
            get {
                if (Invoice != null) 
                    return InvoiceNumber + " " + InvoiceDate.ToString("dd.MM.yyyy");
                else
                    return VATInvoiceNumber + " " + VATInvoiceDate.ToString("dd.MM.yyyy");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String InvoiceVersionText {
            get {
                if (Invoice != null && !String.IsNullOrEmpty(Invoice.VersionNumber) )
                    return Invoice.VersionNumber + " " + Invoice.VersionDate.ToString("dd.MM.yyyy");
                else
                    return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String CorrectionInvoiceText {
            get {
                if (!String.IsNullOrEmpty(CorrectionInvoiceNumber) )
                    return CorrectionInvoiceNumber + " " + CorrectionInvoiceDate.ToString("dd.MM.yyyy");
                else
                    return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String CorrectionInvoiceVersionText {
            get {
                return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String CorrectionInvoiceNumber {
            get {
                if (Invoice != null && Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION)
                    return Invoice.Number;
                else
                    return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CorrectionInvoiceDate {
            get {
                if (Invoice != null && Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION)
                    return Invoice.Date;
                return default(DateTime);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String VersionNumber {
            get {
                if (InvoiceVersion != null && !String.IsNullOrEmpty(InvoiceVersion.VersionNumber))
                    return InvoiceVersion.VersionNumber;
                else
                    return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime VersionDate {
            get {
                if (InvoiceVersion != null && !String.IsNullOrEmpty(InvoiceVersion.VersionNumber))
                    return InvoiceVersion.VersionDate;
                else
                    return default(DateTime);
            }
        }
        //
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVAT_10;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost_10;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVAT_18;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost_18;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummVAT_20;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost_20;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost_0;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummCost_NoVAT;

        public Decimal SummCost_18_Correct {
            get {
                if (VATInvoiceType == "—‘¿" ||
                    BuhRecordType == "AON") {
                    return 0;
                }
                else
                    return SummCost_18;
            }
        }


        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll {
            get { 
                return SummVAT_10 + SummCost_10 + SummVAT_18 + SummCost_18 + SummVAT_20 + SummCost_20 + SummCost_0 + SummCost_NoVAT;
            }
        }



        uint IBookPay20144Record.C1_SequenceNumber {
            get { return SequenceNumber; }
        }

        fmCAVTInvoiceOperationType IBookPay20144Record.C2_OperationType {
            get { return OperationType; }
        }

        string IBookPay20144Record.C3_InvoceNumberDate {
            get { return InvoiceText; }
        }

        string IBookPay20144Record.C4_InvoiceChangeNumberDate {
            get { return InvoiceVersionText; }
        }

        string IBookPay20144Record.C5_CorrectionNumberDate {
            get { return CorrectionInvoiceText; }
        }

        string IBookPay20144Record.C6_CorrectionChangeNumberDate {
            get { return CorrectionInvoiceVersionText; }
        }

        string IBookPay20144Record.C7_PartyName {
            get { return Party.Name; }
        }

        string IBookPay20144Record.C8_PartyInnKpp {
            get { return Party.INN + " / " + Party.KPP; }
        }
    }

}