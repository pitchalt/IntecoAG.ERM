using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.StatementAccount {

    /// <summary>
    /// Реализация ручного распознавания и сопоставления платёжных документов
    /// </summary>
    [NavigationItem("Finance")]
    [NonPersistent]
    public class fmMapDocsManual : csCComponent
    {
        public fmMapDocsManual(Session ses)
            : base(ses) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmMapDocsManual);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private fmCDocRCB _PaymentDocument;   // Платёжный документ, для которого подбирается документ выписки
        private XPCollection<fmCDocRCB> _StatementAccountDocuments;   // Список докуументов выписок

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Платёжный документ, для которого подбирается документ выписки
        /// </summary>
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value);
            }
        }


        /// <summary>
        /// Список документов, полученных по выписке
        /// </summary>
        public XPCollection<fmCDocRCB> StatementAccountDocuments {
            get { return _StatementAccountDocuments; }
            set {
                SetPropertyValue<XPCollection<fmCDocRCB>>("StatementAccountDocuments", ref _StatementAccountDocuments, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        public bool MapDocs() {

            return true;
        }

        #endregion
    }

}
