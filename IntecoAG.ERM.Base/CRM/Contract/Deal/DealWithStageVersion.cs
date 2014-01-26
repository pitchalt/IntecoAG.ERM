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
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using FileHelpers;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.CRM.Contract.Deal {

    /// <summary>
    /// Класс crmDealWithStageVersion
    /// </summary>
    //[Appearance("crmDealWithStageVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6 OR not isnull(crmComplexContractVersion)", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("crmDealWithStageVersion.GotoMainActionHidden", AppearanceItemType = "Action", Criteria = "not isnull(crmComplexContractVersion)", TargetItems = "GotoMainAction", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("crmDealWithStageVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    //    [Persistent("crmDealWithStageVersion")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealWithStageVersion : crmDealVersion, IVersionSupport, IVersionBusinessLogicSupport, csIImportSupport {
        public crmDealWithStageVersion(Session ses) : base(ses) { }
        public crmDealWithStageVersion(Session session, VersionStates state) : base(session, state) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public override void VersionAfterConstruction() {
            this.StageStructure = new crmStageStructure(this.Session, this.VersionState);
            this.StageStructure.DealVersion = this;
            base.VersionAfterConstruction();
        }

        public void StageStructureCreate() {
            this.StageStructure = new crmStageStructure(this.Session, this.VersionState);
            this.StageStructure.DealVersion = this;
        }
        #region ПОЛЯ КЛАССА

        public override crmContractParty Customer {
            set {
                base.Customer = value;
                if (!IsLoading) {
                    this.StageStructure.Customer = this.Customer;
                }
            }
        }

        public override crmContractParty Supplier {
            set {
                base.Supplier = value;
                if (!IsLoading) {
                    this.StageStructure.Supplier = this.Supplier;
                }
            }
        }
        public override string DealCode {
            set {
                base.DealCode = value;
                if (!IsLoading) {
                    StageStructure.FirstStage.Code = value;
                }
            }
        }

        [PersistentAlias("StageStructure.FirstStage.Code")]
        public StageType StageType {
            get {
                return StageStructure.FirstStage.StageType;
            }
            set {
                StageStructure.FirstStage.StageType = value;
                OnChanged("StageType");
            }
        }

        public override DateTime DateBegin {
            set {
                base.DateBegin = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.DateBegin = value;
                }
            }
        }

        public override DateTime DateEnd {
            set {
                base.DateEnd = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.DateEnd = value;
                }
            }
        }

        public override DateTime DateFinish {
            set {
                base.DateFinish = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.DateFinish = value;
                }
            }
        }

        public override string DescriptionShort {
            set {
                base.DescriptionShort = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.DescriptionShort = value;
                }
            }
        }

        public override string DescriptionLong {
            set {
                base.DescriptionLong = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.DescriptionLong = value;
                }
            }
        }

        public override crmCostModel CostModel {
            set {
                base.CostModel = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.CostModel = value;
                }
            }
        }

        public override IntecoAG.ERM.CS.Nomenclature.csValuta Valuta {
            set {
                IntecoAG.ERM.CS.Nomenclature.csValuta old = base.Valuta;
                base.Valuta = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.Valuta = value;
                    if (this.PaymentValuta == null || this.PaymentValuta == old) {
                        this.PaymentValuta = value;
                    }
                }
            }
        }

        public override IntecoAG.ERM.CS.Nomenclature.csValuta PaymentValuta {
            set {
                base.PaymentValuta = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.PaymentValuta = value;
                }
            }
        }

        public override IntecoAG.ERM.CS.Finance.csNDSRate NDSRate {
            set {
                base.NDSRate = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.NDSRate = value;
                }
            }
        }

        public override IntecoAG.ERM.FM.Order.fmCOrder Order {
            set {
                base.Order = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.Order = value;
                }
            }
        }

        public override IntecoAG.ERM.FM.fmCostItem CostItem {
            set {
                base.CostItem = value;
                if (!IsLoading) {
                    this.StageStructure.FirstStage.CostItem = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("StageStructure.FirstStage.CurrentCost")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmCostCol CurrentCost {
            get { return StageStructure == null? null : StageStructure.FirstStage.CurrentCost; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("StageStructure.FirstStage.CurrentPayment")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmCostCol CurrentPayment {
            get { return StageStructure == null? null : StageStructure.FirstStage.CurrentPayment; }
        }

        [PersistentAlias("StageStructure.FirstStage.SubStages")]
        [Aggregated]
        public XPCollection<crmStage> Stages {
            get { return StageStructure.FirstStage.SubStages; }
        }


        #endregion


        #region СВОЙСТВА КЛАССА

        public override object MainObject {
            get { return this.ContractDeal as crmDealWithStage; }
        }

        public override Boolean IsStaged {
            get { return true; }
        }

        public override Int32 StageCount {
            get { return Stages.Count; }
        }

        public override Int32 StageWithoutOrder {
            get {
                Int32 count = 0;
                foreach (crmStage stage in Stages)
                    if (stage.Order == null)
                        count++;
                return Stages.Count;
            }
        }

        #endregion


        #region МЕТОДЫ

        public void ApproveVersion() {
            if (this.ContractDeal as crmDealWithStage != null) (this.ContractDeal as crmDealWithStage).ApproveVersion(this);
        }

        #endregion


        //#region КНОПКИ

        //public void ApproveVersion() {
        //    ((crmDealWithStage)this.ContractDeal).ApproveVersion(this);
        //}

        //#endregion


        #region IVersionBusinessLogicSupport

        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(((crmDealWithStage)(this.MainObject)).Current), vHelper);

            /*
            IVersionSupport srcObj = (IVersionSupport)(((crmDealWithStage)(this.MainObject)).Current);

            // Разбиваем создание версии на два этапа
            Dictionary<IVersionSupport, IVersionSupport> dict = vHelper.CreateNewVersionStep1(srcObj, vHelper);

            // Исправляем положение с "паразитными объектами"
            // Находим копию crmStage из числа новых версий
            crmStage copyStage = null;
            foreach (IVersionSupport key in dict.Keys) {
                if (dict[key] as crmStage == null) continue;
                copyStage = dict[key] as crmStage;
                break;
            }

            //// SHU 2011-10-17
            //copyStage.DateBegin = System.DateTime.Now;
            //copyStage.DateEnd = System.DateTime.Now;
            //copyStage.DateFinish = System.DateTime.Now;

            
            // Находим копию crmStageStructure из числа новых версий
            crmStageStructure copyStageStructure = null;
            foreach (IVersionSupport key in dict.Keys) {
                if (dict[key] as crmStageStructure == null) continue;
                copyStageStructure = dict[key] as crmStageStructure;
                break;
            }
            
            //// SHU 2011-10-17
            //copyStageStructure.FirstStage.DateBegin = System.DateTime.Now;
            //copyStageStructure.FirstStage.DateEnd = System.DateTime.Now;
            //copyStageStructure.FirstStage.DateFinish = System.DateTime.Now;

            // В копии dict[srcObj] заменяем структуру этапов и первый этап
            crmDealWithStageVersion copySrcObj = dict[srcObj] as crmDealWithStageVersion;

            //// SHU 2011-10-17
            //copySrcObj.StageStructure.FirstStage.DateBegin = System.DateTime.Now;
            //copySrcObj.StageStructure.FirstStage.DateEnd = System.DateTime.Now;
            //copySrcObj.StageStructure.FirstStage.DateFinish = System.DateTime.Now;

            ////copySrcObj.StageStructure.FirstStage.Delete();
            //copySrcObj.StageStructure.Delete();
            //copySrcObj.StageStructure = null;
            //copySrcObj.StageStructure = copyStageStructure;
            ////copySrcObj.StageStructure.FirstStage.Delete();
            ////copySrcObj.StageStructure.FirstStage = copyStage;
            

            //// Дополняем словарь новыми занчениями
            ////copySrcObj.StageStructure.Delete();
            ////copySrcObj.StageStructure = null;
            //dict.Add(copySrcObj.StageStructure, copyStageStructure);
            //dict.Add(copySrcObj.StageStructure.FirstStage, dict[((crmDealWithStageVersion)srcObj).StageStructure.FirstStage]);
            

            return vHelper.CreateNewVersionStep2(srcObj, dict, vHelper);
            */
        }

        public void Approve(IVersionSupport obj) {
        }

        /*
        public override Dictionary<IVersionSupport, IVersionSupport> GenerateCopyOfObjects(List<IVersionSupport> list, Session ssn, IVersionSupport sourceObj) {
            Dictionary<IVersionSupport, IVersionSupport>  dict = base.GenerateCopyOfObjects(list, ssn, sourceObj);

            // Находим копию crmStage из числа новых версий
            crmStage newStage = null;
            foreach (IVersionSupport srcObj in dict.Keys) {
                if (dict[srcObj] as crmStage == null) continue;
                newStage = dict[srcObj] as crmStage;
                break;
            }
            if (newStage == null) return dict;

            // Находим в dict повторение объекта и заменяем его на копию, а пустой удаляем
            foreach (IVersionSupport srcObj in dict.Keys) {
                if (dict[srcObj] as crmStageStructure == null) continue;

                crmStageStructure newStageStructure = dict[srcObj] as crmStageStructure;
                newStageStructure.FirstStage.Delete();
                newStageStructure.FirstStage = newStage;
                break;
            }

            return dict;
        }
        */
        #endregion

        [DelimitedRecord(";")]
        public class DealDataImport {
            public String OrderCode;
            public String StageCode;
            public String NomenclatureCode;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Count;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Price;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummaAll;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? PayAvans;
            [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
            public DateTime? DateContract;
            [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
            public DateTime? DateFactDelivery;
            [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
            public DateTime? DateFactPayment;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? SummaPayment;
            public String LastColumns;
        }

        public void Import(IObjectSpace os, String file_name) {
            FileHelperEngine<DealDataImport> engine = new FileHelperEngine<DealDataImport>();
            engine.Options.IgnoreFirstLines = 1;
            engine.Options.IgnoreEmptyLines = true;
            Int32 line = 0;
            //            DealDataImport[] deal_data = engine.ReadStream(reader);
            DealDataImport[] deal_data = engine.ReadFile(file_name);
            IList<fmCOrder> orders = new List<fmCOrder>();
            IList<crmStage> stages = new List<crmStage>();
            IList<crmDeliveryUnit> delivery_units = new List<crmDeliveryUnit>();
            IList<crmDeliveryItem> delivery_items = new List<crmDeliveryItem>();
            IList<crmPaymentUnit> payment_units = new List<crmPaymentUnit>();
            IList<crmPaymentItem> payment_items = new List<crmPaymentItem>();
            IList<csNomenclature> materials = os.GetObjects<csNomenclature>();
            foreach (DealDataImport record in deal_data) {
                fmCOrder order = null;
                crmStage stage = null;
                crmDeliveryUnit delivery_unit = null;
                crmDeliveryItem delivery_item = null;
                crmPaymentUnit payment_unit = null;
                line++;
                //                crmPaymentItem payment_item = null;
                if (!String.IsNullOrEmpty(record.OrderCode)) {
                    order = orders.FirstOrDefault(x => x.Code == record.OrderCode);
                    if (order == null) {
                        order = os.FindObject<fmCOrder>(new BinaryOperator("Code", record.OrderCode, BinaryOperatorType.Equal));
                        if (order == null)
                            throw new ArgumentException("Неизвестный заказ: " + record.OrderCode + " Строка: " + line, "OrderCode");
                        else
                            orders.Add(order);
                    }
                }
                if (String.IsNullOrEmpty(record.StageCode)) {
                    throw new ArgumentException("Номер этапа пустой. Строка: " + line, "StageCode");
                }
                if (record.StageCode.Substring(0, 3) == "Adv") {
                    stage = StageStructure.FirstStage;
                }
                else {
                    stage = StageStructure.Stages.FirstOrDefault(x => x.Code == record.StageCode);
                    if (stage == null) {
                        stage = StageStructure.FirstStage.SubStagesCreate();
                        stage.Code = record.StageCode;
                    }
                    if (!stages.Contains(stage)) {
                        stage.Order = order;
                        stage.StageType = Contract.StageType.FINANCE;
                        stage.DeliveryMethod = DeliveryMethod.UNITS_SHEDULE;
                        stage.PaymentMethod = PaymentMethod.SCHEDULE;
                        //                        stage.DateEnd = stage.DateBegin;
                        //                        stage.DateFinish = stage.DateEnd;
                        stages.Add(stage);
                    }
                }
                if (record.StageCode.Substring(0, 3) != "Adv") {
                    if (order == null) {
                        throw new ArgumentException("Заказ не найден", "OrderCode");
                    }
                    if (record.DateContract == null) {
                        throw new ArgumentException("Незаполнена плановая дата исполнения. Строка: " + line, "DateContract");
                    }
                    delivery_unit = stage.DeliveryPlan.DeliveryUnits.FirstOrDefault(x => x.DatePlane == record.DateContract);
                    if (record.DateContract > stage.DateEnd)
                        stage.DateEnd = (DateTime)record.DateContract;
                    if (delivery_unit == null) {
                        delivery_unit = stage.DeliveryPlan.DeliveryUnitCreate();
                        delivery_unit.DatePlane = (DateTime)record.DateContract;
                    }
                    if (!delivery_units.Contains(delivery_unit))
                        delivery_units.Add(delivery_unit);
                    delivery_unit.Order = order;
                    if (record.Count == null)
                        throw new ArgumentException("Кол-во пустое. Строка: " + line, "Count");
                    if (record.Price == null)
                        throw new ArgumentException("Цена пустая. Строка: " + line, "Price");
                    if (String.IsNullOrEmpty(record.NomenclatureCode))
                        throw new ArgumentException("Номенклатура пустая. Строка: " + line, "NomenclatureCode");
                    if (!record.NomenclatureCode.Contains("*I") && !record.NomenclatureCode.Contains("*E")) {

//                        csMaterial material = ;
                        csNomenclature nomenclature = materials.FirstOrDefault(x => x.CodeTechnical == record.NomenclatureCode);
                        if (nomenclature == null) {
                            throw new ArgumentException("Номенклатура: " + record.NomenclatureCode + " ненайдена. Строка: " + line, "NomenclatureCode");
                        }
                        delivery_item = delivery_unit.DeliveryItems.FirstOrDefault(x => x.Nomenclature == nomenclature);
                        if (delivery_item == null) {
                            if (nomenclature is csMaterial) {
                                delivery_item = delivery_unit.DeliveryItemsCreateMaterial();
                                ((crmDeliveryMaterial)delivery_item).Material = nomenclature as csMaterial;
                            }
                            else if (nomenclature is csService) {
                                delivery_item = delivery_unit.DeliveryItemsCreateService();
                                ((crmDeliveryService)delivery_item).Service = nomenclature as csService;
                            }
                            else {
                                throw new ArgumentException("Номенклатура: " + record.NomenclatureCode + " неверного типа. Строка: " + line, "NomenclatureCode");
                            }
                        }
                        delivery_item.CostCalculateMethod = CostCalculateMethod.CALC_COST;
                        delivery_item.NDSCalculateMethod = NDSCalculateMethod.FROM_COST;
                        delivery_item.FullCalculateMethod = FullCalculateMethod.CALC_FULL;
                        delivery_item.Price = (Decimal)record.Price;
                        delivery_item.CountUnit = delivery_item.Nomenclature.BaseUnit;
                        if (delivery_items.Contains(delivery_item))
                            delivery_item.CountValue += (Decimal)record.Count;
                        else {
                            delivery_item.CountValue = (Decimal)record.Count;
                            delivery_items.Add(delivery_item);
                        }
                    }
                }
                if (record.DateContract == null) {
                    throw new ArgumentException("Дата плановая исполнения не заполнена. Строка: " + line, "DateContract");
                }
                payment_unit = stage.PaymentPlan.PaymentUnits.FirstOrDefault(x => x.DatePlane == record.DateContract && x is crmPaymentCasheLess);
                //if (payment_unit != null && !payment_units.Contains(payment_unit)) {
                //    os.Delete(payment_unit);
                //    payment_unit = null;
                //}
                if (payment_unit == null) {
                    payment_unit = stage.PaymentPlan.PaymentCasheLessCreate();
                    payment_unit.DatePlane = (DateTime)record.DateContract;
                    if (payment_unit.DatePlane > stage.DateFinish)
                        stage.DateFinish = payment_unit.DatePlane;
                }
                payment_unit.Order = order;
                if (!payment_units.Contains(payment_unit)) {
                    ((crmPaymentCasheLess)payment_unit).SummFull = (Decimal)record.SummaPayment;
                    payment_units.Add(payment_unit);
                }
                else {
                    ((crmPaymentCasheLess)payment_unit).SummFull += (Decimal)record.SummaPayment;
                }

                //                payment_item = payment_unit.PaymentItems.FirstOrDefault(x => x.Order == order);
                //                if (payment_item == null) {
                //                    payment_item = payment_unit.PaymentItemsCreateMoney();
                //                }
                //if (payment_unit.PaymentItems.Count == 0) {
                //    payment_item = payment_unit.PaymentItemsCreateMoney();
                //}
                //else {
                //    payment_item = payment_unit.PaymentItems[0];
                //}
                //if (payment_items.Contains(payment_item)) {
                //    payment_item.SummFull += (Decimal)record.SummaPayment;
                //    payment_item.AccountSumma += (Decimal)record.SummaPayment;
                //}
                //else {
                //    payment_item.SummFull = (Decimal)record.SummaPayment;
                //    payment_item.AccountSumma = (Decimal)record.SummaPayment;
                //    payment_items.Add(payment_item);
                //}
            }
            IList<crmDeliveryUnit> del_delivery_units = new List<crmDeliveryUnit>();
            IList<crmDeliveryItem> del_delivery_items = new List<crmDeliveryItem>();
            IList<crmPaymentUnit> del_payment_units = new List<crmPaymentUnit>();
            IList<crmPaymentItem> del_payment_items = new List<crmPaymentItem>();
            foreach (crmStage stage in stages) {
                foreach (crmDeliveryUnit delivery_unit in stage.DeliveryPlan.DeliveryUnits) {
                    if (!delivery_units.Contains(delivery_unit)) {
                        del_delivery_units.Add(delivery_unit);
                        foreach (crmDeliveryItem item in delivery_unit.DeliveryItems)
                            del_delivery_items.Add(item);
                    }
                    else {
                        foreach (crmDeliveryItem item in delivery_unit.DeliveryItems)
                            if (!delivery_items.Contains(item))
                                del_delivery_items.Add(item);
                    }
                }
                foreach (crmPaymentUnit payment_unit in stage.PaymentPlan.PaymentUnits) {
                    if (!payment_units.Contains(payment_unit)) {
                        del_payment_units.Add(payment_unit);
                        foreach (crmPaymentItem item in payment_unit.PaymentItems)
                            del_payment_items.Add(item);
                    }
                    else {
                        foreach (crmPaymentItem item in payment_unit.PaymentItems)
                            if (!payment_items.Contains(item))
                                del_payment_items.Add(item);
                    }
                }
            }
            // Не удаляем PaymentItems, поскольку не создаем их, а создаем PaymentUnitCashLess, который сам создает Item
            //            os.Delete(del_payment_items);
            os.Delete(del_payment_units);
            os.Delete(del_delivery_items);
            os.Delete(del_delivery_units);
        }
    }
}