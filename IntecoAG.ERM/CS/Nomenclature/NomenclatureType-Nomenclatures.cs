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
using DevExpress.Xpo;

namespace IntecoAG.ERM.CS.Nomenclature
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class csNomenclatureType
    {
        [Association("csNomenclatureType-Nomenclatures"), Aggregated]
        public XPCollection<Nomenclature> Nomenclatures {
            get {
                return GetCollection<Nomenclature>("Nomenclatures");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Nomenclature
    {
        private csNomenclatureType _NomenclatureType;
        [Association("csNomenclatureType-Nomenclatures")]
        public csNomenclatureType NomenclatureType {
            get { return _NomenclatureType; }
            set { SetPropertyValue("NomenclatureType", ref _NomenclatureType, value); }
        }
    }

}
