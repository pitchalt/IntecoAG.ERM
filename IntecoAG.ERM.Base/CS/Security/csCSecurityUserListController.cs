using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS.Security
{
    public partial class csCSecurityUserListController : ViewController
    {
        public csCSecurityUserListController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        private void UpdateListAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                String context_type = ConfigurationManager.AppSettings["SyncUserWithAD.ContextType"];
                String context_name = ConfigurationManager.AppSettings["SyncUserWithAD.ContextName"];
                String container_name = ConfigurationManager.AppSettings["SyncUserWithAD.ContainerName"];
                String group_name = ConfigurationManager.AppSettings["SyncUserWithAD.GroupName"];
                ContextType ctx_type;
                switch (context_type) {
                    case "Machine":
                        ctx_type = ContextType.Machine;
                        break;
                    case "Domain":
                        ctx_type = ContextType.Domain;
                        break;
                    case "ApplicationDirectory":
                        ctx_type = ContextType.ApplicationDirectory;
                        break;
                    default:
                        ctx_type = ContextType.Machine;
                        break;
                }
                csCSecurityUserLogic.UpdateUserListFromActiveDirectoryGroup(os, 
                    ctx_type, 
                    context_name, 
                    container_name, 
                    group_name);
                os.CommitChanges();
            }
            ObjectSpace.CommitChanges();
            ((ListView)View).CollectionSource.Reload();
        }
    }
}
