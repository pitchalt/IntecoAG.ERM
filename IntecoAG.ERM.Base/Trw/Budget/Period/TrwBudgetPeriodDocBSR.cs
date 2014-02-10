using System;
using System.ComponentModel;
using System.IO;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.Trw.Nomenclature;
//
namespace IntecoAG.ERM.Trw.Budget.Period {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class TrwBudgetPeriodDocBSR : TrwBudgetPeriodDoc, csIImportSupport {

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class LineBSR: Line {
            private TrwBudgetPeriodDocBSR _DocBSR;
            [Association("TrwBudgetPeriodDocBSR-TrwBudgetPeriodDocBSRLine")]
            public TrwBudgetPeriodDocBSR DocBSR {
                get { return _DocBSR; }
                set {
                    SetPropertyValue<TrwBudgetPeriodDocBSR>("DocBSR", ref _DocBSR, value);
                    if (!IsLoading) {
                        Doc = value;
                    }
                }
            }
            private String _SaleNomCode;
            public String SaleNomCode {
                get { return _SaleNomCode; }
                set { SetPropertyValue<String>("SaleNomCode", ref _SaleNomCode, value); }
            }
            private TrwSaleNomenclature _SaleNomenclature;
            public TrwSaleNomenclature SaleNomenclature {
                get { return _SaleNomenclature; }
                set { SetPropertyValue<TrwSaleNomenclature>("SaleNomenclature", ref _SaleNomenclature, value); }
            }
            [PersistentAlias("SaleNomenclature.Order")]
            public fmCOrder FmOrder {
                get {
                    return SaleNomenclature != null ? SaleNomenclature.Order : null;
                }
            }
            [PersistentAlias("SaleNomenclature.Order.Subject")]
            public fmCSubject FmSubject {
                get {
                    return SaleNomenclature != null ?
                        SaleNomenclature.Order != null ?
                        SaleNomenclature.Order.Subject : null : null;
                }
            }
            //
            public LineBSR(Session session) : base(session) { }
            public override void AfterConstruction() {
                base.AfterConstruction();
            }
        }

        [Association("TrwBudgetPeriodDocBSR-TrwBudgetPeriodDocBSRLine"), Aggregated]
        public XPCollection<LineBSR> DocBSRLines {
            get { return GetCollection<LineBSR>("DocBSRLines"); }
        }

        public TrwBudgetPeriodDocBSR(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public void Import(IObjectSpace os, String file_name) {
            TrwBudgetPeriodDocBSRLogic.ImportDocBSRLines(this, os, File.OpenText(file_name));
        }
    }

}
