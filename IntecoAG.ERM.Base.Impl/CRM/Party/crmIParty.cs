using System;
using System.ComponentModel;

using DC = DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.Sync;

namespace IntecoAG.ERM.CRM.Party {

    [LikeSearchPathList(new string[] { 
        "Name", 
        "INN", 
        "AddressFact.AddressString",
        "Person.Name", 
        "Person.Address.AddressString"
    })]
    //[LikeSearchPathList(new string[] { 
    //    "Name", 
    //    "INN", 
    //    "AddressFact.AddressString",
    //    "Person.Name"
    //})]
    [DC.DomainComponent]
    public interface crmIParty : csICodedComponent, SyncISyncObject {

        [VisibleInListView(false)]
        crmIPerson Person { get; }
        [Browsable(false)]
        crmCParty Party { get; }

        [DC.FieldSize(300)]
        [VisibleInListView(false)]
        String NameFull { get; set; }

        [VisibleInListView(false)]
        csCountry Country { get; }

        Boolean IsClosed { get;  }
        //[DC.Aggregated]
        [VisibleInListView(false)]
        //        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        csIAddress AddressFact { get; }

        [VisibleInDetailView(false)]
        [VisibleInLookupListView(true)]
        String AddressFactString { get; }

        //[DC.Aggregated]
//        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [VisibleInListView(false)]
        csIAddress AddressPost { get; }
        /// <summary>
        /// Российские реквизиты
        /// </summary>
        [DC.FieldSize(25)]
        [VisibleInListView(false)]
        String RegCode { get; }

        [VisibleInLookupListView(true)]
        [DC.FieldSize(15)]
        String INN { get; set; }

        [VisibleInLookupListView(true)]
        [DC.FieldSize(15)]
        String KPP { get; set; }

        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        [VisibleInLookupListView(false)]
        ManualCheckStateEnum ManualCheckStatus {
            get;
            set;
        }
    }
}
