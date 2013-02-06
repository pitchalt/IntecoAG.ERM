using System;
//
using DevExpress.Xpo;
//

namespace IntecoAG.ERM.FM.Docs {

    // http://www.docstandard.com/obrazcy/tomvk/obrazec-xoxm2s.htm

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCDocRCBPaymentRequest : fmCDocRCB 
    {
        public fmCDocRCBPaymentRequest(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCBPaymentOrder);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private String _Accepter; // Акцептор
        private DateTime _AccepterDate;    // Дата акцепта

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Акцептор
        /// </summary>
        [Size(150)]
        public String Accepter {
            get { return _Accepter; }
            set {
                SetPropertyValue<String>("Accepter", ref _Accepter, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Дата акцепта
        /// </summary>
        public DateTime AccepterDate {
            get { return _AccepterDate; }
            set {
                SetPropertyValue<DateTime>("AccepterDate", ref _AccepterDate, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
