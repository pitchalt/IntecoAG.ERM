using System;
using System.Collections.Generic;
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
    /// Абстрактный класс для импорта
    /// </summary>
    [Persistent("fmSAImporter")]
    //public class fmImporter : csCComponent
    public abstract class fmCSAImporter : csCCodedComponent
    {
        public fmCSAImporter(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSATaskImporter);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ ПОСТОБРАБОТКИ

        /// <summary>
        /// Метод импорта
        /// </summary>
        /// <returns></returns>
        //public abstract object Import();
        public virtual fmCSAImportResult Import(fmCSAImportResult importResult) {   //, params object[] args) {
            return null;
        }

        //public virtual void PostProcess(fmCDocRCB doc) {
        //}

        //public virtual void BankProccess(fmImportResult ir) {
        //}

        public virtual bool RequisitesBankProccess(fmCDocRCBRequisites requisites) {
            return true;
        }

        //public virtual void PartyProccessByAccount(fmCSAImportResult ir) {
        //}

        public virtual void PartyProccessByAccountRequisite(fmCDocRCBRequisites requisites) {
        }

        //public virtual void PartyProccessByINNandKPP(fmImportResult ir) {
        //}

        public virtual bool RequisitesPartyProccessByINNandKPP(fmCDocRCBRequisites requisites) {
            return true;
        }
        
        //public virtual void StatementAccountProccess(fmImportResult ir) {
        //}

        public virtual bool RequisitesStatementAccountProccess(fmCDocRCBRequisites requisites) {
            return true;
        }

        public virtual void RequisitesStatementAccountProccess(fmCSAImportResult ir) {
        }

        //public virtual bool CreateDocRCBProccess(fmImportResult ir) {
        //    return true;
        //}

        //public virtual void StrongMappingProcess() {
        //}

        //public virtual void DecentMappingProcess() {
        //}

        #endregion


        #region Вспомогательные методы

        public string ReplaceWhiteSpace(string line, string outline) {
            if (string.IsNullOrEmpty(Regex.Replace(line, @"\s+", ""))) return outline;
            return line;
        }


        public string AddLine(string line, string sourceText) {
            string res = sourceText;
            if (!string.IsNullOrEmpty(line)) {
                res += ((string.IsNullOrEmpty(sourceText)) ? "" : Environment.NewLine) + line;
            }
            return res;
        }


        /// <summary>
        /// Проверка ИНН на значимость
        /// </summary>
        /// <param name="INN"></param>
        /// <returns></returns>
        public virtual bool CheckValueINN(string INN) {
            Decimal innNum = 0;
            bool res = Decimal.TryParse(INN, out innNum);
            if (!res) return res;
            if (innNum <= 0) return false;
            return true;
        }

        ///// <summary>
        ///// Определение по внутренней записи в позиции "кодировка=DOS" или "кодировка=Windows"
        ///// </summary>
        ///// <returns></returns>
        //private string TestFileEncoding(byte[] mfile) {
        //    // Для Windows имеем подстроку:  CA EE E4 E8 F0 EE E2 EA E0 3D 57 69 6E 64 6F 77 73
        //    byte[] winStr = {0xCA,0xEE,0xE4,0xE8,0xF0,0xEE,0xE2,0xEA,0xE0,0x3D,0x57,0x69,0x6E,0x64,0x6F,0x77,0x73};

        //    // Для DOS     имеем подстроку:  8A AE A4 A8 E0 AE A2 AA A0 3D 44 4F 53
        //    byte[] dosStr = {0x8A,0xAE,0xA4,0xA8,0xE0,0xAE,0xA2,0xAA,0xA0,0x3D,0x44,0x4F,0x53};

        //    if (mfile == null || mfile.LongLength == 0) return "";

        //    foreach (int i in mfile.StartIndex(winStr)) {
        //        return "Windows";
        //    }

        //    foreach (int i in mfile.StartIndex(dosStr)) {
        //        return "DOS";
        //    }

        //    return "";
        //}

        /*
        private string ByteArrayToHex16(byte[] byteArray) {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < byteArray.Length; i++) {
                sb.Append(byteArray[i].ToString("X2"));
            }
            return sb.ToString();
        }
        */

        /// <summary>
        /// Запись в журнал
        /// </summary>
        /// <param name="line_doc"></param>
        public virtual void WriteLog(fmCSAImportResult result, string line) {
        }

        #endregion
    }

}
