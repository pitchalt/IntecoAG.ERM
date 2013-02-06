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
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;

using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Класс ContractImplementation, представляющий объект Договора
    /// </summary>
    [Persistent("crmContractImplementation")]
    public abstract partial class ContractImplementation : BaseObject, ICategorizedItem
    {
        public ContractImplementation(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.Contract = new Contract(this.Session);
        }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        protected Contract _Contract;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Aggregated]
        public Contract Contract {
            get { return _Contract; }
            set { SetPropertyValue<Contract>("Contract", ref _Contract, value); }
        }

        protected crmContractCategory _Category;
        public crmContractCategory Category {
            get { return _Category; }
            set { SetPropertyValue<crmContractCategory>("Category", ref _Category, value); }
        }

        private DateTime _DateOpen;
        public DateTime DateOpen {
            get { return _DateOpen; }
            set { SetPropertyValue<DateTime>("DateOpen", ref _DateOpen, value); }
        }

        private DateTime? _DateClose;
        public DateTime? DateClose {
            get { return _DateClose; }
            set {
                SetPropertyValue<DateTime?>("DateClose", ref _DateClose, value);
                if (!IsLoading) {
                    if (value != null)
                        IsClosed = true;
                    else
                        IsClosed = false;
                }
            }
        }

        private bool _IsClosed;
        public bool IsClosed {
            get { return _IsClosed; }
            set {
                if (!IsLoading) {
                    if (!_IsClosed)
                        if (DateClose == null)
                            value = false;
                    else
                        DateClose = null;
                }
                SetPropertyValue<bool>("IsClosed", ref _IsClosed, value); 
            }
        }
        private DateTime _DateSign;
        [VisibleInListView(false)]
        public DateTime DateSign {
            get { return _DateSign; }
            set { SetPropertyValue<DateTime>("DateSign", ref _DateSign, value); }
        }


        //private ContractVersion _CurrentVersion;
        [VisibleInListView(false)]
        [NonPersistent]
        public virtual ContractVersion CurrentVersion {
            get {
                // Временно разбор по типам, пока не сделан общий способ
                SimpleContract sc = this as SimpleContract;
                if (sc != null) return sc.Current;

                ComplexContract cc = this as ComplexContract;
                if (cc != null) return cc.Current;

                return null;
            }
        }

        #endregion


        #region МЕТОДЫ

        ///// <summary>
        ///// Стандартный метод (XAF его использует, чтобы показать объект в интерфейсе)
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    string Res = "";
        //    Res = Description;
        //    return Res;
        //}

        #endregion

        
        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (crmContractCategory) value;
            }
        }
    }

}