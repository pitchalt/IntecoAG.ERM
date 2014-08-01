using System;
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.CRM {

    public enum crmActState { 
        ACT_NEW = 1,
        ACT_CONFIRMED = 2,
        ACT_DELETED = 3,
        ACT_DECLINE = 4
    }

    [Persistent("crmActLine")]
    public class crmAct_Line : XPObject {
        [Association("CrmAct-CrmActLine")]
        public crmAct Act;
        //
        private crmStage _Stage;
        [DataSourceProperty("Act.Deal.Current.StageStructure.Stages")]
        public crmStage Stage {
            get { return _Stage; }
            set { SetPropertyValue<crmStage>("Stage", ref _Stage, value); }
        }
//    {get { Act.Deal.Current.StageStructure.Stages}}
        public csNomenclature Nomenclature;
        public csValuta ObligationValuta;
        public Decimal ObligationSumm;
        public csValuta BalanceValuta;
        public Decimal BalanceSumm;

        public crmAct_Line(Session session) : base(session) { }
    }

    [Persistent("crmAct")]
    [NavigationItem("Contract")]
    [VisibleInReports]
    public class crmAct : BaseObject {

        private crmActState _State;
        public crmActState State {
            get { return _State; }
            set {
                SetPropertyValue<crmActState>("State", ref _State, value);
                if (!IsLoading) { 
                    
                }
            }
        }

        private String _Number;
        public String Number {
            get { return _Number; }
            set { SetPropertyValue<String>("Number", ref _Number, value); }
        }

        private Boolean _IsNotNumber;
        public Boolean IsNotNumber {
            get { return _IsNotNumber; }
            set { SetPropertyValue<Boolean>("IsNotNumber", ref _IsNotNumber, value); }
        }

        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
        }

        private crmCParty _Customer;
        public crmCParty Customer {
            get { return _Customer; }
            set {
                SetPropertyValue<crmCParty>("Customer", ref _Customer, value);
                if (!IsLoading) {
                    if (Deal != null && value != null && Deal.Customer != value)
                        Deal = null;
                    DealSourceRefresh();
                }
            }
        }

        private crmCParty _Supplier;
        public crmCParty Supplier {
            get { return _Supplier; }
            set { 
                SetPropertyValue<crmCParty>("Supplier", ref _Supplier, value);
                if (!IsLoading) {
                    if (Deal != null && value != null && Deal.Supplier != value)
                        Deal = null;
                    DealSourceRefresh();
                }
            }
        }

//        private Boolean _IsDealSet;
        private crmContractDeal _Deal;
        [Association("CrmContractDeal-CrmAct")]
        [DataSourceProperty("DealSource")]
        public crmContractDeal Deal {
            get { return _Deal; }
            set { 
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
                if (!IsLoading) { 
                    if (value != null) {
//                        _IsDealSet = true;
                        SetPropertyValue<crmCParty>("Customer", ref _Customer,  value.Customer);
                        SetPropertyValue<crmCParty>("Supplier", ref _Supplier,  value.Supplier);
//                        _IsDealSet = false;
                        DealSourceRefresh();
                    }
                }
            }
        }
        private XPCollection<crmContractDeal> _DealSource;
        [Browsable(false)]
        public XPCollection<crmContractDeal> DealSource {
            get {
                if (_DealSource == null) {
                    _DealSource = new XPCollection<crmContractDeal>(this.Session);
                    DealSourceRefresh();
                }
                return _DealSource;
            }
        }
        protected void DealSourceRefresh() {
            if (_DealSource == null)
                return;
//            _DealSource.LoadingEnabled = false;
            if (Customer == null && Supplier == null)
                _DealSource.Criteria = null;
            if (Customer != null && Supplier == null)
                _DealSource.Criteria = new BinaryOperator("Customer", Customer) |
                    new BinaryOperator("Supplier", Customer);
            if (Customer == null && Supplier != null)
                _DealSource.Criteria = new BinaryOperator("Customer", Supplier) |
                    new BinaryOperator("Supplier", Supplier);
            if (Customer != null && Supplier != null)
                _DealSource.Criteria = new BinaryOperator("Customer", Customer) & new BinaryOperator("Supplier", Supplier) |
                new BinaryOperator("Customer", Supplier) & new BinaryOperator("Supplier", Customer);
        }

        [Association("CrmAct-CrmActLine")]
        [Aggregated]
        public XPCollection<crmAct_Line> Lines {
            get { return GetCollection<crmAct_Line>("Lines"); }
        }

        public crmAct(Session session): base(session) {}

        public override void AfterConstruction() {
            base.AfterConstruction();
            State = crmActState.ACT_NEW;
        }
    }
}
