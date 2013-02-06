using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.HRM.Organization;

namespace IntecoAG.ERM.CRM.Party {

    public class crmCPartyUserPartyCurrent : ICustomFunctionOperator {
        
        const String Name = "ERMCurrentUserParty";

        static crmCPartyUserPartyCurrent() {
            if (CriteriaOperator.GetCustomFunction(Name) == null)
                CriteriaOperator.RegisterCustomFunction(new crmCPartyUserPartyCurrent());
        }

        String ICustomFunctionOperator.Name {
            get { return Name; }
        }

        public object Evaluate(params object[] operands) {
            if (crmUserParty.CurrentUserParty != null) {
                return crmUserParty.CurrentUserParty.Value;
            }
            return null;
        }

        public Type ResultType(params Type[] operands) {
            return typeof(crmUserParty);
        }

        public static void Register() { }
    }

}