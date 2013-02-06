using System;
using System.ComponentModel;
//
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.PaymentRequest {

    /// <summary>
    /// Персональные данные для служебной записки 
    /// </summary>
    [Persistent("fmCPRPaymentRequestMemorandumPeson")]
    public class fmCPRPaymentRequestMemorandumPeson : csCComponent
    {
        public fmCPRPaymentRequestMemorandumPeson(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCPRPaymentRequestMemorandumPeson);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private hrmStaff _Staff;    // Персона (где Таб. номер - это Code)
        private crmPhysicalPerson _PhysicalPerson;   // Физическое лицо, не входящее в список сотрудников
        private String _PersonalAccount;   // Лицевой счёт
        private String _TabNo;   // Табельный номер
        private hrmDepartment _PersonDepartment;   // ПОдразделение
        private decimal _PersonSumm; // Сумма 
        private fmPaymentRequestMemorandum _PaymentRequestMemorandum; // Ссылка на служебную записку

        #endregion

        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Сотрудник предприятия
        /// </summary>
        public hrmStaff Staff {
            get {
                return _Staff;
            }
            set {
                hrmStaff old = Staff;
                if (old != value) {
                    _Staff = value;
                    if (!IsLoading) {
                        if (value != null) {
                            PhysicalPerson = value.PhysicalPerson;
                            TabNo = value.BuhCode;
                            PersonDepartment = value.Department;
                        } else {
                            PhysicalPerson = null;
                        }
                        OnChanged("Staff", old, value);
                    }
                }
                //SetPropertyValue<hrmStaff>("Staff", ref _Staff, value);
            }
        }

        /// <summary>
        /// Физическое лицо, не входящее в список сотрудников
        /// </summary>
        public crmPhysicalPerson PhysicalPerson {
            get {
                return _PhysicalPerson;
            }
            set {
                crmPhysicalPerson old = PhysicalPerson;
                if (old != value) {
                    _PhysicalPerson = value;
                    if (!IsLoading) {
                        hrmStaff staff = null;
                        CriteriaOperator criteria = new BinaryOperator(new OperandProperty("PhysicalPerson"), new ConstantValue(PhysicalPerson), BinaryOperatorType.Equal);
                        //CriteriaOperator.And(new BinaryOperator(new OperandProperty("PhysicalPerson"), new ConstantValue(parent), BinaryOperatorType.Equal),
                        //                     new BinaryOperator(new OperandProperty("PaymentRequest"), new ConstantValue(request), BinaryOperatorType.Equal));
                        staff = Session.FindObject<hrmStaff>(PersistentCriteriaEvaluationBehavior.InTransaction, criteria);
                        if (Staff != staff) {
                            Staff = staff;
                        }
                        OnChanged("PhysicalPerson", old, value);
                    }
                }
                //SetPropertyValue<crmPhysicalPerson>("PhysicalPerson", ref _PhysicalPerson, value);
            }
        }

        /// <summary>
        /// Лицевой счёт
        /// </summary>
        [Size(30)]
        public String PersonalAccount {
            get {
                return _PersonalAccount;
            }
            set {
                SetPropertyValue<String>("PersonalAccount", ref _PersonalAccount, value);
            }
        }

        /// <summary>
        /// Табельный номер
        /// </summary>
        [Size(30)]
        public String TabNo {
            get {
                return _TabNo;
            }
            set {
                SetPropertyValue<String>("TabNo", ref _TabNo, value);
            }
        }

        /// <summary>
        /// Подразделение
        /// </summary>
        public hrmDepartment PersonDepartment {
            get {
                return _PersonDepartment;
            }
            set {
                SetPropertyValue<hrmDepartment>("PersonDepartment", ref _PersonDepartment, value);
            }
        }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal PersonSumm {
            get {
                return _PersonSumm;
            }
            set {
                SetPropertyValue<decimal>("PersonSumm", ref _PersonSumm, value);
            }
        }

        /// <summary>
        /// Ссылка на служебную записку
        /// </summary>
        //[Browsable(false)]
        [Association("fmPRPaymentRequestMemorandum-fmCPRPaymentRequestMemorandumPeson")]
        public fmPaymentRequestMemorandum PaymentRequestMemorandum {
            get {
                return _PaymentRequestMemorandum;
            }
            set {
                SetPropertyValue<fmPaymentRequestMemorandum>("PaymentRequestMemorandum", ref _PaymentRequestMemorandum, value);
            }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
