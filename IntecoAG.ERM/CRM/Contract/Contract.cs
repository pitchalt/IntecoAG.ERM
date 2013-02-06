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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp;

using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Common;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� crmContract, �������������� ������ ��������
    /// </summary>
    //[DefaultClassOptions]
    [MiniNavigation("This", "�������� ��������", TargetWindow.Current, 1)]
    //[RepresentativeProperty("This")]
    [Persistent("crmContract")]
    public partial class crmContract : BaseObject
    {
        public crmContract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
//            this.ContractDocument = new crmContractDocument(this.Session);
        }


        #region ���� ������

        #endregion


        #region �������� ������

        private string _Description;
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        private crmContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public crmContractDocument ContractDocument {
            get { return _ContractDocument;}
            set { 
                SetPropertyValue<crmContractDocument>("ContractDocument", ref _ContractDocument, value);
                if (!IsLoading) {
                    this.ContractDocuments.Add(this.ContractDocument);
                }
            }
        }

        //protected crmContractCategory _ContractCategory;
        //public crmContractCategory ContractCategory {
        //    get { return _ContractCategory; }
        //    set { SetPropertyValue<crmContractCategory>("ContractCategory", ref _ContractCategory, value); }
        //}

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
        // �����
        private String _Delo;
        [Size(10)]
        public String Delo {
            get { return _Delo; }
            set { SetPropertyValue<String>("Delo", ref _Delo, value); }
        }


        #endregion


        #region ������

        #endregion

    }

}