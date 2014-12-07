using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.FinPlan {

    public partial class FmFinPlanDocLineController : ViewController {
        public FmFinPlanDocLineController() {
            InitializeComponent();
            RegisterActions(components);
            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(FmFinPlanDocLine);
        }

        protected NewObjectViewController NewController = null;
        protected DeleteObjectsViewController DeleteController = null;

        protected override void OnActivated() {
            base.OnActivated();
            ListView list_view = View as ListView;
            list_view.CurrentObjectChanged += new EventHandler(list_view_CurrentObjectChanged);
            NewController = Frame.GetController<NewObjectViewController>();
            DeleteController = Frame.GetController<DeleteObjectsViewController>();
        }

        void list_view_CurrentObjectChanged(object sender, EventArgs e) {
//            throw new NotImplementedException();
            ListView list_view = View as ListView;
            FmFinPlanDocLine current = list_view.CurrentObject as FmFinPlanDocLine;
            SetNewState(current);
        }

        private void SetNewState(FmFinPlanDocLine line) {
            if (NewController != null ) {
                NewController.NewObjectAction.Active.SetItemValue(typeof(FmFinPlanDocLineController).FullName, 
                    line != null && (line.SubLines.BindingBehavior & DevExpress.Xpo.CollectionBindingBehavior.AllowNew) != 0 );
                DeleteController.DeleteAction.Active.SetItemValue(typeof(FmFinPlanDocLineController).FullName,
                    line != null && (line.SubLines.BindingBehavior & DevExpress.Xpo.CollectionBindingBehavior.AllowRemove) != 0);
            }
        }

        protected override void OnDeactivated() {
            ListView list_view = View as ListView;
            list_view.CurrentObjectChanged -= new EventHandler(list_view_CurrentObjectChanged);
            NewController = null;
            base.OnDeactivated();
        }
    }
}
