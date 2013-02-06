using System;
using System.ComponentModel;
//
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Регистратор счетов
    /// </summary>
    [NavigationItem("Money")]
    [Persistent("fmPRRegistrator")]
    public class fmCPRRegistrator : csCComponent
    {
        public fmCPRRegistrator(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRRegistrator);
            this.CID = Guid.NewGuid();

            Date = DateTime.Now;
            UseCounter++;
//            ReceiverDepartment = new hrmDepartment(this.Session);
//            Valuta = new csValuta(this.Session);
        }

        #region ПОЛЯ КЛАССА

        private String _Number; // номер регистрации
        private DateTime _Date; // дата поступления в финансовый отдел
        private Int32 _IntNumber;
        private Int32 _NextNumber;

        private String _InvoiceNumber; // номер счёта
        private DateTime _InvoiceDate; // дата счёта

        private csValuta _Valuta; // Валюта заявки
        private decimal _Summ; // Сумма заявки
        private csValuta _PaymentValuta; // Валюта заявки

        private fmCPRPaymentRequest _PaymentRequest;

        #endregion

        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Номер регистрации в числовой форме
        /// </summary>
        [Browsable(false)]
        [Indexed(Unique=true)]
        public Int32 IntNumber {
            get { return _IntNumber; }
            set { 
                SetPropertyValue<Int32>("IntNumber", ref _IntNumber, value);
                if (!IsLoading && value > 0) {
                    Number = value.ToString("000000");
                }
            }
        }
        /// <summary>
        /// Очередной номер
        /// </summary>
        [Browsable(false)]
        public Int32 NextNumber {
            get { return _NextNumber; }
            set {
                SetPropertyValue<Int32>("NextNumber", ref _NextNumber, value);
            }
        }
        /// <summary>
        /// Номер регистрации в символьной форме
        /// </summary>
        [Size(50)]
        public String Number {
            get { return _Number; }
            set {
                SetPropertyValue<String>("Number", ref _Number, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Дата поступления в финансовый отдел
        /// </summary>
        //[RuleRequiredField]
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }

        /// <summary>
        /// Номер счёта
        /// </summary>
        //[Appearance("fmCDocRCBPaymentOrder.DocNumber.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(50)]
        public String InvoiceNumber {
            get { return _InvoiceNumber; }
            set {
                SetPropertyValue<String>("InvoiceNumber", ref _InvoiceNumber, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Дата счёта
        /// </summary>
        //[RuleRequiredField]
        public DateTime InvoiceDate {
            get { return _InvoiceDate; }
            set {
                SetPropertyValue<DateTime>("InvoiceDate", ref _InvoiceDate, value);
            }
        }

        /// <summary>
        /// Наименование получателя
        /// </summary>
        public fmCPRPaymentRequest PaymentRequest {
            get { return _PaymentRequest; }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
                if (!IsLoading && value != null) {
                    this.InvoiceDate = value.ExtDocDate;
                    this.InvoiceNumber = value.ExtDocNumber;
                    this.Summ = value.Summ;
                    this.Valuta = value.Valuta;
                    this.PaymentValuta = value.PaymentValuta;
                }
            }
        }
        /// <summary>
        /// Получатель
        /// </summary>
        [PersistentAlias("PaymentRequest.PartyPayReceiver")]
        public crmCParty Receiver {
            get {
                if (PaymentRequest != null)
                    return PaymentRequest.PartyPayReceiver;
                else
                    return null;
            }
        }

        /// <summary>
        /// Комментарий заявки 
        /// </summary>
        [PersistentAlias("PaymentRequest.Comment")]
        public String Comment {
            get {
                if (PaymentRequest != null)
                    return PaymentRequest.Comment;
                else
                    return null;
            }
        }

        /// <summary>
        /// Статус заявки
        /// </summary>
        [PersistentAlias("PaymentRequest.State")]
        public PaymentRequestStates State {
            get {
                if (PaymentRequest != null)
                    return PaymentRequest.State;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Сумма заявки
        /// </summary>
        //[RuleRequiredField]
        public decimal Summ {
            get { return _Summ; }
            set {
                SetPropertyValue<decimal>("Summ", ref _Summ, value);
            }
        }


        /// <summary>
        /// Валюта заявки
        /// </summary>
        //[RuleRequiredField]
        public csValuta Valuta {
            get { return _Valuta; }
            set {
                SetPropertyValue<csValuta>("Valuta", ref _Valuta, value);
            }
        }

        /// <summary>
        /// Валюта платежа
        /// </summary>
        //[RuleRequiredField]
        public csValuta PaymentValuta {
            get { return _PaymentValuta; }
            set {
                SetPropertyValue<csValuta>("PaymentValuta", ref _PaymentValuta, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        public static Int32 GenerateNumber(Session ses ) {
            CriteriaOperator oper = CriteriaOperator.And(new BinaryOperator("IntNumber", -1, BinaryOperatorType.Equal));
#pragma warning disable 0162
            for (int probe = 0; probe < 5; probe++) {
#pragma warning restore 0162
                using (UnitOfWork uow = new UnitOfWork(ses.DataLayer)) {
                    fmCPRRegistrator ng = uow.FindObject<fmCPRRegistrator>(
                        PersistentCriteriaEvaluationBehavior.BeforeTransaction, oper);
                    if (ng == null) {
                        ng = new fmCPRRegistrator(uow) {
                            IntNumber = -1,
                            NextNumber = 0
                        };
                    }
                    Int32 number = ++ng.NextNumber;
                    //
                    uow.CommitChanges();
                    //
                    //return ng.NextNumber;
                    //                    return inv_type.Prefix + per + ng.NextNumber.ToString("00000");
                    return number;
                }
            }
            throw new LockConflictException();
        }

        #endregion

        public override bool ReadOnlyGet() {
            return true;
        }
    }

}
