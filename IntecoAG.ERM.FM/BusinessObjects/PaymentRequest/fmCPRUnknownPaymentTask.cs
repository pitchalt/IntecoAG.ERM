using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Account;

namespace IntecoAG.ERM.FM.PaymentRequest {

    // Вид Заявки, отражающий документ выписки и соответствующий платёжный документ, 
    // для которого не удалось найти подходящую Заявку других видов

    /// <summary>
    /// Статусы заявки
    /// </summary>
    public enum UnknownPaymentStates {
        /// <summary>
        /// На обработку
        /// </summary>
        FOR_PROCESSING = 1,
        /// <summary>
        /// Обработана и закрыта
        /// </summary>
        CLOSED = 2
    }

    //[NavigationItem]
    [Persistent("fmPRUnknownPaymentTask")]
    public class fmCPRUnknownPaymentTask : csCComponent
    {
        public fmCPRUnknownPaymentTask(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmPaymentRequestMemorandum);
            this.CID = Guid.NewGuid();

            this.Date = DateTime.Now;
            this.State = UnknownPaymentStates.FOR_PROCESSING;
        }

        #region ПОЛЯ КЛАССА

        //private String _Subject;   // Предмет оплаты, описание
        //private crmCParty _Receiver;   // Получатель
        //private crmBank _ReceiverBank;   // Банк получателя
        //private crmBankAccount _ReceiverBankAccount;   // Счёт получателя

        private DateTime _Date;   // Дата создания задачи

        private fmCDocRCB _PaymentDocument;
        private UnknownPaymentStates _State;   // Статус задачи

        private string _Comment; // Комментарий

        private hrmDepartment _Curator; // Подразделение-куратор (Финансовый отдел)
        private hrmStaff _Manager;   // Сотрудник, ответственный за исполнение задачи

        private hrmDepartment _TaskPerformer;   // Подразделение-исполнитель задачи

        #endregion


        #region СВОЙСТВА КЛАССА
        /*
        /// <summary>
        /// Subject - Предмет оплаты, описание
        /// </summary>
        public String Subject {
            get { return _Subject; }
            set {
                SetPropertyValue<String>("Subject", ref _Subject, value);
            }
        }

        /// <summary>
        /// Receiver - Получатель
        /// </summary>
        public crmCParty Receiver {
            get { return _Receiver; }
            set {
                SetPropertyValue<crmCParty>("Receiver", ref _Receiver, value);
            }
        }

        /// <summary>
        /// ReceiverBank - банк получателя
        /// </summary>
        public crmBank ReceiverBank {
            get { return _ReceiverBank; }
            set {
                SetPropertyValue<crmBank>("ReceiverBank", ref _ReceiverBank, value);
            }
        }

        /// <summary>
        /// ReceiverBankAccount - расчётный счёт получателя
        /// </summary>
        public crmBankAccount ReceiverBankAccount {
            get { return _ReceiverBankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("ReceiverBankAccount", ref _ReceiverBankAccount, value);
            }
        }
        */

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime Date {
            get {
                return _Date;
            }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public UnknownPaymentStates State {
            get {
                return _State;
            }
            set {
                SetPropertyValue<UnknownPaymentStates>("State", ref _State, value);
            }
        }

        
        /// <summary>
        /// Платёжный документ, для которого не найдена подходящая Заявка
        /// </summary>
        //[Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public fmCDocRCB PaymentDocument {
            get { return _PaymentDocument; }
            set { SetPropertyValue<fmCDocRCB>("PaymentDocument", ref _PaymentDocument, value); }
        }


        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment {
            get {
                return _Comment;
            }
            set {
                SetPropertyValue<string>("Comment", ref _Comment, value);
            }
        }

        /// <summary>
        /// Подразделение-куратор (Финансовый отдел)
        /// </summary>
        [RuleRequiredField]
        public hrmDepartment Curator {
            get {
                return _Curator;
            }
            set {
                SetPropertyValue<hrmDepartment>("Curator", ref _Curator, value);
            }
        }

