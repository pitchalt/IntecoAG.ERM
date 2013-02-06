using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.StatementAccount;

using NUnit.Framework;

namespace IntecoAG.ERM.FM
{

    /// <summary>
    ///  Исследование автопривязки
    /// </summary>
    [TestFixture, Description("Исследование автопривязки")]
    public class AutoBindingAnalisys : AutoBindingBaseTest
    {
        public override void Init() {
            base.Init();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        struct HardConditionPair {
            public fmCPRPaymentRequest paymentRequest;
            public fmCDocRCB paymentDocument;
        };

        #region Исследование автопривязки

        [Test, Description("Исследование автопривязки")]
        [Category("Debug")]
        public void AutoBindingResearch() {
            // Задача исследования. 
            // 1. Подсчёт количества загруженных документов в систему за определённый период (за 18-19 апреля, например).
            // 2. Подсчёт количества заявок в системе со статусом IN_PAY, да и вообще сколько заявок с какими статусами.
            // 3. Подсчёт актального количества связей платёжных документов и заявок.
            // 4. Подсчёт количества потенциально возможных связей платёжных документов и заявок.
            // 5. Если обнаружились непривязанные, но могшие быть привязанными автоматически заявками, выяснить, почему они не привязались.

            //Decimal _Accuracy = 0.01m;  // Сопоставление с точностью до копейки


            DateTime startDate = new DateTime(2012, 4, 1);
            DateTime endDate = new DateTime(2012, 4, 20);
            Debug.WriteLine(String.Format("{0} {1} - {2}", "Интервал дат: ", startDate, endDate));
            Debug.WriteLine("\n");

            using (UnitOfWork uow = new UnitOfWork(Common.dataLayer)) {



                // Анализ Платёжных документов. Общая таблица с аналитическими признаками в границах указаннных дат.
                Dictionary<String, csValuta> dictValuta = new Dictionary<string, csValuta>();
                XPQuery<csValuta> valutas = new XPQuery<csValuta>(uow);
                var queryValuta = (from valuta in valutas
                                    //where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                    select valuta).Distinct<csValuta>();
                foreach (var valuta in queryValuta) {
                    dictValuta.Add(valuta.CodeCurrencyValue, valuta);
                }


                // Список счетов "нашей" организации
                //List<crmBankAccount> = new 
                XPQuery<crmBankAccount> ourBankAccounts = new XPQuery<crmBankAccount>(uow);
                var OurBankAccounts = (from bankAccount in ourBankAccounts
                                      where bankAccount.PrefferedParty.Code == "2518"
                                       select bankAccount).ToList<crmBankAccount>();
                Debug.WriteLine(String.Format("{0} {1}", "Всего счетов нашей организации", OurBankAccounts.Count()));
                Debug.WriteLine("\n");

                // --------------------- ПЛАТЁЖНЫЕ ДОКУМЕНТЫ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ПЛАТЁЖНЫЕ ДОКУМЕНТЫ");
                Debug.WriteLine("\n");


                String prmDocType = ""; //"Платежное поручение";
                XPQuery<fmCDocRCB> paymentDocuments = new XPQuery<fmCDocRCB>(uow);
                var queryPaymentDocuments = (from paymentDocument in paymentDocuments
                                            where paymentDocument.DocDate.Date >= startDate
                                               && paymentDocument.DocDate.Date < endDate
                                               && (prmDocType == "" || paymentDocument.DocType == prmDocType)
                                            select new {
                                                PaymentDocument = paymentDocument,
                                                DocType = paymentDocument.DocType,
                                                DocDate = paymentDocument.DocDate,
                                                IsDocDate = (paymentDocument.DocDate != DateTime.MinValue),

                                                IsSum = (paymentDocument.PaymentCost != 0m),
                                                SumIn = ((paymentDocument.PaymentReceiverRequisites.BankAccount.PrefferedParty.Code == "2518") ? paymentDocument.PaymentCost : 0m),
                                                SumOut = ((paymentDocument.PaymentPayerRequisites.BankAccount.PrefferedParty.Code == "2518") ? paymentDocument.PaymentCost : 0m),

                                                //PaymentValuta = crmBankAccount.GetValutaByBankAccount(uow, paymentDocument.PaymentPayerRequisites.BankAccount),
                                                PaymentValuta = paymentDocument.PaymentPayerRequisites.BankAccount.Number.Substring(5, 3),
                                                PaymentReceiverValuta = paymentDocument.PaymentReceiverRequisites.BankAccount.Number.Substring(5, 3),
                                                
                                                ReceiverAccount = (paymentDocument.PaymentReceiverRequisites.AccountParty != ""),
                                                PayerAccount = (paymentDocument.PaymentPayerRequisites.AccountParty != ""),
                                                ReceiverBankAccount = (paymentDocument.PaymentReceiverRequisites.BankAccount != null),
                                                PayerBankAccount = (paymentDocument.PaymentPayerRequisites.BankAccount != null),
                                                
                                                ReceiverBankRCBIC = (paymentDocument.PaymentReceiverRequisites.RCBIC != ""),
                                                ReceiverBank = (paymentDocument.PaymentReceiverRequisites.Bank != null),
                                                PayerBankRCBIC = (paymentDocument.PaymentPayerRequisites.RCBIC != ""),
                                                PayerBank = (paymentDocument.PaymentPayerRequisites.Bank != null),

                                                ReceiverINN = (paymentDocument.PaymentReceiverRequisites.INN != ""),
                                                PayerINN = (paymentDocument.PaymentPayerRequisites.INN != ""),

                                                ReceiverKPP = (paymentDocument.PaymentReceiverRequisites.KPP != ""),
                                                PayerKPP = (paymentDocument.PaymentPayerRequisites.KPP != ""),

                                                DateIn = (paymentDocument.ReceivedByPayerBankDate != DateTime.MinValue),
                                                DateOut = (paymentDocument.DeductedFromPayerAccount != DateTime.MinValue),
                                                DateAccountChanged = (paymentDocument.ReceivedByPayerBankDate != DateTime.MinValue) ? paymentDocument.ReceivedByPayerBankDate : ((paymentDocument.DeductedFromPayerAccount != DateTime.MinValue) ? paymentDocument.DeductedFromPayerAccount : DateTime.MinValue)
                                            }).ToList();

                // Статистика по queryPaymentDocuments
                Int32 TotalPaymentCount = queryPaymentDocuments.Count();
                Debug.WriteLine(String.Format("{0} {1}", "Всего платёжных документов в системе", TotalPaymentCount));
                Debug.WriteLine("\n");

                // Нашша сторона - плательщик (РАСХОДНЫЕ)
                List<fmCDocRCB> totalConditionPaymentDocument = new List<fmCDocRCB>();
                var queryOurDocPayer = (from doc in queryPaymentDocuments
                                       where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                     select doc).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Наша сторона - плательщик (РАСХОДНЫЕ)", queryOurDocPayer.Count()));
                foreach (var doc in queryOurDocPayer) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                    if (!totalConditionPaymentDocument.Contains(doc.PaymentDocument)) {
                        totalConditionPaymentDocument.Add(doc.PaymentDocument);
                    }
                }
                Debug.WriteLine("\n");

                // Нашша сторона - получатель (ПРИХОДНЫЕ)
                var queryOurDocReceiver = from doc in queryPaymentDocuments
                                          where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentReceiverRequisites.BankAccount)
                                          select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Наша сторона - получатель (ПРИХОДНЫЕ)", queryOurDocReceiver.Count()));
                foreach (var doc in queryOurDocReceiver) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Нашша сторона - плательщик и получатель одновременно
                var queryOurPayerAndReceiver = (from doc in queryPaymentDocuments
                                               where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                                  && OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentReceiverRequisites.BankAccount)
                                       select doc).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Наша сторона - плательщик и получатель одновременно", queryOurPayerAndReceiver.Count()));
                foreach (var doc in queryOurPayerAndReceiver) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Статистика по типам документов (сколько каких) (в привязках к заявкам будут фигурировать только документы прихода нефинансовоготипа)
                var queryPaymentDocumentsByDocType = from doc in queryPaymentDocuments
                                                     group doc by new {
                                                         doc.DocType
                                                        }
                                                        into gDoc
                                                      select new {
                                                          DocType = gDoc.Key.DocType,
                                                          Count = gDoc.Count()
                                                      };
                Debug.WriteLine(String.Format("{0}", "Статистика по типам документов (сколько каких) (в привязках к заявкам будут фигурировать только документы прихода нефинансовоготипа)"));
                foreach (var doc in queryPaymentDocumentsByDocType) {
                    Debug.WriteLine(String.Format("{0} {1}", doc.DocType, doc.Count));
                }
                Debug.WriteLine("\n");

                // В разрезе валют
                var queryPaymentDocumentsByValuta = from doc in queryPaymentDocuments
                                                     group doc by new {
                                                         doc.PaymentValuta
                                                     }
                                                    into gDoc
                                                    select new {
                                                        PaymentValuta = gDoc.Key.PaymentValuta,
                                                        Count = gDoc.Count()
                                                    };
                Debug.WriteLine(String.Format("{0}", "В разрезе валют по плательщику"));
                foreach (var doc in queryPaymentDocumentsByValuta) {
                    Debug.WriteLine(String.Format("{0} {1}", doc.PaymentValuta, doc.Count));
                }
                Debug.WriteLine("\n");


                // В разрезе кодов валютных ценностей ЦБ для счёта получателя
                var queryPaymentDocumentsByValuta_1 = from doc in queryPaymentDocuments
                                                    group doc by new {
                                                        doc.PaymentReceiverValuta
                                                    }
                                                        into gDoc
                                                        select new {
                                                            PaymentReceiverValuta = gDoc.Key.PaymentReceiverValuta,
                                                            Count = gDoc.Count()
                                                        };
                Debug.WriteLine(String.Format("{0}", "В разрезе валют по получателю"));
                foreach (var doc in queryPaymentDocumentsByValuta_1) {
                    Debug.WriteLine(String.Format("{0} {1}", doc.PaymentReceiverValuta, doc.Count));
                }
                Debug.WriteLine("\n");


                // Заполнение INN
                // Не заполнено INN хотя бы у одной стороны
                var queryNoINN_1 = from doc in queryPaymentDocuments
                                 where !doc.ReceiverINN || !doc.PayerINN
                                 select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено INN хотя бы у одной стороны", queryNoINN_1.Count()));
                Debug.WriteLine("\n");

                // Не заполнено INN обеих стороны
                var queryNoINN_2 = from doc in queryPaymentDocuments
                                 where !doc.ReceiverINN & !doc.PayerINN
                                 select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено INN обеих стороны", queryNoINN_2.Count()));
                foreach (var doc in queryNoINN_2) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не заполнено INN получателя
                var queryNoINN_3 = from doc in queryPaymentDocuments
                                   where !doc.ReceiverINN
                                   select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено INN получателя", queryNoINN_3.Count()));
                foreach (var doc in queryNoINN_3) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не заполнено INN плательщика
                var queryNoINN_4 = from doc in queryPaymentDocuments
                                   where !doc.PayerINN
                                   select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено INN плательщика", queryNoINN_4.Count()));
                foreach (var doc in queryNoINN_4) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");


                // Заполнение KPP
                // Не заполнено KPP хотя бы у одной стороны
                var queryNoKPP = from doc in queryPaymentDocuments
                                 where !doc.ReceiverKPP || !doc.PayerKPP
                                 select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено KPP хотя бы у одной стороны", queryNoKPP.Count()));
                Debug.WriteLine("\n");

                // Не заполнено INN хотя бы у одной стороны, но при этом есть KPP
                var queryNoINNExistsKPP = from doc in queryPaymentDocuments
                                 where (!doc.ReceiverINN & doc.ReceiverKPP) || (!doc.PayerINN & doc.PayerKPP)
                                 select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не заполнено INN хотя бы у одной стороны, но при этом есть KPP", queryNoINNExistsKPP.Count()));
                foreach (var doc in queryNoINNExistsKPP) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не распознана хотя бы одна сторона
                var queryNoParty = from doc in queryPaymentDocuments
                                   where doc.PaymentDocument.PaymentPayerRequisites.Party == null || doc.PaymentDocument.PaymentReceiverRequisites.Party == null
                                   select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознана хотя бы одна сторона", queryNoParty.Count()));
                foreach (var doc in queryNoParty) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не распознана сторона плательщика
                var queryNoParty_1 = from doc in queryPaymentDocuments
                                   where doc.PaymentDocument.PaymentPayerRequisites.Party == null
                                   select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознана сторона плательщика", queryNoParty_1.Count()));
                foreach (var doc in queryNoParty_1) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не распознана сторона получателя
                var queryNoParty_2 = from doc in queryPaymentDocuments
                                     where doc.PaymentDocument.PaymentReceiverRequisites.Party == null
                                     select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознана сторона получателя", queryNoParty_2.Count()));
                foreach (var doc in queryNoParty_2) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не распознан счёт хотя бы одной стороны
                var queryNoBankAccount = from doc in queryPaymentDocuments
                                         where doc.PaymentDocument.PaymentPayerRequisites.BankAccount == null || doc.PaymentDocument.PaymentReceiverRequisites.BankAccount == null
                                        select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознан счёт хотя бы одной стороны", queryNoBankAccount.Count()));
                Debug.WriteLine("\n");

                // Не распознан счёт плательщика ("наш" в существующих условиях)
                var queryNoPayerBankAccount = from doc in queryPaymentDocuments
                                         where doc.PaymentDocument.PaymentPayerRequisites.BankAccount == null
                                         select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознан счёт плательщика", queryNoPayerBankAccount.Count()));
                foreach (var doc in queryNoPayerBankAccount) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");

                // Не распознан счёт получателя (не "наш" в существующих условиях)
                var queryNoReceiverBankAccount = from doc in queryPaymentDocuments
                                              where doc.PaymentDocument.PaymentReceiverRequisites.BankAccount == null
                                              select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Не распознан счёт получателя", queryNoReceiverBankAccount.Count()));
                foreach (var doc in queryNoReceiverBankAccount) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                }
                Debug.WriteLine("\n");



