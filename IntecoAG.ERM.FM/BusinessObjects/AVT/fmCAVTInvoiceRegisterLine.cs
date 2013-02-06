using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {

    public enum fmCAVTInvoiceRegisterLineType {
        LINE_IN = 1,
        LINE_OUT = 2,
        UNKNOW = 3
    }

    [MiniNavigation("This", "Запись регистра", TargetWindow.Default, 1)]
    [MiniNavigation("Invoice", "Счет-Фактура", TargetWindow.NewModalWindow, 2)]
    [MiniNavigation("InvoiceVersion", "Версия Счет-Фактуры", TargetWindow.NewModalWindow, 3)]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "LineType;InvoiceVersion")]
    [Persistent("fmAVTInvoiceRegisterLine")]
    public class fmCAVTInvoiceRegisterLine : BaseObject {
        //
        public fmCAVTInvoiceRegisterLine(Session session) : base(session) { }

        UInt32 _SequenceNumber;
        fmCAVTInvoiceRegister _InvoiceRegisterIn;
        fmCAVTInvoiceRegister _InvoiceRegisterOut;
        DateTime _DateTransfer;
        private fmCAVTInvoiceTransferType _TransferType;
        private fmCAVTInvoiceOperationType _OperationType;
        fmCAVTInvoiceBase _Invoice;
        fmCAVTInvoiceVersion _InvoiceVersion;

        [Association("fmCAVTInvoiceRegister-fmCAVTInvoiceRegisterInLines")]
        [RuleRequiredField(TargetCriteria = "InvoiceRegisterOut == Null")]
        [VisibleInListView(false)]
        public fmCAVTInvoiceRegister InvoiceRegisterIn {
            get { return _InvoiceRegisterIn; }
            set {
                if (!IsLoading && InvoiceRegisterOut != null) throw new Exception("Already IN, Type can not change");
                SetPropertyValue<fmCAVTInvoiceRegister>("InvoiceRegisterIn", ref _InvoiceRegisterIn, value);
            }
        }
        [Association("fmCAVTInvoiceRegister-fmCAVTInvoiceRegisterOutLines")]
        [RuleRequiredField(TargetCriteria = "InvoiceRegisterIn == Null")]
        [VisibleInListView(false)]
        public fmCAVTInvoiceRegister InvoiceRegisterOut {
            get { return _InvoiceRegisterOut; }
            set {
                if (!IsLoading && InvoiceRegisterIn != null) throw new Exception("Already IN, Type can not change");
                SetPropertyValue<fmCAVTInvoiceRegister>("InvoiceRegisterOut", ref _InvoiceRegisterOut, value);
            }
        }
        [Persistent("LineType")]
        public fmCAVTInvoiceRegisterLineType LineType {
            get {
                if (InvoiceRegisterIn != null)
                    return fmCAVTInvoiceRegisterLineType.LINE_IN;
                if (InvoiceRegisterOut != null)
                    return fmCAVTInvoiceRegisterLineType.LINE_OUT;
                return fmCAVTInvoiceRegisterLineType.UNKNOW;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 SequenceNumber {
            get { return _SequenceNumber; }
            set { SetPropertyValue<UInt32>("SequenceNumber", ref _SequenceNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [RuleRequiredField]
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
        [RuleRequiredField]
        [RuleUniqueValue("", DefaultContexts.Save)]
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
        public String CorrectionInvoiceNumber {
            get {
                if (Invoice != null && Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION)
                    return Invoice.Number;
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTransfer {
            get { return _DateTransfer; }
            set { SetPropertyValue<DateTime>("DateTransfer", ref _DateTransfer, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTInvoiceTransferType TransferType {
            get { return _TransferType; }
            set { SetPropertyValue<fmCAVTInvoiceTransferType>("TransferType", ref _TransferType, value); }
        }
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
        [PersistentAlias("InvoiceVersion.Valuta")]
        public csValuta Valuta {
            get {
                if (InvoiceVersion != null)
                    return InvoiceVersion.Valuta;
                else
                    return null;
            }
        }

        [Persistent]
        public crmCParty Party {
            get {
                if (InvoiceVersion != null) {
                    if (LineType == fmCAVTInvoiceRegisterLineType.LINE_IN)
                        return InvoiceVersion.Supplier;
                    if (LineType == fmCAVTInvoiceRegisterLineType.LINE_OUT)
                        return InvoiceVersion.Customer;
                }
                return null;
            }
        }

        public String INN_KPP {
            get {
                if (Party != null && !String.IsNullOrEmpty(Party.INN)) {
                    if (String.IsNullOrEmpty(Party.KPP))
                        return Party.INN;
                    else
                        return Party.INN + " / " + Party.KPP;
                }
                return String.Empty;
            }
        }

        [Persistent]
        public Decimal SummAVT {
            get {
                if (Invoice != null && InvoiceVersion != null && Invoice.InvoiceIntType != fmCAVTInvoiceIntType.CORRECTION)
                    return Invoice.SummAVT;
                else
                    return 0;
            }
        }
        [Persistent]
        public Decimal SummAll {
            get {
                if (Invoice != null && InvoiceVersion != null && Invoice.InvoiceIntType != fmCAVTInvoiceIntType.CORRECTION)
                    return Invoice.SummAll;
                else
                    return 0;
            }
        }

        [Persistent]
        public Decimal DeltaSummAVTInc {
            get {
                if (Invoice != null && InvoiceVersion != null &&
                    Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION)
                    return InvoiceVersion.DeltaSummAVTAdd;
                else
                    return 0;
            }
        }
        [Persistent]
        public Decimal DeltaSummAVTDec {
            get {
                if (Invoice != null && InvoiceVersion != null &&
                    Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION )
                    return InvoiceVersion.DeltaSummAVTSub;
                else
                    return 0;
            }
        }
        [Persistent]
        public Decimal DeltaSummAllInc {
            get {
                if (Invoice != null && InvoiceVersion != null &&
                    Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION)
                    return InvoiceVersion.DeltaSummAllAdd;
                else
                    return 0;
            }
        }
        [Persistent]
        public Decimal DeltaSummAllDec {
            get {
                if (Invoice != null && InvoiceVersion != null &&
                    Invoice.InvoiceIntType == fmCAVTInvoiceIntType.CORRECTION )
                    return InvoiceVersion.DeltaSummAllSub;
                else
                    return 0;
            }
        }
    }


}
