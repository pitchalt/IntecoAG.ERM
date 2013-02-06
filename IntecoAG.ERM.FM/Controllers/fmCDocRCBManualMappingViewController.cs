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
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCDocRCBManualMappingViewController : ViewController {

        public fmCDocRCBManualMappingViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void MapDocuments_Execute(object sender, SimpleActionExecuteEventArgs e) {
            // Обработка привязок документов выписок к платёжным документам. Кнопка "Сохранить" получается как бы лишней

            if (View == null || View.CurrentObject == null || View.CurrentObject as fmCPRRepaymentJurnal == null) return;

            fmCPRRepaymentJurnal rr = View.CurrentObject as fmCPRRepaymentJurnal;
//            foreach (fmCSAStatementAccountDoc doc in rr.StatementAccountDocuments) {
//               doc.PaymentDocument = rr.PaymentDocument;
//            }
           
            this.ObjectSpace.CommitChanges();

            // Обновление списка
//            rr.StatementAccountDocuments.Reload();
        }
    }
}
