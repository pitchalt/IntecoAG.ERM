using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Xpo;
//
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.CRM.Party;
//
namespace IntecoAG.ERM.FM.StatementAccount {
    /// <summary>
    /// Логика импорта
    /// </summary>
    static public class fmCSAStatementAccountImportLogic {
        
        static public void PostProcess(IObjectSpace os, fmCSATaskImporter taskImporter, fmCSAImportResult importResult) {
            // Постобработка: Добавление банков, контрагентов, счетов
            if (importResult == null) return;

            using (IObjectSpace nos = os.CreateNestedObjectSpace()) {
                fmCSATaskImporter task = nos.GetObject<fmCSATaskImporter>(taskImporter);
                fmCSAImportResult result = nos.GetObject<fmCSAImportResult>(importResult);

                // Определение и добавление банков
                foreach (fmCSAStatementAccount sa in result.StatementOfAccounts) {
                    foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                        fmCSAStatementAccountDoc sad = requisites.StatementOfAccountDoc;
                        task.Importer.RequisitesBankProccess(sad.PaymentPayerRequisites);
                        task.Importer.RequisitesBankProccess(sad.PaymentReceiverRequisites);
                        nos.CommitChanges();
                    }
                }

                // Определение стороны по банку, счёту и значению свойства предпочтительная сторона объекта BankAccount
                //task.Importer.PartyProccessByAccount(result);
                //nos.CommitChanges();
                foreach (fmCSAStatementAccount sa in result.StatementOfAccounts) {
                    foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                        fmCSAStatementAccountDoc sad = requisites.StatementOfAccountDoc;
                        task.Importer.PartyProccessByAccountRequisite(sad.PaymentPayerRequisites);
                        task.Importer.PartyProccessByAccountRequisite(sad.PaymentReceiverRequisites);
                        nos.CommitChanges();
                    }
                }

                // Определение стороны по ИНН и КПП (если не определилась на предыдущем шаге как предпочтительная сторона в счёте)
                foreach (fmCSAStatementAccount sa in result.StatementOfAccounts) {
                    foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                        fmCSAStatementAccountDoc sad = requisites.StatementOfAccountDoc;
                        task.Importer.RequisitesPartyProccessByINNandKPP(sad.PaymentPayerRequisites);
                        task.Importer.RequisitesPartyProccessByINNandKPP(sad.PaymentReceiverRequisites);
                        nos.CommitChanges();
                    }
                }

