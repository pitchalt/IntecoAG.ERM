namespace IntecoAG.ERM.Module {
    partial class CheckChoiceActionForPivotGridController {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.singleChoiceAction1 = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // singleChoiceAction1
            // 
            this.singleChoiceAction1.Caption = "CheckChoiceActionForPivotGrid";
            this.singleChoiceAction1.Category = "FullTextSearch";
            this.singleChoiceAction1.ConfirmationMessage = null;
            this.singleChoiceAction1.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            this.singleChoiceAction1.Id = "5e42e0d9-e102-4be1-92ec-868157ebb14e";
            this.singleChoiceAction1.ImageMode = DevExpress.ExpressApp.Actions.ImageMode.UseItemImage;
            this.singleChoiceAction1.ImageName = "kappfinder_16x16.png";
            choiceActionItem1.Caption = "Entry 1";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem2.Caption = "Entry 2";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem3.Caption = "Entry 3";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            this.singleChoiceAction1.Items.Add(choiceActionItem1);
            this.singleChoiceAction1.Items.Add(choiceActionItem2);
            this.singleChoiceAction1.Items.Add(choiceActionItem3);
            this.singleChoiceAction1.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.singleChoiceAction1.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.singleChoiceAction1.Shortcut = null;
            this.singleChoiceAction1.Tag = null;
            this.singleChoiceAction1.TargetObjectsCriteria = null;
            this.singleChoiceAction1.TargetViewId = null;
            this.singleChoiceAction1.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.singleChoiceAction1.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.singleChoiceAction1.ToolTip = "Поиск";
            this.singleChoiceAction1.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.singleChoiceAction1.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.singleChoiceAction1_Execute);
            // 
            // CheckChoiceActionForPivotGridController
            // 
            this.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction1;
    }
}
