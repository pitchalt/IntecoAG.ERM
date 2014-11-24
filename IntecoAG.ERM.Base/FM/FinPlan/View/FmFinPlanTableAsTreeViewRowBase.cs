using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan.View {

    [NonPersistent]
    public abstract class FmFinPlanTableAsTreeViewRowBase : XPBaseObject, ITreeNode {
        public FmFinPlanTableAsTreeViewRowBase(Session session) : base(session) { }

        public abstract FmFinPlanTableAsTreeView TableAsTreeView { get; set; }
        
        protected String _Code;
        [Browsable(false)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        [Browsable(false)]
        public abstract String CodeFull { get; }

        public abstract ITableView TableView { get; }

        public abstract ITableViewRow TableViewRow { get; set; }

        protected XPCollection<FmFinPlanTableAsTreeViewRow> _Items;
        public virtual XPCollection<FmFinPlanTableAsTreeViewRow> Items {
            get {
                if (_Items == null) {
                    _Items = new XPCollection<FmFinPlanTableAsTreeViewRow>(this.Session);
                    foreach (ITableViewRow row in TableViewRow.Rows) {
                        _Items.Add(
                            new FmFinPlanTableAsTreeViewRow(this.Session) { 
                                TableAsTreeView = TableAsTreeView,
                                UpRowBase = this,
                                TableViewRow = row
                            } );
                    }
                }
                return _Items;
            }
        }


        public virtual IBindingList Children {
            get { return Items; }
        }

        public virtual String Name {
            get { return CodeFull; }
        }

        public abstract ITreeNode Parent { get; }

        public class CellValue: ITableViewCell {
            public CellValue(Decimal value) { RealValue = value; }
            public Decimal RealValue;

            public String Value {
                get { return RealValue.ToString(); }
                set { }
            }

            public Boolean IsReadOnly {
                get { return true; }
            }

            public override String ToString() {
                return Value;
            }
        }

        private ITableViewCell _Col000 = new CellValue(0);
        public ITableViewCell Col000 {
            get { return _Col000; }
            set { _Col000 = value; }
        }
        private ITableViewCell _Col001 = new CellValue(1);
        public ITableViewCell Col001 {
            get { return _Col001; }
            set { _Col001 = value; }
        }
        private ITableViewCell _Col002 = new CellValue(2);
        public ITableViewCell Col002 {
            get { return _Col002; }
            set { _Col002 = value; }
        }
        private ITableViewCell _Col003 = new CellValue(3);
        public ITableViewCell Col003 {
            get { return _Col003; }
            set { _Col003 = value; }
        }
        private ITableViewCell _Col004 = new CellValue(4);
        public ITableViewCell Col004 {
            get { return _Col004; }
            set { _Col004 = value; }
        }
        private ITableViewCell _Col005 = new CellValue(5);
        public ITableViewCell Col005 {
            get { return _Col005; }
            set { _Col005 = value; }
        }
        private ITableViewCell _Col006 = new CellValue(6);
        public ITableViewCell Col006 {
            get { return _Col006; }
            set { _Col006 = value; }
        }
        private ITableViewCell _Col007 = new CellValue(7);
        public ITableViewCell Col007 {
            get { return _Col007; }
            set { _Col007 = value; }
        }
        private ITableViewCell _Col008 = new CellValue(8);
        public ITableViewCell Col008 {
            get { return _Col008; }
            set { _Col008 = value; }
        }
        private ITableViewCell _Col009 = new CellValue(9);
        public ITableViewCell Col009 {
            get { return _Col009; }
            set { _Col009 = value; }
        }
        private ITableViewCell _Col010 = new CellValue(10);
        public ITableViewCell Col010 {
            get { return _Col010; }
            set { _Col009 = value; }
        }

    }

}
