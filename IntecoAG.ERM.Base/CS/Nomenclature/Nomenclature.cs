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
//
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
//
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.Trw;
//

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// Класс Номенклатура, понимаемый достаточно широко (материалы, услуги, финансы)
    /// </summary>
    [DefaultProperty("NameShort")]
    [Persistent("csNomenclature")]
    public abstract partial class csNomenclature : BaseObject
    {
        public csNomenclature(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА
        //
        private string _Code;
        /// <summary>
        /// Код номенклатуры
        /// </summary>
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
        //
        private string _CodeTechnical;
        /// <summary>
        /// Технический код номенклатуры
        /// </summary>
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string CodeTechnical {
            get { return _CodeTechnical; }
            set { SetPropertyValue("CodeTechnical", ref _CodeTechnical, value); }
        }
        /// <summary>
        /// Наименование номенклатуры
        /// </summary>
        private string _NameShort;
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string NameShort {
            get { return _NameShort; }
            set { SetPropertyValue("NameShort", ref _NameShort, value); }
        }

        /// <summary>
        /// Полное наименование
        /// </summary>
        private string _NameFull;
        public string NameFull
        {
            get { return _NameFull; }
            set { SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private csUnit _BaseUnit;
        public csUnit BaseUnit
        {
            get { return _BaseUnit; }
            set { SetPropertyValue<csUnit>("BaseUnit", ref _BaseUnit, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private fmCostItem _CostItem;
        public fmCostItem CostItem
        {
            get { return _CostItem; }
            set { SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
//        public override string ToString()
//        {
//            string Res = "";
//            Res = this.Code + " " + this.Name;
//            return Res;
//        }

        #endregion

        //[Browsable(false)]
        //public csNomenclatureType Category {
        //    get {
        //        return NomenclatureType;
        //    }
        //}

        //ITreeNode ICategorizedItem.Category {
        //    get {
        //        return NomenclatureType;
        //    }
        //    set {
        //        NomenclatureType = (csNomenclatureType) value;
        //    }
        //}
        #region Trw

        private TrwSaleNomenclatureType _TrwSaleNomenclatureType;
        public TrwSaleNomenclatureType TrwSaleNomenclatureType {
            get { return _TrwSaleNomenclatureType; }
            set { SetPropertyValue<TrwSaleNomenclatureType>("TrwSaleNomenclatureType", ref _TrwSaleNomenclatureType, value); }
        }

        private TrwSaleNomenclatureMeasurementUnit _TrwMeasurementUnit;
        public TrwSaleNomenclatureMeasurementUnit TrwMeasurementUnit {
            get { return _TrwMeasurementUnit; }
            set { SetPropertyValue<TrwSaleNomenclatureMeasurementUnit>("TrwMeasurementUnit", ref _TrwMeasurementUnit, value); }
        }

        #endregion Trw
    }

}