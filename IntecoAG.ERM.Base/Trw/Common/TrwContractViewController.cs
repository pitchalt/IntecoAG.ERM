using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.Trw.Contract;

namespace IntecoAG.ERM.Module.Trw.Common {
    public partial class TrwContractViewController : ObjectViewController {
        public TrwContractViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            View.SelectionChanged += new EventHandler(View_SelectionChanged);
            UpdateActionState();
        }

        void View_SelectionChanged(object sender, EventArgs e) {
            UpdateActionState();
        }

        protected override void OnDeactivated() {
            UpdateActionState();
        }

        void UpdateActionState() {
            if (View.SelectedObjects.Count > 0) {
                SendToTrwAction.Enabled.SetItemValue(typeof(TrwContractViewController).Name, true);
            }
            else {
                SendToTrwAction.Enabled.SetItemValue(typeof(TrwContractViewController).Name, false);
            }
        }

        private void SendToTrwAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (e.SelectedObjects.Count < 1) return;
//            using (){
            IObjectSpace os = Application.CreateObjectSpace();
            TrwContractExchangeDoc doc = os.CreateObject<TrwContractExchangeDoc>();
            crmContractDeal deal = null;
            foreach (crmContractDeal sel_deal in e.SelectedObjects) {
                deal = os.GetObject<crmContractDeal>(sel_deal);
                doc.Deals.Add(deal);
            }
            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, doc);
//            }
        }
    }
}
