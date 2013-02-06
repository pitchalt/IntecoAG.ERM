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
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Order {

    [DC.DomainComponent]
    public interface fmIOrder: gfmIAnalytic {
        hrmIStaff Manager { get; set;  }
        hrmIStaff ManagerPlanDepartment { get; set; }
        fmISubject Subject { get; }

        crmContractDeal SourceDeal { get; set; }
        crmIParty SourceParty { get; set; }
        String SourceOther { get; set; }
        String SourceName { get; }

        Int32 BuhIntNum { get; set; }
        String BuhAccount { get; set; }
        fmСOrderAnalitycAccouterType AnalitycAccouterType { get; set; }
        fmСOrderAnalitycAVT AnalitycAVT { get; set; }
        csNDSRate AVTRate { get; set; }
        Decimal KoeffKB { get; set; }
        Decimal KoeffOZM { get; set; }

        fmСOrderAnalitycWorkType AnalitycWorkType { get; set; }
        fmСOrderAnalitycOrderSource AnalitycOrderSource { get; set; }
        fmСOrderAnalitycFinanceSource AnalitycFinanceSource { get; set;}
        fmСOrderAnalitycMilitary AnalitycMilitary { get; set;}
        fmСOrderAnalitycOKVED AnalitycOKVED { get; set; }
        fmСOrderAnalitycFedProg AnalitycFedProg { get; set; }

        void CopyTo(fmIOrder to);
    }

    public static class fmIOrderLogic { 
        public static void CopyTo(fmIOrder from, fmIOrder to) {
            to.Code = from.Code;
            to.Name = from.Name;
            to.NameFull = from.NameFull;
//            to.Subject = from.Subject as fmCSubjectExt;
            to.DateBegin = from.DateBegin;
            to.DateEnd = from.DateEnd;
            to.BuhAccount = from.BuhAccount;
            to.BuhIntNum = from.BuhIntNum;
            to.AVTRate = from.AVTRate;
            to.KoeffKB = from.KoeffKB;
            to.KoeffOZM = from.KoeffOZM;
            to.Manager = from.Manager;
            to.ManagerPlanDepartment = from.ManagerPlanDepartment;
            to.SourceParty = from.SourceParty;
            to.SourceDeal = from.SourceDeal;
            to.SourceOther = from.SourceOther;
            to.AnalitycAccouterType = from.AnalitycAccouterType;
            to.AnalitycAVT = from.AnalitycAVT;
            to.AnalitycFinanceSource = from.AnalitycFinanceSource;
            to.AnalitycMilitary = from.AnalitycMilitary;
            to.AnalitycOrderSource = from.AnalitycOrderSource;
            to.AnalitycWorkType = from.AnalitycWorkType;
            to.AnalitycOKVED = from.AnalitycOKVED;
            to.AnalitycFedProg = from.AnalitycFedProg;
        }
    }
}
