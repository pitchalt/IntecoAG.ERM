using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.NonPersistent;

namespace IntecoAG.ERM.FM.Order {
    public class fmCOrderManageDocSM : StateMachine<fmCOrderManageDoc>, IStateMachineUISettings {

        private IState StateExecution;

        public fmCOrderManageDocSM() {
            StateExecution = new State(this, fmIOrderManageDocStatus.Execution);
            IState StateAcceptMaker = new State(this, fmIOrderManageDocStatus.AcceptMaker);
            IState StateAcceptPlanDepartment = new State(this, fmIOrderManageDocStatus.AcceptPlanDepartment);
            IState StateAcceptAccountDepartment = new State(this, fmIOrderManageDocStatus.AcceptAccountDepartment);
            IState StateRejected = new State(this, fmIOrderManageDocStatus.Rejected);

            StateExecution.Transitions.Add(new Transition(StateAcceptMaker,"Утвердить", 1));
            StateExecution.Transitions.Add(new Transition(StateRejected, "Отклонить", 2));
            StateAcceptMaker.Transitions.Add(new Transition(StateExecution, "Доработать", 1));
            StateAcceptMaker.Transitions.Add(new Transition(StateAcceptPlanDepartment, "Утвердить", 2));
            StateAcceptMaker.Transitions.Add(new Transition(StateRejected, "Отклонить", 3));
            StateAcceptPlanDepartment.Transitions.Add(new Transition(StateAcceptAccountDepartment, "Утвердить", 1));
            StateAcceptPlanDepartment.Transitions.Add(new Transition(StateAcceptMaker, "Вернуть", 2));

            States.Add(StateExecution);
            States.Add(StateAcceptMaker);
            States.Add(StateAcceptPlanDepartment);
            States.Add(StateAcceptAccountDepartment);
            States.Add(StateRejected);
        }
        
        public override IState StartState {
            get { return StateExecution; }
        }

        public override string Name {
            get { return "Изменить статус"; }
        }

        public override string StatePropertyName {
            get { return "Status"; }
        }

        public bool ExpandActionsInDetailView {
            get { return true; }
        }
    }
}