                // --------------------- ЗАЯВКИ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ЗАЯВКИ");
                Debug.WriteLine("\n");


                // Анализ Платёжных документов. Общая таблица с аналитическими признаками в границах указаннных дат.
                //String prmReqType = ""; //"Платежное поручение";
                XPQuery<fmCPRPaymentRequest> paymentRequests = new XPQuery<fmCPRPaymentRequest>(uow);
                var queryPaymenRequests = (from paymentRequest in paymentRequests
                                             select new {
                                                 PaymentRequest = paymentRequest,
                                                 SumIn = 0m,
                                                 SumOut = paymentRequest.Summ,   // По факту все заявки - расходные
                                                 ObligationValuta = paymentRequest.Valuta
                                             }).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Всего заявок в системе", queryPaymenRequests.Count()));
                Debug.WriteLine("\n");

                // Сколько заявок в статусе К оплате или оплачена
                var queryInPaymentPaymenRequests = (from paymentRequest in queryPaymenRequests
                                                    where (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                                    select paymentRequest).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Сколько заявок в статусе К оплате или оплачена: ", queryInPaymentPaymenRequests.Count()));
                Debug.WriteLine("\n");

                // Заявки, в которых не задана сторона получателя
                var queryRequestNoReceive = from doc in queryPaymenRequests
                                                 where doc.PaymentRequest.PartyPayReceiver == null
                                                 select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не задана сторона получателя", queryRequestNoReceive.Count()));
                foreach (var doc in queryRequestNoReceive) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                // Заявки, в которых не задана сторона плательщика
                var queryRequestNoSender = from doc in queryPaymenRequests
                                            where doc.PaymentRequest.PartyPaySender == null
                                            select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не задана сторона плательщика", queryRequestNoSender.Count()));
                foreach (var doc in queryRequestNoSender) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                // Заявки, в которых не задана сумма
                var queryRequestNoSum = from doc in queryPaymenRequests
                                           where doc.PaymentRequest.Summ == 0m
                                           select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не задана сумма", queryRequestNoSum.Count()));
                foreach (var doc in queryRequestNoSum) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                // Заявки, в которых не задана валюта обязательств
                var queryRequestNoObligationBaluta = from doc in queryPaymenRequests
                                        where doc.ObligationValuta == null
                                        select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не задана валюта обязательств", queryRequestNoObligationBaluta.Count()));
                foreach (var doc in queryRequestNoObligationBaluta) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                // Заявки, в которых не указана дата К оплате
                var queryRequestNoDate = from doc in queryPaymenRequests
                                                     where doc.PaymentRequest.Date == null
                                                     select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не указана дата К оплате", queryRequestNoDate.Count()));
                foreach (var doc in queryRequestNoDate) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                //// Заявки, в которых не указан номер
                //var queryRequestNoNumber = from doc in queryPaymenRequests
                //                         where doc.PaymentRequest.Number == null
                //                         select doc;
                //Debug.WriteLine(String.Format("{0} {1}", "Заявки, в которых не указан номер", queryRequestNoNumber.Count()));
                //foreach (var doc in queryRequestNoNumber) {
                //    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                //}
                //Debug.WriteLine("\n");

                // Заявки, сформированные правильно
                var queryRequestRightly = from doc in queryPaymenRequests
                                          where doc.PaymentRequest.Date != null
                                              && doc.ObligationValuta != null
                                              && doc.PaymentRequest.Summ != 0m
                                              && doc.PaymentRequest.PartyPaySender != null
                                              && doc.PaymentRequest.PartyPayReceiver != null
                                          select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, сформированные правильно", queryRequestRightly.Count()));
                foreach (var doc in queryRequestRightly) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");

                // Заявки, прошедшие стадию утверждения Финансовым отделом
                var queryRequestInPayment = from doc in queryPaymenRequests
                                            where (doc.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || doc.PaymentRequest.State == PaymentRequestStates.IN_BANK || doc.PaymentRequest.State == PaymentRequestStates.PAYED)
                                          select doc;
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, прошедшие стадию утверждения Финансовым отделом (в данном случае: К оплате или Оплачены)", queryRequestInPayment.Count()));
                foreach (var doc in queryRequestInPayment) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                }
                Debug.WriteLine("\n");


