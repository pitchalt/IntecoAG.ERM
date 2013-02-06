using System;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.Metadata;


namespace IntecoAG.ERM.Module {

    /// <summary>
    /// Класс, управляющий состояниями приписанного к нему конкретного бизнес-класса
    /// В НАСТОЯЩИЙ МОМЕНТ (2011-06-23) ПАРАМЕТРИЗАЦИЮ КЛАССА ПО ТИПАМ TIn и TOut УБРАНА ПО ПРИЧИНЕ НЕПОДДЕРЖКИ ЭТОГО XAF'ом
    /// TIn и TOut - входящее и исходящее сообщения (гуид объекта, результат операции подтверждения и т.п.).
    /// TIn и TOut - сериализуемые в XML типы. Загружаются по требованию (Delay)
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    //[DefaultClassOptions]
    [Persistent]
    public partial class UserTask : BaseUserTask {

        public UserTask(Session session)
            : base(session) {
        }


        #region СВОЙСТВА

        /// <summary>
        /// Наиенование задачи с полной квалификацией, например, "IntecoAG.ERM.TaskInvoice" 
        /// </summary>
        public override string Name {
            get { return this.GetType().ToString(); }
        }

        static string _Subject = "New Invoice Subject";
        public override string Subject {
            get { return _Subject; }
        }

        static string _Description = "New Invoice Description";
        public override string Description {
            get { return _Description; }
        }

        #endregion

    }

}
