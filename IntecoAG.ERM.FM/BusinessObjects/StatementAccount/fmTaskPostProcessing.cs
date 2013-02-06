using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// fmTaskPostProcessing - задача сопоставления прототипов платёжных документов с полученными по выписке
    /// </summary>
    [NavigationItem("Finance")]
    [DefaultProperty("Name")]
    [NonPersistent]
    public class fmTaskPostProcessing : csCCodedComponent
    {
        public fmTaskPostProcessing(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmTaskImporter);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        public XPCollection<fmCDocRCB> Prototype {
            get {
                XPCollection<fmCDocRCB> prototype = new XPCollection<fmCDocRCB>(this.Session);
                if (!prototype.IsLoaded) prototype.Load();
                return prototype;
            }
        }

        public XPCollection<fmCDocRCB> Imported {
            get {
                XPCollection<fmCDocRCB> imported = new XPCollection<fmCDocRCB>(this.Session);
                if (!imported.IsLoaded) imported.Load();
                return imported;
            }
        }



        #endregion



        #region МЕТОДЫ

        /// <summary>
        /// Выполнение задачи (Вызов метода импорта)
        /// </summary>
        /// <returns></returns>
        public virtual object ExecuteTask() {
            if (Importer != null) {
                return Importer.Import(this);
            }
            throw new Exception("Import procedure not found");
        }

        #endregion
    }

}
