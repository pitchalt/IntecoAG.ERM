using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    /// <summary>
    /// Прочие документы - кроме вот этих: платежное поручение, платежное требование, заявление на аккредитив, инкассовое поручение
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBOthers : fmCDocRCB
    {
        public fmCDocRCBOthers(Session session)
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
