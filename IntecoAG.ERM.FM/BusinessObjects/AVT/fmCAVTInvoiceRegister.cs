using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.AVT {
    /// <summary>
    /// 
    /// </summary>
    [Persistent("fmAVTInvoiceRegister")]
    [VisibleInReports]
    [NavigationItem("AVT")]
    public class fmCAVTInvoiceRegister : BaseObject {
        public fmCAVTInvoiceRegister(Session session) : base(session) { }

        private crmCParty _Party;
        private String _Period;
        private fmCAVTBookBuhImport _BookBuhImport;
        /// <summary>
        /// 
        /// </summary>
        public fmCAVTBookBuhImport BookBuhImport {
            get { return _BookBuhImport; }
            set { SetPropertyValue<fmCAVTBookBuhImport>("BookBuhImport", ref _BookBuhImport, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public crmCParty Party {
            get { return _Party; }
            set { SetPropertyValue<crmCParty>("Party", ref _Party, value); }
        }

        /// <summary>
        /// Реестр входящих
        /// </summary>
        [Aggregated]
        [Association("fmCAVTInvoiceRegister-fmCAVTInvoiceRegisterInLines", typeof(fmCAVTInvoiceRegisterLine))]
        public XPCollection<fmCAVTInvoiceRegisterLine> InLines {
            get { return GetCollection<fmCAVTInvoiceRegisterLine>("InLines"); }
        }
        /// <summary>
        /// Реестр исходящих
        /// </summary>
        [Aggregated]
        [Association("fmCAVTInvoiceRegister-fmCAVTInvoiceRegisterOutLines", typeof(fmCAVTInvoiceRegisterLine))]
        public XPCollection<fmCAVTInvoiceRegisterLine> OutLines {
            get { return GetCollection<fmCAVTInvoiceRegisterLine>("OutLines"); }
        }
        /// <summary>
        /// Период YYYYK
        /// </summary>
        [Size(5)]
        public String Period {
            get { return _Period;  }
            set { SetPropertyValue<String>("Period", ref _Period, value); }
        }
        /// <summary>
        /// Год
        /// </summary>
        [NonPersistent]
        public String PeriodYYYY {
            get { return Period != null && Period.Length == 5 ? Period.Substring(0,4) : String.Empty; }
        }
        /// <summary>
        /// Квартал
        /// </summary>
        [NonPersistent]
        public String PeriodKV {
            get { return Period != null && Period.Length == 5 ? Period.Substring(4, 1) : String.Empty; }
        }
        
        public fmCAVTInvoiceRegisterLine AddInInvoice(fmCAVTInvoiceBase invoice) { 
            foreach (fmCAVTInvoiceRegisterLine line in InLines) {
                if (line.Invoice == invoice) {
                    return line;
                }
            }
            fmCAVTInvoiceRegisterLine newline = new fmCAVTInvoiceRegisterLine(this.Session);
            newline.Invoice = invoice;
            InLines.Add(newline);
            return newline;
        }
        
    }

}
