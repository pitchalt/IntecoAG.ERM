#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.StatementAccount;

namespace IntecoAG.ERM.FM.Docs
{
    [Flags]
    public enum ImportErrorState : uint {
        RCBIC_NOT_DEFINED = 1,   // Не задан БИК
        PREFFERED_PARTY_WARNING = 2,   // Предпочтительная сторона не сопадает с заданной по ИНН и КПП
        PREFFERED_PARTY_IS_CLOSED = 4,   // Предпочтительная сторона закрыта
        PREFFERED_PARTY_NOT_DEFINED = 8,   // Предпочтительная сторона не определена
        PARTY_IS_CLOSED = 16,   // Найденная сторона оказалась закрытой
        LEGAL_PERSON_IS_CLOSED = 32,   // Найденное юридическое лицо оказалаоь закрытым
        ADDRESS_NOT_DEFINED = 64,   // доопределить адрес
        LEGAL_PERSON_NOT_FOUND = 128,   // Не найдено юридическое лицо
        PARTY_NOT_DEFINED = 256,   // сторона не определена
        KPP_NOT_DEFINED = 512,   // Не задано КПП
        PHYSICAL_PARTY_NOT_DEFINED = 1024,   // Физическая сторона не устанволена
        DEFINE_OBJECTS = 2048,   // Доопределить недостающие объекты и создать расчётный счёт
        LEGAL_PERSON_KPP_PROBLEM = 4096   // Юридическое лицо не имеет KPP, а оно обязательно при сохранении
    }

    /// <summary>
    /// Класс fmCPaymentRequisites, представляющий платёжные реквизиты, например на платёжных полручениях
    /// Перечисление реквизитов - в "ПОЛОЖЕНИЕ О БЕЗНАЛИЧНЫХ РАСЧЕТАХ В РОССИЙСКОЙ ФЕДЕРАЦИИ" 
    /// Глава 2. РАСЧЕТНЫЕ ДОКУМЕНТЫ, ПОРЯДОК ИХ ЗАПОЛНЕНИЯ, ПРЕДСТАВЛЕНИЯ, ОТЗЫВА И ВОЗВРАТА. Пункт 2.10
    /// </summary>
    [DefaultProperty("NameParty")]
    [Persistent("fmDocRCBRequisites")]
    public partial class fmCDocRCBRequisites : csCComponent
    {
        public fmCDocRCBRequisites(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ComponentType = typeof(fmCDocRCBRequisites);
            this.CID = Guid.NewGuid();

            AllowEditProperty = true;
        }

        #region ПОЛЯ КЛАССА

        private bool _AllowEditProperty;   // Разрешение редактирования

        // Организация (не банк) crmCParty
        private crmCParty _Party;
        private crmCPerson _Person;
        private String _NameParty;
        private String _INN;
        private String _KPP;
        private String _AccountParty;

        // Банк crmBank
        private crmBankAccount _BankAccount;
        private crmBank _Bank;
        private String _BankName;
        private String _BankLocation;
        private String _RCBIC;
        private String _AccountBank;

        private fmCSAStatementAccount _StatementOfAccount;   // Выписка

        private uint _ImportErrorStates;

        private fmCSAStatementAccountDoc _StatementOfAccountDoc;   // Документ выписки
        private fmCDocRCB _PaymentDoc;   // Платёжный документ

        #endregion


        #region СВОЙСТВА КЛАССА


        /// <summary>
        /// Разрешение редактирования
        /// </summary>
        [ImmediatePostData]
        [Browsable(false)]
        public bool AllowEditProperty {
            get { return _AllowEditProperty; }
            set {
                SetPropertyValue<bool>("AllowEditProperty", ref _AllowEditProperty, value);
            }
        }

        /// <summary>
        /// Персона (если она обнаружена, а сторона не была найдена)
        /// </summary>
        [Browsable(false)]
        public crmCPerson Person {
            get { return _Person; }
            set {
                SetPropertyValue<crmCPerson>("Person", ref _Person, value);
            }
        }

        [Browsable(false)]
        public List<crmBankAccount> BankAccountList {
            get {
                // Находим все такие Person, что Person.Partys содержит Party

                XPQuery<crmBankAccount> bankAccount = new XPQuery<crmBankAccount>(this.Session);
                var query = from bac in bankAccount
                            //where bac.Person.Partys.Contains<crmCParty>(Party)  // Закомментарил, т.к. не знаю пока как исправить
                            select bac;
                // Замечание. bac.Person.Partys является пустым набором, не знаю пока как грузить
                return (List<crmBankAccount>)(Activator.CreateInstance(typeof(List<>).MakeGenericType(query.ElementType), query));
            }
        }


