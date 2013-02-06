using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Заявка на оплату по договору
    
    //[NavigationItem]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [VisibleInReports]
    public class fmCPRPaymentRequestContract : fmCPRPaymentRequest
    {
        public fmCPRPaymentRequestContract(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRPaymentRequestContract);
            this.CID = Guid.NewGuid();
            DayRegisterPart = "1";
        }

        #region ПОЛЯ КЛАССА

        private String _DayRegisterPart;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// День
        /// </summary>
        [Size(3)]
        public String DayRegisterPart {
            get { return _DayRegisterPart; }
            set { SetPropertyValue<String>("DayRegisterPart", ref _DayRegisterPart, value); }
        }

        /// <summary>
        /// Договор
        /// </summary>
        [Delayed]
        public crmContract Contract {
            get {
                return GetDelayedPropertyValue<crmContract>("Contract");
            }
            set {
                SetDelayedPropertyValue<crmContract>("Contract", value);
            }
        }

        /// <summary>
        /// Сделка
        /// </summary>
        [DataSourceProperty("ContractDealSource")]
        [RuleRequiredField]
        [Delayed]
        public crmContractDeal ContractDeal {
            get {
                return GetDelayedPropertyValue<crmContractDeal>("ContractDeal");
            }
            set {
                crmContractDeal old = ContractDeal;
                SetDelayedPropertyValue<crmContractDeal>("ContractDeal", value); 
                if (!IsLoading && value != old) {
                    //this.PartyPayDebitor = value.Supplier;
                    //this.PartyPayCreditor = value.Customer;
                    if (value != null) {
                        this.Curator = value.CuratorDepartment;
                    } else {
                        this.Curator = null;
                    }

                    if (this.PartyPayDebitor == null && this.PartyPayCreditor == null) {
                        this.PartyPayDebitor = value.Supplier;
                        this.PartyPayCreditor = value.Customer;
                    } else if (this.PartyPayDebitor == null && this.PartyPayCreditor != null) {
                        if (this.PartyPayCreditor == value.Customer) {
                            this.PartyPayDebitor = value.Supplier;
                        } else {
                            this.PartyPayDebitor = value.Customer;
                        }
                    } else if (this.PartyPayDebitor != null && this.PartyPayCreditor == null) {
                        if (this.PartyPayDebitor == value.Supplier) {
                            this.PartyPayCreditor = value.Customer;
                        } else {
                            this.PartyPayCreditor = value.Supplier;
                        }
                    } else if (this.PartyPayCreditor != null && this.PartyPayCreditor != null) {
                        // NOP
                    }
                    //if ((old.Supplier == value.Supplier || old.Supplier == value.Customer) && (old.Customer == value.Customer || old.Customer == value.Supplier)) {
                    //    // NOP
                    //}
                }
            }
        }

        /// <summary>
        /// Источник данных сделок
        /// </summary>
        [Browsable(false)]
        public XPCollection<crmContractDeal> ContractDealSource {
            get {

                /*
                CriteriaOperatorCollection oper = new CriteriaOperatorCollection();

                if (this.PartyPayDebitor != null) {
                    oper.Add(new BinaryOperator("Supplier", this.PartyPayDebitor));
                }
                if (this.PartyPayCreditor != null) {
                    oper.Add(new BinaryOperator("Customer", this.PartyPayCreditor));
                }
                if ((object)oper == null) {
                    return new XPCollection<crmContractDeal>(this.Session);
                }
                else {
                    return new XPCollection<crmContractDeal>(this.Session, CriteriaOperator.Or(oper));
                }
                */

                CriteriaOperatorCollection oper = new CriteriaOperatorCollection();

                if (this.PartyPayDebitor != null && this.PartyPayCreditor == null) {
                    oper.Add(new BinaryOperator("Supplier", this.PartyPayDebitor));
                    oper.Add(new BinaryOperator("Customer", this.PartyPayDebitor));
                    return new XPCollection<crmContractDeal>(this.Session, CriteriaOperator.Or(oper));
                } else if (this.PartyPayDebitor == null && this.PartyPayCreditor != null) {
                    oper.Add(new BinaryOperator("Supplier", this.PartyPayCreditor));
                    oper.Add(new BinaryOperator("Customer", this.PartyPayCreditor));
                    return new XPCollection<crmContractDeal>(this.Session, CriteriaOperator.Or(oper));
                } else if (this.PartyPayDebitor != null && this.PartyPayCreditor != null) {
                    CriteriaOperatorCollection oper2 = new CriteriaOperatorCollection();
                    CriteriaOperatorCollection operOr = new CriteriaOperatorCollection();

                    oper.Add(new BinaryOperator("Supplier", this.PartyPayDebitor));
                    oper.Add(new BinaryOperator("Customer", this.PartyPayCreditor));
                    oper2.Add(new BinaryOperator("Customer", this.PartyPayDebitor));
                    oper2.Add(new BinaryOperator("Supplier", this.PartyPayCreditor));
                    operOr.Add(CriteriaOperator.And(oper));
                    operOr.Add(CriteriaOperator.And(oper2));
                    return new XPCollection<crmContractDeal>(this.Session, CriteriaOperator.Or(operOr));
                }
                return new XPCollection<crmContractDeal>(this.Session);
            }
        }

        public override crmCParty PartyPayDebitor {
            get {
                return base.PartyPayDebitor;
            }
            set {
                crmCParty old = base.PartyPayDebitor;
                base.PartyPayDebitor = value;
                if (!IsLoading) {
                    if (ContractDeal == null || (value != ContractDeal.Supplier && value != ContractDeal.Customer)) {
                        ContractDeal = null;
                    }
//                    OnChanged("ContractDeal");
                }
            }
        }

        public override crmCParty PartyPayCreditor {
            get {
                return base.PartyPayCreditor;
            }
            set {
                crmCParty old = base.PartyPayCreditor;
                base.PartyPayCreditor = value;
                if (!IsLoading) {
                    if (ContractDeal == null || (value != ContractDeal.Supplier && value != ContractDeal.Customer)) {
                        ContractDeal = null;
                    }
//                    OnChanged("ContractDeal");
                }
            }
        }

        /// <summary>
        /// Ответственный договорного отдела
        /// </summary>
        [PersistentAlias("FBKManager")]
        [RuleRequiredField]
        [DataSourceProperty("ManagerContractDepartment")]
        public hrmStaff ContractManager {
            get { return FBKManager; }
            set {
                hrmStaff old = FBKManager;
                if (old != value) {
                    FBKManager = value;
                    if (!IsLoading)
                        OnChanged("PlanManager", old, value);
                }
            }
        }

        [Browsable(false)]
        public XPCollection<hrmStaff> ManagerContractDepartment {
            get { return crmCSettingsContract.GetInstance(this.Session).ManagerGroupOfContractStaffs; }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
