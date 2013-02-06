using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CRM.Contract.Deal;

namespace IntecoAG.ERM.Module {

    /// <summary>
    /// 
    /// </summary>
    //[DefaultClassOptions]
    [Persistent]
    public class DealWithoutStageTaskInstanceDefinition : UserTask {
        public DealWithoutStageTaskInstanceDefinition(Session session)
            : base(session) {
        }

        #region Нехранимые свойства для редактирования объекта

        // Здесь для каждого значимого по каким-либо соображениям свойства бизнес-объекта создаётся
        // нехранимое поле для его редактирования



        private crmDealWithoutStageVersion _DealWithoutStageVersion;
        [NonPersistent]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmDealWithoutStageVersion DealWithoutStageVersion {
            //get { return _DealWithoutStageVersion; }
            //set { SetPropertyValue<crmDealWithoutStageVersion>("DealWithoutStageVersion", ref _DealWithoutStageVersion, value); }
            get { return (_DealWithoutStageVersion == null) ? null : _DealWithoutStageVersion; }
            set { if (_DealWithoutStageVersion != null) _DealWithoutStageVersion = value; }
        }




        /*
        Party _From;
        [NonPersistent]
        public Party From {
            get { return (_boInvoice == null) ? null : _boInvoice.From; }
            set { if (_boInvoice != null) _boInvoice.From = value; }
        }

        Party _To;
        [NonPersistent]
        public Party To {
            get { return (_boInvoice == null) ? null : _boInvoice.To; }
            set { if (_boInvoice != null) _boInvoice.To = value; }
        }

        decimal _Summa;
        [NonPersistent]
        public decimal Summa {
            get { return (_boInvoice == null) ? 0 : _boInvoice.Summa; }
            set { if (_boInvoice != null) _boInvoice.Summa = value; }
        }

        string _Number;
        [Size(20)]
        [NonPersistent]
        public string Number {
            get { return (_boInvoice == null) ? null : _boInvoice.Number; }
            set { if (_boInvoice != null) _boInvoice.Number = value; }
        }

        DateTime _DateTime;
        [NonPersistent]
        public DateTime DateTime {
            get { return (_boInvoice == null) ? DateTime.MinValue : _boInvoice.DateTime; }
            set { if (_boInvoice != null) _boInvoice.DateTime = value; }
        }
        */

        // ССЫЛКА на то бизнес-объект, для которого формируется задача
        object _boInstance;
        [Browsable(false)]
        public object boInstance {
            get { return _boInstance; }
            set { SetPropertyValue<object>("boInstance", ref _boInstance, value); }
        }

        #endregion


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
        /// Создание новой задачи. Вызывается из присоединённого административного объекта, который также назнает свойства
        /// новорожденной задаче
        /// </summary>
        public static DealWithoutStageTaskInstanceDefinition create(Session ssn, TaskParameters tp) {
            DealWithoutStageTaskInstanceDefinition tid = new DealWithoutStageTaskInstanceDefinition(ssn);
            tid._TaskInitiator = null; // Если создаётся через интерфейс, то TaskInitiator заполняется, иначе - обнуляется
            return tid;
        }

        #endregion

    }

}
