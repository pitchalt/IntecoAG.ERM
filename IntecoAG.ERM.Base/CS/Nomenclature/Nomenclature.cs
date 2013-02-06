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
//using IntecoAG.ERM.CS;
// === IntecoAG namespaces ===

namespace IntecoAG.ERM.CS.Nomenclature
{
    /// <summary>
    /// Класс Номенклатура, понимаемый достаточно широко (материалы, услуги, финансы)
    /// </summary>
    [Persistent("crmNomenclature")]
    public abstract partial class Nomenclature : XPObject
    {
        public Nomenclature() : base() { }
        public Nomenclature(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
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

    }

}