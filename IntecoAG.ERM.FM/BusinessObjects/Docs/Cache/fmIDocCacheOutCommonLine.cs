using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.Docs.Cache {

    [DomainComponent]
    public interface fmIDocCacheOutCommonLine : fmIDocCacheLine {
        fmIDocCacheOutCommon DocCacheOutCommon { get; set; }

        fmCOrder Order { get; set; }

        Decimal AVTSumma { get; set; }
        csNDSRate AVTRate { get; set; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheOutCommon));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheOutCommonLine))]
    public class fmIDocCacheOutCommonLineLogic {
        public static String Get_AnaliticCode(fmIDocCacheOutCommonLine instance) {
            if (instance.Order != null)
                return instance.Order.Code;
            else
                return String.Empty;
        }


        public static void AfterChange_DocCacheOutCommon(fmIDocCacheOutCommonLine instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.DocCacheOutCommon != null) {
                instance.Order = instance.DocCacheOutCommon.Order;
                instance.AVTRate = instance.DocCacheOutCommon.AVTRate;
            }
        }
    //    public static string Get_CalculatedProperty(fmIDocCacheOutCommon instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(fmIDocCacheOutCommon instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(fmIDocCacheOutCommon instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(fmIDocCacheOutCommon instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    }
}
