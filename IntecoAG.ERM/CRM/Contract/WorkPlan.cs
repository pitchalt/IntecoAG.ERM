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
using DevExpress.Persistent.BaseImpl;

using System.Windows.Forms;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс WorkPlan, представляющий план работ по Договору
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmWorkPlan")]
    public partial class WorkPlan : BaseRecord, IContractFactory, INewVersionSupport, IVersionMainObject   //BasePlan
    {
        public WorkPlan(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();

            //this.Current = new WorkPlanVersion(this.Session);
            //((WorkPlanVersion)(this.Current)).WorkPlan = this;
            //this.Current.Current = this.Current;  // Первая версия рабочего плана (WorkPlan) ссылается на себя, т.к. станет CURRENT

//            this.Current = new WorkPlanVersion(this.Session);
//            this.Current.WorkPlan = this;
 //           this.Current.Current = this.Current;  // Первая версия контракта ссылается на себя, т.к. станет CURRENT
 //           ((WorkPlanVersion)(this.Current)).WorkPlan = this;
 //           ((WorkPlanVersion)this.Current).Current = ((WorkPlanVersion)this.Current);  // Первая версия контракта ссылается на себя, т.к. станет CURRENT

 //           this.WorkPlanVersions.Add(this.Current);

            this.Current = new WorkPlanVersion(this.Session, VersionStates.VERSION_NEW);
            this.WorkPlanVersions.Add(this.Current);

        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// WorkPlanVersion
        /// </summary>
        private WorkPlanVersion _Current;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public WorkPlanVersion Current {
            get { return _Current; }
            set { SetPropertyValue<WorkPlanVersion>("Current", ref _Current, value); }
        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion(WorkPlanVersion scVersion) {
            WorkPlanVersion newcur = null;
            foreach (WorkPlanVersion cont in this.WorkPlanVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version not in VersionList");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (WorkPlanVersion cont in this.WorkPlanVersions) {
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
        }


        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        }

        #endregion


        #region IContractFactory Members

        BaseObject IContractFactory.Create(crmContractNewForm frm) {
            this.Current.ContractDocument.Delete();
            this.Current.ContractDocument = frm.Document;
            this.Current.ContractCategory = frm.Category;
            //                wp.Current.Ca
            if (frm.OurRole == PartyRole.CUSTOMER) {
                this.Current.Customer.Party = frm.OurParty;
                this.Current.Supplier.Party = frm.PartnerParty;
            }
            else {
                this.Current.Supplier.Party = frm.OurParty;
                this.Current.Customer.Party = frm.PartnerParty;
            }
            this.Current.StageStructure.FirstStage.DateBegin = frm.DateBegin;
            this.Current.StageStructure.FirstStage.DateEnd = frm.DateEnd;
            this.Current.StageStructure.FirstStage.Code = "ВЕД" + frm.Document.Number;
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