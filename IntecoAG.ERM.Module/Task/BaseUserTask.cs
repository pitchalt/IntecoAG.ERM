using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using System.Linq;

namespace IntecoAG.ERM.Module {
    public abstract partial class BaseUserTask : BaseTask {
        public BaseUserTask(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            _State = UserTaskState.Created;

            if (this.TaskAdminGuid == null | this.TaskAdminGuid == Guid.Empty) {
                AppUser currentUser = this.Session.GetObjectByKey<AppUser>(SecuritySystem.CurrentUserId);
                if (currentUser != null) _TaskInitiator = currentUser;
            }
        }

        /// <summary>
        /// Короткое кодовое обозначение задачи
        /// </summary>
        public virtual string Name
        {
            get { return null; }
        }

        /// <summary>
        /// Короткое описание задачи
        /// </summary>
        public virtual string Subject {
            get { return null; }
        }

        /// <summary>
        /// Полное описание задачи
        /// </summary>
        public virtual string Description {
            get { return null; }
        }

        private AppUser _Owner;
        //[Browsable(false)]
        public AppUser Owner {
            get { return _Owner; }
            set { SetPropertyValue<AppUser>("Owner", ref _Owner, value); }
        }

        // Сон задачи
        private bool _IsSuspended;
        //[Browsable(false)]
        public bool IsSuspended {
            get { return _IsSuspended; }
        }

        /// <summary>
        /// Guid той административной задачи, которая порождает данный объект и формирует его настройки
        /// </summary>
        private Guid _TaskAdminGuid;
        public Guid TaskAdminGuid {
            get { return _TaskAdminGuid; }
            set { SetPropertyValue<Guid>("TaskAdminGuid", ref _TaskAdminGuid, value); }
        }

        // Guid для реализации обратного вызова. Это Guid соттветствующего управляющего объекта типа BPInvoiceProcess
        private Guid _CallBackObject;
        public Guid CallBackObject {
            get { return _CallBackObject; }
            set { SetPropertyValue<Guid>("CallBackObject", ref _CallBackObject, value); }
        }

        private DelegationType _Delegation;
        public DelegationType Delegation {
            get { return _Delegation; }
            set { SetPropertyValue<DelegationType>("Delegation", ref _Delegation, value); }
        }


        #region АТРИБУТЫ ИЗ ДОКУМЕНТАЦИИ

        private int _ID;
        public int ID {
            get { return _ID; }
            set { SetPropertyValue<int>("ID", ref _ID, value); }
        }

        private TaskType _TaskType;
        public TaskType TaskType {
            get { return _TaskType; }
            set { SetPropertyValue<TaskType>("TaskType", ref _TaskType, value); }
        }

        ///// <summary>
        ///// Квалифицирующее имя задачи. Свойство описано ниже
        ///// </summary>
        //private string _Name;
        //public string Name {
        //    get { return _Name; }
        //    set { SetPropertyValue<string>("Name", ref _Name, value); }
        //}

        /// <summary>
        /// Состояние задачи 
        /// </summary>
        private UserTaskState _State;
        //[Browsable(false)]
        [ImmediatePostData(true)]
        public UserTaskState State {
            get { return _State; }
            set { SetPropertyValue<UserTaskState>("State", ref _State, value); }
        }

        /// <summary>
        /// 0 = highest
        /// </summary>
        private uint _Priority;
        public uint Priority {
            get { return _Priority; }
            set { SetPropertyValue<uint>("Priority", ref _Priority, value); }
        }

        protected AppUser _TaskInitiator;
        //[Browsable(false)]
        public AppUser TaskInitiator {
            get { return _TaskInitiator; }
        }

        private AppUser _ActualOwner;
        //[Browsable(false)]
        public AppUser ActualOwner {
            get { return _ActualOwner; }
        }

        /// <summary>
        /// The time in UTC when the task has been created.
        /// </summary>
        private DateTime _CreatedOn;
        //[Browsable(false)]
        public DateTime CreatedOn {
            get { return _CreatedOn; }
            set { SetPropertyValue<DateTime>("CreatedOn", ref _CreatedOn, value); }
        }

