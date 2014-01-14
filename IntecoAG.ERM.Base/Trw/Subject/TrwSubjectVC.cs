using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
//
namespace IntecoAG.ERM.Trw.Subject {

//
    public partial class TrwSubjectVC : ViewController {
        public TrwSubjectVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        private TrwSubjectImportDealParameters _ImportActionParameters;

        private void ImportSaleDealsAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            TrwSubject trw_subj = e.CurrentObject as TrwSubject;
            if (trw_subj == null) return;
            
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                trw_subj = os.GetObject<TrwSubject>(trw_subj);

                TrwSubjectLogic.FillSaleDeals(os, trw_subj, _ImportActionParameters);
                os.CommitChanges();
            }
            _ImportActionParameters = null;
        }


        private void ImportSaleDealsAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            _ImportActionParameters = new TrwSubjectImportDealParameters();
            _ImportActionParameters.MaxCount = 0;
            _ImportActionParameters.VolumePercent = 0;
            _ImportActionParameters.CreateOtherDeal = false;
            e.View = Application.CreateDetailView(os, _ImportActionParameters);
//                new CollectionSource(objectSpace, typeof(Note)), true);

        }

        private void ImportBayDealsAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            TrwSubject trw_subj = e.CurrentObject as TrwSubject;
            if (trw_subj == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                trw_subj = os.GetObject<TrwSubject>(trw_subj);
                TrwSubjectLogic.FillBayDeals(os, trw_subj, _ImportActionParameters);
                os.CommitChanges();
            }
            _ImportActionParameters = null;
        }

        private void ImportBayDealsAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            _ImportActionParameters = new TrwSubjectImportDealParameters();
            _ImportActionParameters.MaxCount = 5;
            _ImportActionParameters.VolumePercent = 0.6M;
            _ImportActionParameters.CreateOtherDeal = true;
            e.View = Application.CreateDetailView(os, _ImportActionParameters);
        }

        StateMachineController _StateController;

        protected override void OnActivated() {
            base.OnActivated();
            _StateController = Frame.GetController<StateMachineController>();
            _StateController.TransitionExecuting += new EventHandler<ExecuteTransitionEventArgs>(StateController_TransitionExecuting);
        }

        protected override void OnDeactivated() {
            _StateController.TransitionExecuting -= new EventHandler<ExecuteTransitionEventArgs>(StateController_TransitionExecuting);
            base.OnDeactivated();
        }

        void StateController_TransitionExecuting(object sender, ExecuteTransitionEventArgs e) {
            if (e.Transition.TargetState.StateMachine.GetType() == typeof(TrwSubjectSM)) {
                MyTransition trans = e.Transition as MyTransition;
                if (trans != null && trans.ValidationContext != null) {
                    Validator.RuleSet.Validate(e.TargetObject, trans.ValidationContext);
                }
            } 
        }
    }

    [NonPersistent]
    public class TrwSubjectImportDealParameters {
        public TrwSubjectImportDealParameters() : base() { }

        private Int32 _MaxCount;
        public Int32 MaxCount {
            get { return _MaxCount; }
//            set { SetPropertyValue<Int32>("MaxCount", ref _MaxCount, value); }
            set { _MaxCount = value; }
        }

        private Decimal _VolumePercent;
//        [ModelDefault]
        public Decimal VolumePercent {
            get { return _VolumePercent; }
            //            set { SetPropertyValue<Decimal>("VolumePercent", ref _VolumePercent, value); }
            set { _VolumePercent = value; }
        }

        private Boolean _CreateOtherDeal;
        public  Boolean CreateOtherDeal {
            get { return _CreateOtherDeal; }
            set { _CreateOtherDeal = value; }
        }
    }
}
