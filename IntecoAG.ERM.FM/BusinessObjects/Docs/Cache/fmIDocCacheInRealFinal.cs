using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.Docs.Cache {
    [DomainComponent]
    [NavigationItem("Sale")]
    public interface fmIDocCacheInRealFinal: fmIDocCacheIn {
        IList<fmIDocCacheInRealPrepare> LinkedCacheIn { get; }

        String KKMNumber { get; set; }
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheInRealFinal));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheInRealFinal))]
    public class fmIDocCacheInRealFinalLogic {
        public static IList<fmIDocCacheLine> Get_Lines(fmIDocCacheInRealFinal instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
            // Use this method to implement calculated properties.
            return null;
        }
    //    public static void AfterChange_PersistentProperty(fmIDocCacheInRealFinal instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(fmIDocCacheInRealFinal instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(fmIDocCacheInRealFinal instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    }
}
