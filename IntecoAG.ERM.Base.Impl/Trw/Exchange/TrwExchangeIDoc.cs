using System;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Exchange {

//    [DomainComponent]
    public interface TrwExchangeIDoc<T>
        where T : TrwExchangeIExportableObject
    {

        String DocNumber { get; }
        DateTime DocDate { get; }

        DateTime DocDateConfirm { get; }

        IList<TrwExchangeIDocObjectLink<T>> ObjectLinks { get; }

        TrwExchangeIDocObjectLink<T> ObjectLinksCreate(IObjectSpace os, T obj);
    }

    public static class TrwExchangeIDocLogic<T>
        where T : TrwExchangeIExportableObject
    {

    }
    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwIExchangeDoc));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwIExchangeDoc))]
    //public class TrwIExchangeDocLogic {
    //    public static string Get_CalculatedProperty(TrwIExchangeDoc instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwIExchangeDoc instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwIExchangeDoc instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwIExchangeDoc instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
