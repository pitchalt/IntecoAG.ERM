using System;
using System.Collections.Generic;
using System.Collections;
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
using IntecoAG.ERM.FM.Docs.Analitic;

namespace IntecoAG.ERM.FM.StatementAccount {

    /// <summary>
    /// Методы распознавания и сопоставления платёжных документов
    /// </summary>
    [NonPersistent]
    public class fmMapDocs<T> : csCComponent where T : fmCDocRCB
    {
        public fmMapDocs(Session ses)
            : base(ses) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmMapDocs<T>);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Сопоставление документов выписок и платёжек по абсолютному критерию: номер документа, дата документа, счёт плательщика, банк плательщика, сумма
        /// </summary>
        /// <param name="ir"></param>
        public void HardMappingProcess() {
            // Перебор всех Платёжных документов

            XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            XPQuery<fmCSAStatementAccountDoc> accDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryPaymentDocs = from paymentDoc in paymentDocs
                                   join accDoc in accDocs on paymentDoc equals accDoc.PaymentDocument
                                   //
                                   where accDoc == null

//                                   !(from statementDoc in paymentDocs
//                                      select statementDoc.PaymentDocument).Contains(paymentDoc)
                                   select paymentDoc;
            
            foreach (var doc in queryPaymentDocs) {
                XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                var queryStatementDocs = from statementDoc in statementDocs
                                         where statementDoc.PaymentDocument == null   // документ ещё не привязан
                                            && statementDoc.DocDate == doc.DocDate   // Дата документа
                                            && statementDoc.DocNumber == doc.DocNumber   // Номер документа
                                            && statementDoc.PaymentCost == doc.PaymentCost   // Сумма
                                            && statementDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty    // Номер счёта плательщика
                                            && statementDoc.PaymentPayerRequisites.Bank == doc.PaymentPayerRequisites.Bank   // Банк плательщика
                                         select statementDoc;
                // Добавить проверку единственности !!!
                foreach (var statementDoc in queryStatementDocs) {
                    statementDoc.PaymentDocument = doc;

                    // Возможно, здесь надо провести корректировку значений свойств Платёжного документа doc по значениям аналогичных свойств документа выписки statementDoc
                    break;
                }
            }
        }


        /// <summary>
        /// Сопоставление документов выписок и платёжек по абсолютному критерию: номер документа, дата документа, счёт плательщика, банк плательщика, сумма
        /// </summary>
        /// <param name="ir"></param>
        public void HardMappingProcess(T doc) {
            XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryStatementDocs = from statementDoc in statementDocs
                                        where statementDoc.PaymentDocument == null   // документ ещё не привязан
                                        && statementDoc.DocDate == doc.DocDate   // Дата документа
                                        && statementDoc.DocNumber == doc.DocNumber   // Номер документа
                                        && statementDoc.PaymentCost == doc.PaymentCost   // Сумма
                                        && statementDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty    // Номер счёта плательщика
                                        && statementDoc.PaymentPayerRequisites.Bank == doc.PaymentPayerRequisites.Bank   // Банк плательщика
                                        select statementDoc;
            // Добавить проверку единственности !!!
            foreach (var statementDoc in queryStatementDocs) {
                statementDoc.PaymentDocument = doc;

                // Возможно, здесь надо провести корректировку значений свойств Платёжного документа doc по значениям аналогичных свойств документа выписки statementDoc
                break;
            }
        }

