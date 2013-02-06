using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    ///  Класс бизнесс-логики для Заявки на оплату
    /// </summary>
    public static class fmCPRPaymentRequestBusinesLogic
    {
        #region ПОЛЯ КЛАССА

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        /*
        public static void DoTransitAction(IObjectSpace os, fmCPRPaymentRequest paymentRequest, PaymentRequestStates oldStatus, PaymentRequestStates newStatus) {
            switch (oldStatus) {
                case PaymentRequestStates.OPEN:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            paymentRequest.DateContractOrPlan = paymentRequest.Date;
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.REGISTERED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            paymentRequest.DateContractOrPlan = paymentRequest.Date;
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.ACCEPTED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            paymentRequest.DateContractOrPlan = default(DateTime);
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            paymentRequest.DateBudget = DateTime.Now;
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.DELETED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            paymentRequest.Date = DateTime.Now;
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.DECLINED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            paymentRequest.DateContractOrPlan = paymentRequest.Date;
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            paymentRequest.DateFinance = default(DateTime);
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.IN_BUDGET:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            paymentRequest.DateContractOrPlan = default(DateTime);
                            paymentRequest.DateBudget = default(DateTime);
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            paymentRequest.DateFinance = DateTime.Now;
                            // Информация о заявке пишется в специальный регистрирующий журнал
                            fmCPRRegistrator ra = os.CreateObject<fmCPRRegistrator>();
                            ra.PaymentRequest = paymentRequest;
                            ra.IntNumber = fmCPRRegistrator.GenerateNumber(((ObjectSpace)os).Session);
                            paymentRequest.Number = ra.Number;
                            break;
                        case PaymentRequestStates.IN_BANK:
                            //paymentRequest.DateFinance = DateTime.Now;
                            //// Информация о заявке пишется в специальный регистрирующий журнал
                            //fmCPRRegistrator ra = os.CreateObject<fmCPRRegistrator>();
                            //ra.PaymentRequest = paymentRequest;
                            //ra.IntNumber = fmCPRRegistrator.GenerateNumber(((ObjectSpace)os).Session);
                            //paymentRequest.Number = ra.Number;
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.IN_PAYMENT:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            paymentRequest.DateFinance = default(DateTime);
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.IN_BANK:   // то же, что и IN_PAYMENT, но означает, что в банк отправлена платёжка
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            paymentRequest.DateFinance = default(DateTime);
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.PAYED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.SUSPENDED:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            paymentRequest.DateFinance = DateTime.Now;
                            break;
                        case PaymentRequestStates.IN_BANK:
                            //paymentRequest.DateFinance = DateTime.Now;
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:
                    switch (newStatus) {
                        case PaymentRequestStates.OPEN:
                            break;
                        case PaymentRequestStates.REGISTERED:
                            break;
                        case PaymentRequestStates.ACCEPTED:
                            break;
                        case PaymentRequestStates.DECLINED:
                            break;
                        case PaymentRequestStates.DELETED:
                            break;
                        case PaymentRequestStates.IN_BUDGET:
                            break;
                        case PaymentRequestStates.IN_PAYMENT:
                            break;
                        case PaymentRequestStates.IN_BANK:
                            break;
                        case PaymentRequestStates.PAYED:
                            break;
                        case PaymentRequestStates.SUSPENDED:
                            break;
                        case PaymentRequestStates.SUSPENDED_BUDGET:
                            break;
                        case PaymentRequestStates.FINANCE_PAYMENT:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        */

        public static void DoApproveAction(IObjectSpace os, fmCPRPaymentRequest paymentRequest) {
            switch (paymentRequest.State) {
                case PaymentRequestStates.OPEN:
                    paymentRequest.State = PaymentRequestStates.REGISTERED;
                    paymentRequest.DateContractOrPlan = paymentRequest.Date;
                    break;
                case PaymentRequestStates.REGISTERED:
                    if (paymentRequest as fmPaymentRequestMemorandum != null) {
                        paymentRequest.State = PaymentRequestStates.IN_BUDGET;
                        paymentRequest.DateBudget = DateTime.Now;
                    }
                    else {
                        paymentRequest.State = PaymentRequestStates.ACCEPTED;
                        paymentRequest.DateContractOrPlan = paymentRequest.Date;
                    }
                    break;
                case PaymentRequestStates.ACCEPTED:
                    paymentRequest.State = PaymentRequestStates.IN_BUDGET;
                    paymentRequest.DateBudget = DateTime.Now;
                    break;
                case PaymentRequestStates.DELETED:
                    paymentRequest.State = PaymentRequestStates.REGISTERED;
                    paymentRequest.Date = DateTime.Now;
                    break;
                case PaymentRequestStates.DECLINED:
                    paymentRequest.State = PaymentRequestStates.REGISTERED;
                    paymentRequest.DateContractOrPlan = paymentRequest.Date;
                    break;
                case PaymentRequestStates.IN_BUDGET:
                    paymentRequest.State = PaymentRequestStates.IN_PAYMENT;
                    paymentRequest.DateFinance = DateTime.Now;
                    // Информация о заявке пишется в специальный регистрирующий журнал
                    fmCPRRegistrator ra = os.CreateObject<fmCPRRegistrator>();
                    ra.PaymentRequest = paymentRequest;
                    ra.IntNumber = fmCPRRegistrator.GenerateNumber(((ObjectSpace)os).Session);
                    paymentRequest.Number = ra.Number;
                    break;
                case PaymentRequestStates.IN_PAYMENT:
                    paymentRequest.State = PaymentRequestStates.IN_BANK;
                    break;
                case PaymentRequestStates.IN_BANK:   // то же, что и IN_PAYMENT, но означает, что в банк отправлена платёжка
                    break;
                case PaymentRequestStates.PAYED:
                    break;
                case PaymentRequestStates.SUSPENDED:
                    paymentRequest.State = PaymentRequestStates.IN_PAYMENT;
                    paymentRequest.DateFinance = DateTime.Now;
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:
                    paymentRequest.State = PaymentRequestStates.IN_BUDGET;
                    paymentRequest.DateBudget = DateTime.Now;
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:
                    break;
                case PaymentRequestStates.TEMPLATE:
                    break;
                default:
                    break;
            }
        }

        public static void DoDeclineAction(IObjectSpace os, fmCPRPaymentRequest paymentRequest) {
            switch (paymentRequest.State) {
                case PaymentRequestStates.OPEN:
                    paymentRequest.State = PaymentRequestStates.DELETED;
                    break;
                case PaymentRequestStates.REGISTERED:
                    paymentRequest.State = PaymentRequestStates.DELETED;
                    break;
                case PaymentRequestStates.ACCEPTED:
                    paymentRequest.State = PaymentRequestStates.REGISTERED;
                    paymentRequest.DateContractOrPlan = paymentRequest.Date;
                    //paymentRequest.DateBudget = DateTime.Now;
                    break;
                case PaymentRequestStates.DELETED:
                    break;
                case PaymentRequestStates.DECLINED:
                    paymentRequest.State = PaymentRequestStates.DELETED;
                    break;
                case PaymentRequestStates.IN_BUDGET:
                    paymentRequest.State = PaymentRequestStates.DECLINED;
                    break;
                case PaymentRequestStates.IN_PAYMENT:
                    paymentRequest.State = PaymentRequestStates.DECLINED;
                    break;
                case PaymentRequestStates.IN_BANK:   // то же, что и IN_PAYMENT, но означает, что в банк отправлена платёжка
                    break;
                case PaymentRequestStates.PAYED:
                    break;
                case PaymentRequestStates.SUSPENDED:
                    paymentRequest.State = PaymentRequestStates.DECLINED;
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:
                    paymentRequest.State = PaymentRequestStates.DECLINED;
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:
                    paymentRequest.State = PaymentRequestStates.DELETED;
                    break;
                case PaymentRequestStates.TEMPLATE:
                    paymentRequest.State = PaymentRequestStates.DELETED;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Выполнение действия "Отложить"
        /// </summary>
        /// <param name="os"></param>
        /// <param name="paymentRequest"></param>
        /// <param name="strCaption">Надпись на кнопке</param>
        /// <param name="strToolTip">Подсказка на кнопке</param>
        public static void DoSuspendAction(IObjectSpace os, fmCPRPaymentRequest paymentRequest) {
            switch (paymentRequest.State) {
                case PaymentRequestStates.OPEN:
                    break;
                case PaymentRequestStates.REGISTERED:
                    break;
                case PaymentRequestStates.ACCEPTED:
                    break;
                case PaymentRequestStates.DELETED:
                    break;
                case PaymentRequestStates.DECLINED:
                    break;
                case PaymentRequestStates.IN_BUDGET:
                    paymentRequest.State = PaymentRequestStates.SUSPENDED_BUDGET;
                    //paymentRequest.DateFinance = DateTime.Now;
                    break;
                case PaymentRequestStates.IN_PAYMENT:
                    paymentRequest.State = PaymentRequestStates.SUSPENDED;
                    //paymentRequest.DateBudget = DateTime.Now;
                    break;
                case PaymentRequestStates.IN_BANK:   // то же, что и IN_PAYMENT, но означает, что в банк отправлена платёжка
                    break;
                case PaymentRequestStates.PAYED:
                    break;
                case PaymentRequestStates.SUSPENDED:
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:
                    break;
                case PaymentRequestStates.TEMPLATE:
                    break;
                default:
                    break;
            }
        }




        /// <summary>
        /// Определение доступности действия "Утердить" заявку
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public static bool EnableApproveAction(fmCPRPaymentRequest paymentRequest) {
            PaymentRequestStates status = paymentRequest.State;
            bool isEnable = true;

            switch (status) {
                case PaymentRequestStates.OPEN:   // Заявка открыта, то есть создана пользователем и не объявлена им окончательно оформленной
                    isEnable = false;
                    break;
                case PaymentRequestStates.REGISTERED:  // Пользователь завершил формирование заявки
                    isEnable = false;
                    break;
                case PaymentRequestStates.ACCEPTED:
                    isEnable = false;
                    break;
                case PaymentRequestStates.DELETED:
                    isEnable = false;
                    break;
                case PaymentRequestStates.DECLINED:
                    isEnable = true;
                    break;
                case PaymentRequestStates.IN_BUDGET:  // Утверждена Бюджетно-аналитиченским отделом, Учтена в бюджете
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_PAYMENT: // Утверждена Финансовым отделом, т.е. В оплату
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_BANK: // Утверждена Финансовым отделом, т.е. В оплату и в банк отправлена платёжка
                    isEnable = true;
                    break;
                case PaymentRequestStates.PAYED:   // Оплачена. Насколько я понял, ставится после того, как появилась выписка с фактом оплаты.
                    isEnable = true;
                    break;
                case PaymentRequestStates.SUSPENDED:   // Отложена финансовым отделом
                    isEnable = false;
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:   // Отложена бюджетным отделом
                    isEnable = false;
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:   // В результате финансовой операции
                    isEnable = true;
                    break;
                case PaymentRequestStates.TEMPLATE:   // Шаблон
                    isEnable = true;
                    break;
                default:
                    break;
            }

            return isEnable;
        }

        /// <summary>
        /// Определение доступности действия "Отклонить" заявку
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public static bool EnableDeclinAction(fmCPRPaymentRequest paymentRequest) {
            PaymentRequestStates status = paymentRequest.State;
            bool isEnable = true;

            switch (status) {
                case PaymentRequestStates.OPEN:   // Заявка открыта, то есть создана пользователем и не объявлена им окончательно оформленной
                    isEnable = true;
                    break;
                case PaymentRequestStates.REGISTERED:  // Пользователь завершил формирование заявки
                    isEnable = false;
                    break;
                case PaymentRequestStates.ACCEPTED:
                    isEnable = false;
                    break;
                case PaymentRequestStates.DELETED:
                    isEnable = true;
                    break;
                case PaymentRequestStates.DECLINED:
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_BUDGET:  // Утверждена Бюджетно-аналитиченским отделом, Учтена в бюджете
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_PAYMENT: // Утверждена Финансовым отделом, т.е. В оплату
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_BANK: // Утверждена Финансовым отделом, т.е. В оплату и в банк отправлена платёжка
                    isEnable = true;
                    break;
                case PaymentRequestStates.PAYED:   // Оплачена. Насколько я понял, ставится после того, как появилась выписка с фактом оплаты.
                    isEnable = true;
                    break;
                case PaymentRequestStates.SUSPENDED:   // Отложена финансовым отделом
                    isEnable = false;
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:   // Отложена бюджетным отделом
                    isEnable = false;
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:   // В результате финансовой операции
                    isEnable = false;
                    break;
                case PaymentRequestStates.TEMPLATE:   // Шаблон
                    isEnable = false;
                    break;
                default:
                    break;
            }

            return isEnable;
        }

        /// <summary>
        /// Определение доступности действия "Отложить" заявку
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public static bool EnableSuspendAction(fmCPRPaymentRequest paymentRequest) {
            PaymentRequestStates status = paymentRequest.State;
            bool isEnable = true;

            switch (status) {
                case PaymentRequestStates.OPEN:   // Заявка открыта, то есть создана пользователем и не объявлена им окончательно оформленной
                    isEnable = true;
                    break;
                case PaymentRequestStates.REGISTERED:  // Пользователь завершил формирование заявки
                    isEnable = true;
                    break;
                case PaymentRequestStates.ACCEPTED:
                    isEnable = true;
                    break;
                case PaymentRequestStates.DELETED:
                    isEnable = true;
                    break;
                case PaymentRequestStates.DECLINED:
                    isEnable = true;
                    break;
                case PaymentRequestStates.IN_BUDGET:  // Утверждена Бюджетно-аналитиченским отделом, Учтена в бюджете
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_PAYMENT: // Утверждена Финансовым отделом, т.е. В оплату
                    isEnable = false;
                    break;
                case PaymentRequestStates.IN_BANK: // Утверждена Финансовым отделом, т.е. В оплату и в банк отправлена платёжка
                    isEnable = true;
                    break;
                case PaymentRequestStates.PAYED:   // Оплачена. Насколько я понял, ставится после того, как появилась выписка с фактом оплаты.
                    isEnable = true;
                    break;
                case PaymentRequestStates.SUSPENDED:   // Отложена финансовым отделом
                    isEnable = true;
                    break;
                case PaymentRequestStates.SUSPENDED_BUDGET:   // Отложена бюджетным отделом
                    isEnable = true;
                    break;
                case PaymentRequestStates.FINANCE_PAYMENT:   // В результате финансовой операции
                    isEnable = true;
                    break;
                case PaymentRequestStates.TEMPLATE:   // Шаблон
                    isEnable = true;
                    break;
                default:
                    break;
            }

            return isEnable;
        }


        public static fmCPRPaymentRequest CreateRequest(IObjectSpace workOS, fmCPRRepaymentTask repaymentTask, Type objType, FinRequestKind requestKind) {

            if (repaymentTask == null)
                return null;

            fmCPRPaymentRequest req = workOS.CreateObject(objType) as fmCPRPaymentRequest;
            if (req != null) {
                req.State = PaymentRequestStates.OPEN;
                //req.ExtDocDate = DateTime.Now;
                req.Date = DateTime.Now;
                if (repaymentTask.PaymentDocument != null) {
                    fmCDocRCB doc = repaymentTask.PaymentDocument;
                    fmCDocRCB nDoc = workOS.GetObject<fmCDocRCB>(doc);
                    if (nDoc.PaymentPayerRequisites != null)
                        req.PartyPaySender = nDoc.PaymentPayerRequisites.Party;
                    if (nDoc.PaymentReceiverRequisites != null)
                        req.PartyPayReceiver = nDoc.PaymentReceiverRequisites.Party;
                    //                    req.Number = nDoc.DocNumber;
                    req.ExtDocNumber = nDoc.DocNumber;
                    req.ExtDocDate = nDoc.DocDate;
                    //                    req.Date = DateTime.Now;
                    req.Summ = nDoc.PaymentCost;   // По умолчанию указывваем полную сумму платёжного документа
                    req.PayDate = nDoc.GetAccountDateChange();
                    req.PaymentValuta = nDoc.GetAccountValuta();
                    //req.Valuta = nDoc.GetAccountValuta();   // По умолчания указываем валюту платежа в качестве валюты обязательств
                    req.Comment = nDoc.PaymentFunction;

                    // Случай финансовой заявки
                    fmCPRPaymentRequestFinOrder reqFin = req as fmCPRPaymentRequestFinOrder;
                    if (reqFin != null) {
                        reqFin.FinanceRequestKind = requestKind;

                        if (requestKind == FinRequestKind.BANK_COMISSION || requestKind == FinRequestKind.PAYMENT_PERCENTS) {
                            if (nDoc.PaymentReceiverRequisites.Party == null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount.Bank != null) {
                                nDoc.PaymentReceiverRequisites.Party = nDoc.PaymentReceiverRequisites.BankAccount.Bank.Party;
                            }
                        }
                        if (requestKind == FinRequestKind.RECEIVING_PERCENTS) {
                            if (nDoc.PaymentPayerRequisites.Party == null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount.Bank != null) {
                                nDoc.PaymentPayerRequisites.Party = nDoc.PaymentPayerRequisites.BankAccount.Bank.Party;
                            }
                        }
                        if (requestKind == FinRequestKind.PURSHASE_CURRENCY || requestKind == FinRequestKind.SALE_CURRENCY) {
                            if (repaymentTask.BankAccount == nDoc.PaymentPayerRequisites.BankAccount) {
                                if (nDoc.PaymentReceiverRequisites.Party == null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount.Bank != null) {
                                    nDoc.PaymentReceiverRequisites.Party = nDoc.PaymentReceiverRequisites.BankAccount.Bank.Party;
                                }
                            } else if (repaymentTask.BankAccount == nDoc.PaymentReceiverRequisites.BankAccount) {
                                if (nDoc.PaymentPayerRequisites.Party == null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount.Bank != null) {
                                    nDoc.PaymentPayerRequisites.Party = nDoc.PaymentPayerRequisites.BankAccount.Bank.Party;
                                }
                            }
                        }

                        reqFin.SetSum(nDoc.PaymentCost);

                        reqFin.State = PaymentRequestStates.FINANCE_PAYMENT;   // ПОзволено редактирование Заказа, статьи, суммы
                        //nDoc.State = PaymentDocProcessingStates.PROCESSED;
                    }
                }
            }
            return req;
        }

        static public IList<csCSecurityRole> GetActualRoles(IObjectSpace os, csCSecurityUser user) {
            return fmPaymentRequestMemorandum.GetActualRoles(((ObjectSpace)os).Session, os.GetObject<csCSecurityUser>(user));
        }
        #endregion

    }

}
