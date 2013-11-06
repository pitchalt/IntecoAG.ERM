using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.Trw.Exchange;
//
namespace IntecoAG.ERM.Trw.Contract {

    public enum TrwContractPartyType {
        PARTY_UNKNOW    = 0,
        PARTY_CUSTOMER  = 1,
        PARTY_SUPPLIER  = 2
    }
    [DomainComponent]
//    [NonPersistentDc]
    public interface TrwIContractParty: TrwExchangeIExportableObject {
        TrwIContract TrwContract { get; }
        TrwContractPartyType TrwContractPartyType { get; }
        TrwContractType TrwContractType { get; set; }

        String TrwInternalNumber { get; }

        TrwICfr     TrwCfr { get; }
        TrwIPerson  TrwCfrPerson { get; }

        TrwIPerson  TrwPartyPerson { get; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwIContract));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwIContract))]
    //public class TrwIContractLogic {
    //    public static string Get_CalculatedProperty(TrwIContract instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwIContract instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwIContract instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwIContract instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
