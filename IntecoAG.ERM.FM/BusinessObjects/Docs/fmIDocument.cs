using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Docs {
    [DomainComponent]
    public interface fmIDocument {
        crmCParty DocParty { get; set; }
        [FieldSize(120)]
        String    DocPartyName { get; set; }
        [FieldSize(8)]
        String DocPartyOKPO { get; }
        hrmDepartment DocPartyDepartment { get; set; }
        [FieldSize(120)]
        String DocPartyDepartmentName { get; set; }
        [FieldSize(7)]
        String DocOKUDCode { get; }
        [FieldSize(10)]
        String DocNumber { get; set; }
        [RuleRequiredField()]
        DateTime DocDate { get; set; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(IDocument));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocument))]
    public class IDocumentLogic {
        public static string Get_DocPartyOKPO(fmIDocument instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
            // Use this method to implement calculated properties.
            return "07501739";
        }
        public static void AfterChange_DocParty(fmIDocument instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.DocParty != null) 
                instance.DocPartyName = instance.DocParty.Name;  
        }
        public static void AfterChange_DocPartyDepartment(fmIDocument instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.DocPartyDepartment != null)
                instance.DocPartyDepartmentName = instance.DocPartyDepartment.Name;
        }
        //    public static void AfterConstruction(IDocument instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(IDocument instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    }
}
