using System;
using System.ComponentModel;
//
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IntecoAG.ERM.FM {

    /// <summary>
    /// Технический объект, позволяющий предотвратить повторное выполнение RequestAutoBindingUpdater
    /// </summary>
    [Persistent("fmAutoBindingUpdater")]
    public class fmAutoBindingUpdater : BaseObject
    {
        public fmAutoBindingUpdater(Session session)
            : base(session) {
        }

        private Boolean _AutoBindingUpdater;

        [Browsable(false)]
        public Boolean AutoBindingUpdater {
            get {
                return _AutoBindingUpdater;
            }
            set {
                SetPropertyValue<Boolean>("AutoBindingUpdater", ref _AutoBindingUpdater, value);
            }
        }
    }

}
