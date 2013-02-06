using System;
using System.Collections.Generic;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.CS
{

    public interface ICustomFilter {

        #region ПОЛЯ

        ListView LV { get; set; }
        DetailView DV { get; set; }
        DevExpress.ExpressApp.DC.ITypeInfo objectTypeInfo { get; set; }
        ViewController CriteriaController { get; set; }
        Type ObjectType { get; }
        string CriterionString { get; }
        string AdditionalCriterionString { get; set; }

        #endregion

        #region 

        void ApplyFilter();
        void ClearFilter();

        #endregion
    }

}