        /// <summary>
        /// Организация
        /// </summary>
        [ImmediatePostData]
        public crmCParty Party {
            get { return _Party; }
            set { 
                SetPropertyValue<crmCParty>("Party", ref _Party, value);
                if (!IsLoading && Party != null) {
                    NameParty = Party.NameFull;
                    INN = Party.INN;
                    KPP = Party.KPP;

                    OnChanged("NameFull");
                    OnChanged("INN");
                    OnChanged("KPP");
                }
            }
        }

        /// <summary>
        /// PayerBankAccount - Счёт плательщика
        /// </summary>
        [ImmediatePostData]
        //[DataSourceProperty("Party.Person.BankAccounts")] //, DataSourcePropertyIsNullMode.SelectAll, "Person = @This.Party.Person" )]
        //[DataSourceProperty("BankAccountList")]
        [DataSourceProperty("Party.Person.BankAccounts", DataSourcePropertyIsNullMode.SelectAll)]  //"Person = @This.Party.Person" )]
        public crmBankAccount BankAccount {
            get { return _BankAccount; }
            set {
                SetPropertyValue<crmBankAccount>("BankAccount", ref _BankAccount, value);
                if (!IsLoading && BankAccount != null) {   // && AllowEditProperty) {
                    AccountParty = string.Empty;
                    AccountBank = string.Empty;
                    RCBIC = string.Empty;
                    // = string.Empty;
                    //LocationBank = string.Empty;

                    if (BankAccount.Bank != null) {
                        AccountParty = BankAccount.Number;
                        AccountBank = BankAccount.Bank.KorAcc;
                        RCBIC = BankAccount.Bank.BIC;
                        // = BankAccount.Bank.FullName;
                        //if (BankAccount.Bank.Party != null && BankAccount.Bank.Party.AddressLegal != null) {
                        //    LocationBank = BankAccount.Bank.Party.AddressLegal.AddressString;
                        //} else {
                        //    LocationBank = string.Empty;
                        //}
                    }

                    OnChanged("AccountParty");
                    OnChanged("AccountBank");
                    OnChanged("BIC");
                    OnChanged("Bank");
                    //OnChanged("LocationBank");
                }
            }
        }


        // СВОЙСТВА ПЛАТЕЛЬЩИКА/ПОЛУЧАТЕЛЯ

        /// <summary>
        /// Наименвоание плательзика/получателя
        /// </summary>
        //[Appearance("fmCDocRCBRequisites.NameParty.Enabled", Method = "AllowEditPayer", Enabled = false)]
        //[RuleRequiredField]
        [Size(300)]
        public String NameParty {
            get { return _NameParty; }
            set {
                SetPropertyValue<String>("NameParty", ref _NameParty, value);
            }
        }

