using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
namespace IntecoAG.XafExt.Bpmn {
    public partial class XafExtBpmnAcceptableObjectController : ViewController {

        private static String _ControllerActivateString = typeof(XafExtBpmnAcceptableObjectController).FullName;

        public XafExtBpmnAcceptableObjectController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
        }

        protected override void OnViewChanging(View view) {
            ObjectView obj_view = view as ObjectView;
            if (obj_view != null && obj_view.ObjectTypeInfo.ImplementedInterfaces.Contains(XafTypesInfo.Instance.FindTypeInfo(typeof(XafExtBpmnIAcceptableObject))))
                this.Active[_ControllerActivateString ] = true;
            else
                this.Active[_ControllerActivateString ] = false;
            base.OnViewChanging(view);
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            foreach (XafExtBpmnIAcceptableObject obj in e.SelectedObjects) {
                obj.Accept(ObjectSpace);
            }
            ObjectSpace.CommitChanges();
        }

        private void RejectAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            foreach (XafExtBpmnIAcceptableObject obj in e.SelectedObjects) {
                obj.Reject(ObjectSpace);
            }
            ObjectSpace.CommitChanges();
        }
    }
}