        /// <summary>
        /// Сотрудник, ответственный за исполнение задачи
        /// </summary>
        [RuleRequiredField]
        [DataSourceProperty("Curator.Staffs", DataSourcePropertyIsNullMode.SelectAll)]
        public hrmStaff Manager {
            get {
                return _Manager;
            }
            set {
                SetPropertyValue<hrmStaff>("Manager", ref _Manager, value);
            }
        }


        /// <summary>
        /// Подразделение-исполнитель задачи
        /// </summary>
        [RuleRequiredField]
        public hrmDepartment TaskPerformer {
            get {
                return _TaskPerformer;
            }
            set {
                SetPropertyValue<hrmDepartment>("TaskPerformer", ref _TaskPerformer, value);
            }
        }

        
        /// <summary>
        /// Список ассоциированных Заявок и сумм
        /// </summary>
        [Aggregated]
        [Association("fmCPRUnknownPaymentTask-fmCPRUnknownPaymentTaskLines", typeof(fmCPRUnknownPaymentTaskLine))]
        public XPCollection<fmCPRUnknownPaymentTaskLine> UnknownPaymentTaskLines {
            get { return GetCollection<fmCPRUnknownPaymentTaskLine>("UnknownPaymentTaskLines"); }
        }

        #endregion

        #region МЕТОДЫ



        /// <summary>
        /// Заполнение списка "Дата, сумма, Заявка=null, Задача"
        /// </summary>
        public void FillRepaymentTaskLine() {

            /*
            XPQuery<fmCSAOperationJournal> operationJournals = new XPQuery<fmCSAOperationJournal>(this.Session);
            var queryOperationJournals = from operationJournal in operationJournals
                                         group operationJournal by
                                            new {
                                                operationJournal.OperationDate,
                                                operationJournal.PaymentDocument
                                            }
                                             into registerGroup
                                             where registerGroup.Key.PaymentDocument == PaymentDocument
                                             select new {
                                                 OperationDate = registerGroup.Key.OperationDate //,
                                                 // Sum = (PaymentDocument.PaymentReceiverRequisites.INN == GetOurParty().INN) ? registerGroup.Sum(row => row.SumIn) : registerGroup.Sum(row => row.SumOut)
                                             };

            foreach (var operationJournal in queryOperationJournals) {
                XPQuery<fmCPRRepaymentTaskLine> repaymentTaskLines = new XPQuery<fmCPRRepaymentTaskLine>(this.Session);
                var queryRepaymentTaskLines = from repaymentTaskLine in repaymentTaskLines
                                              where repaymentTaskLine.PaymentDate == operationJournal.OperationDate
                                                 && repaymentTaskLine.RepaymentTask == this
                                              select repaymentTaskLine;
                if (queryRepaymentTaskLines.Count() == 0) {
                    // Добавление записи (записи могут только добавляться по смыслу выписок)
                    fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session);
                    rtl.PaymentDate = operationJournal.OperationDate;
                    rtl.PaymentRequest = null;
                    rtl.RepaymentTask = this;
                    if (PaymentDocument.PaymentReceiverRequisites.INN == GetOurParty().INN) {
                        rtl.Sum = (from OpJ in operationJournals
                                   where OpJ.OperationDate == operationJournal.OperationDate
                                      && OpJ.PaymentDocument == PaymentDocument
                                      && OpJ.PaymentDocument.PaymentReceiverRequisites.INN == GetOurParty().INN
                                   select OpJ.SumIn).Sum();
                    } else {
                        rtl.Sum = (from OpJ in operationJournals
                                   where OpJ.OperationDate == operationJournal.OperationDate
                                      && OpJ.PaymentDocument == PaymentDocument
                                      && OpJ.PaymentDocument.PaymentPayerRequisites.INN == GetOurParty().INN
                                   select OpJ.SumIn).Sum();
                    }
                }
            }

            // Добавление строк в fmCPRRepaymentTaskLine, которые порождены уже привязанными заявками, не имеющими отражения
            // в fmCPRRepaymentTaskLine, с датами, не отражёнными в fmCSAOperationJournal
            XPQuery<fmCPRRepaymentJurnal> repaymentJournals = new XPQuery<fmCPRRepaymentJurnal>(this.Session, true);
            var queryRepaymentJournals = from repaymentJournal in repaymentJournals
                                         where repaymentJournal.PaymentDocument == PaymentDocument
                                         //&& repaymentJournal.PaymentRequest == request
                                         select repaymentJournal;
            var queryRegs = from repaymentJournal in queryRepaymentJournals
                            group repaymentJournals by
                                new {
                                    repaymentJournal.PaymentDate,
                                    repaymentJournal.PaymentRequest,
                                    repaymentJournal.PaymentDocument
                                } into registerGroup
                            where registerGroup.Key.PaymentDocument == PaymentDocument
                            select new {
                                PaymentDate = registerGroup.Key.PaymentDate,
                                PaymentRequest = registerGroup.Key.PaymentRequest
                            };
            foreach (var item in queryRegs) {
                XPQuery<fmCPRRepaymentTaskLine> repaymentTaskLines = new XPQuery<fmCPRRepaymentTaskLine>(this.Session, true);
                var queryRepaymentTaskLines = from repaymentTaskLine in repaymentTaskLines
                                              where repaymentTaskLine.PaymentRequest == null
                                                 && repaymentTaskLine.PaymentDate == item.PaymentDate
                                                 && repaymentTaskLine.RepaymentTask == this
                                              select repaymentTaskLine;
                if (queryRepaymentTaskLines.Count() == 0) {
                    // Добавление записи (записи могут только добавляться по смыслу выписок)
                    fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session);
                    rtl.PaymentDate = item.PaymentDate;
                    rtl.PaymentRequest = null;
                    rtl.Sum = -item.PaymentRequest.Summ;
                    rtl.RepaymentTask = this;
                    this.RepaymentTaskLines.Add(rtl);
                }
            }
            */
        }

