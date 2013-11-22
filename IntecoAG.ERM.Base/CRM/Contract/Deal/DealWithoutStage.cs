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

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Party;
using DevExpress.Data.Filtering;

namespace IntecoAG.ERM.CRM.Contract.Deal
{

    /// <summary>
    /// Класс crmDealWithoutStage
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Current.ContractDocument.Number", 
        "Current.Customer.Party.Name", 
        "Current.Supplier.Party.Name"
    })]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealWithoutStage : crmDealWithStage, IVersionBusinessLogicSupport, IVersionMainObject
    {
        public crmDealWithoutStage(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();

            this.Current = new crmDealWithoutStageVersion(this.Session, VersionStates.VERSION_NEW);
            //this.DealVersions.Add(this.Current);
            this.DealVersions.Add(this.Current);
            //this.ContractDocument = new crmContractDocument(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        #endregion


        //#region IVersionBusinessLogicSupport

        //// Также при утверждении пишется информация в регистры

        //public void ApproveVersion(crmDealWithoutStageVersion scVersion) {
        //    crmDealWithoutStageVersion newcur = null;
        //    foreach (crmDealWithoutStageVersion cont in this.DealVersions) //DealVersions)
        //        if (cont == scVersion) newcur = cont;
        //    if (newcur == null) throw new Exception("Version not in VersionList");
        //    VersionHelper vHelper;

        //    if (scVersion.VersionState == VersionStates.VERSION_NEW) {
        //        vHelper = new VersionHelper(this.Session);
        //        vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

        //        crmDealWithoutStage dealVersionMainObj = (crmDealWithoutStage)scVersion.MainObject;

        //        //scVersion.StageStructure.Stages[0].DeliveryPlan.DeliveryUnits[0].DeliveryItems[0]
        //        // Здесь вставить перебор FinanceDeal и прочее с целью добавления в коллекцию StageMains и ObligationUnitMains
        //        // В коллекциии scVersion.Stages находятся все этапы из полной их иерархии (т.е. рекурсия не нужна)

        //        // Пополнение коллекции ObligationUnitMain (имеются две разновидности: Delivery и Payment)
        //        //foreach (crmObligationUnitMain obligationUnitMain in GetFinanceDeliveryList(stage.DeliveryPlan)) {
        //        foreach (crmObligationUnitMain obligationUnitMain in scVersion.CreateFinanceDeliveryList(scVersion.DeliveryPlan)) {
        //            if (dealVersionMainObj.ObligationUnitMains.IndexOf(obligationUnitMain) == -1)
        //                dealVersionMainObj.ObligationUnitMains.Add(obligationUnitMain);
        //        }

        //        //foreach (crmObligationUnitMain paymentUnitMain in GetFinancePaymentList(stage.PaymentPlan)) {
        //        foreach (crmObligationUnitMain paymentUnitMain in scVersion.CreateFinancePaymentList(scVersion.PaymentPlan)) {
        //            if (dealVersionMainObj.ObligationUnitMains.IndexOf(paymentUnitMain) == -1)
        //                dealVersionMainObj.ObligationUnitMains.Add(paymentUnitMain);
        //        }

        //        // Отрабатывание регистров
        //        RegisterClear(scVersion);
        //        RegisterOperations(scVersion);

        //    } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

        //        foreach (crmDealWithoutStageVersion cont in this.DealVersions) { //DealVersions) {
        //            if (cont == scVersion) continue;
        //            if (cont.VersionState == VersionStates.VERSION_CURRENT) {
        //                vHelper = new VersionHelper(this.Session);
        //                vHelper.SetVersionStateExt(cont, VersionStates.VERSION_OLD);
        //            } else if (cont.VersionState == VersionStates.VERSION_PROJECT) {
        //                vHelper = new VersionHelper(this.Session);
        //                vHelper.SetVersionStateExt(cont, VersionStates.VERSION_DECLINE);
        //            }
        //        }

        //        vHelper = new VersionHelper(this.Session);
        //        vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

        //        this.Current = scVersion;

        //        crmDealWithoutStage dealVersionMainObj = (crmDealWithoutStage)scVersion.MainObject;

        //        // -- Когда происходит Утверждение не новой версии, то старые этипы нельзя удалять, а можно только закрывать
        //        foreach (crmObligationUnitMain obligationUnitMain in dealVersionMainObj.ObligationUnitMains) {
        //            obligationUnitMain.Closed = true;
        //            if (obligationUnitMain.DateClosed == null || obligationUnitMain.DateClosed == DateTime.MinValue) obligationUnitMain.DateClosed = System.DateTime.Now;
        //        }
        //        // --

        //        // Отрабатывание регистров
        //        RegisterClear(scVersion);
        //        RegisterOperations(scVersion);
        //    }
        //}

        //#region РАБОТА С РЕГИСТРАМИ

        //protected virtual void RegisterOperations(crmDealWithoutStageVersion scVersion) {
        //    if (scVersion == null) return;

        //    // Добавляем запись в crmPlaneFactRegister для scVersion
        //    AddPFRegisterRecords(scVersion);

        //    // Добавляем запись в crmDebtorCreditorDebtRegister для scVersion
        //    AddDCDRegisterRecords(scVersion);

        //    // Добавляем запись в crmCashFlowRegister для scVersion
        //    AddCFRegisterRecords(scVersion);
        //}

        //#region РЕГИСТР ПЛАН-ФАКТ. ЗАПОЛНЕНИЕ.

        //private void AddPFRegisterRecords(crmDealWithoutStageVersion scVersion) {
        //    if (scVersion == null) return;


        //    /* !!! Временно отключаем проверку !!!
        //    // Примечание. В этом регистр пишутся только контролируемые организации
        //    // Проверяем, что организация контролируемая. Это значит, что для её crmPartyRu найдётся какой-либо объект UserParty
        //    OperandProperty prop = new OperandProperty("Party");
        //    CriteriaOperator op = prop == scVersion.;
        //    XPCollection<crmUserParty> userPartyCol = new XPCollection<crmUserParty>(this.Session, op, null);
        //    userPartyCol.Criteria = op;
        //    userPartyCol.Reload();
        //    if (!userPartyCol.IsLoaded) userPartyCol.Load();

        //    if (userPartyCol.Count == 0) return; 
        //    */

        //    //// Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //    //Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //    //XPCollection<crmPlaneFactRegister> pfrColl = new XPCollection<crmPlaneFactRegister>(this.Session);
        //    //pfrColl.LoadingEnabled = false;

        //    using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {

        //        crmDealWithoutStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithoutStageVersion>(scVersion.Oid);

        //        // Поставки отслеживаются в регистре
        //        if (scVersion1.DeliveryPlan != null) {
        //            foreach (crmDeliveryUnit du in scVersion1.DeliveryPlan.DeliveryUnits) {
        //                //if (du != null) continue;
        //                foreach (crmDeliveryItem di in du.DeliveryItems) {
        //                    //pfrColl.Add(CreatePlaneFactRegister(uow, scVersion, di));
        //                    CreatePlaneFactRegister(uow, scVersion1, di);
        //                }
        //            }
        //        }

        //        // Платежи отслеживаются в регистре
        //        if (scVersion1.PaymentPlan != null) {
        //            foreach (crmPaymentUnit pu in scVersion1.PaymentPlan.PaymentUnits) {
        //                //if (pu != null) continue;
        //                foreach (crmPaymentItem pi in pu.PaymentItems) {
        //                    //pfrColl.Add(CreatePlaneFactRegister(uow, scVersion, pi));
        //                    CreatePlaneFactRegister(uow, scVersion1, pi);
        //                }
        //            }
        //        }

        //        uow.CommitChanges();
        //    }

        //}

        ///// <summary>
        ///// Создание записи регистра План-Факт
        ///// </summary>
        ///// <param name="scVersion"></param>
        ///// <param name="di"></param>
        ///// <returns></returns>
        //private crmPlaneFactRegister CreatePlaneFactRegister(Session ssn, crmDealWithoutStageVersion scVersion, crmDeliveryItem di) {
        //    crmPlaneFactRegister pfr = new crmPlaneFactRegister(ssn);

        //    // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //    Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //    pfr.Token = token;

        //    if (scVersion.ContractDeal != null) {
        //        pfr.Contract = scVersion.ContractDeal.Contract;
        //    }
        //    pfr.ContractDeal = scVersion.ContractDeal;
        //    pfr.CostItem = di.CostItem;
        //    pfr.Creditor = scVersion.Customer.Party;
        //    pfr.Debitor = scVersion.Supplier.Party;
        //    pfr.fmOrder = di.Order;
        //    pfr.Subject = (di.Order != null) ? di.Order.Subject : null;
        //    pfr.PlaneFact = PlaneFact.PLAN;

        //    //!Паша пока без финансовых сделок
        //    //            if (scVersion.FinancialDeal != null) {
        //    //                pfr.FinancialDeal = di.FinancialDeal.FinancialDeal;
        //    //            }

        //    pfr.Valuta = di.Valuta;
        //    pfr.Cost = di.Price;
        //    //pfr.CostInRUR = di.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //    pfr.Nomenclature = di.Nomenclature;
        //    pfr.MeasureUnit = di.CountUnit;
        //    pfr.Volume = di.CountValue;

        //    return pfr;
        //}

        ///// <summary>
        ///// Создание записи регистра План-Факт
        ///// </summary>
        ///// <param name="scVersion"></param>
        ///// <param name="di"></param>
        ///// <returns></returns>
        //private crmPlaneFactRegister CreatePlaneFactRegister(Session ssn, crmDealWithoutStageVersion scVersion, crmPaymentItem pi) {
        //    crmPlaneFactRegister pfr = new crmPlaneFactRegister(ssn);

        //    // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //    Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //    pfr.Token = token;

        //    if (scVersion.ContractDeal != null) {
        //        pfr.Contract = scVersion.ContractDeal.Contract;
        //    }
        //    pfr.ContractDeal = scVersion.ContractDeal;
        //    pfr.CostItem = pi.CostItem;
        //    pfr.Creditor = scVersion.Customer.Party;
        //    pfr.Debitor = scVersion.Supplier.Party;
        //    pfr.fmOrder = pi.Order;
        //    pfr.Subject = (pi.Order != null) ? pi.Order.Subject : null;
        //    pfr.PlaneFact = PlaneFact.PLAN;

        //    //!Паша пока без финансовых сделок
        //    //            if (scVersion.FinancialDeal != null) {
        //    //                pfr.FinancialDeal = di.FinancialDeal.FinancialDeal;
        //    //            }

        //    pfr.Valuta = pi.Valuta;
        //    pfr.Cost = pi.SummFull;
        //    //pfr.CostInRUR = di.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //    pfr.Nomenclature = pi.Nomenclature;

        //    return pfr;
        //}

        //#endregion



        //private void AddDCDRegisterRecords(crmDealWithoutStageVersion scVersion) {
        //    if (scVersion == null) return;

        //    // Примечание. В этом регистр пишутся все организации, а не только контролируемые

        //    // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //    Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //    using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {

        //        crmDealWithoutStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithoutStageVersion>(scVersion.Oid);

        //        // Заполняем две записи. Вторая является инвертированной к первой по отношению Первичная сторона - Вторичная сторона

            
        //        // Первая запись
        //        crmDebtorCreditorDebtRegister dcdr1 = new crmDebtorCreditorDebtRegister(uow);

        //        dcdr1.Token = token;

        //        if (scVersion1.ContractDeal != null) {
        //            dcdr1.Contract = scVersion1.ContractDeal.Contract;
        //        }
        //        dcdr1.ContractDeal = scVersion1.ContractDeal;
        //        dcdr1.CostItem = scVersion1.CostItem;
        //        dcdr1.PrimaryParty = scVersion1.Customer.Party;
        //        dcdr1.ContragentParty = scVersion1.Supplier.Party;
        //        dcdr1.fmOrder = scVersion1.Order;
        //        dcdr1.Subject = (scVersion1.Order != null) ? scVersion1.Order.Subject : null;
        //        dcdr1.PlaneFact = PlaneFact.PLAN;

        //        //dcdr.Nomenclature = scVersion1.;
        //        if (scVersion1.FinancialDeal != null) {
        //            dcdr1.FinancialDeal = scVersion1.FinancialDeal.FinancialDeal;
        //        }

        //        //dcdr1.MeasureUnit = scVersion1.Order;
        //        //dcdr1.Volume = scVersion1.;

        //        //---
        //        dcdr1.DebitValuta = scVersion1.Valuta;
        //        dcdr1.DebitCost = scVersion1.Price;
        //        //dcdr1.DebitCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //---
        //        dcdr1.CreditValuta = null;
        //        dcdr1.CreditCost = 0;
        //        //dcdr1.CreditCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //---
        //        //dcdr1.BalanceValuta = scVersion1.Valuta;
        //        //dcdr1.BalanceCost = dcdr1.DebitCost - dcdr1.CreditCost;
        //        //dcdr1.BalanceCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //dcdr1.Save();


        //        // Вторая запись
        //        crmDebtorCreditorDebtRegister dcdr2 = new crmDebtorCreditorDebtRegister(uow);

        //        dcdr2.Token = token;

        //        if (scVersion1.ContractDeal != null) {
        //            dcdr2.Contract = scVersion1.ContractDeal.Contract;
        //        }
        //        dcdr2.ContractDeal = scVersion1.ContractDeal;
        //        dcdr2.CostItem = scVersion1.CostItem;
        //        dcdr2.PrimaryParty = scVersion1.Supplier.Party;
        //        dcdr2.ContragentParty = scVersion1.Customer.Party;
        //        dcdr2.fmOrder = scVersion1.Order;
        //        dcdr2.Subject = (scVersion1.Order != null) ? scVersion1.Order.Subject : null;
        //        dcdr2.PlaneFact = PlaneFact.PLAN;

        //        if (scVersion1.FinancialDeal != null) {
        //            dcdr2.FinancialDeal = scVersion1.FinancialDeal.FinancialDeal;
        //        }

        //        //---
        //        dcdr2.DebitValuta = null;
        //        dcdr2.DebitCost = 0;
        //        //dcdr2.DebitCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //---
        //        dcdr2.CreditValuta = scVersion1.Valuta;
        //        dcdr2.CreditCost = scVersion1.Price;
        //        //dcdr2.CreditCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //---
        //        //dcdr2.BalanceValuta = scVersion1.Valuta;
        //        //dcdr2.BalanceCost = dcdr2.DebitCost - dcdr2.CreditCost;
        //        //dcdr.BalanceCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //        //dcdr2.Save();
        //        uow.CommitChanges();
        //    }
        //}


        //#region РЕГИСТР 2.1.4.3	Движение денежных средств. ЗАПОЛНЕНИЕ.

        //private void AddCFRegisterRecords(crmDealWithoutStageVersion scVersion) {
        //    if (scVersion == null) return;

        //    /* !!! Временно отключаем проверку !!!
        //    // Примечание. В этом регистр пишутся только контролируемые организации
        //    // Проверяем, что организация контролируемая. Это значит, что для её crmPartyRu найдётся какой-либо объект UserParty
        //    OperandProperty prop = new OperandProperty("Party");
        //    CriteriaOperator op = prop == scVersion.;
        //    XPCollection<crmUserParty> userPartyCol = new XPCollection<crmUserParty>(this.Session, op, null);
        //    userPartyCol.Criteria = op;
        //    userPartyCol.Reload();
        //    if (!userPartyCol.IsLoaded) userPartyCol.Load();

        //    if (userPartyCol.Count == 0) return; 
        //    */

        //    // Цикл по финансовым этапам и их техническим подэтапам
        //    using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {

        //        crmDealWithoutStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithoutStageVersion>(scVersion.Oid);

        //        //// Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //        //Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //        //XPCollection<crmCashFlowRegister> cfrColl = new XPCollection<crmCashFlowRegister>(this.Session);
        //        //cfrColl.LoadingEnabled = false;

        //        // Пишем записи для каждой поставки
        //        if (scVersion1.PaymentUnits != null) {
        //            foreach (crmPaymentUnit pu in scVersion1.PaymentUnits) {
        //                foreach (crmPaymentItem pi in pu.PaymentItems) {
        //                    //cfrColl.Add(CreateCFRegister(scVersion, pi));
        //                    // Примечание 2. Этот регистр отслеживает только финансовые платёжные обязательства PaymentMoney
        //                    if (pi as crmPaymentMoney == null) continue;
        //                    CreateCFRegister(uow, scVersion1, pi);
        //                }
        //            }
        //            //foreach (crmPaymentItem pi in scVersion.Settlement.PaymentItems) {
        //            //    cfrColl.Add(CreateCFRegister(scVersion, pi));
        //            //}
        //        }

        //        uow.CommitChanges();
        //    }
        //}

        //private crmCashFlowRegister CreateCFRegister(Session ssn, crmDealWithoutStageVersion scVersion, crmPaymentItem pi) {
        //    crmCashFlowRegister cfr = new crmCashFlowRegister(ssn);

        //    // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
        //    Guid token = ((crmDealWithoutStage)scVersion.MainObject).Oid;

        //    cfr.Token = token;

        //    if (scVersion.ContractDeal != null) {
        //        cfr.Contract = scVersion.ContractDeal.Contract;
        //    }
        //    cfr.ContractDeal = scVersion.ContractDeal;
        //    cfr.CostItem = pi.CostItem;
        //    cfr.PrimaryParty = scVersion.Customer.Party;
        //    cfr.ContragentParty = scVersion.Supplier.Party;
        //    cfr.fmOrder = pi.Order;
        //    cfr.Subject = (pi.Order != null) ? pi.Order.Subject : null;
        //    //cfr.PlaneFact = PlaneFact.PLAN;
        //    cfr.Section = CashFlowRegisterSection.CONTRACT_PLAN;
            
        //    //!Паша пока без финансовый сделок
        //    //if (scVersion.FinancialDeal != null) {
        //    //    cfr.FinancialDeal = pi.FinancialDeal.FinancialDeal;
        //    //}

        //    cfr.Valuta = scVersion.Valuta;
        //    cfr.Cost = scVersion.Price;
        //    //cfr.CostInRUR = scVersion.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

        //    cfr.PaymentCost = pi.AccountSumma;
        //    cfr.PaymentValuta = pi.AccountValuta;

        //    return cfr;
        //}

        //#endregion

        //#endregion


        //public IVersionSupport CreateNewVersion() {
        //    VersionHelper vHelper = new VersionHelper(this.Session);
        //    return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        //}

        //public void Approve(IVersionSupport obj) {
        //    crmDealWithoutStageVersion approvingObj = obj as crmDealWithoutStageVersion;
        //    if (approvingObj == null) {
        //        DevExpress.XtraEditors.XtraMessageBox.Show("Документ не может быть утверждён");
        //        return;
        //    }
        //    ApproveVersion(approvingObj);
        //}

        //#endregion


        //#region IVersionMainObject

        //public VersionRecord GetCurrent() {
        //    return (VersionRecord)this.Current;
        //}

        //#endregion
    }
}