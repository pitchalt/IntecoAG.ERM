using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.XafExt.DC;

namespace IntecoAG.ERM.FM.Tax {

//    [Persistent("FmTaxНалогПодразделение")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class СтруктурноеПодразделение : НалогСубъект {
        public СтруктурноеПодразделение(Session session)
            : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private Налогоплательщик _Налогоплательщик;
        [Association("FmTaxНалогоплательщик-FmTaxСтруктурноеПодразделение")]
        public Налогоплательщик Налогоплательщик {
            get { return _Налогоплательщик; }
            set { SetPropertyValue<Налогоплательщик>("Налогоплательщик", ref _Налогоплательщик, value); }
        }

        [PersistentAlias("Налогоплательщик.ИНН")]
        public String ИНН {
            get { return Налогоплательщик != null ? Налогоплательщик.ИНН : null; }
        }

        private String _КПП;
        [Size(9)]
        [RuleRequiredField]
        public String КПП {
            get { return _КПП; }
            set {
                SetPropertyValue<String>("КПП", ref _КПП, value);
            }
        }

        public override string ToString() {
            return '(' + ИНН + '/' + КПП + ") " + Наименование;
        }

    }

}