        /// <summary>
        /// Кем создана задача 
        /// </summary>
        private string _CreatedBy;
        public string CreatedBy {
            get { return _CreatedBy; }
            set { SetPropertyValue<string>("CreatedBy", ref _CreatedBy, value); }
        }

        /// <summary>
        /// The time in UTC when the task has been activated.
        /// </summary>
        private DateTime _ActivationTime;
        //[Browsable(false)]
        public DateTime ActivationTime {
            get { return _ActivationTime; }
            set { SetPropertyValue<DateTime>("ActivationTime", ref _ActivationTime, value); }
        }

        /// <summary>
        /// The time in UTC when the task will expire.
        /// </summary>
        private DateTime _ExpirationTime;
        //[Browsable(false)]
        public DateTime ExpirationTime {
            get { return _ExpirationTime; }
            set { SetPropertyValue<DateTime>("ExpirationTime", ref _ExpirationTime, value); }
        }

        private bool _IsSkipable;
        //[Browsable(false)]
        public bool IsSkipable {
            get { return _IsSuspended; }
            set { SetPropertyValue<bool>("IsSkipable", ref _IsSkipable, value); }
        }

//        private bool _HasPotentialOwners;
        //[Browsable(false)]
        public bool HasPotentialOwners {
            get {
                if (this.PotentialOwners != null && this.PotentialOwners.Count > 0) return this.PotentialOwners.Count > 0;
                return false;
            }
        }

        private bool _StartByExists;
        //[Browsable(false)]
        public bool StartByExists {
            get { return _StartByExists; }
            set { SetPropertyValue<bool>("StartByExists", ref _StartByExists, value); }
        }

        private bool _CompleteByExists;
        //[Browsable(false)]
        public bool CompleteByExists {
            get { return _CompleteByExists; }
            set { SetPropertyValue<bool>("CompleteByExists", ref _CompleteByExists, value); }
        }

        /// <summary>
        /// type="tPresentationName"
        /// </summary>
        private string _PresentationName;
        //[Browsable(false)]
        public string PresentationName {
            get { return _PresentationName; }
            set { SetPropertyValue<string>("PresentationName", ref _PresentationName, value); }
        }

        /// <summary>
        /// type="tPresentationSubject"
        /// </summary>
        private string _PresentationSubject;
        //[Browsable(false)]
        public string PresentationSubject {
            get { return _PresentationSubject; }
            set { SetPropertyValue<string>("PresentationSubject", ref _PresentationSubject, value); }
        }

        private bool _RenderingMethodExists;
        //[Browsable(false)]
        public bool RenderingMethodExists {
            get { return _RenderingMethodExists; }
            set { SetPropertyValue<bool>("RenderingMethodExists", ref _RenderingMethodExists, value); }
        }

        private bool _HasOutput;
        //[Browsable(false)]
        public bool HasOutput {
            get { return _HasOutput; }
            set { SetPropertyValue<bool>("HasOutput", ref _HasOutput, value); }
        }

        private bool _HasFault;
        //[Browsable(false)]
        public bool HasFault {
            get { return _HasFault; }
            set { SetPropertyValue<bool>("HasFault", ref _HasFault, value); }
        }

        private bool _HasAttachments;
        //[Browsable(false)]
        public bool HasAttachments {
            get { return _HasAttachments; }
            set { SetPropertyValue<bool>("HasAttachments", ref _HasAttachments, value); }
        }

        private bool _HasComments;
        //[Browsable(false)]
        public bool HasComments {
            get { return _HasComments; }
            set { SetPropertyValue<bool>("HasComments", ref _HasComments, value); }
        }

        private bool _Escalated;
        //[Browsable(false)]
        public bool Escalated {
            get { return _Escalated; }
            set { SetPropertyValue<bool>("Escalated", ref _Escalated, value); }
        }

        /// <summary>
        /// type="tPresentationSubject"
        /// </summary>
        private string _PrimarySearchBy;
        //[Browsable(false)]
        public string PrimarySearchBy {
            get { return _PrimarySearchBy; }
            set { SetPropertyValue<string>("PrimarySearchBy", ref _PrimarySearchBy, value); }
        }

