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
    ///  Î‡ÒÒ ContractDocument, ÔÂ‰ÒÚ‡‚Îˇ˛˘ËÈ Ó·˙ÂÍÚ ƒÓ„Ó‚Ó‡
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("crmContractCategory")]
    public partial class crmContractCategory : BaseObject, ITreeNode
    {
        public crmContractCategory() : base() { }
        public crmContractCategory(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        [Size(10)]
        private string _Code;
        public string Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        [Size(70)]
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
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


        #region Ã≈“Œƒ€

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