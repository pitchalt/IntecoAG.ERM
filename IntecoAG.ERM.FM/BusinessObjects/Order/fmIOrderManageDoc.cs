using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Order {

    public enum fmIOrderManageDocStatus { 
        Execution = 1,
        AcceptMaker = 2,
        AcceptPlanDepartment = 3,
        AcceptAccountDepartment = 4,
        Rejected = 5
    }

    [DC.DomainComponent]
    public interface fmIOrderManageDoc : fmIOrder, fmIFinIndexStructure {
        fmIOrderManageDocStatus Status { get; }
        fmIOrderExt Order { get; set; }
    }
}
