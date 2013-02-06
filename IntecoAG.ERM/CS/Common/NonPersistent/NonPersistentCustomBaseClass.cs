using System;

namespace IntecoAG.ERM.CS
{

    //[DevExpress.ExpressApp.DC.DomainComponent]
    /// <summary>
    /// Базовый класс для создания NonPersistent-объектов
    /// </summary>
    [DevExpress.Xpo.NonPersistent]
    public abstract class NonPersistentCustomBaseClass
    {
        public NonPersistentCustomBaseClass() { }
        
        public NonPersistentCustomBaseClass(DevExpress.Xpo.Session Session) { _Session = Session; }


        /// <summary>
        /// Session
        /// </summary>
        private DevExpress.Xpo.Session _Session;
        public DevExpress.Xpo.Session Session {
            get { return _Session; }
            set { _Session = value; }
        }
    }
}