                // Заявки, прошедшие утверждение фин. отдела и сформированные правильно
                List<fmCPRPaymentRequest> totalConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                var queryInPaymentRequestRightly = (from doc in queryPaymenRequests
                                          where doc.PaymentRequest.Date != null
                                              && doc.ObligationValuta != null
                                              && doc.PaymentRequest.Summ != 0m
                                              && doc.PaymentRequest.PartyPaySender != null
                                              && doc.PaymentRequest.PartyPayReceiver != null
                                              && (doc.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || doc.PaymentRequest.State == PaymentRequestStates.IN_BANK || doc.PaymentRequest.State == PaymentRequestStates.PAYED)
                                          select doc).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, прошедшие утверждение фин. отдела и сформированные правильно", queryInPaymentRequestRightly.Count()));
                foreach (var doc in queryInPaymentRequestRightly) {
                    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                    if (!totalConditionPaymentRequest.Contains(doc.PaymentRequest)) {
                        totalConditionPaymentRequest.Add(doc.PaymentRequest);
                    }
                }
                Debug.WriteLine("\n");




                // --------------------- ЗАЯВКИ и ПЛАТЁЖНЫЕ ДОКУМЕНТЫ -----------------------//
                Debug.WriteLine("\n");
                Debug.WriteLine("ЗАЯВКИ и ПЛАТЁЖНЫЕ ДОКУМЕНТЫ");
                Debug.WriteLine("\n");

