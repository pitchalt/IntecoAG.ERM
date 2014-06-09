using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.FM.FinJurnal;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.CS.Finance;
//
namespace IntecoAG.ERM.FM.Docs.Cache {

    [NavigationItem("Sale")]
    [DomainComponent]
    public interface fmIDocCacheKKMPayment {
        Int32 Month { get; }
        DateTime Date { get; set; }
        String KKMPaymentNumber { get; set; }
        fmCOrder Order { get; set; }

//        IList<csNDSRate> NdsRates { get; }
        IList<fmIDocCacheKKM> KKMs { get; }
        fmCFJSaleOperation Operation { get; set; }

        [Aggregated]
        IList<fmIDocCacheKKMPaymentLine> Lines { get; }
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
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

    [DomainLogic(typeof(fmIDocCacheKKMPayment))]
    public class fmIDocCacheKKMPaymentLogic {
        public static Int32 Get_Month(fmIDocCacheKKMPayment instance) {
            return instance.Date.Year * 100 + instance.Date.Month;
        }

        public static void AfterChange_Order(fmIDocCacheKKMPayment instance) {
            if (instance.Order != null) {
                foreach (fmIDocCacheKKMPaymentLine line in instance.Lines) {
                    line.Order = instance.Order;
                }
            }
        }
        public static void AfterChange_Operation(fmIDocCacheKKMPayment instance) {
            if (instance.Operation != null) {
                foreach (fmIDocCacheKKMPaymentLine line in instance.Lines) {
                    line.Operation = instance.Operation;
                }
            }
        }

        public static void AfterConstruction(fmIDocCacheKKMPayment instance, IObjectSpace os) {
            instance.Date = CommonMethods.DateTimeNow();
            IList<fmIDocCacheKKM> kkms = os.GetObjects<fmIDocCacheKKM>();
            foreach (fmIDocCacheKKM kkm in kkms.Where(x => x.DateFrom <= instance.Date && instance.Date <= x.DateTo)) {
                instance.KKMs.Add(kkm);
            }
            RefreshLines(os, instance);
        }
        public static void RefreshLines(IObjectSpace os, fmIDocCacheKKMPayment instance) {
            IList<csNDSRate> rates = os.GetObjects<csNDSRate>();
            csNDSRate vat18 = rates.FirstOrDefault(x => x.Numerator == 18);
            csNDSRate vat10 = rates.FirstOrDefault(x => x.Numerator == 10);
            foreach (fmIDocCacheKKM kkm in instance.KKMs) {
                fmIDocCacheKKMPaymentLine line = instance.Lines.FirstOrDefault(x => x.KKM == kkm);
                if (line == null) {
                    line = os.CreateObject<fmIDocCacheKKMPaymentLine>();
                    instance.Lines.Add(line);
                    line.KKM = kkm;
                    line.VatRate = vat18;
                    //
                    line = os.CreateObject<fmIDocCacheKKMPaymentLine>();
                    instance.Lines.Add(line);
                    line.KKM = kkm;
                    line.VatRate = vat10;
                }
            }
        }
    }
}
