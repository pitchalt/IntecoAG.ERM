using System;
using System.ComponentModel;
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
//
namespace IntecoAG.ERM.Trw.Budget.Period {

    [Persistent("TrwBudgetPeriodDoc")]
    public abstract class TrwBudgetPeriodDoc : csCComponent {

        [Association("TrwBudgetPeriod-TrwBudgetPeriodDoc")]
        public TrwBudgetPeriod Period;

        [Persistent("TrwBudgetPeriodDocLine")]
        public class Line: BaseObject {
            [Association("TrwBudgetPeriodDoc-TrwBudgetPeriodDocLine")]
            public TrwBudgetPeriodDoc Doc;

            public Decimal Period00;
            public Decimal Period01;
            public Decimal Period02;
            public Decimal Period03;
            public Decimal Period04;
            public Decimal Period05;
            public Decimal Period06;
            public Decimal Period07;
            public Decimal Period08;
            public Decimal Period09;
            public Decimal Period10;
            public Decimal Period11;
            public Decimal Period12;
            public Decimal Period13;

            
            public Decimal this[Int32 index] {
                get {
                    switch (index) { 
                        case 0:
                            return Period00;
                        case 1:
                            return Period01;
                        case 2:
                            return Period02;
                        case 3:
                            return Period03;
                        case 4:
                            return Period04;
                        case 5:
                            return Period05;
                        case 6:
                            return Period06;
                        case 7:
                            return Period07;
                        case 8:
                            return Period08;
                        case 9:
                            return Period09;
                        case 10:
                            return Period10;
                        case 11:
                            return Period11;
                        case 12:
                            return Period12;
                        case 13:
                            return Period13;
                        default:
                            throw new IndexOutOfRangeException("Index: " + index.ToString());
                    }
                }
                set {
                    switch (index) { 
                        case 0:
                            Period00 = value;
                            break;
                        case 1:
                            Period01 = value;
                            break;
                        case 2:
                            Period02 = value;
                            break;
                        case 3:
                            Period03 = value;
                            break;
                        case 4:
                            Period04 = value;
                            break;
                        case 5:
                            Period05 = value;
                            break;
                        case 6:
                            Period06 = value;
                            break;
                        case 7:
                            Period07 = value;
                            break;
                        case 8:
                            Period08 = value;
                            break;
                        case 9:
                            Period09 = value;
                            break;
                        case 10:
                            Period10 = value;
                            break;
                        case 11:
                            Period11 = value;
                            break;
                        case 12:
                            Period12 = value;
                            break;
                        case 13:
                            Period13 = value;
                            break;
                        default:
                            throw new IndexOutOfRangeException("Index: " + index.ToString());
                    }
                }
            }   

            public Line(Session session) : base(session) { }
            public override void AfterConstruction() {
                base.AfterConstruction();
            }
        }

        public String Name;
        public String Number;
        public DateTime Date;

        [Association("TrwBudgetPeriodDoc-TrwBudgetPeriodDocLine"), Aggregated]
        public XPCollection<Line> Lines {
            get {
                return GetCollection<Line>("Lines");
            }
        }

        public TrwBudgetPeriodDoc(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Date = DateTime.Now;
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }
    }

}
