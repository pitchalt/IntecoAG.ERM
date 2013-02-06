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
    /// Класс ComplexContract, представляющий объект Договора
    /// </summary>
    [DefaultClassOptions]
    //[Persistent("crmComplexContract")]
    public partial class ComplexContract : ContractImplementation, IContractFactory, INewVersionSupport, IVersionMainObject
    {
        public ComplexContract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();

            // Важная строка: устанавливает, чтобы Oid определялся в AfterConstruction. Свойство - статическое
            // Это позволяет избежать лишних сохранений (с целью получения Oid)
            OidInitializationMode = DevExpress.Persistent.BaseImpl.OidInitializationMode.AfterConstruction;

            // Логика работы. 
            // В интерфейсе светится список всех объектов SimpleContract (и ComplexContract). При попытке создать
            // новый SimpleContract контроллер перехватывает операцию New и создаёт сам новый объект SimpleContract 
            // В AfterConstruction при этом создаётся объект SimpleContractVersion со статусом VERSION_NEW (заодно 
            // создаётся объект ContractDocument), ссылка на который записывается в Current. После этого контроллер
            // открывает форму редактирования созданного объекта SimpleContractVersion.
            // По завершению операции Approve создаётся копия записи SimpleContractVersion, которой назначается статус 
            // VERSION_CURRENT, а бывшей записи SimpleContractVersion со статусом VERSION_NEW устанавливается статус
            // CURRENT
            // См. контроллер ReplaceWinNewObjectViewController

            // ContractDocument cd = new ContractDocument(this.Session);



            
            // SHU 2011-07-20
            this.Current = new ComplexContractVersion(this.Session);
            this.Current.ComplexContract = this;
            //this.Current.Current = this.Current;  // Первая версия контракта ссылается на себя, т.к. станет CURRENT
            // Паша Помоему предыдущий оператор и следующий должны давать одно и тоже?
            this.ComplexContractVersions.Add(this.Current);

            //this.Current.ContractDocument = this.ContractDocument;
            
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private ComplexContractVersion _Current;
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public ComplexContractVersion Current {
            get { return _Current; }
            set { SetPropertyValue<ComplexContractVersion>("Current", ref _Current, value); }
        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion(ComplexContractVersion scVersion) {
            ComplexContractVersion newcur = null;
            foreach (ComplexContractVersion cont in this.ComplexContractVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version not in VersionList");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (ComplexContractVersion cont in this.ComplexContractVersions) {
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

            // Set Current for WorkPlans after approve ComplexContract
            {
                foreach (WorkPlanVersion wpv in this.Current.WorkPlanVersions) {
                    if (wpv.VersionState == VersionStates.VERSION_CURRENT) {
                        wpv.WorkPlan.Current = wpv;
                    }

                    // Присоединяем стороны к контракту
                    if (this.Current.ContractPartys.IndexOf(wpv.Customer) == -1) this.Current.ContractPartys.Add(wpv.Customer);
                    if (this.Current.ContractPartys.IndexOf(wpv.Supplier) == -1) this.Current.ContractPartys.Add(wpv.Supplier);
                }

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
            this.Current.Category = frm.Category;
            this.Current.DocumentCategory = frm.Document.DocumentCategory;
            this.Current.Number = frm.Document.Number;
            this.Current.Date = frm.Document.Date;
            //                wp.Current.Ca
            crmContractParty cp = new crmContractParty(this.Session);
            cp.Party = frm.OurParty;
            this.Current.ContractPartys.Add(cp);
            cp = new crmContractParty(this.Session);
            cp.Party = frm.PartnerParty;
            this.Current.ContractPartys.Add(cp);
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