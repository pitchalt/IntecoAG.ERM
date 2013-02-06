using System;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Security;

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Вид Заявки, отражающий комиссии банка (мемориальные, банковские ордера) и т.п.

    /// <summary>
    /// Виды финансовых заявок
    /// </summary>
    public enum FinRequestKind {
        /// <summary>
        /// Покупка/продажа валюты
        /// </summary>
        PURSHASE_CURRENCY = 1,
        /// <summary>
        /// Покупка/продажа валюты
        /// </summary>
        SALE_CURRENCY = 2,
        /// <summary>
        /// Внутренний перевод денег (с одного счёта на другой)
        /// </summary>
        INTERNAL_TRANSFER_MONEY = 3,
        /// <summary>
        /// Уплата процентов
        /// </summary>
        PAYMENT_PERCENTS = 4,
        /// <summary>
        /// Получение процентов
        /// </summary>
        RECEIVING_PERCENTS = 5,
        /// <summary>
        /// Комиссия банка
        /// </summary>
        BANK_COMISSION = 6
    }


    //[NavigationItem]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCPRPaymentRequestFinOrder : fmCPRPaymentRequest
    {
        public fmCPRPaymentRequestFinOrder(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRPaymentRequestFinOrder);
            this.CID = Guid.NewGuid();

            //this.State = PaymentRequestStates.PAYED;

            fmCPRPaymentRequestObligation finObligation = new fmCPRPaymentRequestObligation(Session);
            this.PaySettlmentOfObligations.Add(finObligation);

            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            csCSecurityUser user1 = SessionHelper.GetObjectInSession<csCSecurityUser>(user, Session);
            if (user1 != null && user1.Staff != null) {
                this.FBKManager = user1.Staff; //.Department;
                this.DepartmentOfState = user1.Staff.Department;
            }

        }

        #region ПОЛЯ КЛАССА

        //private String _Subject;   // Предмет оплаты, описание
        //private crmCParty _Receiver;   // Получатель
        //private crmBank _ReceiverBank;   // Банк получателя
        //private crmBankAccount _ReceiverBankAccount;   // Счёт получателя

        //private fmCDocRCBOthers _PaymentOtherDocument;
        
        private fmCOrderExt _Order;
        private fmCostItem _CostItem;

        private FinRequestKind _FinanceRequestKind;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Предмет оплаты, описание
        /// </summary>
        public FinRequestKind FinanceRequestKind {
            get {
                return _FinanceRequestKind;
            }
            set {
                SetPropertyValue<FinRequestKind>("FinanceRequestKind", ref _FinanceRequestKind, value);
            }
        }


        ///// <summary>
        ///// Сумма
        ///// </summary>
        //public Decimal Sum {
        //    get {
        //        Decimal sum = 0m;
        //        if (!IsLoading) {
        //            if (this.PaySettlmentOfObligations.Count() > 0) {
        //                sum = this.PaySettlmentOfObligations[0].Summ;
        //            }
        //        }
        //        return sum;
        //    }
        //}

        /// <summary>
        /// Заказ
        /// </summary>
        public fmCOrderExt Order {
            get {
                return _Order;
            }
            set {
                fmCOrderExt old = _Order;
                if (value != old) {
                    _Order = value;
                    if (!IsLoading) {
                        if (this.PaySettlmentOfObligations.Count() > 0) {
                            this.PaySettlmentOfObligations[0].Order = value;
                        }
                        OnChanged("Order", old, value);
                    }

                }
            }
        }

        /// <summary>
        /// Заказ
        /// </summary>
        public fmCostItem CostItem {
            get {
                return _CostItem;
            }
            set {
                fmCostItem old = _CostItem;
                if (value != old) {
                    _CostItem = value;
                    if (!IsLoading) {
                        if (this.PaySettlmentOfObligations.Count() > 0) {
                            this.PaySettlmentOfObligations[0].CostItem = value;
                        }
                        OnChanged("CostItem", old, value);
                    }

                }
            }
        }

        #endregion

        #region МЕТОДЫ

        public void SetSum(Decimal Sum) {
            // Условие контракта: this.PaySettlmentOfObligations.Count() > 0
            this.PaySettlmentOfObligations[0].Summ = Sum;
        }

        public void SetOrder(fmCOrderExt Order) {
            // Условие контракта: this.PaySettlmentOfObligations.Count() > 0
            this.PaySettlmentOfObligations[0].Order = Order;
        }

        public void SetCostItem(fmCostItem CostItem) {
            // Условие контракта: this.PaySettlmentOfObligations.Count() > 0
            this.PaySettlmentOfObligations[0].CostItem = CostItem;
        }

        //private void SetParty(FinRequestKind RequestKind) {
        //    switch (RequestKind) {
        //        case FinRequestKind.PAYMENT_PERCENTS :
        //        case FinRequestKind.BANK_COMISSION :
        //            // Уплата денег банку как организации
        //            this.PartyPayReceiver = this.
        //            break;
        //        case FinRequestKind.RECEIVING_PERCENTS :
        //            // Получение денег от банка как от организации
        //            break;
        //    }
        //}

        #endregion

    }

}
