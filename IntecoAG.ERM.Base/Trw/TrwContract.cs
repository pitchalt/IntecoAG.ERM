using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.References;
using IntecoAG.ERM.Trw.Subject;
using IntecoAG.ERM.Trw.Exchange;
//
namespace IntecoAG.ERM.Trw {

    [NavigationItem("Trw")]
    [Persistent("TrwContract")]
    [DefaultProperty("Name")]
    public class TrwContract : BaseObject, TrwIContract {

//        private fmCSubject _Subject;
//        public fmCSubject Subject {
//            get { return _Subject; }
//            set {
//                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
//                if (!IsLoading && value != null) {
//                    TrwNumber = "D" + value.TrwCode;
//                    TrwDate = value.DateBegin;
//                }
//            }
//        }

        [Persistent("TrwExportState")]
        private TrwExchangeExportStates _TrwExportState;
        [PersistentAlias("_TrwExportState")]
        public TrwExchangeExportStates TrwExportState {
            get { return _TrwExportState; }
        }
        public void TrwExportStateSet(TrwExchangeExportStates state) {
            SetPropertyValue<TrwExchangeExportStates>("TrwExportState", ref _TrwExportState, state);
        }

        public String Name {
            get {
                return TrwNumber + " " + TrwDate.ToString("D");
            }
        }

        public TrwContractSuperType ContractSuperType;

        private String _TrwNumber;
        public String TrwNumber {
            get { return _TrwNumber; }
            set { SetPropertyValue<String>("TrwNumber", ref _TrwNumber, value); }
        }

        private DateTime _TrwDate;
        public DateTime TrwDate {
            get { return _TrwDate; }
            set { SetPropertyValue<DateTime>("TrwDate", ref _TrwDate, value); }
        }

        public TrwIContractParty TrwCustomerParty {
            get { return null; }
        }

        public TrwIContractParty TrwSupplierParty {
            get { return null; }
        }

        public TrwIPerson TrwPartyPerson {
            get { return PartyPerson; }
        }

        private crmCPerson _PartyPerson;
        public crmCPerson PartyPerson {
            get { return _PartyPerson; }
            set { SetPropertyValue<crmCPerson>("PartyPerson", ref _PartyPerson, value); }
        }

        private TrwContractMarket _TrwContractMarket;
        public TrwContractMarket TrwContractMarket {
            get { return _TrwContractMarket; }
            set { SetPropertyValue<TrwContractMarket>("TrwContractMarket", ref _TrwContractMarket, value); }
        }

 //       public TrwRefCNM _TrwContractMarket;
 //       public TrwRefCNM TrwContractMarket {
 //           get { return _TrwContractMarket; }
 //           set { SetPropertyValue<TrwRefCNM>("TrwContractMarket", ref _TrwContractMarket, value); }
 //       }

        public IList<TrwIOrder> TrwSaleOrders {
            get { return new ListConverter<TrwIOrder, TrwOrder>(TrwOrders); }
        }

        [Association("TrwContract-TrwOrder")]
        [Browsable(false)]
        public XPCollection<TrwOrder> TrwOrders {
            get { return GetCollection<TrwOrder>("TrwOrders"); }
        } 

        private String _TrwSubject;
        public String TrwSubject {
            get { return _TrwSubject; }
            set { SetPropertyValue<String>("TrwSubject", ref _TrwSubject, value); }
        }

        private DateTime _TrwDateSigning;
        public DateTime TrwDateSigning {
            get { return _TrwDateSigning; }
            set { SetPropertyValue<DateTime>("TrwDateSigning", ref _TrwDateSigning, value); }
        }

        private DateTime _TrwDateValidFrom;
        public DateTime TrwDateValidFrom {
            get { return _TrwDateValidFrom; }
            set { SetPropertyValue<DateTime>("TrwDateValidFrom", ref _TrwDateValidFrom, value); }
        }

        private DateTime _TrwDateValidToPlan;
        public DateTime TrwDateValidToPlan {
            get { return _TrwDateValidToPlan; }
            set { SetPropertyValue<DateTime>("TrwDateValidToPlan", ref _TrwDateValidToPlan, value); }
        }

        private DateTime _TrwDateValidToFact;
        public DateTime TrwDateValidToFact {
            get { return _TrwDateValidToFact; }
            set { SetPropertyValue<DateTime>("TrwDateValidToFact", ref _TrwDateValidToFact, value); }
        }

        private csValuta _TrwObligationCurrency;
        public csValuta TrwObligationCurrency {
            get { return _TrwObligationCurrency; }
            set { SetPropertyValue<csValuta>("TrwObligationCurrency", ref _TrwObligationCurrency, value); }
        }

        private Decimal _TrwObligationSumma;
        public Decimal TrwObligationSumma {
            get { return _TrwObligationSumma; }
            set { SetPropertyValue<Decimal>("TrwObligationSumma", ref _TrwObligationSumma, value); }
        }

        private csValuta _TrwPaymentCurrency;
        public csValuta TrwPaymentCurrency {
            get { return _TrwPaymentCurrency; }
            set { SetPropertyValue<csValuta>("TrwPaymentCurrency", ref _TrwPaymentCurrency, value); }
        }

        private csNDSRate _TrwVATRate;
        public csNDSRate TrwVATRate {
            get { return _TrwVATRate; }
            set { SetPropertyValue<csNDSRate>("TrwVATRate", ref _TrwVATRate, value); }
        }

        public TrwContract(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            _TrwExportState = TrwExchangeExportStates.CREATED;
        }

        //public IBindingList Children {
        //    get { return new BindingList<TrwIOrder>(TrwSaleOrders); }
        //}

        //public ITreeNode Parent {
        //    get { return null; }
        //}
    }

}
