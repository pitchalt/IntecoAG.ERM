using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.ERM.CS
{
    [NonPersistent]   // Чтобы разнородные объекты не попадали в одну коллекцию
    public class ObjectCollection : BaseObject //XPObject
    {
        public ObjectCollection() : base() { }

        public ObjectCollection(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        
        #region ПОЛЯ КЛАССА

        /// <summary>
        /// Код
        /// </summary>
        private string _Code;
        [DisplayName("5-код")]
        [Size(5)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        #endregion
    }

}