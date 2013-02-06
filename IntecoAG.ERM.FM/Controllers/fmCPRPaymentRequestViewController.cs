using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
//
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCPRPaymentRequestViewController : ObjectViewController {

        public static string DO_ENABLED = "DO_ENABLED";
        public static string DO_ACTIVE = "DO_ACTIVE";

        public fmCPRPaymentRequestViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            EnableButton();

            // Настройка Надписей и Подсказок на кнопках
            //ActionAppearance();
        }

        private void EnableButton() {
            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCPRPaymentRequest == null)
                return;
            fmCPRPaymentRequest current = View.CurrentObject as fmCPRPaymentRequest;
            if (current.State != PaymentRequestStates.IN_PAYMENT && current.State != PaymentRequestStates.IN_BANK) {
                this.InBankAction.Enabled[DO_ENABLED] = false;
                this.InPaymentAction.Enabled[DO_ENABLED] = false;
            } else if (current.State == PaymentRequestStates.IN_PAYMENT) {
                this.InBankAction.Enabled[DO_ENABLED] = true;
                this.InPaymentAction.Enabled[DO_ENABLED] = false;
            } else if (current.State == PaymentRequestStates.IN_BANK) {
                this.InBankAction.Enabled[DO_ENABLED] = false;
                this.InPaymentAction.Enabled[DO_ENABLED] = true;
            }

            // В связи с настройка триады кнопок перехода по бизнесс-процессу, две кнопки пока прячем, 
            // затем их надо будет удалить навсегда. Аминь.
            this.InBankAction.Active[DO_ACTIVE] = false;
            this.InPaymentAction.Active[DO_ACTIVE] = false;
        }

        protected override void OnDeactivated() {
            if (this.InBankAction.Enabled.Contains(DO_ENABLED)) {
                this.InBankAction.Enabled.RemoveItem(DO_ENABLED);
            }
            if (this.InPaymentAction.Enabled.Contains(DO_ENABLED)) {
                this.InPaymentAction.Enabled.RemoveItem(DO_ENABLED);
            }
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            base.OnDeactivated();
        }

        private void StateChange() {
            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCPRPaymentRequest == null)
                return;
            fmCPRPaymentRequest current = View.CurrentObject as fmCPRPaymentRequest;
            if (current.State != PaymentRequestStates.IN_PAYMENT && current.State != PaymentRequestStates.IN_BANK)
                return;
            if (current.State == PaymentRequestStates.IN_BANK)
                current.State = PaymentRequestStates.IN_PAYMENT;
            else
                current.State = PaymentRequestStates.IN_BANK;
            EnableButton();
        }

        private void InBankAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StateChange();
        }

        private void InPaymentAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StateChange();
        }

        private void SuspendAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || e.CurrentObject == null || e.CurrentObject as fmCPRPaymentRequest == null)
                return;
            ObjectSpace.CommitChanges();
            fmCPRPaymentRequest paymentRequest = e.CurrentObject as fmCPRPaymentRequest;
            fmCPRPaymentRequestBusinesLogic.DoSuspendAction(ObjectSpace, paymentRequest);

            ObjectSpace.CommitChanges();
            if (View is DetailView)
                View.Close();
            //fmCPRPaymentRequestBusinesLogic.DoTransitAction(ObjectSpace, e.Object as fmCPRPaymentRequest, (PaymentRequestStates)(e.OldValue), (PaymentRequestStates)(e.NewValue));
        }

        private void ApproveAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || e.CurrentObject == null || e.CurrentObject as fmCPRPaymentRequest == null)
                return;
            ObjectSpace.CommitChanges();
            fmCPRPaymentRequest paymentRequest = e.CurrentObject as fmCPRPaymentRequest;
            fmCPRPaymentRequestBusinesLogic.DoApproveAction(ObjectSpace, paymentRequest);
            ObjectSpace.CommitChanges();
            if (View is DetailView)
                View.Close();
        }

        private void DeclineAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || e.CurrentObject == null || e.CurrentObject as fmCPRPaymentRequest == null)
                return;
            ObjectSpace.CommitChanges();
            fmCPRPaymentRequest paymentRequest = e.CurrentObject as fmCPRPaymentRequest;
            fmCPRPaymentRequestBusinesLogic.DoDeclineAction(ObjectSpace, paymentRequest);
            ObjectSpace.CommitChanges();
            if (View is DetailView)
                View.Close();
        }

    }
}
