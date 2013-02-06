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
    /// Subroutins для импорта платёжек
    /// </summary>
    [NavigationItem("Money")]
    [DefaultProperty("Name")]
    [Persistent("fmSATaskImporter")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class fmCSATaskImporter : csCCodedComponent {
        public fmCSATaskImporter(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSATaskImporter);
            this.CID = Guid.NewGuid();

            Actived = true;
            Saved = false;

            this.ProviderCultureInfoName = CultureInfo.InvariantCulture.Name;
            FormatDateTime = "dd.MM.yyyy";
        }

        #region ПОЛЯ КЛАССА

        private crmBank _Bank;   // банк
        private fmCSAImporter _Importer;    // Ссылка на конкретный алгоритм импорта

        private bool _Actived;   // Импортер доступен для выполнения импорта (доступен пользователю)
        private bool _Saved;   // объект сохранён (для закрытия доступа к некоторым полям навсегда)
        //
        private String _FileFormat;   // Формат файла: 1C и т.п.
        //private CultureInfo _ProviderCultureInfo = CultureInfo.InvariantCulture;
        private string _ProviderCultureInfoName;
        private string _FormatDateTime = "dd.MM.yyyy";    // "d";
        //private String _CodePage;   // кодовая страница
        private int _CodePageNum;   // кодовая страница - номер (866, 1251 и т.п.)

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Saved - объект сохранён (для закрытия доступа к некоторым полям навсегда)
        /// </summary>
        [Browsable(false)]
        public bool Saved {
            get { return _Saved; }
            set { SetPropertyValue<bool>("Saved", ref _Saved, value); }
        }

        /// <summary>
        /// Actived - Импортер доступен для выполнения импорта (доступен пользователю)
        /// </summary>
        //[RuleRequiredField]
        public bool Actived {
            get { return _Actived; }
            set { SetPropertyValue<bool>("Actived", ref _Actived, value); }
        }

        //[Association("crmBank-fmImporters")]
        //[Appearance("fmTaskImporter.Bank.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[Appearance("fmTaskImporter.Bank.Enabled", Criteria = "[Bank] Is Null And [Saved] = True", Enabled = false)]
        [Appearance("fmTaskImporter.Bank.Enabled", Criteria = "[Bank] Is Not Null And [Saved] = True", Enabled = false)]
        [RuleRequiredField]
        public crmBank Bank {
            get { return _Bank; }
            set { SetPropertyValue<crmBank>("Bank", ref _Bank, value); }
        }

        /// <summary>
        /// Importer - Ссылка на конкретный алгоритм импорта
        /// </summary>
        [RuleRequiredField]
        public fmCSAImporter Importer {
            get { return _Importer; }
            set { SetPropertyValue<fmCSAImporter>("Importer", ref _Importer, value); }
        }

        /// <summary>
        /// Формат файла: 1C и т.п.
        /// </summary>
        //[RuleRequiredField]
        //[DataSourceProperty("FileFormats")]
        public String FileFormat {
            get { return _FileFormat; }
            set { SetPropertyValue<String>("FileFormat", ref _FileFormat, value == null ? String.Empty : value.Trim()); }
        }

        ///// <summary>
        ///// CodePage - кодировка файла
        ///// </summary>
        ////[RuleRequiredField]
        //[DataSourceProperty("CodePages")]
        //[Browsable(false)]
        //public String CodePage {
        //    get { return _CodePage; }
        //    set { SetPropertyValue<String>("CodePage", ref _CodePage, value == null ? String.Empty : value.Trim()); }
        //}

        /// <summary>
        /// Кодировка файла - номер (866, 1251 и т.п.)
        /// </summary>
        //[RuleRequiredField]
        //[DataSourceProperty("CodePages")]
        public int CodePageNum {
            get { return _CodePageNum; }
            set { SetPropertyValue<int>("CodePageNum", ref _CodePageNum, value); }
        }

        /// <summary>
        /// Формат даты и времени
        /// </summary>
        //[RuleRequiredField]
        public String FormatDateTime {
            get { return _FormatDateTime; }
            set { SetPropertyValue<String>("FormatDateTime", ref _FormatDateTime, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// Провайдер культуры
        /// </summary>
        //[RuleRequiredField]
        //[DataSourceProperty("System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)")]
        //[DataSourceProperty("CultInfos")]
        [Browsable(false)]
        public CultureInfo ProviderCultureInfo {
            get {
                if (!IsLoading) {
                    //return CultureInfo.GetCultureInfo(ProviderCultureInfoName.Substring(0, ProviderCultureInfoName.IndexOf("\t\t\t")));
                    return CultureInfo.GetCultureInfo(ProviderCultureInfoName);
                }
                else {
                    return CultureInfo.CurrentCulture;
                }
            }
        }

        /// <summary>
        /// ProviderCultureInfoName - кодировка файла
        /// </summary>
        //[RuleRequiredField]
        //[ImmediatePostData]
        //[DataSourceProperty("@This.Cultures")]
        public string ProviderCultureInfoName {
            get { return _ProviderCultureInfoName; }
            set { SetPropertyValue<string>("ProviderCultureInfoName", ref _ProviderCultureInfoName, value); }
        }



        #endregion



        #region МЕТОДЫ
        /// <summary>
        /// При сохранении сделать: ...
        /// </summary>
        protected override void OnSaved() {
            Saved = true;
            base.OnSaved();
        }

        /// <summary>
        /// Выполнение задачи (Вызов метода импорта)
        /// </summary>
        /// <returns></returns>
        public virtual fmCSAImportResult ExecuteTask() {
            if (Importer != null) {
                return Importer.Import(null);
            }
            throw new Exception("Import procedure not found");
        }

        /// <summary>
        /// Выполнение задачи (Вызов метода импорта)
        /// </summary>
        /// <returns></returns>
        public fmCSAImportResult ExecuteTask(Stream stream) {
            if (Importer != null) {
                fmCSAImportResult importResult = new fmCSAImportResult(this.Session);
                importResult.TaskImporter = this;
                importResult.Bank = this.Bank;
                using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(this.CodePageNum))) {
                    importResult.FileBody = sr.ReadToEnd();
                    sr.Close();
                }
                stream.Close();

                if (importResult != null) {
                    Importer.Import(importResult);
                    importResult.ImportDate = System.DateTime.Now;
                }
                return importResult;
            }
            throw new Exception("Import procedure not found");
        }

        #endregion
    }

}
