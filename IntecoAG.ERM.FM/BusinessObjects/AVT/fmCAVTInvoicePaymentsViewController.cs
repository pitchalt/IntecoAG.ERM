using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.FM.AVT {
    public partial class fmCAVTInvoicePaymentsViewController : ViewController {
        public fmCAVTInvoicePaymentsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private NewObjectViewController _NewCntrl;

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
                _NewCntrl = Frame.GetController<NewObjectViewController>();
                if (_NewCntrl != null)
                    _NewCntrl.CollectDescendantTypes += new EventHandler<CollectTypesEventArgs>(NewCntrl_CollectDescendantTypes);
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (_NewCntrl != null)
                _NewCntrl.CollectDescendantTypes -= new EventHandler<CollectTypesEventArgs>(NewCntrl_CollectDescendantTypes);

        }

        void NewCntrl_CollectDescendantTypes(object sender, CollectTypesEventArgs e) {
            //e.Types.Add(typeof(fmCAVTInvoicePayment));
        }


    }
}
