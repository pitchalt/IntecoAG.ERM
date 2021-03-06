using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.Trw.Person;
using IntecoAG.ERM.Trw.Exchange;
//
namespace IntecoAG.ERM.Trw {
    [DomainComponent]
    public interface TrwIPerson {
        String TrwName { get; set; }
        String TrwINN { get; set; }
        String TrwKPP { get; set; }

        String TrwCity { get; set; }
        String TrwCityPrefix { get; set; }

        [Aggregated]
        IList<TrwIPersonAccount> TrwAccounts { get; }
//        String TrwAddress { get; set; }
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwIPartner));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwIPartner))]
    //public class TrwIPartnerLogic {
    //    public static string Get_CalculatedProperty(TrwIPartner instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwIPartner instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwIPartner instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwIPartner instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
