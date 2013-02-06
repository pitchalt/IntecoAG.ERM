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

    [DefaultClassOptions] // Временно для просмотра
    public class RegistrationLogISNGenerator : XPLiteObject {
        public RegistrationLogISNGenerator(Session session) : base(session) {
        }

        #region ПОЛЯ КЛАССА

        [Browsable(false)]
        [Key(AutoGenerate = true)]
        public int ISN;

        #endregion

        #region СВОЙСТВА КЛАССА

        #endregion

        #region МЕТОДЫ

        public int ReserveNumber() {
            this.Session.BeginTransaction();
            //RegistrationLogISNGenerator regLogISN = new RegistrationLogISNGenerator(this.Session);
            //regLogISN.Save();
            this.Save();
            this.Session.FlushChanges();
            this.Session.CommitTransaction();
            return this.ISN;
        }

        #endregion

    }

}
