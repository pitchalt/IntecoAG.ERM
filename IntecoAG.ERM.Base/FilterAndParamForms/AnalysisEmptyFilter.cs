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

namespace IntecoAG.ERM.Module {
    [Persistent("AnalysisEmptyFilter")]
    public class AnalysisEmptyFilter : ReportParametersObjectBase, ICustomFilter
    {
        public AnalysisEmptyFilter(Session session) : base(session) { }

        public override CriteriaOperator GetCriteria() {
            return null;
        }

        public override SortingCollection GetSorting() {
            return null;
        }

        #region Для фильтра в списках ListView

        private ListView _LV;
        [Browsable(false)]
        public ListView LV {
            get { return _LV; }
            set { SetPropertyValue<ListView>("LV", ref _LV, value); }
        }

        private DetailView _DV;
        [Browsable(false)]
        public DetailView DV {
            get { return _DV; }
            set { SetPropertyValue<DetailView>("DV", ref _DV, value); }
        }

        //private PivotGrid _PG;
        //[Browsable(false)]
        //public ListView PG {
        //    get { return _PG; }
        //    set { SetPropertyValue<ListView>("PG", ref _PG, value); }
        //}

        private DevExpress.ExpressApp.DC.ITypeInfo _objectTypeInfo;
        [Browsable(false)]
        public DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo {
            get { return _objectTypeInfo; }
            set { SetPropertyValue<DevExpress.ExpressApp.DC.ITypeInfo>("objectTypeInfo", ref _objectTypeInfo, value); }
        }

        private ViewController _criteriaController;
        [Browsable(false)]
        public ViewController CriteriaController {
            get { return _criteriaController; }
            set { SetPropertyValue<ViewController>("CriteriaController", ref _criteriaController, value); }
        }

        [Browsable(false)]
        public Type ObjectType { get { return objectTypeInfo.Type; } }

        [Browsable(false)]
        public string CriterionString {
            get { return SearchCriteriaBuilder(); } // Criterion; }
        }

        private string _AdditionalCriterionString;
        [Browsable(false)]
        public string AdditionalCriterionString {
            get { return _AdditionalCriterionString; }
            set { SetPropertyValue<string>("AdditionalCriterionString", ref _AdditionalCriterionString, value); }
        }

        #endregion

        #region ПАРАМЕТРЫ НА ФОРМЕ

        #endregion


        #region МЕТОДЫ

        private CriteriaOperator SearchCriteriaObjectBuilderOLD() {
            return null;
        }

        private CriteriaOperator SearchCriteriaObjectBuilder() {
            return null;
        }

        private string SearchCriteriaBuilder() {
            CriteriaOperator searchCriteriaObjectBuilder = SearchCriteriaObjectBuilder();
            return (object.ReferenceEquals(searchCriteriaObjectBuilder, null)) ? "" : searchCriteriaObjectBuilder.LegacyToString();
        }


        //[Action(ToolTip = "Apply Filter")]
        public void ApplyFilter() {
        }

        public void ClearFilter() {
        }

        #endregion


    }

}
