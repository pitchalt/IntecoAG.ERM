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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.FM;

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


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private string _Code;
        /// <summary>
        /// Код типа номенклатуры
        /// </summary>
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string Code {
            get { return _Code; }
            set { if (_Code != value) SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// Наименование типа номенклатуры
        /// </summary>
        private string _NameShort;
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string NameShort {
            get { return _NameShort; }
            set { if (_NameShort != value) SetPropertyValue("NameShort", ref _NameShort, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _NameFull;
        public string NameFull
        {
            get { return _NameFull; }
            set { if (_NameFull != value) SetPropertyValue<String>("NameFull", ref _NameFull, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private csUnit _BaseUnit;
        public csUnit BaseUnit
        {
            get { return _BaseUnit; }
            set { if (_BaseUnit != value) SetPropertyValue<csUnit>("BaseUnit", ref _BaseUnit, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private fmCostItem _CostItem;
        public fmCostItem CostItem
        {
            get { return _CostItem; }
            set { if (_CostItem != value) SetPropertyValue<fmCostItem>("CostItem", ref _CostItem, value); }
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
    }

}