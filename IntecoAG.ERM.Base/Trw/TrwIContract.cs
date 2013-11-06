using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.Trw.Contract;
//
namespace IntecoAG.ERM.Trw {

    [DomainComponent]
    public interface TrwIContract {
        String TrwNumber { get; }
        DateTime TrwDate { get; }

        TrwIContractParty TrwCustomerParty { get; }
        TrwIContractParty TrwSupplierParty { get; }

        TrwContractMarket TrwContractMarket { get; }

        IList<TrwIOrder> TrwSaleOrders { get; }

        String TrwSubject { get; }

        DateTime TrwDateSigning { get; }
        DateTime TrwDateValidFrom { get; }
        DateTime TrwDateValidToPlan { get; }
        DateTime TrwDateValidToFact { get; }

        csValuta TrwObligationCurrency { get; }
        Decimal TrwObligationSumma { get; }

        csValuta TrwPaymentCurrency { get; }

        csNDSRate TrwVATRate { get; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwIContractDocument));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwIContractDocument))]
    //public class TrwIContractDocumentLogic {
    //    public static string Get_CalculatedProperty(TrwIContractDocument instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwIContractDocument instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwIContractDocument instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwIContractDocument instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
