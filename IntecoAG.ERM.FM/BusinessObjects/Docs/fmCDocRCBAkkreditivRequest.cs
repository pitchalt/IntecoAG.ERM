using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    /// <summary>
    /// Заявка на акрредитив
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBAkkreditivRequest : fmCDocRCB
    {
        public fmCDocRCBAkkreditivRequest(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCBAkkreditivRequest);
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
