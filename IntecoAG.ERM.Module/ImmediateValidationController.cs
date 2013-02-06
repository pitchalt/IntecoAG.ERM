using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Validation;
using System.Collections;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;


namespace IntecoAG.ERM.Module {
    
    public class ImmediateValidationTargetObjectsSelector : ValidationTargetObjectSelector {
        protected override bool NeedToValidateObject(Session session, object targetObject) {
            return true;
        }
    }

    public partial class ImmediateValidationController : ViewController {

        private string validationContext = "Immediate";

        private void ValidateObjects(IEnumerable targets) {
            RuleSetValidationResult result = Validator.RuleSet.ValidateAllTargets(targets);   //, validationContext);

            List<ResultsHighlightController> resultsHighlightControllers = new List<ResultsHighlightController>();
            resultsHighlightControllers.Add(Frame.GetController<ResultsHighlightController>());
            if (View is DetailView) {
                foreach (ListPropertyEditor listPropertyEditor in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                    //if (listPropertyEditor.Frame.GetController<ResultsHighlightController>() == null) continue;
                    if (listPropertyEditor.Frame == null) continue;
                    ResultsHighlightController nestedController = listPropertyEditor.Frame.GetController<ResultsHighlightController>();
                    if (nestedController != null) {
                        resultsHighlightControllers.Add(nestedController);
                    }
                }
            }

            foreach (ResultsHighlightController resultsHighlightController in resultsHighlightControllers) {
                //resultsHighlightController.View.ObjectSpace.ModifiedObjects[0]
                resultsHighlightController.ClearHighlighting();
                if (result.State == ValidationState.Invalid) {
                    resultsHighlightController.HighlightResults(result);
                }
            }
        }

        private void ValidateViewObjects() {
            if (View != null) {
                if (View is ListView) {
                    if (Frame != null && Frame.IsViewControllersActivation) {
                        ValidateObjects(((ListView)View).CollectionSource.List);
                    }
                }
                else {
                    ImmediateValidationTargetObjectsSelector objectsSelector = new ImmediateValidationTargetObjectsSelector();
                    ValidateObjects(objectsSelector.GetObjectsToValidate(((ObjectSpace)View.ObjectSpace).Session, View.CurrentObject));
                }
            }        
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            ValidateViewObjects();
        }

        private void ObjectSpace_ObjectEndEdit(object sender, EventArgs e) {
            ValidateViewObjects();
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            View.ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            //View.ObjectSpace.ObjectEndEdit += new EventHandler<EndEditEventArgs>(ObjectSpace_ObjectEndEdit);
            if (View is DetailView) {
                View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
                //View.SelectionChanged += new EventHandler(View_CurrentObjectChanged);
            }
            ValidateViewObjects();
        }

        protected override void OnDeactivated() {
            View.ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            //View.ObjectSpace.ObjectEndEdit -= new EventHandler<EndEditEventArgs>(ObjectSpace_ObjectEndEdit);
            if (View is DetailView) {
                View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
                //View.SelectionChanged -= new EventHandler(View_CurrentObjectChanged);
            }
            base.OnDeactivated();
        }

        void View_CurrentObjectChanged(object sender, EventArgs e) {
            ValidateViewObjects();
        }

        public ImmediateValidationController() {
            TargetViewType = DevExpress.ExpressApp.ViewType.Any;
            TargetViewNesting = Nesting.Any;
        }

    }
}