                // Соединение только по совпадению плательщиков (с абсолютно минимальными ограничениями)
                var queryPayers = from paymentRequest in queryPaymenRequests
                                  join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                               on paymentRequest.PaymentRequest.PartyPaySender equals paymentDocument.PaymentDocument.PaymentPayerRequisites.Party
                               //into RD
                               //from doc in RD
                                  select new {
                                      paymentRequest,
                                      paymentDocument //= doc
                               };
                Debug.WriteLine(String.Format("{0} {1}", "Соединение только по совпадению плательщиков (заявка, платёжка). Всего вариантов возможно: ", queryPayers.Count()));
                //foreach (var doc in queryPayers) {
                //    Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                //}
                //Debug.WriteLine("\n");

                // При этом задействовано заявок
                var queryPayersRequests = (from req in queryPayers
                                              select req.paymentRequest).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано заявок: ", queryPayersRequests.Count()));

                // При этом задействовано документов
                var queryPayersDocs = (from req in queryPayers
                                          select req.paymentDocument).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано документов: ", queryPayersDocs.Count()));
                Debug.WriteLine("\n");


                // Соединение по совпадению обеих сторон
                var queryBothParty = (from paymentRequest in queryPaymenRequests
                                     join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                               on new {
                                   P1 = paymentRequest.PaymentRequest.PartyPaySender,
                                   P2 = paymentRequest.PaymentRequest.PartyPayReceiver
                               } equals new {
                                   P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.Party,
                                   P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.Party
                               }
                               into RD
                               from doc in RD
                                select new {
                                    paymentRequest,
                                    paymentDocument = doc
                                }).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по совпадению обеих сторон (заявка, платёжка)", queryBothParty.Count()));
                //foreach (var doc in queryBothParty) {
                //    Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                //}
                //Debug.WriteLine("\n");

                // При этом задействовано заявок
                var queryBothPartyRequests = (from req in queryBothParty
                                              select req.paymentRequest).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано заявок: ", queryBothPartyRequests.Count()));

                // При этом задействовано документов
                var queryBothPartyDocs = (from req in queryBothParty
                                              select req.paymentDocument).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано документов: ", queryBothPartyDocs.Count()));
                Debug.WriteLine("\n");


                // Соединение по совпадению обеих сторон и попаданию даты оплаты платёжного документа в интервал даты "В оплату" заявки
                var queryBothPartyAndDate = (from paymentRequest in queryPaymenRequests
                                            join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                     on new {
                                         P1 = paymentRequest.PaymentRequest.PartyPaySender,
                                         P2 = paymentRequest.PaymentRequest.PartyPayReceiver
                                     } equals new {
                                         P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.Party,
                                         P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.Party
                                     }
                                    into RD
                                    from doc in RD
                                             // DateInPayment - пустая во всех аявках: where doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.DateInPayment.Date && doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.DateInPayment.AddDays(3).Date
                                             where doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.ExtDocDate.Date && doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.ExtDocDate.AddDays(16).Date
                                             select new {
                                        paymentRequest,
                                        paymentDocument = doc
                                    }).ToList();
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по совпадению обеих сторон и попаданию даты оплаты платёжного документа в интервал даты В оплату заявки (заявка, платёжка)", queryBothPartyAndDate.Count()));
                //foreach (var doc in queryBothPartyAndDate) {
                //    Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                //}
                //Debug.WriteLine("\n");

