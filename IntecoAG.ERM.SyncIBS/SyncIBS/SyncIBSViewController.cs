using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Numeric;
using System.Text;

using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.FM.FinJurnal;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.SyncIBS {
    
    public partial class SyncIBSViewController : ViewController {

        public SyncIBSViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
        }

        private void SyncIBSAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (e.CurrentObject == null) return;

            Type ot = e.CurrentObject.GetType();
            if (ot == typeof(fmCFJSaleDoc)) {
                fmCFJSaleDoc doc = (fmCFJSaleDoc) e.CurrentObject;
                ObjectSpace.CommitChanges();
                SyncIBSExchangeLogic.ExportTo(this.ObjectSpace, doc);
            }
        }

        //private void OrderCatalog_Execute(object sender, SimpleActionExecuteEventArgs e) {
        //    foreach (var item in OrderExchangeLogic.Catalog(this.ObjectSpace)) {
        //        Debug.WriteLine(item.UserOrgCode + " - " + item.SubjectCode + " - " + item.Code + " - " + 
        //                    item.DateOpen.ToString() + " - " + item.DateClose.ToString());
        //    }
        //}
    }
}
