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
    /// Методы обработки результатов импорта
    /// </summary>
    //[NavigationItem("Finance")]
    //[DefaultProperty("CheckedPath")]
    [Persistent("fmImportResultPostProcess")]
    //[MapInheritance(MapInheritanceType.OwnTable)]
    public class fmImportResultPostProcess : csCComponent
    {
        protected internal fmImportResultPostProcess(Session ses)
            : base(ses) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmImportResultPostProcess);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private int LineNum = 0;

        private string fileName = "";
        private DateTime fileCreationTime = DateTime.MinValue;
        private string hashFile = "";

        private string CheckedPath;   // Проверяемый каталог, в который грузятся входные файлы
        private string TreatedPath;   // Каталог, в к-й сбрасываются отработанные файлы (пока не используется)

        private String FileFormat;   // Формат файла: 1C и т.п.
        private String FileBody;   // Содержимое файла

        private String FormatDateTime;
        private CultureInfo ProviderCultureInfo;
        private Int32 CodePageNum;
        
        private crmBank Bank = null;

        //fmStatementOfAccounts statementOfAccounts = null;   // Выписка

        XPCollection<fmStatementOfAccounts> statementOfAccountCol = null;

        fmImportResult importResult = null;

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Обработка каталога с файлами. Метод нужен пока для отладки метода обработки потока типа FileStream
        /// </summary>
        /// <returns></returns>
        //public override object Import(params object[] args) {
        public object Import(fmTaskImporter taskImporterFile) {   //, params object[] args) {
            
            //foreach (object obj in args) {
            //    if (obj as crmBank != null) {
            //        Bank = obj as crmBank;
            //        break;
            //    }
            //}


            // Настройка
            fmTaskImporterFile tif = (fmTaskImporterFile)taskImporterFile;

            CheckedPath = tif.CheckedPath;
            TreatedPath = tif.TreatedPath;

            FileFormat = tif.FileFormat;

            FormatDateTime = tif.FormatDateTime;
            ProviderCultureInfo = tif.ProviderCultureInfo;
            CodePageNum = tif.CodePageNum;

            Bank = tif.Bank;

            fileName = tif.CheckedPath;
            fileCreationTime = tif.SourceFileInfo.CreationTime;
            hashFile = tif.HashFile;


            // Объект результата импорта
            importResult = new fmImportResult(this.Session);
            importResult.TaskImporter = taskImporterFile;   // Запоминаем ссылку на задачу импорта
            importResult.FileName = fileName;
            importResult.FileDate = fileCreationTime;

            importResult.Hash = hashFile;
            importResult.ImportDate = System.DateTime.Now;

            // Исполнение алгоритма
            FileStream fs = tif.SourceFileStream;
            XPCollection<fmStatementOfAccounts> saCol = StreamProcess(fs);

            importResult.FileBody = FileBody;

            //using (FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {  //  File.OpenRead(fi.FullName)) {
            //    sa = StreamProcess(fs);
            //    fs.Close();
            //}

            /*  ПЕРЕНЕСЕНО В КОНТРОЛЛЕР
            // Постобработка: Добавление контрагентов, банков, счетов
            // Обработка выписки
            foreach (fmCDocRCB doc in sa.RCBDocuments) {
                // Контрагент-плательщик doc.PaymentPayerRequisites
                AddParty(doc.PaymentPayerRequisites);

                // Контрагент-получатель doc.PaymentReceiverRequisites
                AddParty(doc.PaymentReceiverRequisites);
            }
            */

            return importResult;
        }

        /// <summary>
        /// Постобработка документа: добавление контрагентов, банков, счетов
        /// </summary>
        /// <param name="doc"></param>
        public void PostProcess(fmCDocRCB doc) {
            // Контрагент-плательщик doc.PaymentPayerRequisites
            bool payerResult = AddParty(doc.PaymentPayerRequisites);

            // Контрагент-получатель doc.PaymentReceiverRequisites
            bool receiverResult = AddParty(doc.PaymentReceiverRequisites);

            // Отметка об успешности/неуспешности обработки
            doc.PostProcessResult = payerResult & receiverResult;
        }

        public bool AddParty(fmCDocRCBRequisites pr) {
            if (pr.INN == "") return false;

            CriteriaOperator criteriaAND = new GroupOperator();
            ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

            CriteriaOperator criteria1 = new BinaryOperator(new OperandProperty("INN"), new ConstantValue(pr.INN), BinaryOperatorType.Equal);
            CriteriaOperator criteria2 = new BinaryOperator(new OperandProperty("KPP"), new ConstantValue(pr.KPP), BinaryOperatorType.Equal);
            CriteriaOperator criteria3 = CriteriaOperator.Parse("'" + pr.KPP + "' != ''");

            ((GroupOperator)criteriaAND).Operands.Add(criteria1);
            ((GroupOperator)criteriaAND).Operands.Add(criteria2);
            ((GroupOperator)criteriaAND).Operands.Add(criteria3);

            crmCParty party = null;
            crmCLegalPerson lp = null;
            crmCLegalPersonUnit lpu = null;

            bool lpResult = true;

            party = this.Session.FindObject<crmCParty>(criteriaAND);
            if (party == null) {
                // Ищем только по ИНН
                lp = this.Session.FindObject<crmCLegalPerson>(criteria1);   // Поиск только по ИНН
                if (lp != null) {
                    lp.KPP = ReplaceWhiteSpace(lp.KPP, "-");
                    lp.Party.KPP = ReplaceWhiteSpace(lp.Party.KPP, "-");

                    // Здесь контраент crmCLegalPerson нашёлся по ИНН, но у него нет нужного КПП, надо добавить филиал
                    // Проверяем филиал на существование
                    if (!string.IsNullOrEmpty(ReplaceWhiteSpace(pr.KPP, ""))) {
                        lpu = this.Session.FindObject<crmCLegalPersonUnit>(criteriaAND);   // Поиск по ИНН и КПП
                        if (lpu == null) {
                            lpu = new crmCLegalPersonUnit(this.Session);
                            lpu.Party.INN = pr.INN;
                            lpu.Party.KPP = ReplaceWhiteSpace(pr.KPP, "-");
                            lpu.KPP = lpu.Party.KPP;
                            if (pr.NameParty != null) {
                                lpu.Party.Name = pr.NameParty.Substring(0, Math.Min(pr.NameParty.Length, 100));
                                lpu.Party.NameFull = pr.NameParty.Substring(0, Math.Min(pr.NameParty.Length, 100));
                            }
                            lpu.Party.Person = lp.Person;

                            lpu.Party.AddressFact.City = "-"; // Не обнаружено, откуда взять значение адреса для контрагентов
                            lpu.Party.AddressLegal.City = "-";
                            lpu.Party.AddressPost.City = "-";

                            lpu.LegalPerson = lp;
                            ((crmCLegalPerson)lp).LegalPersonUnits.Add(lpu);
                        }

                        pr.Party = lpu.Party;
                    } else {
                        // КПП не указано, не удалось создать филиал
                        lpResult = false;
                    }
                } else {
                    lpResult = false;
                }
            } else {
                party.KPP = ReplaceWhiteSpace(party.KPP, "-");
                pr.Party = party;
            }

            
            // Банк добавляем, если организация определилась или была создана. Поиск Банка производится по БИК
            bool bankResult = false;
            if (party != null && party.Person != null) {
                bankResult = BankProccess1(party.Person, ref pr);
            } else if (lpu != null && lpu.Party != null && lpu.Party.Person != null) {
                bankResult = BankProccess1(lpu.Party.Person, ref pr);
            }

            return lpResult & bankResult;
        }

        public bool BankProccess1(crmCPerson person, ref fmCDocRCBRequisites pr) {
            if (pr.RCBIC == "") return false;
            crmBankAccount PersonBankAccount = null;
            crmBank PersonBank = null;

            CriteriaOperator criteriaBank = new BinaryOperator(new OperandProperty("RCBIC"), new ConstantValue(pr.RCBIC), BinaryOperatorType.Equal);
            PersonBank = this.Session.FindObject<crmBank>(criteriaBank);

            // Поиск расчётного счёта
            if (PersonBank == null) {
                PersonBank = new crmBank(this.Session);
                //PersonBank.BIC = pr.BIC;
                PersonBank.RCBIC = pr.RCBIC;
                PersonBank.Name = pr.BankName;
            }

            CriteriaOperator criteriaAND = new GroupOperator();
            ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

            OperandProperty propBank = new OperandProperty("Bank");
            CriteriaOperator opBank = propBank == PersonBank;

            OperandProperty propPerson = new OperandProperty("Person");
            CriteriaOperator opPerson = propPerson == person;

            CriteriaOperator criteriaAccount = new BinaryOperator(new OperandProperty("Number"), new ConstantValue(pr.AccountParty), BinaryOperatorType.Equal);

            ((GroupOperator)criteriaAND).Operands.Add(opBank);
            ((GroupOperator)criteriaAND).Operands.Add(opPerson);
            ((GroupOperator)criteriaAND).Operands.Add(criteriaAccount);

            PersonBankAccount = this.Session.FindObject<crmBankAccount>(criteriaAND);
            if (PersonBankAccount == null) {
                PersonBankAccount = new crmBankAccount(this.Session);
                PersonBankAccount.Bank = PersonBank;
                PersonBankAccount.Person = person;
                PersonBankAccount.Number = pr.AccountParty;
            }
            pr.BankAccount = PersonBankAccount;

            return true;
        }

        /// <summary>
        /// Определение банков по данным из рексвизитов. Если банк отсутствует в системе, то он создаётся.
        /// </summary>
        /// <param name="ir"></param>
        public void BankProccess(fmImportResult ir) {
            // Определение банков
            foreach (fmStatementOfAccounts sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                    if (requisites.RCBIC != "") {
                        crmBank PersonBank = null;

                        // Поиск банка
                        CriteriaOperator criteriaBank = new BinaryOperator(new OperandProperty("RCBIC"), new ConstantValue(ReplaceWhiteSpace(requisites.RCBIC, "")), BinaryOperatorType.Equal);
                        PersonBank = this.Session.FindObject<crmBank>(criteriaBank);
                        if (PersonBank == null) {
                            PersonBank = new crmBank(this.Session);
                            //PersonBank.BIC = requisites.BIC;
                            PersonBank.RCBIC = requisites.RCBIC;
                            PersonBank.Name = requisites.BankName;
                            PersonBank.Save();
                        }
                        requisites.Bank = PersonBank;


                        //XPQuery<crmBank> banks = new XPQuery<crmBank>(this.Session);
                        //var queryBanks = from bank in banks
                        //                 where bank.RCBIC == ReplaceWhiteSpace(requisites.RCBIC, "")
                        //                 select bank;
                        //bool BankExists = false;
                        //foreach (var bank in queryBanks) {
                        //    BankExists = true;
                        //    PersonBank = bank;
                        //    break;
                        //}
                        //if (!BankExists) {
                        //    PersonBank = new crmBank(this.Session);
                        //    //PersonBank.BIC = requisites.BIC;
                        //    PersonBank.RCBIC = requisites.RCBIC;
                        //    PersonBank.Name = requisites.BankName;
                        //}
                        //requisites.Bank = PersonBank;

                    } else {
                        // ОБРАБОТКА ImportErrorState код 1
                    }
                }
            }
        }


        /// <summary>
        /// Определение банков по данным из рексвизитов. Если банк отсутствует в системе, то он создаётся.
        /// </summary>
        /// <param name="ir"></param>
        public void RequisitesBankProccess(fmCDocRCBRequisites requisites) {
            // Определение банков
            if (requisites.RCBIC != "") {
                crmBank PersonBank = null;

                // Поиск банка
                CriteriaOperator criteriaBank = new BinaryOperator(new OperandProperty("RCBIC"), new ConstantValue(ReplaceWhiteSpace(requisites.RCBIC, "")), BinaryOperatorType.Equal);
                PersonBank = this.Session.FindObject<crmBank>(criteriaBank);
                if (PersonBank == null) {
                    PersonBank = new crmBank(this.Session);
                    //PersonBank.BIC = requisites.BIC;
                    PersonBank.RCBIC = requisites.RCBIC;
                    PersonBank.Name = requisites.BankName;
                    PersonBank.Save();
                }
                requisites.Bank = PersonBank;
            } else {
                // ОБРАБОТКА ImportErrorState код 1
            }
        }


        /// <summary>
        /// Определение стороны как предпочтительной стороны по банку и счёту из BankAccount.
        /// При наличии заданных ИНН и КПП производится контрольная проверка равенства реквизитов ИНН и КПП предпочтительной
        /// стороны и указанных в документе, также проверяется, чтобы предпочтительная сторона не была закрытой.
        /// </summary>
        /// <param name="ir"></param>
        public void PartyProccessByAccount(fmImportResult ir) {
            // Во вторую очередь производится определение стороны как предпочтительной стороны по банку и счёту из BankAccount. 
            // При наличии заданных ИНН и КПП производится контрольная проверка равенства реквизитов ИНН и КПП предпочтительной
            // стороны и указанных в документе, также проверяется, чтобы предпочтительная сторона не была закрытой. 

            // Определение стороны
            foreach (fmStatementOfAccounts sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {

                    XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
                    var query = from bankAccount in bankAccounts
                                where bankAccount.Number == sa.BankAccountText & bankAccount.Bank == requisites.Bank
                                select bankAccount;

                    foreach (var ba in query) {
                        if (ba.PrefferedParty != null) {
                            if (!ba.PrefferedParty.IsClosed) {

                                // Проверка по ИНН и КПП
                                if (!string.IsNullOrEmpty(requisites.INN) && !string.IsNullOrEmpty(requisites.KPP)) {
                                    XPQuery<crmCParty> parties = new XPQuery<crmCParty>(this.Session);
                                    var queryParties = from party in parties
                                                       where party.INN == requisites.INN & party.KPP == requisites.KPP & !party.IsClosed
                                                       select party;
                                    foreach (var p in queryParties) {
                                        if (ba.PrefferedParty.INN == p.INN && ba.PrefferedParty.KPP == p.KPP) {
                                            // Всё нормально
                                            requisites.Party = p;
                                            requisites.Person = p.Person;
                                        } else {
                                            // ОБРАБОТКА (Предпочтительная сторона не сопадает с заданной по ИНН и КПП) ImportErrorState код 2
                                        }
                                        break;
                                    }
                                } else {
                                    requisites.Party = ba.PrefferedParty;
                                    requisites.Person = ba.PrefferedParty.Person;
                                }
                            } else {
                                // ОБРАБОТКА (Предпочтительная сторона закрыта) ImportErrorState код 4
                            }
                        } else {
                            // ОБРАБОТКА (Предпочтительная сторона не определена) ImportErrorState код 8
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Определение стороны по ИНН и КПП
        /// </summary>
        /// <param name="ir"></param>
        public void PartyProccessByINNandKPP(fmImportResult ir) {
            // Во вторую очередь производится определение стороны как предпочтительной стороны по банку и счёту из BankAccount. 
            // При наличии заданных ИНН и КПП производится контрольная проверка равенства реквизитов ИНН и КПП предпочтительной
            // стороны и указанных в документе, также проверяется, чтобы предпочтительная сторона не была закрытой. 

            // Определение стороны
            foreach (fmStatementOfAccounts sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                    if (requisites.Party != null) continue;

                    // Поиск по ИНН и КПП
                    if (!string.IsNullOrEmpty(requisites.INN)) {
                        if (requisites.INN.Length == 10) {
                            if (!string.IsNullOrEmpty(requisites.KPP)) {
                            
                                XPQuery<crmCParty> parties = new XPQuery<crmCParty>(this.Session);
                                var queryParties = from party in parties
                                                   where party.INN == requisites.INN & party.KPP == requisites.KPP //& !party.IsClosed
                                                   select party;
                                foreach (var p in queryParties) {
                                    if (!p.IsClosed) {
                                        requisites.Party = p;
                                        requisites.Person = p.Person;
                                    } else {
                                        // ОБРАБОТКА (Найденная сторона оказалась закрытой) ImportErrorState код 16
                                    }
                                    break;
                                }

                                // Не найдена сторона по ИНН и КПП
                                if (requisites.Party == null) {

                                    XPQuery<crmCLegalPerson> legalPersons = new XPQuery<crmCLegalPerson>(this.Session);
                                    var queryLegalPerson = from legalPerson in legalPersons
                                                           where legalPerson.INN == requisites.INN //& legalPerson.KPP == requisites.KPP
                                                           select legalPerson;

                                    crmCLegalPerson lPerson = null;
                                    foreach (var lp in queryLegalPerson) {
                                        if (!lp.IsClosed) {
                                            lPerson = lp;
                                        } else {
                                            // ОБРАБОТКА (Найденное юридическое лицо оказалаоь закрытым) ImportErrorState код 32
                                        }
                                        break;
                                    }

                                    if (lPerson != null) { // Юридическое лицо найдено

                                        // Поиск филиала
                                        OperandProperty prop = new OperandProperty("LegalPerson");
                                        CriteriaOperator op = prop == lPerson;
                                        CriteriaOperator criteriaPersonUnit = new BinaryOperator(new OperandProperty("KPP"), new ConstantValue(ReplaceWhiteSpace(requisites.KPP, "")), BinaryOperatorType.Equal);

                                        CriteriaOperator criteriaAND = new GroupOperator();
                                        ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

                                        ((GroupOperator)criteriaAND).Operands.Add(op);
                                        ((GroupOperator)criteriaAND).Operands.Add(criteriaPersonUnit);

                                        crmCLegalPersonUnit lpu = this.Session.FindObject<crmCLegalPersonUnit>(criteriaAND);
                                        if (lpu == null) {
                                            // Создание филиала
                                            lpu = new crmCLegalPersonUnit(this.Session);
                                            lpu.Party.INN = requisites.INN;
                                            lpu.Party.KPP = ReplaceWhiteSpace(requisites.KPP, "-");
                                            lpu.KPP = lpu.Party.KPP;
                                            if (requisites.NameParty != null) {
                                                lpu.Party.Name = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                                lpu.Party.NameFull = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                            }
                                            lpu.Party.Person = lPerson.Person;

                                            lpu.Party.AddressFact.City = "-"; // Не обнаружено, откуда взять значение адреса для контрагентов
                                            lpu.Party.AddressLegal.City = "-";
                                            lpu.Party.AddressPost.City = "-";
                                            // ОБРАБОТКА (ПОметить, что надо доопределить адрес)  ImportErrorState код 64

                                            lpu.LegalPerson = lPerson;
                                            lPerson.LegalPersonUnits.Add(lpu);

                                            requisites.Party = lpu.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                            requisites.Person = lPerson.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?

                                            lpu.Save();
                                        }




                                        /*
                                        // Ищем филиал с заданным КПП
                                        XPQuery<crmCLegalPersonUnit> legalPersonUnits = new XPQuery<crmCLegalPersonUnit>(this.Session);
                                        var queryLegalPersonUnits = from legalPersonUnit in legalPersonUnits
                                                                    where legalPersonUnit.LegalPerson == lPerson & legalPersonUnit.KPP == ReplaceWhiteSpace(requisites.KPP, "")
                                                                    select legalPersonUnit;

                                        bool unitExists = false;
                                        foreach (var lpu in queryLegalPersonUnits) {
                                            unitExists = true;
                                            requisites.Party = lpu.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                            requisites.Person = lpu.Party.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                            break;
                                        }

                                        if (!unitExists) {
                                            // Создание филиала
                                            crmCLegalPersonUnit lpu = new crmCLegalPersonUnit(this.Session);
                                            lpu.Party.INN = requisites.INN;
                                            lpu.Party.KPP = ReplaceWhiteSpace(requisites.KPP, "-");
                                            lpu.KPP = lpu.Party.KPP;
                                            if (requisites.NameParty != null) {
                                                lpu.Party.Name = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                                lpu.Party.NameFull = requisites.NameParty.Substring(0, Math.Min(requisites.NameParty.Length, 100));
                                            }
                                            lpu.Party.Person = lPerson.Person;

                                            lpu.Party.AddressFact.City = "-"; // Не обнаружено, откуда взять значение адреса для контрагентов
                                            lpu.Party.AddressLegal.City = "-";
                                            lpu.Party.AddressPost.City = "-";
                                            // ОБРАБОТКА (ПОметить, что надо доопределить адрес)  ImportErrorState код 64

                                            lpu.LegalPerson = lPerson;
                                            lPerson.LegalPersonUnits.Add(lpu);

                                            requisites.Party = lpu.Party;   // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?
                                            requisites.Person = lPerson.Person;  // ???????????????? или приводить к типу crmIParty ? Или надо через CID ?

                                            lpu.Save();
                                        }
                                        */
                                    } else {
                                        // ОБРАБОТКА (Не найдено юридическое лицо) ImportErrorState код 128
                                    }


                                    // Всё равно сторона осталась неопределённой
                                    if (requisites.Party == null) {
                                        // ОБРАБОТКА (Отметка, что сторона не определена) ImportErrorState код 256
                                    }
                                }

                            } else {
                                // ОБРАБОТКА (Не задано КПП) ImportErrorState код 512
                            }
                        } else if (requisites.INN.Length == 12) {
                            // Ищем частное лицо
                            XPQuery<crmCBusinessman> Businessmans = new XPQuery<crmCBusinessman>(this.Session);
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
                                XPQuery<crmCPhysicalParty> PhysicalParties = new XPQuery<crmCPhysicalParty>(this.Session);
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
                                }
                            }
                        }
                    } else {
                        // ТРЕБУЕТСЯ ДАЛЬНЕЙШЕЕ ИССЛЕДОВАНИЕ
                    }

                }
            }
        }

        
        /// <summary>
        /// Создание счетов
        /// </summary>
        /// <param name="ir"></param>
        public void StatementAccountProccess(fmImportResult ir) {
            // Определение стороны
            foreach (fmStatementOfAccounts sa in ir.StatementOfAccounts) {
                foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                    if (requisites.BankAccount != null) continue;
                    if (requisites.Party == null || string.IsNullOrEmpty(requisites.AccountBank) || requisites.Bank == null) {
                        // ОБРАБОТКА (Доопределить недостающие объекты и создать расчётный счёт) ImportErrorState код 2048
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

        /// <summary>
        /// Обработка потока
        /// </summary>
        /// <param name="fs"></param>
        public XPCollection<fmStatementOfAccounts> StreamProcess(FileStream fs) {
            // Какая кодировка ?
            Int32 fileLen = (Int32)fs.Length;
            byte[] mfile = new byte[fileLen];

            fs.Read(mfile, 0, fileLen);

            // Запоминание содержимого потока
            FileBody = Encoding.GetEncoding(this.CodePageNum).GetString(mfile);

            bool StatementOfAccountsAchieved = false;
            bool FirstDocumentSectionAchieved = false;

            // Коллекция создаваемых выписок
            statementOfAccountCol = new XPCollection<fmStatementOfAccounts>(this.Session);

            //using (StreamReader sr = new StreamReader(fs)) {
            LineNum = 0;
            fs.Position = 0;
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(this.CodePageNum));

            WriteLog("Начало обработки файла: " + fileName);

            //// Обработка секции Выписка по счёту
            //// Секций СекцияРасчСчет может быть несколько
            //CreateObjectStatementOfAccounts(mfile);

            StatementOfAccountsAchieved = false;

            string nextLine = "";

            // Цикл по созданию выписок по расчётным считам, загружаемым из потока.
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line.Contains("СекцияРасчСчет")) {
                    StatementOfAccountsAchieved = true;
                } else if (line.Contains("СекцияДокумент")) {
                    nextLine = line;
                    break;
                }
                if (!StatementOfAccountsAchieved) continue;

                // Создание выписки
                fmStatementOfAccounts statementOfAccount = CreateStatementOfAccounts(this.Session, ref sr, mfile);   //, ref statementOfAccounts);
                if (statementOfAccount != null) {
                    statementOfAccount.ImportResult = importResult;
                    importResult.StatementOfAccounts.Add(statementOfAccount);
                }

            }

            // Проверка счетов в выписках на их наличие в системе
            if (CheckExistsAllStatementAccountsInSystem(statementOfAccountCol)) {

                // Запоминаем коллекцию выписок
                //importResult.StatementOfAccounts = statementOfAccountCol;

                // Сразу после последней секции СекцияРасчСчет следует СекцияДокумент 

                // Обработка секций документов. Считаем, что секция СекцияРасчСчет - КонецРасчСчет всегда имеется
                // Это нужно для объекта sr (непрерывной последовательности его работы)
                while (!sr.EndOfStream) {
                    string line = "";
                    if (nextLine != "") {
                        line = nextLine;
                        nextLine = "";
                    } else {
                        line = sr.ReadLine();
                        LineNum++;
                    }

                    if (line.Contains("СекцияДокумент")) FirstDocumentSectionAchieved = true;

                    if (!FirstDocumentSectionAchieved) continue;

                    int eqPos = line.IndexOf("=");
                    if (eqPos >= 0) {   // Пропуск всех пока не нужных записей в файле
                        string key = line.Substring(0, eqPos);
                        string item = line.Substring(eqPos + 1);

                        if (key == "СекцияДокумент" & item == "Платежное поручение") {
                            CreatePaymentOrder(this.Session, ref sr);  //, ref statementOfAccounts);
                            //CreateBudgetPaymentOrder(this.Session, ref sr, statementOfAccounts);
                        } else if (key == "СекцияДокумент" & item == "Платежное требование") {
                            CreatePaymentRequest(this.Session, ref sr);   //, ref statementOfAccounts);
                        } else if (key == "СекцияДокумент" & item == "Аккредитив") {
                            CreateAkkreditivRequest(this.Session, ref sr);   //, ref statementOfAccounts);
                        } else if (key == "СекцияДокумент" & item == "Инкассовое поручение") {
                            CreateInkassOrder(this.Session, ref sr);   //, ref statementOfAccounts);
                        } else {
                            //CreateOtherOrder(this.Session, ref sr, ref statementOfAccounts, item);
                            CreateOtherOrder(this.Session, ref sr, item);
                        }
                    }
                }

                // Отметка об успешной загрузке
                importResult.IsImported = true;

                WriteLog("Успешно завершена обработка файла: " + fileName);

            } else {
                // Если хотя бы одни счёт из выписок не был опознан, обработку прекращаем и делаем следующее:
                // Устанавливаем битовый признак этой ошибки.
                // В журнал событий загрузки пишем сообщение об этом.
                // Процесс обработки прекращается.

                WriteLog("Обнаружен счёт, не зарегистрированный в системе. Продолжение импорта невозможно");

                // Отметка об успешной загрузке
                importResult.IsImported = false;

                // Чистим коллекцию и удаляем все выписки
               //importResult.StatementOfAccounts

                WriteLog("Обработка файла прекращена: " + fileName);

            }
            //}

            sr.Close();

            return statementOfAccountCol;
        }

        private bool CheckExistsAllStatementAccountsInSystem(XPCollection<fmStatementOfAccounts>  statementOfAccountCol) {

            bool CommonResult = true;

            // Наша организация
            crmCParty OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }

            foreach (fmStatementOfAccounts sa in statementOfAccountCol) {
                XPQuery<crmBankAccount> bankAccounts = new XPQuery<crmBankAccount>(this.Session);
                var query = from bankAccount in bankAccounts
                            where bankAccount.Number == sa.BankAccountText & bankAccount.Bank == importResult.TaskImporter.Bank //& bankAccount.Person == OurParty.Person
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
                    WriteLog("Cчёт " + sa.BankAccountText + ", банк " + importResult.TaskImporter.Bank.FullName + "(БИК " + importResult.TaskImporter.Bank.RCBIC + ") найден в системе.");
                } else {
                    WriteLog("Cчёт " + sa.BankAccountText + ", банк " + importResult.TaskImporter.Bank.FullName + "(БИК " + importResult.TaskImporter.Bank.RCBIC  + "), не зарегистрирован в системе.");
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
        private fmStatementOfAccounts CreateStatementOfAccounts(Session ssn, ref StreamReader sr, byte[] mfile) {   //, ref fmStatementOfAccounts statementOfAccounts) {
            //fmStatementOfAccountInfo statementOfAccountInfo = new fmStatementOfAccountInfo(ssn);
            fmStatementOfAccounts statementOfAccountInfo = new fmStatementOfAccounts(ssn);

            //// Общие поля выписки
            //statementOfAccountInfo.ImportResult = importResult;
            //importResult.StatementOfAccounts.Add(statementOfAccountInfo);

            statementOfAccountInfo.DocDate = System.DateTime.Now;
            statementOfAccountInfo.DocNumber = "Выписка по счёту № " + "&Account" + " за период " + "&StatementDateFrom - &StatementDateTo";
            
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецРасчСчет") {
                    //statementOfAccounts.AccountInfo.Add(statementOfAccountInfo);
                    //statementOfAccountInfo.StatementOfAccounts = statementOfAccounts;
                    statementOfAccountCol.Add(statementOfAccountInfo);
                    break;
                } else {
                    if (!line.Contains("=")) continue;

                    int eqPos = line.IndexOf("=");
                    string key = line.Substring(0, eqPos);
                    string item = line.Substring(eqPos + 1);
                    
                    switch (key) {
                        case "ДатаНачала":
                            DateTime dateFrom;
                            if (ConvertStringToDateTime(item, out dateFrom)) {
                                statementOfAccountInfo.DateFrom = dateFrom;
                                statementOfAccountInfo.DocNumber = statementOfAccountInfo.DocNumber.Replace("&StatementDateFrom", ReplaceWhiteSpace(item, "-"));
                            }
                            break;
                        case "ДатаКонца":
                            DateTime dateTo;
                            if (ConvertStringToDateTime(item, out dateTo)) {
                                statementOfAccountInfo.DateTo = dateTo;
                                statementOfAccountInfo.DocNumber = statementOfAccountInfo.DocNumber.Replace("&StatementDateTo", ReplaceWhiteSpace(item, "__.__.____"));
                            }
                            break;
                        case "РасчСчет":
                            statementOfAccountInfo.BankAccountText = item;
                            statementOfAccountInfo.DocNumber = statementOfAccountInfo.DocNumber.Replace("&Account", ReplaceWhiteSpace(item, "__.__.____"));
                            break;
                        case "НачальныйОстаток":
                            Decimal balanceOfIncoming;
                            if (ConvertStringToDecimal(item, out balanceOfIncoming)) {
                                statementOfAccountInfo.BalanceOfIncoming = balanceOfIncoming;
                            }
                            break;
                        case "ВсегоПоступило":
                            Decimal totalRecaivedAtAccount;
                            if (ConvertStringToDecimal(item, out totalRecaivedAtAccount)) {
                                statementOfAccountInfo.TotalRecaivedAtAccount = totalRecaivedAtAccount;
                            }
                            break;
                        case "ВсегоСписано":
                            Decimal totalWriteOfAccount;
                            if (ConvertStringToDecimal(item, out totalWriteOfAccount)) {
                                statementOfAccountInfo.TotalWriteOfAccount = totalWriteOfAccount;
                            }
                            break;
                        case "КонечныйОстаток":
                            Decimal balanceOfOutgoing;
                            if (ConvertStringToDecimal(item, out balanceOfOutgoing)) {
                                statementOfAccountInfo.BalanceOfOutgoing = balanceOfOutgoing;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return statementOfAccountInfo;
        }
        #endregion


        #region Создание платёжки: CreatePaymentOrder
        private void CreatePaymentOrder(Session ssn, ref StreamReader sr) { //, ref fmStatementOfAccounts statementOfAccounts) {
            fmCDocRCBPaymentOrder loadedDoc = new fmCDocRCBPaymentOrder(ssn);
            fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            //loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку

            WriteLog("Платёжное поручение");

            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(ref commonLoadedDoc, line); // , statementOfAccounts);
            }
            WriteLog("Завершена обработка Платёжного поручения");
        }
        #endregion


        #region Создание платёжного требования: CreatePaymentRequest
        private void CreatePaymentRequest(Session ssn, ref StreamReader sr) {   //, ref fmStatementOfAccounts statementOfAccounts) {
            fmCDocRCBPaymentRequest loadedDoc = new fmCDocRCBPaymentRequest(ssn);
            fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            ////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            //// Ссылка на выписку
            //statementOfAccounts.RCBDocuments.Add(loadedDoc);
            //loadedDoc.StatementOfAccounts = statementOfAccounts;

            WriteLog("Платёжное требование");

            while (sr.Peek() >= 0) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(ref commonLoadedDoc, line); ///, statementOfAccounts);
            }
            WriteLog("Завершена обработка Платёжного требования");
        }
        #endregion


        #region Создание заявление на аккредити: CreateAkkreditivRequest
        private void CreateAkkreditivRequest(Session ssn, ref StreamReader sr) {   //, ref fmStatementOfAccounts statementOfAccounts) {
            fmCDocRCBAkkreditivRequest loadedDoc = new fmCDocRCBAkkreditivRequest(ssn);
            fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            ////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            //// Ссылка на выписку
            //statementOfAccounts.RCBDocuments.Add(loadedDoc);
            //loadedDoc.StatementOfAccounts = statementOfAccounts;

            WriteLog("Аккредитив");

            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(ref commonLoadedDoc, line); // , statementOfAccounts);
            }
            WriteLog("Завершена обработка Аккредитива");
        }
        #endregion


        #region Создание инкассовое поручение: CreateInkassOrder
        private void CreateInkassOrder(Session ssn, ref StreamReader sr) {   //, ref fmStatementOfAccounts statementOfAccounts) {
            fmCDocRCBInkassOrder loadedDoc = new fmCDocRCBInkassOrder(ssn);
            fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            ////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            //// Ссылка на выписку
            //statementOfAccounts.RCBDocuments.Add(loadedDoc);
            //loadedDoc.StatementOfAccounts = statementOfAccounts;

            WriteLog("Инкассовое поручение");

            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(ref commonLoadedDoc, line); //, statementOfAccounts);
            }
            WriteLog("Завершена обработка Инкассового поручения");
        }
        #endregion


        #region Создание Прочих документов стандартным образом: CreateOtherOrder
        //private void CreateOtherOrder(Session ssn, ref StreamReader sr, ref fmStatementOfAccounts statementOfAccounts, string docTypes) {
        private void CreateOtherOrder(Session ssn, ref StreamReader sr, string docTypes) {
            fmCDocRCBOthers loadedDoc = new fmCDocRCBOthers(ssn);
            fmCDocRCB commonLoadedDoc = (fmCDocRCB)loadedDoc;
            loadedDoc.DocType = docTypes;
            ////loadedDoc.StatementAccount = statementOfAccounts;   // Ссылка на выписку
            //// Ссылка на выписку
            //statementOfAccounts.RCBDocuments.Add(loadedDoc);
            //loadedDoc.StatementOfAccounts = statementOfAccounts;

            WriteLog("Документ Прочие");

            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                LineNum++;

                if (line == "КонецДокумента" && loadedDoc != null) {
                    break;
                }

                if (!line.Contains("=")) continue;

                CommonDocPart(ref commonLoadedDoc, line);  //, statementOfAccounts);
                /*
                int eqPos = line.IndexOf("=");
                string key = line.Substring(0, eqPos);
                string item = line.Substring(eqPos + 1);

                switch (key) {
                    case "СекцияДокумент":
                        loadedDoc.DocType = item;
                        WriteLog("Тип документа прочие: " + item);
                        break;
                    default:
                        break;
                }
                */
            }
            WriteLog("Завершена обработка Документа Прочие");
        }
        #endregion


        #region Обработка общей части документов выписки: CommonDocPart

        /// <summary>
        /// Нахождение выписки по номеру счёта
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private fmStatementOfAccounts FindStatementOfAccount(string account) {
            // Находим подходящую выписку
            foreach (fmStatementOfAccounts sa in statementOfAccountCol) {
                if (sa.BankAccountText == account) {
                    return sa;
                }
            }
            return null;
        }

        private void CommonDocPart(ref fmCDocRCB loadedDoc, string line) { //, fmStatementOfAccounts statementOfAccounts) {

            int eqPos = line.IndexOf("=");
            string key = line.Substring(0, eqPos);
            string item = line.Substring(eqPos + 1);

            switch (key) {
                case "Номер":
                    loadedDoc.DocNumber = item;
                    WriteLog("Номер: " + item);
                    break;
                case "Дата":
                    DateTime docDate;
                    if (ConvertStringToDateTime(item, out docDate)) {
                        loadedDoc.DocDate = docDate;
                        WriteLog("Дата: " + item);
                    }
                    break;
                case "Сумма":
                    Decimal paymentCost;
                    if (ConvertStringToDecimal(item, out paymentCost)) {
                        loadedDoc.PaymentCost = paymentCost;
                    }
                    break;
                case "ПлательщикСчет":
                    loadedDoc.PaymentPayerRequisites.AccountParty = item;

                    fmStatementOfAccounts saPayer = FindStatementOfAccount(item);
                    if (saPayer != null) {
                        saPayer.DocRCBRequisites.Add(loadedDoc.PaymentPayerRequisites);
                        loadedDoc.PaymentPayerRequisites.StatementOfAccount = saPayer;
                    }
                    break;
                case "ДатаСписано":
                    DateTime deductedFromPayerAccount;
                    if (ConvertStringToDateTime(item, out deductedFromPayerAccount)) {
                        loadedDoc.DeductedFromPayerAccount = deductedFromPayerAccount;
                    }
                    break;
                case "ДатаПоступило":
                    DateTime receivedByPayerBankDate;
                    if (ConvertStringToDateTime(item, out receivedByPayerBankDate)) {
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
                        } else {
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
                    } else {
                        loadedDoc.PaymentPayerRequisites.BankName = loadedDoc.PaymentPayerRequisites.BankName + " " + item;
                    }
                    break;
                case "ПлательщикБанк2":
                    loadedDoc.PaymentPayerRequisites.BankLocation = item;
                    if (string.IsNullOrEmpty(loadedDoc.PaymentPayerRequisites.BankName)) {
                        loadedDoc.PaymentPayerRequisites.BankName = item;
                    } else {
                        loadedDoc.PaymentPayerRequisites.BankName = item + " " + loadedDoc.PaymentPayerRequisites.BankName;
                    }
                    break;
                case "ПлательщикБИК":
                    loadedDoc.PaymentPayerRequisites.RCBIC = item;
                    break;
                case "ПлательщикКорсчет":
                    loadedDoc.PaymentPayerRequisites.AccountBank = item;
                    break;
                case "ПолучательСчет":
                    loadedDoc.PaymentReceiverRequisites.AccountParty = item;

                    fmStatementOfAccounts saReceiver = FindStatementOfAccount(item);
                    if (saReceiver != null) {
                        saReceiver.DocRCBRequisites.Add(loadedDoc.PaymentPayerRequisites);
                        loadedDoc.PaymentPayerRequisites.StatementOfAccount = saReceiver;
                    }
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
                        } else {
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
                    } else {
                        loadedDoc.PaymentReceiverRequisites.BankName = loadedDoc.PaymentReceiverRequisites.BankName + " " + item;
                    }
                    break;
                case "ПолучательБанк2":
                    loadedDoc.PaymentReceiverRequisites.BankLocation = item;
                    if (string.IsNullOrEmpty(loadedDoc.PaymentReceiverRequisites.BankName)) {
                        loadedDoc.PaymentReceiverRequisites.BankName = item;
                    } else {
                        loadedDoc.PaymentReceiverRequisites.BankName = item + " " + loadedDoc.PaymentReceiverRequisites.BankName;
                    }
                    break;
                case "ПолучательБИК":
                    loadedDoc.PaymentReceiverRequisites.RCBIC = item;
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
                    if (ConvertStringToInt16(item, out acceptanceDuration)) {
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
                    if (ConvertStringToDateTime(item, out documentSendingDate)) {
                        loadedDoc.DocumentSendingDate = documentSendingDate;
                    }
                    break;


                case "КвитанцияДата":
                    DateTime ticketDate;
                    if (ConvertStringToDateTime(item, out ticketDate)) {
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
        /// Преобразование даты из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        private bool ConvertStringToDateTime(string dtString, out DateTime res) {
            try {
                res = DateTime.ParseExact(dtString, this.FormatDateTime, this.ProviderCultureInfo);
                return true;
            } catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(err);
            } catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(err);
            }
            res = DateTime.MinValue;
            return false;
        }

        /// <summary>
        /// Преобразование числа из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        private bool ConvertStringToDecimal(string dtString, out Decimal res) {
            try {
                res = Decimal.Parse(dtString, this.ProviderCultureInfo.NumberFormat);
                return true;
            } catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(err);
            } catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(err);
            }
            res = Decimal.MinValue;
            return false;
        }

        /// <summary>
        /// Преобразование целого числа из строки
        /// </summary>
        /// <param name="dtString"></param>
        /// <returns></returns>
        private bool ConvertStringToInt16(string dtString, out Int16 res) {
            try {
                res = Int16.Parse(dtString, this.ProviderCultureInfo.NumberFormat);
                return true;
            } catch (FormatException fEx) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), fEx.ToString());
                //throw new Exception(err);
                WriteLog(err);
            } catch (Exception Ex) {
                string err = String.Format("Error is detected. File: '{0}'. Line: {1}. " + "System message:  {2}", fileName, LineNum.ToString(), Ex.ToString());
                //throw new Exception(err);
                WriteLog(err);
            }
            res = Int16.MinValue;
            return false;
        }

        /// <summary>
        /// Запись в журнал
        /// </summary>
        /// <param name="line"></param>
        private void WriteLog(string line) {
            importResult.WriteLog(line);
        }

        private string AddLine(string line, string sourceText) {
            string res = sourceText;
            if (!string.IsNullOrEmpty(line)) {
                res += ((string.IsNullOrEmpty(sourceText)) ? "" : Environment.NewLine) + line;
            }
            return res;
        }

        private string ReplaceWhiteSpace(string line, string outline) {
            if (string.IsNullOrEmpty(Regex.Replace(line, @"\s+", ""))) return outline;
            return line;
        }

        #endregion
    }

}
