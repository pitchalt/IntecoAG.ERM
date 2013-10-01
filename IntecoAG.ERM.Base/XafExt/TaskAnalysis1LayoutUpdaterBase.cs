using DevExpress.ExpressApp.PivotChart;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.Module {

    public abstract class TaskAnalysis1LayoutUpdaterBase {
        protected abstract IAnalysisControl CreateAnalysisControl();
        protected abstract IPivotGridSettingsStore CreatePivotGridSettingsStore(IAnalysisControl control);
        public void Update(Analysis analysis) {
            if (analysis != null && !PivotGridSettingsHelper.HasPivotGridSettings(analysis)) {
                IAnalysisControl control = CreateAnalysisControl();
                control.DataSource = new AnalysisDataSource(analysis, new XPCollection<crmPaymentPlan>(analysis.Session));
                //Customize the settings of the control's fields (their caption, etc.) as required 
                control.Fields["Priority"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
                control.Fields["Subject"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
                control.Fields["AssignedTo.DisplayName"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
                PivotGridSettingsHelper.SavePivotGridSettings(CreatePivotGridSettingsStore(control), analysis);
                analysis.Save();
            }
        }
    }

}