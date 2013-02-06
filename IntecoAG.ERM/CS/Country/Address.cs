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
using System.Linq;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

namespace IntecoAG.ERM.CS.Country
{
    /// <summary>
    /// Класс Country, представляющий стороны Договора
    /// </summary>
    [Persistent("csAddress")]
    [DefaultProperty("Address")]
    public partial class csAddress : BaseObject
    {
        public csAddress(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            XPQuery<csCountry> Countrys = new XPQuery<csCountry>(Session);
            var qc = from ci in Countrys
                     where ci.CodeAlfa2 == "RU"
                     select ci;
            foreach (csCountry co in qc) {
                Country = co;
                break;
            }
            Region = String.Empty;
            City = String.Empty;
            Street = String.Empty;
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Description - описание
        /// </summary>
        private csCountry _Country;
        public csCountry Country {
            get { return _Country; }
            set { SetPropertyValue<csCountry>("Country", ref _Country, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _ZipPostal;
        public string ZipPostal {
            get { return _ZipPostal; }
            set { SetPropertyValue<string>("ZipPostal", ref _ZipPostal, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Region;
        public string Region {
            get { return _Region; }
            set { SetPropertyValue<string>("Region", ref _Region, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _StateProvince;
        public string StateProvince {
            get { return _StateProvince; }
            set { SetPropertyValue<string>("StateProvince", ref _StateProvince, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _City;
        public string City {
            get { return _City; }
            set { SetPropertyValue<string>("City", ref _City, value); }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Street;
        public string Street {
            get { return _Street; }
            set { SetPropertyValue<string>("Street", ref _Street, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _AddressString;
        public string AddressString {
            get { return _AddressString; }
            set { SetPropertyValue<string>("AddressString", ref _AddressString, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address {
            get {
                if (String.IsNullOrEmpty(this.AddressString)) {
                    string cn = Country == null ? "" : Country.NameRuShortLow;
                    return String.Concat(ZipPostal, " ", cn, " ", Region, " ", StateProvince, " ", City, " ", Street);
                }
                else {
                    return this.AddressString;
                }
            }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// 
        /// </summary>
        public csAddress Copy() {
            csAddress cp = new csAddress(this.Session);
            cp.ZipPostal  = this.ZipPostal;
            cp.Country = this.Country;
            cp.City = this.City;
            cp.Region   = this.Region;
            cp.StateProvince = this.StateProvince;
            cp.Street  = this.Street;
            cp.AddressString = this.AddressString;
            return cp;
        }
        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Address;
        }

        #endregion

    }

}