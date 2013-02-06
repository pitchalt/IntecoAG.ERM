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
    [Persistent("crmContractDocumentType")]
    public partial class crmContractDocumentType : BaseObject//, ITreeNode
    {
        public crmContractDocumentType(Session ses) : base(ses) { }


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
        [Association("crmContractDocumentType-crmContractDocument", typeof(crmContractDocument))]
        public XPCollection<crmContractDocument> Contracts {
            get { return GetCollection<crmContractDocument>("Contracts"); }
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