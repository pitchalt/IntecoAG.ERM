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
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;

namespace IntecoAG.ERM.CRM.Contract.Obligation
{
    /// <summary>
    /// Класс ObligationTransfer, представляющий трансферы обязательствам Договора
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract partial class crmObligationTransfer : crmObligation   //, IVersionSupport
    {
        public crmObligationTransfer(Session session) : base(session) { }
        public crmObligationTransfer(Session session, VersionStates state) : base(session, state) { }
        
        public override void VersionAfterConstruction() {
            base.VersionAfterConstruction();
        }

        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Номентклатура
        /// </summary>
//        private CS.csNomenclature.csNomenclature _Nomenclature;
        [NonPersistent]
        public abstract csNomenclature Nomenclature {
            get;
        }

        #endregion

    }

}