using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
namespace IntecoAG.ERM.FM.Docs.Cache {
    [DomainComponent]
    public interface fmIDocCacheJournalLine {
        fmIDocCacheJournal Journal { get; set; }
        Int32 NumberSequence { get; set; }

        [Indexed(Unique=true)]
        fmIDocCache Document { get; set; }
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheJournalLine));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(fmIDocCacheJournalLine))]
    //public class fmIDocCacheJournalLineLogic {
    //    public static string Get_CalculatedProperty(fmIDocCacheJournalLine instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(fmIDocCacheJournalLine instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(fmIDocCacheJournalLine instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(fmIDocCacheJournalLine instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
