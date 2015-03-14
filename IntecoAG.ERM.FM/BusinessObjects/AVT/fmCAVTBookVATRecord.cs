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
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
//
namespace IntecoAG.ERM.FM.AVT {

    [DomainComponent]
    public interface IBookPay20144Record {
        UInt32 C01_SequenceNumber { get; }
        String C02_OperationTypes { get; }
        String C03_InvoceNumberDate { get; }
        String C04_InvoiceChangeNumberDate { get; }
        String C05_CorrectionNumberDate { get; }
        String C06_CorrectionChangeNumberDate { get; }
        String C07_PartyName { get; }
        String C08_PartyInnKpp { get; }
        String C09_IntermediatePartyName { get; }
        String C10_IntermediatePartyInnKpp { get; }
        String C11_PayDocNumberDate { get; }
        String C12_ValutaCodeName { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C13A_SummAllValuta { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C13B_SummAllRub { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C14_SummCost18 { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C15_SummCost10 { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C16_SummCost0 { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C17_SummVat18 { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C18_SummVat10 { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal C19_SummNoVat { get; }
    }

    [DomainComponent]
    public interface IBookBay20144Record {
        UInt32 C01_SequenceNumber { get; }
        String C02_OperationTypes { get; }
        String C03_InvoceNumberDate { get; }
        String C04_InvoiceChangeNumberDate { get; }
        String C05_CorrectionNumberDate { get; }
        String C06_CorrectionChangeNumberDate { get; }
        String B07_PayDocNumberDate { get; }
        DateTime B08_BuhDate { get; }
        String B09_PartyName { get; }
        String B10_PartyInnKpp { get; }
        String B11_IntermediatePartyName { get; }
        String B12_IntermediatePartyInnKpp { get; }
        String B13_CustomsDeclarationNumber { get; }
        String B14_ValutaCodeName { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal B15_SummAllValuta { get; }
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        Decimal B16_SummVat { get; }
    }


    [VisibleInReports]
    [Persistent("fmAVTBookVATRecord")]
//    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "BookVAT;SequenceNumber")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "BookVAT;Invoice;RecordType;BuhRecordType")]
    public class fmCAVTBookVATRecord : XPLiteObject, IBookPay20144Record, IBookBay20144Record {

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

        private fmCAVTBookBuhStruct _BookBuhStruct;
        public fmCAVTBookBuhStruct BookBuhStruct {
            get { return _BookBuhStruct; }
            set { SetPropertyValue<fmCAVTBookBuhStruct>("BookBuhStruct", ref _BookBuhStruct, value); }
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
        private String _BuhRecordType;
        [Size(10)]
        public String BuhRecordType {
            get { return _BuhRecordType; }
            set { SetPropertyValue<String>("BuhRecordType", ref _BuhRecordType, value); }
        }
        private String _VATInvoiceType;
        [Size(3)]
        public String VATInvoiceType {
            get { return _VATInvoiceType; }
            set { SetPropertyValue<String>("VATInvoiceType", ref _VATInvoiceType, value); }
        }
        private String _VATInvoiceNumber;
        [Size(50)]
        public String VATInvoiceNumber {
            get { return _VATInvoiceNumber; }
            set { SetPropertyValue<String>("VATInvoiceNumber", ref _VATInvoiceNumber, value); }
        }
        private String _VATInvoiceRegNumber;
        [Size(50)]
        public String VATInvoiceRegNumber {
            get { return _VATInvoiceRegNumber; }
            set { SetPropertyValue<String>("VATInvoiceRegNumber", ref _VATInvoiceRegNumber, value); }
        }
        private DateTime _VATInvoiceDate;
        public DateTime VATInvoiceDate {
            get { return _VATInvoiceDate; }
            set { SetPropertyValue<DateTime>("VATInvoiceDate", ref _VATInvoiceDate, value); }
        }

        private crmCParty _Party;
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }
        public DateTime PayDate;
        public String PayNumber;
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
        public String PayText {
            get {
                if (String.IsNullOrEmpty(PayNumber))
                    return String.Empty;
                else
                    return PayNumber + " " + PayDate.ToString("dd.MM.yyyy");
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
        private csValuta _Valuta;
        /// <summary>
        /// 
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
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


        private Decimal _SummAll_Correct;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll_Correct {
            get { return _SummAll_Correct; }
            set { SetPropertyValue<Decimal>("SummAll_Correct", ref _SummAll_Correct, value); }
        }

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll_Valuta;

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummAll {
            get {
                if (SummAll_Correct == 0)
                    return SummVAT_10 + SummCost_10 + SummVAT_18 + SummCost_18 + SummVAT_20 + SummCost_20 + SummCost_0 + SummCost_NoVAT + 
                        SummBayVatDeduction + SummBayVatInCost + SummBayCost;
                else
                    return SummAll_Correct;
            }
        }

        [PersistentAlias("SequenceNumber")]
        public UInt32 C01_SequenceNumber {
            get { return SequenceNumber; }
        }

        public String C02_OperationTypes {
            get { return OperationType != null ? OperationType.Code : String.Empty;  }
        }

        public String C03_InvoceNumberDate {
            get { return InvoiceText; }
        }

        public String C04_InvoiceChangeNumberDate {
            get { return InvoiceVersionText; }
        }

        public String C05_CorrectionNumberDate {
            get { return CorrectionInvoiceText; }
        }

        public String C06_CorrectionChangeNumberDate {
            get { return CorrectionInvoiceVersionText; }
        }

        public String C07_PartyName {
            get { return Party != null ? Party.Name : String.Empty; }
        }

        public String C08_PartyInnKpp {
            get { return Party != null ? Party.INN + " / " + Party.KPP : String.Empty; }
        }


        public String C09_IntermediatePartyName {
            get { return String.Empty; }
        }

        public String C10_IntermediatePartyInnKpp {
            get { return String.Empty; }
        }

        public String C11_PayDocNumberDate {
            get { return PayText; }
        }

        public String C12_ValutaCodeName {
            get {
                if (Valuta != null)
                    return Valuta.NameShort + " " + Valuta.Code;
                else
                    return String.Empty;
            }
        }

        public Decimal C13A_SummAllValuta {
            get {
                    return SummAll_Valuta;
            }
        }

        public Decimal C13B_SummAllRub {
            get {
                if (Invoice != null)
                    return Invoice.SummAll;
                else
                    return SummAll;
            }
        }

        public Decimal C14_SummCost18 {
            get {
                return SummCost_18_Correct;
            }
        }

        public Decimal C15_SummCost10 {
            get {
                return SummCost_10; 
            }
        }

        public Decimal C16_SummCost0 {
            get { return SummCost_0; }
        }

        public Decimal C17_SummVat18 {
            get { return SummVAT_18; }
        }

        public Decimal C18_SummVat10 {
            get { return SummVAT_10; }
        }

        public Decimal C19_SummNoVat {
            get { return 0; }
        }

        public String B07_PayDocNumberDate {
            get { return PayText; }
        }

        public DateTime B08_BuhDate {
            get { return BuhDate.Date; }
        }

        public String B09_PartyName {
            get { return Party != null ? Party.Name : String.Empty; }
        }

        public String B10_PartyInnKpp {
            get { return Party != null ? Party.INN + " / " + Party.KPP : String.Empty; }
        }

        public String B11_IntermediatePartyName {
            get { return String.Empty; }
        }

        public String B12_IntermediatePartyInnKpp {
            get { return String.Empty; }
        }

        public String B13_CustomsDeclarationNumber {
            get { return String.Empty; }
        }

        public String B14_ValutaCodeName {
            get {
                if (Valuta != null)
                    return Valuta.NameShort + " " + Valuta.Code;
                else
                    return String.Empty;
            }
        }

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal B15_SummAllValuta {
            get {
                if (SummAll_Correct != 0)
                    return SummAll_Correct;
                if (Invoice != null)
                    return Invoice.SummAll;
                return SummAll;
            }
        }

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal B16_SummVat {
            get {
                return SummBayVatDeduction;
            }
        }

        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayVatDeduction;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayVatCharge;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayVatInCost;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayVatExp;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayVatOtherCredit;
        [Custom("DisplayFormat", "### ### ### ##0.00")]
        public Decimal SummBayCost;

    }

}