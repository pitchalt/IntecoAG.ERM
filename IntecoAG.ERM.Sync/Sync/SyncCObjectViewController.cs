using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Sync {
    public partial class SyncCObjectViewController : ObjectViewController {
        public SyncCObjectViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

//        protected Boolean _IsActivated = false;

        protected override void OnActivated() {
            base.OnActivated();
//            if (! (this.View.CurrentObject is SyncISyncObject)) return;
//            _IsActivated = true;
            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            SyncActionStateUpdate();
        }

        protected override void OnDeactivated() {
//            if (_IsActivated)
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
//            View.ObjectSpace.Committed -= new EventHandler(ObjectSpace_Committed);
            //            _IsActivated = false;
            base.OnDeactivated();
        }

        void View_CurrentObjectChanged(object sender, EventArgs e) {
            if (!(this.View.CurrentObject is SyncISyncObject)) return;
            //SyncISyncObject obj = (SyncISyncObject)this.View.CurrentObject;
            //obj.IsSyncRequired = true;
            SyncActionStateUpdate();
        }

        private void SyncAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (!(this.View.CurrentObject is SyncISyncObject)) return;
            SyncISyncObject obj = (SyncISyncObject) this.View.CurrentObject;
            this.ObjectSpace.SetModified(obj);
            this.ObjectSpace.CommitChanges();
            SyncActionStateUpdate();
        }

        protected void SyncActionStateUpdate() {
            if (this.View.CurrentObject is SyncISyncObject) {
                SyncAction.Active.SetItemValue("SyncCObjectViewController", true);
//                View.ObjectSpace.Committed +=new EventHandler(ObjectSpace_Committed); 
                SyncActionEnabledUpdate();
            }
            else {
                SyncAction.Active.SetItemValue("SyncCObjectViewController", false);
//                View.ObjectSpace.Committed -= new EventHandler(ObjectSpace_Committed);
            }
        }

        void ObjectSpace_Committed2(object sender, EventArgs e) {
            SyncActionEnabledUpdate();
        }

        protected void SyncActionEnabledUpdate() {
            if (this.View.CurrentObject is SyncISyncObject) {
                SyncISyncObject obj = (SyncISyncObject)this.View.CurrentObject;
                if (obj.IsSyncRequired)
                    SyncAction.Enabled.SetItemValue("SyncCObjectViewController", true);
                else
                    SyncAction.Enabled.SetItemValue("SyncCObjectViewController", false);
            }
        }

    }
}
