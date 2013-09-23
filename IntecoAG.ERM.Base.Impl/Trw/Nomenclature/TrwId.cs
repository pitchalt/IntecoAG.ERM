using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Trw.Nomenclature {

    [DomainComponent]
    public interface TrwITemp {
        [FieldSize(22)]
        String TrwCode { get; set; }
        [FieldSize(128)]
        String TrwDescription { get; set; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwICfr));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwICfr))]
    //public class TrwICfrLogic {
    //    public static string Get_CalculatedProperty(TrwICfr instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwICfr instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwICfr instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwICfr instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
