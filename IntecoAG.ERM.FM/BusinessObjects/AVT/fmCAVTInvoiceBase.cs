using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.AVT {

    public enum fmCAVTInvoiceIntType {
        NORMAL = 1,
        CORRECTION = 2
    }
    /// <summary>
    /// Класс счета фактуры 
    /// </summary>
    [Persistent("fmAVTInvoice")]
    [NavigationItem("AVT")]
    [VisibleInReports]
    [DefaultProperty("InvoiceName")]
    [LikeSearchPathList(new string[] { 
        "Number", 
        "RegNumber", 
        "Current.Customer.Name",
        "Current.Customer.INN",
        "Current.Customer.AddressLegal.AddressString",
        "Current.Supplier.Name",
        "Current.Supplier.INN",
        "Current.Supplier.AddressLegal.AddressString",
    })]
    public class fmCAVTInvoiceBase : csCComponent {
        public fmCAVTInvoiceBase(Session ses)
            : base(ses) {
        }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Date = DateTime.Now;
            this.InvoiceIntType = fmCAVTInvoiceIntType.NORMAL;
            if (Current == null) {
                Current = new fmCAVTInvoiceVersion(this.Session);
                AVTInvoiceVersions.Add(Current);
            }
            this.Valuta = Session.FindObject<csValuta>(new BinaryOperator("Code", "RUB"));
        }
        //
        private String _Number;
        private String _RegNumber;
        private DateTime _Date;
        private fmCAVTInvoiceVersion _Current;
        private fmCAVTInvoiceType _InvoiceType;
        private fmCAVTInvoiceIntType _InvoiceIntType;
        private fmCAVTInvoiceBase _InvoiceCorrection;
        private fmCAVTInvoiceVersion _InvoiceCorrectionVersion;
        //
        /// <summary>
        /// Номер
        /// </summary>
        [Appearance("", AppearanceItemType.ViewItem, "InvoiceType.InvoiceDirection = 'AVTInvoiceOut' and InvoiceType.AutoNumber = 'True' or IsApproved", Enabled = false)]
        [Size(50)]
        public String Number {
            get { return _Number; }
            set { SetPropertyValue<String>("Number", ref _Number, value); }
        }
        /// <summary>
        /// Регистрационный номер 
        /// </summary>
        [Size(50)]
        public String RegNumber {
            get { return _RegNumber; }
            set { SetPropertyValue<String>("RegNumber", ref _RegNumber, value); }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Association("fmCAVTInvoice-fmCAVTInvoiceCorrections")]
        public fmCAVTInvoiceBase InvoiceCorrection {
            get { return _InvoiceCorrection; }
            set {
                fmCAVTInvoiceBase old = _InvoiceCorrection;
                if (old != value) {
                    _InvoiceCorrection = value;
                    if (!IsLoading) {
                        if (value != null) {
                            InvoiceIntType = fmCAVTInvoiceIntType.CORRECTION;
                            InvoiceCorrectionVersion = value.Current;
                        }
                        OnChanged("InvoiceCorrection", old, value);
                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [DataSourceProperty("InvoiceCorrection.AVTInvoiceVersions")]
        public fmCAVTInvoiceVersion InvoiceCorrectionVersion {
            get { return _InvoiceCorrectionVersion; }
            set { SetPropertyValue<fmCAVTInvoiceVersion>("InvoiceCorrectionVersion", ref _InvoiceCorrectionVersion, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Association("fmCAVTInvoice-fmCAVTInvoiceCorrections", typeof(fmCAVTInvoiceBase))]
        public XPCollection<fmCAVTInvoiceBase> Corrections {
            get { return GetCollection<fmCAVTInvoiceBase>("Corrections"); }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public String InvoiceName {
            get {
                return Number + " от " + Date.ToString("dd.MM.yyyy");
            }
        }
        /// <summary>
        /// Внутренний тип Счет-Фактуры
        /// </summary>
        public fmCAVTInvoiceIntType InvoiceIntType {
            get { return _InvoiceIntType; }
            set { SetPropertyValue<fmCAVTInvoiceIntType>("InvoiceIntType", ref _InvoiceIntType, value); }
        }
        /// <summary>
        /// Тип Счета-Фактуры
        /// </summary>
        public fmCAVTInvoiceType InvoiceType {
            get { return _InvoiceType; }
            set {
                SetPropertyValue<fmCAVTInvoiceType>("Date", ref _InvoiceType, value);
                if (!IsLoading) {
                    if (_InvoiceType != null) {
                        if (_InvoiceType.InvoiceDirection == fmAVTInvoiceDirection.AVTInvoiceIn) {
                            if (crmUserParty.CurrentUserParty != null) {
                                if (crmUserParty.CurrentUserParty.Value != null) {
                                    this.Customer = crmUserParty.CurrentUserPartyGet(this.Session).Party;
                                }
                            }
                        }
                        if (_InvoiceType.InvoiceDirection == fmAVTInvoiceDirection.AVTInvoiceOut) {
                            if (crmUserParty.CurrentUserParty != null) {
                                if (crmUserParty.CurrentUserParty.Value != null) {
                                    this.Supplier = crmUserParty.CurrentUserPartyGet(this.Session).Party;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Текущая редакция счета фактуры
        /// </summary>
        [Browsable(false)]
        public fmCAVTInvoiceVersion Current {
            get { return _Current; }
            set { SetPropertyValue<fmCAVTInvoiceVersion>("Current", ref _Current, value); }
        }
        /// <summary>
        /// Перечень редакций счета фактуры
        /// </summary>
        [Aggregated]
        [Association("fmAVTInvoiceBase-fmAVTInvoiceVersion", typeof(fmCAVTInvoiceVersion))]
        public XPCollection<fmCAVTInvoiceVersion> AVTInvoiceVersions {
            get {
                return GetCollection<fmCAVTInvoiceVersion>("AVTInvoiceVersions");
            }
        }
        public void AVTInvoiceVersionsAdd(fmCAVTInvoiceVersion ver) {
            if (Current != ver) {
                UInt32 num = 0;
                if (UInt32.TryParse(Current.VersionNumber, out num))
                    ver.VersionNumber = (num + 1).ToString();
                ver.VersionDate = DateTime.Now;
                ver.Source = Current;
            }
            else {
                ver.VersionNumber = String.Empty;
                ver.VersionDate = default(DateTime);
            }
        }
        public void AVTInvoiceVersionsRemove(fmCAVTInvoiceVersion ver) {
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Current.VersionNumber")]
        public String VersionNumber {
            get { return Current.VersionNumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Current.VersionNumber")]
        public DateTime VersionDate {
            get { return Current != null ? Current.VersionDate : DateTime.MinValue; }
        }
        /// <summary>
        /// Покупатель
        /// </summary>
        [PersistentAlias("Current.Customer")]
        public crmCParty Customer {
            get { return Current.Customer; }
            set {
                if (Current != null) {
                    crmCParty old = Customer;
                    if (old != value) {
                        Current.Customer = value;
                        OnChanged("Customer", old, value);
                    }
                }
            }
        }
        /// <summary>
        /// Продавец
        /// </summary>
        [PersistentAlias("Current.Supplier")]
        [VisibleInLookupListView(true)]
        public crmCParty Supplier {
            get { return Current.Supplier; }
            set {
                if (Current != null) {
                    crmCParty old = Supplier;
                    if (old != value) {
                        Current.Supplier = value;
                        OnChanged("Supplier", old, value);
                    }
                }
            }
        }
        /// <summary>
        /// Грузополучатель
        /// </summary>
        [VisibleInListView(false)]
        [PersistentAlias("Current.Consigner")]
        public crmCParty Consigner {
            get { return Current.Consigner; }
            set {
                if (Current != null) {
                    crmCParty old = Consigner;
                    if (old != value) {
                        Current.Consigner = value;
                        OnChanged("Consigner", old, value);
                    }
                }
            }
        }
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public String ConsigerText {
            get {
                if (Customer == Consigner || Consigner == null)
                    return String.Empty;
                else
                    return Consigner.Name + " " + Consigner.AddressLegal.AddressString;
            }
        }
        /// <summary>
        /// Грузоотправитель
        /// </summary>
        [VisibleInListView(false)]
        [PersistentAlias("Current.Shipper")]
        public crmCParty Shipper {
            get { return Current.Shipper; }
            set {
                if (Current != null) {
                    crmCParty old = Shipper;
                    if (old != value) {
                        Current.Shipper = value;
                        OnChanged("Shipper", old, value);
                    }
                }
            }
        }
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public String ShipperText {
            get {
                if (Supplier == Shipper || Shipper == null)
                    return String.Empty;
                else
                    return Shipper.Name + " " + Shipper.AddressLegal.AddressString;
            }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        [PersistentAlias("Current.Valuta")]
        public csValuta Valuta {
            get { return Current.Valuta; }
            set {
                if (Current != null) {
                    csValuta old = Valuta;
                    if (old != value) {
                        Current.Valuta = value;
                        OnChanged("Valuta", old, value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Current.PaymentsText")]
        public String PaymentsText {
            get { return Current.PaymentsText; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("Current.NewPayments")]
        [Aggregated]
        public IList<fmCAVTInvoiceVersion.fmCAVTInvoicePayment> PaymentsList {
            get { return Current.NewPayments; }
        }

        /// <summary>
        /// Стоимость 
        /// </summary>
        [PersistentAlias("Current.SummCost")]
        public Decimal SummCost {
            get { return Current.SummCost; }
            set {
                if (Current != null) {
                    Decimal old = SummCost;
                    if (old != value) {
                        Current.SummCost = value;
                        OnChanged("SummCost", old, value);
                        OnChanged("SummAll");
                    }
                }
            }
        }
        /// <summary>
        /// НДС
        /// </summary>
        [PersistentAlias("Current.SummAVT")]
        public Decimal SummAVT {
            get { return Current.SummAVT; }
            set {
                if (Current != null) {
                    Decimal old = SummAVT;
                    if (old != value) {
                        Current.SummAVT = value;
                        OnChanged("SummAVT", old, value);
                        OnChanged("SummAll");
                    }
                }
            }
        }
        /// <summary>
        /// Всего
        /// </summary>
        public Decimal SummAll {
            get { return SummCost + SummAVT; }
        }
        /// <summary>
        /// Стоимость 
        /// </summary>
        [PersistentAlias("Current.DeltaSummCostAdd")]
        public Decimal DeltaSummCostAdd {
            get { return Current.DeltaSummCostAdd; }
            set {
                if (Current != null) {
                    Decimal old = DeltaSummCostAdd;
                    if (old != value) {
                        Current.DeltaSummCostAdd = value;
                        OnChanged("DeltaSummCostAdd", old, value);
                        OnChanged("DeltaSummAllAdd");
                    }
                }
            }
        }
        /// <summary>
        /// Стоимость 
        /// </summary>
        [PersistentAlias("Current.DeltaSummCostSub")]
        public Decimal DeltaSummCostSub {
            get { return Current.DeltaSummCostSub; }
            set {
                if (Current != null) {
                    Decimal old = DeltaSummCostSub;
                    if (old != value) {
                        Current.DeltaSummCostSub = value;
                        OnChanged("DeltaSummCostSub", old, value);
                        OnChanged("DeltaSummAllSub");
                    }
                }
            }
        }
        /// <summary>
        /// НДС
        /// </summary>
        [PersistentAlias("Current.DeltaSummAVTAdd")]
        public Decimal DeltaSummAVTAdd {
            get { return Current.DeltaSummAVTAdd; }
            set {
                if (Current != null) {
                    Decimal old = DeltaSummAVTAdd;
                    if (old != value) {
                        Current.DeltaSummAVTAdd = value;
                        OnChanged("DeltaSummAVTAdd", old, value);
                        OnChanged("DeltaSummAllAdd");
                    }
                }
            }
        }
        /// <summary>
        /// НДС
        /// </summary>
        [PersistentAlias("Current.DeltaSummAVTSub")]
        public Decimal DeltaSummAVTSub {
            get { return Current.DeltaSummAVTSub; }
            set {
                if (Current != null) {
                    Decimal old = DeltaSummAVTSub;
                    if (old != value) {
                        Current.DeltaSummAVTSub = value;
                        OnChanged("DeltaSummAVTSub", old, value);
                        OnChanged("DeltaSummAllSub");
                    }
                }
            }
        }
        /// <summary>
        /// Всего
        /// </summary>
        public Decimal DeltaSummAllAdd {
            get { return DeltaSummCostAdd + DeltaSummAVTAdd; }
        }
        /// <summary>
        /// Всего
        /// </summary>
        public Decimal DeltaSummAllSub {
            get { return DeltaSummCostSub + DeltaSummAVTSub; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        public XPCollection<fmCAVTInvoiceLine> Lines {
            get {
                if (Current != null)
                    return Current.Lines;
                else
                    return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Action(PredefinedCategory.Edit, Caption = "Утвердить",
            TargetObjectsCriteria = "InvoiceType != 'Null'", AutoCommit = true)]
        public void ApproveAction() {
            UnitOfWork uow = this.Session as UnitOfWork;
            if (uow != null)
                uow.CommitChanges();
            Approve();
            if (uow != null)
                uow.CommitChanges();
        }

        private Boolean _IsApproved;
        private Int32 _IntNumber;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Int32 IntNumber {
            get { return _IntNumber; }
            set { SetPropertyValue<Int32>("IntNumber", ref _IntNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Boolean IsApproved {
            get { return _IsApproved; }
            set { SetPropertyValue<Boolean>("IsApproved", ref _IsApproved, value); }
        }

        public void Approve() {
            if (this.InvoiceType != null && this.Date != DateTime.MinValue) {
                if (this.InvoiceType.InvoiceDirection == fmAVTInvoiceDirection.AVTInvoiceOut) {
                    if (this.InvoiceType.AutoNumber && String.IsNullOrEmpty(Number) && IntNumber == 0) {
                        this.IntNumber = fmCAVTInvoiceNumberGenerator.GenerateNumber(this.Session, this.Oid, this.InvoiceType, this.Date, 0);
                        this.Number = this.InvoiceType.Prefix + this.Date.ToString("yyyyMMdd").Substring(4, 2) + this.IntNumber.ToString("00000");
                        IsApproved = true;
                        this.UseCounter = 1;
                    }
                    if (!this.InvoiceType.AutoNumber && Number != null && IntNumber == 0) {
                        if (Number.Length == 8) {
                            if (Number.Substring(0, this.InvoiceType.Prefix.Length) != this.InvoiceType.Prefix)
                                return;
                            if (Number.Substring(1, 2) != this.Date.ToString("yyyyMMdd").Substring(4, 2))
                                return;
                            Int32 num = 0;
                            if (Int32.TryParse(Number.Substring(3, 5), out num)) {
                                this.IntNumber = num;
                                if (this.IntNumber != 0) {
                                    fmCAVTInvoiceNumberGenerator.GenerateNumber(this.Session, this.Oid, this.InvoiceType, this.Date, this.IntNumber);
                                    IsApproved = true;
                                    this.UseCounter = 1;
                                }
                            }
                        }
                    }
                }

            }
        }

    }
}
