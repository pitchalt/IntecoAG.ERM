using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Subject {
    public partial class TrwSubjectDealVC : ViewController {
        public TrwSubjectDealVC() {
            InitializeComponent();
            RegisterActions(components);
        }
    }
}
