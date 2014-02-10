using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using FileHelpers;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.FinAccount;
//
namespace IntecoAG.ERM.FM.Docs.Fm {

    public static class FmDocsFmInDataLogic {

        [DelimitedRecord(";")]
        public class InDataImport {
            public String AccountCode;
            public String OrderCode;
            public String CostItemCode;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal Summ;
        }

        public static void ImportInData(FmDocsFmInData doc, IObjectSpace os, TextReader reader) { 
            DelimitedFileEngine engine = new DelimitedFileEngine(typeof(InDataImport));
            engine.Options.IgnoreFirstLines = 1;
            InDataImport[] records = (InDataImport[])engine.ReadStream(reader);
            os.Delete(doc.Lines);
            fmCFAAccountSystem fact_system = os.FindObject<fmCFAAccountSystem>(new BinaryOperator("Code", "1000"));
            IList<fmCostItem> cost_items = os.GetObjects<fmCostItem>();
            foreach (InDataImport rec in records) {
                FmDocsFmInData.Line line = doc.LinesCreate();
                line.FactAccount = fact_system.Accounts.FirstOrDefault(x => x.Code == rec.AccountCode);
                line.FmOrder = os.FindObject<fmCOrder>(new BinaryOperator("Code", rec.OrderCode));
                line.FmCostItem = cost_items.FirstOrDefault(x => x.Code == rec.CostItemCode);
                line.Summ = rec.Summ;
            }
        }
    }

}
