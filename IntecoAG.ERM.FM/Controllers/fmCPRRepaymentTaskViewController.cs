using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Reflection;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
//
using IntecoAG.ERM.Module;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.FM.FinIndex;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.Docs;
//
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.PaymentRequest;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.FM.Controllers {

    /// <summary>
    /// �� �������� ��������� ������� ����������� �������� ��������
    /// </summary>
    public partial class fmCPRRepaymentTaskViewController : ViewController //, IMasterDetailViewInfo
    {

        //ListViewProcessCurrentObjectController lvp = null;
        RefreshController refreshController = null;

        public fmCPRRepaymentTaskViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private fmCPRRepaymentTask current = null;
        //private ListViewProcessCurrentObjectController processCurrentObjectController = null;
        //private fmCSAStatementAccountDoc currentStatementDoc = null;

        //private fmCPRPaymentRequest selectedPaymentRequest = null;

        protected override void OnDeactivated() {
            var sactions = this.Actions.Where(sa => sa is SimpleAction);
            foreach (SimpleAction sa in sactions) {
                if (sa.Active.Contains("VISIBLE")) {
                    sa.Active.RemoveItem("VISIBLE");
                }
            }
            if (refreshController != null) {
                refreshController.RefreshAction.Execute -= new SimpleActionExecuteEventHandler(RefreshAction_Execute);
            }
            base.OnDeactivated();
        }

        protected override void OnActivated() {
            base.OnActivated();

            FillNewRequestItems();

            if ((View.CurrentObject as fmCPRRepaymentTask) != null ) {
                if ((View.CurrentObject as fmCPRRepaymentTask).State == RepaymentTaskStates.UNKNOWN) {
                    UnknownAction.Active["VISIBLE"] = false;
                    CloseUnknownAction.Active["VISIBLE"] = true;
                } else {
                    UnknownAction.Active["VISIBLE"] = true;
                    CloseUnknownAction.Active["VISIBLE"] = false;
                }

                // � ������ ������������� ������� - ������������ ������ ������ ��� ��������
                if ((View.CurrentObject as fmCPRRepaymentTask).State == RepaymentTaskStates.UNKNOWN) {
                    (View.CurrentObject as fmCPRRepaymentTask).FillRequestList();
                }
            }
        }
        
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            refreshController = Frame.GetController<RefreshController>();
            if (refreshController == null)
                return;

            refreshController.RefreshAction.Execute += new SimpleActionExecuteEventHandler(RefreshAction_Execute);
        }

        private void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            // � ������ ������������� ������� - ������������ ������ ������ ��� ��������
            if ((View.CurrentObject as fmCPRRepaymentTask) != null && (View.CurrentObject as fmCPRRepaymentTask).State == RepaymentTaskStates.UNKNOWN) {
                (View.CurrentObject as fmCPRRepaymentTask).FillRequestList();
            }
        }

        /// <summary>
        /// ��������� �� ���� ������� ������ ������ ������������ �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnknownAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRRepaymentTask;
            if (current == null)
                return;

            current.State = RepaymentTaskStates.UNKNOWN;
            ObjectSpace.CommitChanges();
        }

        /// <summary>
        /// �������� �������������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseUnknownAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            current = View.CurrentObject as fmCPRRepaymentTask;
            if (current == null)
                return;

            current.CloseUnknownPayment();
            ObjectSpace.CommitChanges();
        }

        /// <summary>
        /// ������� ������ ���������� ���� � �������� � ��������� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRequestAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.SelectedChoiceActionItem != null) {
                // ������� � ����� ������� ������ � ��������� ���� (������������ ������� ������������ �)
                IObjectSpace workOS = ObjectSpace.CreateNestedObjectSpace();
                //IObjectSpace workOS = ObjectSpace;

                //fmCPRPaymentRequest request = CreateRequest(e.SelectedChoiceActionItem.Data as SuperRequest, workOS);
                SuperRequest superRequest = e.SelectedChoiceActionItem.Data as SuperRequest;
                fmCPRPaymentRequest request = fmCPRPaymentRequestBusinesLogic.CreateRequest(workOS, View.CurrentObject as fmCPRRepaymentTask, superRequest.RequestType as Type, GetFinRequestKindByIndex(superRequest.RequestKind));

                Boolean byFin = false;
                if (request.State == PaymentRequestStates.FINANCE_PAYMENT) byFin = true;

                if (request != null) {
                    // ��������
                    BindRequest(request, workOS);
                }

                if (byFin) request.State = PaymentRequestStates.FINANCE_PAYMENT; 

                // �������� ��������
                ShowRequest(workOS, request, e.ShowViewParameters);   //, workOS);

                // ����� ������ � ������
                NewRequestAction.SelectedItem = null;

                // �� ����������� ����� � Root ����� ���������� ���������� ObjectSpace � ������ ��� � ������ ���� ������

            }            
        }

        /// <summary>
        /// ��������� ��������� ������, �������� � ������ ���� ������
        /// </summary>
        /// <param name="obj"></param>
        private void BindRequest(fmCPRPaymentRequest request, IObjectSpace workOS) {
            if (View.CurrentObject as fmCPRRepaymentTask == null)
                return;
            if (request == null)
                return;
            fmCPRRepaymentTask task = View.CurrentObject as fmCPRRepaymentTask;
            fmCPRRepaymentTask nTask = workOS.GetObject<fmCPRRepaymentTask>(task);
                
            // ��������� � ������ ������ � ������
            nTask.AddRequestToAll(request, true);

            // ���������
            nTask.DoBindingRequest(request, true, 0);
        }
        
        /*
        private fmCPRPaymentRequest CreateRequest(SuperRequest superRequest, IObjectSpace workOS) {
            Type objType = superRequest.RequestType as Type;

            fmCPRRepaymentTask repaymentTask = View.CurrentObject as fmCPRRepaymentTask;
            if (repaymentTask == null) return null;

            fmCPRPaymentRequest req = workOS.CreateObject(objType) as fmCPRPaymentRequest;
            if (req != null) {
                req.State = PaymentRequestStates.OPEN;
                //req.ExtDocDate = DateTime.Now;
                req.Date = DateTime.Now;
                if (repaymentTask.PaymentDocument != null) {
                    fmCDocRCB doc = repaymentTask.PaymentDocument;
                    fmCDocRCB nDoc = workOS.GetObject<fmCDocRCB>(doc);
                    if (nDoc.PaymentPayerRequisites != null)
                        req.PartyPaySender = nDoc.PaymentPayerRequisites.Party;
                    if (nDoc.PaymentReceiverRequisites != null)
                        req.PartyPayReceiver = nDoc.PaymentReceiverRequisites.Party;
//                    req.Number = nDoc.DocNumber;
                    req.ExtDocNumber = nDoc.DocNumber;
                    req.ExtDocDate = nDoc.DocDate;
//                    req.Date = DateTime.Now;
                    req.Summ = nDoc.PaymentCost;   // �� ��������� ���������� ������ ����� ��������� ���������
                    req.PayDate = nDoc.GetAccountDateChange();
                    req.PaymentValuta = nDoc.GetAccountValuta();
                    //req.Valuta = nDoc.GetAccountValuta();   // �� ��������� ��������� ������ ������� � �������� ������ ������������
                    req.Comment = nDoc.PaymentFunction;

                    // ������ ���������� ������
                    fmCPRPaymentRequestFinOrder reqFin = req as fmCPRPaymentRequestFinOrder;
                    if (reqFin != null) {
                        int requestKindIndex = superRequest.RequestKind;
                        if (requestKindIndex > 0) {
                            FinRequestKind requestKind = GetFinRequestKindByIndex(requestKindIndex);
                            reqFin.FinanceRequestKind = requestKind;

                            if (requestKind == FinRequestKind.BANK_COMISSION || requestKind == FinRequestKind.PAYMENT_PERCENTS) {
                                if (nDoc.PaymentReceiverRequisites.Party == null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount.Bank != null) {
                                    nDoc.PaymentReceiverRequisites.Party = nDoc.PaymentReceiverRequisites.BankAccount.Bank.Party;
                                }
                            }
                            if (requestKind == FinRequestKind.RECEIVING_PERCENTS) {
                                if (nDoc.PaymentPayerRequisites.Party == null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount.Bank != null) {
                                    nDoc.PaymentPayerRequisites.Party = nDoc.PaymentPayerRequisites.BankAccount.Bank.Party;
                                }
                            }
                            if (requestKind == FinRequestKind.PURSHASE_CURRENCY || requestKind == FinRequestKind.SALE_CURRENCY) {
                                if (repaymentTask.BankAccount == nDoc.PaymentPayerRequisites.BankAccount) {
                                    if (nDoc.PaymentReceiverRequisites.Party == null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount != null && nDoc.PaymentReceiverRequisites.BankAccount.Bank != null) {
                                        nDoc.PaymentReceiverRequisites.Party = nDoc.PaymentReceiverRequisites.BankAccount.Bank.Party;
                                    }
                                } else if (repaymentTask.BankAccount == nDoc.PaymentReceiverRequisites.BankAccount) {
                                    if (nDoc.PaymentPayerRequisites.Party == null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount != null && nDoc.PaymentPayerRequisites.BankAccount.Bank != null) {
                                        nDoc.PaymentPayerRequisites.Party = nDoc.PaymentPayerRequisites.BankAccount.Bank.Party;
                                    }
                                }
                            }
                        }

                        reqFin.SetSum(nDoc.PaymentCost);

                        reqFin.State = PaymentRequestStates.FINANCE_PAYMENT;   // ��������� �������������� ������, ������, �����
                        //nDoc.State = PaymentDocProcessingStates.PROCESSED;
                    }
                }
            }
            return req;
        }
        */

        private ShowViewParameters ShowRequest(IObjectSpace nos, BaseObject obj, ShowViewParameters showViewParameters) {   //, IObjectSpace workOS) {
            if (obj == null)
                return showViewParameters;

            //IObjectSpace nos = ObjectSpace.CreateNestedObjectSpace();
            string DetailViewId = Frame.Application.FindDetailViewId(obj.GetType());
            BaseObject passedObj = nos.GetObject<BaseObject>(obj);

            // ��������� ��� ��� ����, ����� ������� ������ ������������
            if (passedObj as fmCPRPaymentRequest != null) {
                (passedObj as fmCPRPaymentRequest).ExtDocDate = DateTime.MinValue; 
            }

            TargetWindow openMode = TargetWindow.NewModalWindow;
            DetailView dv = Frame.Application.CreateDetailView(nos, DetailViewId, true, passedObj);
            ShowViewParameters svp = new ShowViewParameters() {
                CreatedView = dv,
                TargetWindow = openMode,
                Context = TemplateContext.View,
                CreateAllControllers = true
            };
            showViewParameters.Assign(svp);

            return showViewParameters;
        }

        /// <summary>
        /// ���������� ������ ����� ������
        /// </summary>
        private void FillNewRequestItems() {
            if (NewRequestAction.Items.Count() > 0)
                return;

            NewRequestAction.Items.Clear();

            SuperRequest paymentSuperRequestFinItem = new SuperRequest(typeof(fmCPRPaymentRequestFinOrder), -1);
            ChoiceActionItem paymentRequestFinItem = new ChoiceActionItem("1", "���������� ������", paymentSuperRequestFinItem);   //typeof(fmCPRPaymentRequestFinOrder));
            NewRequestAction.Items.Add(paymentRequestFinItem);

            //MemberInfo[] memberInfos = typeof(FinRequestKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
            //for (int i = 0; i < memberInfos.Length; i++) {
            //    //ddProperty.Items.Add(new ListItem(memberInfos[i].Name, memberInfos[i].GetType().Name));
            //    ChoiceActionItem paymentRequestFinEnumItem = new ChoiceActionItem("1." + i.ToString(), "   - " + memberInfos[i].GetType().Name, typeof(fmCPRPaymentRequestFinOrder));
            //    NewRequestAction.Items.Add(paymentRequestFinEnumItem);
            //}

            IModelLocalizationItemBase node = View.Model.Application.Localization["Enums"]["IntecoAG.ERM.FM.PaymentRequest.FinRequestKind"];
            if (node != null) {
                for (int i = 0; i < node.NodeCount; i++) {
                    string name = node.GetNode(i).GetValue<string>("Name");
                    string value = node.GetNode(i).GetValue<string>("Value");
                    FinRequestKind enumItem = (FinRequestKind)Enum.Parse(typeof(FinRequestKind), name);
                    int index = (int)enumItem;
                    SuperRequest paymentSuperRequestFinEnumItem = new SuperRequest(typeof(fmCPRPaymentRequestFinOrder), index);
                    ChoiceActionItem paymentRequestFinEnumItem = new ChoiceActionItem("1." + (i + 1).ToString(), "   -  " + value, paymentSuperRequestFinEnumItem);   //typeof(fmCPRPaymentRequestFinOrder));
                    NewRequestAction.Items.Add(paymentRequestFinEnumItem);
                }
            }

            SuperRequest objRequesContractItem = new SuperRequest(typeof(fmCPRPaymentRequestContract), -1);
            ChoiceActionItem paymentRequesContractItem = new ChoiceActionItem("2", "������ �� ������ �� ��������", objRequesContractItem);   //typeof(fmCPRPaymentRequestContract));
            NewRequestAction.Items.Add(paymentRequesContractItem);

            SuperRequest objRequestSingleItem = new SuperRequest(typeof(fmCPRPaymentRequestSingle), -1);
            ChoiceActionItem akkreditivRequestSingleItem = new ChoiceActionItem("3", "������ �� ������ �� �������� �����", objRequestSingleItem);   //typeof(fmCPRPaymentRequestSingle));
            NewRequestAction.Items.Add(akkreditivRequestSingleItem);

            SuperRequest objRequestMemorandumItem = new SuperRequest(typeof(fmPaymentRequestMemorandum), -1);
            ChoiceActionItem budgetRequestItem = new ChoiceActionItem("4", "��������� ������� �� ������", objRequestMemorandumItem);   //typeof(fmPaymentRequestMemorandum));
            NewRequestAction.Items.Add(budgetRequestItem);
        }

        public class SuperRequest {

            public Type RequestType;
            public int RequestKind;

            public SuperRequest(Type requestType, int requestKind) {
                RequestType = requestType;
                RequestKind = requestKind;
            }
        }

        private FinRequestKind GetFinRequestKindByIndex(int index) {
            if (Enum.IsDefined(typeof(FinRequestKind), index)) {
                return (FinRequestKind)index;
            } else {
                throw new Exception("Index of FinRequestKind enumerator out of bound");
            }
        }

    }

}
