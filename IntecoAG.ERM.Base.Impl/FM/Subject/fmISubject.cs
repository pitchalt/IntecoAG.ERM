using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Subject {
//
    [DC.DomainComponent]
    public interface fmISubject: gfmIAnalytic {
        hrmIStaff Manager { get; set; }
        hrmIStaff ManagerPlanDepartment { get; set; }


        IList<fmIOrder> Orders { get; }
        fmIDirection Direction { get; }

        crmContractDeal SourceDeal { get; set; }
        String SourceOther { get; set; }
        String SourceName { get; }

        crmIParty SourceParty { get; set; }
        
        fmСOrderAnalitycWorkType AnalitycWorkType { get; set; }
        fmСOrderAnalitycFinanceSource AnalitycFinanceSource { get; set; }
    }
}
