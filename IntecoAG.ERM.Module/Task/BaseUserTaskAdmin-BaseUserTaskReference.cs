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

using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace IntecoAG.ERM.Module {

    #region BaseTaskAdmin_PotentialOwners

    /// <summary>
    /// BaseUserTaskAdmin-BaseUserTaskReference
    /// </summary>
    public partial class BaseTaskAdmin
    {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_PotentialOwnersAdmin")]
        public XPCollection<BaseUserTaskReference> PotentialOwnersAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("PotentialOwnersAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference
    {
        private BaseTaskAdmin _PotentialOwners;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_PotentialOwnersAdmin")]
        public BaseTaskAdmin PotentialOwnersAdmin {
            get { return _PotentialOwners; }
            set { SetPropertyValue<BaseTaskAdmin>("PotentialOwnersAdmin", ref _PotentialOwners, value); }
        }
    }

    #endregion

    #region BaseTaskAdmin_ExcludedOwners

    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTaskAdmin {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_ExcludedOwnersAdmin")]
        public XPCollection<BaseUserTaskReference> ExcludedOwnersAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("ExcludedOwnersAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference {
        private BaseTaskAdmin _ExcludedOwners;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_ExcludedOwnersAdmin")]
        public BaseTaskAdmin ExcludedOwnersAdmin {
            get { return _ExcludedOwners; }
            set { SetPropertyValue<BaseTaskAdmin>("ExcludedOwnersAdmin", ref _ExcludedOwners, value); }
        }
    }

    #endregion

    #region BaseTaskAdmin_BusinessAdministrators

    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTaskAdmin {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_BusinessAdministratorsAdmin")]
        public XPCollection<BaseUserTaskReference> BusinessAdministratorsAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("BusinessAdministratorsAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference {
        private BaseTaskAdmin _BusinessAdministrators;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_BusinessAdministratorsAdmin")]
        public BaseTaskAdmin BusinessAdministratorsAdmin {
            get { return _BusinessAdministrators; }
            set { SetPropertyValue<BaseTaskAdmin>("BusinessAdministratorsAdmin", ref _BusinessAdministrators, value); }
        }
    }

    #endregion

    #region BaseTaskAdmin_PossibleDelegates

    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTaskAdmin {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_PossibleDelegatesAdmin")]
        public XPCollection<BaseUserTaskReference> PossibleDelegatesAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("PossibleDelegatesAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference {
        private BaseTaskAdmin _PossibleDelegates;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_PossibleDelegatesAdmin")]
        public BaseTaskAdmin PossibleDelegatesAdmin {
            get { return _PossibleDelegates; }
            set { SetPropertyValue<BaseTaskAdmin>("PossibleDelegatesAdmin", ref _PossibleDelegates, value); }
        }
    }

    #endregion

    #region BaseTaskAdmin_TaskStakeholders

    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTaskAdmin {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_TaskStakeholdersAdmin")]
        public XPCollection<BaseUserTaskReference> TaskStakeholdersAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("TaskStakeholdersAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference {
        private BaseTaskAdmin _TaskStakeholders;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_TaskStakeholdersAdmin")]
        public BaseTaskAdmin TaskStakeholdersAdmin {
            get { return _TaskStakeholders; }
            set { SetPropertyValue<BaseTaskAdmin>("TaskStakeholdersAdmin", ref _TaskStakeholders, value); }
        }
    }

    #endregion

    #region BaseTaskAdmin_NotificationRecipients

    /// <summary>
    /// 
    /// </summary>
    public partial class BaseTaskAdmin {
        [DebuggerHidden]
        [Association("BaseTaskAdmin_NotificationRecipientsAdmin")]
        public XPCollection<BaseUserTaskReference> NotificationRecipientsAdmin {
            get {
                return GetCollection<BaseUserTaskReference>("NotificationRecipientsAdmin");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class BaseUserTaskReference {
        private BaseTaskAdmin _NotificationRecipients;
        [DebuggerHidden]
        [Browsable(false)]
        [Association("BaseTaskAdmin_NotificationRecipientsAdmin")]
        public BaseTaskAdmin NotificationRecipientsAdmin {
            get { return _NotificationRecipients; }
            set { SetPropertyValue<BaseTaskAdmin>("NotificationRecipientsAdmin", ref _NotificationRecipients, value); }
        }
    }

    #endregion

}
