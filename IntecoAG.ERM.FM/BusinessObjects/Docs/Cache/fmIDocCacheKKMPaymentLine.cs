using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.FM.FinJurnal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Finance;
//
namespace IntecoAG.ERM.FM.Docs.Cache {

    [DomainComponent]
    public interface fmIDocCacheKKMPaymentLine {
        fmIDocCacheKKMPayment KKMPayment { get; set; }

        [RuleRequiredField]
        String CheckNumber { get; set; }
        [RuleRequiredField]
        fmIDocCacheKKM KKM { get; set; }
        [RuleRequiredField]
        fmCOrder Order { get; set; }

        [RuleRequiredField]
        Int32 DocBuhProv { get; set; }
        [RuleRequiredField]
        Int32 DocBuhPck { get; set; }
        Int32 DocBuhNumber { get; set; }
        [RuleRequiredField]
        Int32 AccRealDebet { get; set; }
        [RuleRequiredField]
        Int32 AccRealCredit { get; set; }
        [RuleRequiredField]
        Int32 AccAVTDebet { get; set; }
        [RuleRequiredField]
        Int32 AccAVTCredit { get; set; }

        [RuleRequiredField]
        fmCFJSaleOperation Operation { get; set; }

        [RuleRequiredField]
        csNDSRate VatRate { get; set; }

        [RuleRequiredField]
        Decimal SummAll { get; set; }
        Decimal SummCost { get; set; }
        Decimal SummVat { get; set; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheKKMPayment));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheKKMPaymentLine))]
    public class fmIDocCacheKKMPaymentLineLogic {
        //public static fmCFJSaleOperation Get_Operation(fmIDocCacheKKMPaymentLine instance) {
        //    return instance.Operation != null ? instance.Operation : instance.KKMPayment.Operation;
        //}

        public static void AfterChange_KKMPayment(fmIDocCacheKKMPaymentLine instance) {
            if (instance.KKMPayment != null) {
                instance.Operation = instance.KKMPayment.Operation;
                instance.Order = instance.KKMPayment.Order;
            }
        }

        public static void AfterChange_Operation(fmIDocCacheKKMPaymentLine instance) {
            if (instance.Operation != null) {
//                instance.Order = instance.Operation.Order;
//                instance.VatRate = instance.Operation.AVTRate;
                instance.DocBuhProv = instance.Operation.DocBuhProv;
                instance.DocBuhPck = instance.Operation.DocBuhPck;
                instance.DocBuhNumber = instance.Operation.DocBuhNumber;
                instance.AccRealDebet = instance.Operation.AccRealDebet;
                instance.AccRealCredit = instance.Operation.AccRealCredit;
                instance.AccAVTDebet = instance.Operation.AccAVTDebet;
                instance.AccAVTCredit = instance.Operation.AccAVTCredit;
            }
        }

        public static void AfterChange_CheckNumber(fmIDocCacheKKMPaymentLine instance) {
            Int32 number;
            if (Int32.TryParse(instance.CheckNumber, out number)) {
                instance.DocBuhNumber = number;
            }
        }

        public static void AfterChange_SummAll(fmIDocCacheKKMPaymentLine instance) {
            if (instance.VatRate != null)
                instance.SummVat = csNDSRate.getNDSBack(instance.SummAll, instance.VatRate);
        }
        
        public static void AfterChange_SummVat(fmIDocCacheKKMPaymentLine instance) {
            instance.SummCost = instance.SummAll - instance.SummVat;
        }

        public static void AfterChange_VatRate(fmIDocCacheKKMPaymentLine instance) {
            if (instance.VatRate != null)
                instance.SummVat = csNDSRate.getNDSBack(instance.SummAll, instance.VatRate);
        }
        //public static void AfterConstruction(fmIDocCacheKKMPayment instance) {
        //     The "AfterConstruction" method is executed only once, after an object is created. 
        //     Use this method to initialize new objects with default property values.
        //}
        //public static int SumMethod(fmIDocCacheKKMPayment instance, int val1, int val2) {
        //     You can also define custom methods.
        //    return val1 + val2;
        //}
    }
}
