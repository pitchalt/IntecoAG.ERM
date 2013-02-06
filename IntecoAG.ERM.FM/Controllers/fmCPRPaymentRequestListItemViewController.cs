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
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.PaymentRequest;

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// По текущему документу выписки открывается платёжный документ
    /// </summary>
    public partial class fmCPRPaymentRequestListItemViewController : ViewController {

        public fmCPRPaymentRequestListItemViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private fmCPRRepaymentTask.PaymentRequestListItem current = null;

        protected override void OnActivated() {
            base.OnActivated();
            current = View.CurrentObject as fmCPRRepaymentTask.PaymentRequestListItem;
        }

        /// <summary>
        /// Сделать привязку выбранной заявки
        /// </summary>
        private void ManualBinding_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRRepaymentTask.PaymentRequestListItem;
            if (current == null) return;

            fmCPRRepaymentTask repaymentTask = current.RepaymentTask;
            if (repaymentTask == null)
                return;

            repaymentTask.ManualBinding(current);

            //repaymentTask.FillRepaymentTaskLines();
            //task.FillRequestList();

            //ObjectSpace.CommitChanges();
        }

        private void RemoveRequest_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRRepaymentTask.PaymentRequestListItem;
            if (current == null)
                return;

            fmCPRRepaymentTask repaymentTask = current.RepaymentTask;
            if (repaymentTask == null)
                return;

            repaymentTask.RemoveRequestFromLineStructure(current.PaymentRequest);
        }

    }
}
