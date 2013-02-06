using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace IntecoAG.ERM.Module {
    public interface IMasterDetailViewInfo {
        string MasterDetailViewId { get; }
        Frame MasterDetailViewFrame { get; }
        void AssignMasterDetailViewId(string id);
        void AssignMasterDetailViewFrame(Frame frame);
    }
}

