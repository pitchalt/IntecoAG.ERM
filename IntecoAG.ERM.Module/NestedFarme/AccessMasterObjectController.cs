using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {
    public partial class AccessMasterObjectController : ViewController<ListView> {

        public AccessMasterObjectController() {
            TargetViewNesting = Nesting.Nested;
            TargetObjectType = typeof(SimpleContractVersion);
        }

        protected override void OnActivated() {
            base.OnActivated();
            if (View.CollectionSource is PropertyCollectionSource) {
                PropertyCollectionSource collectionSource = (PropertyCollectionSource)View.CollectionSource;
                collectionSource.MasterObjectChanged += OnMasterObjectChanged;
                if (collectionSource.MasterObject != null)
                    UpdateMasterObject(collectionSource.MasterObject);
            }
        }

        void UpdateMasterObject(object masterObject) {
            SimpleContract MasterObject = (SimpleContract)masterObject;
            //Use the master object as required            
        }

        void OnMasterObjectChanged(object sender, System.EventArgs e) {
            UpdateMasterObject(((PropertyCollectionSource)sender).MasterObject);
        }

        protected override void OnDeactivated() {
            if (View.CollectionSource is PropertyCollectionSource) {
                PropertyCollectionSource collectionSource = (PropertyCollectionSource)View.CollectionSource;
                collectionSource.MasterObjectChanged -= OnMasterObjectChanged;
            }
            base.OnDeactivated();
        }

    }
}
