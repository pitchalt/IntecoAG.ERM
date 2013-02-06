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
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.FM.StatementAccount
{
    /// <summary>
    /// Класс LegalPerson, представляющий юридическое лицо как участника (сторону) Договора
    /// </summary>
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public partial class fmBank : crmBank
    {
        public fmBank(Session ses) : base(ses) { }


        #region ПОЛЯ КЛАССА

        #endregion


        #region СВОЙСТВА КЛАССА

        /// <summary>
        /// Importers - список модулей импорта данных в систему для даного банка из разных источников (Платёжки и т.п.)
        /// </summary>
        //[Aggregated]
        [Association("fmBank-fmImporters", typeof(fmImporter))]
        public XPCollection<fmImporter> Importers {
            get { return GetCollection<fmImporter>("Importers"); }
        }

        #endregion


        #region МЕТОДЫ

        #endregion

    }

}