        /// <summary>
        /// Сопоставление документов выписок и платёжек по нестрогому (decent)  критерию: номер документа, дата документа, счёт плательщика, банк плательщика
        /// </summary>
        /// <param name="ir"></param>
        public void DecentMappingProcess() {
            //// Перебор всех Платёжных документов

            //XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            //var queryPaymentDocs = from paymentDoc in paymentDocs

            //                       && !(from statementDoc in paymentDocs
            //                            select statementDoc.PaymentDocument).Contains(paymentDoc)
            //                       select paymentDoc;

            //foreach (var doc in queryPaymentDocs) {
            //    XPQuery<T> statementDocs = new XPQuery<T>(this.Session);
            XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            XPQuery<fmCSAStatementAccountDoc> accDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryPaymentDocs = from paymentDoc in paymentDocs
                                   join accDoc in accDocs on paymentDoc equals accDoc.PaymentDocument
                                   //
                                   where accDoc == null

                                   //                                   !(from statementDoc in paymentDocs
                                   //                                      select statementDoc.PaymentDocument).Contains(paymentDoc)
                                   select paymentDoc;

            foreach (var doc in queryPaymentDocs) {
                XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                var queryStatementDocs = from statementDoc in statementDocs
                                         where statementDoc.PaymentDocument == null   // документ ещё не привязан
                                            && statementDoc.DocDate == doc.DocDate   // Дата документа
                                            && statementDoc.DocNumber == doc.DocNumber   // Номер документа
                                            && statementDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty    // Номер счёта плательщика
                                            && statementDoc.PaymentPayerRequisites.Bank == doc.PaymentPayerRequisites.Bank   // Банк плательщика
                                         select statementDoc;
                decimal TotalSum = queryStatementDocs.Sum(o => o.PaymentCost);
                if (TotalSum <= doc.PaymentCost) {
                    foreach (var statementDoc in queryStatementDocs) {
                        statementDoc.PaymentDocument = doc;

                        // ??? Возможно, здесь надо провести корректировку значений свойств Платёжного документа doc по значениям аналогичных свойств документа выписки statementDoc
                    }
                } else {

                }
            }
        }

        /// <summary>
        /// Сопоставление документов выписок и платёжек по нестрогому (decent)  критерию: номер документа, дата документа, счёт плательщика, банк плательщика
        /// </summary>
        /// <param name="ir"></param>
        public void DecentMappingProcess(T doc) {
            XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryStatementDocs = from statementDoc in statementDocs
                                        where statementDoc.PaymentDocument == null   // документ ещё не привязан
                                        && statementDoc.DocDate == doc.DocDate   // Дата документа
                                        && statementDoc.DocNumber == doc.DocNumber   // Номер документа
                                        && statementDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty    // Номер счёта плательщика
                                        && statementDoc.PaymentPayerRequisites.Bank == doc.PaymentPayerRequisites.Bank   // Банк плательщика
                                        select statementDoc;
            decimal TotalSum = queryStatementDocs.Sum(o => o.PaymentCost);
            if (TotalSum <= doc.PaymentCost) {
                foreach (var statementDoc in queryStatementDocs) {
                    statementDoc.PaymentDocument = doc;

                    // ??? Возможно, здесь надо провести корректировку значений свойств Платёжного документа doc по значениям аналогичных свойств документа выписки statementDoc
                }
            } else {

            }
        }


