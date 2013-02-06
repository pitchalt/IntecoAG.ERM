using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CRM.Counters {

    // Класс реализует счётчик последовательных номеров документов в журнале регистрации.
    // Чтобы счётчик корректно работал, необходимо, чтобы он сохранял в БД своё изменение мгновенно. 
    // Для этого он вычисляет последний номер, подготавливает запись со следующим номером 

    // Тип XPLiteObject по умолчанию не поддерживет Optimistic Locking

    // [DefaultClassOptions] // Временно для просмотра
    public class RegistrationLogISNCounter : XPLiteObject {
        public RegistrationLogISNCounter(Session session)
            : base(session) {
        }
        public RegistrationLogISNCounter(Session session, int startValue, int step)
            : base(session) {
                StartValue = startValue;
                Step = step;
        }

        #region ПОЛЯ КЛАССА

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int Oid;

        #endregion

        #region СВОЙСТВА КЛАССА

        private int _ISN;
        public int ISN {
            get { return _ISN; }
            set { SetPropertyValue<int>("ISN", ref _ISN, value); }
        }

        private int _StartValue;
        public int StartValue {
            get { return _StartValue; }
            set { SetPropertyValue<int>("StartValue", ref _StartValue, value); }
        }

        private int _Step;
        public int Step {
            get { return _Step; }
            set { SetPropertyValue<int>("Step", ref _Step, value); }
        }

        //private bool _Locked;
        //public bool Locked {
        //    get { return _Locked; }
        //    set { SetPropertyValue<bool>("Locked", ref _Locked, value); }
        //}

        #endregion

        #region МЕТОДЫ

        /// <summary>
        /// Счётчик начинается с 1 и продолжается через 1
        /// </summary>
        /// <returns></returns>
        public int ReserveNumber() {

            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer)) {
                RegistrationLogISNCounter rlic = uow.GetObjectByKey<RegistrationLogISNCounter>(this.Oid);
                rlic.Reload();
                rlic.ISN++;
                uow.CommitChanges();
                return rlic.ISN;
            }

            //this.ISN++;
            //this.Save();
            //this.Session.FlushChanges();
            //return this.ISN;
        }

        /// <summary>
        /// Счётчик начинается с startValue и продолжается через step
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public int ReserveNumber(int startValue, int step) {
            //this.Session.BeginTransaction();
            //RegistrationLogISNGenerator regLogISN = new RegistrationLogISNGenerator(this.Session);
            //regLogISN.Save();
            if (this.ISN == 0) {
                this.ISN = startValue;
            } else {
                this.ISN = this.ISN + step;
            }
            this.Save();
            this.Session.FlushChanges();
            //this.Session.CommitTransaction();
            return this.ISN;
        }

        #endregion

    }

}
