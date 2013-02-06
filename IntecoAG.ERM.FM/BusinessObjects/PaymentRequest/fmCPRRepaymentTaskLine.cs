using System;
using System.ComponentModel;
//
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.FM.PaymentRequest {

    [Persistent("fmPRRepaymentTaskLine")]
    public class fmCPRRepaymentTaskLine : csCComponent, ITreeNode
    {
        public fmCPRRepaymentTaskLine(Session session)
            : base(session) {
        }

        public fmCPRRepaymentTaskLine(Session session, int level, fmCPRPaymentRequestObligation levelObject, fmCPRPaymentRequest paymentRequest, DateTime paymentDate, Decimal operationJournalSum, Decimal requestSum, fmCPRRepaymentTask repaymentTask, fmCPRRepaymentTaskLine parentLine)   //, Boolean isCashFlowRegister)
            : base(session) {
            _PaymentRequest = paymentRequest;
            _PaymentDate = paymentDate;
            _OperationJournalSum = operationJournalSum;
            _RequestSum = requestSum;
            _RepaymentTask = repaymentTask;
            _Level = level;
            _LevelObject = levelObject;

            //_IsCashFlowRegister = isCashFlowRegister;

            if (parentLine != null)
                parentLine.Lines.Add(this);
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRRepaymentTaskLine);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private fmCPRPaymentRequest _PaymentRequest;   // Ссылка на Заявку
        private Decimal _OperationJournalSum;   // Сумма по журналу операций fmCSAOperationJournal
        private Decimal _RequestSum;   // Сумма по Заявке

        private fmCPRRepaymentTask _RepaymentTask; // Ссылка на задачу сопосталвения
        private DateTime _PaymentDate; // Дата прохождения операции по счёту

        private fmCPRRepaymentTaskLine _ParentLine;   // Ссылка на вышестоящую запись

        private int _Level;   // уровень (самый верхний = 0)
        private fmCPRPaymentRequestObligation _LevelObject;   // Объект, который отображается на заданном уровне

        //private Boolean _IsCashFlowRegister;   // Факт, что запись отображена в регистре CashFlowRegister

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Ссылка на Заявку об оплате
        /// </summary>
        public fmCPRPaymentRequest PaymentRequest {
            get { return _PaymentRequest; }
            set {
                SetPropertyValue<fmCPRPaymentRequest>("PaymentRequest", ref _PaymentRequest, value);
            }
        }

        /// <summary>
        /// Дата прохождения операции по счёту
        /// </summary>
        //[RuleRequiredField]
        public DateTime PaymentDate {
            get { return _PaymentDate; }
            set {
                SetPropertyValue<DateTime>("PaymentDate", ref _PaymentDate, value);
            }
        }

        /// <summary>
        /// Сумма по журналу операций
        /// </summary>
        public Decimal OperationJournalSum {
            get {
                return _OperationJournalSum;
            }
            set {
                SetPropertyValue<Decimal>("OperationJournalSum", ref _OperationJournalSum, value);
            }
        }

        /// <summary>
        /// Сумма по Заявке
        /// </summary>
        public Decimal RequestSum {
            get {
                return _RequestSum;
            }
            set {
                SetPropertyValue<Decimal>("RequestSum", ref _RequestSum, value);

                // Пересчёт суммы родительской записи
                if (!IsLoading) {   // && this.ParentLine != null) {
                    if (this.Level == 1) {
                        this.ParentLine.RequestSum = GetSubLevelSum(this.ParentLine);
                    }
                    else if (this.Level == 2) {
                        this.ParentLine.RequestSum = GetSubLevelSum(this.ParentLine);
                    }
                }
            }
        }

        /// <summary>
        /// Уровень (самый верхний = 0)
        /// </summary>
        [Browsable(false)]
        public int Level {
            get {
                return _Level;
            }
            set {
                SetPropertyValue<int>("Level", ref _Level, value);
            }
        }

        /// <summary>
        /// Объект, который отображается на заданном уровне
        /// </summary>
        [Browsable(false)]
        public fmCPRPaymentRequestObligation LevelObject {
            get {
                return _LevelObject;
            }
            set {
                SetPropertyValue<fmCPRPaymentRequestObligation>("LevelObject", ref _LevelObject, value);
            }
        }

        ///// <summary>
        ///// Факт, что запись отображена в регистре CashFlowRegister
        ///// </summary>
        //[Browsable(false)]
        //public Boolean IsCashFlowRegister {
        //    get {
        //        return _IsCashFlowRegister;
        //    }
        //    set {
        //        SetPropertyValue<Boolean>("IsCashFlowRegister", ref _IsCashFlowRegister, value);
        //    }
        //}

        /// <summary>
        /// Ссылка на задачу сопоставления
        /// </summary>
        [Association("fmCPRRepaymentTask-fmCPRRepaymentTaskLines")]
        public fmCPRRepaymentTask RepaymentTask {
            get { return _RepaymentTask; }
            set { SetPropertyValue<fmCPRRepaymentTask>("RepaymentTask", ref _RepaymentTask, value); }
        }


        /// <summary>
        /// Заказ
        /// </summary>
        public fmCOrderExt Order {
            get {
                if (!IsLoading && LevelObject != null) {
                    return LevelObject.Order;
                }
                return null;
            }
        }

        /// <summary>
        /// Статья
        /// </summary>
        public fmCostItem CostItem {
            get {
                if (!IsLoading && LevelObject != null) {
                    return LevelObject.CostItem;
                }
                return null;
            }
        }

        /// <summary>
        /// Ссылка на вышестоящую запись
        /// </summary>
        [Association("fmCPRRepaymentTaskLine-fmCPRRepaymentTaskLines")]
        public fmCPRRepaymentTaskLine ParentLine {
            get {
                return _ParentLine;
            }
            set {
                SetPropertyValue<fmCPRRepaymentTaskLine>("ParentLine", ref _ParentLine, value);
            }
        }

        /// <summary>
        /// Список подчинённых записей
        /// </summary>
        [Aggregated]
        [Association("fmCPRRepaymentTaskLine-fmCPRRepaymentTaskLines", typeof(fmCPRRepaymentTaskLine))]
        public XPCollection<fmCPRRepaymentTaskLine> Lines {
            get {
                return GetCollection<fmCPRRepaymentTaskLine>("Lines");
            }
        }

        #endregion

        #region МЕТОДЫ

        private Decimal GetSubLevelSum(fmCPRRepaymentTaskLine parentLine) {
            Decimal sum = 0;
            foreach (fmCPRRepaymentTaskLine subLine in parentLine.Lines) {
                sum += subLine.RequestSum;
            }
            return sum;
        }

        #endregion

        #region Поддержка дерева

        IBindingList ITreeNode.Children {
            get {
                return Lines;
            }
        }

        string ITreeNode.Name {
            get {
                return PaymentDate.ToString("dd.MM.yyyy");
            }
        }

        ITreeNode ITreeNode.Parent {
            get {
                return ParentLine;
            }
        }

        #endregion
    }

}
