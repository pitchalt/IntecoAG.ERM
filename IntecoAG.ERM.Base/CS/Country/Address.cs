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
using System.ComponentModel;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CS.Country
{
    /// <summary>
    /// Класс Country, представляющий стороны Договора
    /// </summary>
    [Persistent("csAddress")]
    [DefaultProperty("AddressString")]
    public partial class csAddress : BaseObject, csIAddress
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
        }


        #region ПОЛЯ КЛАССА
        private csCountry _Country;
        private String _ZipPostal;
        private String _Region;
        private String _StateProvince;
        private String _CityType;
        private String _City;
        private String _Street;
        //
        private String _AddressComponent;
        private String _AddressHandmake;
        [Persistent("AddressString")]
        [Size(120)]
        private String _AddressString;

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Description - описание
        /// </summary>
        [RuleRequiredField(TargetCriteria = "!IsEmpty")]
        public csCountry Country {
            get { return _Country; }
            set { 
                SetPropertyValue<csCountry>("Country", ref _Country, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(10)]
        public string ZipPostal {
            get { return _ZipPostal == null? String.Empty : _ZipPostal; }
            set { 
                SetPropertyValue<string>("ZipPostal", ref _ZipPostal, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(30)]
        public string Region {
            get { return _Region == null? String.Empty : _Region; }
            set { 
                SetPropertyValue<string>("Region", ref _Region, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(30)]
        public string StateProvince {
            get { return _StateProvince == null? String.Empty : _StateProvince; }
            set { 
                SetPropertyValue<string>("StateProvince", ref _StateProvince, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(5)]
        //[RuleRequiredField(TargetCriteria = "!IsEmpty")]
        public string CityType {
            get { return _CityType == null ? String.Empty : _CityType; }
            set {
                SetPropertyValue<string>("CityType", ref _CityType, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(30)]
        [RuleRequiredField(TargetCriteria = "!IsEmpty")]
        public string City {
            get { return _City == null ? String.Empty : _City; }
            set {
                SetPropertyValue<string>("City", ref _City, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// Description - описание
        /// </summary>
        [Size(60)]
        public string Street {
            get { return _Street == null ? String.Empty : _Street; }
            set { 
                SetPropertyValue<string>("Street", ref _Street, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(120)]
        public String AddressHandmake {
            get { return _AddressHandmake; }
            set {
                SetPropertyValue<String>("AddressHandmake", ref _AddressHandmake, value);
                if (!IsLoading)
                    UpdateCalcField();
            }
        }
        [Browsable(false)]
        public Boolean IsEmpty {
            get {
                return
                    String.IsNullOrEmpty(ZipPostal) &&
                    String.IsNullOrEmpty(Region) &&
                    String.IsNullOrEmpty(StateProvince) &&
                    String.IsNullOrEmpty(City) &&
                    String.IsNullOrEmpty(Street) &&
                    String.IsNullOrEmpty(AddressString); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String AddressComponent {
            get { return this._AddressComponent; }
        }
        /// <summary>
        /// 
        /// </summary>
        [PersistentAlias("_AddressString")]
        [Size(120)]
        public String AddressString {
            get { return _AddressString; }
        }


        #endregion


        #region МЕТОДЫ
        /// <summary>
        /// 
        /// </summary>
        void UpdateCalcField() {
            if (!IsEmpty) {
                StringBuilder sb = new StringBuilder(250);
                if (Country != null) {
                    sb.Append(this.Country.CodeRuAlfa3);
                    sb.Append(' ');
                }
                sb.Append(this.ZipPostal);
                sb.Append(", ");
                if (!String.IsNullOrEmpty(this.Region)) {
                    sb.Append(this.Region);
                    sb.Append(", ");
                }
                if (!String.IsNullOrEmpty(this.StateProvince)) {
                    sb.Append(this.StateProvince);
                    sb.Append(", ");
                }
                sb.Append(this.CityType);
                sb.Append(" ");
                sb.Append(this.City);
                sb.Append(", ");
                sb.Append(this.Street);
                this._AddressComponent = sb.ToString();
            }
            //
            if (String.IsNullOrEmpty(this.AddressHandmake))
                this._AddressString = this.AddressComponent;
            else
                this._AddressString = this.AddressHandmake;
            //
            OnChanged("AddressComponent");
            OnChanged("AddressString");
            OnChanged("IsEmpty");
        }

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
            cp.AddressHandmake = this.AddressHandmake;
            return cp;
        }
        /// <summary>
        /// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AddressString;
        }

        csIAddress csIAddress.Copy() {
            return this.Copy();
        }

        #endregion

    }

}