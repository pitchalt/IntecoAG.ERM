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
    /// Класс ContractDocument, представляющий объект Договора
    /// </summary>
    [DefaultProperty("Code")]
    [Persistent("crmContractDocumentCategory")]
    public partial class crmContractDocumentCategory : BaseObject//, ITreeNode
    {
        public crmContractDocumentCategory() : base() { }
        public crmContractDocumentCategory(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

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
        #endregion
        [Association("crmContractDocumentType-ContractDocument", typeof(ContractDocument))]
        public XPCollection<ContractDocument> Contracts {
            get { return GetCollection<ContractDocument>("Contracts"); }
        }
        /*
        


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
         */
    }

}