                // При этом задействовано заявок
                var queryBothPartyAndDateRequests = (from req in queryBothPartyAndDate
                                              select req.paymentRequest).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано заявок: ", queryBothPartyAndDateRequests.Count()));

                // При этом задействовано документов
                var queryBothPartyAndDateDocs = (from req in queryBothPartyAndDate
                                          select req.paymentDocument).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "При этом задействовано документов: ", queryBothPartyAndDateDocs.Count()));
                Debug.WriteLine("\n");


                // Соединение по совпадению обеих сторон и совпадению сумм в валюте оплаты, случай расходного документа
                var queryBothPartyAndSum = (from paymentRequest in queryPaymenRequests
                                           join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                            on new {
                                               P1 = paymentRequest.PaymentRequest.PartyPaySender,
                                               P2 = paymentRequest.PaymentRequest.PartyPayReceiver
                                            } equals new {
                                                P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.Party,
                                                P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.Party
                                            }
                                           into RD
                                           from doc in RD
                                           where doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.DateInPayment.Date && doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.DateInPayment.AddDays(3).Date
                                              && OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                           select new {
                                               paymentRequest,
                                               paymentDocument = doc
                                           }).ToList();
                Int32 queryBothPartyAndSumCount = 0;
                Int32 queryBothPartyAndSumValutaErrorCount = 0;
                foreach (var doc in queryBothPartyAndSum) {
                    csValuta pValuta = (from valuta in valutas
                                        where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                        select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryBothPartyAndSumValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        queryBothPartyAndSumCount++;
                    }
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по совпадению обеих сторон и совпадению сумм в валюте оплаты (заявка, платёжка), случай расходного документа", queryBothPartyAndSumCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryBothPartyAndSumValutaErrorCount));
                Debug.WriteLine("\n");

                /* Слишком много вариантов
                // Соединение только по совпадению плательщиов и сумм, случай расходного документа
                var queryPayerAndSum = from paymentRequest in queryPaymenRequests
                                       join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                       on paymentRequest.PaymentRequest.PartyPaySender equals paymentDocument.PaymentDocument.PaymentPayerRequisites.Party
                                       into RD
                                       from doc in RD
                                       where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                       //let crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.DateAccountChanged, paymentRequest.ObligationValuta, doc.PaymentValuta)
                                       //let paymentSumOutPaymentValuta = paymentRequest.SumOut * crossCource
                                       //let bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.SumOut), 0.01m) <= 0)
                                       //where bRes
                                       select new {
                                           paymentRequest,
                                           paymentDocument = doc
                                       };
                Int32 queryPayerAndSumCount = 0;
                Int32 queryPayerAndSumValutaErrorCount = 0;
                foreach (var doc in queryPayerAndSum) {
                    csValuta pValuta = (from valuta in valutas
                                        where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                        select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryPayerAndSumValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        queryPayerAndSumCount++;
                    }
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение только по совпадению плательщиов и сумм (заявка, платёжка), случай расходного документа", queryPayerAndSumCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryPayerAndSumValutaErrorCount));
                Debug.WriteLine("\n");
                */

                // Соединение по максимуму условий (абсолютно точное совпадение)
                List<HardConditionPair> hardConditionPair = new List<HardConditionPair>();
                List<fmCPRPaymentRequest> hardConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> hardConditionPaymentDocument = new List<fmCDocRCB>();
                // Полный набор максимальных условий:
                // Стороны попарно совпадают, суммы в пересчёте к сумме платежа совпадают (до 1 коп.), 
                // дата оплаты в платёжном документе отстоит от даты "к оплате" в заявке не более, чем на 2 дня
                var queryMaxConditions = from paymentRequest in queryInPaymentRequestRightly   //queryPaymenRequests
                                         join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                           on new {
                                               P1 = paymentRequest.PaymentRequest.PartyPaySender,
                                               P2 = paymentRequest.PaymentRequest.PartyPayReceiver
                                           } equals new {
                                               P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.Party,
                                               P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.Party
                                           }
                                            into RD
                                            from doc in RD
                                            where OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                               && doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.Date.Date //&& doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.Date.AddDays(3).Date
                                               && (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                         select new {
                                             paymentRequest,
                                             paymentDocument = doc
                                         };
                Int32 queryMaxConditionsCount = 0;
                Int32 queryMaxConditionsValutaErrorCount = 0;
                foreach (var doc in queryMaxConditions) {
                    csValuta pValuta = (from valuta in valutas
                                        where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                        select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryMaxConditionsValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        queryMaxConditionsCount++;
                        hardConditionPair.Add(new HardConditionPair() { paymentDocument = doc.paymentDocument.PaymentDocument, paymentRequest = doc.paymentRequest.PaymentRequest });
                        if (!hardConditionPaymentRequest.Contains(doc.paymentRequest.PaymentRequest)) {
                            hardConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        }
                        if (!hardConditionPaymentDocument.Contains(doc.paymentDocument.PaymentDocument)) {
                            hardConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        }
                    }
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по максимуму условий (абсолютно точное совпадение), случай расходного документа", queryMaxConditionsCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryMaxConditionsValutaErrorCount));
                Debug.WriteLine(String.Format("{0} {1}", "hard Заявок в слое, количество ", hardConditionPaymentRequest.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "hard Документов в слое, количество ", hardConditionPaymentDocument.Count()));
                Debug.WriteLine("\n");






                // Соединение по ослабленному критерию (стороны сравниваются по ИНН, т.е. КПП игнорируется)
                List<HardConditionPair> softConditionPair = new List<HardConditionPair>();
                List<fmCPRPaymentRequest> softConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> softConditionPaymentDocument = new List<fmCDocRCB>();
                // Набор ослабленных условий:
                // Стороны попарно совпадают, суммы в пересчёте к сумме платежа совпадают (до 1 коп.), 
                // дата оплаты в платёжном документе отстоит от даты "к оплате" в заявке не более, чем на 2 дня
                var querySoftConditions = from paymentRequest in queryInPaymentRequestRightly   //queryPaymenRequests
                                         join paymentDocument in queryOurDocPayer   //queryPaymentDocuments
                                           on new {
                                               P1 = paymentRequest.PaymentRequest.PartyPaySender.INN,
                                               P2 = paymentRequest.PaymentRequest.PartyPayReceiver.INN
                                           } equals new {
                                               P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.INN,
                                               P2 = paymentDocument.PaymentDocument.PaymentReceiverRequisites.INN
                                           }
                                            into RD
                                         from doc in RD
                                          where !hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !hardConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument)
                                             && OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                             && doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.Date.Date //&& doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.Date.AddDays(3).Date
                                             && (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                         select new {
                                             paymentRequest,
                                             paymentDocument = doc
                                         };
                Int32 querySoftConditionsCount = 0;
                Int32 querySoftConditionsValutaErrorCount = 0;
                foreach (var doc in querySoftConditions) {
                    csValuta pValuta = (from valuta in valutas
                                        where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                                        select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        querySoftConditionsValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        querySoftConditionsCount++;
                        softConditionPair.Add(new HardConditionPair() {
                            paymentDocument = doc.paymentDocument.PaymentDocument,
                            paymentRequest = doc.paymentRequest.PaymentRequest
                        });
                        //softConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        //softConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        if (!softConditionPaymentRequest.Contains(doc.paymentRequest.PaymentRequest)) {
                            softConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        }
                        if (!softConditionPaymentDocument.Contains(doc.paymentDocument.PaymentDocument)) {
                            softConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        }
                    }
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по ослабленному критерию (стороны сравниваются по ИНН, т.е. КПП игнорируется), случай расходного документа", querySoftConditionsCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", querySoftConditionsValutaErrorCount));
                Debug.WriteLine(String.Format("{0} {1}", "soft Заявок в слое, количество ", softConditionPaymentRequest.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "soft Документов в слое, количество ", softConditionPaymentDocument.Count()));
                Debug.WriteLine("\n");








                // Соединение по ещё более слабому критерию - получатель в платёжном документе отсутствует, т.к. не распознан
                List<HardConditionPair> halfConditionPair = new List<HardConditionPair>();
                List<fmCPRPaymentRequest> halfConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> halfConditionPaymentDocument = new List<fmCDocRCB>();
                // Набор ослабленных условий:
                // Стороны попарно совпадают, суммы в пересчёте к сумме платежа совпадают (до 1 коп.), 
                // дата оплаты в платёжном документе отстоит от даты "к оплате" в заявке не более, чем на 2 дня
                var queryHalfConditions = from paymentRequest in queryInPaymentRequestRightly   //queryPaymenRequests
                                          join paymentDocument in queryOurDocPayer
                                            on new {
                                                P1 = paymentRequest.PaymentRequest.PartyPaySender.INN
                                            } equals new {
                                                P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.INN
                                            }
                                             into RD
                                          from doc in RD
                                          where doc.PaymentDocument.PaymentReceiverRequisites.Party == null
                                             && (!hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !hardConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument))
                                             && (!softConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !softConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument))
                                             && OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                             && doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.Date.Date //&& doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.Date.AddDays(3).Date
                                             && (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                              // Дополнения для оптимизации
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != doc.PaymentDocument.PaymentPayerRequisites.BankAccount.Number.Substring(5, 3) || paymentRequest.SumOut == doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "978" || paymentRequest.SumOut * 40 < doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "840" || paymentRequest.SumOut * 31 < doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "398" || paymentRequest.SumOut < doc.SumOut * 6)
                                             && (paymentRequest.SumOut < doc.SumOut * 40 || paymentRequest.SumOut * 40 < doc.SumOut)
                                          select new {
                                              paymentRequest,
                                              paymentDocument = doc
                                          };
                Int32 queryHalfConditionsTotalCount = queryHalfConditions.Count();
                Int32 queryHalfConditionsCount = 0;
                Int32 queryHalfConditionsValutaErrorCount = 0;
                Int32 HalfCounter = 0;
                foreach (var doc in queryHalfConditions) {
                    csValuta pValuta = dictValuta[doc.paymentDocument.PaymentValuta];
                    //(from valuta in valutas
                    //                where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                    //                select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryHalfConditionsValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        //Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        //halfConditionPair.Add(new ConditionPair() {
                        //    paymentDocument = doc.paymentDocument.PaymentDocument,
                        //    paymentRequest = doc.paymentRequest.PaymentRequest
                        //});
                        //halfConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        //halfConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        if (!halfConditionPaymentRequest.Contains(doc.paymentRequest.PaymentRequest)) {
                            halfConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        }
                        if (!halfConditionPaymentDocument.Contains(doc.paymentDocument.PaymentDocument)) {
                            halfConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        }
                        queryHalfConditionsCount++;
                    }
                    HalfCounter++;
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по ещё более слабому критерию - получатель в платёжном документе отсутствует, т.к. не распознан, случай расходного документа", queryHalfConditionsCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryHalfConditionsValutaErrorCount));
                Debug.WriteLine(String.Format("{0} {1}", "half Заявок в слое, количество ", halfConditionPaymentRequest.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "half Документов в слое, количество ", halfConditionPaymentDocument.Count()));
                Debug.WriteLine("\n");













                // Соединение по самому слабому критерию - получатель в платёжном документе не учитывается вовсе
                List<HardConditionPair> lowConditionPair = new List<HardConditionPair>();
                List<fmCPRPaymentRequest> lowConditionPaymentRequest = new List<fmCPRPaymentRequest>();
                List<fmCDocRCB> lowConditionPaymentDocument = new List<fmCDocRCB>();
                // Набор ослабленных условий:
                // Стороны попарно совпадают, суммы в пересчёте к сумме платежа совпадают (до 1 коп.), 
                // дата оплаты в платёжном документе отстоит от даты "к оплате" в заявке не более, чем на 2 дня
                var queryLowConditions = from paymentRequest in queryPaymenRequests
                                          join paymentDocument in queryOurDocPayer
                                            on new {
                                                P1 = paymentRequest.PaymentRequest.PartyPaySender.INN
                                            } equals new {
                                                P1 = paymentDocument.PaymentDocument.PaymentPayerRequisites.INN
                                            }
                                             into RD
                                          from doc in RD
                                          where 
                                                (!hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !hardConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument))
                                             && (!softConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !softConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument))
                                             && (!halfConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             && !halfConditionPaymentDocument.Contains<fmCDocRCB>(doc.PaymentDocument))
                                             && OurBankAccounts.Contains<crmBankAccount>(doc.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                             && doc.DateAccountChanged.Date > paymentRequest.PaymentRequest.Date.Date //&& doc.DateAccountChanged.Date < paymentRequest.PaymentRequest.Date.AddDays(3).Date
                                             && (paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_BANK || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                             // Дополнения для оптимизации
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != doc.PaymentDocument.PaymentPayerRequisites.BankAccount.Number.Substring(5, 3) || paymentRequest.SumOut == doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "978" || paymentRequest.SumOut * 40 < doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "840" || paymentRequest.SumOut * 31 < doc.SumOut)
                                             && (paymentRequest.ObligationValuta.CodeCurrencyValue != "398" || paymentRequest.SumOut < doc.SumOut * 6)
                                             && (paymentRequest.SumOut < doc.SumOut * 40 || paymentRequest.SumOut * 40 < doc.SumOut)
                                         select new {
                                              paymentRequest,
                                              paymentDocument = doc
                                          };
                Int32 queryLowConditionsTotalCount = queryLowConditions.Count();
                Int32 queryLowConditionsCount = 0;
                Int32 queryLowConditionsValutaErrorCount = 0;
                Int32 LowCounter = 0;
                foreach (var doc in queryLowConditions) {
                    csValuta pValuta = dictValuta[doc.paymentDocument.PaymentValuta];
                        //(from valuta in valutas
                        //                where valuta.CodeCurrencyValue == doc.paymentDocument.PaymentValuta
                        //                select valuta).FirstOrDefault<csValuta>();
                    if (pValuta == null) {
                        queryLowConditionsValutaErrorCount++;
                        continue;
                    }
                    Decimal crossCource = csCNMValutaCourse.GetCrossCourceOnDate(uow, doc.paymentDocument.DateAccountChanged, doc.paymentRequest.ObligationValuta, pValuta);
                    Decimal paymentSumOutPaymentValuta = doc.paymentRequest.SumOut * crossCource;
                    Boolean bRes = (Decimal.Compare(Math.Abs(paymentSumOutPaymentValuta - doc.paymentDocument.PaymentDocument.PaymentCost), 0.01m) <= 0);
                    if (bRes) {
                        //Debug.Write(String.Format("({0}, {1}); ", doc.paymentRequest.PaymentRequest.ExtDocNumber, doc.paymentDocument.PaymentDocument.DocNumber));
                        //lowConditionPair.Add(new ConditionPair() {
                        //    paymentDocument = doc.paymentDocument.PaymentDocument,
                        //    paymentRequest = doc.paymentRequest.PaymentRequest
                        //});
                        queryLowConditionsCount++;
                        //lowConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        //lowConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        if (!lowConditionPaymentRequest.Contains(doc.paymentRequest.PaymentRequest)) {
                            lowConditionPaymentRequest.Add(doc.paymentRequest.PaymentRequest);
                        }
                        if (!lowConditionPaymentDocument.Contains(doc.paymentDocument.PaymentDocument)) {
                            lowConditionPaymentDocument.Add(doc.paymentDocument.PaymentDocument);
                        }
                    }
                    LowCounter++;
                }
                Debug.WriteLine("\n");
                Debug.WriteLine(String.Format("{0} {1}", "Соединение по самому слабому критерию - получатель в платёжном документе не учитывается вовсе, случай расходного документа", queryLowConditionsCount));
                Debug.WriteLine(String.Format("{0} {1}", "Неопознана валюта в платёжном документе, количество ", queryLowConditionsValutaErrorCount));
                Debug.WriteLine(String.Format("{0} {1}", "low Заявок в слое, количество ", lowConditionPaymentRequest.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "low Документов в слое, количество ", lowConditionPaymentDocument.Count()));
                Debug.WriteLine("\n");



                
                // Заявки и платёжные документы, которые не могут быть привязаны друг к другу
                /*
                var queryNoBindingRequests = (from paymentRequest in queryInPaymentRequestRightly
                                            where 
                                            //(paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                                //&& 
                                                  !hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                                && !softConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                                && !halfConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                                && !lowConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                                             orderby paymentRequest.PaymentRequest.ExtDocNumber ascending
                                             select new {
                                                ExtDocNumber = paymentRequest.PaymentRequest.ExtDocNumber,
                                                Number = paymentRequest.PaymentRequest.Number,
                                                Date = paymentRequest.PaymentRequest.Date.Date
                                            }).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, которые не привязываются", queryNoBindingRequests.Count()));
                foreach (var doc in queryNoBindingRequests) {
                    //Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                    Debug.WriteLine(String.Format("{0} \t\t {1} \t\t {2}", doc.ExtDocNumber, doc.Number, doc.Date));
                }
                Debug.WriteLine("\n");
                */

                // Непривязавшиеся заявки из числа претендовавших
                var queryNoBindingRequests = (from paymentRequest in totalConditionPaymentRequest
                                            where 
                                                //(paymentRequest.PaymentRequest.State == PaymentRequestStates.IN_PAYMENT || paymentRequest.PaymentRequest.State == PaymentRequestStates.PAYED)
                                                //&& 
                                                   !hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest)
                                                && !softConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest)
                                                && !halfConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest)
                                                && !lowConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest)
                                             orderby paymentRequest.ExtDocNumber ascending
                                             select new {
                                                ExtDocNumber = paymentRequest.ExtDocNumber,
                                                Number = paymentRequest.Number,
                                                Date = paymentRequest.Date.Date
                                            }).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "Заявки, которые не привязываются", queryNoBindingRequests.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "Из общего числа правильных, утверждённых к оплате", totalConditionPaymentRequest.Count()));
                foreach (var doc in queryNoBindingRequests) {
                    //Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                    Debug.WriteLine(String.Format("{0} \t\t {1} \t\t {2}", doc.ExtDocNumber, doc.Number, doc.Date));
                }
                Debug.WriteLine("\n");

                

                //var queryNoBindingRequests = from paymentRequest in queryPaymenRequests
                //            where !hardConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                //                && !softConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                //                && !halfConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                //                && !lowConditionPaymentRequest.Contains<fmCPRPaymentRequest>(paymentRequest.PaymentRequest)
                //            select paymentRequest;
                //Debug.WriteLine(String.Format("{0} {1}", "Заявки, которые не привязываются", queryNoBindingRequests.Count()));
                //foreach (var doc in queryNoBindingRequests) {
                //    Debug.Write(String.Format("{0}, ", doc.PaymentRequest.ExtDocNumber));
                //}
                //Debug.WriteLine("\n");


                //==========
                /*
                // Нашша сторона - плательщик (РАСХОДНЫЕ)
                var queryNoBindingPaymentDocs = (from paymentDocument in queryOurDocPayer
                                       where !queryOurPayerAndReceiver.Contains(paymentDocument)
                                       //OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                       //   && !(OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                       //        && OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentReceiverRequisites.BankAccount))
                                          && !hardConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                                          && !softConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                                          && !halfConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                                          && !lowConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                                       orderby paymentDocument.PaymentDocument.DocNumber ascending
                                       select new {
                                           DocNumber = paymentDocument.PaymentDocument.DocNumber,
                                           DocDate = paymentDocument.PaymentDocument.DocDate
                                       }).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "Платёжные документы, которые не привязываются (только расходные)", queryNoBindingPaymentDocs.Count()));
                foreach (var doc in queryNoBindingPaymentDocs) {
                    //Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                    Debug.WriteLine(String.Format("{0} \t\t {1}", doc.DocNumber, doc.DocDate));
                }
                Debug.WriteLine("\n");
                */

                // Непривязавшиеся документы из числа тех, что попали в испытание на привязку
                var queryNoBindingPaymentDocs = (from paymentDocument in totalConditionPaymentDocument
                                                 where //!queryOurPayerAndReceiver.Contains(paymentDocument)
                                                     //OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                                     //   && !(OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentPayerRequisites.BankAccount)
                                                     //        && OurBankAccounts.Contains<crmBankAccount>(paymentDocument.PaymentDocument.PaymentReceiverRequisites.BankAccount))
                                                    //&& 
                                                     !hardConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument)
                                                    && !softConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument)
                                                    && !halfConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument)
                                                    && !lowConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument)
                                                 orderby paymentDocument.DocNumber ascending
                                                 select new {
                                                     DocNumber = paymentDocument.DocNumber,
                                                     DocDate = paymentDocument.DocDate
                                                 }).Distinct();
                Debug.WriteLine(String.Format("{0} {1}", "Платёжные документы, которые не привязываются (только расходные)", queryNoBindingPaymentDocs.Count()));
                Debug.WriteLine(String.Format("{0} {1}", "Из общего числа правильных рассматриваемых", totalConditionPaymentDocument.Count()));
                foreach (var doc in queryNoBindingPaymentDocs) {
                    //Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                    Debug.WriteLine(String.Format("{0} \t\t {1}", doc.DocNumber, doc.DocDate));
                }
                Debug.WriteLine("\n");




                //var queryNoBindingPaymentDocs = from paymentDocument in queryOurDocPayer
                //                                where !hardConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                //                                    && !softConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                //                                    && !halfConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                //                                    && !lowConditionPaymentDocument.Contains<fmCDocRCB>(paymentDocument.PaymentDocument)
                //                                select paymentDocument;
                //Debug.WriteLine(String.Format("{0} {1}", "Платёжные документы, которые не привязываются (только расходные)", queryNoBindingPaymentDocs.Count()));
                //foreach (var doc in queryNoBindingPaymentDocs) {
                //    Debug.Write(String.Format("{0}, ", doc.PaymentDocument.DocNumber));
                //}
                //Debug.WriteLine("\n");

            }
        }

        #endregion

    }
}
