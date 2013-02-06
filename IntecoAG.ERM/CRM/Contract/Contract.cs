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

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    ///  Î‡ÒÒ Contract, ÔÂ‰ÒÚ‡‚Îˇ˛˘ËÈ Ó·˙ÂÍÚ ƒÓ„Ó‚Ó‡
    /// </summary>
    [Persistent("crmContract")]
    public partial class Contract : BaseObject
    {
        public Contract() : base() { }
        public Contract(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.ContractDocument = new ContractDocument(this.Session);
            this.ContractDocuments.Add(this.ContractDocument);
        }


        #region œŒÀﬂ  À¿——¿

        #endregion


        #region —¬Œ…—“¬¿  À¿——¿

        private string _Description;
        [VisibleInListView(false)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }

        private ContractDocument _ContractDocument;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public ContractDocument ContractDocument {
            get { return _ContractDocument; }
            set { SetPropertyValue<ContractDocument>("ContractDocument", ref _ContractDocument, value); }
        }

        private ContractImplementation _ContractImplementation;
        [Browsable(false)]
        public ContractImplementation ContractImplementation {
            get { return _ContractImplementation; }
            set { SetPropertyValue<ContractImplementation>("ContractImplementation", ref _ContractImplementation, value); }
        }

//        public string Number {
//            get { return ContractDocument.Number; }
//        }
//        public DateTime Date {
//            get { return ContractDocument.Date; }
//        }

        #endregion


        #region Ã≈“Œƒ€

        public override string ToString()
        {
            string Res = "";
            Res = Description;
            return Res;
        }

        #endregion

    }

}