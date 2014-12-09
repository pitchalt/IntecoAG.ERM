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
    /// Subroutins для импорта платёжек из файлов в формате 1С
    /// http://www.rpb.ru/doc/doku.php?id=1c2clb
    /// http://www.kholmskbank.ru/site_get_file/133
    /// http://v8.1c.ru/edi/edi_stnd/100/
    /// http://all1c77.narod.ru/ckb/1.htm
    ///
    /// </summary>
    //[NavigationItem("Finance")]
    //[DefaultProperty("CheckedPath")]
    //[Persistent("fmStatementAccount1CFileImporter")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class fmCSAImporter1C : fmCSAImporter   // csCComponent
    {
        //public fmStatementAccount1CFileImporter(Session ses)
        //    : base(ses) {
        //}

        protected internal fmCSAImporter1C(Session ses)
            : base(ses) {
        }

        public static fmCSAImporter1C GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            fmCSAImporter1C result = session.FindObject<fmCSAImporter1C>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new fmCSAImporter1C(session);
                result.UseCounter = 1;
                result.Code = "1СFileImporter";
                result.Name = "Загрузка файлов в формате 1С";
                result.Description = "Загрузка файлов в формате 1С";
                result.Save();
            }
            return result;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSAImporter1C);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private int LineNum = 0;

        //private string fileName = "";
        //private DateTime fileCreationTime = DateTime.MinValue;
        //private string hashFile = "";

        //private string CheckedPath;   // Проверяемый каталог, в который грузятся входные файлы
        //private string TreatedPath;   // Каталог, в к-й сбрасываются отработанные файлы (пока не используется)

        //private String FileFormat;   // Формат файла: 1C и т.п.
        //private String FileBody;   // Содержимое файла

        //private String FormatDateTime;
        //private CultureInfo ProviderCultureInfo;
        //private Int32 CodePageNum;

        //private crmBank Bank = null;

        //fmStatementOfAccounts statementOfAccounts = null;   // Выписка

        //XPCollection<fmCSAStatementAccount> statementOfAccountCol = null;

        //fmImportResult importResult = null;

        //private string payerAccount = "";
        //private string receiverAccount = "";

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Обработка Метод нужен пока для отладки метода обработки потока типа FileStream
        /// ??????????????????????????????????????????????????????????????????????????????
        /// </summary>
        /// <returns></returns>
        //public override object Import(params object[] args) {
        public override fmCSAImportResult Import(fmCSAImportResult importResult) {   //, params object[] args) {

            if (importResult == null) throw new ArgumentNullException();
            if (importResult.Bank == null) throw new ArgumentException();

            TextReader tr = new StringReader(importResult.FileBody);
            LineNum = 0;
            StreamProcess(importResult, tr);
            
            if (importResult.ResultCode == 0) importResult.ResultCode = 1;

            return importResult;
        }

        /// <summary>
        /// Определение банков по данным из рексвизитов. Если банк отсутствует в системе, то он создаётся.
        /// </summary>
        /// <param name="ir"></param>
        public override bool RequisitesBankProccess(fmCDocRCBRequisites requisites) {
            bool RES = false;

            // Определение банков
            if (requisites.RCBIC != "") {
                crmBank PersonBank = null;

                // Поиск банка
                /*
                CriteriaOperator criteriaBank = new BinaryOperator(new OperandProperty("RCBIC"), new ConstantValue(ReplaceWhiteSpace(requisites.RCBIC, "")), BinaryOperatorType.Equal);
                PersonBank = this.Session.FindObject<crmBank>(criteriaBank);
                if (PersonBank == null) {
                    PersonBank = new crmBank(this.Session);
                    //PersonBank.BIC = requisites.BIC;
                    PersonBank.RCBIC = requisites.RCBIC;
                    PersonBank.Name = requisites.BankName;
                    //PersonBank.Save();
                    RES = true;
                }
                requisites.Bank = PersonBank;
                */

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
                requisites.ImportErrorStates |= (uint)ImportErrorState.RCBIC_NOT_DEFINED;
            }
            return RES;
        }

        /*
        /// <summary>
        /// Определение стороны как предпочтительной стороны по банку и счёту из BankAccount.
        /// </summary>
        /// <param name="ir"></param>
        public override void PartyProccessByAccount(fmCSAImportResult ir) {
            // Во вторую очередь производится определение стороны как предпочтительной стороны по банку и счёту из BankAccount. 
            // При наличии заданных ИНН и КПП производится контрольная проверка равенства реквизитов ИНН и КПП предпочтительной
            // стороны и указанных в документе, также проверяется, чтобы предпочтительная сторона не была закрытой. 

            // Определение стороны
            foreach (fmCSAStatementAccount sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                    fmCSAStatementAccountDoc sad = requisites.StatementOfAccountDoc;
                    PartyProccessByAccountRequisite(sad.PaymentPayerRequisites);
                    PartyProccessByAccountRequisite(sad.PaymentReceiverRequisites);
                }
            }
        }
        */

        /// <summary>
        /// Определение стороны как предпочтительной стороны по банку и счёту из BankAccount.
        /// </summary>
        /// <param name="requisites"></param>
        public override void PartyProccessByAccountRequisite(fmCDocRCBRequisites requisites) {
            XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session, true);
            var query = from bankAccount in bankAccounts
                        //where bankAccount.Number == sa.BankAccountText &&
                        where bankAccount.Number == requisites.AccountParty &&
                        bankAccount.Bank == requisites.Bank
                        select bankAccount;

            foreach (var ba in query) {
                if (ba.PrefferedParty != null) {
                    if (!ba.PrefferedParty.IsClosed) {
                        if (ba.PrefferedParty.INN == requisites.INN) {
                            requisites.Party = ba.PrefferedParty;
                            requisites.Person = ba.PrefferedParty.Person;
                        }
                        else {
                            //// Проверка по ИНН и КПП
                            ////if (!string.IsNullOrEmpty(requisites.INN) && !string.IsNullOrEmpty(requisites.KPP)) {
                            //if (CheckValueINN(requisites.INN) && !string.IsNullOrEmpty(requisites.KPP)) {
                            //    XPQuery<crmCParty> parties = new XPQuery<crmCParty>(this.Session, true);
                            //    var queryParties = from party in parties
                            //                       where party.INN == requisites.INN &&
                            //                       party.KPP == requisites.KPP &&
                            //                       !party.IsClosed
                            //                       select party;
                            //    foreach (var p in queryParties) {
                            //        if (ba.PrefferedParty.INN == p.INN && ba.PrefferedParty.KPP == p.KPP) {
                            //            // Всё нормально
                            //            requisites.Party = p;
                            //            requisites.Person = p.Person;
                            //        }
                            //        else {
                                        // ОБРАБОТКА (Предпочтительная сторона не сопадает с заданной по ИНН и КПП) ImportErrorState код 2
                                        requisites.ImportErrorStates |= (uint)ImportErrorState.PREFFERED_PARTY_WARNING;
                            //        }
                            //        break;
                            //    }
                            //}
                        }
                    } else {
                        // ОБРАБОТКА (Предпочтительная сторона закрыта) ImportErrorState код 4
                        requisites.ImportErrorStates |= (uint)ImportErrorState.PREFFERED_PARTY_IS_CLOSED;
                    }
                } else {
                    // ОБРАБОТКА (Предпочтительная сторона не определена) ImportErrorState код 8
                    //requisites.ImportErrorStates |= (uint)ImportErrorState.PREFFERED_PARTY_NOT_DEFINED;
                }
                break;
            }
        }

        /// <summary>
        /// Определение стороны по ИНН и КПП
        /// </summary>
        /// <param name="ir"></param>
        public override bool RequisitesPartyProccessByINNandKPP(fmCDocRCBRequisites requisites) {
            // Во вторую очередь производится определение стороны как предпочтительной стороны по банку и счёту из BankAccount. 
            // При наличии заданных ИНН и КПП производится контрольная проверка равенства реквизитов ИНН и КПП предпочтительной
            // стороны и указанных в документе, также проверяется, чтобы предпочтительная сторона не была закрытой. 

            bool RES = false;

            // Определение стороны
            if (requisites.Party != null) return false;

            fmCSAImportResult importResult;
            if (requisites.StatementOfAccount != null)
                importResult = requisites.StatementOfAccount.ImportResult;
            else {
                if (requisites.StatementOfAccountDoc.StatementAccountIn != null)
                    importResult = requisites.StatementOfAccountDoc.StatementAccountIn.ImportResult;
                else
                    importResult = requisites.StatementOfAccountDoc.StatementAccountOut.ImportResult;
            }

            string rINN = ReplaceWhiteSpace(requisites.INN, "").Replace(" ", "");
            string rKPP = ReplaceWhiteSpace(requisites.KPP, "").Replace(" ", "");

            // Поиск по ИНН и КПП
            //if (!string.IsNullOrEmpty(rINN)) {
            if (CheckValueINN(rINN)) {
                if (requisites.INN.Length == 10) {
                    if (!string.IsNullOrEmpty(rKPP)) {

                        XPQuery<crmCParty> parties = new XPQuery<crmCParty>(this.Session, true);
                        var queryParties = from party in parties
                                           where party.INN == rINN &&
                                           party.KPP == rKPP //&& !party.IsClosed
                                           select party;
                        foreach (var p in queryParties) {
                            if (!p.IsClosed) {
                                requisites.Party = p;
                                requisites.Person = p.Person;
                            }
                            else {
                                // ОБРАБОТКА (Найденная сторона оказалась закрытой) ImportErrorState код 16
                                requisites.ImportErrorStates |= (uint)ImportErrorState.PARTY_IS_CLOSED;
                            }
                            break;
                        }

                        // Не найдена сторона по ИНН и КПП
                        if (requisites.Party == null) {

                            XPQuery<crmCLegalPerson> legalPersons = new XPQuery<crmCLegalPerson>(this.Session, true);
                            var queryLegalPerson = from legalPerson in legalPersons
                                                   where legalPerson.INN == rINN //& legalPerson.KPP == requisites.KPP
                                                   select legalPerson;

                            crmCLegalPerson lPerson = null;
                            foreach (var lp in queryLegalPerson) {
                                if (!lp.IsClosed) {
                                    lPerson = lp;
                                }
                                else {
                                    // ОБРАБОТКА (Найденное юридическое лицо оказалаоь закрытым) ImportErrorState код 32
                                    requisites.ImportErrorStates |= (uint)ImportErrorState.LEGAL_PERSON_IS_CLOSED;
                                }
                                break;
                            }

                            if (lPerson != null) { // Юридическое лицо найдено

                                // Поиск филиала

                                /*
                                OperandProperty prop = new OperandProperty("LegalPerson");
                                CriteriaOperator op = prop == lPerson;
                                CriteriaOperator criteriaPersonUnit = new BinaryOperator(new OperandProperty("KPP"), new ConstantValue(rKPP), BinaryOperatorType.Equal);

                                CriteriaOperator criteriaAND = new GroupOperator();
                                ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

                                ((GroupOperator)criteriaAND).Operands.Add(op);
                                ((GroupOperator)criteriaAND).Operands.Add(criteriaPersonUnit);

                                crmCLegalPersonUnit lpu = this.Session.FindObject<crmCLegalPersonUnit>(criteriaAND);
                                if (lpu == null) {
                                    // Создание филиала
                                    lpu = new crmCLegalPersonUnit(this.Session);
                                    lpu.Party.INN = rINN;
                                    lpu.Party.KPP = rKPP;
                                    lpu.KPP = rKPP;
                                    if (requisites.NameParty != null) {
                                        lpu.Party.Name = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                        lpu.Party.NameFull = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                    }
                                    lpu.Party.Person = lPerson.Person;

                                    lpu.Party.AddressFact.City = "-"; // Не обнаружено, откуда взять значение адреса для контрагентов
                                    lpu.Party.AddressLegal.City = "-";
                                    lpu.Party.AddressPost.City = "-";
                                    // ОБРАБОТКА (Пометить, что надо доопределить адрес)  ImportErrorState код 64
                                    requisites.ImportErrorStates |= (uint)ImportErrorState.ADDRESS_NOT_DEFINED;

                                    lpu.LegalPerson = lPerson;
                                    lPerson.LegalPersonUnits.Add(lpu);

                                    requisites.Party = lpu.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                    requisites.Person = lPerson.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?


                                    // Поправляем KPP для crmCLegalPerson lPerson, если оно пустое - ставим прочерк, пишем в журнал событий и отмечаем в маске
                                    if (string.IsNullOrEmpty(ReplaceWhiteSpace(lPerson.KPP, "").Replace(" ", ""))) {
                                        lPerson.KPP = "-";
                                        lPerson.Party.KPP = "-";
                                        lpu.KPP = rKPP;

                                        // ОБРАБОТКА (Юридическое лицо не имеет KPP, а оно обязательно при сохранении) ImportErrorState код 4096
                                        requisites.ImportErrorStates |= (uint)ImportErrorState.LEGAL_PERSON_KPP_PROBLEM;
                                        WriteLog(importResult, "Юридическое лицо " + lPerson.NameFull + " не имеет КПП, которое обязательно для сохранения импорта после создания филиала. КПП было установлено как прочерк: '-'");
                                    }

                                    RES = true;
                                    //lpu.Save();
                                }
                                */

                                XPQuery<crmCLegalPersonUnit> legalPersonUnits = new XPQuery<crmCLegalPersonUnit>(this.Session, true);
                                var querylegalPersonUnits = from legalPersonUnit in legalPersonUnits
                                                            //where legalPersonUnit.LegalPerson == lPerson &&
                                                            where legalPersonUnit.Party.INN == lPerson.INN &&
                                                                  legalPersonUnit.Party.KPP == rKPP
                                                            select legalPersonUnit;

                                crmCLegalPersonUnit lpu = null;
                                foreach (var lpUnit in querylegalPersonUnits) {
                                    //if (lpUnit.LegalPerson == lPerson && lpUnit.KPP.Replace(" ", "") == rKPP) {
                                    lpu = lpUnit;
                                    break;
                                    //}
                                }

                                if (lpu == null) {
                                    // Создание филиала
                                    lpu = new crmCLegalPersonUnit(this.Session);
                                    lpu.ManualCheckStatus = ManualCheckStateEnum.NO_CHECKED;
                                    lpu.LegalPerson = lPerson;  // Эту строчку нельзя ни в коем случае ставить после строки lpu.Party.KPP = rKPP;
                                    lpu.Party.INN = rINN;
                                    lpu.Party.KPP = rKPP;
                                    //lpu.KPP = rKPP;
                                    if (requisites.NameParty != null) {
                                        lpu.Party.Name = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                        lpu.Party.NameFull = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                    }
                                    lpu.Party.Person = lPerson.Person;

                                    lpu.Party.AddressFact.City = "-"; // Не обнаружено, откуда взять значение адреса для контрагентов
                                    lpu.Party.AddressLegal.City = "-";
                                    lpu.Party.AddressPost.City = "-";
                                    // ОБРАБОТКА (Пометить, что надо доопределить адрес)  ImportErrorState код 64
                                    requisites.ImportErrorStates |= (uint)ImportErrorState.ADDRESS_NOT_DEFINED;

                                    lPerson.LegalPersonUnits.Add(lpu);

                                    requisites.Party = lpu.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                    requisites.Person = lPerson.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?


                                    // Поправляем KPP для crmCLegalPerson lPerson, если оно пустое - ставим прочерк, пишем в журнал событий и отмечаем в маске
                                    if (string.IsNullOrEmpty(ReplaceWhiteSpace(lPerson.KPP, "").Replace(" ", ""))) {
                                        lPerson.KPP = "-";
                                        lPerson.Party.KPP = "-";
                                        lpu.KPP = rKPP;

                                        // ОБРАБОТКА (Юридическое лицо не имеет KPP, а оно обязательно при сохранении) ImportErrorState код 4096
                                        requisites.ImportErrorStates |= (uint)ImportErrorState.LEGAL_PERSON_KPP_PROBLEM;
                                        WriteLog(importResult, "Юридическое лицо " + lPerson.NameFull + " не имеет КПП, которое обязательно для сохранения импорта после создания филиала. КПП было установлено как прочерк: '-'");
                                    }

                                    RES = true;
                                    //lpu.Save();
                                }

                            }
                            else {
                                // ОБРАБОТКА (Не найдено юридическое лицо) ImportErrorState код 128
                                requisites.ImportErrorStates |= (uint)ImportErrorState.LEGAL_PERSON_NOT_FOUND;
                            }


                            // Всё равно сторона осталась неопределённой
                            if (requisites.Party == null) {
                                // ОБРАБОТКА (Отметка, что сторона не определена) ImportErrorState код 256
                                requisites.ImportErrorStates |= (uint)ImportErrorState.PARTY_NOT_DEFINED;
                            }
                        }

                    }
                    else {
                        // ОБРАБОТКА (Не задано КПП) ImportErrorState код 512
                        requisites.ImportErrorStates |= (uint)ImportErrorState.KPP_NOT_DEFINED;
                    }
                }
                else if (requisites.INN.Length == 12) {
                    // Ищем частное лицо
                    XPQuery<crmCBusinessman> Businessmans = new XPQuery<crmCBusinessman>(this.Session, true);
                    var queryBusinessmans = from businessman in Businessmans
                                            where businessman.INN == requisites.INN
                                            select businessman;

                    bool businessmanExists = false;
                    foreach (var businessman in queryBusinessmans) {
                        businessmanExists = true;
                        requisites.Party = businessman.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                        requisites.Person = businessman.Party.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                        break;
                    }

                    if (!businessmanExists) {
                        // Ищем физическое лицо
                        XPQuery<crmCPhysicalParty> PhysicalParties = new XPQuery<crmCPhysicalParty>(this.Session, true);
                        var queryPhysicalParties = from physicalParty in PhysicalParties
                                                   where physicalParty.INN == requisites.INN
                                                   select physicalParty;

                        bool physicalPartyExists = false;
                        foreach (var physicalParty in queryPhysicalParties) {
                            physicalPartyExists = true;
                            requisites.Party = physicalParty.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                            requisites.Person = physicalParty.Party.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                            break;
                        }

                        if (!physicalPartyExists) {
                            // ОБРАБОТКА (Физическая сторона не устанволена) ImportErrorState код 1024
                            requisites.ImportErrorStates |= (uint)ImportErrorState.PHYSICAL_PARTY_NOT_DEFINED;
                        }
                    }
                }
            }
            else {
                // ТРЕБУЕТСЯ ДАЛЬНЕЙШЕЕ ИССЛЕДОВАНИЕ
            }

            return RES;
        }

        /*
        /// <summary>
        /// Создание счетов
        /// </summary>
        /// <param name="ir"></param>
        public override void StatementAccountProccess(fmImportResult ir) {
            // Определение стороны
            foreach (fmStatementOfAccounts sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                    if (requisites.BankAccount != null) continue;
                    if (requisites.Party == null || string.IsNullOrEmpty(requisites.AccountBank) || requisites.Bank == null) {
                        // ОБРАБОТКА (Доопределить недостающие объекты и создать расчётный счёт) ImportErrorState код 2048
                        requisites.ImportErrorStates |= (uint)ImportErrorState.DEFINE_OBJECTS;
                    } else {
                        // Создание расчётного счёта
                        XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
                        var querybankAccounts = from bankAccount in bankAccounts
                                                where bankAccount.Number == requisites.AccountBank && bankAccount.Bank == requisites.Bank && bankAccount.Person == requisites.Party.Person
                                                select bankAccount;

                        bool bankAccountExists = false;
                        foreach (var bankAccount in querybankAccounts) {
                            bankAccountExists = true;
                            break;
                        }

                        if (!bankAccountExists) {
                            crmBankAccount ba = new crmBankAccount(this.Session);
                            ba.Person = requisites.Party.Person;
                            ba.Bank = requisites.Bank;
                            ba.Number = requisites.AccountBank;
                        }

                    }
                }
            }
        }
        */

        /*
        /// <summary>
        /// Создание счетов
        /// </summary>
        /// <param name="ir"></param>
        public override bool RequisitesStatementAccountProccess(fmCDocRCBRequisites requisites) {
            // Определение стороны

            bool RES = false;

            if (requisites.BankAccount != null) return false;
            if (requisites.Party == null || string.IsNullOrEmpty(requisites.AccountParty) || requisites.Bank == null) {
                // ОБРАБОТКА (Доопределить недостающие объекты и создать расчётный счёт) ImportErrorState код 2048
                requisites.ImportErrorStates |= (uint)ImportErrorState.DEFINE_OBJECTS;
            }
            else {
                // Создание расчётного счёта
                //XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
                //var querybankAccounts = from bankAccount in bankAccounts
                //                        where bankAccount.Number == requisites.AccountParty && 
                //                              bankAccount.Bank == requisites.Bank && 
                //                              bankAccount.Person == requisites.Party.Person
                //                        select bankAccount;

                //bool bankAccountExists = false;
                //foreach (var bankAccount in querybankAccounts) {
                //    bankAccountExists = true;
                //    break;
                //}

                CriteriaOperator criteriaAND = new GroupOperator();
                ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

                CriteriaOperator criteria1 = new BinaryOperator(new OperandProperty("Number"), new ConstantValue(ReplaceWhiteSpace(requisites.AccountParty, "")), BinaryOperatorType.Equal);
                //CriteriaOperator criteria3 = CriteriaOperator.Parse("'" + pr.KPP + "' != ''");

                OperandProperty propBank = new OperandProperty("Bank");
                CriteriaOperator opBank = propBank == requisites.Bank;

                OperandProperty propPerson = new OperandProperty("Person");
                CriteriaOperator opPerson = propPerson == requisites.Party.Person;
                CriteriaOperator criteria3 = CriteriaOperator.Parse("Person.INN == '" + requisites.Party.Person.INN + "'");

                ((GroupOperator)criteriaAND).Operands.Add(criteria1);
                ((GroupOperator)criteriaAND).Operands.Add(opBank);
                //((GroupOperator)criteriaAND).Operands.Add(opPerson);
                ((GroupOperator)criteriaAND).Operands.Add(criteria3);

                crmBankAccount account = this.Session.FindObject<crmBankAccount>(criteriaAND);
                if (account == null) {
                    //if (!bankAccountExists) {
                    crmBankAccount ba = new crmBankAccount(this.Session);
                    ba.Person = requisites.Party.Person;
                    ba.Bank = requisites.Bank;
                    ba.Number = requisites.AccountParty;
                    RES = true;
                }
            }
            return RES;
        }
        */


        /// <summary>
        /// Создание счетов
        /// </summary>
        /// <param name="ir"></param>
        public override bool RequisitesStatementAccountProccess(fmCDocRCBRequisites requisites) {
            // Определение стороны

            bool RES = false;

            if (requisites.BankAccount != null) return false;
            if (requisites.Party == null || string.IsNullOrEmpty(requisites.AccountParty) || requisites.Bank == null) {
                // ОБРАБОТКА (Доопределить недостающие объекты и создать расчётный счёт) ImportErrorState код 2048
                requisites.ImportErrorStates |= (uint)ImportErrorState.DEFINE_OBJECTS;
            } else {
                if (requisites.AccountParty == null)
                    requisites.AccountParty = String.Empty;
                else
                    requisites.AccountParty = requisites.AccountParty.Trim();
                if (!String.IsNullOrEmpty(requisites.AccountParty)) {
                    // Создание расчётного счёта
                    XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session, true);
                    var querybankAccounts = from bankAccount in bankAccounts
                                            where bankAccount.Number == requisites.AccountParty &&
                                                  bankAccount.Bank == requisites.Bank
                                            //&& bankAccount.Person == requisites.Party.Person
                                            select bankAccount;

                    crmBankAccount account = null;
                    foreach (var bankAccount in querybankAccounts) {
                        account = bankAccount;
                        break;
                    }
                    if (account == null) {
                        account = new crmBankAccount(this.Session);
                        account.Person = requisites.Party.Person;
                        account.Bank = requisites.Bank;
                        account.Number = requisites.AccountParty;
                    }
                    requisites.BankAccount = account;
                    RES = true;
                }
            }
            return RES;
        }


        public override void RequisitesStatementAccountProccess(fmCSAImportResult ir) {

            //// Наша организация
            //crmCParty OurParty = null;
            //if (crmUserParty.CurrentUserParty != null) {
            //    if (crmUserParty.CurrentUserParty.Value != null) {
            //        OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
            //    }
            //}

            //foreach (fmCSAStatementAccount sa in importResult.StatementOfAccounts) {
            //    XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
            //    var query = from bankAccount in bankAccounts
            //                where bankAccount.Number == sa.BankAccountText &&
            //                      bankAccount.Bank == importResult.TaskImporter.Bank
            //                //&& bankAccount.Person.INN == OurParty.INN
            //                select bankAccount;

            //    bool IsExists = false;
            //    foreach (var ba in query) {
            //        sa.BankAccount = ba;
            //        sa.Bank = ba.Bank;
            //        sa.BankName = ba.Bank.Name;
            //        IsExists = true;
            //        break;
            //    }
            //}


            // Определение стороны
            foreach (fmCSAStatementAccount sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {

                    //// Заполнение SumIn и SumOut
                    //if (OurParty != null) {
                    //    if (requisites.Party == OurParty || requisites.) {
                    //    }
                    //}

                    if (requisites.BankAccount != null) continue;
                    if (requisites.Party == null || string.IsNullOrEmpty(requisites.AccountParty) || requisites.Bank == null) {
                        // ОБРАБОТКА (Доопределить недостающие объекты и создать расчётный счёт) ImportErrorState код 2048
                        requisites.ImportErrorStates |= (uint)ImportErrorState.DEFINE_OBJECTS;
                    }
                    else {
                        CriteriaOperator criteriaAND = new GroupOperator();
                        ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

                        CriteriaOperator criteria1 = new BinaryOperator(new OperandProperty("Number"), new ConstantValue(ReplaceWhiteSpace(requisites.AccountParty, "")), BinaryOperatorType.Equal);
                        //CriteriaOperator criteria3 = CriteriaOperator.Parse("'" + pr.KPP + "' != ''");

                        OperandProperty propBank = new OperandProperty("Bank");
                        CriteriaOperator opBank = propBank == requisites.Bank;

                        OperandProperty propPerson = new OperandProperty("Person");
                        CriteriaOperator opPerson = propPerson == requisites.Party.Person;

                        ((GroupOperator)criteriaAND).Operands.Add(criteria1);
                        ((GroupOperator)criteriaAND).Operands.Add(opBank);
                        ((GroupOperator)criteriaAND).Operands.Add(opPerson);

                        crmBankAccount account = this.Session.FindObject<crmBankAccount>(criteriaAND);
                        if (account == null) {
                            //if (!bankAccountExists) {
                            crmBankAccount ba = new crmBankAccount(this.Session);
                            ba.Person = requisites.Party.Person;
                            ba.Bank = requisites.Bank;
                            ba.Number = requisites.AccountParty;
                        }

                    }
                }
            }
        }


        /// <summary>
        /// Обработка потока
        /// </summary>
        /// <param name="fs"></param>
        public void StreamProcess(fmCSAImportResult importResult, TextReader sr) {
            // Какая кодировка ?
            //            Int32 fileLen = (Int32)fs.Length;
            //            byte[] mfile = new byte[fileLen];

            //            fs.Read(mfile, 0, fileLen);

            // Запоминание содержимого потока
            //            FileBody = Encoding.GetEncoding(this.CodePageNum).GetString(mfile);

            //bool StatementOfAccountsAchieved = false;
            bool FirstDocumentSectionAchieved = true;

            // Коллекция создаваемых выписок
            //statementOfAccountCol = new XPCollection<fmCSAStatementAccount>(this.Session);

            //using (StreamReader sr = new StreamReader(fs)) {
            //LineNum = 0;
            //fs.Position = 0;
            //StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(this.CodePageNum));

            WriteLog(importResult, "Начало обработки выписок");

            //// Обработка секции Выписка по счёту
            //// Секций СекцияРасчСчет может быть несколько
            //CreateObjectStatementOfAccounts(mfile);

            //StatementOfAccountsAchieved = false;

            //string nextLine = "";

            // Цикл по созданию выписок по расчётным счетам, загружаемым из потока.
            string line = null;
            while ((line = sr.ReadLine()) != null) {
                LineNum++;

                if (line.Contains("СекцияРасчСчет")) {
                    //StatementOfAccountsAchieved = true;
                    // Создание выписки
                    try {
                        CreateStatementOfAccounts(importResult, this.Session, sr);   //, ref statementOfAccounts);
                        continue;
                    } catch (Exception e) {
                        importResult.ResultCode = -1;
                        WriteLog(importResult, e.Message);
                    }
                    if (importResult.ResultCode == -1) {
                        WriteLog(importResult, "Загрузка выписки (счёта) завершилась неудачно. Строка " + line.ToString());
                        importResult.IsImported = false;
                        return;
                    }
                }
                else {
                    if (line.Contains("СекцияДокумент")) {
                        if (FirstDocumentSectionAchieved) {
                            FirstDocumentSectionAchieved = false;
                            if (!CheckExistsAllStatementAccountsInSystem(importResult)) {
                                // Если хотя бы одни счёт из выписок не был опознан, обработку прекращаем и делаем следующее:
                                // Устанавливаем битовый признак этой ошибки.
                                // В журнал событий загрузки пишем сообщение об этом.
                                // Процесс обработки прекращается.

                                WriteLog(importResult, "Обнаружен счёт, не зарегистрированный в системе. Продолжение импорта невозможно");

                                // Отметка об успешной загрузке
                                importResult.IsImported = false;
                                importResult.ResultCode = -1;

                                // Чистим коллекцию и удаляем все выписки
                                //importResult.StatementOfAccounts

                                WriteLog(importResult, "Обработка выписок прекращена");
                                return;
                            }
                        }

                        try {
                            ProcessDocumentFromStream(importResult, sr, line);
                        } catch (Exception e) {
                            importResult.ResultCode = -2;
                            WriteLog(importResult, e.Message);
                        }
                        if (importResult.ResultCode == -2) {
                            WriteLog(importResult, "Загрузка последнего документа завершилась неудачно. Строка " + line.ToString());
                            importResult.IsImported = false;
                            return;
                        }
                    }
                    else {
                        int eqPos = line.IndexOf("=");
                        if (eqPos > 0) {
                            string key = line.Substring(0, eqPos);
                            string item = line.Substring(eqPos + 1);
                            if (key == "ДатаНачала") {
                                DateTime date;
                                if (ConvertStringToDateTime(importResult, item, out date))
                                    importResult.DateFrom = date;
                            }
                            if (key == "ДатаКонца") {
                                DateTime date;
                                if (ConvertStringToDateTime(importResult, item, out date))
                                    importResult.DateTo = date;
                            }
                            if (key == "РасчСчет") {
                                if (AccountGet(importResult.Bank, item) == null) {
                                    WriteLog(importResult, "Обнаружен счёт "+ item + "не зарегистрированный в системе. Продолжение импорта невозможно");
                                    importResult.IsImported = false;
                                    importResult.ResultCode = -1;

                                // Чистим коллекцию и удаляем все выписки
                                //importResult.StatementOfAccounts

                                    WriteLog(importResult, "Обработка выписок прекращена");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            // Отметка об успешной загрузке
            importResult.IsImported = true;
            WriteLog(importResult, "Успешно завершена обработка файла: ");
        }

        protected crmBankAccount AccountGet(crmBank bank, String number) {
            return bank.Session.FindObject<crmBankAccount>(
                    CriteriaOperator.And( 
                        new BinaryOperator("Bank", bank),
                        new BinaryOperator("Number", number)
                    ));
        }

        protected void ProcessDocumentFromStream(fmCSAImportResult importResult, TextReader sr, String line) {

            int oldResultCode = importResult.ResultCode;
            importResult.ResultCode = -2; // Предварительная установка признака, что документы не все обработались.

            // Запоминаем коллекцию выписок
            //importResult.StatementOfAccounts = statementOfAccountCol;

            // Сразу после последней секции СекцияРасчСчет следует СекцияДокумент 

            // Обработка секций документов. Считаем, что секция СекцияРасчСчет - КонецРасчСчет всегда имеется
            // Это нужно для объекта sr (непрерывной последовательности его работы)

            //if (line_doc.Contains("СекцияДокумент")) FirstDocumentSectionAchieved = true;

            //if (!FirstDocumentSectionAchieved) continue;

            int eqPos = line.IndexOf("=");
            if (eqPos >= 0) {   // Пропуск всех пока не нужных записей в файле
                string key = line.Substring(0, eqPos);
                string item = line.Substring(eqPos + 1);

                //payerAccount = "";
                //receiverAccount = "";
                fmCSAStatementAccountDoc loadedDoc = null;
                if (key == "СекцияДокумент" && item == "Платежное поручение") {
                    loadedDoc = CreateStatementDocument(importResult, this.Session, ref sr, typeof(fmCDocRCBPaymentOrder), item);
                }
                else if (key == "СекцияДокумент" && item == "Платежное требование") {
                    loadedDoc = CreateStatementDocument(importResult, this.Session, ref sr, typeof(fmCDocRCBPaymentRequest), item);
                }
                else if (key == "СекцияДокумент" && item == "Аккредитив") {
                    loadedDoc = CreateStatementDocument(importResult, this.Session, ref sr, typeof(fmCDocRCBAkkreditivRequest), item);
                }
                else if (key == "СекцияДокумент" && item == "Инкассовое поручение") {
                    loadedDoc = CreateStatementDocument(importResult, this.Session, ref sr, typeof(fmCDocRCBInkassOrder), item);
                }
                else {
                    loadedDoc = CreateStatementDocument(importResult, this.Session, ref sr, typeof(fmCDocRCBOthers), item);
                }

                //if (loadedDoc != null) {
                //    // Получаем выписку, к которой относится документ
                //    fmCSAStatementAccount sa = null;
                //    sa = FindStatementOfAccount(importResult, loadedDoc.PaymentPayerRequisites.AccountParty);
                //    if (sa != null) {
                //        sa.DocRCBRequisites.Add(loadedDoc.PaymentPayerRequisites);
                //        sa.PayOutDocs.Add(loadedDoc);
                //        loadedDoc.StatementAccountOut = sa;
                //    }
                //    sa = FindStatementOfAccount(importResult, loadedDoc.PaymentReceiverRequisites.AccountParty);
                //    if (sa != null) {
                //        sa.DocRCBRequisites.Add(loadedDoc.PaymentReceiverRequisites);
                //        sa.PayInDocs.Add(loadedDoc);
                //        loadedDoc.StatementAccountIn = sa;
                //    }
                //}
            }

            importResult.ResultCode = oldResultCode; // Восстанавливаем
        }

        /// <summary>
        /// Проверка всех счетов выписок на присутствие в системе
        /// </summary>
        /// <param name="statementOfAccountCol"></param>
        /// <returns></returns>
        protected bool CheckExistsAllStatementAccountsInSystem(fmCSAImportResult importResult) {

            bool CommonResult = true;

            // Наша организация
            crmCParty OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }

            foreach (fmCSAStatementAccount sa in importResult.StatementOfAccounts) {
                XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
                var query = from bankAccount in bankAccounts
                            where bankAccount.Number == sa.BankAccountText &&
                                  bankAccount.Bank == importResult.TaskImporter.Bank
                            //&& bankAccount.Person.INN == OurParty.INN
                            select bankAccount;

                bool IsExists = false;
                foreach (var ba in query) {
                    sa.BankAccount = ba;
                    sa.Bank = ba.Bank;
                    sa.BankName = ba.Bank.Name;
                    IsExists = true;
                    break;
                }

                if (IsExists) {
                    WriteLog(importResult, "Cчёт " + sa.BankAccountText + ", банк " + importResult.TaskImporter.Bank.FullName + "(БИК " + importResult.TaskImporter.Bank.RCBIC + ") найден в системе.");
                }
                else {
                    WriteLog(importResult, "Cчёт " + sa.BankAccountText + ", банк " + importResult.TaskImporter.Bank.FullName + "(БИК " + importResult.TaskImporter.Bank.RCBIC + "), не зарегистрирован в системе.");
                    CommonResult = false;
                    //break;
                }

            }
            return CommonResult;
        }

        #region Создание объекта документа выписки
        /*
        private void CreateObjectStatementOfAccounts(byte[] mfile) {
            //fmStatementOfAccounts statementOfAccounts = objectSpace.CreateObject<fmStatementOfAccounts>();
            statementOfAccounts = new fmStatementOfAccounts(this.Session);


            statementOfAccounts.Bank = this.Bank;
            statementOfAccounts.BankName = this.Bank.Name;
            //statementOfAccounts.CodePage = this.CodePageNum.ToString();
            //statementOfAccounts.DataFormat = this.FileFormat;

            statementOfAccounts.ImportResult.Importer = this;   // Запоминаем ссылку на конкретный загрузчик, в данном случае этот класс
            statementOfAccounts.ImportResult.FileName = fileName;
            statementOfAccounts.ImportResult.FileDate = fileCreationTime;
            statementOfAccounts.ImportResult.FileBody = Encoding.GetEncoding(this.CodePageNum).GetString(mfile);

            statementOfAccounts.ImportResult.Hash = hashFile;
            statementOfAccounts.ImportResult.ImportDate = System.DateTime.Now;

            statementOfAccounts.DocDate = System.DateTime.Now;
            statementOfAccounts.DocNumber = statementOfAccounts.BankName + " + выписка от " + statementOfAccounts.DocDate.ToString("dd.MM.yyyy hh:mm:ss");

            WriteLog("Создана выписка: " + statementOfAccounts.DocNumber + " от " + statementOfAccounts.DocDate);
        }
*/
        #endregion

        #region Формирование выписки: CreateStatementOfAccounts

        protected fmCSAStatementAccount CreateStatementOfAccounts(fmCSAImportResult importResult, Session ssn, TextReader sr) {   //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmStatementOfAccountInfo statementOfAccountInfo = new fmStatementOfAccountInfo(ssn);

            //// Общие поля выписки
            //statementOfAccountInfo.ImportResult = importResult;
            //importResult.StatementOfAccounts.Add(statementOfAccountInfo);

            DateTime dateFrom = default(DateTime);
            DateTime dateTo = default(DateTime);
            String bankAccountNumber = String.Empty;
            Decimal balanceOfIncoming = 0;
            Decimal totalRecaivedAtAccount = 0;
            Decimal totalWriteOfAccount = 0;
            Decimal balanceOfOutgoing = 0;

            string line = null;
            while ((line = sr.ReadLine()) != null) {
                LineNum++;

                if (line == "КонецРасчСчет") {
                    //statementOfAccounts.AccountInfo.Add(statementOfAccountInfo);
                    //statementOfAccountInfo.StatementOfAccounts = statementOfAccounts;
                    break;
                } else {
                    if (!line.Contains("=")) continue;

                    int eqPos = line.IndexOf("=");
                    string key = line.Substring(0, eqPos);
                    string item = line.Substring(eqPos + 1);
                    
                    switch (key) {
                        case "ДатаНачала":
                            if (ConvertStringToDateTime(importResult, item, out dateFrom)) {
                            }
                            break;
                        case "ДатаКонца":
                            if (ConvertStringToDateTime(importResult, item, out dateTo)) {
                            }
                            break;
                        case "РасчСчет":
                            bankAccountNumber = item;
                            break;
                        case "НачальныйОстаток":
                            if (ConvertStringToDecimal(importResult, item, out balanceOfIncoming)) {
                            }
                            break;
                        case "ВсегоПоступило":
                            if (ConvertStringToDecimal(importResult, item, out totalRecaivedAtAccount)) {
                            }
                            break;
                        case "ВсегоСписано":
                            if (ConvertStringToDecimal(importResult, item, out totalWriteOfAccount)) {
                            }
                            break;
                        case "КонечныйОстаток":
                            if (ConvertStringToDecimal(importResult, item, out balanceOfOutgoing)) {
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            fmCSAStatementAccount statementOfAccountInfo = null;
            foreach (fmCSAStatementAccount sa in importResult.StatementOfAccounts) {
                if (sa.BankAccountText == bankAccountNumber) {
                    statementOfAccountInfo = sa;
                    break;
                }
            }
            if (statementOfAccountInfo == null) {
                statementOfAccountInfo = new fmCSAStatementAccount(ssn);
                statementOfAccountInfo.BankAccountText = bankAccountNumber;
                importResult.StatementOfAccounts.Add(statementOfAccountInfo);
                statementOfAccountInfo.DocDate = System.DateTime.Now;
            }

            // 2012-04-12 Некоторые банки величины периодна расписывают по каждому дню в этом периоде когда была операция
            // 2012-04-12 Видимо, разумнее брать даты от и до самые широки - из ImportResult, либо, конечно, выправлять даты
            if (statementOfAccountInfo.DateFrom == DateTime.MinValue) {
                statementOfAccountInfo.DateFrom = dateFrom;
            } else {
                if (dateFrom < statementOfAccountInfo.DateFrom) {
                    statementOfAccountInfo.DateFrom = dateFrom;
                }
            }
            if (statementOfAccountInfo.DateTo == DateTime.MinValue) {
                statementOfAccountInfo.DateTo = dateTo;
            } else {
                if (dateTo > statementOfAccountInfo.DateTo) {
                    statementOfAccountInfo.DateTo = dateTo;
                }
            }
            statementOfAccountInfo.DocNumber = String.Format("Выписка по счёту № {0} за период {1} - {2}", bankAccountNumber, dateFrom.ToString("dd.MM.yyyy"), dateTo.ToString("dd.MM.yyyy"));
            
            // 2012-04-12
            statementOfAccountInfo.BalanceOfIncoming += Math.Abs(balanceOfIncoming);
            statementOfAccountInfo.TotalRecaivedAtAccount += Math.Abs(totalRecaivedAtAccount);
            statementOfAccountInfo.TotalWriteOfAccount += Math.Abs(totalWriteOfAccount);
            statementOfAccountInfo.BalanceOfOutgoing += Math.Abs(balanceOfOutgoing);
            return statementOfAccountInfo;
        }
        #endregion


        #region Создание документа выписки: CreateStatementDocument
        protected fmCSAStatementAccountDoc CreateStatementDocument(fmCSAImportResult importResult, Session ssn, ref TextReader sr, Type typeOfDocument, string typeDocumentName) {
            fmCSAStatementAccountDoc loadedDoc = new fmCSAStatementAccountDoc(ssn);
            //loadedDoc.TypeOfDocument = typeOfDocument;
            if (typeOfDocument != null) loadedDoc.NameTypeOfRCBDocument = typeOfDocument.FullName;
            loadedDoc.DocType = typeDocumentName;
            loadedDoc.ImportResult = importResult;

            WriteLog(importResult, "Начата обработка: " + typeDocumentName);

            string line = null;
            while ((line = sr.ReadLine()) != null) {
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(importResult, loadedDoc, line); // , statementOfAccounts);
            }

            fmCSAStatementAccount saPayer = FindStatementOfAccount(importResult, loadedDoc.PaymentPayerRequisites.RCBIC, loadedDoc.PaymentPayerRequisites.AccountParty );
            if (saPayer != null) {
                  saPayer.DocRCBRequisites.Add(loadedDoc.PaymentPayerRequisites);
                  saPayer.PayOutDocs.Add(loadedDoc);
                  loadedDoc.StatementAccountOut = saPayer;
                //loadedDoc.PaymentPayerRequisites.StatementOfAccount = saPayer;
            }
            fmCSAStatementAccount saReceiver = FindStatementOfAccount(importResult, loadedDoc.PaymentReceiverRequisites.RCBIC, loadedDoc.PaymentReceiverRequisites.AccountParty);
            if (saReceiver != null) {
                saReceiver.DocRCBRequisites.Add(loadedDoc.PaymentReceiverRequisites);
                  saReceiver.PayInDocs.Add(loadedDoc);
                  loadedDoc.StatementAccountIn = saReceiver;
                //loadedDoc.PaymentPayerRequisites.StatementOfAccount = saReceiver;
            }
            WriteLog(importResult, "Завершена обработка: " + typeDocumentName);
            return loadedDoc;
        }
        #endregion

        /*
        #region Создание платёжки: CreatePaymentOrder
        private fmCStatementAccountDoc CreatePaymentOrder(Session ssn, ref StreamReader sr)
        { //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmCDocRCBPaymentOrder loadedDoc = new fmCDocRCBPaymentOrder(ssn);
            //fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            ////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            
            fmCStatementAccountDoc loadedDoc = new fmCStatementAccountDoc(ssn);
            loadedDoc.TypeOfDocument = typeof(fmCDocRCBPaymentOrder);

            WriteLog("Платёжное поручение");

            while (!sr.EndOfStream) {
                string line_doc = sr.ReadLine();
                LineNum++;

                if (line_doc == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line_doc.Contains("=")) continue;

                CommonDocPart(loadedDoc, line_doc); // , statementOfAccounts);
            }
            WriteLog("Завершена обработка Платёжного поручения");
            return loadedDoc;
        }
        #endregion


        #region Создание платёжного требования: CreatePaymentRequest
        private fmCStatementAccountDoc CreatePaymentRequest(Session ssn, ref StreamReader sr)
        {   //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmCDocRCBPaymentRequest loadedDoc = new fmCDocRCBPaymentRequest(ssn);
            //fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            //////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            ////// Ссылка на выписку
            ////statementOfAccounts.RCBDocuments.Add(loadedDoc);
            ////loadedDoc.StatementOfAccounts = statementOfAccounts;

            fmCStatementAccountDoc loadedDoc = new fmCStatementAccountDoc(ssn);
            loadedDoc.TypeOfDocument = typeof(fmCDocRCBPaymentRequest);

            WriteLog("Платёжное требование");

            while (sr.Peek() >= 0) {
                string line_doc = sr.ReadLine();
                LineNum++;

                if (line_doc == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line_doc.Contains("=")) continue;

                CommonDocPart(loadedDoc, line_doc); ///, statementOfAccounts);
            }
            WriteLog("Завершена обработка Платёжного требования");
            return loadedDoc;
        }
        #endregion


        #region Создание заявление на аккредити: CreateAkkreditivRequest
        private fmCStatementAccountDoc CreateAkkreditivRequest(Session ssn, ref StreamReader sr)
        {   //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmCDocRCBAkkreditivRequest loadedDoc = new fmCDocRCBAkkreditivRequest(ssn);
            //fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            //////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            ////// Ссылка на выписку
            ////statementOfAccounts.RCBDocuments.Add(loadedDoc);
            ////loadedDoc.StatementOfAccounts = statementOfAccounts;

            fmCStatementAccountDoc loadedDoc = new fmCStatementAccountDoc(ssn);
            loadedDoc.TypeOfDocument = typeof(fmCDocRCBPaymentRequest);

            WriteLog("Аккредитив");

            while (!sr.EndOfStream) {
                string line_doc = sr.ReadLine();
                LineNum++;

                if (line_doc == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line_doc.Contains("=")) continue;

                CommonDocPart(loadedDoc, line_doc); // , statementOfAccounts);
            }
            WriteLog("Завершена обработка Аккредитива");
            return loadedDoc;
        }
        #endregion


        #region Создание инкассовое поручение: CreateInkassOrder
        private fmCStatementAccountDoc CreateInkassOrder(Session ssn, ref StreamReader sr)
        {   //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmCDocRCBInkassOrder loadedDoc = new fmCDocRCBInkassOrder(ssn);
            //fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            //////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            ////// Ссылка на выписку
            ////statementOfAccounts.RCBDocuments.Add(loadedDoc);
            ////loadedDoc.StatementOfAccounts = statementOfAccounts;

            fmCStatementAccountDoc loadedDoc = new fmCStatementAccountDoc(ssn);
            loadedDoc.TypeOfDocument = typeof(fmCDocRCBInkassOrder);

            WriteLog("Инкассовое поручение");

            while (!sr.EndOfStream) {
                string line_doc = sr.ReadLine();
                LineNum++;

                if (line_doc == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line_doc.Contains("=")) continue;

                CommonDocPart(loadedDoc, line_doc); //, statementOfAccounts);
            }
            WriteLog("Завершена обработка Инкассового поручения");
            return loadedDoc;
        }
        #endregion


        #region Создание Прочих документов стандартным образом: CreateOtherOrder
        //private void CreateOtherOrder(Session ssn, ref StreamReader sr, ref fmStatementOfAccounts statementOfAccounts, string docTypes) {
        private fmCStatementAccountDoc CreateOtherOrder(Session ssn, ref StreamReader sr, string docType)
        {
            //fmCDocRCBOthers loadedDoc = new fmCDocRCBOthers(ssn);
            //fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            //loadedDoc.DocType = docTypes;
            //////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            ////// Ссылка на выписку
            ////statementOfAccounts.RCBDocuments.Add(loadedDoc);
            ////loadedDoc.StatementOfAccounts = statementOfAccounts;

            fmCStatementAccountDoc loadedDoc = new fmCStatementAccountDoc(ssn);
            loadedDoc.TypeOfDocument = typeof(fmCDocRCBOthers);
            loadedDoc.DocType = docType;

            WriteLog("Документ Прочие");

            while (!sr.EndOfStream) {
                string line_doc = sr.ReadLine();
                LineNum++;

                if (line_doc == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line_doc.Contains("=")) continue;

                CommonDocPart(loadedDoc, line_doc);  //, statementOfAccounts);
                
                //int eqPos = line_doc.IndexOf("=");
                //string key = line_doc.Substring(0, eqPos);
                //string item = line_doc.Substring(eqPos + 1);

                //switch (key) {
                //    case "СекцияДокумент":
                //        loadedDoc.DocType = item;
                //        WriteLog("Тип документа прочие: " + item);
                //        break;
                //    default:
                //        break;
                //}
                
            }
            WriteLog("Завершена обработка Документа Прочие");
            return loadedDoc;
        }
        #endregion
*/

        #region Обработка общей части документов выписки: CommonDocPart

        /// <summary>
        /// Нахождение выписки по номеру счёта
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public fmCSAStatementAccount FindStatementOfAccount(fmCSAImportResult importResult, String bic, String account) {
            // Находим подходящую выписку
            foreach (fmCSAStatementAccount sa in importResult.StatementOfAccounts) {
                if (sa.BankAccountText == account &&
                    ( (bic == null || bic.Trim() == "") || (sa.Bank == null || sa.Bank.RCBIC == bic))) {   // 2012-04-11
                    return sa;
                }
            }
            return null;
        }

        public virtual void CommonDocPart(fmCSAImportResult importResult, fmCSAStatementAccountDoc loadedDoc, string line) { //, fmStatementOfAccounts statementOfAccounts) {

            int eqPos = line.IndexOf("=");
            string key = line.Substring(0, eqPos);
            string item = line.Substring(eqPos + 1);
            item = ReplaceWhiteSpace(item, " ").Trim();   // 2012-04-11

            switch (key) {
                case "Номер":
                    loadedDoc.DocNumber = item;
                    WriteLog(importResult, "Номер: " + item);
                    break;
                case "Дата":
                    DateTime docDate;
                    if (ConvertStringToDateTime(importResult, item, out docDate)) {
                        loadedDoc.DocDate = docDate;
                        WriteLog(importResult, "Дата: " + item);
                    }
                    break;
                case "Сумма":
                    Decimal paymentCost;
                    if (ConvertStringToDecimal(importResult, item, out paymentCost)) {
                        loadedDoc.PaymentCost = Math.Abs(paymentCost);
                    }
                    break;
                case "ПлательщикСчет":
                    loadedDoc.PaymentPayerRequisites.AccountParty = item;
                    //payerAccount = item;
                    break;
                case "ДатаСписано":
                    DateTime deductedFromPayerAccount;
                    if (ConvertStringToDateTime(importResult, item, out deductedFromPayerAccount)) {
                        loadedDoc.DeductedFromPayerAccount = deductedFromPayerAccount;
                    }
                    break;
                case "ДатаПоступило":
                    DateTime receivedByPayerBankDate;
                    if (ConvertStringToDateTime(importResult, item, out receivedByPayerBankDate)) {
                        loadedDoc.ReceivedByPayerBankDate = receivedByPayerBankDate;
                    }
                    break;
                case "ПлательщикИНН":
                    loadedDoc.PaymentPayerRequisites.INN = item;
                    break;
                case "ПлательщикКПП":
                    loadedDoc.PaymentPayerRequisites.KPP = item;
                    break;
                case "Плательщик1":
                    if (!string.IsNullOrEmpty(item)) {
                        if (string.IsNullOrEmpty(loadedDoc.PaymentPayerRequisites.NameParty)) {
                            loadedDoc.PaymentPayerRequisites.NameParty = item + Environment.NewLine;
                        }
                        else {
                            loadedDoc.PaymentPayerRequisites.NameParty = item + Environment.NewLine + loadedDoc.PaymentPayerRequisites.NameParty;
                        }
                    }
                    break;
                case "Плательщик2":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentPayerRequisites.NameParty += "Р/с " + item;
                    }
                    break;
                case "Плательщик3":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentPayerRequisites.NameParty += " в " + item;
                    }
                    break;
                case "Плательщик4":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentPayerRequisites.NameParty += item;
                    }
                    break;
                case "ПлательщикРасчСчет":
                    loadedDoc.PaymentPayerRequisites.AccountParty = item;
                    break;
                case "ПлательщикБанк1":
                    if (string.IsNullOrEmpty(loadedDoc.PaymentPayerRequisites.BankName)) {
                        loadedDoc.PaymentPayerRequisites.BankName = item;
                    }
                    else {
                        loadedDoc.PaymentPayerRequisites.BankName = loadedDoc.PaymentPayerRequisites.BankName + " " + item;
                    }
                    break;
                case "ПлательщикБанк2":
                    loadedDoc.PaymentPayerRequisites.BankLocation = item;
                    if (string.IsNullOrEmpty(loadedDoc.PaymentPayerRequisites.BankName)) {
                        loadedDoc.PaymentPayerRequisites.BankName = item;
                    }
                    else {
                        loadedDoc.PaymentPayerRequisites.BankName = item + " " + loadedDoc.PaymentPayerRequisites.BankName;
                    }
                    break;
                case "ПлательщикБИК":
                    loadedDoc.PaymentPayerRequisites.RCBIC = item; //((item.Length == 8) ? "0" : "") + item;
                    break;
                case "ПлательщикКорсчет":
                    loadedDoc.PaymentPayerRequisites.AccountBank = item;
                    break;
                case "ПолучательСчет":
                    loadedDoc.PaymentReceiverRequisites.AccountParty = item;
                    //receiverAccount = item;

                    break;
                case "ПолучательИНН":
                    loadedDoc.PaymentReceiverRequisites.INN = item;
                    break;
                case "ПолучательКПП":
                    loadedDoc.PaymentReceiverRequisites.KPP = item;
                    break;
                case "Получатель1":
                    if (!string.IsNullOrEmpty(item)) {
                        if (string.IsNullOrEmpty(loadedDoc.PaymentReceiverRequisites.NameParty)) {
                            loadedDoc.PaymentReceiverRequisites.NameParty = item + Environment.NewLine;
                        }
                        else {
                            loadedDoc.PaymentReceiverRequisites.NameParty = item + Environment.NewLine + loadedDoc.PaymentReceiverRequisites.NameParty;
                        }
                    }
                    break;
                case "Получатель2":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentReceiverRequisites.NameParty += "Р/с " + item;
                    }
                    break;
                case "Получатель3":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentReceiverRequisites.NameParty += " в " + item;
                    }
                    break;
                case "Получатель4":
                    if (!string.IsNullOrEmpty(item)) {
                        loadedDoc.PaymentReceiverRequisites.NameParty += item;
                    }
                    break;
                case "ПолучательРасчСчет":
                    loadedDoc.PaymentReceiverRequisites.AccountParty = item;
                    break;
                case "ПолучательБанк1":
                    if (string.IsNullOrEmpty(loadedDoc.PaymentReceiverRequisites.BankName)) {
                        loadedDoc.PaymentReceiverRequisites.BankName = item;
                    }
                    else {
                        loadedDoc.PaymentReceiverRequisites.BankName = loadedDoc.PaymentReceiverRequisites.BankName + " " + item;
                    }
                    break;
                case "ПолучательБанк2":
                    loadedDoc.PaymentReceiverRequisites.BankLocation = item;
                    if (string.IsNullOrEmpty(loadedDoc.PaymentReceiverRequisites.BankName)) {
                        loadedDoc.PaymentReceiverRequisites.BankName = item;
                    }
                    else {
                        loadedDoc.PaymentReceiverRequisites.BankName = item + " " + loadedDoc.PaymentReceiverRequisites.BankName;
                    }
                    break;
                case "ПолучательБИК":
                    loadedDoc.PaymentReceiverRequisites.RCBIC = item; //((item.Length == 8) ? "0" : "") + item;
                    //if (loadedDoc.PaymentReceiverRequisites.RCBIC == null && importResult.Bank.RCBIC == "" ) {
                    //    loadedDoc.PaymentReceiverRequisites.Bank = importResult.Bank;
                    //    loadedDoc.PaymentReceiverRequisites.RCBIC = importResult.Bank.RCBIC;
                    //    loadedDoc.PaymentReceiverRequisites.BankName = importResult.Bank.Name;
                    //    loadedDoc.PaymentReceiverRequisites.BankLocation = importResult.Bank.Party.AddressLegal.AddressString;
                    //}
                    break;
                case "ПолучательКорсчет":
                    loadedDoc.PaymentReceiverRequisites.AccountBank = item;
                    break;

                case "ВидПлатежа":
                    loadedDoc.PaymentKind = item;
                    break;
                case "ВидОплаты":
                    loadedDoc.OperationKind = item;
                    break;

                case "СрокПлатежа":
                    loadedDoc.PaymentDeadLine = item;
                    break;
                case "Очередность":
                    loadedDoc.PaymentSequence = item;
                    break;
                case "НазначениеПлатежа":
                    //loadedDoc.PaymentFunction = item;
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;

                // Считаем, что в файле поля НазначениеПлатежа с номерами от 1 до 6, если есть таковые, идут в порядке возрастания
                // номеров (это временное ограничние и скорее всего не обязательное)
                case "НазначениеПлатежа1":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;
                case "НазначениеПлатежа2":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;
                case "НазначениеПлатежа3":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;
                case "НазначениеПлатежа4":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;
                case "НазначениеПлатежа5":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;
                case "НазначениеПлатежа6":
                    loadedDoc.PaymentFunction = AddLine(item, loadedDoc.PaymentFunction);
                    break;



                case "СрокАкцепта":
                    Int16 acceptanceDuration;
                    if (ConvertStringToInt16(importResult, item, out acceptanceDuration)) {
                        loadedDoc.AcceptanceDuration = acceptanceDuration;
                    }
                    break;
                case "ВидАккредитива":
                    loadedDoc.AkkreditiveKind = item;
                    break;
                case "УсловиеОплаты":
                    loadedDoc.PaymentCondition = item;
                    break;
                case "PaymentByRepresentation":
                    loadedDoc.PaymentCondition = item;
                    break;
                case "ДополнУсловия":
                    loadedDoc.AdvancedConditions = item;
                    break;
                case "НомерСчетаПоставщика":
                    loadedDoc.AccountNumberSupplier = item;
                    break;
                case "ДатаОтсылкиДок":
                    DateTime documentSendingDate;
                    if (ConvertStringToDateTime(importResult, item, out documentSendingDate)) {
                        loadedDoc.DocumentSendingDate = documentSendingDate;
                    }
                    break;


                case "КвитанцияДата":
                    DateTime ticketDate;
                    if (ConvertStringToDateTime(importResult, item, out ticketDate)) {
                        loadedDoc.TicketDate = ticketDate;
                    }
                    break;
                case "КвитанцияВремя":
                    loadedDoc.TicketTime = item;
                    break;
                case "КвитанцияСодержание":
                    loadedDoc.TicketContent = item;
                    break;


                // Поля Бюджетной платёжки  fmCDocRCBBudgetPaymentOrder
                case "СтатусСоставителя":
                    loadedDoc.CompilerStatus = item;
                    break;
                case "ПоказательКБК":
                    loadedDoc.KBKStatus = item;
                    break;
                case "ОКАТО":
                    loadedDoc.OKATO = item;
                    break;
                case "ПоказательОснования":
                    loadedDoc.ReasonIndicator = item;
                    break;
                case "ПоказательПериода":
                    loadedDoc.PeriodIndicator = item;
                    break;
                case "ПоказательНомера":
                    loadedDoc.NumberIndicator = item;
                    break;
                case "ПоказательДаты":
                    loadedDoc.DateIndicator = item;
                    break;
                case "ПоказательТипа":
                    loadedDoc.TypeIndicator = item;
                    break;

                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// Преобразование даты из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        public bool ConvertStringToDateTime(fmCSAImportResult importResult, string dtString, out DateTime res) {
            try {
                res = DateTime.ParseExact(dtString, importResult.TaskImporter.FormatDateTime, importResult.TaskImporter.ProviderCultureInfo);
                return true;
            }
            catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            res = DateTime.MinValue;
            return false;
        }

        /// <summary>
        /// Преобразование числа из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        public bool ConvertStringToDecimal(fmCSAImportResult importResult, string dtString, out Decimal res) {
            try {
                res = Decimal.Parse(dtString, importResult.TaskImporter.ProviderCultureInfo.NumberFormat);
                return true;
            }
            catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            res = Decimal.MinValue;
            return false;
        }

        /// <summary>
        /// Преобразование целого числа из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        public bool ConvertStringToInt16(fmCSAImportResult importResult, string dtString, out Int16 res) {
            try {
                res = Int16.Parse(dtString, importResult.TaskImporter.ProviderCultureInfo.NumberFormat);
                return true;
            }
            catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. crmAct_Line: {1}. " + "System message:  {2}", "", LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(importResult, err);
            }
            res = Int16.MinValue;
            return false;
        }

        /// <summary>
        /// Запись в журнал
        /// </summary>
        /// <param name="line_doc"></param>
        public override void WriteLog(fmCSAImportResult importResult, string line) {
            if (importResult == null)
                return;
            importResult.WriteLog(line);
        }

        /// <summary>
        /// Проверка ИНН на значимость
        /// </summary>
        /// <param name="INN"></param>
        /// <returns></returns>
        public override bool CheckValueINN(string INN) {
            Decimal innNum = 0;
            bool res = Decimal.TryParse(INN, out innNum);
            if (!res) return false;
            if (innNum <= 0) return false;
            return true;
        }

        #endregion
    }

}