        /// <summary>
        /// ИНН плательзика/получателя
        /// </summary>
        [Size(30)]
        public String INN {
            get { return _INN; }
            set { SetPropertyValue<String>("INN", ref _INN, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// КПП плательзика/получателя
        /// </summary>
        [Size(30)]
        public String KPP {
            get { return _KPP; }
            set { SetPropertyValue<String>("KPP", ref _KPP, value == null ? String.Empty : value.Trim()); }
        }

        /// <summary>
        /// Номер счёта плательзика/получателя
        /// </summary>
        //[RuleRequiredField]
        [Size(50)]
        public String AccountParty {
            get { return _AccountParty; }
            set { SetPropertyValue<String>("AccountParty", ref _AccountParty, value == null ? String.Empty : value.Trim()); }
        }


        // БАНКОВСКИЕ СВОЙСТВА

        /// <summary>
        /// Банк плательзика/получателя
        /// </summary>
        [Size(300)]
        public crmBank Bank {
            get { return _Bank; }
            set {
                SetPropertyValue<crmBank>("Bank", ref _Bank, value);
            }
        }

        /// <summary>
        /// Наименвоание банка плательзика/получателя
        /// </summary>
        //[RuleRequiredField]
        [Size(300)]
        public String BankName {
            get { return _BankName; }
            set {
                SetPropertyValue<String>("BankName", ref _BankName, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// Расположение банка плательзика/получателя
        /// </summary>
        [Size(300)]
        public String BankLocation {
            get { return _BankLocation; }
            set {
                SetPropertyValue<String>("BankLocation", ref _BankLocation, value == null ? String.Empty : value.Trim());
            }
        }

        /// <summary>
        /// БИК банка плательзика/получателя
        /// </summary>
        //[RuleRequiredField]
        [Size(20)]
        public String RCBIC {
            get { return _RCBIC; }
            set {
                string value1 = (value == null) ? String.Empty : value.Trim();
                value1 = ((value1.Length == 8) ? "0" : "") + value;
                // Обработка других ошибок пока отсутствует.
                SetPropertyValue<String>("RCBIC", ref _RCBIC, value1);
            }
        }

        /// <summary>
        /// Корр. счёт плательзика/получателя
        /// </summary>
        //[RuleRequiredField]
        [Size(50)]
        public String AccountBank {
            get { return _AccountBank; }
            set { SetPropertyValue<String>("AccountBank", ref _AccountBank, value == null ? String.Empty : value.Trim()); }
        }


        [Association("fmStatementOfAccounts-fmCDocRCBRequisites")]
        public fmCSAStatementAccount StatementOfAccount {
            get { return _StatementOfAccount; }
            set { SetPropertyValue<fmCSAStatementAccount>("StatementOfAccount", ref _StatementOfAccount, value); }
        }

        /// <summary>
        /// Документ выписки
        /// </summary>
        public fmCSAStatementAccountDoc StatementOfAccountDoc {
            get {
                return _StatementOfAccountDoc;
            }
            set {
                SetPropertyValue<fmCSAStatementAccountDoc>("StatementOfAccountDoc", ref _StatementOfAccountDoc, value);
            }
        }

        /// <summary>
        /// Платёжный документ
        /// </summary>
        public fmCDocRCB PaymentDoc {
            get {
                return _PaymentDoc;
            }
            set {
                SetPropertyValue<fmCDocRCB>("PaymentDoc", ref _PaymentDoc, value);
            }
        }

        /// <summary>
        /// Битовый массив кодов ошибок
        /// </summary>
        public uint ImportErrorStates {
            get { return _ImportErrorStates; }
            set { SetPropertyValue<uint>("ImportErrorStates", ref _ImportErrorStates, value); }
        }


        /// <summary>
        /// Список ошибок согласно коду ImportErrorStates
        /// </summary>
        [PersistentAlias("ImportErrorStates")]
        [Size(1000)]
        public String ImportErrorDescription {
            get {
                String res = "";
                if ((ImportErrorStates & 1) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Не задан БИК";
                }
                if ((ImportErrorStates & 2) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Предпочтительная сторона не сопадает с заданной по ИНН и КПП";
                }
                if ((ImportErrorStates & 4) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Предпочтительная сторона закрыта";
                }
                if ((ImportErrorStates & 8) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Предпочтительная сторона не определена";
                }
                if ((ImportErrorStates & 16) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Найденная сторона оказалась закрытой";
                }
                if ((ImportErrorStates & 32) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Найденное юридическое лицо оказалаоь закрытым";
                }
                if ((ImportErrorStates & 64) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Необходимо доопределить адрес";
                }
                if ((ImportErrorStates & 128) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Не найдено юридическое лицо";
                }
                if ((ImportErrorStates & 256) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Сторона не определена";
                }
                if ((ImportErrorStates & 512) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Не задано КПП";
                }
                if ((ImportErrorStates & 1024) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Физическая сторона не устанволена";
                }
                if ((ImportErrorStates & 2048) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Доопределить недостающие объекты и создать расчётный счёт";
                }
                if ((ImportErrorStates & 4096) != 0) {
                    res += ((res=="") ? "" : Environment.NewLine) + "Юридическое лицо не имеет KPP, а оно обязательно при сохранении";
                }
                return res;
            }
        }

        #endregion


        #region МЕТОДЫ

        public void SetAllowEdit() {
            AllowEditProperty = true;
        }

        public void SetDisAllowEdit() {
            AllowEditProperty = false;
        }

        protected override void OnSaving() {
            AllowEditProperty = false;
            base.OnSaving();
        }

        #endregion
    }

}