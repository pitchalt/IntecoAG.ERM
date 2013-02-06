using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.FM.Account;
//

namespace IntecoAG.ERM.FM.Controllers {
    /// <summary>
    /// Данный контроллер поддерживает бизнес-процесс обработки задачи на обработку заявки на оплату.
    /// Заявки бывают нескольких видов: на оплату по договору, разового платежа и по служебной записке.
    /// Обработка в целом выражается в том, что заявка проходит оценку в нескольких подразделениях, которые её либо
    /// отклоняют, либо утверждают. После утверждения финансовым отделом заявка считается подготовленной к оплате.
    /// Список подразделений, которые обрабатывают заявку: 
    /// Курирую.щее подразделение создаёт заявку
    /// Плановый отдел утверждает служебную записку
    /// Договорной отдел утверждает заявки на оплату по договору или разовый платёж
    /// Бюджетно-аналитический отдел проверяет все заявки
    /// Финансовый отдел проверяет все заявки
    /// </summary>
    public partial class fmPaymentRequestTaskSupportViewController : ViewController {

        protected enum USER_TYPE{
            ACCEPT,
            BUDJET,
            PAY
        }

        //private BaseObject watchObject;

        public fmPaymentRequestTaskSupportViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() {
            base.OnActivated();
            //View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            //View_CurrentObjectChanged(View, new System.EventArgs());

            //this.ApproveAction.Active["isActive"] = true;
            //this.DeclineAction.Active["isActive"] = true;
            //this.SuspendAction.Active["isActive"] = true;

            //this.ApproveAction.Enabled["isEnabled"] = true;
            //this.DeclineAction.Enabled["isEnabled"] = true;
            //this.SuspendAction.Enabled["isEnabled"] = true;
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            //if (watchObject != null) {
            //    watchObject.Changed -= fmPaymentRequestTaskSupportViewController_Changed;
            //}
            //View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
        }

        //private void fmPaymentRequestTaskSupportViewController_Changed(object sender, ObjectChangeEventArgs e) {
        //    if (e.PropertyName == "State") {
        //        fmCPRPaymentRequestBusinesLogic.DoTransitAction(ObjectSpace, e.Object as fmCPRPaymentRequest, (PaymentRequestStates)(e.OldValue), (PaymentRequestStates)(e.NewValue));
        //    }
        //}

        protected USER_TYPE user_type;

        protected override void OnViewChanged() {
            base.OnViewChanged();
            if (View != null && View.ObjectSpace != null) {
                fmCSettingsFinance fs = fmCSettingsFinance.GetInstance(((ObjectSpace)View.ObjectSpace).Session);
            }

            //if (View.CurrentObject as fmCPRPaymentRequest != null && watchObject != null) {
            //    watchObject = View.CurrentObject as BaseObject;
            //    watchObject.Changed -= fmPaymentRequestTaskSupportViewController_Changed;
            //    watchObject = null;
            //}
        }

        /*
        // Реализовано через AppearanceAttribute в классе fmCPRPaymentRequest
        void View_CurrentObjectChanged(object sender, EventArgs e) {
            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCPRPaymentRequest == null) return;

            fmCPRPaymentRequest paymentRequest = View.CurrentObject as fmCPRPaymentRequest;

            this.ApproveAction.Enabled.SetItemValue(this.GetType().ToString(), fmCPRPaymentRequestBusinesLogic.EnableApproveAction(paymentRequest));
            this.DeclineAction.Enabled.SetItemValue(this.GetType().ToString(), fmCPRPaymentRequestBusinesLogic.EnableDeclinAction(paymentRequest));

             Подписка на событие OnChanged объекта
            if (watchObject != null) {
                watchObject.Changed -= fmPaymentRequestTaskSupportViewController_Changed;
            }
            watchObject = null;
            if (View.CurrentObject as fmCPRPaymentRequest != null) {
                watchObject = View.CurrentObject as BaseObject;
                watchObject.Changed += fmPaymentRequestTaskSupportViewController_Changed;
            }
        }
        */

        private void ApproveRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || e.CurrentObject == null || e.CurrentObject as fmCPRPaymentRequest == null) return;
            ObjectSpace.CommitChanges();
            fmCPRPaymentRequest paymentRequest = e.CurrentObject as fmCPRPaymentRequest;
            fmCPRPaymentRequestBusinesLogic.DoApproveAction(ObjectSpace, paymentRequest);
            ObjectSpace.CommitChanges();
            if (View is DetailView)
                View.Close();
        }

        private void DeclineRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || e.CurrentObject == null || e.CurrentObject as fmCPRPaymentRequest == null) return;
            ObjectSpace.CommitChanges();
            fmCPRPaymentRequest paymentRequest = e.CurrentObject as fmCPRPaymentRequest;
            fmCPRPaymentRequestBusinesLogic.DoDeclineAction(ObjectSpace, paymentRequest);
            ObjectSpace.CommitChanges();
            if (View is DetailView)
                View.Close();
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

    }
}
