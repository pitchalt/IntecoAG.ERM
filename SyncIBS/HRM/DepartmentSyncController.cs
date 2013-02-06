using System;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Demos;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.IBS.SyncService;
using IntecoAG.IBS.SyncService.Messages.XDP;

namespace IntecoAG.ERM.HRM {
    public partial class DepartmentSyncController : LongOperationController {
        public DepartmentSyncController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void SyncDepartmentListAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StartLongOperation();
        }

        protected override void DoWorkCore(LongOperation longOperation) {
            //Random random = new Random();
            IObjectSpace os = Application.CreateObjectSpace();
            //            IList<FullyAuditedBatchCreationObject> collection = updatingObjectSpace.GetObjects<FullyAuditedBatchCreationObject>();
            int index = 0;
            try {
                HTTPSyncService syncservice = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
                XWDPXLIA msg_in = new XWDPXLIA();
                msg_in.CMD = "LIST";
                XWDPXLOA msg_out = syncservice.XWDPXL0N(msg_in);
                foreach (XWDPXLOADPLIST dps in msg_out.DPLIST) {
                    hrmDepartment dp;
                    IList<hrmDepartment> dpl = os.GetObjects<hrmDepartment>(new BinaryOperator("BuhCode", dps.DPBUHCODE.ToString(), BinaryOperatorType.Equal));
                    if (dpl.Count > 0)
                        dp = dpl[0];
                    else
                        dp = os.CreateObject<hrmDepartment>();
                    if (dp.Code != dps.DPCODE)
                        dp.Code = dps.DPCODE;
                    if (dp.BuhCode != dps.DPBUHCODE.ToString())
                        dp.BuhCode = dps.DPBUHCODE.ToString();
                    if (dp.Name != dps.DPNAME)
                        dp.Name = dps.DPNAME;
                    if (dp.IsClosed != dps.DPISCLOSED)
                        dp.IsClosed = dps.DPISCLOSED;
                    os.CommitChanges();
                    //
                    if (longOperation.Status == LongOperationStatus.InProgress) {
                        longOperation.RaiseProgressChanged((int)((++index * 100) / msg_out.DPLIST.Count), "Update Departnent " + index.ToString() + " from " + msg_out.DPLIST.Count.ToString());
//                        longOperation.RaiseProgressChanged((int)((++index * 100) / dpl.Count), "Update Departnent " + index.ToString() + " from " + msg_out.DPLIST.Count.ToString());
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
            //os.Rollback();
        }

        protected override IProgressControl CreateProgressControl() {
            return new ProgressForm("Update department List", 0, 100);
        }

        
    }
}
