using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using IntecoAG.ERM.CRM.Party;
//
using DC = DevExpress.ExpressApp.DC;
//
namespace IntecoAG.ERM.HRM.Organization {
    [DC.DomainComponent]
    [DC.XafDefaultProperty("NameFull")]
    public interface hrmIStaff {
        Boolean IsClosed { get; }
        DateTime DateBegin { get; set; }
        DateTime DateEnd { get; set; }
        crmPhysicalPersonSex Sex { get; set; }
        String FirstName {get; set;}
        String MiddleName { get; set; }
        String LastName { get; set; }
        String NameFull { get; }
        hrmIStaffPost Post {get; set; } 
    }
}
