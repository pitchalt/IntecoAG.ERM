using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.XAFExt.CDS;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
//
namespace IntecoAG.ERM.GCR.Codif {

    [NonPersistent]
    public class gcrCodifDeal: LinqQuery<gcrCodifDeal, crmContractDeal>   { 

        public gcrCodifDeal(Session ses): base(ses) { }

        public gcrCodifDeal(Session ses, crmContract contract,  crmCParty our_party, crmContractDeal deal) : base(ses) {
            _Contract = contract;
            _Deal = deal;
            crmCParty cust_party = null;
            crmCParty supl_party = null;
            if (deal.Current.Customer != null)
                cust_party = deal.Current.Customer.Party;
            if (deal.Current.Supplier != null)
                supl_party = deal.Current.Supplier.Party;
            if (cust_party != our_party)
                _Party = cust_party;
            else
                _Party = supl_party;
        }

        private crmCParty _Party;
        private crmContract _Contract;
        private crmContractDeal _Deal;

        public crmContract Contract {
            get { return _Contract; }
        }

        public crmContractDeal Deal {
            get { return _Deal; }
        }

        public String ContractNumberInternal {
            get { return String.Empty; }
        }

        public String ContractNumberOrganization {
            get {
                if (Contract.ContractDocument != null)
                    return Contract.ContractDocument.Number;
                else
                    return String.Empty;
            }
        }

        public String ContractNumberIn {
            get {
                return ContractNumberOrganization;
            }
        }

        public crmCParty Party {
            get { return _Party; }
        }

        public String PartyINN {
            get { return Party != null ? Party.INN : String.Empty; }
        }
        public String PartyKPP {
            get { return Party != null ? Party.KPP : String.Empty; }
        }
        public String PartyName {
            get { return Party != null ? Party.Name : String.Empty; }
        }

        public String GeneralCustomer {
            get { return String.Empty; }
        }

        public String ContractCategory {
            get { return String.Empty; }
        }

        public String ContractDescription {
            get { return Deal.Current.DescriptionShort; }
        }

        public DateTime ContractDateSign {
            get { return ContractDateLegal; }
        }

        public DateTime ContractDateLegal {
            get {
                if (Contract.ContractDocument != null)
                    return Contract.ContractDocument.Date;
                else
                    return default(DateTime);
            }
        }

        public DateTime DealDateBegin {
            get {
                return Deal.Current.DateBegin;
            }
        }

        public DateTime DealDateEnd {
            get {
                return Deal.Current.DateBegin;
            }
        }

        public csValuta DealValutaObligation {
            get {
                return Deal.Current.Valuta;
            }
        }

        public csValuta DealValutaPayments {
            get {
                return Deal.Current.PaymentValuta;
            }
        }

        public Decimal DealPrice {
            get {
                return Deal.Current.Price;
            }
        }

        public csNDSRate NDSRate {
            get {
                return Deal.Current.NDSRate;
            }
        }

        public fmCOrder Order {
            get {
                return Deal.Current.Order;
            }
        }

        public String AppendixNumber {
            get {
                if (Deal.ContractDocument != Contract.ContractDocument)
                    return Deal.ContractDocument.Number;
                else
                    return String.Empty;
            }
        }
        public String AppendixDescription {
            get {
                if (Deal.ContractDocument != Contract.ContractDocument)
                    return Deal.Current.DescriptionShort;
                else
                    return String.Empty;
            }
        }
        public DateTime AppendixDate {
            get {
                if (Deal.ContractDocument != Contract.ContractDocument)
                    return Deal.ContractDocument.Date;
                else
                    return default(DateTime);
            }
        }
        public String AppendixType {
            get {
                if (Deal.ContractDocument != Contract.ContractDocument)
                    return String.Empty;
                else
                    return String.Empty;
            }
        }
        public String AppendixComment {
            get { 
                return String.Empty; 
            }
        }
        public override IQueryable<gcrCodifDeal> GetQuery() {
//            XPQuery<crmContractDeal> deals = new XPQuery<crmContractDeal>(ses);
            crmCParty user_party = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    user_party = (crmCParty)crmUserParty.CurrentUserPartyGet(this.session).Party;
                }
            }

            var query = from ldeal in Provider
//                        where ldeal.Current.Customer.Party == user_party ||
//                              ldeal.Current.Supplier.Party == user_party 
                        select new gcrCodifDeal(this.session, ldeal.Contract,
                            user_party, 
                            ldeal);
            return query;
        }
//
        public override IEnumerator<gcrCodifDeal> GetEnumerator() {
            crmCParty user_party = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    user_party = (crmCParty)crmUserParty.CurrentUserPartyGet(this.session).Party;
                }
            }

            XPCollection<crmContractDeal> deals = new XPCollection<crmContractDeal>(
                PersistentCriteriaEvaluationBehavior.BeforeTransaction, this.session,
                GroupOperator.Or(
                    new BinaryOperator("Current.Customer.Party", user_party, BinaryOperatorType.Equal),
                    new BinaryOperator("Current.Supplier.Party", user_party, BinaryOperatorType.Equal)
                )
            );
//            IList<gcrCodifDeal> result = new List<gcrCodifDeal>(10000);
//            foreach (crmContractDeal ldeal in deals) {
//                result.Add(new gcrCodifDeal(this.session, ldeal.Contract, user_party, ldeal));
//            }
            //            foreach ()
//            return GetQuery().GetEnumerator();
            return deals.Select<crmContractDeal, gcrCodifDeal>(
                    x => new gcrCodifDeal(this.session, x.Contract, user_party, x)
                ).GetEnumerator();
        }

    }
}
