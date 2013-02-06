using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
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
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.AVT {
    /// <summary>
    /// Класс счета фактуры 
    /// </summary>
    //[Persistent("fmAVTInvoiceJurnalLine")]
    //[NavigationItem("AVT")]
    [NonPersistent]
    public class fmCAVTInvoiceJurnalLine : XPLiteObject {
        public fmCAVTInvoiceJurnalLine(Session ses)
            : base(ses) {
        }
        //
        [Browsable(false)]
        [Key(AutoGenerate=true)]
        public Guid Oid;
        //
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        //
        private DateTime _Date;
        private fmCAVTInvoiceVersion _InvoiceVersion;
        /// <summary>
        /// Дата 
        /// </summary>
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value); 
            }
        }
        /// <summary>
        /// Версия счета фактуры
        /// </summary>
        public fmCAVTInvoiceVersion InvoiceVersion {
            get { return _InvoiceVersion; }
            set {
                SetPropertyValue<fmCAVTInvoiceVersion>("InvoiceVersion ", ref _InvoiceVersion, value);
            }
        }

    }
}
