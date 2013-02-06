using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

using System.Linq;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {
    [NonPersistent]
    public static class FunctionalTaskReference : Object {

        /// <summary>
        /// Заполнение коллекции пользователей из массива
        /// </summary>
        /// <param name="mArray"></param>
        /// <param name="UserAssociationTypeName"></param>
        /// <param name="ual"></param>
        public static void ProcessElements(Array mArray, string UserAssociationTypeName, ref DealWithoutStageTaskInstanceDefinition task, List<AppUser> ual, Session ssn) {
            foreach (string elem in mArray) {
                Array mElems = elem.Split(new string[] { CommonConstants.ElemSeparator }, StringSplitOptions.None);

                if (((string)(mElems.GetValue(0))).Trim() == UserAssociationTypeName) {
                    // Восстанавливаем пользователя по его UserName
                    AppUser au = null;
                    if (!string.IsNullOrEmpty((string)(mElems.GetValue(1)))) {
                        au = UserTaskReference.PopulateUserByUserName((string)(mElems.GetValue(1)), ssn);
                    }
                    if (au != null) {
                        if (ual == null) ual = new List<AppUser>();
                        if (ual.IndexOf(au) == -1) ual.Add(au);
                    }
                }
            }
        }


        public static void ProcessElements(TaskParameters tp, ref DealWithoutStageTaskInstanceDefinition task, Session ssn) {

            foreach (string userName in tp.BusinessAdministrators) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.BusinessAdministrators.IndexOf(au) == -1) au.BaseUserTaskBusinessAdministrators.Add(task);
                }
            }

            foreach (string userName in tp.ExcludedOwners) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.ExcludedOwners.IndexOf(au) == -1) au.BaseUserTaskExcludedOwners.Add(task);
                }
            }

            foreach (string userName in tp.NotificationRecipients) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.NotificationRecipients.IndexOf(au) == -1) au.BaseUserTaskNotificationRecipients.Add(task);
                }
            }

            foreach (string userName in tp.PossibleDelegates) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.PossibleDelegates.IndexOf(au) == -1) au.BaseUserTaskPossibleDelegates.Add(task);
                }
            }

            foreach (string userName in tp.PotentialOwners) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.PotentialOwners.IndexOf(au) == -1) au.BaseUserTaskPotentialOwners.Add(task);
                }
            }

            foreach (string userName in tp.TaskStakeholders) {
                // Восстанавливаем пользователя по его UserName
                AppUser au = UserTaskReference.PopulateUserByUserName(userName, ssn);
                if (au != null) {
                    if (task.TaskStakeholders.IndexOf(au) == -1) au.BaseUserTaskTaskStakeholders.Add(task);
                }
            }

        }

        public static void FillUsersFromAdminTaskCollection(DealWithoutStageTaskInstanceDefinitionAdmin tiid, ref DealWithoutStageTaskInstanceDefinition task) {

            foreach (BaseUserTaskReference butr in tiid.BusinessAdministratorsAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.BusinessAdministrators.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskBusinessAdministrators.Add(task);
                }
            }

            foreach (BaseUserTaskReference butr in tiid.ExcludedOwnersAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.ExcludedOwners.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskExcludedOwners.Add(task);
                }
            }

            foreach (BaseUserTaskReference butr in tiid.NotificationRecipientsAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.NotificationRecipients.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskNotificationRecipients.Add(task);
                }
            }

            foreach (BaseUserTaskReference butr in tiid.PossibleDelegatesAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.PossibleDelegates.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskPossibleDelegates.Add(task);
                }
            }

            foreach (BaseUserTaskReference butr in tiid.PotentialOwnersAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.PotentialOwners.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskPotentialOwners.Add(task);
                }
            }

            foreach (BaseUserTaskReference butr in tiid.TaskStakeholdersAdmin) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    if (AppUserbutr != null & task.TaskStakeholders.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskTaskStakeholders.Add(task);
                }
            }

            // Роли и группы пока не обрабатываем

            //if (butr.GetType() == typeof(RoleTaskReference)) {
            //    RoleTaskReference rolebutr = (RoleTaskReference)butr;
            //    //AppUser AppUserbutr = rolebutr.AppUser;
            //    //if (ual.IndexOf(AppUserbutr) == -1) ual.Add(AppUserbutr);
            //}
            //if (butr.GetType() == typeof(GroupTaskReference)) {
            //    GroupTaskReference groupbutr = (GroupTaskReference)butr;
            //    //AppUser AppUserbutr = userbutr.AppUser;
            //    //if (ual.IndexOf(AppUserbutr) == -1) ual.Add(AppUserbutr);
            //}
        }

    }
}

