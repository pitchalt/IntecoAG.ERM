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
using IntecoAG.IBS.SyncService.Messages.XTB;
//
namespace IntecoAG.ERM.HRM {
    
    public partial class StaffSyncController : LongOperationController {
        public StaffSyncController() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected class StaffSyncLongOperation : LongOperation {
            public StaffSyncLongOperation(  LongOperationDelegate longOperationDelegate, 
                                            ISyncService sync_service, 
                                            XWTBXCOA staff_list) : base(longOperationDelegate) 
            {
                _SyncService = sync_service;
                _StaffList = staff_list;
            }
            //            public StaffSyncLongOperation(LongOperationParametrizedDelegate longOperationParametrizedDelegate);
            private ISyncService _SyncService;
            private XWTBXCOA _StaffList;

            public ISyncService SyncService {
                get { return _SyncService; }
            }
            public XWTBXCOA StaffList {
                get { return _StaffList; } 
            }
        }

//        private HTTPSyncService _SyncService;
//        private XWTBXCOA _StaffList;

        private void SyncAllAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HTTPSyncService _SyncService = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
            XWTBXCIA lprm = new XWTBXCIA();
            lprm.CMD = "CATALOG";
            XWTBXCOA _StaffList = _SyncService.XWTBXC0N(lprm);
            LongOperation long_operation = new StaffSyncLongOperation(StaffListSyncProcess, _SyncService, _StaffList);
            StartLongOperation(long_operation);
        }

        private void SyncChangesAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HTTPSyncService _SyncService = new HTTPSyncService(ConfigurationManager.AppSettings["IBS.SyncService"]);
            XWTBXCIA lprm = new XWTBXCIA();
            lprm.CMD = "CHANGES";
            lprm.DPGROUPCODE = 2;
            lprm.UPDTSTART = new DateTime(2011, 01, 01);
            lprm.UPDTSTOP = new DateTime(2011, 01, 31);
            XWTBXCOA _StaffList = _SyncService.XWTBXC0N(lprm);
            LongOperation long_operation = new StaffSyncLongOperation(StaffListSyncProcess, _SyncService, _StaffList);
            StartLongOperation(long_operation);
        }

        protected void StaffListSyncProcess(LongOperation longOperation) {
            StaffSyncLongOperation operation = (StaffSyncLongOperation)longOperation;
            XWTBXLIA msg_in = new XWTBXLIA();
            //Random random = new Random();
            try {
                //            IList<FullyAuditedBatchCreationObject> collection = updatingObjectSpace.GetObjects<FullyAuditedBatchCreationObject>();
                int current = 0;
                int count = 0;
                //Assert.AreEqual(lres.VOLIST.Count, 20);
                foreach (var item in operation.StaffList.TBLIST) {
                    //                System.Console.WriteLine(item.TBCODE + " " + item.TBBUHCODE + " " + item.TBDPCODE);
                    count++;
                    current++;
                    msg_in.TBBUHCODE.Add(item.TBBUHCODE);
                    if (count >= 100 || current == operation.StaffList.TBLIST.Count) {
                        if (longOperation.Status == LongOperationStatus.Cancelling) {
                            return;
                        }
                        msg_in.CMD = "LIST";
                        XWTBXLOA list_res = operation.SyncService.XWTBXL0N(msg_in);
                        using (IObjectSpace os = Application.CreateObjectSpace()) {
                            foreach (var item2 in list_res.TBLIST) {
                                System.Console.WriteLine(item2.TBCODE + " " + item2.TBLASTNAME + " " + item2.TBFIRSTNAME + " " + item2.TBMIDDLENAME + " " + item2.TBDPCODE);
                                hrmStaff staff;
                                IList<hrmStaff> staffs = os.GetObjects<hrmStaff>(new BinaryOperator("BuhCode", item2.TBBUHCODE.ToString(), BinaryOperatorType.Equal));
                                if (staffs.Count > 1 || staffs.Count < 0)
                                    continue;
                                if (staffs.Count == 1)
                                    staff = staffs[0];
                                else
                                    staff = os.CreateObject<hrmStaff>();
                                //staff.Code = item2.TBCODE;
                                staff.BuhCode = item2.TBBUHCODE.ToString();
                                staff.FirstName = item2.TBFIRSTNAME;
                                staff.MiddleName = item2.TBMIDDLENAME;
                                staff.LastName = item2.TBLASTNAME;
                                if (item2.TBSEX == "Ж")
                                    staff.Sex = CRM.Party.crmPhysicalPersonSex.FEMALE;
                                else
                                    staff.Sex = CRM.Party.crmPhysicalPersonSex.MALE;
                                //
                                //staff.DateBegin = item2.TBDTBEGIN;

                                IList<hrmDepartment> deps = os.GetObjects<hrmDepartment>(
                                    new BinaryOperator("BuhCode", item2.TBDPCODE.ToString(), BinaryOperatorType.Equal));
                                if (deps.Count > 0)
                                    staff.Department = deps[0];
                                //                                staff.IsClosed = item2.TBISCLOSED;
                            }
                            os.CommitChanges();
                        }
                        if (longOperation.Status == LongOperationStatus.InProgress) {
                            longOperation.RaiseProgressChanged(
                                (int)((++current * 100) / operation.StaffList.TBLIST.Count), 
                                "Update Staff " + current.ToString() + " from " + operation.StaffList.TBLIST.Count.ToString());
                            //longOperation.RaiseProgressChanged((int)((++index * 100) / dpl.Count), "Update Departnent " + index.ToString() + " from " + msg_out.DPLIST.Count.ToString());
                        }
                        count = 0;
                        msg_in = new XWTBXLIA();
                    }
                }
            }
            catch (LongOperationTerminateException) {
                longOperation.CancelAsync();
            }
            catch (Exception e) {
                longOperation.TerminateAsync();
                throw e;
            }
        }

        protected override IProgressControl CreateProgressControl() {
            return new ProgressForm("Update department List", 0, 100);
        }

    }
}
