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

namespace IntecoAG.ERM.FM.StatementAccount {

    // Выписка
    
    [NavigationItem]
    [Persistent("fmStatementOfAccountInfo")]
    public class fmStatementOfAccountInfo : csCComponent
    {
        public fmStatementOfAccountInfo(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmStatementOfAccounts);
            this.CID = Guid.NewGuid();
        }

        #region ПОЛЯ КЛАССА

        private DateTime _DateFrom;    // Дата с
        private DateTime _DateTo;   // Дата по

        private crmBank _Bank;
        private crmBankAccount _BankAccount;

        private String _BankName;   // банк файла выписки
        private String _BankAccountText;   // Счёт из файла выписки


        private Decimal _BalanceOfIncoming;   // Входящее сальдо
        private Decimal _BalanceOfOutgoing;   // Исходящее сальдо

        private Decimal _TotalRecaivedAtAccount;   // Всего поступило
        private Decimal _TotalWriteOfAccount;   // Всего списано

        private String _DataFormat;   // Формат данных: 1C и т.п.
        private String _CodePage;   // Кодировка файла с данными выписки (по стандартам 1С DOS или WINDOWS)

        private fmStatementOfAccounts _StatementOfAccounts;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// DateFrom - Дата с
        /// </summary>
        [Browsable(false)]
        public DateTime DateFrom {
            get { return _DateFrom; }
            set {
                SetPropertyValue<DateTime>("DateFrom", ref _DateFrom, value);
            }
        }

        /// <summary>
        /// DateTo - Дата с
        /// </summary>
        [Browsable(false)]
        public DateTime DateTo {
            get { return _DateTo; }
            set {
                SetPropertyValue<DateTime>("DateTo", ref _DateTo, value);
            }
        }

        /// <summary>
        /// Bank - банк
        /// </summary>
        public crmBank Bank {
            get { return _Bank; }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }

        /// <summary>
        /// BankName - Наименование банка из файла выписки, в настоящий момент (2012-02-13 - это название родительского каталога, в к-м лежит файл выписки)
        /// </summary>
        public string BankName {
            get { return _BankName; }
            set {
                SetPropertyValue<string>("BankName", ref _BankName, value);
            }
        }

        /// <summary>
        /// BankAccount - банковский счёт
        /// </summary>
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
            }
        }

        /// <summary>
        /// BankAccountText - Счёт из файла выписки
        /// </summary>
        public string BankAccountText {
            get { return _BankAccountText; }
            set {
                SetPropertyValue<string>("BankAccountText", ref _BankAccountText, value);
            }
        }

        /// <summary>
        /// BalanceOfIncoming - Входящее сальдо
        /// </summary>
        public Decimal BalanceOfIncoming {
            get { return _BalanceOfIncoming; }
            set {
                SetPropertyValue<Decimal>("BalanceOfIncoming", ref _BalanceOfIncoming, value);
            }
        }

        /// <summary>
        /// BalanceOfOutgoing - Исходящее сальдо
        /// </summary>
        public Decimal BalanceOfOutgoing {
            get { return _BalanceOfOutgoing; }
            set {
                SetPropertyValue<Decimal>("BalanceOfOutgoing", ref _BalanceOfOutgoing, value);
            }
        }

        /// <summary>
        /// TotalRecaivedAtAccount - Всего поступило
        /// </summary>
        public Decimal TotalRecaivedAtAccount {
            get { return _TotalRecaivedAtAccount; }
            set {
                SetPropertyValue<Decimal>("TotalRecaivedAtAccount", ref _TotalRecaivedAtAccount, value);
            }
        }

        /// <summary>
        /// TotalWriteOfAccount - Всего списано
        /// </summary>
        public Decimal TotalWriteOfAccount {
            get { return _TotalWriteOfAccount; }
            set {
                SetPropertyValue<Decimal>("TotalWriteOfAccount", ref _TotalWriteOfAccount, value);
            }
        }

        /// <summary>
        /// DataFormat - Формат данных: 1C и т.п.
        /// </summary>
        public String DataFormat {
            get { return _DataFormat; }
            set {
                SetPropertyValue<String>("DataFormat", ref _DataFormat, value);
            }
        }

        /// <summary>
        /// CodePage - Кодировка файла с данными выписки (по стандартам 1С DOS или WINDOWS)
        /// </summary>
        public String CodePage {
            get { return _CodePage; }
            set {
                SetPropertyValue<String>("CodePage", ref _CodePage, value);
            }
        }

        [Association("fmStatementOfAccounts-fmStatementOfAccountInfo")]
        public fmStatementOfAccounts StatementOfAccounts {
            get { return _StatementOfAccounts; }
            set { SetPropertyValue<fmStatementOfAccounts>("StatementOfAccounts", ref _StatementOfAccounts, value); }
        }

        #endregion

        #region МЕТОДЫ

        #endregion

    }

}
