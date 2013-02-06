using DevExpress.ExpressApp.PivotChart;
using DevExpress.ExpressApp.PivotChart.Win;

using IntecoAG.ERM.Module;

namespace IntecoAG.ERM.Module.Win {

    public class TaskAnalysis1LayoutUpdater : TaskAnalysis1LayoutUpdaterBase {
       protected override IAnalysisControl CreateAnalysisControl() {
          return new AnalysisControlWin();
       }
       protected override DevExpress.Persistent.Base.IPivotGridSettingsStore CreatePivotGridSettingsStore(IAnalysisControl control) {
          return new PivotGridControlSettingsStore(((AnalysisControlWin)control).PivotGrid);
       }
    }

}
