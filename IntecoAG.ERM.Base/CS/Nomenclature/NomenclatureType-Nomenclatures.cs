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
    public partial class NomenclatureType
    {
        [Association("crmNomenclatureType-Nomenclatures"), Aggregated]
        public XPCollection<Nomenclature> _Nomenclatures {
            get {
                return GetCollection<Nomenclature>("_Nomenclatures");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class Nomenclature
    {
        private NomenclatureType _NomenclatureType;
        [Association("crmNomenclatureType-Nomenclatures")]
        public NomenclatureType NomenclatureType {
            get { return _NomenclatureType; }
            set { SetPropertyValue("NomenclatureType", ref _NomenclatureType, value); }
        }
    }

}