        /// <summary>
        /// Список платёжных документов по жёсткому критерию
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetPaymentDocsByHardCriteria<typeDoc>() where typeDoc : fmCDocRCB {
            //XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            //var queryPaymentDocs = from paymentDoc in paymentDocs
            //                       where !(from statementDoc in paymentDocs
            //                       //    select statementDoc.PaymentDocument).Contains(paymentDoc)
            //                       select paymentDoc;

            //foreach (var doc in queryPaymentDocs) {
            //    // Отбираем только те, сумма которых не покрыта полностью
            //    XPQuery<T> paymentDocs1 = new XPQuery<T>(this.Session);

            /*
            XPQuery<typeDoc> paymentDocs = new XPQuery<typeDoc>(this.Session);
            XPQuery<fmCStatementAccountDoc> accDocs = new XPQuery<fmCStatementAccountDoc>(this.Session);
            var queryPaymentDocs = from paymentDoc in paymentDocs
                                   join accDoc in accDocs on paymentDoc equals accDoc.PaymentDocument
                                   where accDoc == null
                                   //where !(from statementDoc in accDocs
                                   //     select statementDoc.PaymentDocument).Contains(paymentDoc)
                                   select paymentDoc;
            */

            
            //CriteriaOperator criteriaAND = new GroupOperator();
            //((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

            //CriteriaOperator criteria1 = new BinaryOperator(new OperandProperty("INN"), new ConstantValue(pr.INN), BinaryOperatorType.Equal);
            //CriteriaOperator criteria2 = new BinaryOperator(new OperandProperty("KPP"), new ConstantValue(pr.KPP), BinaryOperatorType.Equal);
            //CriteriaOperator criteria3 = CriteriaOperator.Parse("'" + pr.KPP + "' != ''");

            //((GroupOperator)criteriaAND).Operands.Add(criteria1);
            //((GroupOperator)criteriaAND).Operands.Add(criteria2);
            //((GroupOperator)criteriaAND).Operands.Add(criteria3);

            //crmCParty party = null;
            //crmCLegalPerson lp = null;
            //crmCLegalPersonUnit lpu = null;

            //bool lpResult = true;

            //party = this.Session.FindObject<crmCParty>(criteriaAND);
            //if (party == null) {

            //OperandProperty prop = new OperandProperty("LegalPerson");
            //CriteriaOperator op = prop == lPerson;
            //CriteriaOperator criteriaPersonUnit = new BinaryOperator(new OperandProperty("KPP"), new ConstantValue(rKPP), BinaryOperatorType.Equal);

            //CriteriaOperator criteriaAND = new GroupOperator();
            //((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

            //((GroupOperator)criteriaAND).Operands.Add(op);
            //((GroupOperator)criteriaAND).Operands.Add(criteriaPersonUnit);
            
            //InOperator inOp = new InOperator(
            /*
            foreach (var doc in queryPaymentDocs) {
                XPQuery<fmCStatementAccountDoc> statementDocs = new XPQuery<fmCStatementAccountDoc>(this.Session);
                var queryPaymentDocs1 = from statementDoc in statementDocs
                                        where statementDoc.PaymentDocument == doc
                                        select statementDoc;
                int count = queryPaymentDocs1.Count();
                if (count == 0)
                    yield return doc;
            }
            */

            XPCollection<typeDoc> docCol = new XPCollection<typeDoc>(Session);
            if (!docCol.IsLoaded) docCol.Load();
            foreach (typeDoc doc in docCol) {
                XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                var queryPaymentDocs1 = from statementDoc in statementDocs
                                        where statementDoc.PaymentDocument == doc
                                        select statementDoc;
                int count = queryPaymentDocs1.Count();
                if (count == 0)
                    yield return doc;
            }

        }

        /// <summary>
        /// Список платёжных документов по мягкому критерию
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetPaymentDocsBySoftCriteria() {
            //XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            //var queryPaymentDocs = from paymentDoc in paymentDocs
            //                       where !(from statementDoc in paymentDocs
            //                       //    select statementDoc.PaymentDocument).Contains(paymentDoc)
            //                       select paymentDoc;

            //foreach (var doc in queryPaymentDocs) {
            //    // Отбираем только те, сумма которых не покрыта полностью
            //    XPQuery<T> paymentDocs1 = new XPQuery<T>(this.Session);
            XPQuery<T> paymentDocs = new XPQuery<T>(this.Session);
            XPQuery<fmCSAStatementAccountDoc> accDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryPaymentDocs = from paymentDoc in paymentDocs
                                   join accDoc in accDocs on paymentDoc equals accDoc.PaymentDocument
                                   //
                                   where accDoc == null

                                   //                                   !(from statementDoc in paymentDocs
                                   //                                      select statementDoc.PaymentDocument).Contains(paymentDoc)
                                   select paymentDoc;

            foreach (var doc in queryPaymentDocs) {
                XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
                var queryPaymentDocs1 = from statementDoc in statementDocs
                                        where statementDoc.PaymentDocument == doc
                                        select statementDoc;
                decimal TotalSum = queryPaymentDocs1.Sum(o => o.PaymentCost);
                if (TotalSum < doc.PaymentCost) {
                    yield return doc;
                }
            }
        }

