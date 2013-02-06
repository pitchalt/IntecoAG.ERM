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

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CS.Nomenclature
{

    /// <summary>
    /// Класс NomenclatureType представляет базовую функциональность
    /// </summary>
    //[Persistent("csNomenclatureType"), OptimisticLocking(false)]
    //[DefaultClassOptions]
    [Persistent("csNomenclatureType")]
    public partial class csNomenclatureType : BaseObject, ITreeNode //, IHDenormalization<NomenclatureType>
    {
        public csNomenclatureType(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Код типа номенклатуры
        /// </summary>
        private string _Code;
        public string Code {
            get { return _Code; }
            set { if (_Code != value) SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// Наименование типа номенклатуры
        /// </summary>
        private string _Name;
        public string Name {
            get { return _Name; }
            set { if (_Name != value) SetPropertyValue("Name", ref _Name, value); }
        }

        #endregion


        #region Разузлование дерева:  TopType - SubTypes

        // Ассоциация список аналитик нижнего уровня
        csNomenclatureType _TopType;
        [Association("SubTypes"), Aggregated]
        public csNomenclatureType TopType {
            get { return _TopType; }
            set {
//                if (IsLoading) {
//                    _TopType = value;
//                }
//                else {
//                    List<NomenclatureType> fd = new List<NomenclatureType>(this.FullDownLevel);
//                    foreach (var downobj in fd)
//                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<csNomenclatureType>("TopType", ref _TopType, value);
//                    foreach (var downobj in fd)
//                        downobj.CrossRefAdd(downobj);
//                }
            }
        }

        [Association("SubTypes", typeof(csNomenclatureType))]
        public XPCollection<csNomenclatureType> SubTypes {
            get { return GetCollection<csNomenclatureType>("SubTypes"); }
        }


/*
        #region Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        
        [Association("NomenclatureTypeCrossRef", typeof(NomenclatureType), UseAssociationNameAsIntermediateTableName = true)]
        public IList<NomenclatureType> FullDownLevel {
            get { return GetCollection<NomenclatureType>("FullDownLevel"); }
        }

        [Association("NomenclatureTypeCrossRef", typeof(NomenclatureType), UseAssociationNameAsIntermediateTableName = true)]
        public IList<NomenclatureType> FullUpLevel {
            get { return GetCollection<NomenclatureType>("FullUpLevel"); }
        }

        #region Правая сторона связи
        [Association("NomenclatureTypeSelfRef-UpNomenclatureTypes")]
        protected IList<NomenclatureTypeSelfRef> RightLinks {
            get {
                return GetList<NomenclatureTypeSelfRef>("RightLinks");
            }
        }
        [ManyToManyAlias("RightLinks", "NomenclatureType")]
        public IList<NomenclatureType> FullUpLevel {
            get { return GetList<NomenclatureType>("FullUpLevel"); }
        }
        #endregion
        
        #region Левая сторона связи
        [Association("NomenclatureTypeSelfRef-DownNomenclatureTypes")]
        protected IList<NomenclatureTypeSelfRef> LeftLinks {
            get {
                return GetList<NomenclatureTypeSelfRef>("LeftLinks");
            }
        }
        [ManyToManyAlias("LeftLinks", "NomenclatureType")]
        public IList<NomenclatureType> FullDownLevel {
            get { return GetList<NomenclatureType>("FullDownLevel"); }
        }
        #endregion

        #endregion


        #region Методы для рекурсивного изменения связи NomenclatureTypeCrossRef в случаях изменения иерархии
        protected void CrossRefAdd(NomenclatureType obj) {
            FullDownLevel.Add(obj);
            if (TopType != null)
                TopType.CrossRefAdd(obj);
        }

        protected void CrossRefRemove(NomenclatureType obj) {
            if (obj != this)
                FullDownLevel.Remove(obj);
            if (TopType != null)
                TopType.CrossRefRemove(obj);
        }
        #endregion

        #region Разузлование дерева: реализация интерфейса

        // Ассоциация список аналитик нижнего уровня
        public NomenclatureType UpLevel {
            get { return TopType; }
            set { TopType = value; }
        }

        public IList<NomenclatureType> DownLevel {
            get { return SubTypes; }
        }

        #endregion
        */

        #endregion


        public System.ComponentModel.IBindingList Children {
            get { return SubTypes; }
        }

        public ITreeNode Parent {
            get { return TopType; }
        }

        string ITreeNode.Name {
            get { return Code; }
        }
    }


    ///// <summary>
    ///// Связывающий класс
    ///// </summary>
    //public class NomenclatureTypeSelfRef : BaseObject
    //{
    //    public NomenclatureTypeSelfRef(Session session) : base(session) { }

    //    private NomenclatureType _UpNomenclatureType;
    //    [Association("NomenclatureTypeSelfRef-UpNomenclatureTypes")]
    //    public NomenclatureType UpNomenclatureType {
    //        get { return _UpNomenclatureType; }
    //        set { SetPropertyValue("UpNomenclatureType", ref _UpNomenclatureType, value); }
    //    }

    //    private NomenclatureType _DownNomenclatureType;
    //    [Association("NomenclatureTypeSelfRef-DownNomenclatureTypes")]
    //    public NomenclatureType DownNomenclatureType {
    //        get { return _DownNomenclatureType; }
    //        set { SetPropertyValue("DownNomenclatureType", ref _DownNomenclatureType, value); }
    //    }
    //}

}