using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using FileHelpers;
//
using DevExpress.ExpressApp;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
//
using IntecoAG.ERM.Trw.Budget.Period;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget {

    public static class TrwBudgetPeriodLogic {

        [DelimitedRecord(";")]
        public class InContractBsrRecord {
            public String TrwSaleNomCode;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period00;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period01;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period02;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period03;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period04;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period05;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period06;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period07;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period08;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period09;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period10;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period11;
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period12;
            [FieldOptional]
            [FieldConverter(ConverterKind.Decimal, ",")]
            public Decimal? Period13;
        }

        public static void ImportInContractBSR(TrwBudgetPeriod period, IObjectSpace os, TextReader reader) {
            DelimitedFileEngine engine = new DelimitedFileEngine(typeof(InContractBsrRecord));
            engine.Options.IgnoreFirstLines = 1;
            InContractBsrRecord[] records = (InContractBsrRecord[])engine.ReadStream(reader);
            os.Delete(period.InContractBSR);
            foreach (InContractBsrRecord rec in records) {
                if (!String.IsNullOrEmpty(rec.TrwSaleNomCode)) {
                    TrwBudgetPeriodInContractBSR in_bsr = os.CreateObject<TrwBudgetPeriodInContractBSR>();
                    period.InContractBSR.Add(in_bsr);
                    in_bsr.SaleNomCode = rec.TrwSaleNomCode.Trim();
                    in_bsr.SaleNomenclature = os.FindObject<TrwSaleNomenclature>(new BinaryOperator("TrwCode", rec.TrwSaleNomCode.Trim()));
//                    if (in_bsr.SaleNomenclature == null)
//                        throw new InvalidDataException("Unknow nomencalture: " + rec.TrwSaleNomCode);
                    in_bsr.Period00 = rec.Period00 != null ? (Decimal)rec.Period00 : 0;
                    in_bsr.Period01 = rec.Period01 != null ? (Decimal)rec.Period01 : 0;
                    in_bsr.Period02 = rec.Period02 != null ? (Decimal)rec.Period02 : 0;
                    in_bsr.Period03 = rec.Period03 != null ? (Decimal)rec.Period03 : 0;
                    in_bsr.Period04 = rec.Period04 != null ? (Decimal)rec.Period04 : 0;
                    in_bsr.Period05 = rec.Period05 != null ? (Decimal)rec.Period05 : 0;
                    in_bsr.Period06 = rec.Period06 != null ? (Decimal)rec.Period06 : 0;
                    in_bsr.Period07 = rec.Period07 != null ? (Decimal)rec.Period07 : 0;
                    in_bsr.Period08 = rec.Period08 != null ? (Decimal)rec.Period08 : 0;
                    in_bsr.Period09 = rec.Period09 != null ? (Decimal)rec.Period09 : 0;
                    in_bsr.Period10 = rec.Period10 != null ? (Decimal)rec.Period10 : 0;
                    in_bsr.Period11 = rec.Period11 != null ? (Decimal)rec.Period11 : 0;
                    in_bsr.Period12 = rec.Period12 != null ? (Decimal)rec.Period12 : 0;
                }
            }
        }
    }
}
