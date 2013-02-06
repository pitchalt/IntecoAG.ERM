using System;
using System.Collections.Generic;
//
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Docs.Cache {
    [DomainComponent]
    public interface fmIDocCacheInRealPrepareLine: fmIDocCacheLine  {

        fmIDocCacheInRealPrepare DocCacheInRealPrepare { get; set; }

        fmCOrder Order { get; set; }

        Decimal AVTSumma { get; set; }
        csNDSRate AVTRate { get; set; }

        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheInRealPrepareLine));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheInRealPrepareLine))]
    public class fmIDocCacheInRealPrepareLineLogic {
    //    public static string Get_CalculatedProperty(fmIDocCacheInRealPrepareLine instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
        public static String Get_AnaliticCode(fmIDocCacheInRealPrepareLine instance) {
            if (instance.Order != null)
                return instance.Order.Code;
            else
                return String.Empty;
        }


        public static void AfterChange_DocCacheInRealPrepare(fmIDocCacheInRealPrepareLine instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.DocCacheInRealPrepare != null) {
                instance.Order = instance.DocCacheInRealPrepare.Order;
                instance.AVTRate = instance.DocCacheInRealPrepare.AVTRate;
            }
        }
    //    public static void AfterConstruction(fmIDocCacheInRealPrepareLine instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(fmIDocCacheInRealPrepareLine instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    }
}
