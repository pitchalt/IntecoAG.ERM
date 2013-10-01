using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
//
namespace IntecoAG.XafExt.Bpmn {


    public class StateChangedEventArgs : EventArgs {
        public StateChangedEventArgs(Boolean acceptable, Boolean rejectable) {
            _IsAcceptable = acceptable;
            _IsRejectable = rejectable;
        }

        private Boolean _IsAcceptable;
        public Boolean IsAcceptable {
            get { return _IsAcceptable; }
        }

        private Boolean _IsRejectable;
        public Boolean IsRejectable {
            get { return _IsRejectable; }
        }
    };

    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    [DomainComponent]
    [NonPersistentDc]
    public interface XafExtBpmnIAcceptableObject {

        event StateChangedEventHandler StateChangedEvent;

        Boolean IsAcceptable { get; }
        Boolean IsRejectable { get; }

        void Accept(IObjectSpace os);
        void Reject(IObjectSpace os);
    }
}
