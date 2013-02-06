using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
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
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.StatementAccount {

    // Содержимое файла и некоторые его характеристики

    [NavigationItem("Money")]
    [DefaultProperty("FileName")]
    [Persistent("fmSAImportResult")]
    public class fmCSAImportResult : BaseObject   //csCComponent
    {
        public fmCSAImportResult(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            //this.ComponentType = typeof(fmImportResult);
            //this.CID = Guid.NewGuid();
            ImportLog = string.Empty;
            ResultCode = 0;
            //StatementOfAccounts = new XPCollection<fmStatementOfAccounts>(this.Session);
        }

        #region ПОЛЯ КЛАССА

        private String _ImportLog;   // журнал событий импорта
        private String _FileName;   // наименование файла
        private DateTime _FileDate;    // Дата и время файла
        private String _FileBody;   // содержимое файла выписки
        private Boolean _IsImported;   // Признак, что файл успешно импортирован
        private DateTime _ImportDate;   // Дата и время последней попытки импорта
        private String _Hash;   // Хэш файла
        private DateTime _DateFrom;
        private DateTime _DateTo;
        private crmBank _Bank;
        //private XPCollection<fmStatementOfAccounts> _StatementOfAccounts;   // Коллекция выписок

        private fmCSATaskImporter _TaskImporter;   // Ссылка на задачу импорта

        //private XPCollection<fmStatementOfAccounts> _StatementOfAccounts;   // Коллекция выписок, образованных в результатет данной операции импорта

        private int _ResultCode;   // Результат обработки (на всех 3-х стадиях).

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// ImportLog - журнал событий импорта
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String ImportLog {
            get { return _ImportLog; }
            set {
                SetPropertyValue<String>("ImportLog", ref _ImportLog, value == null ? String.Empty : value.Trim());
            }
        }

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
        /// 
        /// </summary>
        public DateTime DateFrom {
            get { return _DateFrom; }
            set {
                SetPropertyValue<DateTime>("DateFrom", ref _DateFrom, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTo {
            get { return _DateTo; }
            set {
                SetPropertyValue<DateTime>("DateTo", ref _DateTo, value);
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

        ///// <summary>
        ///// StatementOfAccounts - Коллекция выписок
        ///// </summary>
        //public XPCollection<fmStatementOfAccounts> StatementOfAccounts {
        //    get { return _StatementOfAccounts; }
        //    set {
        //        SetPropertyValue<XPCollection<fmStatementOfAccounts>>("StatementOfAccounts", ref _StatementOfAccounts, value);
        //    }
        //}
        /// <summary>
        /// Ссылка на задачу импорета
        /// </summary>
        public crmBank Bank {
            get { return _Bank; }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }
        /// <summary>
        /// Ссылка на задачу импорета
        /// </summary>
        public fmCSATaskImporter TaskImporter {
            get { return _TaskImporter; }
            set {
                SetPropertyValue<fmCSATaskImporter>("TaskImporter", ref _TaskImporter, value);
            }
        }

        ///// <summary>
        ///// Коллекция выписок, образованных в результатет данной операции импорта
        ///// </summary>
        //public XPCollection<fmStatementOfAccounts> StatementOfAccounts {
        //    get { return _StatementOfAccounts; }
        //    set {
        //        SetPropertyValue<XPCollection<fmStatementOfAccounts>>("StatementOfAccounts", ref _StatementOfAccounts, value);
        //    }
        //}

        /// <summary>
        /// Список выписок
        /// </summary>
        [Association("fmImportResult-fmStatementOfAccounts", typeof(fmCSAStatementAccount))]
        public XPCollection<fmCSAStatementAccount> StatementOfAccounts {
            get { return GetCollection<fmCSAStatementAccount>("StatementOfAccounts"); }
        }

        [Association("fmImportResult-fmStatementAccountDoc", typeof(fmCSAStatementAccountDoc))]
        public XPCollection<fmCSAStatementAccountDoc> StatementAccountDocs {
            get { return GetCollection<fmCSAStatementAccountDoc>("StatementAccountDocs"); }
        }

        /// <summary>
        /// Результат обработки (на всех 3-х стадиях):
        /// 0 - ни одна стадия не пройдена
        /// 1 - пройдена 1-я стадия (импорт выписок и документов выписок)
        /// 2 - пройдена вторая стадия (постпроцесс - распознавание банков, контрагентов, счетов)
        /// 3 - пройдена 3-я стадия (привязка документов, регистр)
        /// </summary>
        [Browsable(false)]
        public int ResultCode {
            get { return _ResultCode; }
            set {
                SetPropertyValue<int>("ResultCode", ref _ResultCode, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        public void WriteLog(string line) {
            string datetimeStr = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ff") + "    ";
            ImportLog += ((string.IsNullOrEmpty(ImportLog)) ? "" : Environment.NewLine) + datetimeStr + line;
        }

        /// <summary>
        /// Автоматическая привязка Платёжных документов к Заявкам
        /// </summary>
        public void AutoBinding(fmCPRRepaymentTask RepaymentTask) {
            foreach (fmCSAStatementAccount sa in this.StatementOfAccounts) {
                sa.AutoBinding(RepaymentTask);
            }
        }

        #endregion

    }

}
