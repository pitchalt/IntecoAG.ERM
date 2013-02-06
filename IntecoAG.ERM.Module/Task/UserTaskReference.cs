using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {
    //[DefaultClassOptions]
    [Persistent]
    public partial class UserTaskReference : BaseUserTaskReference {
        public UserTaskReference(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            UserAssociationType = Module.UserAssociationType.user;
        }


        private AppUser _AppUser;
        public AppUser AppUser {
            get { return _AppUser; }
            set { SetPropertyValue<AppUser>("AppUser", ref _AppUser, value); }
        }

        /// <summary>
        /// Результат - список AppUser, вычисленный по параметрам.
        /// Из Butrs пользователи собираются по спискам самих пользователей, по ролям и по группам
        /// Из userNameList пользователи воссоздаются по их логинам (UserName)
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public override List<AppUser> GetUserList(IList<BaseUserTaskReference> Butrs, IList<string> userNameList) {
            List<AppUser> Res = new List<AppUser>();
            foreach (BaseUserTaskReference butr in Butrs) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) Res.Add(userbutr.AppUser);
            }

            foreach (string username in userNameList) {
                AppUser appUser = PopulateUserByUserName(username, this.Session);
                if (appUser != null) {
                    if (Res.IndexOf(appUser) == -1) Res.Add(appUser);
                }
            }

            return Res;
        }

        /// <summary>
        /// Определение пользователя типа AppUser по его UserName
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AppUser PopulateUserByUserName(string userID, Session ssn) {
            XPCollection<AppUser> appUsers = new XPCollection<AppUser>(ssn);
            if (!appUsers.IsLoaded) appUsers.Load();
            var lau = (from appuser in appUsers where appuser.UserName == userID.Trim() select appuser);

            IEnumerator enumlau = lau.GetEnumerator();
            while (enumlau.MoveNext()) {
                return (AppUser)enumlau.Current;
            }

            return null;
        }


        /// <summary>
        /// Собирает пользователей из всех соответствующих подсписков коллекции BaseUserTaskReference и добавляет их к 
        /// соответствующему списку пользователей задачи
        /// Сбор пользвателей из ролей и групп пока не реализован!
        /// </summary>
        /// <param name="Butrs"></param>
        /// <param name="ual"></param>
        public static void FillUsersFromAdminTaskCollection(XPCollection<BaseUserTaskReference> Butrs, ref DealWithoutStageTaskInstanceDefinition task, UserListType userListType) {
            foreach (BaseUserTaskReference butr in Butrs) {
                UserTaskReference userbutr = butr as UserTaskReference;
                if (userbutr != null) {
                    AppUser AppUserbutr = userbutr.AppUser;
                    switch (userListType) {
                        case UserListType.BusinessAdministrators:
                            if (AppUserbutr != null & task.BusinessAdministrators.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskBusinessAdministrators.Add(task);
                            break;
                        case UserListType.ExcludedOwners:
                            if (AppUserbutr != null & task.ExcludedOwners.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskExcludedOwners.Add(task);
                            break;
                        case UserListType.NotificationRecipients:
                            if (AppUserbutr != null & task.NotificationRecipients.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskNotificationRecipients.Add(task);
                            break;
                        case UserListType.PossibleDelegates:
                            if (AppUserbutr != null & task.PossibleDelegates.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskPossibleDelegates.Add(task);
                            break;
                        case UserListType.PotentialOwners:
                            if (AppUserbutr != null & task.PotentialOwners.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskPotentialOwners.Add(task);
                            break;
                        case UserListType.TaskStakeholders:
                            if (AppUserbutr != null & task.TaskStakeholders.IndexOf(AppUserbutr) == -1) AppUserbutr.BaseUserTaskTaskStakeholders.Add(task);
                            break;
                    }
                }
                if (butr.GetType() == typeof(RoleTaskReference)) {
                    RoleTaskReference rolebutr = (RoleTaskReference)butr;
                    //AppUser AppUserbutr = rolebutr.AppUser;
                    //if (ual.IndexOf(AppUserbutr) == -1) ual.Add(AppUserbutr);
                }
                if (butr.GetType() == typeof(GroupTaskReference)) {
                    GroupTaskReference groupbutr = (GroupTaskReference)butr;
                    //AppUser AppUserbutr = userbutr.AppUser;
                    //if (ual.IndexOf(AppUserbutr) == -1) ual.Add(AppUserbutr);
                }
            }
        }


    }

}
