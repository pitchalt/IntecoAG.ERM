using System;
using System.Collections;
using System.ComponentModel;

using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CS {

    [NonPersistent]
    [Appearance("", AppearanceItemType.Action, "UseCounter != 0", TargetItems = "Delete", Enabled = false)]
    public abstract class csCComponent : BaseObject, csIComponent {
        protected csCComponent() { }

        public csCComponent(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private XPCollection<AuditDataItemPersistent> auditTrail;

        /// <summary>
        /// 
        /// </summary>
        public XPCollection<AuditDataItemPersistent> AuditTrail {
            get {
                if (auditTrail == null) {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }

        private Guid _CID;
        private Type _Type;
        private Int32 _UseCounter;

        [Persistent("ReadOnly")]
        protected Boolean _ReadOnly;

        [Browsable(false)]
        public Int32 UseCounter {
            get { return _UseCounter; }
            set { SetPropertyValue<Int32>("UseCounter", ref _UseCounter, value); }
        }

        protected override void OnDeleting() {
            if (UseCounter != 0)
                throw new InvalidOperationException("Delete error UseCounter <> 0");
            else
                base.OnDeleting();
        }
         
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

        public Boolean ReadOnlyUpdate() {
            Boolean value = ReadOnlyGet();
            Boolean old  = _ReadOnly;
            if (value != old) {
                _ReadOnly = value;
                OnChanged("ReadOnly", old, value);
            }
            return ReadOnly;
        }

        public virtual Boolean ReadOnlyGet() {
            return false;
        }
        /// <summary>
        /// Паша!!! Контроллер обрабатывающий это свойство находиться
        /// в FM модуле fmComponentViewController
        /// </summary>
        [Browsable(false)]
        [PersistentAlias("_ReadOnly")]
        public Boolean ReadOnly {
            get { 
                return _ReadOnly; 
            }
        }
    }

}