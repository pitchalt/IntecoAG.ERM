using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM.Docs.Cache {

    [DomainComponent]
    public interface fmIDocCacheOutCommon : fmIDocCacheOut {
        [RuleRequiredField]
        fmCOrder Order { get; set; }

        Decimal AVTSumma { get; set; }
        [RuleRequiredField]
        csNDSRate AVTRate { get; set; }

        [Aggregated]
        IList<fmIDocCacheOutCommonLine> CommonLines { get; }

        void UpdateAVTSumma();

        void Approve();
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

    [DomainLogic(typeof(fmIDocCacheOutCommon))]
    public class fmIDocCacheOutCommonLogic {
        public static void AfterConstruction(fmIDocCacheOutCommon instance, IObjectSpace os) {
            // The "AfterConstruction" method is executed only once, after an object is created. 
            // Use this method to initialize new objects with default property values.
            instance.JournalType = fmIDocCacheJournalType.JOURNAL_PREPARE;
//            instance.Status = fmIDocCacheInRealPrepareStatus.CREATED;
            instance.PayPartyType = fmIDocCachePartyType.PARTY_PARTY;
            instance.CommonLines.Add(os.CreateObject<fmIDocCacheOutCommonLine>());
        }

        //public static void OnSaving(fmIDocCacheInRealPrepare instance) { 
        //}

        public static IList<fmIDocCacheLine> Get_Lines(fmIDocCacheOutCommon instance) {
            return new ListConverter<fmIDocCacheLine, fmIDocCacheOutCommonLine>(instance.CommonLines); 
        }
//
        public static void AfterChange_Summa(fmIDocCacheOutCommon instance) {
            if (instance.AVTRate != null) {
                instance.UpdateAVTSumma();
//                instance.AVTSumma = csNDSRate.getNDS(instance.Summa, instance.AVTRate);
            }
        }
        public static void AfterChange_AVTRate(fmIDocCacheOutCommon instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.AVTRate != null) {
                instance.UpdateAVTSumma();
//                instance.AVTSumma = csNDSRate.getNDS(instance.Summa, instance.AVTRate);
            }
            foreach (var line in instance.CommonLines) {
//                if (line_doc.AVTRate == null)
                line.AVTRate = instance.AVTRate;
            }
        }
        public static void AfterChange_Order(fmIDocCacheOutCommon instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            foreach (var line in instance.CommonLines) {
                //                if (line_doc.AVTRate == null)
                line.Order = instance.Order;
            }
        }
        public static void AfterChange_AVTSumma(fmIDocCacheOutCommon instance) {
            if (instance.AVTRate != null) {
                if (instance.AVTRate.Numerator != 0) {
                    instance.DescriptionContent = instance.AVTRate.Name + " = " + instance.AVTSumma.ToString("N2");
                }
                else {
                    instance.DescriptionContent = instance.AVTRate.Name;
                }
            }
        }
    //    public static int SumMethod(fmIDocCacheInRealPrepare instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
        public static void UpdateAVTSumma(fmIDocCacheOutCommon instance) {
            if (instance.AVTRate != null) {
                instance.AVTSumma = csNDSRate.getNDSBack(instance.Summa, instance.AVTRate);
            }
        }

        public static void Approve(fmIDocCacheOutCommon instance, IObjectSpace os) {
            //switch (instance.Status) {
            //    case fmIDocCacheInRealPrepareStatus.CREATED:
            //        fmIDocCacheJournalLine line_doc = fmIDocCacheJournalLogic.RegisterDocument(os, instance);
            //        instance.DocNumber = line_doc.NumberSequence.ToString("D6");
            //        instance.Status = fmIDocCacheInRealPrepareStatus.PREPARED;
            //        break;
            //    case fmIDocCacheInRealPrepareStatus.PREPARED:
            //        break;
            //    case fmIDocCacheInRealPrepareStatus.PAYED:
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