                // Создание счетов
                foreach (fmCSAStatementAccount sa in result.StatementOfAccounts) {
                    foreach (fmCDocRCBRequisites requisites in sa.DocRCBRequisites) {
                        fmCSAStatementAccountDoc sad = requisites.StatementOfAccountDoc;
                        task.Importer.RequisitesStatementAccountProccess(sad.PaymentPayerRequisites);
                        task.Importer.RequisitesStatementAccountProccess(sad.PaymentReceiverRequisites);
                        nos.CommitChanges();
                    }
                }


            }

            importResult.ResultCode = 2;
        }

        static public void ClearImportResult(fmCSAImportResult importResult) {

            // Вариант зачистки только документов
            foreach (fmCSAStatementAccount sa in importResult.StatementOfAccounts) {
                ////sa.PayInDocs.DeleteObjectOnRemove = true;
                //while (sa.PayInDocs.Count() > 0) {
                //    //sa.PayInDocs.Remove(sa.PayInDocs[0]);
                //    fmCSAStatementAccountDoc saDoc = sa.PayInDocs[0];
                //    sa.PayInDocs.Remove(saDoc);
                //    saDoc.Delete();
                //}
                

                ////sa.PayOutDocs.DeleteObjectOnRemove = true;
                //while (sa.PayOutDocs.Count() > 0) {
                //    //sa.PayOutDocs.Remove(sa.PayOutDocs[0]);
                //    fmCSAStatementAccountDoc saDoc = sa.PayOutDocs[0];
                //    sa.PayOutDocs.Remove(saDoc);
                //    saDoc.Delete();
                //}

                importResult.Session.Delete(sa.PayInDocs);
                importResult.Session.Delete(sa.PayOutDocs);
                importResult.Session.PurgeDeletedObjects();

            }
            importResult.ResultCode = 0;

            /*
            // Вариант зачистки документов и выписок
            //importResult.StatementOfAccounts.DeleteObjectOnRemove = true;
            while (importResult.StatementOfAccounts.Count() > 0) {
                fmCSAStatementAccount sa = importResult.StatementOfAccounts[0];

                //sa.PayInDocs.DeleteObjectOnRemove = true;
                while (sa.PayInDocs.Count() > 0) {
                    fmCSAStatementAccountDoc saDoc = sa.PayInDocs[0];
                    sa.PayInDocs.Remove(saDoc);
                    saDoc.Delete();
                }

                //sa.PayOutDocs.DeleteObjectOnRemove = true;
                while (sa.PayOutDocs.Count() > 0) {
                    fmCSAStatementAccountDoc saDoc = sa.PayOutDocs[0];
                    sa.PayOutDocs.Remove(saDoc);
                    saDoc.Delete();
                }

                importResult.StatementOfAccounts.Remove(sa);
                sa.Delete();
            }
            importResult.ResultCode = 0;
            */
        }

        static public fmCSAImportResult ImportProcess(IObjectSpace os, fmCSATaskImporter taskImporter) {
            // Стадия 1. Загрузка выписок и документов выписок.
            // Возможные варианты исхода:
            // 0. Признака, что результаты импорта были зачищены. Все документы и выписки удалены. Возможен перезапуск загрузки из просмотра ImportResult.
            // 1. Ошибок нет. Тогда переход на Стадию 2.
            // -1. Ошибка загрузки секции счетов. Некоторые счета не найдены в системе.
            // -2. Признака, что не все документы выписки загрузились. Документы должны быть удалены.
            // 2. Ошибка загрузки секции выписок. Тогда переход на показ ImportResult, минуя все остальные стадии.
            // 3. Ошибка: не найден хотя бы один счёт в системе. Тогда переход на показ ImportResult, минуя все остальные стадии.
            //    В ImportResult процедура может быть продолжена (например, пользователь ввёл недостающие счета).
            // 4. Неожиданная ошибка обработки документов выписок (сбой в памяти и т.п.). Тогда переход на показ ImportResult, минуя все остальные стадии.
            // 5. Распознаваемая ошибка обработки документов выписок (не пройдена проверка формата, синтаксиса и т.п.). Тогда переход на показ ImportResult, минуя все остальные стадии.
            // Короче, при любом исходе всегда переход на результат импорта.

            //fmCSATaskImporter taskImporter = View.CurrentObject as fmCSATaskImporter;

            // Выполнение Стадии 1.
            fmCSAImportResult importResult = taskImporter.ExecuteTask() as fmCSAImportResult;
            if (importResult == null)
                throw new Exception("ImportResult object was not created.");
            os.CommitChanges();

            ImportProcessDop(os, importResult);

            return importResult;
        }

        static public void ImportProcessDop(IObjectSpace os, fmCSAImportResult importResult) {

            fmCSATaskImporter taskImporter = importResult.TaskImporter;
            
            // Чистка (Документы выписок, возможно сами выписки).
            if (importResult.ResultCode == -2) {
                fmCSAStatementAccountImportLogic.ClearImportResult(importResult);
                os.CommitChanges();
                // После этого importResult.ResultCode == 0
            }


            // Выполнение Стадии 2.
            if (importResult.ResultCode == 1) {
                // Постобработка: Добавление контрагентов, банков, счетов.
                // Обработка выписки
                fmCSAStatementAccountImportLogic.PostProcess(os, taskImporter, importResult);
                os.CommitChanges();
            }

            // Выполнение стадии 3.
            if (importResult.ResultCode == 2) {
                // Создание Платёжных документов для всех непривязавшихся документов выписок, запись в регистр  и т.п. В разбивку по типам.
                fmCSAMapDocs<fmCDocRCB> mpCreateDocs = os.CreateObject<fmCSAMapDocs<fmCDocRCB>>();

                mpCreateDocs.CreateAllPaymentDocuments(os, importResult);
                os.CommitChanges();

                mpCreateDocs.UpdateAllRegister(os, importResult);
                os.CommitChanges();
            }

            // Выполнение автопривязки
            if (importResult.ResultCode == 3) {
                importResult.AutoBinding(null);
                os.CommitChanges();
            }
        }


        //public static crmCParty GetOurParty(Session ssn) {
        //    // Наша организация
        //    crmCParty OurParty = null;
        //    if (crmUserParty.CurrentUserParty != null) {
        //        if (crmUserParty.CurrentUserParty.Value != null) {
        //            OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(ssn).Party;
        //        }
        //    }
        //    return OurParty;
        //}

    }
}
