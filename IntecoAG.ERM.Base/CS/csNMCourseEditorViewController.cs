using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CS.Nomenclature {
    public partial class csNMCourseEditorViewController : ViewController {

        public csNMCourseEditorViewController() {
            InitializeComponent();
            RegisterActions(components);
        }


        //protected override void OnFrameAssigned() {
        //    DefaultBarActionItemsFactory.CustomizeActionControl += new EventHandler<CustomizeActionControlEventArgs<ActionBase>>(DefaultBarActionItemsFactory_CustomizeActionControl);
        //}

        //void DefaultBarActionItemsFactory_CustomizeActionControl(object sender, CustomizeActionControlEventArgs<ActionBase> e) {
        //    if (e.Action.Id == CourseDateAction.Id) {
        //        BarEditItem barItem = (BarEditItem)e.ActionControl.Control;
        //        RepositoryItemDateEdit repositoryItem = (RepositoryItemDateEdit)barItem.Edit;
        //        repositoryItem.Mask.UseMaskAsDisplayFormat = true;
        //        repositoryItem.Mask.EditMask = "dd.MM.yyyy";
        //    }
        //}


        private void CourseDateAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            if (View == null || View.CurrentObject == null)
                return;
            csCNMCourseEditor current = View.CurrentObject as csCNMCourseEditor;
            if (current == null)
                return;

            if (e.ParameterCurrentValue != null && e.ParameterCurrentValue.ToString() != string.Empty) {
                current.CourseDate =  Convert.ToDateTime(e.ParameterCurrentValue);
                current.FillValutaCourceCollection(current.CourseDate);
                ObjectSpace.CommitChanges();
            }
        }
    }
}
