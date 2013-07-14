using System;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.FM.FinAccount;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.HRM.Organization;
//
namespace IntecoAG.ERM.FM.Docs.Cache {

    public enum fmIDocCachePartyType { 
        PARTY_PARTY = 1,
        PARTY_STAFF = 2
    }

    public enum fmIDocCacheJournalType { 
        JOURNAL_MAIN = 1,
        JOURNAL_PREPARE = 2
    }

    [DomainComponent]
    public interface fmIDocCache : fmIDocument {
        [RuleRequiredField()]
        fmIDocCacheJournalType JournalType { get; set; }
        fmIDocCachePartyType PayPartyType { get; set; }

        [FieldSize(120)]
        String PayPartyName { get; set; }
        [NonPersistentDc]
        [RuleRequiredField(TargetCriteria = "PayPartyType == 'PARTY_PARTY'")]
        [Appearance("", Criteria = "PayPartyType == 'PARTY_STAFF'", Enabled = false)]
        crmIParty PayPartyParty { get; set; }
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        crmCParty PayPartyPartyC { get; set; }
        [RuleRequiredField(TargetCriteria = "PayPartyType == 'PARTY_STAFF'")]
        [Appearance("", Criteria = "PayPartyType == 'PARTY_PARTY'", Enabled = false)]
        hrmStaff PayPartyStaff { get; set; }
        [FieldSize(60)]
        String AnaliticCode { get; }

        [DataSourceCriteria("AccountSystem.Code == '1000' && IsSelectabled")]
        fmCFAAccount AccountDebit { get; set; }
        [DataSourceCriteria("AccountSystem.Code == '1000' && IsSelectabled")]
        fmCFAAccount AccountCredit { get; set; }

        Decimal Summa { get; set; }
        Int64 SummaSenior { get; }
        [FieldSize(120)]
        String SummaSeniorString { get; }
        [FieldSize(2)]
        String SummaJuniorString { get; }
        String SummaString { get; }
        [FieldSize(120)]
        String DescriptionBase { get; set; }
        [FieldSize(60)]
        String DescriptionContent { get; set; }
        [FieldSize(60)]
        String DescriptionAppendix { get; set; }
        [FieldSize(1)]
        String AVTMode { get; set; }

        [Aggregated]
        [NonPersistentDc]
        IList<fmIDocCacheLine> Lines { get; }

        [DataSourceProperty("GrossAccountStaffList")]
        hrmStaff GrossAccountStaff { get; set; }
        [Browsable(false)]
        [NonPersistentDc]
        IList<hrmStaff> GrossAccountStaffList { get; }
        hrmStaff CashierStaff { get; set; }

        void UpdatePartyName();
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(fmIDocCache));
    //     }
    //     base.Setup(application);
    // }

    [DomainLogic(typeof(fmIDocCache))]
    public class fmIDocCacheLogic {

        //public static String Get_PayPartyName(fmIDocCache instance) {
        //    // A "Get_" method is executed when getting a target property value. The target property should be readonly.
        //    // Use this method to implement calculated properties.
        //    if (instance.PayPartyParty != null)
        //        return instance.PayPartyParty.Name;
        //    else if (instance.PayPartyStaff != null)
        //        return instance.PayPartyStaff.NameFull;
        //    else
        //        return String.Empty;
        //}

        public static void UpdatePartyName(fmIDocCache instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
            // Use this method to implement calculated properties.
            if (instance.PayPartyParty != null)
                instance.PayPartyName =  instance.PayPartyParty.Name;
            else if (instance.PayPartyStaff != null)
                instance.PayPartyName = instance.PayPartyStaff.NameFull;
            else
                instance.PayPartyName = String.Empty;
        }

        public static IList<hrmStaff> Get_GrossAccountStaffList(fmIDocCache instance, IObjectSpace os) {
            return fmCSettingsFinance.GetInstance(os).ManagerGroupOfSignAccountDepartmentStaffs;
        }

        public static crmIParty Get_PayPartyParty(fmIDocCache instance) {
            return instance.PayPartyPartyC;
        }
        public static String Get_AnaliticCode(fmIDocCache instance) {
            if (instance.Lines.Count > 0)
                return instance.Lines[0].AnaliticCode;
            else
                return String.Empty;
        }

        public static Int64 Get_SummaSenior(fmIDocCache instance) {
            return (Int64) instance.Summa;
        }

        public static String Get_SummaString(fmIDocCache instance) {
            return instance.SummaSeniorString + " руб. " + instance.SummaJuniorString + " коп.";
        }

        public static void Set_PayPartyParty(fmIDocCache instance,  crmIParty party) {
            if (party != null)
                instance.PayPartyPartyC = party.Party;
            else
                instance.PayPartyPartyC = null;
        }

        public static void AfterChange_PayPartyParty(fmIDocCache instance) {
            if (instance.PayPartyParty != null) {
                instance.PayPartyType = fmIDocCachePartyType.PARTY_PARTY;
                instance.UpdatePartyName();
            }
        }

        public static void AfterChange_PayPartyStaff(fmIDocCache instance) {
            if (instance.PayPartyStaff != null) {
                instance.PayPartyType = fmIDocCachePartyType.PARTY_STAFF;
                instance.UpdatePartyName();
            }
        }

        public static void AfterChange_PayPartyType(fmIDocCache instance) {
            if (instance.PayPartyType == fmIDocCachePartyType.PARTY_STAFF) 
                instance.PayPartyParty = null;
            if (instance.PayPartyType == fmIDocCachePartyType.PARTY_PARTY)
                instance.PayPartyStaff = null;
        }

        public static String Get_SummaSeniorString(fmIDocCache instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
            Int64 val = (Int64) instance.Summa;
            return RSDN.RusNumber.Str(val, true);
        }
        public static String Get_SummaJuniorString(fmIDocCache instance) {
            // A "Get_" method is executed when getting a target property value. The target property should be readonly.
            //        // Use this method to implement calculated properties.
            Int64 val = (Int64)instance.Summa;
            val = ((Int64)(instance.Summa * 100)) - val * 100;
            return val.ToString("D2");
        }
        public static void AfterChange_PersistentProperty(fmIDocCache instance) {
            // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
            // Use this method to refresh dependant property values.
        }
        public static void AfterConstruction(fmIDocCache instance, IObjectSpace os) {
            // The "AfterConstruction" method is executed only once, after an object is created. 
            // Use this method to initialize new objects with default property values.
            instance.DocDate = DateTime.Now;
            if (crmUserParty.CurrentUserParty != null) {
                if (crmUserParty.CurrentUserParty.Value != null) {
                    instance.DocParty = crmUserParty.CurrentUserPartyGet(os).Party;
                }
            }
            csCSecurityUser user = SecuritySystem.CurrentUser as csCSecurityUser;
            user = os.GetObject<csCSecurityUser>(user);
            if (user != null && user.Staff != null) {
                instance.DocPartyDepartment = user.Staff.Department;
            }
            
            instance.AccountDebit = os.FindObject<fmCFAAccount>(
                CriteriaOperator.And( 
                    new BinaryOperator("AccountSystem.Code", "1000"),
                    new BinaryOperator("BuhCode", "5011")
                    ));
            instance.AccountCredit = os.FindObject<fmCFAAccount>(
                CriteriaOperator.And( 
                    new BinaryOperator("AccountSystem.Code", "1000"),
                    new BinaryOperator("BuhCode", "6242")
                    ));
            instance.AVTMode = "1";
        }
    //    public static int SumMethod(fmIDocCache instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    }
}
