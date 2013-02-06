using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace IntecoAG.ERM.Module.Win
{
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class ERMBaseWinModule : ModuleBase
    {
        public ERMBaseWinModule() {
            InitializeComponent();
        }
    }
}
