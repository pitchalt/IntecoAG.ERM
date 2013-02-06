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
    /// Subroutins для импорта платёжек из файлов в формате 1С. Специально для ГазПромБанка
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCSAImporter1CGazPromBank : fmCSAImporter1C
    {
        protected internal fmCSAImporter1CGazPromBank(Session ses)
            : base(ses) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSAImporter1CGazPromBank);
            this.CID = Guid.NewGuid();
        }

        public static new fmCSAImporter1CGazPromBank GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            fmCSAImporter1CGazPromBank result = session.FindObject<fmCSAImporter1CGazPromBank>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new fmCSAImporter1CGazPromBank(session);
                result.UseCounter = 1;
                result.Code = "1СfmCSAImporter1CGazPromBank";
                result.Name = "Загрузка файлов в формате 1С для ГазПромБанка только";
                result.Description = "Загрузка файлов в формате 1С для ГазПромБанка только";
                result.Save();
            }
            return result;
        }

        #region МЕТОДЫ

        /// <summary>
        /// Определение банков по данным из рексвизитов. Если банк отсутствует в системе, то он создаётся.
        /// </summary>
        /// <param name="ir"></param>
        public override bool RequisitesBankProccess(fmCDocRCBRequisites requisites) {
            bool RES = false;

            // Определение банков
            if (requisites.RCBIC != "") {
                crmBank PersonBank = null;

                XPQuery<crmBank> banks = new XPQuery<crmBank>(this.Session, true);
                var queryBanks = from bank in banks
                            where bank.RCBIC == ReplaceWhiteSpace(requisites.RCBIC, "")
                            select bank;

                foreach (var bank in queryBanks) {
                    PersonBank = bank;
                    break;
                }
                if (PersonBank == null) {
                    PersonBank = new crmBank(this.Session);
                    //PersonBank.BIC = requisites.BIC;
                    PersonBank.RCBIC = requisites.RCBIC;
                    PersonBank.Name = requisites.BankName;
                    RES = true;
                }
                requisites.Bank = PersonBank;
            }
            else {
                // ОБРАБОТКА (Не задан БИК) ImportErrorState код 1
                // Определяем БИК из задачи импорта (которая создаётся для конкретного банка), но только в случае,
                // если имеет место следующая ситуация:
                // - текущий документ выписки является банковским ордером;
                // - банк является получателем платежа

                if (requisites.StatementOfAccount != null
                    && requisites.StatementOfAccountDoc.DocType == "Банковский ордер"
                    && requisites.StatementOfAccount.ImportResult.TaskImporter.Bank != null 
                    && requisites.StatementOfAccount.PayOutDocs.Contains(requisites.StatementOfAccountDoc)) {
                    requisites.Bank = requisites.StatementOfAccount.ImportResult.TaskImporter.Bank;
                    if (requisites.StatementOfAccount != null
                        && requisites.StatementOfAccount.ImportResult != null
                        && requisites.StatementOfAccount.ImportResult.TaskImporter.Bank != null
                        && requisites.StatementOfAccount.ImportResult.TaskImporter.Bank.Party != null
                        && requisites.StatementOfAccount.ImportResult.TaskImporter.Bank.Party.AddressLegal != null
                    ) {
                        requisites.BankLocation = requisites.StatementOfAccount.ImportResult.TaskImporter.Bank.Party.AddressLegal.AddressString;
                    }
                    requisites.BankName = requisites.StatementOfAccount.ImportResult.TaskImporter.Bank.FullName;
                } else {
                    requisites.ImportErrorStates |= (uint)ImportErrorState.RCBIC_NOT_DEFINED;
                }
            }
            return RES;
        }

        #endregion
    }

}