        /// <summary>
        /// The time in UTC when the task should have been started. This time corresponds to the respective start deadline
        /// </summary>
        private DateTime _StartBy;
        //[Browsable(false)]
        public DateTime StartBy {
            get { return _StartBy; }
            set { SetPropertyValue<DateTime>("StartBy", ref _StartBy, value); }
        }

        private bool _Skipable;
        //[Browsable(false)]
        public bool Skipable {
            get { return _Skipable; }
            set { SetPropertyValue<bool>("Skipable", ref _Skipable, value); }
        }

        /// <summary>
        /// The time in UTC when the task should have been started. This time corresponds to the respective start deadline
        /// </summary>
        private DateTime _CompleteBy;
        //[Browsable(false)]
        public DateTime CompleteBy {
            get { return _CompleteBy; }
            set { SetPropertyValue<DateTime>("CompleteBy", ref _CompleteBy, value); }
        }

        /// <summary>
        /// The task’s presentation name.
        /// </summary>
        private string _PresName;
        //[Browsable(false)]
        public string PresName {
            get { return _PresName; }
            set { SetPropertyValue<string>("PresName", ref _PresName, value); }
        }

        /// <summary>
        /// The task’s presentation subject.
        /// </summary>
        private string _PresSubject;
        //[Browsable(false)]
        public string PresSubject {
            get { return _PresSubject; }
            set { SetPropertyValue<string>("PresSubject", ref _PresSubject, value); }
        }

        /// <summary>
        /// Наименование (в данном случае) DetailView
        /// </summary>
        private string _RenderingMethName;
        //[Browsable(false)]
        public string RenderingMethName {
            get { return _RenderingMethName; }
            set { SetPropertyValue<string>("RenderingMethName", ref _RenderingMethName, value); }
        }

        ///// <summary>
        ///// Вместо UserId Owner
        ///// </summary>
        //private string _UserId;
        ////[Browsable(false)]
        //public string UserId {
        //    get { return _UserId; }
        //    set { SetPropertyValue<string>("UserId", ref _UserId, value); }
        //}

        private string _Group;
        //[Browsable(false)]
        public string Group {
            get { return _Group; }
            set { SetPropertyValue<string>("Group", ref _Group, value); }
        }

        private string _GenericHumanRole;
        //[Browsable(false)]
        public string GenericHumanRole {
            get { return _GenericHumanRole; }
            set { SetPropertyValue<string>("GenericHumanRole", ref _GenericHumanRole, value); }
        }

        #endregion


        #region ДЕЙСТВИЯ ЗАДАЧИ