        /*
        public void CreateOtherPaymentDocument() {
            XPQuery<fmCStatementAccountDoc> StatementDocs = new XPQuery<fmCStatementAccountDoc>(this.Session);
            var queryStatementDocs = from statementDoc in StatementDocs
                                     where statementDoc.PaymentDocument == null && 
                                           statementDoc.DocType != null &&
                                           statementDoc.DocType.Trim() != ""
//                                           !string.IsNullOrEmpty(statementDoc.DocType)
                                     select statementDoc;

            foreach (var doc in queryStatementDocs) {
                fmCDocRCBOthers otherDoc = new fmCDocRCBOthers(this.Session);
                doc.PaymentDocument = otherDoc;
                AssignCommonProperty(otherDoc, doc);
                otherDoc.DocType = doc.DocType;
            }
        }
        */
         
        //public void CreatePaymentDocument(fmStatementOfAccount statementAccount) {
        public void CreatePaymentDocument<typeDoc>(fmCSAStatementAccount statementAccount) where typeDoc : fmCDocRCB {

            DateTime DatePeriodBegin = statementAccount.DateFrom.Date;
            DateTime DatePeriodEnd = statementAccount.DateTo.Date;

            XPQuery<fmCSAStatementAccountDoc> statementDocs = new XPQuery<fmCSAStatementAccountDoc>(this.Session);
            var queryStatementDocs = from statementDoc in statementDocs
                                     where statementDoc.PaymentDocument == null
                                     && (statementDoc.PaymentPayerRequisites.StatementOfAccount == statementAccount || statementDoc.PaymentReceiverRequisites.StatementOfAccount == statementAccount)
                                     && statementDoc.NameTypeOfRCBDocument == typeof(typeDoc).FullName
                                     //&& statementDoc.PaymentPayerRequisites.AccountParty == statementAccount.BankAccountText
                                     select statementDoc;

            foreach (var doc in queryStatementDocs) {

                //if (doc.NameTypeOfRCBDocument != typeof(typeDoc).FullName) continue;

                // Проверка наличия Платёжного документа, подходящего для данного документа выписки doc
                // 1. Тип документа, например, fmCDocRCBPaymentOrder и т.п.
                // 2. Номер счёта плательщика
                // 3. Номер документа DocNumber
                // 4. Дата документа DocDate

                XPQuery<typeDoc> paymentDocs = new XPQuery<typeDoc>(this.Session, true);
                var queryPaymentDocs = from paymentDoc in paymentDocs
                                       where paymentDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty &&
                                         paymentDoc.DocNumber == doc.DocNumber &&
                                         paymentDoc.DocDate == doc.DocDate
                                       select paymentDoc;

                bool isFound = false;
                foreach (var paymentDoc in queryPaymentDocs) {
                    isFound = true;

                    // Найден какой-то Платёжный документ заданного типа -> сумма выписки учитывается в регистре, а сам Платёжный документ припиывается 
                    // документу выписки
                    doc.PaymentDocument = paymentDoc;
                    //AssignCommonProperty(paymentDoc, doc); // ??? Надо обновлять ???

                    // Минусуем
                    // ????????? Даты границ берутся как даты документа или как даты списания со счёта ???????????????
                    /*
                    XPQuery<fmSettlmentRegister> registers = new XPQuery<fmSettlmentRegister>(this.Session, true);
                    var queryRegisters = from register in registers
                                             where register.PaymentDocument == paymentDoc
                                             && register.PaymentDocument.DocDate.Date >= DatePeriodBegin
                                             && register.PaymentDocument.DocDate.Date <= DatePeriodEnd
                                             && register.PaymentDocument.ComponentType.FullName == typeof(typeDoc).FullName
                                             select register;
                    */
                    XPQuery<fmSettlmentRegister> registers = new XPQuery<fmSettlmentRegister>(this.Session, true);
                    var queryRegisters = from register in registers
                                         where register.PaymentDocument == paymentDoc
                                         && register.DeductedFromPayerAccount.Date >= DatePeriodBegin
                                         && register.DeductedFromPayerAccount.Date <= DatePeriodEnd
                                         && register.PaymentDocument.ComponentType.FullName == typeof(typeDoc).FullName
                                         select register;

                    foreach (var reg in queryRegisters) {
                        if (reg.Sum > 0) {
                            reg.Sum = -reg.Sum;
                        }
                    }

                    // Внесение корректировок в регистр. Правила следующие:
                    // 1. Если сверх имеющихся параметров (тип документа, номер документа, дата окумента, номер Счёта плательщика)
                    //    совпала сумма, то ничего не делаем, т.к. это попросту повторный документ выписки (уже присылали ранее)
                    // 2. Если сумма не совпала, то находим разность и вносим в регистр эту разность.
                    //if (paymentDoc.PaymentCost != doc.PaymentCost) {
                    decimal diff = doc.PaymentCost - paymentDoc.PaymentCost;

                    // Размещение в регистре
                    fmSettlmentRegister rg = new fmSettlmentRegister(Session);
                    rg.PaymentDocument = paymentDoc;
                    rg.StatementAccountDoc = doc;
                    rg.Sum = diff;
                    //}

                    break;
                }

                if (!isFound) {
                    // Не найден никакой документ заданного типа -> создаётся Платёжный документ соответствующего типа
                    fmCDocRCB paymentDoc = null;
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentOrder).FullName) paymentDoc = new fmCDocRCBPaymentOrder(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentRequest).FullName) paymentDoc = new fmCDocRCBPaymentRequest(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBAkkreditivRequest).FullName) paymentDoc = new fmCDocRCBAkkreditivRequest(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBInkassOrder).FullName) paymentDoc = new fmCDocRCBInkassOrder(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBOthers).FullName) paymentDoc = new fmCDocRCBOthers(Session);

                    paymentDoc.DocType = doc.DocType;
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc);

                    // Размещение в регистре
                    fmSettlmentRegister rg = new fmSettlmentRegister(Session);
                    rg.PaymentDocument = paymentDoc;
                    rg.StatementAccountDoc = doc;
                    rg.Sum = doc.PaymentCost;
                }

            }

            /*  
            foreach (var doc in queryStatementDocs) {

                // Проверка наличия Платёжного документа, подходящего для данного документа выписки doc
                // 1. Тип документа, например, fmCDocRCBPaymentOrder и т.п.
                // 2. Номер счёта плательщика
                // 3. Номер документа DocNumber
                // 4. Дата документа DocDate


                CriteriaOperator criteriaAND = new GroupOperator();
                ((GroupOperator)criteriaAND).OperatorType = GroupOperatorType.And;

                // Номер счёта
                // Объяснение для Павла: если я создал Платёжный документ по какому-то предшествующему документу выписки,
                // то в этом Платёжном документе определён Плательщик как объект. С другой стороны, в документе выписки должен был определиться
                // Плательщик одинаковый с Плательщиком в родственном (т.е. оплачивающий доугую часть суммы) документе выписки. 
                // Поэтому можно сравнивать по реквизитам, а не по самим счетам.
                CriteriaOperator criteria1 = new BinaryOperator(new OperandProperty("PaymentPayerRequisites.AccountParty"), new ConstantValue(doc.PaymentPayerRequisites.AccountParty), BinaryOperatorType.Equal);
                
                // Номер документа
                CriteriaOperator criteria2 = new BinaryOperator(new OperandProperty("DocNumber"), new ConstantValue(doc.DocNumber), BinaryOperatorType.Equal);

                // Дата документа
                CriteriaOperator criteria3 = new BinaryOperator(new OperandProperty("DocDate"), new ConstantValue(doc.DocDate), BinaryOperatorType.Equal);

                // Проверка совпадения типа
                CriteriaOperator criteria4 = new BinaryOperator(new OperandProperty("ComponentType"), new ConstantValue(Type.GetType(doc.NameTypeOfRCBDocument)), BinaryOperatorType.Equal);

                //OperandProperty propBank = new OperandProperty("Bank");
                //CriteriaOperator opBank = propBank == requisites.Bank;

                //OperandProperty propPerson = new OperandProperty("Person");
                //CriteriaOperator opPerson = propPerson == requisites.Party.Person;
                //CriteriaOperator criteria3 = CriteriaOperator.Parse("Person.INN == '" + requisites.Party.Person.INN + "'");

                ((GroupOperator)criteriaAND).Operands.Add(criteria1);
                ((GroupOperator)criteriaAND).Operands.Add(criteria3);
                ((GroupOperator)criteriaAND).Operands.Add(criteria3);
                ((GroupOperator)criteriaAND).Operands.Add(criteria4);

                fmCDocRCB paymentDoc = this.Session.FindObject<fmCDocRCB>(criteriaAND);
                if (paymentDoc == null) {
                    // Не найден никакой документ заданного типа -> создаётся Платёжный документ соответствующего типа
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentOrder).FullName) paymentDoc = new fmCDocRCBPaymentOrder(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentRequest).FullName) paymentDoc = new fmCDocRCBPaymentRequest(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBAkkreditivRequest).FullName) paymentDoc = new fmCDocRCBAkkreditivRequest(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBInkassOrder).FullName) paymentDoc = new fmCDocRCBInkassOrder(Session);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBOthers).FullName) paymentDoc = new fmCDocRCBOthers(Session);

                    paymentDoc.DocType = doc.DocType;
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc);

                } else {
                    // Найден какой-то Платёжный документ заданного типа -> сумма выписки учитывается в регистре, а сам Платёжный документ припиывается 
                    // документу выписки
                    doc.PaymentDocument = paymentDoc;
                    //AssignCommonProperty(paymentDoc, doc); // ??? Надо обновлять ???
                }
            }
            */

        }

        private void AssignCommonProperty(fmCDocRCB paymentDoc, fmCSAStatementAccountDoc doc) {
            paymentDoc.AcceptanceDuration = doc.AcceptanceDuration;
            paymentDoc.AccountNumberSupplier = doc.AccountNumberSupplier;
            paymentDoc.AdvancedConditions = doc.AdvancedConditions;
            paymentDoc.AkkreditiveKind = doc.AkkreditiveKind;
            paymentDoc.CompilerStatus = doc.CompilerStatus;
            paymentDoc.CompilerStatus = doc.CompilerStatus;
            paymentDoc.DateIndicator = doc.DateIndicator;
            paymentDoc.DeductedFromPayerAccount = doc.DeductedFromPayerAccount;
            paymentDoc.DocDate = doc.DocDate;
            paymentDoc.DocNumber = doc.DocNumber;
            //paymentDoc.DocType = doc.DocType;
            paymentDoc.DocumentSendingDate = doc.DocumentSendingDate;
            paymentDoc.KBKStatus = doc.KBKStatus;
            paymentDoc.NumberIndicator = doc.NumberIndicator;
            paymentDoc.OKATO = doc.OKATO;
            paymentDoc.OperationKind = doc.OperationKind;
            paymentDoc.PaymentByRepresentation = doc.PaymentByRepresentation;
            paymentDoc.PaymentCondition = doc.PaymentCondition;
            paymentDoc.PaymentCost = doc.PaymentCost;
            paymentDoc.PaymentCostCopybook = doc.PaymentCostCopybook;
            paymentDoc.PaymentDeadLine = doc.PaymentDeadLine;
            //paymentDoc.PaymentDocument = doc.PaymentDocument;
            paymentDoc.PaymentFunction = doc.PaymentFunction;
            paymentDoc.PaymentFunctionCode = doc.PaymentFunctionCode;
            paymentDoc.PaymentKind = doc.PaymentKind;
            paymentDoc.PaymentPayerRequisites = doc.PaymentPayerRequisites;
            paymentDoc.PaymentReceiverRequisites = doc.PaymentReceiverRequisites;
            paymentDoc.PaymentResField = doc.PaymentResField;
            paymentDoc.PaymentSequence = doc.PaymentSequence;
            paymentDoc.PeriodIndicator = doc.PeriodIndicator;
            paymentDoc.ReasonIndicator = doc.ReasonIndicator;
            paymentDoc.ReceivedByPayerBankDate = doc.ReceivedByPayerBankDate;
            paymentDoc.TicketContent = doc.TicketContent;
            paymentDoc.TicketDate = doc.TicketDate;
            paymentDoc.TicketTime = doc.TicketTime;
            paymentDoc.TypeIndicator = doc.TypeIndicator;
        }

        #endregion
    }

}
