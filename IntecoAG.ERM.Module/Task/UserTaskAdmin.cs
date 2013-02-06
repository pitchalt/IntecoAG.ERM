using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// Класс (и затем объект) для подддержки административных настроек присоединённой задачи.
    /// Этот класс абстрактный и инкапсулирует в себе максимум функциональности, которую можно вынести в базовый класс
    /// В качестве административных настроек могут быть, к примеру, такие:
    /// - приоритет
    /// - списки пользователей
    /// </summary>
    /// <typeparam name="TIn">Тип (входного) бизнес-объекта (например, Invoice)</typeparam>
    /// <typeparam name="TOut">Тип (выходного) бизнес-объекта</typeparam>
    public abstract partial class UserTaskAdmin : BaseTaskAdmin {
        public UserTaskAdmin(Session session)
            : base(session) {
        }


        #region СВОЙСТВА

        private bool _CanCallFromUI;
        public override bool CanCallFromUI {
            get { return _CanCallFromUI; }
            set { SetPropertyValue<Boolean>("CanCallFromUI", ref _CanCallFromUI, value); }
        }


        private bool _MustMergeUsersFromBusinessObject;
        public bool MustMergeUsersFromBusinessObject {
            get { return _MustMergeUsersFromBusinessObject; }
            set { SetPropertyValue<bool>("MustMergeUsersFromBusinessObject", ref _MustMergeUsersFromBusinessObject, value); }
        }

        private uint _Priority;
        public uint Priority {
            get { return _Priority; }
            set { SetPropertyValue<uint>("Priority", ref _Priority, value); }
        }


        /// <summary>
        /// Guid той задачи, которая порождается данным административным объектом
        /// </summary>
        private Guid _TaskGuid;
        public Guid TaskGuid {
            get { return _TaskGuid; }
            set { SetPropertyValue<Guid>("TaskGuid", ref _TaskGuid, value); }
        }


        /// <summary>
        /// Guid для реализации обратного вызова. Это Guid соттветствующего управляющего объекта типа BPInvoiceProcess
        /// </summary>
        private Guid _CallBackObject;
        public Guid CallBackObject {
            get { return _CallBackObject; }
            set { SetPropertyValue<Guid>("CallBackObject", ref _CallBackObject, value); }
        }


        private string _DetailView;
        /// <summary>
        /// Наименование того DetailView, которое должно быть вызвано для показа задачи (UserTask)
        /// Правила такие. Учёт DetailView производится только после того, как в UsertaskAdmin выполнен
        /// метод create() и только в случае, когда задача не находится в состоянии suspend. В противном случае,
        /// отображается стандартный DetailView для объекта BaseUserTask
        /// </summary>
        public string DetailView {
            get { return _DetailView; }
            set { SetPropertyValue<string>("DetailView", ref _DetailView, value); }
        }

        #endregion

        
        #region МЕТОДЫ

        // Методы работы с пользователями: 
        // Слияние списков пользователей;
        // Формирование пользователей из списка системных (Windows, ActiveDirectory и т.п.)
        // Формирование списка пользователей по списку ролей
        // Формирование списка пользователей по списку групп (групп пока нет)
        // Формирование списка пользователей по списку пользователей из другой задачи (копирование), можно даже задачи копировать с польователями

        public override List<AppUser> MergeUserLists(List<AppUser> userList1, List<AppUser> userList2) {
            List<AppUser> resultList = new List<AppUser>();
            resultList = userList1;
            foreach (AppUser au in userList2) {
                if (!userList1.Contains(au)) userList1.Add(au);
            }
            return resultList;
        }

        #endregion

    }

}
