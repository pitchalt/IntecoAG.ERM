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
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс crmContractDocument, представляющий объект Договора
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("crmContractCategory")]
    public partial class crmContractCategory : BaseObject, ITreeNode
    {
        public crmContractCategory(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        private string _Code;
        [Size(10)]
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        private string _Name;
        [Size(70)]
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }

        [NonPersistent]
        public string FullCode {
            get {
                if (UpCategory != null) {
                    return this.UpCategory.FullCode + "." + this.Code;
                } else {
                    return this.Code;
                }
            }
        }

        private crmContractCategory _UpCategory;
        [Association("crmCategory-Categories")]
        [VisibleInListView(false)]
        public crmContractCategory UpCategory {
            get { return _UpCategory; }
            set { SetPropertyValue<crmContractCategory>("UpCategory", ref _UpCategory, value); }
        }

        [Aggregated]
        [Association("crmCategory-Categories", typeof(crmContractCategory))]
        public XPCollection<crmContractCategory> DownCategorys {
            get { return GetCollection<crmContractCategory>("DownCategorys"); }
        }
        
        #endregion


        #region МЕТОДЫ

        #endregion


        IBindingList ITreeNode.Children {
            get { return DownCategorys; }
        }

        string ITreeNode.Name {
            get { return Name; }
        }

        ITreeNode ITreeNode.Parent {
            get { return UpCategory; }
        }
    }

}