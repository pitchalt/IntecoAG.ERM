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
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using System.Diagnostics;

using DevExpress.Persistent.Validation;
using DevExpress.XtraEditors.Repository;

using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.Xpo.Metadata;

namespace IntecoAG.ERM.CRM.Contract
{

    // �� ������ �������� �� ���� ������������: 
    // http://documentation.devexpress.com/#Xaf/CustomDocument3009
    // http://documentation.devexpress.com/#Xaf/CustomDocument3008
    // http://documentation.devexpress.com/#Xaf/CustomDocument3251
    // http://documentation.devexpress.com/#Xaf/CustomDocument3051
    // http://documentation.devexpress.com/#Xaf/CustomDocument3217 - �������� ���������


    // ����� ���������� ImmediateValidationController ��� ������� ����� ������:
    // ��� ������� ���, ����� ������-�������, ����������� ������ �� ����� �� ������������ �������� ����� �� �� ���������
    // ���� � ������ �� ������: http://www.devexpress.com/Support/Center/e/E1524.aspx
    // ����� ��. http://www.devexpress.com/Support/Center/e/E458.aspx - ���������� ValidateDetailViewController, ���������� 
    // ��� ������������ ����� �������� ����� � ListView � ���������� ������������ ������ - �������� � ���������� ��� AAA.
    // � ���������, ���������� ValidateDetailViewController (� ������� �� ImmediateValidationController) ����������: ���� ������
    // ����� ������, �� � ��� ���������� ����, � DetailView ������� ��� ���������� � ���������. ������� ���������� 
    // ValidateDetailViewController �������� �� �������.

    /// <summary>
    /// ����� AAA, ������� �������� ������ �������
    /// </summary>
    //[DefaultClassOptions]

    #region ������-�������
    // http://documentation.devexpress.com/#Xaf/CustomDocument3203
    // ���������� ���������: http://documentation.devexpress.com/#XPO/CustomDocument4928, http://documentation.devexpress.com/#XPO/CustomDocument2047
    //[EditorStateRuleAttribute("DoHideMemoProperty", "Memo; Description", EditorState.Hidden, "DoHide", ViewType.Any)]
    //[EditorStateRuleAttribute("DoDisableMemoProperty", "Memo; Description", EditorState.Disabled, "DoDisable", ViewType.Any)]
    //[EditorStateRuleAttribute("DoDisableCodeProperty", "Code", EditorState.Disabled, "Len(Name) > 5", ViewType.Any)]

    //[Appearance("RemoveSaveButton", AppearanceItemType = "Action", TargetItems = "Save; SaveAndClose",
    //Enabled = false, Criteria = "DoHide", Context = "Any")]

    // http://documentation.devexpress.com/#Xaf/DevExpressExpressAppConditionalAppearanceAppearanceAttributeMembersTopicAll
    // http://documentation.devexpress.com/#Xaf/CustomDocument3286
    // ����������� ������ Delete ����� ��� ������ ������ �������, � ������� ���� DoHide = true
    //[Appearance("DeleteHidden", AppearanceItemType = "Action", Criteria = "DoHide = 'True'", TargetItems = "Delete", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]

