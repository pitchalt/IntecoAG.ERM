using System;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Demos;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XZK;
//
namespace IntecoAG.ERM.SyncIBS.FM {

    public partial class SyncIBSOrderViewController : LongOperationController {
        public SyncIBSOrderViewController() {
            InitializeComponent();
            RegisterActions(components);
        }
        private void SyncDepartmentListAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StartLongOperation(new LongOperation(DoWorkCore));
        }

        protected void DoWorkCore(LongOperation longOperation) {
            //Random random = new Random();
            IObjectSpace os = Application.CreateObjectSpace();
            //            IList<FullyAuditedBatchCreationObject> collection = updatingObjectSpace.GetObjects<FullyAuditedBatchCreationObject>();
            int index = 0;
            try {
                IList<OrderExchangeLogic.OrderShort> short_orders = OrderExchangeLogic.Catalog(os);
                IList<fmCOrderExt> orders = os.GetObjects<fmCOrderExt>();
                foreach (OrderExchangeLogic.OrderShort short_order in short_orders) {
                    fmCOrderExt order = orders.First(item => item.Code == short_order.Code);
                    if (order == null) continue;
                    if (!short_order.IsClosed) {
                        if (order.Status == fmIOrderStatus.Project || order.Status == fmIOrderStatus.FinOpened)
                            order.Status = fmIOrderStatus.BuhOpened;
                    }
                    else {
                        order.Status = fmIOrderStatus.BuhClosed;
                        order.IsClosed = true;
                    }
                    os.CommitChanges();
                    //
                    if (longOperation.Status == LongOperationStatus.InProgress) {
                        longOperation.RaiseProgressChanged((int)((++index * 100) / short_orders.Count), "Update Departnent " + index.ToString() + " from " + short_orders.Count.ToString());
                    }
                    if (longOperation.Status == LongOperationStatus.Cancelling) {
                        return;
                    }

                }
            }
            catch (LongOperationTerminateException) {
                os.Rollback();
                longOperation.CancelAsync();
            }
            catch (Exception e) {
                longOperation.TerminateAsync();
                throw e;
            }
            //os.Rollback();
        }

        protected override IProgressControl CreateProgressControl() {
            return new ProgressForm("Update department List", 0, 100);
        }
    }
}
