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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{

    /// <summary>
    /// Класс crmDealWithStage
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Current.ContractDocument.Number", 
        "Current.Customer.Party.Name", 
        "Current.Supplier.Party.Name"
    })]
    //[DefaultClassOptions]
//    [Persistent("crmDealWithStage")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealWithStage : crmContractDeal, IVersionBusinessLogicSupport, IVersionMainObject
    {
        public crmDealWithStage(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();

            this.Current = new crmDealWithStageVersion(this.Session, VersionStates.VERSION_NEW);
            //this.DealVersions.Add(this.Current);
            this.DealVersions.Add(this.Current);

            // SHU 2011-10-26 Закомментил из за ненужности и порождения ошибок
            //this.ContractDocument = new crmContractDocument(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        //private crmDealWithStageVersion _Current;
        //[Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public crmDealWithStageVersion Current {
        //    get { return _Current; }
        //    set { SetPropertyValue<crmDealWithStageVersion>("Current", ref _Current, value); }
        //}

        #endregion


        #region МЕТОДЫ

        #endregion

        #region IVersionBusinessLogicSupport

        public void ApproveVersion(crmDealWithStageVersion scVersion) {
            crmDealWithStageVersion newcur = null;
            foreach (crmDealWithStageVersion cont in this.DealVersions) //DealVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version is not in Version List");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

                crmDealWithStage dealVersionMainObj = (crmDealWithStage)scVersion.MainObject;

                //scVersion.StageStructure.Stages[0].DeliveryPlan.DeliveryUnits[0].DeliveryItems[0]
                // Здесь вставить перебор FinanceDeal и прочее с целью добавления в коллекцию StageMains и ObligationUnitMains
                // В коллекциии scVersion.Stages находятся все этапы из полной их иерархии (т.е. рекурсия не нужна)
                IList<crmStage> stageList = GetStageList(scVersion);
                foreach (crmStage stage in stageList) {
                    if (stage.StageType != StageType.FINANCE) continue;
                    crmStageMain stageMain = scVersion.CreateStageMain(stage);
                    if (dealVersionMainObj.StageMains.IndexOf(stageMain) == -1)
                        dealVersionMainObj.StageMains.Add(stageMain);

                    // Пополнение коллекции ObligationUnitMain (имеются две разновидности: Delivery и Payment)
                    //foreach (crmObligationUnitMain obligationUnitMain in GetFinanceDeliveryList(stage.DeliveryPlan)) {
                    foreach (crmObligationUnitMain obligationUnitMain in scVersion.CreateFinanceDeliveryList(stage.DeliveryPlan)) {
                        if (dealVersionMainObj.ObligationUnitMains.IndexOf(obligationUnitMain) == -1)
                            dealVersionMainObj.ObligationUnitMains.Add(obligationUnitMain);
                    }

                    //foreach (crmObligationUnitMain paymentUnitMain in GetFinancePaymentList(stage.PaymentPlan)) {
                    foreach (crmObligationUnitMain paymentUnitMain in scVersion.CreateFinancePaymentList(stage.PaymentPlan)) {
                        if (dealVersionMainObj.ObligationUnitMains.IndexOf(paymentUnitMain) == -1)
                            dealVersionMainObj.ObligationUnitMains.Add(paymentUnitMain);
                    }
                }

                // Отрабатывание регистров
                RegisterClear(scVersion);
                RegisterOperations(scVersion);

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (crmDealWithStageVersion cont in this.DealVersions) { //DealVersions) {
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

                crmDealWithStage dealVersionMainObj = (crmDealWithStage)scVersion.MainObject;

                // -- Когда происходит Утверждение не новой версии, то старые этапы нельзя удалять, а можно только закрывать
                foreach (crmStageMain stageMaim in dealVersionMainObj.StageMains) {
                    stageMaim.Closed = true;
                    if (stageMaim.DateClosed == null || stageMaim.DateClosed == DateTime.MinValue) stageMaim.DateClosed = System.DateTime.Now;
                }
                foreach (crmObligationUnitMain obligationUnitMain in dealVersionMainObj.ObligationUnitMains) {
                    obligationUnitMain.Closed = true;
                    if (obligationUnitMain.DateClosed == null || obligationUnitMain.DateClosed == DateTime.MinValue) obligationUnitMain.DateClosed = System.DateTime.Now;
                }
                // --

                // Здесь вставить перебор FinanceDeal и прочее с целью добавления в коллекцию StageMains и ObligationUnitMains
                // В коллекции scVersion.Stages находятся все этапы из полной их иерархии (т.е. рекурсия не нужна)
                foreach (crmStage stage in scVersion.Stages) {
                    if (stage.StageType != StageType.FINANCE) continue;

                    // SHU 2011-11-01 Создаём накрывающий объект для данного этапа с типом Finance
                    if (stage.StageMain == null) {
                        stage.StageMain = new crmStageMain(this.Session);
                        stage.StageMain.Current = stage;
                    }

                    // Пополнение коллекции StageMains
                    if (dealVersionMainObj.StageMains.IndexOf(stage.StageMain) == -1)
                        dealVersionMainObj.StageMains.Add(stage.StageMain);

                    // Пополнение коллекции ObligationUnitMain (имеются две разновидности: Delivery и Payment)
                    foreach (crmObligationUnitMain obligationUnitMain in GetFinanceDeliveryList(stage.DeliveryPlan)) {
                        if (dealVersionMainObj.ObligationUnitMains.IndexOf(obligationUnitMain) == -1)
                            dealVersionMainObj.ObligationUnitMains.Add(obligationUnitMain);
                    }

                    foreach (crmObligationUnitMain paymentUnitMain in GetFinancePaymentList(stage.PaymentPlan)) {
                        if (dealVersionMainObj.ObligationUnitMains.IndexOf(paymentUnitMain) == -1)
                            dealVersionMainObj.ObligationUnitMains.Add(paymentUnitMain);
                    }
                }

                // Отрабатывание регистров
                RegisterClear(scVersion);
                RegisterOperations(scVersion);
            }
        }

        /// <summary>
        /// Список всех этапов
        /// </summary>
        /// <param name="scVersion"></param>
        /// <returns></returns>
        private IList<crmStage> GetStageList(crmDealWithStageVersion scVersion) {
            IList<crmStage> res = new List<crmStage>();

            foreach (crmStage stage in scVersion.Stages) {
                if (res.IndexOf(stage) == -1) res.Add(stage);
                IList<crmStage> res1 = GetStageList(stage);
                //if (res1.Count == 0) break;
                foreach (crmStage stage1 in res1) if (res1.IndexOf(stage) == -1) res.Add(stage1);
            }

            return res;
        }
        private IList<crmStage> GetStageList(crmStage prmStage) {
            IList<crmStage> res = new List<crmStage>();

            foreach (crmStage stage in prmStage.SubStages) {
                if (res.IndexOf(stage) == -1) res.Add(stage);
                IList<crmStage> res1 = GetStageList(stage);
                //if (res1.Count == 0) break;
                foreach (crmStage stage1 in res1) if (res1.IndexOf(stage) == -1) res.Add(stage1);
            }

            return res;
        }

        /// <summary>
        /// Список главных объектов для этапов, имеющих тип StageType.FINANCE в случае рекурсивного доступа.
        /// </summary>
        /// <param name="prmStage"></param>
        /// <returns></returns>
        private IList<crmStageMain> GetFinanceStageList(crmStage prmStage) {
            IList<crmStageMain> res = new List<crmStageMain>();

            foreach (crmStage stage in prmStage.SubStages) {
                if (stage.StageType != StageType.FINANCE) continue;
                if (stage.StageMain == null) continue;
                if (!res.Contains(stage.StageMain)) res.Add(stage.StageMain);
                IList<crmStageMain> res1 = GetFinanceStageList(stage);
                foreach (crmStageMain stageMain in res1) if (!res.Contains(stageMain)) res.Add(stageMain);
            }

            return res;
        }

        /// <summary>
        /// Список главных объектов для обязательств доставки этапа, имеющего тип StageType.FINANCE
        /// </summary>
        /// <param name="prmStage"></param>
        /// <returns></returns>
        private IList<crmObligationUnitMain> GetFinanceDeliveryList(crmDeliveryPlan prmDeliveryPlan) {
            IList<crmObligationUnitMain> res = new List<crmObligationUnitMain>();
            foreach (crmObligationUnit deliveryUnit in prmDeliveryPlan.DeliveryUnits) {
                if (!res.Contains(deliveryUnit.ObligationUnitMain)) res.Add(deliveryUnit.ObligationUnitMain);
            }
            return res;
        }

        /// <summary>
        /// Список главных объектов для обязательств оплаты этапа, имеющего тип StageType.FINANCE
        /// </summary>
        /// <param name="prmStage"></param>
        /// <returns></returns>
        private IList<crmObligationUnitMain> GetFinancePaymentList(crmPaymentPlan prmPaymentPlan) {
            IList<crmObligationUnitMain> res = new List<crmObligationUnitMain>();
            foreach (crmObligationUnit paymentUnit in prmPaymentPlan.PaymentUnits) {
                if (!res.Contains(paymentUnit.ObligationUnitMain)) res.Add(paymentUnit.GetObligationUnitMain());
            }
            return res;
        }


        #region РАБОТА С РЕГИСТРАМИ

        protected virtual void RegisterOperations(crmDealWithStageVersion scVersion) {
            if (scVersion == null) return;

            // Добавляем запись в crmPlaneFactRegister для scVersion
            AddPFRegisterRecords(scVersion);

            // Добавляем запись в crmDebtorCreditorDebtRegister для scVersion
            AddDCDRegisterRecords(scVersion);

            // Добавляем запись в crmCashFlowRegister для scVersion
            AddCFRegisterRecords(scVersion);
        }

        private bool IsChildOfFinanceStage(crmStage stage) {
            bool res = false;
            crmStage stg = stage;
            if (stg.StageType == StageType.FINANCE) return true;

            while (stg.TopStage != null) {
                if (stg.TopStage.StageType == StageType.FINANCE) return true;
                //if (stg == stg.TopStage) return false;
                stg = stg.TopStage;
            }
            
            return res;
        }


        /// <summary>
        /// Создание записи регистра План-Факт
        /// </summary>
        /// <param name="scVersion"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        private crmPlaneFactRegister CreatePlaneFactCommonRegister(Session ssn, crmDealWithStageVersion scVersion, crmStage stage, crmObligationTransfer obligationItem) {
            crmPlaneFactRegister pfr = new crmPlaneFactRegister(ssn);

            // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
            Guid token = ((crmDealWithStage)scVersion.MainObject).Oid;

            pfr.Token = token;

            pfr.Contract = scVersion.ContractDeal.Contract;
            pfr.ContractDeal = scVersion.ContractDeal;
            pfr.CostItem = scVersion.CostItem;
            pfr.Creditor = scVersion.Customer.Party;
            pfr.Debitor = scVersion.Supplier.Party;
            pfr.fmOrder = scVersion.Order;
            pfr.Subject = (scVersion.Order != null) ? scVersion.Order.Subject : null;
            pfr.PlaneFact = PlaneFact.PLAN;
            pfr.Stage = stage;
            
            // ??? pfr.CostInRUR = scVersion.;

            //pfr.FinancialDeal - заменяется на финансовый этап
            //!Паша пока без финансовых сделок
            //            if (scVersion.FinancialDeal != null) {
            //                pfr.FinancialDeal = di.FinancialDeal.FinancialDeal;
            //            }

            //pfr.Nomenclature = scVersion.DeliveryUnit.DeliveryItems;   // SHU!!! Не ясно, откуда брать номенклатуру
            //pfr.FinancialDeal = scVersion.FinancialDeal.FinancialDeal;

            //pfr.MeasureUnit = scVersion.Order;
            //pfr.Volume = scVersion.;

            //pfr.Valuta = scVersion.Valuta;
            //pfr.Cost = scVersion.Price;
            //pfr.CostInRUR = scVersion.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

            pfr.Nomenclature = obligationItem.Nomenclature;

            crmDeliveryItem deliveryItem = obligationItem as crmDeliveryItem;
            if (deliveryItem != null) {
                pfr.MeasureUnit = deliveryItem.CountUnit;
                pfr.Volume = deliveryItem.CountValue;

                pfr.Valuta = deliveryItem.Valuta;
                pfr.Cost = deliveryItem.Price;
             }

            crmPaymentItem paymentItem = obligationItem as crmPaymentItem;
            if (paymentItem != null) {
                pfr.MeasureUnit = null;
                pfr.Volume = 0;

                pfr.Valuta = paymentItem.Valuta;
                pfr.Cost = paymentItem.SummFull;
            }

            return pfr;
        }

        private void AddPFRegisterRecords(crmDealWithStageVersion scVersion) {
            if (scVersion == null) return;

            /* !!! Временно отключаем проверку !!!
            // Примечание. В этом регистр пишутся только контролируемые организации
            // Проверяем, что организация контролируемая. Это значит, что для её crmPartyRu найдётся какой-либо объект UserParty
            OperandProperty prop = new OperandProperty("Party");
            CriteriaOperator op = prop == scVersion.;
            XPCollection<crmUserParty> userPartyCol = new XPCollection<crmUserParty>(this.Session, op, null);
            userPartyCol.Criteria = op;
            userPartyCol.Reload();
            if (!userPartyCol.IsLoaded) userPartyCol.Load();

            if (userPartyCol.Count == 0) return; 
            */

            using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {
                crmDealWithStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithStageVersion>(scVersion.Oid);

                // Цикл по финансовым этапам и их техническим подэтапам
                foreach (crmStage stage in GetStageList(scVersion1)) {
                    if (!IsChildOfFinanceStage(stage)) continue;

                    foreach (crmDeliveryUnit du in stage.DeliveryPlan.DeliveryUnits) {
                        foreach (crmDeliveryItem di in du.DeliveryItems) {
                            CreatePlaneFactCommonRegister(uow, scVersion1, stage, di);
                        }
                    }

                    foreach (crmPaymentUnit pu in stage.PaymentPlan.PaymentUnits) {
                        foreach (crmPaymentItem pi in pu.PaymentItems) {
                            CreatePlaneFactCommonRegister(uow, scVersion1, stage, pi);
                        }
                    }
                }
                uow.CommitChanges();
            }

        }

        private void AddDCDRegisterRecords(crmDealWithStageVersion scVersion) {
            if (scVersion == null) return;

            // Примечание. В этом регистр пишутся все организации, а не только контролируемые

            // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
            Guid token = ((crmDealWithStage)scVersion.MainObject).Oid;

            using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {

                crmDealWithStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithStageVersion>(scVersion.Oid);

                // Заполняем две записи. Вторая является инвертированной к первой по отношению Первичная сторона - Вторичная сторона

                foreach (crmStage stage in GetStageList(scVersion1)) {
                    if (!IsChildOfFinanceStage(stage)) continue;

                    // Список обязательств
                    IList<crmObligationUnit> obligationUnits = new List<crmObligationUnit>();
                    foreach (crmDeliveryUnit du in stage.DeliveryPlan.DeliveryUnits) {
                        if (obligationUnits.IndexOf(du) == -1) obligationUnits.Add(du);
                    }
                    foreach (crmPaymentUnit pu in stage.PaymentPlan.PaymentUnits) {
                        if (obligationUnits.IndexOf(pu) == -1) obligationUnits.Add(pu);
                    }

                    // Обход всех обязательств этапа DeliveryItem и PaymentItem
                    foreach (crmObligationUnit du in obligationUnits) {
                        //foreach (crmDeliveryItem di in du.DeliveryItems) {
                            // Первая запись
                            crmDebtorCreditorDebtRegister dcdr1 = new crmDebtorCreditorDebtRegister(uow);

                            dcdr1.Token = token;

                            if (scVersion1.ContractDeal != null) {
                                dcdr1.Contract = scVersion1.ContractDeal.Contract;
                            }
                            dcdr1.ContractDeal = scVersion1.ContractDeal;
                            dcdr1.CostItem = scVersion1.CostItem;
                            dcdr1.PrimaryParty = scVersion1.Customer.Party;
                            dcdr1.ContragentParty = scVersion1.Supplier.Party;
                            dcdr1.fmOrder = scVersion1.Order;
                            dcdr1.Subject = (scVersion1.Order != null) ? scVersion1.Order.Subject : null;
                            dcdr1.PlaneFact = PlaneFact.PLAN;

                            dcdr1.Stage = stage;
                            dcdr1.ObligationUnit = du;
                            dcdr1.ObligationUnitDateTime = du.DatePlane;

                            //dcdr2.StageTech = ;
                            //dcdr2.StageTech2 = ;

                            //dcdr.Nomenclature = scVersion1.;
                            //if (scVersion1.FinancialDeal != null) {
                            //    dcdr1.FinancialDeal = scVersion1.FinancialDeal.FinancialDeal;
                            //}

                            //dcdr1.MeasureUnit = scVersion1.Order;
                            //dcdr1.Volume = scVersion1.;

                            //---
                            dcdr1.DebitValuta = scVersion1.Valuta;
                            dcdr1.DebitCost = scVersion1.Price;
                            //dcdr1.DebitCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //---
                            dcdr1.CreditValuta = scVersion1.Valuta;  //null;
                            dcdr1.CreditCost = 0;
                            //dcdr1.CreditCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //---
                            //dcdr1.BalanceValuta = scVersion1.Valuta;
                            //dcdr1.BalanceCost = dcdr1.DebitCost - dcdr1.CreditCost;
                            
                            // ??? dcdr1.BalanceCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //dcdr1.Save();


                            // Вторая запись
                            crmDebtorCreditorDebtRegister dcdr2 =  new crmDebtorCreditorDebtRegister(uow);

                            dcdr2.Token = token;

                            if (scVersion1.ContractDeal != null) {
                                dcdr2.Contract = scVersion1.ContractDeal.Contract;
                            }
                            dcdr2.ContractDeal = scVersion1.ContractDeal;
                            dcdr2.CostItem = scVersion1.CostItem;
                            dcdr2.PrimaryParty = scVersion1.Supplier.Party;
                            dcdr2.ContragentParty = scVersion1.Customer.Party;
                            dcdr2.fmOrder = scVersion1.Order;
                            dcdr2.Subject = (scVersion1.Order != null) ? scVersion1.Order.Subject : null;
                            dcdr2.PlaneFact = PlaneFact.PLAN;

                            dcdr2.Stage = stage;
                            dcdr2.ObligationUnit = du;
                            dcdr2.ObligationUnitDateTime = du.DatePlane;

                            //dcdr2.StageTech = ;
                            //dcdr2.StageTech2 = ;

                            //if (scVersion1.FinancialDeal != null) {
                            //    dcdr2.FinancialDeal = scVersion1.FinancialDeal.FinancialDeal;
                            //}

                            //---
                            dcdr2.DebitValuta = scVersion1.Valuta;    //null;
                            dcdr2.DebitCost = 0;
                            //dcdr2.DebitCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //---
                            dcdr2.CreditValuta = scVersion1.Valuta;
                            dcdr2.CreditCost = scVersion1.Price;
                            //dcdr2.CreditCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //---
                            //dcdr2.BalanceValuta = scVersion1.Valuta;
                            //dcdr2.BalanceCost = dcdr2.DebitCost - dcdr2.CreditCost;
                            
                            // ??? dcdr.BalanceCostInRUR = scVersion1.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

                            //dcdr2.Save();
                        //}
                    }
                }
                uow.CommitChanges();
            }
        }

        private void AddCFRegisterRecords(crmDealWithStageVersion scVersion) {
            if (scVersion == null) return;

            /* !!! Временно отключаем проверку !!!
            // Примечание. В этом регистр пишутся только контролируемые организации
            // Проверяем, что организация контролируемая. Это значит, что для её crmPartyRu найдётся какой-либо объект UserParty
            OperandProperty prop = new OperandProperty("Party");
            CriteriaOperator op = prop == scVersion.;
            XPCollection<crmUserParty> userPartyCol = new XPCollection<crmUserParty>(this.Session, op, null);
            userPartyCol.Criteria = op;
            userPartyCol.Reload();
            if (!userPartyCol.IsLoaded) userPartyCol.Load();

            if (userPartyCol.Count == 0) return; 
            */

            // Цикл по финансовым этапам и их техническим подэтапам
            using (UnitOfWork uow = this.Session.BeginNestedUnitOfWork()) {

                crmDealWithStageVersion scVersion1 = uow.GetObjectByKey<crmDealWithStageVersion>(scVersion.Oid);

                foreach (crmStage stage in GetStageList(scVersion1)) {
                    if (!IsChildOfFinanceStage(stage)) continue;

                    // Список обязательств
                    IList<crmObligationUnit> obligationUnits = new List<crmObligationUnit>();
                    //foreach (crmDeliveryUnit du in stage.DeliveryPlan.DeliveryUnits) {
                    //    if (obligationUnits.IndexOf(du) == -1) obligationUnits.Add(du);
                    //}
                    foreach (crmPaymentUnit pu in stage.PaymentPlan.PaymentUnits) {
                        if (obligationUnits.IndexOf(pu) == -1) obligationUnits.Add(pu);
                    }

                    // Обход всех обязательств этапа DeliveryItem и PaymentItem
                    foreach (crmPaymentUnit pu in obligationUnits) {
                        foreach (crmPaymentItem pi in pu.PaymentItems) {
                            // Примечание 2. Этот регистр отслеживает только финансовые платёжные обязательства PaymentMoney
                            if (pi as crmPaymentMoney == null) continue;
                            CreateCFRegister(uow, scVersion1, stage, pi);
                        }
                    }
                }
                uow.CommitChanges();
            }
        }
            
        private crmCashFlowRegister CreateCFRegister(Session ssn, crmDealWithStageVersion scVersion, crmStage stage, crmPaymentItem pi) {
            crmCashFlowRegister cfr = new crmCashFlowRegister(ssn);

            // Используем ((crmDealWithoutStage)scVersion.MainObject).Oid как token (метку для разспознавания набора связанных записей)
            Guid token = ((crmDealWithStage)scVersion.MainObject).Oid;

            cfr.Token = token;

            if (scVersion.ContractDeal != null) {
                cfr.Contract = scVersion.ContractDeal.Contract;
            }
            cfr.ContractDeal = scVersion.ContractDeal;
            cfr.CostItem = pi.CostItem;
            cfr.PrimaryParty = scVersion.Customer.Party;
            cfr.ContragentParty = scVersion.Supplier.Party;
            cfr.fmOrder = pi.Order;
            cfr.Subject = (pi.Order != null) ? pi.Order.Subject : null;
            //cfr.PlaneFact = PlaneFact.PLAN;
            cfr.Section = CashFlowRegisterSection.CONTRACT_PLAN;

            //cfr.Stage = stage;
            cfr.PaymentItem = pi;
            cfr.ObligationUnit = pi.PaymentUnit;
            cfr.ObligationUnitDateTime = pi.PaymentUnit.DatePlane;
            
            //cfr.StageTech = ;
            //cfr.StageTech2 = ;

            //!Паша пока без финансовый сделок
            //if (scVersion.FinancialDeal != null) {
            //    cfr.FinancialDeal = pi.FinancialDeal.FinancialDeal;
            //}

            cfr.Valuta = pi.Valuta;
            cfr.Cost = pi.SummFull;
            //cfr.CostInRUR = scVersion.Price;  // Надо вычислять с учётом прайса, курса по прайсу и т.п.

            cfr.PaymentCost = pi.AccountSumma;
            cfr.PaymentValuta = pi.AccountValuta;

            return cfr;
        }

        #endregion

        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        }

        public void Approve(IVersionSupport obj) {
            crmDealWithStageVersion approvingObj = obj as crmDealWithStageVersion;
            if (approvingObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("Документ не может быть утверждён");
                return;
            }
            ApproveVersion(approvingObj);
        }

        #endregion
