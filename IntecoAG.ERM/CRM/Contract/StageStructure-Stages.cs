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
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.CRM.Contract
{
    /// <summary>
    /// Мастер-класс
    /// </summary>
    public partial class crmStageStructure
    {
        [Association("crmStageStructure-crmStage")]
        [Browsable(false)]
        public XPCollection<crmStage> Stages {
            get {
                return GetCollection<crmStage>("Stages");
            }
        }
    }

    /// <summary>
    /// Detail class
    /// </summary>
    public partial class crmStage
    {
        private crmStageStructure _StageStructure;
        [Association("crmStageStructure-crmStage")]
        [Browsable(false)]
        public crmStageStructure StageStructure {
            get { return _StageStructure; }
            set { SetPropertyValue<crmStageStructure>("StageStructure", ref _StageStructure, value); }
        }
    }

}
