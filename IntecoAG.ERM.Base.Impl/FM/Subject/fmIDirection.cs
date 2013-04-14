using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.GFM;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Subject {

    public enum fmIDirectionStatus {
        LOADED = 0,
        PROJECT = 1,
        OPENED = 2,
        CLOSED = 3
    }

    [DC.DomainComponent]
    public interface fmIDirection: gfmIAnalytic {
        fmIDirectionStatus Status { get; }
        hrmIStaff Manager { get; set;  }
        IList<fmISubject> Subjects { get; }
    }
}
