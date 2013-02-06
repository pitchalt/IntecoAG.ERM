namespace IntecoAG.ERM.Module {

    /// <summary>
    /// ������-������� ��� �������
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
    /// ���������� ������-�������� �� �������
    /// </summary>
    public enum ApproveResult {
        Undefined,
        Success,
        Declain
    }

    /// <summary>
    /// ��������� ������
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
    /// ������������ ����, ���� ������ ����� ���� �����������
    /// </summary>
    public enum DelegationType {
        Anybody,
        PotentialOwners,
        Other,
        Nobody
    }


    /// <summary>
    /// ������������ ����� ����������� �������������: User, Role, Group
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
    /// ��� ������ �������������
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