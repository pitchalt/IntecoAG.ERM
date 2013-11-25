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
namespace IntecoAG.ERM.Trw.Subject {
    public partial class TrwSubjectVC : ViewController {
        public TrwSubjectVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ImportSaleDeals_Action_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            TrwSubject subj = e.CurrentObject as TrwSubject;
            if (subj == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                subj = os.GetObject<TrwSubject>(subj);
                foreach (crmContractDeal 
                os.CommitChanges();
            }
        }
    }
}
