using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.SystemModule;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Module {

    public partial class ValidateDetailViewController : ViewController {
        WinDetailViewController controller = null;
        object previouslySelectedObject = null;
        public ValidateDetailViewController() {
            TargetObjectType = typeof(AAA);
        }

        protected override void OnActivated() {
            base.OnActivated();
            controller = Frame.GetController<WinDetailViewController>();
            // Prevents asking about saving. 
            controller.SuppressConfirmation = true;
            if (View is DetailView && View.IsRoot) {
                // Raises when closing the detail view.
                ((WinWindow)Frame).Closing += new CancelEventHandler(WindowClosing_EventHandler);
                ObjectSpace.Refreshing += new EventHandler<CancelEventArgs>(ObjectSpace_Refreshing);
            }
            if (View.IsRoot) {
                View.CurrentObjectChanged += new EventHandler(CurrentObjectChanged_EventHandler);
            }
            previouslySelectedObject = View.CurrentObject;
        }

        bool IsRefreshing = false;
        void ObjectSpace_Refreshing(object sender, CancelEventArgs e) {
            IsRefreshing = true;
        }

        void CurrentObjectChanged_EventHandler(object sender, EventArgs e) {
            if (!IsRefreshing && previouslySelectedObject != null && !ReferenceEquals(View.CurrentObject, previouslySelectedObject)) {
                if (!InternalIsValid(previouslySelectedObject)) {
                    View.CurrentObject = View.ObjectSpace.GetObject(previouslySelectedObject);
                    InternalSave();
                    return;
                };
            }
            previouslySelectedObject = View.CurrentObject;
            IsRefreshing = false;
        }

        void WindowClosing_EventHandler(object sender, CancelEventArgs e) {
            InternalSafe(e);
        }

        private bool InternalIsValid(object obj) {
            return Validator.RuleSet.ValidateTarget(obj, new ContextIdentifiers("Save")).State == ValidationState.Valid;
        }

        private void InternalSafe(CancelEventArgs e) {
            bool failed = !InternalIsValid(View.CurrentObject);
            InternalSave();
            // Cancels the current action.
            e.Cancel = failed;
        }

        private void InternalSave() {
            // Fires in the root detail view.
            if (controller.SaveAction.Active.ResultValue && controller.SaveAction.Enabled.ResultValue) {
                controller.SaveAction.DoExecute();
            }
            else {
                // Fires in the list view.
                if (ObjectSpace.IsModified) {
                    ObjectSpace.CommitChanges();
                }
            }
        }

    }
}

