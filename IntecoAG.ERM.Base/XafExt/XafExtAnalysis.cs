using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CS;
//using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CRM.Contract.Analitic;
using IntecoAG.ERM.CRM.Contract.Obligation;

namespace IntecoAG.ERM.XAFExt {
    //[NonPersistent]
    //[Appearance("XafExtAnalysis.FilterSocket.Visible", AppearanceItemType = "LayoutItem", TargetItems = "FilterSocket", Criteria = "not ApplyTopAppearance", Visibility = ViewItemVisibility.Hide, Enabled = false)]
    //[Appearance("BindAnalysisDataHidden", AppearanceItemType = "Action", TargetItems = "BindAnalysisData", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    //[Appearance("UnbindAnalysisDataHidden", AppearanceItemType = "Action", TargetItems = "UnbindAnalysisData", Visibility = ViewItemVisibility.ShowEmptySpace, Context = "Any")]
    [Persistent("XafExtAnalysis")]
    public class XafExtAnalysis : Analysis
    {
        public XafExtAnalysis(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        #region ПАРАМЕТРЫ НА ФОРМЕ

        private string adminCriteria;

        [CriteriaOptions("DataType")]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        [VisibleInListView(false)]
        [Custom("RowCount", "0")]
        public string AdminCriteria {
            get { return adminCriteria; }
            set {
                SetPropertyValue<string>("AdminCriteria", ref adminCriteria, value);
            }
        }

        #endregion

    }

}
