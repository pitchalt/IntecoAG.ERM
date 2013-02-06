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
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� CostModel, �������������� ������ ��������
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("crmCostModel")]
    public partial class crmCostModel : BaseObject
    {
        public crmCostModel() : base() { }
        public crmCostModel(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Code
        /// </summary>
        [Size(10)]
        private string _Code;
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }
        /// <summary>
        /// Name - ��������
        /// </summary>
        [Size(70)]
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }
        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        #endregion


        #region ������


        #endregion

    }

}