        /// <summary>
        /// Если задача имеет непустой список PotentialOwner, задача становиться Ready
        /// Если PotentialOwner содержит одного пользователя, то Reserved и пользователь владелец.
        /// </summary>
        [Action(ToolTip = "Activate user task")]
        public virtual void activate() {
            if (_State != UserTaskState.Created) throw new Exception("State must be Created");

            if (Owner == null) {
                if (PotentialOwners.Count == 1) {
                    State = UserTaskState.Reserved;
                    Owner = PotentialOwners[0];
                } else {
                    if (PotentialOwners.Count > 1) {
                        State = UserTaskState.Ready;
                    } else {
                        throw new Exception("Owner is not defined");
                    }
                }
            } else {
                State = UserTaskState.Reserved;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Action(ToolTip = "Nominate")]
        public virtual void Nominate(List<AppUser> userList) {
            // Паша: Выполнить PotentialUsers = userList; activate();
            // ограничение список пользователей не пустой
        }


        /// <summary>
        /// Закрепляет задачу за пользователем. После этого пользователь может создать сам счёт (BOInvoice) как таковой.
        /// Текущий пользователь становится владельцем задачи и задача становится Reserved
        /// </summary>
        [Action(ToolTip = "Claim user task")]
        public virtual void claim() {
            if (_State != UserTaskState.Ready) throw new Exception("State must be Ready");

            if (SecuritySystem.CurrentUser != null) {
                State = UserTaskState.Reserved;
                if (CurrentUserInList(PotentialOwners)) {
                    Owner = GetCurrentAppUser();   
                    State = UserTaskState.Reserved;
                } else {
                    throw new Exception("Current user not in PotentialOwners");
                }
            } else {
                throw new Exception("Current user is not defined");
            }
        }

        /// <summary>
        /// Определение того, что текущий пользователь SecuritySystem.CurrentUser состоит в списке userList
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public virtual bool CurrentUserInList(IList<AppUser> userList) {
            return userList.IndexOf(GetCurrentAppUser()) > -1;
        }

        /// <summary>
        /// Определение того, что текущий пользователь SecuritySystem.CurrentUser состоит в списке userList
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public virtual AppUser GetCurrentAppUser() {
            AppUser currentAppUser = Session.GetObjectByKey<AppUser>(SecuritySystem.CurrentUserId);
            return currentAppUser;
        }

        /// <summary>
        /// Статус задачи InProgress, пользователь может работать с интерфейсом задачи.
        /// </summary>
        [Action(ToolTip = "Start user task")]  //, TargetObjectsCriteria = "State == 3")]
        public virtual void start() {
            if (_State != UserTaskState.Ready & _State != UserTaskState.Reserved) throw new Exception("State must be Ready or Reserved");

            if (_State != UserTaskState.Reserved) claim();

            if (SecuritySystem.CurrentUser != null) {
                if (GetCurrentAppUser() != _Owner) throw new Exception("Current user is not owner");
                //// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ВРЕМЕННО ЗАКОММЕНТАРИМ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //// Создаём счёт (обращение к методу create(BOInvoice prmBOInvoice) в классе TaskInvoiceManager)
                //MethodInfo mInfo = typeof(TaskInvoiceManager).GetMethod("create");   //, new Type[] { typeof(string) });
                //if (mInfo != null) {
                //    mInfo.Invoke(this, new object[] { null });
                //}
                State = UserTaskState.InProgress;
            } else {
                throw new Exception("Current user is not defined");
            }
        }

        /// <summary>
        /// Статус задачи Reserved, пользователь не может работать с интерфейсом
        /// </summary>
        [Action(ToolTip = "Stop user task")]
        public virtual void stop() {
            if (_State != UserTaskState.InProgress) throw new Exception("State must be InProgress");

            if (SecuritySystem.CurrentUser != null) {
                if (GetCurrentAppUser() != _Owner) throw new Exception("Current user is not owner");
                State = UserTaskState.Reserved;
            } else {
                throw new Exception("Current user is not defined");
            }
        }

        public void release() {
        }


        /// <summary>
        /// Статус задачи Completed, возвратить сообщение TOut
        /// </summary>
        [Action(ToolTip = "Complete user task")]
        public virtual void complete() {
            if (_State != UserTaskState.InProgress) throw new Exception("State must be InProgress");

            if (SecuritySystem.CurrentUser != null) {

                if (GetCurrentAppUser() != _Owner) throw new Exception("Current user is not owner");


                // Здесь зачинается создание задачи для подтверждения TaskInvoiceProcess
                /*
                // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ВРЕМЕННО ЗАКОММЕНТАРИМ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                // Создаём счёт (обращение к методу create(BOInvoice prmBOInvoice) в классе TaskInvoiceManager)
                MethodInfo mInfo = typeof(TaskInvoiceManager).GetMethod("complete");   //, new Type[] { typeof(string) });
                if (mInfo != null) {
                    mInfo.Invoke(this, null);   //, new object[] { null });
                }
                */

                // Пишем сообщение MsgOut, но оно находится в наследованном классе:
                CommonMethods.setProperty(this, "MsgOut", ApproveResult.Success);

                State = UserTaskState.Completed;
            } else {
                throw new Exception("Current user is not defined");
            }

        }

        public virtual void remove() {
        }

        public virtual void fail() {
        }

        public virtual void skip() {
        }

        public virtual void forward() {
        }

        public virtual void delegate_() {
        }

        #endregion


        #region ОБРАБОТКА ДЕЙСТВИЙ ПОЛЬЗОВАТЕЛЕЙ


        /// <summary>
        /// Если пользователь закрыл окно, в статусе InProgress, то задача засыпает
        /// </summary>
        public virtual void suspend() {
            _IsSuspended = true;
        }

        /// <summary>
        /// Для задача в состоянии Suspended
        /// Если задача заснувшая в статусе InProgress, то активируется пользовательский интерфейс.
        /// </summary>
        public virtual void resume() {
            _IsSuspended = false;
        }

        #endregion

    }

}
