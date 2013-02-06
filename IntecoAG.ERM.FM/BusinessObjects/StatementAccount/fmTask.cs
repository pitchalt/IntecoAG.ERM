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
    /// Класс для поддержки задач (возможно переделать в интерфейс, перенести в Base.Impl.CS.Task)
    /// </summary>
    [Persistent("fmTask")]
    public abstract class fmTask : csCComponent
    {
        public fmTask(Session session)
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

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Выполнение задачи
        /// </summary>
        /// <returns></returns>
        public abstract object ExecuteTask();

        #endregion
    }

}
