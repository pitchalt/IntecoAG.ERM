using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.XafExt;
using IntecoAG.XafExt.Bpmn;

namespace IntecoAG.ERM.Trw.Exchange {

    //public abstract class TrwExchangeDocBase : csCComponent { 
    //    public TrwExchangeDocBase(Session session) : base(session) { }
    //    public override void AfterConstruction() {            
    //        base.AfterConstruction();
    //    }
    //    public abstract class TrwExchangeDocObjectLinkBase : csCComponent {
    //        public TrwExchangeDocObjectLinkBase(Session session) : base(session) { }
    //        public override void AfterConstruction() {            
    //            base.AfterConstruction();
    //        }
    //    }
    //}

    [Persistent("TrwExchangeDoc")]
    public abstract class TrwExchangeDoc : csCComponent, XafExtBpmnIAcceptableObject
    {

        [Persistent("TrwExchangeDocObjectLink")]
        public abstract class ObjectLink : csCComponent {
            public ObjectLink(Session session) : base(session) { }
            
            public override void AfterConstruction() {            
                base.AfterConstruction();
            }

            private TrwExchangeDoc _ExchangeDoc;
            [Association("TrwExchangeDoc-TrwExchangeDocObjectLink")]
            public TrwExchangeDoc ExchangeDoc {
                get { return _ExchangeDoc; }
                set { 
                    SetPropertyValue<TrwExchangeDoc>("ExchangeDoc", ref _ExchangeDoc, value);
                    if (!IsLoading && value != null) {
                        SequenceNumberSet(value.NextLinkNumberGet());
                    }
                }
            }
            [Persistent("SequenceNumber")]
            private Int32 _SequenceNumber;
            [PersistentAlias("_SequenceNumber")]
            [VisibleInListView(true)]
            [VisibleInDetailView(true)]
            public Int32 SequenceNumber {
                get { return _SequenceNumber; }
            }
            public void SequenceNumberSet(Int32 value) {
                Int32 old = _SequenceNumber;
                if (old != value) {
                    _SequenceNumber = value;
                    OnChanged("SequenceNumber", old, value);
                }
            }

            public abstract TrwExchangeIExportableObject ExchangeObject { get; }
        
        }

        public class ObjectLinkCollection<T> : XPCollection<T>
            where T : ObjectLink
        {
            private TrwExchangeDoc _TrwExchangeDoc;
            public ObjectLinkCollection(Session session, TrwExchangeDoc doc): base(session) {
                _TrwExchangeDoc = doc;
                foreach (T item in doc.ObjectLinks) {
                    this.Add(item);
                }
            }

            public TrwExchangeDoc TrwExchangeDoc {
                get { return _TrwExchangeDoc; }
            }

            public override int BaseAdd(object newObject) {
                TrwExchangeDoc.ObjectLinks.Add(newObject as ObjectLink);
                return base.BaseAdd(newObject);
            }

            public override bool BaseRemove(object theObject) {
                TrwExchangeDoc.ObjectLinks.Remove(theObject as ObjectLink);
                return base.BaseRemove(theObject);
            }

        }

        public TrwExchangeDoc(Session session) : base(session) { }
        public override void AfterConstruction() {            
            base.AfterConstruction();
            _DateCreate = DateTime.Now;
            _DocDate = _DateCreate;
            StateSet(TrwExchangeExportStates.CREATED);
        }

        [Persistent("State")]
        private TrwExchangeExportStates _State;
        [PersistentAlias("_State")]
        public TrwExchangeExportStates State {
            get { return _State; }
        }
        public void StateSet(TrwExchangeExportStates value) {
            TrwExchangeExportStates old = _State;
            if (old != value) {
                _State = value;
                OnChanged("State", old, value);
                if (StateChangedEvent != null)
                    StateChangedEvent(this, new StateChangedEventArgs(IsAcceptable, IsRejectable));
            }
        }

        [Persistent("NextLinkNumber")]
        protected Int32 _NextLinkNumber;

        public Int32 NextLinkNumberGet() {
            _NextLinkNumber++;
            return _NextLinkNumber;
        }

        [Persistent("DateCreate")]
        private DateTime _DateCreate;

        [PersistentAlias("_DateCreate")]
        public DateTime DateCreate {
            get { return _DateCreate; }
        }

        [Browsable(false)]
        [Aggregated]
        [Association("TrwExchangeDoc-TrwExchangeDocObjectLink")]
        public XPCollection<ObjectLink> ObjectLinks {
            get { return GetCollection<ObjectLink>("ObjectLinks"); }
        }

        String _DocNumber;
        public String DocNumber {
            get { return _DocNumber; }
            set { SetPropertyValue<String>("DocNumber", ref _DocNumber, value); }
        }

        private DateTime _DocDate;
        public DateTime DocDate {
            get { return _DocDate; }
            set { SetPropertyValue<DateTime>("DocDate", ref _DocDate, value); }
        }

        [Persistent("DocDateConfirm")]
        private DateTime _DocDateConfirm;
        [PersistentAlias("_DocDateConfirm")]
        public DateTime DocDateConfirm {
            get { return _DocDateConfirm; }
        }

        #region XafExtBpmnIAcceptableObject

        public event StateChangedEventHandler StateChangedEvent;

        public virtual bool IsAcceptable {
            get { 
                return State == TrwExchangeExportStates.CREATED;
            }
        }

        public virtual bool IsRejectable {
            get { return false; }
        }

        public virtual void Accept(IObjectSpace os) {
            _DocDateConfirm = DateTime.Now;
            OnChanged("DocDateConfirm");
            StateSet(TrwExchangeExportStates.EXPORTED);
        }

        public virtual void Reject(IObjectSpace os) {
        }

        #endregion
    }
}
