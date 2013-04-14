using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.NonPersistent;

namespace IntecoAG.ERM.FM.Subject {

    public class fmCDirectionSM : StateMachine<fmCDirection>, IStateMachineUISettings {

        private State StateLoaded;

        public fmCDirectionSM() {
            StateLoaded = new State(this, fmIDirectionStatus.LOADED);
            State StateProject = new State(this, fmIDirectionStatus.PROJECT);
            State StateOpened = new State(this, fmIDirectionStatus.OPENED);
            State StateClosed = new State(this, fmIDirectionStatus.CLOSED);

            StateClosed.TargetObjectCriteria = "IsStatusClosedAllow";

            StateLoaded.Transitions.Add(new Transition(StateOpened,"Открыть", 1));
            StateLoaded.Transitions.Add(new Transition(StateClosed, "Закрыть", 2));
            StateProject.Transitions.Add(new Transition(StateOpened, "Открыть", 1));
            StateProject.Transitions.Add(new Transition(StateClosed, "Закрыть", 2));
            StateOpened.Transitions.Add(new Transition(StateClosed, "Закрыть", 1));
            StateClosed.Transitions.Add(new Transition(StateOpened, "Повт.Открыть", 1));

            States.Add(StateLoaded);
            States.Add(StateProject);
            States.Add(StateOpened);
            States.Add(StateClosed);
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
