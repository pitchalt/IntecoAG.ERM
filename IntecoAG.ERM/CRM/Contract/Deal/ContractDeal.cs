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
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Forms;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CRM.Contract.Analitic;

using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Contract.Deal
{
    /// <summary>
    /// ������� ������
    /// </summary>
    public enum DealStates {
        DEAL_PROJECT = 1,   // ������
        DEAL_FORMATION = 2,  // ����������
        DEAL_RESOLVED = 3,  // ��������������
        DEAL_CONCLUDED = 4,  // ��������
        DEAL_CLOSED = 5  // ������
    }

    public enum KindOfDeal {
        DEAL_WITH_STAGE = 1,
        DEAL_WITHOUT_STAGE = 2
//        DEAL_LONG_SERVICE = 3
    }

    public enum ContractKind {
        CONTRACT = 1,
        ADDENDUM = 2
    }
    /// <summary>
    /// ����� crmContractDeal, �������������� ������ ��������
    /// </summary>
    [LikeSearchPathList(new string[] { 
        "Current.ContractDocument.Number", 
        "Current.Customer.Party.Name", 
        "Current.Customer.Party.INN", 
        "Current.Supplier.Party.Name",
        "Current.Supplier.Party.INN"
    })]
    [MiniNavigation("Project", "�������� ���������", TargetWindow.Default, 1)]
    [MiniNavigation("Current", "�������� �������", TargetWindow.Default, 2)]
    [MiniNavigation("Contract", "�������", TargetWindow.Default, 3)]
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [VisibleInReports]
    [Persistent("crmDeal")]
    public partial class crmContractDeal : BaseObject
        //, ICategorizedItem
    {
        public crmContractDeal(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.State = DealStates.DEAL_PROJECT;
        }


        #region ���� ������

        #endregion


        #region �������� ������

        //protected crmContract _Contract;
        //[ExpandObjectMembers(ExpandObjectMembers.Always)]
        //[Aggregated]
        //public crmContract crmContract {
        //    get { return _Contract; }
        //    set { SetPropertyValue<crmContract>("Contract", ref _Contract, value); }
        //}
        // ��� ����������� ���������: �������/�������������� ����������

        [Browsable(false)]
        public crmDealVersion Project {
            get { 
                foreach (crmDealVersion dv in this.DealVersions) {
                    if (dv.VersionState == VersionStates.VERSION_NEW ||
                        dv.VersionState == VersionStates.VERSION_PROJECT)
                        return dv;
                }
                return this.Current;
            }
        }
        protected ContractKind _ContractKind;
        public ContractKind ContractKind {
            get { return _ContractKind; }
            set { SetPropertyValue<ContractKind>("ContractKind", ref _ContractKind, value); }
        }

        public String Name {
            get { 
                String ret;
                crmContractDocument cont_doc;
                if (this.Contract != null)
                    cont_doc = this.Contract.ContractDocument;
                else 
                    cont_doc = null;
                if (cont_doc != null) {
                    if (this.ContractDocument != null && cont_doc != this.ContractDocument)
                        ret = cont_doc.FullName + " " + this.ContractDocument.FullName;
                    else 
                        ret = cont_doc.FullName;
                } else {
                    if (this.ContractDocument != null )
                        ret = this.ContractDocument.FullName;
                    else
                        ret = "";
                }
                //if (this.ContractDocument != null) {
                //    if (this.Contract.ContractDocument == this.ContractDocument) {
                //        ret = this.ContractDocument.FullName;
                //    }
                //    else {
                //        ret = this.Contract.ContractDocument.FullName + this.ContractDocument.FullName;
                //    }
                //}
                //if (this.ContractDocument != null) { 
                //    ret = 
                //}
                //}
                return ret;
            }
        }

        [PersistentAlias("Current.Customer.Party")]
        public crmPartyRu Customer {
            get {
                if (Current != null)
                    if (Current.Customer != null)
                        return Current.Customer.Party;
                return null;
            }
            set { Current.Customer.Party = value;  }
        }

        [PersistentAlias("Current.Supplier.Party")]
        public crmPartyRu Supplier {
            get { 
                if (Current != null)
                    if (Current.Supplier != null)
                        return Current.Supplier.Party; 
                return null;
            }
            set { Current.Supplier.Party = value; }
        }

        /// <summary>
        /// Curator
        /// </summary>
        private hrmDepartment _CuratorDepartment;
        public hrmDepartment CuratorDepartment {
            get { return _CuratorDepartment; }
            set { SetPropertyValue<hrmDepartment>("CuratorDepartment", ref _CuratorDepartment, value); }
        }
        //
        private DateTime _DateRegistration;
        public DateTime DateRegistration {
            get { return _DateRegistration; }
            set { SetPropertyValue<DateTime>("DateRegistration", ref _DateRegistration, value); }
        }
        // �������������� ������������: ������������, �������������� �����������
        private hrmStaff _UserRegistrator;
        public hrmStaff UserRegistrator {
            get { return _UserRegistrator; }
            set { SetPropertyValue<hrmStaff>("UserRegistrator", ref _UserRegistrator, value); }
        }

        // �������������� �������������: �������������, �������������� ����������� ��������. ������������ ������������� �� ��������������� ������������
        protected hrmDepartment _DepartmentRegistrator;
        public hrmDepartment DepartmentRegistrator {
            get { return _DepartmentRegistrator; }
            set {
                SetPropertyValue<hrmDepartment>("DepartmentRegistrator", ref _DepartmentRegistrator, value);
            }
        }

        private DealStates _State;
        public DealStates State {
            get { return _State; }
            set { SetPropertyValue<DealStates>("State", ref _State, value); }
        }

        //
        protected crmContractCategory _Category;
        //[RuleRequiredField("crmDealRegistrationForm.Category.Required", "Next")]
        //[Appearance("crmDealRegistrationForm.Category.Caption.Bold", AppearanceItemType = "LayoutItem", FontColor = "Red", FontStyle = FontStyle.Bold)]
        public crmContractCategory Category {
            get { return _Category; }
            set { SetPropertyValue<crmContractCategory>("Category", ref _Category, value); }
        }

        private crmContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [DataSourceProperty("ContractDocuments")]
        public crmContractDocument ContractDocument {
            get { return _ContractDocument; }
            set { 
                SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value); 
            }
        }

        public XPCollection<crmContractDocument> ContractDocuments {
//        public IList<crmContractDocument> ContractDocuments {
            get {
                if (this.Contract != null)
                    return Contract.ContractDocuments;
                else
                    return null;
//                        BindingList<crmContractDocument>();
            }
        }

        private crmDealVersion _Current;
        //[Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public crmDealVersion Current {
            get { return _Current; }
            set { SetPropertyValue<crmDealVersion>("Current", ref _Current, value); }
        }

        #endregion


        #region ������

        #endregion

        /*
        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (crmContractCategory) value;
            }
        }
        */


        #region ������ � ����������

        protected virtual void RegisterClear(crmDealVersion scVersion) {
            if (scVersion == null) return;

            // ���������� ((crmDealWithoutStage)scVersion.MainObject).Oid ��� token (����� ��� �������������� ������ ��������� �������)
            Guid token = ((crmContractDeal)scVersion.MainObject).Oid;

            // ������ �������� 
            //FindAndDeletePFRegisterRecords(scVersion);
            //FindAndDeleteDCDRegisterRecords(scVersion);  // ������� ����� ���� ����������� ���������
            FindAndDeletePFRegisterRecords(token);
            FindAndDeleteDCDRegisterRecords(token);
            FindAndDeleteCFRegisterRecords(token);
        }

        protected virtual void FindAndDeletePFRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmPlaneFactRegister> RegColl = new XPCollection<crmPlaneFactRegister>(this.Session, criteria, null);
            if (!RegColl.IsLoaded) RegColl.Load();
            RegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (RegColl.Count > 0) RegColl.Remove(RegColl[0]);
        }

        protected virtual void FindAndDeleteDCDRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmDebtorCreditorDebtRegister> dcdRegColl = new XPCollection<crmDebtorCreditorDebtRegister>(this.Session, criteria, null);
            if (!dcdRegColl.IsLoaded) dcdRegColl.Load();
            dcdRegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (dcdRegColl.Count > 0) dcdRegColl.Remove(dcdRegColl[0]);
        }

        protected virtual void FindAndDeleteCFRegisterRecords(Guid token) {  //(crmDealWithoutStageVersion scVersion) {
            CriteriaOperator criteria = new BinaryOperator("Token", token, BinaryOperatorType.Equal);

            XPCollection<crmCashFlowRegister> cfRegColl = new XPCollection<crmCashFlowRegister>(this.Session, criteria, null);
            if (!cfRegColl.IsLoaded) cfRegColl.Load();
            cfRegColl.DeleteObjectOnRemove = true;

            // �������� �������
            while (cfRegColl.Count > 0) cfRegColl.Remove(cfRegColl[0]);
        }

        #endregion

    }

}