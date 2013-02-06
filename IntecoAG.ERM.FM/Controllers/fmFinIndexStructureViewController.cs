using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmFinIndexStructureViewController : ViewController {
        public fmFinIndexStructureViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnViewChanging(View view) {
            if (view.CurrentObject is fmIFinIndexStructure) {
                csIComponent comp = view.CurrentObject as csIComponent;
                if (comp != null && !comp.ReadOnly)
                    base.OnViewChanging(view);
                else
                    this.Active.SetItemValue("fmFinIndexStructureViewController.CompReadOnly", false);
            } else
                this.Active.SetItemValue("fmFinIndexStructureViewController.NotIndexStructure", false);
        }

        private void UpdateFinStructureAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
                using (IObjectSpace os = this.ObjectSpace.CreateNestedObjectSpace()) {
                    fmIFinIndexStructure index_structure = os.GetObject(e.CurrentObject)
                            as fmIFinIndexStructure;
                    IList<fmCFinIndex> index_col = os.GetObjects<fmCFinIndex>();
                    if (index_structure != null) {
                        index_structure.UpdateIndexStructure(index_col);
                        os.CommitChanges();
                    }
                }
        }
    }
}
