using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.Module {
    public partial class NestedListViewControllerBase : ViewController, IMasterDetailViewInfo {
        public NestedListViewControllerBase() {
            InitializeComponent();
            RegisterActions(components);

            TargetViewNesting = Nesting.Nested;
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        private string masterDetailViewIdCore = String.Empty;
        public Frame masterDetailViewFrameCore = null;
        
        #region IMasterDetailViewInfo Members
        
        public string MasterDetailViewId {
            get { return masterDetailViewIdCore; }
        }

        public Frame MasterDetailViewFrame {
            get { return masterDetailViewFrameCore; }
        }
 
        public void AssignMasterDetailViewId(string id) {
            masterDetailViewIdCore = id;
        }

        public void AssignMasterDetailViewFrame(Frame frame) {
            masterDetailViewFrameCore = frame;
        }
        
        #endregion

    }
}