    // ����������� ������ Delete ��� ������� ������ �����
    [Appearance("DeleteHidden", AppearanceItemType = "Action", TargetItems = "Delete", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    //[Appearance("RemoveDeleteButton1", AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Enabled = false)]

    // ����������� ������ Delete ����� ��� ������ ������ �������, � ������� ����� HideDeleteButton ����� �������� true
    //[Appearance("DeleteHidden", AppearanceItemType = "Action", TargetItems = "Delete", Method = "HideDeleteButton", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "DetailView")]

    #endregion

    public class AAA : BaseObject  //, IDXDataErrorInfo ������� � �� ������������ ���� ���������, ������ �� �����, � ��� ����� ��� repository, �� � ��� ����� �� ���������
    {
        public AAA(Session ses) : base(ses) { }


        #region ���� ������

        #endregion


        #region �������� ������

        /// <summary>
        /// Name - ��������
        /// </summary>
        private string _Name;
        [ImmediatePostData]
        [Index(0)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        /// <summary>
        /// Code - ��������
        /// </summary>
        private string _Code;
        [RuleValueComparison("CheckCode", DefaultContexts.Save,
           ValueComparisonType.LessThan, 10, SkipNullOrEmptyValues = true,
           CustomMessageTemplate = "The Amount must not be less than {RightOperand}.")]
        [Index(1)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        /// <summary>
        /// Description - ��������
        /// </summary>
        private string _Description;
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        /// <summary>
        /// DoDisable - ��������
        /// </summary>
        private bool _DoDisable;
        [ImmediatePostData]
        public bool DoDisable {
            get { return _DoDisable; }
            set { SetPropertyValue("DoDisable", ref _DoDisable, value); }
        }

        /// <summary>
        /// DoHide - ��������
        /// </summary>
        private bool _DoHide;
        [ImmediatePostData]
        public bool DoHide {
            get { return _DoHide; }
            set { SetPropertyValue("DoHide", ref _DoHide, value); }
        }

        /// <summary>
        /// Memo - ��������
        /// </summary>
        private string _Memo;
        public string Memo {
            get { return _Memo; }
            set { SetPropertyValue("Memo", ref _Memo, value); }
        }


        /// <summary>
        /// Count - ��������
        /// </summary>
        private string _Count;
        [RuleRequiredField("RuleRequiredField for Count", DefaultContexts.Save, "A Count must be specified")]
        public string Count {
            get { return _Count; }
            set { SetPropertyValue("Count", ref _Count, value); }
        }

        private double _Amount;
        [RuleValueComparison("RuleRequiredField for Account .Amount", DefaultContexts.Save, 
           ValueComparisonType.LessThan, 100, SkipNullOrEmptyValues = false,
           CustomMessageTemplate = "The Amount must not be less than {RightOperand}.")]
        public double Amount {
            get { return _Amount; }
            set { SetPropertyValue("Amount", ref _Amount, value); }
        }


        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set {
                SetPropertyValue("Date", ref _Date, value);
            }
        }



        // ���������, ����� Code ��� ������ Name
        [RuleFromBoolProperty("CodeLengthLessNameLength", DefaultContexts.Save, "Code Length must be Less Name Length")]
        [NonPersistent]
        private bool CodeLengthLessNameLength {
            get { return (string.IsNullOrEmpty(Name) | string.IsNullOrEmpty(Code)) || Convert.ToString(Name).Length > Convert.ToString(Code).Length; }
        }

        
        private BBB _BBB;
        [RuleRequiredField("BBB_Required", DefaultContexts.Save, CustomMessageTemplate = "Cannot be blank")]
        public BBB BBB {
            get { return _BBB; }
            set {
                SetPropertyValue("BBB", ref _BBB, value);
            }
        }

        #endregion


        #region ������ ������

        // Prevent from being deleted - ������ ���������� ������� ����������� �������, ������� �� �� ���������. ��
        // ��� � �������� � ������� ������ �������� - ��� ����� ���� ����������� ������� [Appearance...]
        protected override void OnDeleting() {
            throw new Exception("Cannot be deleted");
        }

        // ���������� ����� ������ ��. � �������� [Appearance...] - ������ ������������� (�� ��������)
        private bool HideDeleteButton() {
            return true;
        }
        #endregion

    }




    // ��������������� ����� ��� ������������ ������ ��������� � ������ AAA (�������� � �� BBB �� ������ �������� �������������)
    public class BBB : BaseObject {

        public BBB(Session session)
            : base(session) { }

        private string _Name;
        public string Name {
            get { return _Name; }
            set {
                SetPropertyValue("Name", ref _Name, value);
            }
        }


        /// <summary>
        /// BusinesData - ������
        /// </summary>
        private string _BusinesData;
        [ImmediatePostData]
        public string BusinesData {
            get { return _BusinesData; }
            set { SetPropertyValue("BusinesData", ref _BusinesData, value); }
        }

    }


}