        /// <summary>
        /// Ручная привязка по кнопке
        /// </summary>
        /// <param name="request"></param>
        public virtual void ManualBinding(fmCPRPaymentRequest request) {
            /*
            // Разброс суммы заявки (напоминание: заявка не встречается в списке fmCPRRepaymentTaskLine согласно принципу формирования списка подходящих заявок)
            Decimal startSum = request.Summ;
            XPQuery<fmCPRRepaymentTaskLine> repaymentTaskLines = new XPQuery<fmCPRRepaymentTaskLine>(this.Session, true);
            var queryRepaymentTaskLines = from repaymentTaskLine in repaymentTaskLines
                                          where repaymentTaskLine.PaymentRequest == null
                                             && repaymentTaskLine.RepaymentTask == this
                                             && repaymentTaskLine.Sum > 0
                                          orderby repaymentTaskLine.PaymentDate
                                          select repaymentTaskLine;
            foreach (var repaymentTaskLine in queryRepaymentTaskLines) {
                // Подсчёт покрытия текущего платежа заявками
                Decimal coverSum = (from repaymentTaskLineCover in repaymentTaskLines
                                    where repaymentTaskLineCover.PaymentRequest != null
                                       && repaymentTaskLineCover.RepaymentTask == this
                                       && repaymentTaskLineCover.PaymentDate == repaymentTaskLine.PaymentDate
                                    select repaymentTaskLineCover.Sum).Sum();
                Decimal diffSum = repaymentTaskLine.Sum - coverSum;
                Decimal newSum = Math.Min(diffSum, startSum);
                if (diffSum > 0) {
                    // Создание записи на разность
                    fmCPRRepaymentTaskLine rtl = new fmCPRRepaymentTaskLine(Session);
                    rtl.PaymentDate = repaymentTaskLine.PaymentDate;
                    rtl.PaymentRequest = request;
                    rtl.Sum = newSum;
                    rtl.RepaymentTask = this;
                    this.RepaymentTaskLines.Add(rtl);
                    startSum -= newSum;
                    if (startSum <= 0)
                        return;
                }
            }
            */
        }

        private crmCParty GetOurParty() {
            // Наша организация
            crmCParty OurParty = null;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    OurParty = (crmCParty)crmUserParty.CurrentUserPartyGet(this.Session).Party;
                }
            }
            return OurParty;
        }

        /*
        public void ClearRequestCollection() {
            _AllRequests = null;
        }
        */


        #endregion

    }

}
