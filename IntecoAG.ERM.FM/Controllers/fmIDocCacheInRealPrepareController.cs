using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.FM.Docs.Cache;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmIDocCacheInRealPrepareController : ViewController {
        public fmIDocCacheInRealPrepareController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ApproveAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            fmIDocCacheInRealPrepare obj = e.CurrentObject as fmIDocCacheInRealPrepare;
            if (obj == null) return;
            this.ObjectSpace.CommitChanges();
            obj.Approve();
            this.ObjectSpace.CommitChanges();
        }
    }
}
