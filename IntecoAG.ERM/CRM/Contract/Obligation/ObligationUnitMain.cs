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
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{

    /// <summary>
    /// Класс crmObligationUnitMain
    /// </summary>
    [Persistent("crmObligationUnitMain")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmObligationUnitMain : BaseObject //, IContractDealFactory, IVersionBusinessLogicSupport, IVersionMainObject
    {
        public crmObligationUnitMain(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private crmObligationUnit _Current;
        //[Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmObligationUnit Current {
            get { return _Current; }
            set { SetPropertyValue<crmObligationUnit>("Current", ref _Current, value); }
        }

        private bool _Closed;
        public bool Closed {
            get { return _Closed; }
            set { SetPropertyValue<bool>("Closed", ref _Closed, value); }
        }

        private DateTime _DateClosed;
        public DateTime DateClosed {
            get { return _DateClosed; }
            set { SetPropertyValue<DateTime>("DateClosed", ref _DateClosed, value); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion


/*
        #region IVersionBusinessLogicSupport

        public void ApproveVersion(crmFinancialDealVersion scVersion) {
            crmFinancialDealVersion newcur = null;
            foreach (crmFinancialDealVersion cont in this.DealVersions) //DealVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version not in VersionList");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (crmFinancialDealVersion cont in this.DealVersions) { //DealVersions) {
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

            // !!!SHU Проверить 4 строки ниже!
            this.Category = scVersion.Category;
            this.Contract.ContractDocument.Number = scVersion.ContractDocument.Number;
            this.Contract.ContractDocument.Date = scVersion.ContractDocument.Date;
            this.Contract.ContractDocument.DocumentCategory = scVersion.ContractDocument.DocumentCategory;

            //// Set Current for WorkPlans after approve crmComplexContract
            //{
            //    foreach (crmWorkPlanVersion wpv in this.Current.WorkPlanVersions) {
            //        if (wpv.VersionState == VersionStates.VERSION_CURRENT) {
            //            wpv.WorkPlan.Current = wpv;
            //        }

            //        // Присоединяем стороны к контракту
            //        if (this.Current.ContractPartys.IndexOf(wpv.Customer) == -1) this.Current.ContractPartys.Add(wpv.Customer);
            //        if (this.Current.ContractPartys.IndexOf(wpv.Supplier) == -1) this.Current.ContractPartys.Add(wpv.Supplier);
            //    }

            //    Session.FlushChanges();
            //}
        }


        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        }

        public void Approve(IVersionSupport obj) {
            crmFinancialDealVersion approvingObj = obj as crmFinancialDealVersion;
            if (approvingObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не может быть утверждён");
                return;
            }
            ApproveVersion(approvingObj);
        }

        #endregion

        #region IContractFactory Members

        BaseObject IContractDealFactory.Create(crmDealRegistrationForm frm) {
            this.Current.Category = frm.Category;

            //this.Current.ContractDocument.DocumentCategory = frm.Document.DocumentCategory;
            //this.Current.ContractDocument.Number = frm.Document.Number;
            //this.Current.ContractDocument.Date = frm.Document.Date;
            //                wp.Current.Ca
            crmContractParty cp = new crmContractParty(this.Session);
            cp.Party = frm.OurParty;
            //this.Current.ContractPartys.Add(cp);
            cp = new crmContractParty(this.Session);
            cp.Party = frm.PartnerParty;
            //this.Current.ContractPartys.Add(cp);
            this.Current.DateBegin = frm.DateBegin;
            this.Current.DateEnd = frm.DateEnd;

            this.Current.DescriptionShort = frm.DescriptionShort;
            ((crmDealWithoutStageVersion)this.Current).DealCode = frm.Number;
            this.Category = frm.Category;

            //this.Category
            //this.Contract
            //this.Current
            //this.DateRegistration
            //this.DealVersions вывел из проекта с заменой на FinancialDealVersions
            //this.FinancialDealVersions
            //this.State


            // Для сохранения данных о Служебной записке (не понятно, что у неё такое DocumentCategory)
            this.ContractDocument.Date = frm.Date;
            this.ContractDocument.Number = frm.Number;
            this.ContractDocument.DocumentCategory = frm.DocumentCategory;
            this.Current.Category = frm.Category;

            // Данные о контрактном документе, которым оформлен данный проект договора (ведомости)
            //this.Current.ContractDocument.Delete();
            //this.Current.ContractDocument = frm.Document;
            this.Current.ContractDocument.Date = frm.Date;
            this.Current.ContractDocument.Number = frm.Number;
            this.Current.ContractDocument.DocumentCategory = frm.DocumentCategory;

            this.Current.Price = frm.Price;
            this.Current.Valuta = frm.Valuta;

            if (frm.OurRole == PartyRole.CUSTOMER) {
                this.Current.Customer.Party = frm.OurParty;
                this.Current.Supplier.Party = frm.PartnerParty;
            } else {
                this.Current.Supplier.Party = frm.OurParty;
                this.Current.Customer.Party = frm.PartnerParty;
            }

            crmFinancialDealVersion dws = this.Current as crmFinancialDealVersion;
            if (dws != null) {
                dws.StageStructure.FirstStage.DateBegin = frm.DateBegin;
                dws.StageStructure.FirstStage.DateEnd = frm.DateEnd;
                dws.StageStructure.FirstStage.Code = "ВЕД" + frm.Number;
            }


//this.Current.Code
//Date Begin
//Date End
//Party
//Party
//Date
//Document Category
//Number

            return this;
        }

        #endregion

        #region IVersionMainObject

        public VersionRecord GetCurrent() {
            return (VersionRecord)this.Current;
        }

        #endregion
*/
    }
}