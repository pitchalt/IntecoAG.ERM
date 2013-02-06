using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBPaymentOrder : fmCDocRCB
    {
        public fmCDocRCBPaymentOrder(Session session)
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
