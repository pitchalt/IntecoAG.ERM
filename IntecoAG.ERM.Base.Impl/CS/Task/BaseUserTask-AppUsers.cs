#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace IntecoAG.ERM.Module {

    #region BaseUserTask_PotentialOwners

    public partial class BaseUserTask {
        [Association("BaseUserTasks_PotentialOwners", typeof(AppUser))]
        public XPCollection<AppUser> PotentialOwners { get { return GetCollection<AppUser>("PotentialOwners"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_PotentialOwners", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskPotentialOwners { get { return GetCollection<BaseUserTask>("BaseUserTaskPotentialOwners"); } }
    }

    #endregion


    
    #region BaseUserTask_ExcludedOwners

    public partial class BaseUserTask {
        [Association("BaseUserTasks_ExcludedOwners", typeof(AppUser))]
        public XPCollection<AppUser> ExcludedOwners { get { return GetCollection<AppUser>("ExcludedOwners"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_ExcludedOwners", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskExcludedOwners { get { return GetCollection<BaseUserTask>("BaseUserTaskExcludedOwners"); } }
    }

    #endregion

    
    
    #region BaseUserTask_BusinessAdministrators

    public partial class BaseUserTask {
        [Association("BaseUserTasks_BusinessAdministrators", typeof(AppUser))]
        public XPCollection<AppUser> BusinessAdministrators { get { return GetCollection<AppUser>("BusinessAdministrators"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_BusinessAdministrators", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskBusinessAdministrators { get { return GetCollection<BaseUserTask>("BaseUserTaskBusinessAdministrators"); } }
    }

    #endregion

    
    
    #region BaseUserTask_PossibleDelegates

    public partial class BaseUserTask {
        [Association("BaseUserTasks_PossibleDelegates", typeof(AppUser))]
        public XPCollection<AppUser> PossibleDelegates { get { return GetCollection<AppUser>("PossibleDelegates"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_PossibleDelegates", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskPossibleDelegates { get { return GetCollection<BaseUserTask>("BaseUserTaskPossibleDelegates"); } }
    }

    #endregion

    
    
    #region BaseUserTask_TaskStakeholders

    public partial class BaseUserTask {
        [Association("BaseUserTasks_TaskStakeholders", typeof(AppUser))]
        public XPCollection<AppUser> TaskStakeholders { get { return GetCollection<AppUser>("TaskStakeholders"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_TaskStakeholders", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskTaskStakeholders { get { return GetCollection<BaseUserTask>("BaseUserTaskTaskStakeholders"); } }
    }

    #endregion

    
    
    #region BaseUserTask_NotificationRecipients

    public partial class BaseUserTask {
        [Association("BaseUserTasks_NotificationRecipients", typeof(AppUser))]
        public XPCollection<AppUser> NotificationRecipients { get { return GetCollection<AppUser>("NotificationRecipients"); } }
    }

    public partial class AppUser {
        [Association("BaseUserTasks_NotificationRecipients", typeof(BaseUserTask))]
        public XPCollection<BaseUserTask> BaseUserTaskNotificationRecipients { get { return GetCollection<BaseUserTask>("BaseUserTaskNotificationRecipients"); } }
    }

    #endregion

}
