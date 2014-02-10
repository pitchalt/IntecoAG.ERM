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
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.FinAccount;
//
namespace IntecoAG.ERM.FM.Docs.Fm {

    [Persistent("FmDocsFmInData")]
    [NavigationItem("Finance")]
    public class FmDocsFmInData : csCComponent, csIImportSupport {

        [Persistent("FmDocsFmInDataLine")]
        public class Line : BaseObject {
            [Association("FmDocsFmInData-FmDocsFmInDataLine")]
            public FmDocsFmInData Doc;

            public fmCOrder FmOrder;
            public fmCostItem FmCostItem;
            public fmCFAAccount FactAccount;
            public Decimal Summ;

            public Line(Session session) : base(session) { }
        }

        public fmCFAAccountSystem FactAccountSystem;
        public fmCFAAccountSystem PlanAccountSystem;

        public String Number;
        public DateTime Date;
        public String Name;

        [Association("FmDocsFmInData-FmDocsFmInDataLine"), Aggregated]
        public XPCollection<Line> Lines {
            get {
                return GetCollection<Line>("Lines");
            }
        }
        public Line LinesCreate() { 
            Line line = new Line(this.Session);
            Lines.Add(line);
            return line;
        }

        public FmDocsFmInData(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Date = DateTime.Now;
        }

        protected override void OnDeleting() {
            throw new InvalidOperationException("Delete is not allowed");
        }

        public void Import(IObjectSpace os, string file_name) {
            FmDocsFmInDataLogic.ImportInData(this, os, File.OpenText(file_name));
        }
    }

}
