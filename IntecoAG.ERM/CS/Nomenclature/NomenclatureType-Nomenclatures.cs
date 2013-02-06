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
        [Association("csNomenclatureType-Nomenclatures")]
        public XPCollection<csNomenclature> Nomenclatures {
            get {
                return GetCollection<csNomenclature>("Nomenclatures");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class csNomenclature
    {
        private csNomenclatureType _NomenclatureType;
        [Association("csNomenclatureType-Nomenclatures")]
        public csNomenclatureType NomenclatureType {
            get { return _NomenclatureType; }
            set { SetPropertyValue("NomenclatureType", ref _NomenclatureType, value); }
        }
    }

}
