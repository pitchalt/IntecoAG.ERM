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
using DevExpress.Data.Filtering;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.FinAccount;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XZK;
//
namespace IntecoAG.ERM.SyncIBS.FM {

    public partial class SyncIBSOrderViewController : LongOperationController {
        public SyncIBSOrderViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void SyncOrderListAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StartLongOperation(new LongOperation(DoWorkCore));
        }

        protected void DoWorkCore(LongOperation longOperation) {
            //Random random = new Random();
            IObjectSpace os = Application.CreateObjectSpace();
            //            IList<FullyAuditedBatchCreationObject> collection = updatingObjectSpace.GetObjects<FullyAuditedBatchCreationObject>();
            int index = 0;
            try {
                XWZKXCOA short_orders = SynIBSOrderExchangeLogic.Catalog(os);
                IList<fmCOrderExt> orders = os.GetObjects<fmCOrderExt>();
                IList<fm—OrderAnalitycAccouterType> acc_types = os.GetObjects<fm—OrderAnalitycAccouterType>();
                IList<fmCFAAccount> accounts = os.GetObjects<fmCFAAccount>(new BinaryOperator("AccountSystem.Code", "1000"));
                foreach (XWZKXCOAZKLIST short_order in short_orders.ZKLIST) {
                    fmCOrderExt order = orders.First(item => item.Code == short_order.ZKCODE);
                    if (order == null) {
                        Trace.TraceWarning("IBSOrderSyncAll: Order >" + short_order.ZKCODE + "< not found");
                        continue;
                    }
                    if (!short_order.ZKISCLOSED) {
                        if (order.Status == fmIOrderStatus.Project || order.Status == fmIOrderStatus.FinOpened) {
                            order.Status = fmIOrderStatus.Opened;
                            order.IsClosed = false;
                        }
                    }
                    else {
                        if (order.Status == fmIOrderStatus.Project || order.Status == fmIOrderStatus.FinClosed) {
                            order.Status = fmIOrderStatus.Closed;
                            order.IsClosed = true;
                        }
                    }
                    if (!String.IsNullOrEmpty(short_order.ZKACCOUNTTYPE)) {
                        order.AnalitycAccouterType = acc_types.FirstOrDefault(x => x.Code == short_order.ZKACCOUNTTYPE);
                    }
                    if (!String.IsNullOrEmpty(short_order.ZKACCOUNTCODE)) {
                        order.BuhAccount = accounts.FirstOrDefault(x => x.BuhCode == short_order.ZKACCOUNTCODE);
                    }
                    os.CommitChanges();
                    //
                    if (longOperation.Status == LongOperationStatus.InProgress) {
                        longOperation.RaiseProgressChanged((int)((++index * 100) / short_orders.ZKLIST.Count), "Œ·ÌÓ‚ÎˇÂÏ Á‡Í‡Á " + index.ToString() + " ËÁ " + short_orders.ZKLIST.Count.ToString());
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
            return new ProgressForm("Œ·ÌÓ‚ÎˇÂÏ ÒÔËÒÓÍ Á‡Í‡ÁÓ‚", 0, 100);
        }
    }
}
