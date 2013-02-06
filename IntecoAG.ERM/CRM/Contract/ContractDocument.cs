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
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

using System.Text;
using DevExpress.ExpressApp;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// ����� ContractDocument, �������������� ������ ��������
    /// </summary>
    [DefaultProperty("FullName")]
    [Persistent("crmContractDocument")]
    public partial class ContractDocument : BaseObject
    {
        public ContractDocument() : base() { }
        public ContractDocument(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ���� ������

        #endregion


        #region �������� ������

        private string _Number;
        //[Appearance("crmContractDocument.Number.Require.Caption", AppearanceItemType = "LayoutItem", BackColor = "Red", FontColor = "Black", FontStyle = System.Drawing.FontStyle.Bold, Criteria = "isnull(Number)")]
        //[Appearance("crmContractDocument.Number.Require.Field", BackColor = "Red", FontColor = "Black", Criteria = "isnull(Number)")]

        //[Appearance("Number.Caption.Italic", AppearanceItemType.LayoutItem, "FontStyle = 'Italic'", FontStyle = FontStyle.Italic)]
        //[Appearance("Number.Caption.Regular", AppearanceItemType.LayoutItem, "FontStyle = 'Regular'", FontStyle = FontStyle.Regular)]
        //[Appearance("Number.Caption.Strikeout", AppearanceItemType.LayoutItem, "FontStyle = 'Strikeout'", FontStyle = FontStyle.Strikeout)]
        //[Appearance("Number.Caption.Underline", AppearanceItemType.LayoutItem, "FontStyle = 'Underline'", FontStyle = FontStyle.Underline)]
        //[Appearance("Number.Caption.BackColor.Red", AppearanceItemType.LayoutItem, "Severity = 'Severe'", BackColor = "Red", FontColor = "Black", Priority = 1)]
        //[Appearance("Number.Caption.Blue", AppearanceItemType.LayoutItem, "Priority = 'Low'", FontColor = "Blue")]
        //[Appearance("Number.Caption.FontClor.Red", AppearanceItemType.LayoutItem, "Priority = 'High'", FontColor = "Red")]
        //[RuleRequiredField("crmContractDocument.Number.Required.Immediate", "Immediate")]
        [RuleRequiredField("crmContractDocument.Number.Required", "Save")]
        [Appearance("Number.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [Size(30)]
        [ImmediatePostData]
        public string Number {
            get { return _Number; }
            set { 
                SetPropertyValue("Number", ref _Number, value);
                if (!IsLoading) {
                    OnChanged("FullName");
                }
            }
        }

        private DateTime _Date;
        [Appearance("Date.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [RuleRequiredField("crmContractDocument.Date.Required", "Save")]
        //[RuleRequiredField("crmContractDocument.Date.Required.Immediate", "Immediate")]
        //[ImmediatePostData]
        public DateTime Date {
            get { return _Date; }
            set { 
                SetPropertyValue<DateTime>("Date", ref _Date, value);
                if (!IsLoading) {
                    OnChanged("FullName");
                }
            }
        }

        private crmContractDocumentCategory _DocumentCategory;
        [Association("crmContractDocumentType-ContractDocument")]
//        [VisibleInListView(false)]
        [Appearance("DocumentCategory.Caption.Bold", AppearanceItemType = "LayoutItem", FontStyle = FontStyle.Bold)]
        [RuleRequiredField("crmContractDocument.DocumentCategory.Required", "Save")]
        //[RuleRequiredField("crmContractDocument.DocumentCategory.Require.Immediate", "Immediate")]
        public crmContractDocumentCategory DocumentCategory {
            get { return _DocumentCategory; }
            set { 
                SetPropertyValue<crmContractDocumentCategory>("DocumentCategory", ref _DocumentCategory, value);
                if (!IsLoading) {
                    OnChanged("FullName");
                }
            }
        }
        [VisibleInListView(false)]
        public string FullName {
            get { return ((DocumentCategory == null) ? "" : DocumentCategory.Code) + " � " + ((Number == null) ? "      " : Number) + " �� " + Date.ToString("d"); }
        }
        #endregion


        #region ������

        #endregion

    }

}