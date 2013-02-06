using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
//
using IntecoAG.ERM.CS;
//
namespace IntecoAG.ERM.CS.Settings {

    [Persistent("csSettings")]
    [NavigationItem("Settings")]
    //[NavigationItem("Settings.SettingsCommon")]
    public abstract class csCSettings : csCCodedComponent {
        public csCSettings(Session ses)
            : base(ses) {
        }

        [RuleUniqueValue("", DefaultContexts.Save)]
        public override string Code {
            get {
                return base.Code;
            }
            set {
                base.Code = value;
            }
        }

    }
}
