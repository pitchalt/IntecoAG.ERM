using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.ERM.FM.PaymentRequest {
    /// <summary>
    /// Вариант оплаты
    /// </summary>
    public enum fmPRPayType {
        /// <summary>
        /// Предоплата
        /// </summary>
        PREPAYMENT = 1,
        /// <summary>
        /// Постоплата
        /// </summary>
        POSTPAYMENT = 2
    }
}
