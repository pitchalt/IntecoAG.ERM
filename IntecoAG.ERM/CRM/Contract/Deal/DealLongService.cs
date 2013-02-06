#region Copyright (c) 2011 INTECOAG.
/*
{*******************************************************************}
{                                                                   }
{       Copyright (c) 2011 INTECOAG.                                }
{                                                                   }
{                                                                   }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2011 INTECOAG.

using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract.Analitic;

namespace IntecoAG.ERM.CRM.Contract.Deal
{

    /// <summary>
    /// ����� crmDealLongService
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Current.ContractDocument.Number", 
        "Current.Customer.Party.Name", 
        "Current.Supplier.Party.Name"
    })]
    //[DefaultClassOptions]
//    [Persistent("crmDealLongService")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class crmDealLongService : crmContractDeal, IVersionBusinessLogicSupport, IVersionMainObject
    {
        public crmDealLongService(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();

            this.Current = new crmDealLongServiceVersion(this.Session, VersionStates.VERSION_NEW);
            //this.DealVersions.Add(this.Current);
            this.DealVersions.Add(this.Current);
            this.ContractDocument = new crmContractDocument(this.Session);
        }


        #region ���� ������

        #endregion


        #region �������� ������

        //private crmDealLongServiceVersion _Current;
        //[Aggregated]
        //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        //public crmDealLongServiceVersion Current {
        //    get { return _Current; }
        //    set { SetPropertyValue<crmDealLongServiceVersion>("Current", ref _Current, value); }
        //}

        #endregion


        #region ������

        #endregion

        #region IVersionBusinessLogicSupport

        public void ApproveVersion(crmDealLongServiceVersion scVersion) {
            crmDealLongServiceVersion newcur = null;
            foreach (crmDealLongServiceVersion cont in this.DealVersions) //DealVersions)
                if (cont == scVersion) newcur = cont;
            if (newcur == null) throw new Exception("Version not in VersionList");
            VersionHelper vHelper;

            if (scVersion.VersionState == VersionStates.VERSION_NEW) {
                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

                // ������������� ���������
                RegisterOperations(scVersion);

            } else if (scVersion.VersionState == VersionStates.VERSION_PROJECT) {

                foreach (crmDealLongServiceVersion cont in this.DealVersions) { //DealVersions) {
                    if (cont == scVersion) continue;
                    if (cont.VersionState == VersionStates.VERSION_CURRENT) {
                        vHelper = new VersionHelper(this.Session);
                        vHelper.SetVersionStateExt(cont, VersionStates.VERSION_OLD);
                    } else if (cont.VersionState == VersionStates.VERSION_PROJECT) {
                        vHelper = new VersionHelper(this.Session);
                        vHelper.SetVersionStateExt(cont, VersionStates.VERSION_DECLINE);
                    }
                }

                vHelper = new VersionHelper(this.Session);
                vHelper.SetVersionStateExt(scVersion, VersionStates.VERSION_CURRENT);

                this.Current = scVersion;

                // ������������� ���������
                RegisterOperations(scVersion);

                Session.FlushChanges();
            }

            // !!!SHU ��������� 4 ������ ����!
            //this.Category = scVersion.Category;
            //this.Contract.ContractDocument.Number = scVersion.ContractDocument.Number;
            //this.Contract.ContractDocument.Date = scVersion.ContractDocument.Date;
            //this.Contract.ContractDocument.DocumentCategory = scVersion.ContractDocument.DocumentCategory;

            //// Set Current for WorkPlans after approve crmComplexContract
            //{
            //    foreach (crmWorkPlanVersion wpv in this.Current.WorkPlanVersions) {
            //        if (wpv.VersionState == VersionStates.VERSION_CURRENT) {
            //            wpv.WorkPlan.Current = wpv;
            //        }

            //        // ������������ ������� � ���������
            //        if (this.Current.ContractPartys.IndexOf(wpv.Customer) == -1) this.Current.ContractPartys.Add(wpv.Customer);
            //        if (this.Current.ContractPartys.IndexOf(wpv.Supplier) == -1) this.Current.ContractPartys.Add(wpv.Supplier);
            //    }

            //    Session.FlushChanges();
            //}
        }


        #region ������ � ����������

        private void RegisterOperations(crmDealLongServiceVersion scVersion) {
            // ������ �������� 
            FindAndDeletePFRegisterRecords(scVersion);
            FindAndDeleteDCDRegisterRecords(scVersion);  // ������� ����� ���� ����������� ���������

            // ��������� ������ � crmPlaneFactRegister ��� scVersion
            AddPFRegisterRecords(scVersion);

            // ��������� ������ � crmDebtorCreditorDebtRegister ��� scVersion
            AddDCDRegisterRecords(scVersion);

            // ��������� ������ � crmCashFlowRegister ��� scVersion
            AddCFRegisterRecords(scVersion);
        }

        private void FindAndDeletePFRegisterRecords(crmDealLongServiceVersion scVersion) {
            // ������� � �������� crmPlaneFactRegister ������ � ���� �� �����������, ��� � scVersion, � ������� �

            //������� �������� Creditor
            //������� �������  Debitor
            //����
            //�����   fmOrder
            //�������  Contract
            //������� ������� (���������) ContracDeal  
            //����  
            //���������� ������   FinancialDeal
            //������ ������ (���)   CostItem
            //������������ ������������   Nomenclature
            //����/����   PlaneFact = PLANE

            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            //CriteriaOperator criteriaDocNumberYear = new BinaryOperator("DocNumberYear", DocNumberYear, BinaryOperatorType.Equal);
            //((GroupOperator)criteria).Operands.Add(criteriaDocNumberYear);

            //CriteriaOperator criteriaDocNumberDepartment = null;
            //if (!string.IsNullOrEmpty(DocNumberDepartment)) {
            //    criteriaDocNumberDepartment = new BinaryOperator("DocNumberDepartment", DocNumberDepartment, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteria).Operands.Add(criteriaDocNumberDepartment);
            //}


            OperandProperty propCreditor = new OperandProperty("Creditor");
            CriteriaOperator opCreditor = propCreditor == scVersion.Customer;
            ((GroupOperator)criteria).Operands.Add(opCreditor);

            OperandProperty propDebitor = new OperandProperty("Debitor");
            CriteriaOperator opDebitor = propDebitor == scVersion.Supplier;
            ((GroupOperator)criteria).Operands.Add(opDebitor);

            OperandProperty propOrder = new OperandProperty("fmOrder");
            CriteriaOperator opOrder = propOrder == scVersion.Order;
            ((GroupOperator)criteria).Operands.Add(opOrder);

            OperandProperty propContract = new OperandProperty("Contract");
            CriteriaOperator opContract = propContract == scVersion.ContractDeal.Contract;
            ((GroupOperator)criteria).Operands.Add(opContract);

            OperandProperty propContracDeal = new OperandProperty("ContracDeal");
            CriteriaOperator opContracDeal = propContracDeal == scVersion.ContractDeal;
            ((GroupOperator)criteria).Operands.Add(opContracDeal);

            OperandProperty propCostItem = new OperandProperty("CostItem");
            CriteriaOperator opCostItem = propCostItem == scVersion.CostItem;
            ((GroupOperator)criteria).Operands.Add(opCostItem);

            //OperandProperty propNomenclature = new OperandProperty("Nomenclature");
            //CriteriaOperator opNomenclature = propNomenclature == this.Current.ContractDeal.Nomenclature;

            OperandProperty propPlaneFact = new OperandProperty("PlaneFact");
            CriteriaOperator opPlaneFact = propPlaneFact == 1;   // PlaneFact.PLAN;
            ((GroupOperator)criteria).Operands.Add(opPlaneFact);

            OperandProperty propValuta = new OperandProperty("Valuta");
            CriteriaOperator opValuta = propValuta == scVersion.Valuta;
            ((GroupOperator)criteria).Operands.Add(opValuta);

            XPCollection<crmPlaneFactRegister> RegColl = new XPCollection<crmPlaneFactRegister>(this.Session, criteria, null);
            if (!RegColl.IsLoaded) RegColl.Load();
            RegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (RegColl.Count > 0) RegColl.Remove(RegColl[0]);
        }

        private void FindAndDeleteDCDRegisterRecords(crmDealLongServiceVersion scVersion) {
            // ������� � ��������� crmDebtorCreditorDebtRegister � crmCashFlowRegister ������ � ���� �� �����������, ��� � scVersion, � ������� �

            //������� ��������
            //����
            //�����
            //������� ����������
            //�������
            //������� �������
            //���������� ������
            //������ ������ (���)
            //����/����

            CriteriaOperator criteria = null;
            criteria = new GroupOperator();
            ((GroupOperator)criteria).OperatorType = GroupOperatorType.And;

            //CriteriaOperator criteriaDocNumberYear = new BinaryOperator("DocNumberYear", DocNumberYear, BinaryOperatorType.Equal);
            //((GroupOperator)criteria).Operands.Add(criteriaDocNumberYear);

            //CriteriaOperator criteriaDocNumberDepartment = null;
            //if (!string.IsNullOrEmpty(DocNumberDepartment)) {
            //    criteriaDocNumberDepartment = new BinaryOperator("DocNumberDepartment", DocNumberDepartment, BinaryOperatorType.Equal);
            //    ((GroupOperator)criteria).Operands.Add(criteriaDocNumberDepartment);
            //}


            OperandProperty propPrimaryParty = new OperandProperty("PrimaryParty");
            CriteriaOperator opPrimaryParty = propPrimaryParty == scVersion.Customer;
            ((GroupOperator)criteria).Operands.Add(opPrimaryParty);

            OperandProperty propContragentParty = new OperandProperty("ContragentParty");
            CriteriaOperator opContragentParty = propContragentParty == scVersion.Supplier;
            ((GroupOperator)criteria).Operands.Add(opContragentParty);

            OperandProperty propOrder = new OperandProperty("fmOrder");
            CriteriaOperator opOrder = propOrder == scVersion.Order;
            ((GroupOperator)criteria).Operands.Add(opOrder);

            OperandProperty propContract = new OperandProperty("Contract");
            CriteriaOperator opContract = propContract == scVersion.ContractDeal.Contract;
            ((GroupOperator)criteria).Operands.Add(opContract);

            OperandProperty propContracDeal = new OperandProperty("ContracDeal");
            CriteriaOperator opContracDeal = propContracDeal == scVersion.ContractDeal;
            ((GroupOperator)criteria).Operands.Add(opContracDeal);

            //OperandProperty propFinancialDeal = new OperandProperty("FinancialDeal");
            //CriteriaOperator opFinancialDeal = propFinancialDeal == scVersion.FinancialDeal.FinancialDeal;
            //((GroupOperator)criteria).Operands.Add(opFinancialDeal);

            OperandProperty propCostItem = new OperandProperty("CostItem");
            CriteriaOperator opCostItem = propCostItem == scVersion.CostItem;
            ((GroupOperator)criteria).Operands.Add(opCostItem);

            //OperandProperty propNomenclature = new OperandProperty("Nomenclature");
            //CriteriaOperator opNomenclature = propNomenclature == this.Current.ContractDeal.Nomenclature;

            OperandProperty propPlaneFact = new OperandProperty("PlaneFact");
            CriteriaOperator opPlaneFact = propPlaneFact == 1;   // PlaneFact.PLAN;
            ((GroupOperator)criteria).Operands.Add(opPlaneFact);

            OperandProperty propValuta = new OperandProperty("Valuta");
            CriteriaOperator opValuta = propValuta == scVersion.Valuta;
            ((GroupOperator)criteria).Operands.Add(opValuta);

            XPCollection<crmDebtorCreditorDebtRegister> dcdRegColl = new XPCollection<crmDebtorCreditorDebtRegister>(this.Session, criteria, null);
            if (!dcdRegColl.IsLoaded) dcdRegColl.Load();
            dcdRegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (dcdRegColl.Count > 0) dcdRegColl.Remove(dcdRegColl[0]);


            XPCollection<crmCashFlowRegister> cfRegColl = new XPCollection<crmCashFlowRegister>(this.Session, criteria, null);
            if (!cfRegColl.IsLoaded) cfRegColl.Load();
            cfRegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (cfRegColl.Count > 0) cfRegColl.Remove(cfRegColl[0]);

        }

        private void AddPFRegisterRecords(crmDealLongServiceVersion scVersion) {
            crmPlaneFactRegister pfr = new crmPlaneFactRegister(this.Session);
            pfr.Contract = scVersion.ContractDeal.Contract;
            pfr.ContractDeal = scVersion.ContractDeal;
            pfr.CostItem = scVersion.CostItem;
            pfr.Creditor = scVersion.Customer.Party;
            pfr.Debitor = scVersion.Supplier.Party;
            pfr.fmOrder = scVersion.Order;
            pfr.PlaneFact = PlaneFact.PLAN;

            //pfr.Nomenclature = scVersion.DeliveryUnit.DeliveryItems;   // SHU!!! �� ����, ������ ����� ������������
            //pfr.FinancialDeal = scVersion.FinancialDeal.FinancialDeal;

            //pfr.MeasureUnit = scVersion.Order;
            //pfr.Volume = scVersion.;

            pfr.Valuta = scVersion.Valuta;
            pfr.Cost = scVersion.Price;
            //pfr.CostInRUR = scVersion.Price;  // ���� ��������� � ������ ������, ����� �� ������ � �.�.

            pfr.Save();
        }

        private void AddDCDRegisterRecords(crmDealLongServiceVersion scVersion) {
            crmDebtorCreditorDebtRegister dcdr = new crmDebtorCreditorDebtRegister(this.Session);
            dcdr.Contract = scVersion.ContractDeal.Contract;
            dcdr.ContractDeal = scVersion.ContractDeal;
            dcdr.CostItem = scVersion.CostItem;
            dcdr.PrimaryParty = scVersion.Customer.Party;
            dcdr.ContragentParty = scVersion.Supplier.Party;
            dcdr.fmOrder = scVersion.Order;
            dcdr.PlaneFact = PlaneFact.PLAN;

            //dcdr.Nomenclature = scVersion.;
            //dcdr.FinancialDeal = scVersion.FinancialDeal.FinancialDeal;

            //dcdr.MeasureUnit = scVersion.Order;
            //dcdr.Volume = scVersion.;

            //---
            dcdr.DebitValuta = scVersion.Valuta;
            dcdr.DebitCost = scVersion.Price;
            //dcdr.DebitCostInRUR = scVersion.Price;  // ���� ��������� � ������ ������, ����� �� ������ � �.�.

            //---
            dcdr.CreditValuta = scVersion.Valuta;
            dcdr.CreditCost = scVersion.Price;
            //dcdr.CreditCostInRUR = scVersion.Price;  // ���� ��������� � ������ ������, ����� �� ������ � �.�.

            //---
            dcdr.BalanceValuta = scVersion.Valuta;
            dcdr.BalanceCost = scVersion.Price;
            //dcdr.BalanceCostInRUR = scVersion.Price;  // ���� ��������� � ������ ������, ����� �� ������ � �.�.

            dcdr.Save();
        }

        private void AddCFRegisterRecords(crmDealLongServiceVersion scVersion) {
            crmCashFlowRegister cfr = new crmCashFlowRegister(this.Session);
            cfr.Contract = scVersion.ContractDeal.Contract;
            cfr.ContractDeal = scVersion.ContractDeal;
            cfr.CostItem = scVersion.CostItem;
            cfr.PrimaryParty = scVersion.Customer.Party;
            cfr.ContragentParty = scVersion.Supplier.Party;
            cfr.fmOrder = scVersion.Order;
            cfr.PlaneFact = PlaneFact.PLAN;

            //cfr.Nomenclature = scVersion.;
            //cfr.FinancialDeal = scVersion.FinancialDeal.FinancialDeal;

            //cfr.MeasureUnit = scVersion.Order;
            //cfr.Volume = scVersion.;

            //---
            cfr.Valuta = scVersion.Valuta;
            cfr.Cost = scVersion.Price;
            //dcdr.CostInRUR = scVersion.Price;  // ���� ��������� � ������ ������, ����� �� ������ � �.�.

            //---
            //cfr.CalculateCost = scVersion.Valuta;   // SHU!!! ������ �����? ��������� ���������.
            //cfr.CalculateValuta = scVersion.Price;   // SHU!!! ������ �����? ������ �������.

            cfr.Save();
        }

        #endregion


        public IVersionSupport CreateNewVersion() {
            VersionHelper vHelper = new VersionHelper(this.Session);
            return vHelper.CreateNewVersion((IVersionSupport)(this.Current), vHelper);
        }

        public void Approve(IVersionSupport obj) {
            crmDealLongServiceVersion approvingObj = obj as crmDealLongServiceVersion;
            if (approvingObj == null) {
                DevExpress.XtraEditors.XtraMessageBox.Show("�������� �� ����� ���� ��������");
                return;
            }
            ApproveVersion(approvingObj);
        }

        #endregion
/*
        #region IContractFactory Members

        BaseObject IContractDealFactory.Create(crmCommonRegistrationForm frm) {
            this.Current.Category = frm.Category;
            //this.Current.ContractDocument.DocumentCategory = frm.Document.DocumentCategory;
            //this.Current.ContractDocument.Number = frm.Document.Number;
            //this.Current.ContractDocument.Date = frm.Document.Date;
            //                wp.Current.Ca
            crmContractParty cp = new crmContractParty(this.Session);
            cp.Party = frm.OurParty;
            //this.Current.ContractPartys.Add(cp);
            cp = new crmContractParty(this.Session);
            cp.Party = frm.PartnerParty;
            //this.Current.ContractPartys.Add(cp);
            this.Current.DateBegin = frm.DateBegin;
            this.Current.DateEnd = frm.DateEnd;

            this.Current.DescriptionShort = frm.DescriptionShort;
            //((crmDealWithoutStageVersion)this.Current).DealCode = frm.ContractDocument.Number;
            this.Category = frm.Category;

            //this.Category
            //this.Contract
            //this.Current
            //this.DateRegistration
            //this.DealVersions ����� �� ������� � ������� �� DealWithStageVersions
            //this.DealWithStageVersions
            //this.State

            // ��� ���������� ������ � ��������� ������� (�� �������, ��� � �� ����� DocumentCategory)
            //this.ContractDocument = frm.ContractDocument;
            //this.ContractDocument.Date = frm.Date;
            //this.ContractDocument.Number = frm.Number;
            //this.ContractDocument.DocumentCategory = frm.DocumentCategory;

            this.Current.Price = frm.Price;
            this.Current.Valuta = frm.Valuta;

            // ������ � ����������� ���������, ������� �������� ������ ������ �������� (���������)
            //this.Current.ContractDocument.Delete();
            //this.Current.ContractDocument = frm.ContractDocument;
            //this.Current.ContractDocument.Date = frm.Date;
            //this.Current.ContractDocument.Number = frm.Number;
            //this.Current.ContractDocument.DocumentCategory = frm.DocumentCategory;
            this.Current.Category = frm.Category;

            if (frm.OurRole == PartyRole.CUSTOMER) {
                this.Current.Customer.Party = frm.OurParty;
                this.Current.Supplier.Party = frm.PartnerParty;
            } else {
                this.Current.Supplier.Party = frm.OurParty;
                this.Current.Customer.Party = frm.PartnerParty;
            }

            crmDealLongServiceVersion dlsv = this.Current as crmDealLongServiceVersion;
            //if (dlsv != null) {
            //    dlsv.StageStructure.FirstStage.DateBegin = frm.DateBegin;
            //    dlsv.StageStructure.FirstStage.DateEnd = frm.DateEnd;
            //    dlsv.StageStructure.FirstStage.Code = "���" + frm.Number;
            //}


            //this.Current.Code
            //Date Begin
            //Date End
            //Party
            //Party
            //Date
            //Document Category
            //Number

            return this;
        }

        #endregion
*/

        #region IVersionMainObject

        public VersionRecord GetCurrent() {
            return (VersionRecord)this.Current;
        }

        #endregion

    }
}