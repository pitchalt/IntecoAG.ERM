using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.Docs.Cache {

    public enum fmIDocCacheInRealPrepareStatus {
        CREATED = 1,
        PREPARED = 2,
        PAYED = 3,
        SALE_COMPLETE = 4
    }

    [DomainComponent]
    [NavigationItem("Sale")]
    [Appearance("", AppearanceItemType.ViewItem, "Status != 'CREATED'", Enabled = false, TargetItems="DocNumber;DocDate")]
    public interface fmIDocCacheInRealPrepare : fmIDocCacheIn {

        fmIDocCacheInRealPrepareStatus Status { get; set; } 

        [BackReferenceProperty("LinkedCacheIn")]
        fmIDocCacheInRealFinal DocCacheInReal { get; set; }

        [FieldSize(6)]
        String ChequeNumber { get; set; }

        [RuleRequiredField]
        fmCOrder Order { get; set; }

        Decimal AVTSumma { get; set; }
        [RuleRequiredField]
        csNDSRate AVTRate { get; set; }

        [Aggregated]
        IList<fmIDocCacheInRealPrepareLine> PrepareLines { get; }

        void UpdateAVTSumma();

        void Approve();
        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheInRealPrepare));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheInRealPrepare))]
    public class fmIDocCacheInRealPrepareLogic {
        public static void AfterConstruction(fmIDocCacheInRealPrepare instance, IObjectSpace os) {
            // The "AfterConstruction" method is executed only once, after an object is created. 
            // Use this method to initialize new objects with default property values.
            instance.JournalType = fmIDocCacheJournalType.JOURNAL_PREPARE;
            instance.Status = fmIDocCacheInRealPrepareStatus.CREATED;
            instance.PayPartyType = fmIDocCachePartyType.PARTY_PARTY;
            instance.PrepareLines.Add(os.CreateObject<fmIDocCacheInRealPrepareLine>());
        }

        //public static void OnSaving(fmIDocCacheInRealPrepare instance) { 
        //}

        public static IList<fmIDocCacheLine> Get_Lines(fmIDocCacheInRealPrepare instance) {
            return new ListConverter<fmIDocCacheLine, fmIDocCacheInRealPrepareLine>(instance.PrepareLines); 
        }
//
        public static void AfterChange_Summa(fmIDocCacheInRealPrepare instance) {
            if (instance.AVTRate != null) {
                instance.UpdateAVTSumma();
//                instance.AVTSumma = csNDSRate.getNDS(instance.Summa, instance.AVTRate);
            }
        }
        public static void AfterChange_AVTRate(fmIDocCacheInRealPrepare instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            if (instance.AVTRate != null) {
                instance.UpdateAVTSumma();
//                instance.AVTSumma = csNDSRate.getNDS(instance.Summa, instance.AVTRate);
            }
            foreach (var line in instance.PrepareLines) {
//                if (line.AVTRate == null)
                line.AVTRate = instance.AVTRate;
            }
        }
        public static void AfterChange_Order(fmIDocCacheInRealPrepare instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
            foreach (var line in instance.PrepareLines) {
                //                if (line.AVTRate == null)
                line.Order = instance.Order;
            }
        }
        public static void AfterChange_AVTSumma(fmIDocCacheInRealPrepare instance) {
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
        public static void UpdateAVTSumma(fmIDocCacheInRealPrepare instance) {
            if (instance.AVTRate != null) {
                instance.AVTSumma = csNDSRate.getNDSBack(instance.Summa, instance.AVTRate);
            }
        }

        public static void Approve(fmIDocCacheInRealPrepare instance, IObjectSpace os) {
            switch (instance.Status) {
                case fmIDocCacheInRealPrepareStatus.CREATED:
                    fmIDocCacheJournalLine line = fmIDocCacheJournalLogic.RegisterDocument(os, instance);
                    instance.DocNumber = line.NumberSequence.ToString("D6");
                    instance.Status = fmIDocCacheInRealPrepareStatus.PREPARED;
                    break;
                case fmIDocCacheInRealPrepareStatus.PREPARED:
                    break;
                case fmIDocCacheInRealPrepareStatus.PAYED:
                    break;
                default:
                    break;
            }
        }
    }
}
