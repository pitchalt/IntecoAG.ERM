using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
//using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.CS {
    public partial class CommitInfoController : ViewController {
        public CommitInfoController() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
            UnSignEvents();
        }
        protected override void OnViewChanged() {
            base.OnViewChanged();
            SignEvents();
        }

        protected override void OnActivated() {
            base.OnActivated();
            //SignEvents();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            UnSignEvents();
        }

        void SignEvents() {
            if (ObjectSpace != null) {
                ObjectSpace.Committing += new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
                ObjectSpace.Committed += new EventHandler(ObjectSpace_Committed);
            }
        }

        void UnSignEvents() {
            if (ObjectSpace != null) {
                ObjectSpace.Committing -= new EventHandler<CancelEventArgs>(ObjectSpace_Committing);
                ObjectSpace.Committed -= new EventHandler(ObjectSpace_Committed);
            }
        }

        void ObjectSpace_Committing(object sender, CancelEventArgs e) {
            ICommitInfo ci = View.CurrentObject as ICommitInfo;
            if (ci == null) return;
            e.Cancel = ci.OnCommiting();
        }

        void ObjectSpace_Committed(object sender, EventArgs e) {
            ICommitInfo ci = View.CurrentObject as ICommitInfo;
            if (ci == null) return;
            ci.OnCommited();
        }

    }
}
