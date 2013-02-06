namespace IntecoAG.ERM.Module
{
    partial class ERMBaseModule
    {
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
            // 
            // ERMBaseModule
            // 
            this.AdditionalBusinessClasses.Add(typeof(DevExpress.Persistent.BaseImpl.Analysis));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotChart.Win.PivotChartWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.Win.PivotGridWindowsFormsModule));
            // Using persistent objects of Assembly IntecoAG.ERM
            this.AdditionalBusinessClassAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(global::IntecoAG.ERM.CS.TreeStructure)));
            // Using persistent objects of Assembly DevExpress.ExpressApp.Validation.v10.2
            this.AdditionalBusinessClassAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(global::DevExpress.ExpressApp.Validation.AllContextsView.DisplayableValidationResultItem)));

        }

        #endregion
    }
}
