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

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Editors;

using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CS;

namespace IntecoAG.ERM.Module {

    //public class FilteringCriterion<classTtype>  : BaseObject {
    //[DefaultClassOptions, ImageName("Action_Filter")]
    //[NavigationItem(true)]
    [Persistent("AdminCriterion")]
    public class AdminCriterion : BaseObject
    {

        public AdminCriterion(Session session) : base(session) { }

       //[Browsable(false)]
        private Type ObjectType { get { return typeof(IntecoAG.ERM.CRM.Contract.Analitic.crmCashFlowRegister); } }


        [CriteriaObjectTypeMember("ObjectType"), Size(-1), ImmediatePostData]
        public string Criterion {
            get { return GetPropertyValue<string>("Criterion"); }
            set {
                SetPropertyValue<string>("Criterion", value);
            }
        }

        //[Browsable(false)]
        public string CriterionString {
            get {
                return Criterion;
            }
        }
    }
}
