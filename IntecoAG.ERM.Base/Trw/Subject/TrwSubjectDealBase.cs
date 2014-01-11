using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.Trw.Subject {

    public enum TrwSubjectDealType { 
        TRW_SUBJECT_DEAL_REAL = 1,
        TRW_SUBJECT_DEAL_CONS_DEAL = 16,
        TRW_SUBJECT_DEAL_CONS_PARTNER = 17,
        TRW_SUBJECT_DEAL_CONS_OTHER = 18
    }

    [Persistent("TrwSubjectDeal")]
    [Appearance("", AppearanceItemType.ViewItem, "DealType == 'TRW_SUBJECT_DEAL_REAL'", TargetItems = "PersonInternal,CrmContractDeals,DealBudget", Enabled = false)]
    public abstract class TrwSubjectDealBase : XPObject {

        private TrwSubjectDealType _DealType;
        public TrwSubjectDealType DealType {
            get { return _DealType; }
            set { 
                SetPropertyValue<TrwSubjectDealType>("DealType", ref _DealType, value);
                if (!IsLoading) {
                    switch (value) { 
                        case TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL:
                            CrmContractDeals.Clear();
                            break;
                        case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_DEAL:
                            UpdateConsolidateDeal();
                            break;
                        case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_PARTNER:
                            CrmContractDeals.Clear();
                            UpdateConsolidateDeal();
                            break;
                        case TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_OTHER:
                            CrmContractDeals.Clear();
                            UpdateConsolidateDeal();
                            break;

                    }
                }
            }
        }

        public virtual void UpdateConsolidateDeal(Boolean force = false) {
            if (DealBudget == null || force) {
                if (DealBudget == null)
                    DealBudget = new TrwContract(this.Session);
                DealBudget.TrwNumber = "DC" + TrwSubjectBase.Code + TrwSubjectBase.SubjectDealNumberGet().ToString("D3");
                DealBudget.TrwDate = new DateTime(TrwSubjectBase.Period.Year, 1, 1);
                DealBudget.TrwDateSigning = DealBudget.TrwDate;
                DealBudget.TrwDateValidFrom = DealBudget.TrwDate;
                DealBudget.TrwDateValidToPlan = new DateTime(TrwSubjectBase.Period.Year + 1, 1, 1);
            }
        }

        public IList<TrwIOrder> TrwOrders {
            get {
                return DealBudget != null ? 
                    new ListConverter<TrwIOrder, TrwOrder>( DealBudget.TrwOrders) : 
                    new ListConverter<TrwIOrder, TrwOrder>( Deal.TrwOrders);
            }
        }

        [NonPersistent]
        public abstract TrwSubject TrwSubjectBase { get; } 

        private crmContractDeal _Deal;
//        [DataSourceProperty("DealSource")]
        public crmContractDeal Deal {
            get { return _Deal; }
            set { 
                SetPropertyValue<crmContractDeal>("Deal", ref _Deal, value);
                if (!IsLoading) {
                    if (value != null) {
                        DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_REAL;
                        CrmContractDeals.Clear();
                        CrmContractDeals.Add(value);
                    }
                    else {
                        DealType = TrwSubjectDealType.TRW_SUBJECT_DEAL_CONS_PARTNER;
                    }
                }
            }
        }
        [Browsable(false)]
        public abstract XPCollection<crmContractDeal> DealSource { get; }

        public abstract crmCPerson Person { get; }

        private crmCPerson _PersonInternal;
        public crmCPerson PersonInternal {
            get { return _PersonInternal; }
            set { 
                SetPropertyValue<crmCPerson>("PersonInternal", ref _PersonInternal, value);
                if (!IsLoading && value != null) {
                    DealBudget.PartyPerson = value;
                }
            }
        }

        private TrwContract _DealBudget;
//        [DataSourceProperty("DealBudgetSource")]
        public TrwContract DealBudget {
            get { return _DealBudget; }
            set { 
                SetPropertyValue<TrwContract>("DealBudget", ref _DealBudget, value);
                if (!IsLoading)
                    DealBudgetChanged();
            }
        }
        [Browsable(false)]
        public abstract XPCollection<TrwContract> DealBudgetSource { get; }

        protected virtual void DealBudgetChanged() { 

        }

        private fmCSubject _Subject;
        [DataSourceProperty("SubjectSource")]
        public fmCSubject Subject {
            get { return _Subject; }
            set { SetPropertyValue<fmCSubject>("Subject", ref _Subject, value); }
        }

        [Browsable(false)]
        public abstract IList<fmCSubject> SubjectSource { get; }


        public TrwIContract TrwContract {
            get {
                if (Deal != null) {
                    return Deal;
                }
                else {
                    return DealBudget;
                }
            }
        }

        [Browsable(false)]
        [Aggregated]
        [Association("TrwSubjectDealBase-TrwSubjectDealDealLink")]
        public XPCollection<TrwSubjectDealDealLink> DealLinks {
            get { return GetCollection<TrwSubjectDealDealLink>("DealLinks"); }
        }
        [ManyToManyAlias("DealLinks", "CrmContractDeal")]
        public IList<crmContractDeal> CrmContractDeals {
            get { return GetList<crmContractDeal>("CrmContractDeals"); }
        }

        public TrwSubjectDealBase(Session session): base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected override void OnDeleting() {
            base.OnDeleting();
            if (DealBudget != null && DealBudget.TrwExportState != Exchange.TrwExchangeExportStates.CONFIRMED)
                DealBudget.TrwExportStateSet(Exchange.TrwExchangeExportStates.DELETED);
        }

        //public virtual ITreeNode Parent {
        //    get { return null; }  
        //}
        //public abstract IBindingList Children { get; }

        //public virtual String Name {
        //    get { return TrwContract != null ? TrwContract.TrwNumber : null; }
        //}

    }
}
