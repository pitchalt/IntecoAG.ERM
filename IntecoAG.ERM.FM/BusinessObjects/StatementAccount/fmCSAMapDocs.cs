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
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM.StatementAccount {

    /// <summary>
    /// Методы распознавания и сопоставления платёжных документов
    /// </summary>
    [NonPersistent]
    public class fmCSAMapDocs<T> : csCComponent where T : fmCDocRCB
    {
        public fmCSAMapDocs(Session ses)
            : base(ses) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCSAMapDocs<T>);
            this.CID = Guid.NewGuid();
        }

//        private class groupRegister {
//            public DateTime OperationDate;
//            public fmCDocRCB PaymentDocument;
//            public decimal TotalSumIn;
//            public decimal TotalSumOut;
//        }

//        private class groupSA {
//            public DateTime OperationDate;
//            public fmCSAStatementAccountDoc StatementDocument;
//            public decimal TotalSumIn;
//            public decimal TotalSumOut;
//        }

        private class OperationJurnal {
            public DateTime Date = default(DateTime);
            public fmCDocRCB PayDoc = null;
            public fmCSAStatementAccountDoc StatementDoc = null;
            public Decimal CurrentSumIn = 0;
            public Decimal CurrentSumOut = 0;
            public Decimal StatementSumIn = 0;
            public Decimal StatementSumOut = 0;
        }

        #region МЕТОДЫ

        /// <summary>
        /// Создание всех (в расслоении по типам) Платёжных документов
        /// </summary>
        /// <param name="importResult"></param>
        public void CreateAllPaymentDocuments(IObjectSpace os, fmCSAImportResult importResult) {
            if (importResult == null) return;
            
            using (IObjectSpace nos = os.CreateNestedObjectSpace()) {
                fmCSAImportResult ir = nos.GetObject<fmCSAImportResult>(importResult);
                foreach (fmCSAStatementAccount sa in ir.StatementOfAccounts) {

                    CreatePaymentDocuments<fmCDocRCBPaymentOrder>(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();

                    CreatePaymentDocuments<fmCDocRCBPaymentRequest>(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();

                    CreatePaymentDocuments<fmCDocRCBAkkreditivRequest>(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();

                    CreatePaymentDocuments<fmCDocRCBInkassOrder>(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();

                    CreatePaymentDocuments<fmCDocRCBOthers>(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();

                    // 2012-04-12 Можно отказаться от параметра типа?
                    //CreatePaymentDocuments(((ObjectSpace)nos).Session, sa);
                    //nos.CommitChanges();


                    //UpdateRegister(((ObjectSpace)nos).Session, sa);
                    //nos.CommitChanges();
                }

                importResult.ResultCode = 3;
           }
        }

        //public void CreatePaymentDocument(fmStatementOfAccount statementAccount) {
        public void CreatePaymentDocuments<typeDoc>(Session ses,  fmCSAStatementAccount statementAccount) where typeDoc : fmCDocRCB {

            //DateTime DatePeriodBegin = statementAccount.DateFrom.Date;
            //DateTime DatePeriodEnd = statementAccount.DateTo.Date;

            /* 2012-04-11
            XPQuery<fmCSAStatementAccountDoc> docs = new XPQuery<fmCSAStatementAccountDoc>(ses, true);
            var queryStatementDocs = from statementDoc in docs
                                     where //statementDoc.PaymentDocument == null
//                                         && (statementAccount.PayInDocs.Contains<fmCSAStatementAccountDoc>(statementDoc) || statementAccount.PayOutDocs.Contains<fmCSAStatementAccountDoc>(statementDoc))
                                           (statementDoc.PaymentPayerRequisites.StatementOfAccount == statementAccount ||
                                            statementDoc.PaymentReceiverRequisites.StatementOfAccount == statementAccount)
                                         && statementDoc.NameTypeOfRCBDocument == typeof(typeDoc).FullName
                                     select statementDoc;
            */

            var queryStatementDocs = (statementAccount.PayInDocs.Union(statementAccount.PayOutDocs)).Where(d => d.NameTypeOfRCBDocument == typeof(typeDoc).FullName);

            foreach (var doc in queryStatementDocs) {

                //if (doc.NameTypeOfRCBDocument != typeof(typeDoc).FullName) continue;

                // Проверка наличия Платёжного документа, подходящего для данного документа выписки doc
                // 1. Тип документа, например, fmCDocRCBPaymentOrder и т.п.
                // 2. Номер счёта плательщика
                // 3. Номер документа DocNumber
                // 4. Дата документа DocDate

                XPQuery<typeDoc> paymentDocs = new XPQuery<typeDoc>(ses, true);
                /* 2012-04-11
                var queryPaymentDocs = from paymentDoc in paymentDocs
                                       where (doc.PaymentPayerRequisites.AccountParty == statementAccount.BankAccountText 
                                             && paymentDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty 
                                           ||
                                              doc.PaymentReceiverRequisites.AccountParty == statementAccount.BankAccountText 
                                             && paymentDoc.PaymentReceiverRequisites.AccountParty == doc.PaymentReceiverRequisites.AccountParty) 
                                           && paymentDoc.DocNumber == doc.DocNumber 
                                           && paymentDoc.DocDate == doc.DocDate
                                       select paymentDoc;
                */
                // Для входящих платежей - дополнительная проверка по тексту PaymentFunction
                // В качестве признака входящего/исходящего платежа можно взять условие непустоты полей StatementAccountIn/StatementAccountOut
                // 2012-04-12 Телефонный разговор: сменилась концепция проверки подходящего платёжного документа
                /*
                var queryPaymentDocs = from paymentDoc in paymentDocs
                                       where (doc.PaymentPayerRequisites.AccountParty == statementAccount.BankAccountText
                                             && paymentDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty
                                           ||
                                              doc.PaymentReceiverRequisites.AccountParty == statementAccount.BankAccountText
                                             && paymentDoc.PaymentReceiverRequisites.AccountParty == doc.PaymentReceiverRequisites.AccountParty)
                                           && paymentDoc.PaymentPayerRequisites.INN == doc.PaymentPayerRequisites.INN && paymentDoc.PaymentPayerRequisites.KPP == doc.PaymentPayerRequisites.KPP
                                           && paymentDoc.PaymentReceiverRequisites.INN == doc.PaymentReceiverRequisites.INN && paymentDoc.PaymentReceiverRequisites.KPP == doc.PaymentReceiverRequisites.KPP
                                           && paymentDoc.DocNumber == doc.DocNumber
                                           && paymentDoc.DocDate == doc.DocDate
                                           && (doc.StatementAccountIn == null || paymentDoc.PaymentFunction == doc.PaymentFunction)
                                       select paymentDoc;
                */
                // 2012-04-12 Новый способ проверки: Тип (учитывается как параметр), Номер документа, Дата документа, БИК банка плательщика,
                // счёт плательщика, БИК банка получаетля, счёт получателя
                var queryPaymentDocs = from paymentDoc in paymentDocs
                                       where paymentDoc.DocType == doc.DocType
                                           && paymentDoc.DocNumber == doc.DocNumber
                                           && paymentDoc.DocDate == doc.DocDate
                                           && paymentDoc.PaymentPayerRequisites.RCBIC == doc.PaymentPayerRequisites.RCBIC && paymentDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty
                                           && paymentDoc.PaymentReceiverRequisites.RCBIC == doc.PaymentReceiverRequisites.RCBIC && paymentDoc.PaymentReceiverRequisites.AccountParty == doc.PaymentReceiverRequisites.AccountParty
                                           && paymentDoc.PaymentFunction == doc.PaymentFunction
                                       select paymentDoc;


                bool isFound = false;
                foreach (var paymentDoc in queryPaymentDocs) {
                    isFound = true;

                    // Найден какой-то Платёжный документ заданного типа -> сумма выписки учитывается в регистре, а сам Платёжный документ припиывается 
                    // документу выписки
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc); // ??? Надо обновлять ???

                    break;
                }

                if (!isFound) {
                    // Не найден никакой документ заданного типа -> создаётся Платёжный документ соответствующего типа
                    fmCDocRCB paymentDoc = null;
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentOrder).FullName) paymentDoc = new fmCDocRCBPaymentOrder(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentRequest).FullName) paymentDoc = new fmCDocRCBPaymentRequest(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBAkkreditivRequest).FullName) paymentDoc = new fmCDocRCBAkkreditivRequest(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBInkassOrder).FullName) paymentDoc = new fmCDocRCBInkassOrder(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBOthers).FullName) paymentDoc = new fmCDocRCBOthers(ses);

                    paymentDoc.DocType = doc.DocType;
                    paymentDoc.State = PaymentDocProcessingStates.IMPORTED;
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc);

                }

            }

        }


        // 2012-04-12 От шаблона по типу можно отказаться?
        public void CreatePaymentDocuments(Session ses, fmCSAStatementAccount statementAccount) {
            var queryStatementDocs = (statementAccount.PayInDocs.Union(statementAccount.PayOutDocs));

            foreach (var doc in queryStatementDocs) {

                //if (doc.NameTypeOfRCBDocument != typeof(typeDoc).FullName) continue;

                // Проверка наличия Платёжного документа, подходящего для данного документа выписки doc
                // 1. Тип документа, например, fmCDocRCBPaymentOrder и т.п.
                // 2. Номер счёта плательщика
                // 3. Номер документа DocNumber
                // 4. Дата документа DocDate

                XPQuery<fmCDocRCB> paymentDocs = new XPQuery<fmCDocRCB>(ses, true);
                // 2012-04-12 Новый способ проверки: Тип (учитывается как параметр), Номер документа, Дата документа, БИК банка плательщика,
                // счёт плательщика, БИК банка получаетля, счёт получателя
                var queryPaymentDocs = from paymentDoc in paymentDocs
                                       where paymentDoc.DocType == doc.DocType
                                           && paymentDoc.DocNumber == doc.DocNumber
                                           && paymentDoc.DocDate == doc.DocDate
                                           && paymentDoc.PaymentPayerRequisites.RCBIC == doc.PaymentPayerRequisites.RCBIC && paymentDoc.PaymentPayerRequisites.AccountParty == doc.PaymentPayerRequisites.AccountParty
                                           && paymentDoc.PaymentReceiverRequisites.RCBIC == doc.PaymentReceiverRequisites.RCBIC && paymentDoc.PaymentReceiverRequisites.AccountParty == doc.PaymentReceiverRequisites.AccountParty
                                           && paymentDoc.PaymentFunction == doc.PaymentFunction
                                       select paymentDoc;


                bool isFound = false;
                foreach (var paymentDoc in queryPaymentDocs) {
                    isFound = true;

                    // Найден какой-то Платёжный документ заданного типа -> сумма выписки учитывается в регистре, а сам Платёжный документ припиывается 
                    // документу выписки
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc); // ??? Надо обновлять ???

                    break;
                }

                if (!isFound) {
                    // Не найден никакой документ заданного типа -> создаётся Платёжный документ соответствующего типа
                    fmCDocRCB paymentDoc = null;
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentOrder).FullName)
                        paymentDoc = new fmCDocRCBPaymentOrder(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBPaymentRequest).FullName)
                        paymentDoc = new fmCDocRCBPaymentRequest(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBAkkreditivRequest).FullName)
                        paymentDoc = new fmCDocRCBAkkreditivRequest(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBInkassOrder).FullName)
                        paymentDoc = new fmCDocRCBInkassOrder(ses);
                    if (doc.NameTypeOfRCBDocument == typeof(fmCDocRCBOthers).FullName)
                        paymentDoc = new fmCDocRCBOthers(ses);

                    paymentDoc.DocType = doc.DocType;
                    paymentDoc.State = PaymentDocProcessingStates.IMPORTED;
                    doc.PaymentDocument = paymentDoc;
                    AssignCommonProperty(paymentDoc, doc);
                }
            }
        }

        public void UpdateAllRegister(IObjectSpace os, fmCSAImportResult importResult)
        {
            if (importResult == null) return;

            using (IObjectSpace nos = os.CreateNestedObjectSpace())
            {
                fmCSAImportResult ir = nos.GetObject<fmCSAImportResult>(importResult);
                foreach (fmCSAStatementAccount sa in ir.StatementOfAccounts)
                {
                    UpdateRegister(((ObjectSpace)nos).Session, sa);
                    nos.CommitChanges();
                }
            }
        }

        private void UpdateRegister(Session ses, fmCSAStatementAccount statementAccount) {

            DateTime DatePeriodBegin = statementAccount.DateFrom.Date;
            DateTime DatePeriodEnd = statementAccount.DateTo.Date;

            XPQuery<fmCSAOperationJournal> OperJur = new XPQuery<fmCSAOperationJournal>(ses, true);

            var OperJurFact = from oper in OperJur
                               where oper.OperationDate.Date >= DatePeriodBegin && 
                                    oper.OperationDate.Date < DatePeriodEnd.Date.AddDays(1) &&
                                    oper.BankAccount == statementAccount.BankAccount
                               group oper by
                                   new {
                                       oper.OperationDate,
                                       oper.PaymentDocument
                                   } into registerGroup
                               select new OperationJurnal {
                                   Date = registerGroup.Key.OperationDate,
                                   PayDoc = registerGroup.Key.PaymentDocument,
                                   StatementDoc = null,
                                   CurrentSumIn = registerGroup.Sum(oper => oper.SumIn),
                                   CurrentSumOut = registerGroup.Sum(oper => oper.SumOut)
                               };

            XPQuery<fmCSAStatementAccountDoc> StatAccountDoc = new XPQuery<fmCSAStatementAccountDoc>(ses, true);
            var OperJurStatIn = from doc in StatAccountDoc 
                                where doc.PaymentReceiverRequisites.StatementOfAccount == statementAccount &&
                                      doc.ReceivedByPayerBankDate != DateTime.MinValue
                                select new OperationJurnal {
                                    Date = doc.ReceivedByPayerBankDate,
                                    PayDoc = doc.PaymentDocument,
                                    StatementDoc = doc,
                                    StatementSumIn = doc.PaymentCost,
                                };
            var OperJurStatOut = from doc in StatAccountDoc
                                where doc.PaymentPayerRequisites.StatementOfAccount == statementAccount &&
                                      doc.DeductedFromPayerAccount != DateTime.MinValue
                                select new OperationJurnal {
                                    Date = doc.DeductedFromPayerAccount,
                                    PayDoc = doc.PaymentDocument,
                                    StatementDoc = doc,
                                    StatementSumOut = doc.PaymentCost,
                                };

            var OperJurUnion = from oper in OperJurFact.Union(OperJurStatIn).Union(OperJurStatOut)
                               group oper by
                                   new {
                                       Date = oper.Date,
                                       PayDoc = oper.PayDoc
                                   } into oper_group
                               select new OperationJurnal {
                                   Date = oper_group.Key.Date,
                                   PayDoc = oper_group.Key.PayDoc,
//                                   StatementOper = oper_group.First(oper => oper.StatementDoc != null),
                                   CurrentSumIn = oper_group.Sum(oper => oper.CurrentSumIn),
                                   CurrentSumOut = oper_group.Sum(oper => oper.CurrentSumOut),
                                   StatementSumIn = oper_group.Sum(oper => oper.StatementSumIn),
                                   StatementSumOut = oper_group.Sum(oper => oper.StatementSumOut)
                               };

            foreach (OperationJurnal oper in OperJurUnion) {
                if (oper.CurrentSumIn != oper.StatementSumIn || oper.CurrentSumOut != oper.StatementSumOut) {
                    fmCSAOperationJournal oper_delta = new fmCSAOperationJournal(ses);
                    oper_delta.DateRecord = DateTime.Now;
//                    oper_delta.
                    oper_delta.OperationDate = oper.Date;
                    oper_delta.BankAccount = statementAccount.BankAccount;
                    oper_delta.PaymentDocument = oper.PayDoc;
//                    oper_delta.StatementAccountDoc = itemSA.StatementDocument;
                    oper_delta.SumIn = oper.StatementSumIn - oper.CurrentSumIn;
                    oper_delta.SumOut = oper.StatementSumOut - oper.CurrentSumOut;

                    // Валюта, для которой указаны суммы SumIn и SumOut
                    oper_delta.Valuta = crmBankAccount.GetValutaByBankAccount(ses, statementAccount.BankAccount);   //GetValutaByBankAccount(ses, statementAccount.BankAccount);
                }
            }

        }

        //private csValuta GetValutaByBankAccount(Session ses, crmBankAccount bankAccount) {
        //    //XPQuery<csValuta> valutas = new XPQuery<csValuta>(ses);
        //    //return (from valuta in valutas
        //    //                    where valuta.CodeCurrencyValue == bankAccount.Number.Substring(5, 3)
        //    //                    select valuta).First();
        //    return new XPQuery<csValuta>(ses).Where(p => p.CodeCurrencyValue == bankAccount.Number.Substring(5, 3)).First();
        //}

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
            paymentDoc.DocType = doc.DocType;   // 2012-04-12 раскомментил
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
            //paymentDoc.PaymentPayerRequisites = doc.PaymentPayerRequisites;   // 2012-04-11
            //paymentDoc.PaymentReceiverRequisites = doc.PaymentReceiverRequisites;   // 2012-04-11
            paymentDoc.PaymentResField = doc.PaymentResField;
            paymentDoc.PaymentSequence = doc.PaymentSequence;
            paymentDoc.PeriodIndicator = doc.PeriodIndicator;
            paymentDoc.ReasonIndicator = doc.ReasonIndicator;
            paymentDoc.ReceivedByPayerBankDate = doc.ReceivedByPayerBankDate;
            paymentDoc.TicketContent = doc.TicketContent;
            paymentDoc.TicketDate = doc.TicketDate;
            paymentDoc.TicketTime = doc.TicketTime;
            paymentDoc.TypeIndicator = doc.TypeIndicator;

            AssignRequsites(paymentDoc.PaymentPayerRequisites, doc.PaymentPayerRequisites);
            AssignRequsites(paymentDoc.PaymentReceiverRequisites, doc.PaymentReceiverRequisites);
        }

        private void AssignRequsites(fmCDocRCBRequisites ToRequisites, fmCDocRCBRequisites FromRequisites) {
            ToRequisites.AccountBank = FromRequisites.AccountBank;
            ToRequisites.AccountParty = FromRequisites.AccountParty;
            ToRequisites.Bank = FromRequisites.Bank;
            ToRequisites.BankAccount = FromRequisites.BankAccount;
            ToRequisites.BankLocation = FromRequisites.BankLocation;
            ToRequisites.BankName = FromRequisites.BankName;
            ToRequisites.INN = FromRequisites.INN;
            ToRequisites.KPP = FromRequisites.KPP;
            ToRequisites.NameParty = FromRequisites.NameParty;
            ToRequisites.Party = FromRequisites.Party;
            ToRequisites.Person = FromRequisites.Person;
            ToRequisites.RCBIC = FromRequisites.RCBIC;
//            ToRequisites.StatementOfAccount = FromRequisites.StatementOfAccount;
        }

        #endregion
    }

}
