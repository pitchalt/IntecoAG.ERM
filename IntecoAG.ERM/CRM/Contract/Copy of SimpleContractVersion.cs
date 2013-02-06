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
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;

using System.Windows.Forms;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс SimpleContractVersion, представляющий объект Договора
    /// </summary>
    [DefaultClassOptions]
    [Persistent("crmSimpleContractVersion")]
    public partial class SimpleContractVersion : ContractVersion
    {
        public SimpleContractVersion() : base() { }
        public SimpleContractVersion(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            OidInitializationMode = DevExpress.Persistent.BaseImpl.OidInitializationMode.AfterConstruction;
            this.VersionState = VersionStates.VERSION_NEW;
            this.Contragent = new ContractParty(this.Session);
            this.Customer = new ContractParty(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        // Заказчик
        private ContractParty _Customer;
        [Persistent("Customer")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public ContractParty Customer {
            get { return _Customer; }
            set { SetPropertyValue<ContractParty>("Customer", ref _Customer, value); }
        }
        // Исполнитель
        private ContractParty _Contragent;
        [Persistent("Contragent")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public ContractParty Contragent {
            get { return _Contragent; }
            set { SetPropertyValue<ContractParty>("Contragent", ref _Contragent, value); }
        }
        // План поставок
        private DeliveryPlan _DeliveryPlan;
        [Persistent("DeliveryPlan")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public DeliveryPlan DeliveryPlan {
            get { return _DeliveryPlan; }
            set { SetPropertyValue<DeliveryPlan>("DeliveryPlan", ref _DeliveryPlan, value); }
        }

        // План расчётов платежей)
        private PaymentPlan _PaymentPlan;
        [Persistent("PaymentPlan")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public PaymentPlan PaymentPlan {
            get { return _PaymentPlan; }
            set { SetPropertyValue<PaymentPlan>("PaymentPlan", ref _PaymentPlan, value); }
        }




        /// <summary>
        /// Current - Ссылка на версию с признаком VersionState = CURRENT
        /// </summary>
        protected SimpleContractVersion _Current;
        public SimpleContractVersion Current {
            get { return _Current; }
            set { SetPropertyValue<SimpleContractVersion>("Current", ref _Current, value); }
        }

        #endregion


        #region МЕТОДЫ

        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        protected override void OnSaved() {
            // Обновление списка ContractPartys и WorkPlanVersions
            // Customer, Contragent
            
            List<ContractParty> ContractPartyListForRemove = new List<ContractParty>();

            foreach (ContractParty cp in this.ContractPartys) {
                if (cp != Customer & cp != Contragent) ContractPartyListForRemove.Add(cp);
            }
            foreach (ContractParty cp in ContractPartyListForRemove) this.ContractPartys.Remove(cp);

            if (this.ContractPartys.IndexOf(this.Customer) == -1) this.ContractPartys.Add(Customer);
            if (this.ContractPartys.IndexOf(this.Contragent) == -1) this.ContractPartys.Add(Contragent);

            base.OnSaved();
        }

        #endregion

        
        #region МЕТОДЫ ДЛЯ ПОДДЕРЖКИ ВЕРСИОННОСТИ

        public override List<IVersionSupport> GetDependentObjects() {
            List<IVersionSupport> htDependenceObjects = new List<IVersionSupport>();
            
//            if (ContractWork != null) {
//                htDependenceObjects.Add(this.ContractWork);
//            }

            if (Contragent != null) {
                htDependenceObjects.Add(Contragent);
            }

            if (Customer != null) {
                htDependenceObjects.Add(Customer);
            }

            if (this.PaymentPlan != null) {
                htDependenceObjects.Add(PaymentPlan);
            }

            if (this.DeliveryPlan != null) {
                htDependenceObjects.Add(DeliveryPlan);
            }

            
            if (this.WorkPlanVersions != null) {
                if (!WorkPlanVersions.IsLoaded) WorkPlanVersions.Load();
                foreach (WorkPlanVersion cd in WorkPlanVersions) {
                    htDependenceObjects.Add(cd);
                }
            }

            if (this.ContractPartys != null) {
                if (!ContractPartys.IsLoaded) ContractPartys.Load();
                foreach (ContractParty cd in ContractPartys) {
                    htDependenceObjects.Add(cd);
                }
            }
            
            return htDependenceObjects;
        }

        public override IVersionSupport CreateCopyObjects() {
            SimpleContractVersion objCopy = new SimpleContractVersion(this.Session);

            // Ищем запись c VersionState = CURRENT (это this.Current)

/*
            XPCollection<SimpleContractVersion> currentVers = new XPCollection<SimpleContractVersion>(this.Session); //new Session());  //, criteria, sortProps);
            if (!currentVers.IsLoaded) currentVers.Load();

            // Выбираем все записи SimpleContractVersion, у которых
            // Contract == this.Contract
            OperandProperty propContract = new OperandProperty("Contract"); ;
            CriteriaOperator opContract = propContract == this.Contract;

            CriteriaOperator criteria = new GroupOperator(GroupOperatorType.And,
                new BinaryOperator(new OperandProperty("VersionState"), 1, BinaryOperatorType.Equal),
                new NotOperator(new NullOperator("Contract")),
                opContract
                );

            currentVers.Filter = criteria;   // opCur;

            //CriteriaOperator op = CriteriaOperator.Parse("VersionState = 1");  // + VersionStates.CURRENT.ToString() + "");
            //currentVers.Filter = op;

            if (currentVers.Count == 0) return null;
*/
            // Новую версию создаём из текущей версии (CURRENT)
            SimpleContractVersion scv = (SimpleContractVersion)this.Current;   // currentVers[0];

            objCopy.Current = scv.Current;
            objCopy.Contract = scv.Contract;
            objCopy.LinkToEditor = scv.LinkToEditor;
            objCopy.ContractDocument = scv.ContractDocument;
            objCopy.VersionContractDocument = scv.VersionContractDocument;
            objCopy.Description = scv.Description;
            objCopy.VersionedContract = scv.VersionedContract;
            objCopy.VersionState = scv.VersionState;
            objCopy.PrevVersion = scv.PrevVersion;
            objCopy.PaymentPlan = scv.PaymentPlan;
            objCopy.IsOfficial = false;
            objCopy.IsCurrent = false;
            objCopy.DeliveryPlan = scv.DeliveryPlan;
            objCopy.Customer = scv.Customer;
            objCopy.Contragent = scv.Contragent;

            //objCopy.WorkPlanVersions.AddRange(scv.WorkPlanVersions);
            //objCopy.ContractVersionPartys.AddRange(scv.ContractVersionPartys);

            return (IVersionSupport)objCopy;
        }

        public override void SetReferences(CreatetVersionHelper CopyObjectHelper) {
            //this._ContractWork = (ContractWork)CopyObjectHelper.GetCopyObject(_ContractWork);

            this._Customer = (ContractParty)CopyObjectHelper.GetCopyObject(_Customer);
            this._Contragent = (ContractParty)CopyObjectHelper.GetCopyObject(_Contragent);

            //this._LinkToEditor = (IVersionSupport)CopyObjectHelper.GetCopyObject(_LinkToEditor);
            this._PaymentPlan = (PaymentPlan)CopyObjectHelper.GetCopyObject(_PaymentPlan);
            this._DeliveryPlan = (DeliveryPlan)CopyObjectHelper.GetCopyObject(_DeliveryPlan);

        }

        public override void SetReferences(CreatetVersionHelper CopyObjectHelper, VersionStates VersionState) {
//            this._ContractWork = (ContractWork)CopyObjectHelper.GetCopyObject(_ContractWork);
//            this._ContractWork.VersionState = VersionState;


            this._Customer = (ContractParty)CopyObjectHelper.GetCopyObject(_Customer);
            this._Customer.VersionState = VersionState;
            
            this._Contragent = (ContractParty)CopyObjectHelper.GetCopyObject(_Contragent);
            this._Contragent.VersionState = VersionState;

            this._PaymentPlan = (PaymentPlan)CopyObjectHelper.GetCopyObject(_PaymentPlan);
            this._PaymentPlan.VersionState = VersionState;

            this._DeliveryPlan = (DeliveryPlan)CopyObjectHelper.GetCopyObject(_DeliveryPlan);
            this._DeliveryPlan.VersionState = VersionState;

            //this._LinkToEditor = (IVersionSupport)CopyObjectHelper.GetCopyObject(_LinkToEditor);
            //this._LinkToEditor.VersionState = VersionState;

            this.VersionState = VersionState;
        }

        #endregion


        #region ОБРАБОТКА СИТУАЦИИ С ТЕКУЩЕЙ ЗАПИСЬЮ

        public override void SetVersionAsCurrent() {
            XPCollection<SimpleContractVersion> currentVers = new XPCollection<SimpleContractVersion>(this.Session); //new Session());  //, criteria, sortProps);
            if (!currentVers.IsLoaded) currentVers.Load();

            // Выбираем все записи SimpleContractVersion, у которых
            // Current == this.Current
            OperandProperty prop = new OperandProperty("Current");
            CriteriaOperator op = prop == (SimpleContractVersion)this.Current;
            currentVers.Filter = op;

            //Session.BeginTransaction();

            //SimpleContractVersion scv = ((SimpleContractVersion)this.Current);
            if (this.VersionState == VersionStates.VERSION_NEW) {
                // Создаётся копия текущей записи, которой (записи, а не копии) предназначено стать CURRENT
                //if (this.VersionState == VersionStates.VERSION_NEW) {
                    this.VersionState = VersionStates.CURRENT;
                    //this.Save();
                //}
                
                
                /*
                // Старый механизм создания версии
                CreatetVersionHelper cov = new CreatetVersionHelper(this.Session, VersionStates.VERSION_CURRENT);
                SimpleContractVersion curObj = (SimpleContractVersion)cov.CopyProcessing(this);
                */

                // Новый механизм создания версии
                SimpleContractVersion curObj = (SimpleContractVersion)CreateNewVersion(this);

                //SimpleContractVersion objCopy = (SimpleContractVersion)CreateCopyObject();
                curObj.VersionState = VersionStates.VERSION_CURRENT;

                //curObj.Current = null;
            } else if (this.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (SimpleContractVersion cont in currentVers) {
                    if (cont.VersionState == VersionStates.VERSION_CURRENT) {
                        cont.VersionState = VersionStates.VERSION_OLD;
                    } else if (cont.VersionState == VersionStates.VERSION_PROJECT) {
                        cont.VersionState = VersionStates.VERSION_DECLINE;
                    }
                }

/*
                // Находим запись со статусом CURRENT
                XPCollection<SimpleContractVersion> currentVersion = new XPCollection<SimpleContractVersion>(this.Session); //new Session());  //, criteria, sortProps);
                if (!currentVersion.IsLoaded) currentVersion.Load();

                // Выбираем все записи SimpleContractVersion, у которых
                // Contract == this.Contract
                OperandProperty propContract = new OperandProperty("Contract"); ;
                CriteriaOperator opContract = propContract == this.Contract;

                //currentVersion.Filter = op;

                //CriteriaOperator opCur = CriteriaOperator.Parse("VersionState = 1");

                CriteriaOperator criteria = new GroupOperator(GroupOperatorType.And,
                    new BinaryOperator(new OperandProperty("VersionState"), 1, BinaryOperatorType.Equal),
                    new NotOperator(new NullOperator("Contract")),
                    opContract
                    );

                currentVersion.Filter = criteria;   // opCur;

                if (currentVersion.Count == 0) {
                    Session.RollbackTransaction();
                    throw new Exception("Object with VersionState = CURRENT not found");   //return;
                }
*/
                SimpleContractVersion cover = (SimpleContractVersion)this.Current;   // currentVers[0];

                //cover.Contract = this.Contract;
                cover.ContractDocument = this.ContractDocument;
                cover.VersionContractDocument = this.VersionContractDocument;
                cover.Description = this.Description;
                cover.VersionedContract = this.VersionedContract;
                cover.Contragent = this.Contragent;
                cover.Customer = this.Customer;

                cover.Contract = this.Contract;
                cover.LinkToEditor = this.LinkToEditor;
                cover.IsOfficial = false;
                cover.IsCurrent = false;

                cover.PrevVersion = this.PrevVersion;
                cover.PaymentPlan = this.PaymentPlan;
                cover.DeliveryPlan = this.DeliveryPlan;

                // Аккуратно меняем состав WorkPlanVersions
                //while (cover.WorkPlanVersions.Count > 0) cover.WorkPlanVersions.Remove(cover.WorkPlanVersions[0]);
                //cover.WorkPlanVersions.AddRange(this.WorkPlanVersions);

                // Список удаляемых
                List<WorkPlanVersion> lwpvDel = new List<WorkPlanVersion>();
                foreach (WorkPlanVersion wpv in cover.WorkPlanVersions) {
                    if (cover.WorkPlanVersions.IndexOf(wpv) == -1) lwpvDel.Add(wpv);
                }
                foreach (WorkPlanVersion wpv in lwpvDel) {
                    cover.WorkPlanVersions.Remove(wpv);
                }

                // Добавляем недостающие 
                foreach (WorkPlanVersion wpv in this.WorkPlanVersions) {
                    if (cover.WorkPlanVersions.IndexOf(wpv) == -1) cover.WorkPlanVersions.Add(wpv);
                }


                // Аккуратно меняем состав ContractPartys
                //while (cover.ContractPartys.Count > 0) cover.ContractPartys.Remove(cover.ContractPartys[0]);
                //cover.ContractPartys.AddRange(this.ContractPartys);

                // Список удаляемых
                List<ContractParty> lcpDel = new List<ContractParty>();
                foreach (ContractParty cp in cover.ContractPartys) {
                    if (cover.ContractPartys.IndexOf(cp) == -1) lcpDel.Add(cp);
                }
                foreach (ContractParty cp in lcpDel) {
                    cover.ContractPartys.Remove(cp);
                }

                // Добавляем недостающие 
                List<ContractParty> lcpAdd = new List<ContractParty>();
                lcpAdd.AddRange(cover.ContractPartys);
                foreach (ContractParty cp in lcpAdd) {
                    cover.ContractPartys.Add(cp);
                }

                //foreach (ContractParty cp in this.ContractPartys) {
                //    if (cover.ContractPartys.IndexOf(cp) == -1) cover.ContractPartys.Add(cp);
                //}

                // Документры просто заменяем
                while (cover.ContractDocuments.Count > 0) cover.ContractDocuments.Remove(cover.ContractDocuments[0]);
                cover.ContractDocuments.AddRange(this.ContractDocuments);

                cover.Save();  // - Крайняя мера
                Session.FlushChanges();  // - Ещё более суровая крайняя мера

                this.VersionState = VersionStates.VERSION_CURRENT;
            }

            //Session.CommitTransaction();
        }

/*
        public override IVersionSupport CreateNewVersion(IVersionSupport sourceObj) {
            
            //////XPCollection<SimpleContractVersion> currentVersion = new XPCollection<SimpleContractVersion>(this.Session); //new Session());  //, criteria, sortProps);
            //////if (!currentVersion.IsLoaded) currentVersion.Load();

            //////// Current == this.Current
            //////OperandProperty prop = new OperandProperty("Current");
            //////CriteriaOperator op = prop == (SimpleContractVersion)this.Current;
            //////currentVersion.Filter = op;

            //////if (currentVersion.Count == 0) {
            //////    Session.RollbackTransaction();
            //////    throw new Exception("Object with VersionState = CURRENT not found");   //return;
            //////}
            


            // Объект в статусе CURRENT
            SimpleContractVersion cover = (SimpleContractVersion)this.Current;
            //if (cover == null) {
            //    throw new Exception("Object with VersionState = CURRENT not found");
            //}

            VersionStates vs = VersionStates.VERSION_PROJECT;

            //Session.BeginTransaction();

            CreatetVersionHelper CopyObjectHelper = new CreatetVersionHelper(this.Session, vs);
            SimpleContractVersion newObj = (SimpleContractVersion)CopyObjectHelper.CopyProcessing(cover, VersionStates.VERSION_PROJECT);

            // В newObj переносим только копии зависимых объектов

            newObj.Contract = cover.Contract;
            newObj.ContractDocument = cover.ContractDocument;
            newObj.VersionContractDocument = cover.VersionContractDocument;
            newObj.Description = cover.Description;
            newObj.VersionedContract = cover.VersionedContract;
            newObj.LinkToEditor = cover.LinkToEditor;

            newObj.PrevVersion = this.PrevVersion;
            newObj.IsOfficial = false;
            newObj.IsCurrent = false;

            newObj.ContractDocuments.AddRange(cover.ContractDocuments);

            while (newObj.WorkPlanVersions.Count > 0) newObj.WorkPlanVersions.Remove(newObj.WorkPlanVersions[0]);
            foreach (WorkPlanVersion wpv in cover.WorkPlanVersions) {
                newObj.WorkPlanVersions.Add((WorkPlanVersion)CopyObjectHelper.GetCopyObject(wpv));
            }

            //while (newObj.ContractPartys.Count > 0) newObj.ContractPartys.Remove(newObj.ContractPartys[0]);
            List<ContractParty> lcp = new List<ContractParty>();
            ContractParty Cust = null;
            ContractParty Contr = null;
            foreach (ContractParty cp in cover.ContractPartys) {
                ContractParty copycp = (ContractParty)CopyObjectHelper.GetCopyObject(cp);
                //newObj.ContractPartys.Add(copycp);
                lcp.Add(copycp);
                //if (cover._Customer == cp) newObj._Customer = copycp;
                //if (cover._Contragent == cp) newObj._Contragent = copycp;
                if (cover._Customer == cp) Cust = copycp;
                if (cover._Contragent == cp) Contr = copycp;
            }
            while (newObj.ContractPartys.Count > 0) newObj.ContractPartys.Remove(newObj.ContractPartys[0]);
            newObj.ContractPartys.AddRange(lcp);
            if (Cust != null) newObj._Customer = Cust;
            if (Contr != null) newObj._Contragent = Contr;

            //newObj._Customer = (ContractParty)CopyObjectHelper.GetCopyObject(cover.Customer);
            //newObj._Contragent = (ContractParty)CopyObjectHelper.GetCopyObject(cover.Contragent);

            newObj._PaymentPlan = (PaymentPlan)CopyObjectHelper.GetCopyObject(cover.PaymentPlan);
            newObj._DeliveryPlan = (DeliveryPlan)CopyObjectHelper.GetCopyObject(cover.DeliveryPlan);

            newObj.VersionState = vs;

            //Session.CommitTransaction();

            return newObj;
        }
*/
        #endregion



        #region КНОПКИ

        /// <summary>
        /// Approve
        /// </summary>
        [Action(ToolTip = "Approving simple contract")]
        public virtual void Approve() {

            //if (DialogResult.OK != MessageBox.Show("Возможны баги! Идти дальше или идти подальше?", "Пост охраны", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)) return;

            // Алгоритм.
            // Если this имеет статус VERSION_NEW, то создаём новый объект со статусом VERSION_CURRENT, который 
            // по остальным параметрам есть копия this. А объекту this назначаем статус CURRENT.
            // Если this имеет статус VERSION_PROJECT, то this становится VERSION_CURRENT (а бывшая VERSION_CURRENT становится 
            // VERSION_OLD) и при этом поля из this переносятся в CURRENT
            this.SetVersionAsCurrent();
        }

        #endregion


    }

}