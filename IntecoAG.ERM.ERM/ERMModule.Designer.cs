namespace IntecoAG.ERM.ERM {
    partial class ERMModule {
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
            // ERMModule
            // 
            this.RequiredModuleTypes.Add(typeof(IntecoAG.ERM.Module.ERMBaseModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.ERM.FM.Win.ERMFinancialWinModule));
            //this.RequiredModuleTypes.Add(typeof(IntecoAG.XAFExt.StateMachine.XAFExtStateMachineModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.XAFExt.CDS.XAFExtCDSModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.ERM.SyncIBS.ERMSyncIBSModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.ERM.Sync.ERMSyncModule));

        }

        #endregion
    }
}