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
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.Xpo.Metadata;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.CRM.Contract.Deal
{

    /// <summary>
    /// Класс crmDealWithStageVersion
    /// </summary>
    //[Appearance("crmDealWithStageVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6 OR not isnull(crmComplexContractVersion)", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("crmDealWithStageVersion.GotoMainActionHidden", AppearanceItemType = "Action", Criteria = "not isnull(crmComplexContractVersion)", TargetItems = "GotoMainAction", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("crmDealWithStageVersion.ApproveHidden", AppearanceItemType = "Action", Criteria = "VersionState = 1 OR VersionState = 2 OR VersionState = 4 OR VersionState = 5 OR VersionState = 6", TargetItems = "VersionApprove", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Persistent("crmDealWithStageVersion")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealWithStageVersion : crmDealVersion, IVersionSupport, IVersionBusinessLogicSupport
    {
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
                    if (this.PaymentValuta == null || this.PaymentValuta == old ) {
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
            get { return StageStructure.FirstStage.CurrentCost; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("StageStructure.FirstStage.CurrentPayment")]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmCostCol CurrentPayment {
            get { return StageStructure.FirstStage.CurrentPayment; }
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

    }
}