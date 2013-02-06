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

namespace IntecoAG.ERM.Module
{
    public partial class ReadOnlyController : ViewController
    {
        public ReadOnlyController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void UpdateViewStateEventHandler(object sender, EventArgs e) {
            View.AllowEdit.SetItemValue("CurrentUser", false);
        }

        protected override void OnActivated() {
            base.OnActivated();
            
            if (View.GetType() == typeof(DashboardView)) {
                return;
            }

            object currentObject = View.CurrentObject;

            string reason = "allowedition";

//            View.AllowEdit.Clear();
//            View.Refresh();

            // Нужные объекты помечены интерфейсом IVersionMainObject


            if (currentObject is IVersionMainObject) {
                IVersionMainObject obj = currentObject as IVersionMainObject;
                if (obj != null) {
                    bool bAllowEdit = false;
                    View.AllowEdit.SetItemValue(reason, bAllowEdit);
                    View.IsRoot = true;
                }
                return;
            }

            if (currentObject is IVersionSupport) {
                IVersionSupport obj = (IVersionSupport)currentObject;
                if (obj != null) {
                    bool bAllowEdit = false;
                    if (obj.VersionState == VersionStates.VERSION_NEW | obj.VersionState == VersionStates.VERSION_PROJECT) {
                        bAllowEdit = true;
                    }

                    View.AllowEdit.SetItemValue(reason, bAllowEdit);
                    //View.Refresh();
                    View.IsRoot = true;
                }
                return;
            }

        }
 
    }
}
