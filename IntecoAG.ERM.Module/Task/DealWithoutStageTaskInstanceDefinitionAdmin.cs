using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

using System.Linq;

using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// Класс для определения и редактирования параметров конкретного типа задач для конкретного типа бизнес-объектов
    /// В основном вся функциональность находится в базовом классе
    /// Этот класс создаёт соответствующую задачу и назначает её параметры.
    /// </summary>
    //[DefaultClassOptions]
    [Persistent]
    public partial class DealWithoutStageTaskInstanceDefinitionAdmin : UserTaskAdmin {
        public DealWithoutStageTaskInstanceDefinitionAdmin(Session session)
            : base(session) {
        }


        #region Сериализуемые свойства

        //public TIn _MsgIn;
        /// <summary>
        /// Сохранение в базе полей MsgIn и MsgOut в строках через сериализацию XML
        /// Автоматически не поднимать из базы.
        /// </summary>
        [Delayed(true)]
        public string MsgInXML {
            get { return GetDelayedPropertyValue<string>("MsgInXML"); }
            set { SetDelayedPropertyValue<string>("MsgInXML", value); }
        }

        //[PersistentAlias("MsgInXML")]
        [NonPersistent]
        public TaskParameters MsgIn {
            get {
                try {
                    return CommonMethods.DeserializeObject<TaskParameters>(MsgInXML);
                } catch {
                    return new TaskParameters();
                }
            }
            set {
                MsgInXML = CommonMethods.SerializeObject<TaskParameters>(value);
            }
        }

        //public TOut _MsgOut;
        /// <summary>
        /// Сохранение в базе полей MsgIn и MsgOut в строках через сериализацию XML
        /// Автоматически не поднимать из базы.
        /// </summary>
        [Delayed(true)]
        public string MsgOutXML {
            get { return GetDelayedPropertyValue<string>("MsgOutXML"); }
            set { SetDelayedPropertyValue<string>("MsgOutXML", value); }
        }

        //[PersistentAlias("MsgOutXML")]
        [NonPersistent]
        public ApproveResult MsgOut {
            get {
                try {
                    return CommonMethods.DeserializeObject<ApproveResult>(MsgOutXML);
                } catch {
                    return ApproveResult.Undefined;
                }
            }
            set {
                MsgOutXML = CommonMethods.SerializeObject<ApproveResult>(value);
            }
        }

        #endregion


        #region МЕТОДЫ

        /// <summary>
        /// Обращение к методу new класса UserTask для создания конкретной задачи под конкретный бизнес-объект
        /// Создание экземпляра задачи, с которой становится завязанным данный объект административных настроек
        /// Алгоритм работы такой. Создаётся экземпляр объекта типа UserTask. Далее, в этот экземпляр переносятся настройки из 
        /// данного админитративного объекта (из this), например, переносятся списки пользователей, прочие настройки.
        /// </summary>
        [Action(ToolTip = "Create concrete user task")]
        public override void create() {
            // Параметры формируются здесь исключительно для демонстрации последующиего алгоритма разбора в методе SetTaskParameters
            TaskParameters tp = GetTaskParams();

            // Создание задачи (вызов new)
            DealWithoutStageTaskInstanceDefinition tid = DealWithoutStageTaskInstanceDefinition.create(this.Session, tp);

            SetTaskParameters(ref tid, this, tp);

            // Вызываем activate() для созданной задачи
            tid.activate();
        }

        //[DebuggerHidden]
        private TaskParameters GetTaskParams() {
            TaskParameters taskParams = new TaskParameters();
            taskParams.Priority = Priority;

            taskParams.ExcludedOwnerString = "";
            taskParams.PotentialOwnerString = "user: VB_ANGARA\\HomoSapiens; role: ; group: ;";
            taskParams.BusinessAdministratorString = "user: VB_ANGARA\\HomoSapiens; role: ; group: ;";

            taskParams.PossibleDelegateString = "";
            taskParams.TaskStakeholderString = "";
            taskParams.NotificationRecipientString = "";

            taskParams.CallBackObject = this.Oid;
            return taskParams;
        }

        /// <summary>
        /// Перенос параметров в задачу.
        /// Данная процедура:
        /// 1. Извлекает пользователей из соотвесттвующих списков данного объекта администрирования
        /// 2. Извлекает пользователей из списков, задачнных через параметр типа TaskParameters
        /// 3. Сливает вместе соответствующие списки из пп 1 и 2
        /// 4. Переносит результат в пользвательскую задачу
        /// 5. Также переносит данные о приоритете и т.д.
        /// </summary>
        /// <param name="task"></param>
        private void SetTaskParameters(ref DealWithoutStageTaskInstanceDefinition task, DealWithoutStageTaskInstanceDefinitionAdmin adminTask, TaskParameters tp) {

            FunctionalTaskReference.ProcessElements(tp, ref task, this.Session);
            FunctionalTaskReference.FillUsersFromAdminTaskCollection(this, ref task);

            task.Priority = this.Priority;
            //task.CallBackObject = this.CallBackObject;
            task.TaskAdminGuid = this.Oid;

        }

        #endregion

    }

}
