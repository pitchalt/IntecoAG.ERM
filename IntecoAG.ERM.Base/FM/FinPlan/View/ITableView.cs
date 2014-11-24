using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.FM.FinPlan.View {

    public interface ITableViewColumn {
        ITableView Table { get; }
        ITableViewColumn UpColumn { get; }
        IList<ITableViewColumn> Columns { get; }
    }

    public interface ITableViewRow {
        ITableView Table { get; }
        ITableViewRow UpRow { get; }
        IList<ITableViewColumn> Rows { get; }
    }

    public interface ITableViewCell {
        Type GetType();
        String  Value { get; set; }
        Boolean IsReadOnly { get; }
        String  ToString();       
    }

    public interface ITableView : ITableViewColumn, ITableViewRow {

    }

}
