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
    public partial class fmCDocRCBImportResultViewController : ViewController {

        public fmCDocRCBImportResultViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        public static string PROCESS_ENABLED = "PROCESS_ENABLED";
        private fmCSAImportResult current = null;

        protected override void OnActivated() {
            base.OnActivated();

            if (View == null || View.CurrentObject == null) {
                ProcessAction.Enabled[PROCESS_ENABLED] = false;
                return;
            }

            // Управление видимостью кнопки
            // Правила.
            // Если в текущем объекте типа fmCDocRCBImportResult ResultCode = 0, то кнопка не доступна 
            //   (ничего не выполнялось как бы, правильность объекта current не гарантирована).
            // Если в текущем объекте типа fmCDocRCBImportResult ResultCode = -1, то кнопка доступна 
            //   (некоторые счета не зарегистрированы).
            // Если в текущем объекте типа fmCDocRCBImportResult ResultCode = 1, то кнопка доступна 
            //   (пройдена Стадия 1, но не 2 и 3).
            // Если в текущем объекте типа fmCDocRCBImportResult ResultCode = 2, то кнопка доступна 
            //   (пройдена Стадия 2, но не 3, необходимо зачистить мусор, к-й образовлася на непройденной Стадии 3).
            // Если в текущем объекте типа fmCDocRCBImportResult ResultCode = 3, то кнопка не доступна 
            //   (пройдены все стадии автоматически).

            /* Отменена блокировка кнопки вечером 2012-04-11
            current = View.CurrentObject as fmCSAImportResult;
            if (current.ResultCode == 0 || 
                current.ResultCode == -1 || current.ResultCode == 1 || current.ResultCode == 2) {
                ProcessAction.Enabled[PROCESS_ENABLED] = true;
            } else {
                ProcessAction.Enabled[PROCESS_ENABLED] = false;
            }
            */
        }

        private void ProcessAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCSAImportResult;
            if (current == null) return;

            fmCSAStatementAccountImportLogic.ImportProcessDop(this.ObjectSpace, current);
        }

        /// <summary>
        /// Операция автоматической привязки счетов к Заявкам об оплате
        /// </summary>
        private void AutoBinding_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCSAImportResult;
            if (current == null) return;
            current.AutoBinding(null);
            ObjectSpace.CommitChanges();
            DevExpress.XtraEditors.XtraMessageBox.Show("Автоматическая привязка Заявок и платёжных документов произведена успешно");
        }
    }
}
