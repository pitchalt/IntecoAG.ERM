using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.NonPersistent;

namespace IntecoAG.ERM.FM.Order {

    public class fmCOrderSM : StateMachine<fmCOrder>, IStateMachineUISettings {

        private State StateLoaded;

        public fmCOrderSM() {
            StateLoaded = new State(this, fmIOrderStatus.Loaded);
            State StateProject = new State(this, fmIOrderStatus.Project);
            State StateOpened = new State(this, fmIOrderStatus.FinOpened);
            State StateClosed = new State(this, fmIOrderStatus.FinClosed);
            State StateDelete = new State(this, fmIOrderStatus.Deleting);

//            StateClosed.TargetObjectCriteria = "IsStatusClosedAllow";

            StateLoaded.Transitions.Add(new Transition(StateProject, "Проект", 1));
            StateLoaded.Transitions.Add(new Transition(StateOpened, "Открыть", 2));
            StateLoaded.Transitions.Add(new Transition(StateClosed, "Закрыть", 3));
            StateLoaded.Transitions.Add(new Transition(StateDelete, "Удалить", 4));

            StateProject.Transitions.Add(new Transition(StateOpened, "Открыть", 1));
            StateProject.Transitions.Add(new Transition(StateClosed, "Закрыть", 2));
            StateOpened.Transitions.Add(new Transition(StateClosed, "Закрыть", 1));
            StateOpened.Transitions.Add(new Transition(StateProject, "Проект", 2));

            StateClosed.Transitions.Add(new Transition(StateOpened, "Повт.Открыть", 1));
            StateDelete.Transitions.Add(new Transition(StateOpened, "Повт.Открыть", 1));

            States.Add(StateLoaded);
            States.Add(StateProject);
            States.Add(StateOpened);
            States.Add(StateClosed);
            States.Add(StateDelete);
        }

        public override IState StartState {
            get { return StateLoaded; }
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
