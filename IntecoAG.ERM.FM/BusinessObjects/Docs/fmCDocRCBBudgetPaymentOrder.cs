using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    // http://formz.ru/forms/platezhka_tax/info

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBBudgetPaymentOrder : fmCDocRCB
    {
        public fmCDocRCBBudgetPaymentOrder(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCBPaymentOrder);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
