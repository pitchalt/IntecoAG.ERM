using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module  {
    //[DefaultClassOptions]
    [Persistent]
    public abstract partial class BaseUserTaskReference : BaseObject {
        public BaseUserTaskReference(Session session)
            : base(session) {
        }

        #region СВОЙСТВА

        private string _UserName;
        /// <summary>
        /// Короткое читаемое имя пользователя (Login) (как из ActiveDirectory), аналогично для ролей и групп
        /// </summary>
        public virtual string UserName {
            get { return _UserName; }
            set { SetPropertyValue<string>("UserName", ref _UserName, value); }
        }

        private string _Name;
        /// <summary>
        /// Читаемое наименование пользователя (ФИО), для ролей и групп - их какие-то наименования
        /// </summary>
        public virtual string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        private UserAssociationType _userAssociationType;
        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public virtual UserAssociationType UserAssociationType
        {
            get { return _userAssociationType; }
            set { SetPropertyValue<UserAssociationType>("UserAssociationType", ref _userAssociationType, value); }
        }

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Получение списка пользователей по его идентификатору (список из 1-го пользователя), по роли, по группе
        /// </summary>
        /// <returns></returns>
        public abstract List<AppUser> GetUserList(IList<BaseUserTaskReference> Butrs, IList<string> userNameList);

        #endregion
    }

}
