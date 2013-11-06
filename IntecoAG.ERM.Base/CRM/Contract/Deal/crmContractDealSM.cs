using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.NonPersistent;
using DevExpress.Persistent.Base;
using BI=DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CRM.Contract.Deal {

    public class crmContractDealSM : StateMachine<crmContractDeal>, IStateMachineUISettings {
        private IState StateProject;

        public crmContractDealSM() {
            StateProject = new State(this, DealStates.DEAL_PROJECT);
            IState StateFormation = new State(this, DealStates.DEAL_FORMATION);
            IState StateResolved = new State(this, DealStates.DEAL_RESOLVED);
            IState StateConcluded = new State(this, DealStates.DEAL_CONCLUDED);
            IState StateClosed = new State(this, DealStates.DEAL_CLOSED);
            IState StateDeleted = new State(this, DealStates.DEAL_DELETED);

            StateProject.Transitions.Add(new Transition(StateClosed, "Закрыть", 1));
            StateProject.Transitions.Add(new Transition(StateDeleted, "Удалить", 2));

            StateFormation.Transitions.Add(new Transition(StateClosed, "Закрыть", 1));
            StateFormation.Transitions.Add(new Transition(StateDeleted, "Удалить", 2));

            States.Add(StateProject);
            States.Add(StateFormation);
            States.Add(StateResolved);
            States.Add(StateConcluded);
            States.Add(StateClosed);
            States.Add(StateDeleted);
        }
        
        public override IState StartState {
            get { return StateProject; }
        }

        public override string Name {
            get { return "Изменить статус"; }
        }

        public override string StatePropertyName {
            get { return "State"; }
        }

        public bool ExpandActionsInDetailView {
            get { return true; }
        }
    }

}