/*
        #region IContractFactory Members

        BaseObject IContractDealFactory.Create(crmCommonRegistrationForm frm) {
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
            //((crmDealWithoutStageVersion)this.Current).DealCode = frm.ContractDocument.Number;
            this.Category = frm.Category;

            //this.Category
            //this.Contract
            //this.Current
            //this.DateRegistration
            //this.DealVersions вывел из проекта с заменой на DealWithStageVersions
            //this.DealWithStageVersions
            //this.State


            // Для сохранения данных о Служебной записке (не понятно, что у неё такое DocumentCategory)
            //this.ContractDocument = frm.ContractDocument;
//            this.ContractDocument.Date = frm.Date;
//            this.ContractDocument.Number = frm.Number;
//            this.ContractDocument.DocumentCategory = frm.DocumentCategory;
            this.Current.Category = frm.Category;

            // Данные о контрактном документе, которым оформлен данный проект договора (ведомости)
            //this.Current.ContractDocument.Delete();
            //this.Current.ContractDocument = frm.ContractDocument;
//            this.Current.ContractDocument.Date = frm.Date;
//            this.Current.ContractDocument.Number = frm.Number;
//            this.Current.ContractDocument.DocumentCategory = frm.DocumentCategory;

            this.Current.Price = frm.Price;
            this.Current.Valuta = frm.Valuta;

            if (frm.OurRole == PartyRole.CUSTOMER) {
                this.Current.Customer.Party = frm.OurParty;
                this.Current.Supplier.Party = frm.PartnerParty;
            } else {
                this.Current.Supplier.Party = frm.OurParty;
                this.Current.Customer.Party = frm.PartnerParty;
            }

            crmDealWithStageVersion dws = this.Current as crmDealWithStageVersion;
            if (dws != null) {
                dws.StageStructure.FirstStage.DateBegin = frm.DateBegin;
                dws.StageStructure.FirstStage.DateEnd = frm.DateEnd;
                //dws.StageStructure.FirstStage.Code = "ВЕД" + frm.ContractDocument.Number;
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
*/

        #region IVersionMainObject

        public VersionRecord GetCurrent() {
            return (VersionRecord)this.Current;
        }

        #endregion

    }
}