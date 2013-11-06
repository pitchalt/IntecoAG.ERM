using System;
using System.Collections;
using System.Collections.Generic;
//using System.DirectoryServices;
//using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.CS.Security
{
    public class csCSecurityUserLogic {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="ctx_type"></param>
        /// <param name="server_name"></param>
        /// <param name="container_name"></param>
        /// <param name="group_name"></param>
        static public void UpdateUserListFromActiveDirectoryGroup(IObjectSpace os, ContextType ctx_type, String server_name, String container_name, String group_name)
        {
            PrincipalContext ctx = null;
            //            if (
            if (ctx_type == ContextType.Machine || String.IsNullOrEmpty(container_name)) {
                if (String.IsNullOrEmpty(server_name)) {
                    if (ctx_type == ContextType.Machine)
                        server_name = Environment.MachineName;
                    else  
                        server_name = Environment.UserDomainName;
                }
                ctx = new PrincipalContext(ctx_type, server_name);
            } 
            else {
                ctx = new PrincipalContext(ctx_type, server_name, container_name);
            }
            UpdateUserListFromActiveDirectoryGroup(os, ctx, server_name, group_name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="ctx"></param>
        /// <param name="server_name"></param>
        /// <param name="group_name"></param>
        static public void UpdateUserListFromActiveDirectoryGroup(IObjectSpace os, PrincipalContext ctx, String server_name, String group_name)
        {
            // Create the GroupPrincipal object and set the diplay name property. 
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, group_name);
            UpdateUserListFromActiveDirectoryGroup(os, ctx, server_name, group);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <param name="ctx"></param>
        /// <param name="server_name"></param>
        /// <param name="group"></param>
        static public void UpdateUserListFromActiveDirectoryGroup(IObjectSpace os, PrincipalContext ctx, String server_name, GroupPrincipal group)
        {
            IDictionary<String, csCSecurityUser> os_users = new Dictionary<String, csCSecurityUser>();
            IDictionary<String, Boolean> processed_names = new Dictionary<String, Boolean>();
            foreach (csCSecurityUser os_user in os.GetObjects<csCSecurityUser>(null, true)) {
                String name = os_user.UserName;
                String[] name_parts = name.Split('\\');
                if (name_parts.Length == 2)
                    name = name_parts[1];
                os_users[name] = os_user;
            }
            foreach (UserPrincipal ad_user in group.Members) {
                csCSecurityUser os_user = null;
                if (os_users.ContainsKey(ad_user.SamAccountName))
                    os_user = os_users[ad_user.SamAccountName];
                else {
                    os_user = os.CreateObject<csCSecurityUser>();
                    os_user.UserName = server_name + '\\' + ad_user.SamAccountName;
                    os_user.StaffBuhCode = ad_user.SamAccountName;
                }
                if (!(bool)ad_user.Enabled)
                    os_user.IsActive = false;
                processed_names[ad_user.SamAccountName] = true;
            }
            foreach (String name in os_users.Keys) {
                if (!processed_names.ContainsKey(name)) {
                    os_users[name].IsActive = false;
                }
            }
        }
    }
}
