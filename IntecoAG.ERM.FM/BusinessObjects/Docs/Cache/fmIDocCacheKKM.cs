using System;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.Docs.Cache {

    [NavigationItem("Sale")]
    [DomainComponent]
    [XafDefaultProperty("Name")]
    public interface fmIDocCacheKKM {

        [FieldSize(20)]
        [RuleRequiredField]
        String KKMNumber { get; set; }
        [RuleRequiredField]
        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }
        Boolean IsClosed { get; set; }

        String Name { get; }
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheKKM));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheKKM))]
    public class fmIDocCacheKKMLogic {
        public static String Get_Name(fmIDocCacheKKM instance) {
            return "  Ã " + instance.KKMNumber;
        }
        public static void AfterChange_DateTo(fmIDocCacheKKM instance) {
            if (CommonConstants.DateMinValue < instance.DateTo && 
                instance.DateTo < CommonConstants.DateMaxValue) {
                    instance.IsClosed = true;
            }
        }
        public static void AfterChange_IsClosed(fmIDocCacheKKM instance) {
            if (!instance.IsClosed) {
                instance.DateTo = CommonConstants.DateMaxValue;
            }
        }
        public static void AfterConstruction(fmIDocCacheKKM instance) {
            instance.DateFrom = CommonMethods.DateTimeNow();
            instance.DateTo = CommonConstants.DateMaxValue;
        }
        //public static int SumMethod(fmIDocCacheKKM instance, int val1, int val2) {
        //    // You can also define custom methods.
        //    return val1 + val2;
        //}
    }
}
