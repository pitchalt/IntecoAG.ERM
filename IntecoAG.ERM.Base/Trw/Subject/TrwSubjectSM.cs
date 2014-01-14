using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.NonPersistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.Trw.Subject {

    public class TrwSubjectSM : StateMachine<TrwSubject>, IStateMachineUISettings {
        private IState StateInit;

        public TrwSubjectSM() {
            StateInit = new State(this, TrwSubjectStatus.TRW_SUBJECT_INIT);
            IState StateConfSubjectList = new State(this, TrwSubjectStatus.TRW_SUBJECT_CONF_SUBJECT_LIST);
            IState StateConfDealList = new State(this, TrwSubjectStatus.TRW_SUBJECT_CONF_DEAL_LIST);
            IState StateDeleted = new State(this, TrwSubjectStatus.TRW_SUBJECT_DELETE);

            StateInit.Transitions.Add(new MyTransition(StateConfSubjectList, "Утвердить список тем", 1, "Approve"));
            StateConfSubjectList.Transitions.Add(new MyTransition(StateConfDealList, "Утвердить список договоров", 1, "Approve"));
            StateConfSubjectList.Transitions.Add(new MyTransition(StateInit, "Отклонить", 2, null));
            StateConfDealList.Transitions.Add(new MyTransition(StateConfDealList, "Удалить", 1, null));
            StateConfDealList.Transitions.Add(new MyTransition(StateConfSubjectList, "Отклонить", 2, null));
            StateDeleted.Transitions.Add(new MyTransition(StateConfDealList, "Отклонить", 1, null));
 
            States.Add(StateInit);
            States.Add(StateConfSubjectList);
            States.Add(StateConfDealList);
            States.Add(StateDeleted);
        }

        public override IState StartState {
            get { return StateInit; }
        }

        public override string Name {
            get { return "Изменить Статус"; }
        }

        public override string StatePropertyName {
            get { return "Status"; }
        }

        public bool ExpandActionsInDetailView {
            get { return true; }
        }
    }

    public class MyTransition : Transition {
        public MyTransition(IState state, String caption, Int32 level, String validation_context)
            : base(state, caption, level) {
            ValidationContext = validation_context;
        }
        public String ValidationContext;
    }

}
