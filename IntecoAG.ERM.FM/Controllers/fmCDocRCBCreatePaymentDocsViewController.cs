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

namespace IntecoAG.ERM.FM.Controllers {
    public partial class fmCDocRCBCreatePaymentDocsViewController : ViewController {

        public fmCDocRCBCreatePaymentDocsViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void CreatePaymentDocuments_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View == null || View.CurrentObject == null) return;

            // Создание платёжных документов для всех непривязавшихся документов выписок
            fmCSAMapDocs<fmCDocRCB> mpCreateDocs = this.ObjectSpace.CreateObject<fmCSAMapDocs<fmCDocRCB>>();

            // Создание платёжных документов для всех непривязанных документов выписок всех типов.
            mpCreateDocs.CreatePaymentDocuments<fmCDocRCBPaymentOrder>(View.CurrentObject as fmCSAStatementAccount);
            this.ObjectSpace.CommitChanges();

            mpCreateDocs.CreatePaymentDocuments<fmCDocRCBPaymentRequest>(View.CurrentObject as fmCSAStatementAccount);
            this.ObjectSpace.CommitChanges();

            mpCreateDocs.CreatePaymentDocuments<fmCDocRCBAkkreditivRequest>(View.CurrentObject as fmCSAStatementAccount);
            this.ObjectSpace.CommitChanges();

            mpCreateDocs.CreatePaymentDocuments<fmCDocRCBInkassOrder>(View.CurrentObject as fmCSAStatementAccount);
            this.ObjectSpace.CommitChanges();

            mpCreateDocs.CreatePaymentDocuments<fmCDocRCBOthers>(View.CurrentObject as fmCSAStatementAccount);
            this.ObjectSpace.CommitChanges();
        }
    }
}
