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
using DevExpress.Xpo;

namespace IntecoAG.ERM.CRM.Contract
{

    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class ComplexContract
    {
        [Association("crmComplexContract-ComplexContractVersions"), Aggregated]
        public XPCollection<ComplexContractVersion> ComplexContractVersions {
            get {
                return GetCollection<ComplexContractVersion>("ComplexContractVersions");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class ComplexContractVersion
    {
        private ComplexContract _ComplexContract;
        [Association("crmComplexContract-ComplexContractVersions")]
        public ComplexContract ComplexContract {
            get { return _ComplexContract; }
            set {
                if (!IsLoading) {
                    if (value != null) {
                        this.Category = value.Category;
                        this.Number = value.Contract.ContractDocument.Number;
                        this.Date = value.Contract.ContractDocument.Date;
                        this.DocumentCategory = value.Contract.ContractDocument.DocumentCategory;
                    }
                }
                SetPropertyValue<ComplexContract>("ComplexContract", ref _ComplexContract, value);
            }
        }
    }

}
