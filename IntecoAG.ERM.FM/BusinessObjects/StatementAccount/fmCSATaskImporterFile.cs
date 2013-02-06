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
    /// Задача импорта документов из файлов
    /// </summary>
    //[Persistent("fmTaskImporter")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCSATaskImporterFile : fmCSATaskImporter
    {
        public fmCSATaskImporterFile(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSATaskImporterFile);
            this.CID = Guid.NewGuid();

        }
        
        #region ПОЛЯ КЛАССА

        private string _CheckedPath;   // Путь к файлу с выписками

        #endregion


        #region СВОЙСТВА КЛАССА

        // Специфические поля для файлов

        /// <summary>
        /// Путь к файлу с выписками
        /// </summary>
        [RuleRequiredField]
        public String CheckedPath {
            get { return _CheckedPath; }
            set { SetPropertyValue<String>("CheckedPath", ref _CheckedPath, value == null ? String.Empty : value.Trim()); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Выполнение задачи (Вызов метода импорта)
        /// </summary>
        /// <returns></returns>
        public override fmCSAImportResult ExecuteTask() {
            if (Importer != null) {
                FileStream sourceFileStream = null;
                //if (_SourceFileStream != null) return _SourceFileStream;
                string hashFile = "";
                FileInfo sourceFileInfo = new FileInfo(CheckedPath);
                if (!sourceFileInfo.Exists) throw new Exception("File '" + sourceFileInfo.Name + "' not found");
                if (IsFileLoaded(sourceFileInfo, this.Session, out hashFile)) throw new Exception("File '" + sourceFileInfo.Name + "' has been loaded before");
                fmCSAImportResult importResult = null;
                using (sourceFileStream = new FileStream(sourceFileInfo.FullName, FileMode.Open, FileAccess.Read)) {
                    importResult = ExecuteTask(sourceFileStream);
                }

                if (importResult != null) {
                    importResult.FileName = this.CheckedPath;
                    importResult.FileDate = sourceFileInfo.CreationTime;
                    importResult.Hash = hashFile;
                }
                return importResult;
            }
            throw new Exception("Import procedure not found");
        }


        /// <summary>
        /// Проверка файла на повторную загрузку
        /// </summary>
        /// <returns></returns>
        public static bool IsFileLoaded(FileInfo fi, Session ssn, out string hashFile) {
            string hf = "";
            hf = GetHashFile(fi);
            hashFile = hf;
            //using (FileStream fsHash = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
            //    hashFile = hf;
            //    using (var md5 = new MD5CryptoServiceProvider()) {
            //        var buff = md5.ComputeHash(fsHash);
            //        //hf = Encoding.UTF8.GetString(buff);
            //        //hashFile = hf;
            //        //hashFile = ByteArrayToHex16(buff);
            //        hf = BitConverter.ToString(buff);
            //        hashFile = hf;
            //    }
            //    fsHash.Close();
            //}

            // Поиск записи о файле с таким же хэш
            XPQuery<fmCSAImportResult> fileHashs = new XPQuery<fmCSAImportResult>(ssn);
            int count = (from fh in fileHashs
                         where fh.Hash == hf //& !fh.IsImported
                         select fh).Count();

            if (count != 0) return true;
            return false;
        }


        /// <summary>
        /// Получение файлового хэша
        /// </summary>
        /// <returns></returns>
        public static string GetHashFile(Stream stream) {
            string hf = "";
            using (var md5 = new MD5CryptoServiceProvider()) {
                var buff = md5.ComputeHash(stream);
                hf = BitConverter.ToString(buff);
            }
            return hf;
        }

        ///// <summary>
        ///// Получение файлового хэша
        ///// </summary>
        ///// <returns></returns>
        public static string GetHashFile(FileInfo fi) {
            string hf = "";
            using (FileStream fsHash = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                using (var md5 = new MD5CryptoServiceProvider()) {
                    var buff = md5.ComputeHash(fsHash);
                    //hf = Encoding.UTF8.GetString(buff);
                    //hashFile = hf;
                    //hashFile = ByteArrayToHex16(buff);
                    hf = BitConverter.ToString(buff);
                }
                fsHash.Close();
            }
            return hf;
        }

        #endregion

        #region ИСТОЧНИКИ

        [Browsable(false)]
        public BindingList<string> Cultures {
            get {
                BindingList<string> res = new BindingList<string>();
                var query = from item in CultureInfo.GetCultures(CultureTypes.AllCultures)
                            //orderby item.NativeName
                            orderby item.Name
                            select item;
                foreach (CultureInfo cult in query) {
                    //res.Add(cult.Name + "\t\t\t" + cult.NativeName);
                    res.Add(cult.Name);
                }
                return res;
            }
        }



        //public BindingList<CultureInfo> CultInfos {
        //    get {
        //        BindingList<CultureInfo> res = new BindingList<CultureInfo>();
        //        var query = from item in CultureInfo.GetCultures(CultureTypes.AllCultures)
        //                    //orderby item.NativeName
        //                    orderby item.Name
        //                    select item;
        //        foreach (CultureInfo cult in query) {
        //            res.Add(cult);
        //        }
        //        return res;
        //    }
        //}


        [Browsable(false)]
        public BindingList<string> CodePages {
            get {
                BindingList<string> res = new BindingList<string>();
                var query = from item in Encoding.GetEncodings()
                            orderby item.DisplayName
                            select item;
                foreach (EncodingInfo encode in query) {
                    res.Add(encode.DisplayName);
                }
                return res;
            }
        }

        [Browsable(false)]
        public BindingList<string> FileFormats {
            get {
                BindingList<string> res = new BindingList<string>();
                res.Add("1C");
                return res;
            }
        }

        #endregion
    }

}
