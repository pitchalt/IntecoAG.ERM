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

// === IntecoAG namespaces ===
using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CS.Nomenclature
{

    /// <summary>
    /// Класс NomenclatureType представляет базовую функциональность
    /// </summary>
    [Persistent("csNomenclatureType"), OptimisticLocking(false)]
    public abstract partial class NomenclatureType : XPObject, IHDenormalization<NomenclatureType>
    {
        public NomenclatureType() : base() { }
        public NomenclatureType(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            CrossRefAdd(this);
        }

        
        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Код типа номенткалтуры
        /// </summary>
        private string _Code;
        public string Code {
            get { return _Code; }
            set { if (_Code != value) SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// Наименование типа номенткалтыры
        /// </summary>
        private string _Name;
        public string Name {
            get { return _Name; }
            set { if (_Name != value) SetPropertyValue("Name", ref _Name, value); }
        }

        #endregion


        #region Разузлование дерева:  TopType - SubTypes

        // Ассоциация список аналитик нижнего уровня
        NomenclatureType _TopType;
        [Association("SubTypes"), Aggregated]
        public NomenclatureType TopType {
            get { return _TopType; }
            set {
                if (IsLoading) {
                    _TopType = value;
                }
                else {
                    List<NomenclatureType> fd = new List<NomenclatureType>(this.FullDownLevel);
                    foreach (var downobj in fd)
                        downobj.CrossRefRemove(downobj);
                    SetPropertyValue<NomenclatureType>("TopType", ref _TopType, value);
                    foreach (var downobj in fd)
                        downobj.CrossRefAdd(downobj);
                }
            }
        }

        [Association("SubTypes", typeof(NomenclatureType))]
        public XPCollection<NomenclatureType> SubTypes {
            get { return GetCollection<NomenclatureType>("SubTypes"); }
        }



        // Ассоциация отражающая полную прошивку дерева в соответствии с иерархией
        [Association("NomenclatureTypeCrossRef", typeof(NomenclatureType), UseAssociationNameAsIntermediateTableName = true)]
        public IList<NomenclatureType> FullDownLevel {
            get { return GetCollection<NomenclatureType>("FullDownLevel"); }
        }

        [Association("NomenclatureTypeCrossRef", typeof(NomenclatureType), UseAssociationNameAsIntermediateTableName = true)]
        public IList<NomenclatureType> FullUpLevel {
            get { return GetCollection<NomenclatureType>("FullUpLevel"); }
        }



        // Методы для рекурсивного изменения связи NomenclatureTypeCrossRef в случаях изменения иерархии
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

        #endregion

    }

}