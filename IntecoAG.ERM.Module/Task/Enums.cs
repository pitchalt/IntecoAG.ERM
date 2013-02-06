namespace IntecoAG.ERM.Module {

    /// <summary>
    /// Бизнес-операци над счетами
    /// </summary>
    public enum BPInvoiceState {
        Init,
        New,
        Edit,
        ContractDepartmentApprove,
        PlaneDepartmentApprove,
        BudgetDepartmentApprove,
        Finished
    }

    /// <summary>
    /// Результаты бизнес-операция со счетами
    /// </summary>
    public enum ApproveResult {
        Undefined,
        Success,
        Declain
    }

    /// <summary>
    /// Состояния задачи
    /// </summary>
    public enum UserTaskState {
        Created,
        Ready,
        Reserved,
        InProgress,
        Completed,
        Failed,
        Error,
        Exited,
        Obsolete
    }


    /// <summary>
    /// Перечисление того, кому задача может быть делеирована
    /// </summary>
    public enum DelegationType {
        Anybody,
        PotentialOwners,
        Other,
        Nobody
    }


    /// <summary>
    /// Перечисление типов объединений пользователей: User, Role, Group
    /// </summary>
    public enum UserAssociationType {
        user,
        role,
        group
    }

    /// <summary>
    /// Identifies the task type
    /// </summary>
    public enum TaskType {
        TASK,
        NOTIFICATION
    }

    /// <summary>
    /// Тип списка пользователей
    /// </summary>
    public enum UserListType {
        BusinessAdministrators,
        ExcludedOwners,
        NotificationRecipients,
        PossibleDelegates,
        PotentialOwners,
        TaskStakeholders
    }

}