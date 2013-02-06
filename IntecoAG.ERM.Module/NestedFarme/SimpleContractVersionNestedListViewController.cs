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
    //public partial class SimpleContractVersionNestedListViewController : ViewController {
    //    public SimpleContractVersionNestedListViewController() {
    //        InitializeComponent();
    //        RegisterActions(components);
    //    }
    //}


    public partial class SimpleContractVersionNestedListViewController : NestedListViewControllerBase {
        private SimpleAction saMasterDetailViewInfoAction = null;
        public SimpleContractVersionNestedListViewController() {
            TargetObjectType = typeof(PhoneNumber);
            saMasterDetailViewInfoAction = new SimpleAction(this, "MasterDetailViewInfoAction", DevExpress.Persistent.Base.PredefinedCategory.View);
            saMasterDetailViewInfoAction.Execute += new SimpleActionExecuteEventHandler(saMasterDetailViewInfoAction_Execute);
        }
        void saMasterDetailViewInfoAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            MasterDetailViewInfo();
        }
        private void MasterDetailViewInfo() {
            Console.WriteLine(string.Format("MasterViewId = '{0}'", this.MasterDetailViewId));
        }
        protected override void OnActivated() {
            base.OnActivated();
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }
        void View_ControlsCreated(object sender, EventArgs e) {
            MasterDetailViewInfo();
        }
    }

}
