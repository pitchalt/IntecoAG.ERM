using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    /// <summary>
    /// Инкассовый ордер
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBInkassOrder : fmCDocRCB
    {
        public fmCDocRCBInkassOrder(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCBInkassOrder);
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
