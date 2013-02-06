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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

using System.Windows.Forms;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс SimpleContract, представляющий объект Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmSimpleContract")]
    public partial class SimpleContract : ContractImplementation, IContractFactory, INewVersionSupport, IVersionMainObject
    {
        public SimpleContract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Важная строка: устанавливает, чтобы Oid определялся в AfterConstruction. Свойство - статическое
            // Это позволяет избежать лишних сохранений (с целью получения Oid)
//            OidInitializationMode = DevExpress.Persistent.BaseImpl.OidInitializationMode.AfterConstruction;

            // SHU 2011-07-20
            this.Current = new SimpleContractVersion(this.Session, VersionStates.VERSION_NEW);
            //this.Current.Current = this.Current;  // Первая версия контракта ссылается на себя, т.к. станет CURRENT
            this.SimpleContractVersions.Add(this.Current);
            //this.Current.ContractDocument = this.ContractDocument;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private SimpleContractVersion _Current;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public SimpleContractVersion Current {
            get { return _Current; }
            set { SetPropertyValue<SimpleContractVersion>("Current", ref _Current, value); }
        }

//        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
//        public ContractParty Customer {
//            get { return this.Current.Customer; }
//        }

//        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
//        public ContractParty Contragent {
//            get { return this.Current.Contragent; }
//        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion(SimpleContractVersion scVersion) {
            SimpleContractVersion newcur = null;
            foreach (SimpleContractVersion cont in this.SimpleContractVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version not in VersionList");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

                //vHelper = new VersionHelper(this.Session);
                //SimpleContractVersion newVers = (SimpleContractVersion)vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);

                //vHelper = new VersionHelper(this.Session);
                //vHelper.SetVersionStateExt(newVers, VersionStates.VERSION_CURRENT);
                //newVers.VersionState = VersionStates.VERSION_CURRENT;

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (SimpleContractVersion cont in this.SimpleContractVersions) {
                    if (cont == scVersion) continue;
                    if (cont.VersionState == VersionStates.VERSION_CURRENT) {
                        vHelper = new VersionHelper(this.Session);
                        vHelper.SetVersionStateExt(cont, VersionStates.VERSION_OLD);
                    } else if (cont.VersionState == VersionStates.VERSION_PROJECT) {
                        vHelper = new VersionHelper(this.Session);
                        vHelper.SetVersionStateExt(cont, VersionStates.VERSION_DECLINE);
                    }
                }

                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

                this.Current = scVersion;

                Session.FlushChanges();
            }
            this.Category = scVersion.Category;
            this.Contract.ContractDocument.Number = scVersion.Number;
            this.Contract.ContractDocument.Date = scVersion.Date;
            this.Contract.ContractDocument.DocumentCategory = scVersion.DocumentCategory;
        }


        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        }

        #endregion


        #region IContractFactory Members

        BaseObject IContractFactory.Create(crmContractNewForm frm) {
            this.Current.Category = frm.Category;
            this.Current.DocumentCategory = frm.Document.DocumentCategory;
            this.Current.Number = frm.Document.Number;
            this.Current.Date = frm.Document.Date;
            //                wp.Current.Ca
            if (frm.OurRole == PartyRole.CUSTOMER) {
                this.Current.Customer.Party = frm.OurParty;
                this.Current.Supplier.Party = frm.PartnerParty;
            }
            else {
                this.Current.Supplier.Party = frm.OurParty;
                this.Current.Customer.Party = frm.PartnerParty;
            }
            this.Current.DateBegin = frm.DateBegin;
            this.Current.DateEnd = frm.DateEnd;
            return this;
        }

        #endregion


        #region IVersionMainObject

        public VersionRecord GetCurrent() {
            return (VersionRecord)this.Current;
        }

        #endregion

    }
}