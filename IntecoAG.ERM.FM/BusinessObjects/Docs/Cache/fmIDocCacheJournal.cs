using System;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM.Docs.Cache {
    [DomainComponent]
    public interface fmIDocCacheJournal {
        
        IList<fmIDocCacheJournalLine> Lines { get; }

        [Indexed("DateDelimiter", Unique = true)]
        fmIDocCacheJournalType JournalType { get; set; }

        Int32 DateDelimiter { get; set; }
        Int32 CurrentNumber { get; set; }

        //string PersistentProperty { get; set; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCacheJournal));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCacheJournal))]
    public class fmIDocCacheJournalLogic {
    //    public static string Get_CalculatedProperty(fmIDocCacheJournal instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(fmIDocCacheJournal instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(fmIDocCacheJournal instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
        public static fmIDocCacheJournalLine RegisterDocument(IObjectSpace os_doc, fmIDocCache doc) { //, IObjectSpace os_jur) {
            // You can also define custom methods.
            fmIDocCacheJournalLine line = os_doc.FindObject<fmIDocCacheJournalLine>(new BinaryOperator("Document", doc));
            fmIDocCacheJournal journal = null;
            if (line != null) return line;
            Int32 date_delimiter = 0;
            switch (doc.JournalType) { 
                case fmIDocCacheJournalType.JOURNAL_PREPARE:
                    date_delimiter = doc.DocDate.Year;
                    journal = os_doc.FindObject<fmIDocCacheJournal>(
                        CriteriaOperator.And( new BinaryOperator("JournalType", doc.JournalType), 
                                              new BinaryOperator("DateDelimiter", date_delimiter)));
                    if (journal == null) {
                        journal = os_doc.CreateObject<fmIDocCacheJournal>();
                        journal.JournalType = doc.JournalType;
                        journal.DateDelimiter = date_delimiter;
                    }
                    line = os_doc.CreateObject<fmIDocCacheJournalLine>();
                    journal.Lines.Add(line);
                    journal.CurrentNumber++;
                    line.NumberSequence = journal.CurrentNumber;
                    line.Document = doc;
                    return line;
//                    break;
            }
            return null;
        }
    }
}
