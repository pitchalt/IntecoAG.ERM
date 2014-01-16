using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw.Subject {

    public partial class TrwSubjectDealVC : ObjectViewController {
        public TrwSubjectDealVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            UpdateSubscribe();
            base.OnActivated();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (_Deal != null)
                _Deal.Changed -= new DevExpress.Xpo.ObjectChangeEventHandler(Deal_Changed);
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
        }

        void View_CurrentObjectChanged(object sender, EventArgs e) {
            UpdateSubscribe();
        }

        private TrwSubjectDealBase _Deal;

        private void UpdateSubscribe() {
            if (_Deal != null)
                _Deal.Changed -= new DevExpress.Xpo.ObjectChangeEventHandler(Deal_Changed);
            _Deal = View.CurrentObject as TrwSubjectDealBase;
            if (_Deal != null)
                _Deal.Changed += new DevExpress.Xpo.ObjectChangeEventHandler(Deal_Changed);
        }

        void Deal_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e) {
            if (e.PropertyName == "Nomenclatura")
                System.Console.WriteLine(
                    "DealChange: " + e.PropertyName + 
                    " Old: " + e.OldValue.ToString() +
                    " New: " + e.NewValue.ToString());
        }
    }
}
