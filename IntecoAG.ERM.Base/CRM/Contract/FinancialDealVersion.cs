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
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Класс для 
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Code")]
    [Persistent("crmFinancialDealVersion")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public partial class crmFinancialDealVersion : VersionRecord //, ITreeNode
    {
        public crmFinancialDealVersion(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }


        #region ПОЛЯ КЛАССА
        #endregion


        #region СВОЙСТВА КЛАССА
        /// <summary>
        /// Дата начала события
        /// </summary>
        private DateTime _DateBegin;
        [RuleRequiredField("crmFinancialDealVersion.DateBegin.Required", "Save")]
        //[RuleRequiredField("crmFinancialDealVersion.DateBegin.Required.Immediate", "Immediate")]
        public DateTime DateBegin {
            get { return _DateBegin; }
            set {
                if (!IsLoading) {
                    // Паша правильно написать условие 
                    if (DateEnd.Year > 1900 && DateEnd < value) {
                        value = DateEnd;
                    }
                }
                SetPropertyValue<DateTime>("DateBegin", ref _DateBegin, value);
            }
        }

        /// <summary>
        /// Дата конца события
        /// </summary>
        private DateTime _DateEnd;
        [RuleRequiredField("crmFinancialDealVersion.DateEnd.Required", "Save")]
        //[RuleRequiredField("crmFinancialDealVersion.DateEnd.Required.Immediate", "Immediate")]
        public DateTime DateEnd {
            get { return _DateEnd; }
            set {
                if (!IsLoading) {
                    if (DateBegin > value) {
                        value = DateBegin;
                    }
                }
                SetPropertyValue<DateTime>("DateEnd", ref _DateEnd, value);
            }
        }

        /// <summary>
        /// Code
        /// </summary>
        private string _Code;
        [Size(10)]
        [RuleRequiredField("crmFinancialDealVersion.Code.Required", "Save")]
        //[RuleRequiredField("crmFinancialDealVersion.Code.Required.Immediate", "Immediate")]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        /// <summary>
        /// Name - описание
        /// </summary>
        private string _Name;
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        /// <summary>
        /// Description - описание
        /// </summary>
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        #endregion
        

        #region МЕТОДЫ

        /*
        #region ITreeNode Members

        IBindingList ITreeNode.Children {
            get { return SubFinancialDeals; }
        }

        string ITreeNode.Name {
            get { return Code; }
        }

        ITreeNode ITreeNode.Parent {
            get { return TopFinancialDeal; }
        }

        #endregion
        */

        #endregion

    }

}