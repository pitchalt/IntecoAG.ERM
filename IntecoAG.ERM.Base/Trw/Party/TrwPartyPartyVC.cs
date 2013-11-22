using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Party {
    public partial class TrwPartyPartyVC : ObjectViewController {
        public TrwPartyPartyVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void GetNumberAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            TrwPartyParty party = View.CurrentObject as TrwPartyParty;
            if (party == null) return;
//            ObjectSpace.CommitChanges();
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                party = os.GetObject<TrwPartyParty>(party);
                TrwPartyPartyLogic.SetNumbers(os, party, Application.CreateObjectSpace());
                os.CommitChanges();
            }
        }
    }
}
