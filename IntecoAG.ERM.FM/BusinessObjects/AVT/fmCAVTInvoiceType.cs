using System;
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
//
namespace IntecoAG.ERM.FM.AVT {
    public enum fmAVTInvoiceDirection {
        AVTInvoiceIn = 1,
        AVTInvoiceOut = 2
    }
    /// <summary>
    /// 
    /// </summary>
    [Persistent("fmAVTInvoiceType")]
    [NavigationItem("AVT")]
    public class fmCAVTInvoiceType : csCComponent {
        public fmCAVTInvoiceType(Session ses) : base(ses) { }
        //
        public override void  AfterConstruction() {
 	        base.AfterConstruction();
            
        }
        //
        private String _Name;
        private String _Prefix;
        private Boolean _AutoNumber;
        private fmAVTInvoiceDirection _InvoiceDirection;
        /// <summary>
        /// 
        /// </summary>
        public String Name {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean AutoNumber {
            get { return _AutoNumber; }
            set { SetPropertyValue<Boolean>("AutoNumber", ref _AutoNumber, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Prefix {
            get { return _Prefix; }
            set { SetPropertyValue<String>("Prefix", ref _Prefix, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public fmAVTInvoiceDirection InvoiceDirection {
            get { return _InvoiceDirection; }
            set { SetPropertyValue<fmAVTInvoiceDirection>("InvoiceDirection", ref _InvoiceDirection, value); }
        }

    }
}
