// Developer Express Code Central Example:
// How to use Criteria Property Editors
// 
// This example illustrates the specifics of using Criteria Property Editors in an
// XAF application. The complete description is available in the How to: Use
// Criteria Property Editors (ms-help://DevExpress.Xaf/CustomDocument3143.htm) help
// topic.
// 
// See Also:
// http://www.devexpress.com/scid=Q219209
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E932

using System;
using System.ComponentModel;
//using System.Configuration;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {

    //public class FilteringCriterion<classTtype>  : BaseObject {
    //[DefaultClassOptions, ImageName("Action_Filter")]
    //[NavigationItem(true)]
    [NonPersistent]
    public class FilteringLikeCriterion : BaseObject, ICustomFilter
    {

        public FilteringLikeCriterion(Session session) : base(session) { }

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

        private DevExpress.ExpressApp.DC.ITypeInfo _objectTypeInfo;
        [Browsable(false)]
        public DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo {
            get { return _objectTypeInfo; }
            set { SetPropertyValue<DevExpress.ExpressApp.DC.ITypeInfo>("objectTypeInfo", ref _objectTypeInfo, value); }
        }

        //private Type _objectType;
        //[Browsable(false)]
        //public Type objectType {
        //    get { return _objectType; }
        //    set { SetPropertyValue<Type>("objectType", ref _objectType, value); }
        //}

        /*
        // Вариант для CriteriaController
        private CriteriaController _criteriaController;
        [Browsable(false)]
        public CriteriaController CriteriaController {
            get { return _criteriaController; }
            set { SetPropertyValue<CriteriaController>("CriteriaController", ref _criteriaController, value); }
        }
        */

        private ViewController _criteriaController;
        [Browsable(false)]
        public ViewController CriteriaController {
            get { return _criteriaController; }
            set { SetPropertyValue<ViewController>("CriteriaController", ref _criteriaController, value); }
        }


        //public string Description {
        //    get { return GetDelayedPropertyValue<string>("Description"); }
        //    set { SetDelayedPropertyValue<string>("Description", value); }
        //}

        //private Type ObjectType { get { return typeof(objectType); } }
        //private Type ObjectType { get { return objectType; } }
        [Browsable(false)]
        public Type ObjectType { get { return objectTypeInfo.Type; } }

        // Вместо этого просто строка для ввода искомого значения
        /*
        [CriteriaObjectTypeMember("ObjectType"), Size(-1), ImmediatePostData]
        public string Criterion {
            get { return GetPropertyValue<string>("Criterion"); }
            set { SetPropertyValue<string>("Criterion", value); }
        }
        */


        private string _AdditionalCriterionString;
        [Browsable(false)]
        public string AdditionalCriterionString {
            get { return _AdditionalCriterionString; }
            set { SetPropertyValue<string>("AdditionalCriterionString", ref _AdditionalCriterionString, value); }
        }


        private string _SearchString;
        [ImmediatePostData]
        public string SearchString {
            get { return _SearchString; }
            set { 
                SetPropertyValue<string>("SearchString", ref _SearchString, value);
                /*
                Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

                LV.CollectionSource.Criteria.Clear();
                LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] =
                    CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

                if ((CriteriaController as ListViewFilterPanelController) != null) (CriteriaController as ListViewFilterPanelController).criteriaLikeSearchString = SearchString;
                */
            }
        }


        [Browsable(false)]
        public string CriterionString {
            get {
                if (!string.IsNullOrEmpty(AdditionalCriterionString)) {
                    return "(" + SearchCriteriaBuilder() + ") AND (" + AdditionalCriterionString + ")";
                }
                return SearchCriteriaBuilder();
            }
        }

        private string SearchCriteriaBuilder() {
            string res = "";

            if (string.IsNullOrEmpty(SearchString)) return res;

            // Анализ атрибута LikeSearchPathListAttribute
            LikeSearchPathListAttribute attr = objectTypeInfo.FindAttribute<LikeSearchPathListAttribute>();
            if (attr == null) return res;

            for (int i = 0; i < attr.Values.Length; i++) {
                string elem = attr.Values[i];
                if (string.IsNullOrEmpty(elem)) continue;
                // проверить бы правильность пути, записанного в elem!
                res += ((res == "") ? "" : " OR ") + elem + " like '%" + SearchString + "%'";
            }

            //// SHU 2011-12-08
            //// Добавляем поиск для ILike (только для postgreSql)
            //if (ConfigurationManager.AppSettings["UseILike"] == "True") {
            //    for (int i = 0; i < attr.Values.Length; i++) {
            //        string elem = attr.Values[i];
            //        if (string.IsNullOrEmpty(elem)) continue;
            //        // проверить бы правильность пути, записанного в elem!
            //        res += ((res == "") ? "" : " OR ") +  string.Format("IList({0}, {1})", elem, "'%" + SearchString + "%'");
            //    }
            //}

            return res;
        }

        //protected override void AfterChangeByXPPropertyDescriptor() {
        //    base.AfterChangeByXPPropertyDescriptor();
        //}

        //protected override void  TriggerObjectChanged(ObjectChangeEventArgs args) {

        //    Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

        //    LV.CollectionSource.Criteria.Clear();
        //    LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] = 
        //        CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

        //    if (CriteriaController != null) CriteriaController.criteriaLikeSearchString = SearchString;

        //    base.TriggerObjectChanged(args);
        //}





        //[Action(ToolTip = "Apply Filter")]
        public void ApplyFilter() {
            Type typeObjectOfListView = ((System.Type)(LV.CollectionSource.ObjectTypeInfo.Type)).UnderlyingSystemType;

            LV.CollectionSource.Criteria.Clear();
            LV.CollectionSource.Criteria["@" + Guid.NewGuid().ToString()] =
                CriteriaEditorHelper.GetCriteriaOperator(CriterionString, typeObjectOfListView, LV.ObjectSpace);

            if ((CriteriaController as ListViewFilterPanelController) != null) (CriteriaController as ListViewFilterPanelController).criteriaLikeSearchString = SearchString;
        }

        public void ClearFilter() {
            this.SearchString = "";
            if (LV != null) LV.CollectionSource.Criteria.Clear();
        }

    }
}
