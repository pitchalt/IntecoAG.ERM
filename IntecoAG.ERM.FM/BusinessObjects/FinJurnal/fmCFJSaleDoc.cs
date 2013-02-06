using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.AVT;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.FinJurnal {
    /// <summary>
    /// Класс 
    /// </summary>
    [Persistent("fmFJSaleDoc")]
    [NavigationItem("Sale")]
    public class fmCFJSaleDoc : csCComponent {
        public fmCFJSaleDoc(Session ses) : base(ses) { }
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        //
        private Int32 _Period;
        private String _SalePeriod;
        private String _Name;
        /// <summary>
        /// 
        /// </summary>
        public String SalePeriod {
            get { return _SalePeriod; }
            set {
                String old = _SalePeriod;
                if (old != value) {
                    _SalePeriod = value;
                    if (!IsLoading) {
                        Int32 period = 0;
                        Int32.TryParse(value, out period);
                        Period = period;

                        OnChanged("SalePeriod", old, value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Int32 Period {
            get { return _Period; }
            set {
                SetPropertyValue<Int32>("Period", ref _Period, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Size(70)]
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [Aggregated]
        [Association("fmCFJSaleDoc-fmCFJSale", typeof(fmCFJSaleDocLine))]
        public XPCollection<fmCFJSaleDocLine> DocLines {
            get { return GetCollection<fmCFJSaleDocLine>("DocLines"); }
        }
    }
}
