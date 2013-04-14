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
    public enum fmISubjectStatus {
        LOADED = 0,
        PROJECT = 1,
        OPENED = 2,
        CLOSED = 3,
        DELETE = 9
    }

    public enum fmSubjectSourceType { 
        SOURCE_TYPE_CONTRACT = 1,
        SOURCE_TYPE_OTHER = 2
    }

    [DC.DomainComponent]
    public interface fmISubject: gfmIAnalytic {

        fmISubjectStatus Status { get; set; } 

        hrmIStaff Manager { get; set; }
        hrmIStaff ManagerPlanDepartment { get; set; }


        IList<fmIOrder> Orders { get; }
        fmIDirection Direction { get; set; }

        fmSubjectSourceType SourceType { get; set; }
        crmContractDeal SourceDeal { get; set; }
        String SourceOther { get; set; }
        String SourceName { get; }

        crmIParty SourceParty { get; set; }
        
        fmСOrderAnalitycWorkType AnalitycWorkType { get; set; }
        fmСOrderAnalitycFinanceSource AnalitycFinanceSource { get; set; }
        fmСOrderAnalitycOrderSource AnalitycOrderSource { get; set; }
        fmСOrderAnalitycMilitary AnalitycMilitary { get; set; }
        fmСOrderAnalitycOKVED AnalitycOKVED { get; set; }
        fmСOrderAnalitycFedProg AnalitycFedProg { get; set; }
        fmСOrderAnalitycRegion AnalitycRegion { get; set; }
        fmСOrderAnalitycBigCustomer AnalitycBigCustomer { get; set; }
    }
}
