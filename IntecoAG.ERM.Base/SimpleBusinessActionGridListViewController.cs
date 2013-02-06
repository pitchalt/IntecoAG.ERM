using System;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.Module
{
    /// <summary>
    /// How to add a custom unbound column to the GridControl in ListView to execute a custom business action for a record 
    /// http://www.devexpress.com/Support/Center/e/E1748.aspx
    /// http://documentation.devexpress.com/#Xaf/clsDevExpressPersistentBaseActionAttributetopic
    /// </summary>
    public partial class SimpleBusinessActionGridListViewController : ViewController
    {
        private IModelColumn unboundModelColumn;
        private const string ButtonColumnCaption = "Action";
        private const string ButtonColumnName = "UnboundButtonColumn";
        private RepositoryItemButtonEdit defaultButtonColumnColumnProperties;
        private EditorButton activebutton = new EditorButton(ButtonPredefines.OK);
        private EditorButton inactivebutton = new EditorButton(ButtonPredefines.Close);

        private GridListEditor gridListEditor;
        
        public SimpleBusinessActionGridListViewController() {
            this.TargetViewType = ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView); ;
            this.TargetObjectType = typeof(crmContractDeal);
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            ListView lv = View as ListView;
            if (lv == null) return;

            // ========== ВНИМАНИЕ! ЗДЕСЬ ОТКЛЮЧАЕМ ДЕЙСТВИЕ ПО СОЗДАНИЮ КОЛОНКИ В Grid ==========
            //
            return;
            //
            // ===================================================================================


            //gridListEditor = lv.Editor as GridListEditor;
            //if (gridListEditor != null) {
            //    InitGridView();
            //    InitButtonColumn();
            //}
        }

        private void InitGridView() {
            gridListEditor.GridView.CustomRowCellEdit += gridView_CustomRowCellEdit;
            gridListEditor.GridView.CustomRowCellEditForEditing += gridView_CustomRowCellEdit;
            gridListEditor.GridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
        }

        private void InitButtonColumn() {
            unboundModelColumn = gridListEditor.Model.Columns.GetNode<IModelColumn>(ButtonColumnName);
            if (unboundModelColumn == null) {
                unboundModelColumn = gridListEditor.Model.Columns.AddNode<IModelColumn>(ButtonColumnName);
                unboundModelColumn.PropertyName = ButtonColumnName;
                unboundModelColumn.PropertyEditorType = typeof(DefaultPropertyEditor);
                for (int i = gridListEditor.Columns.Count - 1; i >= 0; i--) {
                    ColumnWrapper cw = gridListEditor.Columns[i];
                    if (cw.PropertyName == unboundModelColumn.PropertyName) {
                        gridListEditor.RemoveColumn(cw);
                        break;
                    }
                }
                gridListEditor.AddColumn(unboundModelColumn);
            }
            GridColumn buttonColumn = gridListEditor.GridView.Columns[unboundModelColumn.PropertyName];
            if (buttonColumn != null) {
                buttonColumn.UnboundType = UnboundColumnType.Boolean;
                buttonColumn.Caption = ButtonColumnCaption;
                buttonColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                buttonColumn.VisibleIndex = 0;
                buttonColumn.Width = 50;
                buttonColumn.OptionsColumn.AllowEdit = true;
                buttonColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
                buttonColumn.OptionsColumn.AllowMove = false;
                buttonColumn.OptionsColumn.AllowShowHide = false;
                buttonColumn.OptionsColumn.AllowSize = false;
                buttonColumn.OptionsColumn.AllowSort = DefaultBoolean.False;
                buttonColumn.OptionsColumn.FixedWidth = true;
                buttonColumn.OptionsColumn.ShowInCustomizationForm = false;
                buttonColumn.OptionsFilter.AllowFilter = false;
                InitButtonEditor();
            }
        }

        private void InitButtonEditor() {
            defaultButtonColumnColumnProperties = new RepositoryItemButtonEdit();
            defaultButtonColumnColumnProperties.TextEditStyle = TextEditStyles.HideTextEditor;
            defaultButtonColumnColumnProperties.Click += buttonColumnColumnProperties_Click;
            gridListEditor.GridView.GridControl.RepositoryItems.Add(defaultButtonColumnColumnProperties);
        }

        private void UpdateButtons(RepositoryItemButtonEdit properties, bool active) {
            EditorButton button = active ? inactivebutton : activebutton;
            if (properties.Buttons[0].Kind != button.Kind) {
                properties.BeginInit();
                properties.Buttons.Clear();
                properties.Buttons.Add(button);
                properties.EndInit();
            }
        }

        private void gridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
            if (e.Column.FieldName == ButtonColumnName) {
                crmContractDeal wp = gridListEditor.GridView.GetRow(e.RowHandle) as crmContractDeal;
                if (wp != null) {
                    RepositoryItemButtonEdit item = defaultButtonColumnColumnProperties.Clone() as RepositoryItemButtonEdit;
                    UpdateButtons(item, true);   //wp.Active);
                    e.RepositoryItem = item;
                }
            }
        }

        private void buttonColumnColumnProperties_Click(object sender, EventArgs e) {
            ButtonEdit editor = (ButtonEdit)sender;
            crmContractDeal wp = gridListEditor.GridView.GetFocusedRow() as crmContractDeal;
            if (wp != null) {
                // Выполняем какое-либо действие, например, метод бизнес-объекта
                //wp.SimpleBusinessAction();

                UpdateButtons(editor.Properties, true);   //wp.Active);
            }
        }

        protected override void OnDeactivated() {
            if (gridListEditor != null && gridListEditor.GridView != null) {
                gridListEditor.GridView.CustomRowCellEdit -= gridView_CustomRowCellEdit;
                gridListEditor.GridView.CustomRowCellEditForEditing -= gridView_CustomRowCellEdit;
                if (unboundModelColumn != null)
                    ((IModelColumns)unboundModelColumn.Parent).Remove(unboundModelColumn);
            }
            if (defaultButtonColumnColumnProperties != null)
                defaultButtonColumnColumnProperties.Click -= buttonColumnColumnProperties_Click;
            base.OnDeactivated();
        }
    }
}
