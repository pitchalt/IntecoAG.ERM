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
            State StateFinOpened = new State(this, fmIOrderStatus.FinOpened);
            State StateFinClosed = new State(this, fmIOrderStatus.FinClosed);
            State StateOpened = new State(this, fmIOrderStatus.Opened);
            State StateClosed = new State(this, fmIOrderStatus.Closed);
            State StateBlocked = new State(this, fmIOrderStatus.Blocked);
            //            State StateDelete = new State(this, fmIOrderStatus.Deleting);
//            StateClosed.TargetObjectCriteria = "IsStatusClosedAllow";

            StateLoaded.Transitions.Add(new Transition(StateProject, "Проект", 1));
            StateLoaded.Transitions.Add(new Transition(StateFinOpened, "Открыть", 2));
            StateLoaded.Transitions.Add(new Transition(StateFinClosed, "Закрыть", 3));

            StateProject.Transitions.Add(new Transition(StateFinOpened, "Открыть", 1));

            StateOpened.Transitions.Add(new Transition(StateFinClosed, "Закрыть", 1));
            StateOpened.Transitions.Add(new Transition(StateProject, "Редактировать", 2));
            StateOpened.Transitions.Add(new Transition(StateBlocked, "Блокировать", 3));

            StateClosed.Transitions.Add(new Transition(StateFinOpened, "Редактировать", 1));

            StateFinClosed.Transitions.Add(new Transition(StateClosed, "Закрыть", 1));
            StateFinClosed.Transitions.Add(new Transition(StateProject, "Отклонить", 2));

            StateFinOpened.Transitions.Add(new Transition(StateOpened, "Открыть", 1));
            StateFinOpened.Transitions.Add(new Transition(StateProject, "Отклонить", 2));

            StateBlocked.Transitions.Add(new Transition(StateOpened, "Разблокировать", 1));

            States.Add(StateLoaded);
            States.Add(StateProject);
            States.Add(StateFinOpened);
            States.Add(StateOpened);
            States.Add(StateFinClosed);
            States.Add(StateClosed);
            States.Add(StateBlocked);
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
