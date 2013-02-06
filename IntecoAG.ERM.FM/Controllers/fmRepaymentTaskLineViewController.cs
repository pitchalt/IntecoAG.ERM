using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.Persistent.Base;
using DevExpress.XtraTreeList;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.XtraEditors.Repository;

using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmRepaymentTaskLineViewController : ObjectViewController {
        public fmRepaymentTaskLineViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            View.AllowEdit.SetItemValue("Info.AllowEdit", true);
        }

        private ObjectTreeList treeList;
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if (View as ListView == null)
                return;
            TreeListEditor treeListEditor = (View as ListView).Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeList = (ObjectTreeList)treeListEditor.TreeList;
                Int32 FieldNumber = 0;
                foreach (RepositoryItem ri in treeList.RepositoryItems) {
                    //if (ri.EditorTypeName == "SingleEdit") {
                    if (FieldNumber == 3) {
                        ri.ReadOnly = false;
                    }
                    FieldNumber++;
                }
                treeList.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeList_CellValueChanged);
                treeList.CellValueChanging += new CellValueChangedEventHandler(treeList_CellValueChanging);
                //treeList.ShownEditor += treeList_ShownEditor;
                treeList.OptionsBehavior.Editable = true;
                treeList.OptionsBehavior.ImmediateEditor = false;
            }
        }
        protected override void OnDeactivated() {
            if (treeList != null) {
                treeList.CellValueChanged -= treeList_CellValueChanged;
                treeList.CellValueChanging -= treeList_CellValueChanging;
                //treeList.ShownEditor -= treeList_ShownEditor;
            }
            base.OnDeactivated();
        }

        /*
        private void treeList_ShownEditor(object sender, EventArgs e) {
            IGridInplaceEdit activeEditor = treeList.ActiveEditor as IGridInplaceEdit;
            if (activeEditor != null && treeList.FocusedObject is IXPSimpleObject) {
                activeEditor.GridEditingObject = treeList.FocusedObject;
            }
        }
        */

        private void treeList_CellValueChanging(object sender, CellValueChangedEventArgs e) {
            // Разрешение редактирования только суммы (3-е поле по счёту, начиная с 0) на уровне 3
            if (e.Node.ParentNode == null) {
                ((ObjectTreeList)sender).CancelCurrentEdit();
            }
            if (e.Node.ParentNode != null && e.Node.ParentNode.ParentNode == null) {
                ((ObjectTreeList)sender).CancelCurrentEdit();
            }
        }

        private void treeList_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            if (e.Column.FieldName == "RequestSum") {
                object newValue = e.Value;
                //if (e.Value is IXPSimpleObject)
                //    newValue = ObjectSpace.GetObject(e.Value);
                ReflectionHelper.SetMemberValue(treeList.FocusedObject, e.Column.FieldName, newValue);
            }
        }
    }
}
