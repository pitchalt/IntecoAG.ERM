using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.Docs.Cache {
    [DomainComponent]
    [VisibleInReports(true)]
    [NavigationItem("Sale")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    public interface fmIDocCacheOut : fmIDocCache {
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheOut));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheOut))]
    public class fmIDocCacheOutLogic {
        public static String Get_DocOKUDCode(fmIDocCacheOut instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
            // Use this method to implement calculated properties.
            return "testOKUD";
        }
    }
}
