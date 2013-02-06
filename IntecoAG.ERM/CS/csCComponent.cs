using System;
using System.Collections;
using System.ComponentModel;

using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CS {

    [NonPersistent]
    public abstract class csCComponent : BaseObject, csIComponent {

        public csCComponent(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private Guid _CID;
        private Type _Type;
         
        [Browsable(false)]
        [Indexed]
        public virtual Guid CID {
            get { return _CID; }
            set { SetPropertyValue<Guid>("CID", ref _CID, value); }
        }

        [Browsable(false)]
        public virtual Object ComponentObject {
            get {
                foreach (Object obj in
                                this.Session.GetObjects(
                                        this.Session.GetClassInfo(this.ComponentType),
                                        new BinaryOperator("CID", this.CID, BinaryOperatorType.Equal),
                                        null, 1, false, false)) {
                    return obj;
                }
                return null;
            }
        }

        public virtual Object GetInterfaceImplementation(Type inter) {
            return this;
        }

        //[Browsable(false)]
        [ValueConverter(typeof(ConverterType2String))]
        public virtual Type ComponentType {
            get { return _Type; }
            set { SetPropertyValue<Type>("ComponentType", ref _Type, value); }
        }
    }

}