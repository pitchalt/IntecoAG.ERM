using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;

namespace IntecoAG.ERM.FM.StatementAccount {

    // Содержимое файла и некоторые его характеристики

    [Persistent("fmStatementAccountFileContent")]
    public class fmStatementAccountFileContent : BaseObject   //csCComponent
    {
        public fmStatementAccountFileContent(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //this.ComponentType = typeof(fmStatementAccountFileContent);
            //this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private String _FileName;   // наименование файла
        private DateTime _FileDate;    // Дата и время файла
        private String _FileBody;   // содержимое файла выписки
        private Boolean _IsImported;   // Признак, что файл успешно импортирован
        private DateTime _ImportDate;   // Дата и время полседней попытки импорта
        private String _Hash;   // Хэш файла

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// FileName - наименование файла
        /// </summary>
        public String FileName {
            get { return _FileName; }
            set {
                SetPropertyValue<String>("FileName", ref _FileName, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// FileDate - Дата и время файла
        /// </summary>
        public DateTime FileDate {
            get { return _FileDate; }
            set {
                SetPropertyValue<DateTime>("FileDate", ref _FileDate, value);
            }
        }

        /// <summary>
        /// FileBody - содержимое файла выписки
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String FileBody {
            get { return _FileBody; }
            set {
                SetPropertyValue<String>("FileBody", ref _FileBody, value);
            }
        }

        /// <summary>
        /// IsImported - Признак, что файл успешно импортирован
        /// </summary>
        public Boolean IsImported {
            get { return _IsImported; }
            set {
                SetPropertyValue<Boolean>("IsImported", ref _IsImported, value);
            }
        }

        /// <summary>
        /// ImportDate - Дата и время последней попытки импорта
        /// </summary>
        public DateTime ImportDate {
            get { return _ImportDate; }
            set {
                SetPropertyValue<DateTime>("ImportDate", ref _ImportDate, value);
            }
        }

        /// <summary>
        /// Hash - Хэш файла
        /// </summary>
        public String Hash {
            get { return _Hash; }
            set {
                SetPropertyValue<String>("Hash", ref _Hash, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
