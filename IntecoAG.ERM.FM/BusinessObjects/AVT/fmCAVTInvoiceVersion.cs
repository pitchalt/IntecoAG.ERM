using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
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

    [DC.DomainComponent]
    public class fmCAVTInvoicePay {
        public fmCAVTInvoicePay() { }
        public String Number;
        public DateTime Date;
    }
    public class ConverterPaymentList2String : ValueConverter<IList<fmCAVTInvoicePay>, String> {

        public override String ConvertTo(IList<fmCAVTInvoicePay> pays) {
            if (pays.Count != 0) {
                IList<String> text_pays = new List<String>(pays.Count);
                foreach (fmCAVTInvoicePay pay in pays)
                    text_pays.Add(pay.Date.ToString("yyyyMMdd") + pay.Number);
                return String.Join(";", text_pays.ToArray<String>());
            }
            else
                return String.Empty;
        }

        public override IList<fmCAVTInvoicePay> ConvertFrom(String str) {
            IList<fmCAVTInvoicePay> pays = new List<fmCAVTInvoicePay>(10);
            if (!String.IsNullOrEmpty(str)) {
                try {
                    foreach (String pay_text in str.Split(';')) {
                        fmCAVTInvoicePay pay = new fmCAVTInvoicePay();
                        pay.Number = pay_text.Substring(8, pay_text.Length - 8);
                        pay.Date = DateTime.ParseExact(pay_text.Substring(0, 8), "yyyyMMdd", null);
                        pays.Add(pay);
                    }
                }
                catch (Exception) {
                }
            }
            return pays;
        }
    }

    /// <summary>
    /// Редакция счета-фактуры
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Number", 
//        "RegNumber", 
        "Customer.Name",
        "Customer.INN",
        "Customer.AddressLegal.AddressString",
        "Supplier.Name",
        "Supplier.INN",
        "Supplier.AddressLegal.AddressString"
    })]

    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "AVTInvoice;VersionNumber")]
    [VisibleInReports]
    [Persistent("fmAVTInvoiceVersion")]
    [DefaultProperty("InvoiceVersionName")]
    [MiniNavigation("This", "Версия Счет-фактуры", TargetWindow.Default, 1)]
    [MiniNavigation("AVTInvoice", "Счет-фактура", TargetWindow.Current, 2)]
    public class fmCAVTInvoiceVersion : csCComponent {
        public fmCAVTInvoiceVersion (Session ses): base(ses) {
        }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
            PaymentsList = new List<fmCAVTInvoicePay>(10);
        }
        //
        [Persistent("fmAVTInvoicePayment")]
        public class fmCAVTInvoicePayment: BaseObject {
            public fmCAVTInvoicePayment(Session session): base(session) { }
            public String Number;
            public DateTime Date;
            [Association("fmCAVTInvoiceVersion-fmCAVTInvoicePayment")]
            public fmCAVTInvoiceVersion InvoiceVersion;
            public fmCDocRCB PayDoc;
        }
        //
        private fmCAVTInvoiceBase _AVTInvoice;
        private fmCAVTInvoiceVersion _Source;
        private String _VersionNumber;
        private DateTime _VersionDate;
        private crmCParty _Customer;
        private crmCParty _Consigner;
        private crmCParty _Supplier;
        private crmCParty _Shipper;
        private csValuta _Valuta;
        private fmCDocRCB _PaymentDoc;
        private String _Payments;
        private hrmStaff _AccountDepartmentSigner;
        private hrmStaff _ManagerSigner;
        private Decimal _SummCost;
        private Decimal _SummAVT;
        private Decimal _DeltaSummCostAdd;
        private Decimal _DeltaSummCostSub;
        private Decimal _DeltaSummAVTAdd;
        private Decimal _DeltaSummAVTSub;
        /// <summary>
        /// Номер редакции
        /// </summary>
        [Size(10)]
        public String VersionNumber {
            get { return _VersionNumber; }
            set { SetPropertyValue<String>("VersionNumber", ref _VersionNumber, value); }
        }
        /// <summary>
        /// Дата редакции 
        /// </summary>
        public DateTime VersionDate {
            get { return _VersionDate; }
            set { SetPropertyValue<DateTime>("VersionDate", ref _VersionDate, value); }
        }
        /// <summary>
        /// Счет фактура данной редакции
        /// </summary>
        [Association("fmAVTInvoiceBase-fmAVTInvoiceVersion")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public fmCAVTInvoiceBase AVTInvoice {
            get { return _AVTInvoice; }
            set {
                if (IsLoading)
                    _AVTInvoice = value;
                else {
                    fmCAVTInvoiceBase old = _AVTInvoice;
                    if (old != value) {
                        if (old != null)
                            old.AVTInvoiceVersionsRemove(this);
                        _AVTInvoice = value;
                        if (value != null)
                            value.AVTInvoiceVersionsAdd(this);
                        OnChanged("AVTInvoice", old, value);
                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonPersistent]
        public String InvoiceVersionName {
            get {
                return VersionNumber + " от " + VersionDate.ToString("dd.MM.yyyy") ;
            }
        }
        /// <summary>
        /// Исходная запись
        /// </summary>
        [Browsable(false)]
        public fmCAVTInvoiceVersion Source {
            get { return _Source; }
            set { SetPropertyValue<fmCAVTInvoiceVersion>("Source", ref _Source, value); }
        }
        /// <summary>
        /// Номер
        /// </summary>
        [PersistentAlias("AVTInvoice.Number")]
        public String Number {
            get { return AVTInvoice.Number; }
        }
        /// <summary>
        /// Дата
        /// </summary>
        [PersistentAlias("AVTInvoice.Date")]
        public DateTime Date {
            get { return AVTInvoice.Date; }
        }
        /// <summary>
        /// Покупатель
        /// </summary>
        public crmCParty Customer {
            get { return _Customer; }
            set {
                if (IsLoading)
                    _Customer = value;
                else {
                    crmCParty old = _Customer;
                    if (old != value) {
                        _Customer = value;
                        if (Consigner == null || Consigner == old)
                            Consigner = value;
                        OnChanged("Customer", old, value);
                    }
                }

            }
        }
        /// <summary>
        /// Грузополучатель
        /// </summary>
        public crmCParty Consigner {
            get { return _Consigner; }
            set { SetPropertyValue<crmCParty>("Consigner", ref _Consigner, value); }
        }
        /// <summary>
        /// Продавец
        /// </summary>
        public crmCParty Supplier {
            get { return _Supplier; }
            set {
                if (IsLoading)
                    _Supplier = value;
                else {
                    crmCParty old = _Supplier;
                    if (old != value) {
                        _Supplier = value;
                        if (Shipper == null || Shipper == old)
                            Shipper = value;
                        OnChanged("Supplier", old, value);
                    }
                }

            }
        }
        /// <summary>
        /// Грузоотправитель
        /// </summary>
        public crmCParty Shipper {
            get { return _Shipper; }
            set { SetPropertyValue<crmCParty>("Shipper", ref _Shipper, value); }
        }
        /// <summary>
        /// Платежный документ
        /// </summary>
        public fmCDocRCB PaymentDoc {
            get { return _PaymentDoc; }
            set { SetPropertyValue<fmCDocRCB>("PaymentDoc", ref _PaymentDoc, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Size(100)]
        [Browsable(false)]
        public String Payments {
            get { return _Payments; }
            set {
                SetPropertyValue<String>("Payments", ref _Payments, value);
                if (!IsLoading)
                    OnChanged("PaymentsText");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String PaymentsText {
            get {
                String result = String.Empty;
                if (PaymentsList != null) {
                    foreach (fmCAVTInvoicePayment pay in NewPayments) {
                        String pay_text = pay.Number + " от " + pay.Date.ToString("dd.MM.yyyy");
                        if (result == String.Empty)
                            result = pay_text;
                        else
                            result = result + ", " + pay_text;
                    }
                }
                return result;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pay"></param>
        //public void PaymentsAdd(fmCAVTInvoicePay pay) {
        //    fmCAVTInvoicePay pay_old = PaymentsList.Where(
        //        rec => rec.Number == pay.Number  &&
        //               rec.Date == pay.Date).FirstOrDefault();
        //    if (pay_old != null) return;
        //    PaymentsList.Add(pay);
        //}
        private IList<fmCAVTInvoicePay> _PaymentsListOld;
        /// <summary>
        /// 
        /// </summary>
        [ValueConverter(typeof(ConverterPaymentList2String))]
        [DC.Aggregated]
        [Browsable(false)]
        public IList<fmCAVTInvoicePay> PaymentsList {
            get {
                return _PaymentsListOld;
            }
            set {
                IList<fmCAVTInvoicePay> old = _PaymentsListOld;
                if (old != value) {
                    _PaymentsListOld = value;
                    if (!IsLoading)
                        OnChanged("PaymentsList");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [Association("fmCAVTInvoiceVersion-fmCAVTInvoicePayment")]
        public XPCollection<fmCAVTInvoicePayment> NewPayments {
            get {
                return GetCollection<fmCAVTInvoicePayment>("NewPayments");
            }
        }
        
        /// <summary>
        /// Валюта счет-фактуры
        /// </summary>
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue<csValuta>("Valuta", ref _Valuta, value); }
        }
        /// <summary>
        /// Главный бухгалтер
        /// </summary>
        public hrmStaff AccountDepartmentSigner {
            get { return _AccountDepartmentSigner; }
            set { SetPropertyValue<hrmStaff>("AccountDepartmentSigner", ref _AccountDepartmentSigner, value); }
        }
        /// <summary>
        /// Руководитель
        /// </summary>
        public hrmStaff ManagerSigner {
            get { return _ManagerSigner; }
            set { SetPropertyValue<hrmStaff>("ManagerSigner", ref _ManagerSigner, value); }
        }
        /// <summary>
        /// Стоимость
        /// </summary>
        public Decimal SummCost {
            get {
                if (!IsLoading && !IsSaving)
                    if (Lines.Count != 0)
                        _SummCost = Lines.Sum(line => line.Cost);
                return _SummCost; 
            }
            set { 
                SetPropertyValue<Decimal>("SummCost", ref _SummCost, value);
                if (!IsLoading) {
                    OnChanged("SummAll");
                }
            }
        }
        /// <summary>
        /// НДС
        /// </summary>
        public Decimal SummAVT {
            get {
                if (!IsLoading && !IsSaving)
                    if (Lines.Count != 0)
                        _SummAVT = Lines.Sum(line => line.AVTSumm);
                return _SummAVT;
            }
            set {
                SetPropertyValue<Decimal>("SummAVT", ref _SummAVT, value);
                if (!IsLoading) {
                    OnChanged("SummAll");
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
        /// Отклонение Стоимость
        /// </summary>
        public Decimal DeltaSummCostAdd {
            get { return _DeltaSummCostAdd; }
            set {
                SetPropertyValue<Decimal>("DeltaSummCostAdd", ref _DeltaSummCostAdd, value);
                if (!IsLoading) {
                    OnChanged("DeltaSummAllAdd");
                }
            }
        }
        /// <summary>
        /// Отклонение Стоимость
        /// </summary>
        public Decimal DeltaSummCostSub {
            get { return _DeltaSummCostSub; }
            set {
                SetPropertyValue<Decimal>("DeltaSummCostSub", ref _DeltaSummCostSub, value);
                if (!IsLoading) {
                    OnChanged("DeltaSummAllSub");
                }
            }
        }
        /// <summary>
        /// Отклонение НДС
        /// </summary>
        public Decimal DeltaSummAVTAdd {
            get { return _DeltaSummAVTAdd; }
            set {
                SetPropertyValue<Decimal>("DeltaSummAVTAdd", ref _DeltaSummAVTAdd, value);
                if (!IsLoading) {
                    OnChanged("DeltaSummAllAdd");
                }
            }
        }
        /// <summary>
        /// Отклонение НДС
        /// </summary>
        public Decimal DeltaSummAVTSub {
            get { return _DeltaSummAVTSub; }
            set {
                SetPropertyValue<Decimal>("DeltaSummAVTSub", ref _DeltaSummAVTSub, value);
                if (!IsLoading) {
                    OnChanged("DeltaSummAllSub");
                }
            }
        }
        /// <summary>
        /// Отклонение Всего
        /// </summary>
        public Decimal DeltaSummAllAdd {
            get { return DeltaSummCostAdd + DeltaSummAVTAdd; }
        }
        /// <summary>
        /// Отклонение Всего
        /// </summary>
        public Decimal DeltaSummAllSub {
            get { return DeltaSummCostSub + DeltaSummAVTSub; }
        }

        /// <summary>
        /// Строчки счета-фактуры
        /// </summary>
        [Aggregated]
        [Association("fmAVTInvoiceVersion-fmAVTInvoiceLine", typeof(fmCAVTInvoiceLine))]
        public XPCollection<fmCAVTInvoiceLine> Lines {
            get { return GetCollection<fmCAVTInvoiceLine>("Lines"); }
        }

        [Action(Caption = "Approve")]
        void Approve() {
            AVTInvoice.Current = this;
        }

    }
}
