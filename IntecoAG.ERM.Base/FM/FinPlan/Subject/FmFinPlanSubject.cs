using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.XafExt;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [NavigationItem("FinPlan")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class FmFinPlanSubject : FmFinPlanPlan {
        public FmFinPlanSubject(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
            _Journal = new FmJournal(this.Session);
            _Journal.FinPlanSet(this);
            _Journal.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _Journal.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _Journal.JournalTypeObjectSet(JournalTypeObject.FM_JTO_SUBJECT);
            _Journal.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_FULL);
            _Journal.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN);
            //
            _JournalPlanYear = new FmJournal(this.Session);
            _JournalPlanYear.FinPlanSet(this);
            _JournalPlanYear.JournalTypeAccountingSet(JournalTypeAccounting.FM_JTA_FINANCIAL);
            _JournalPlanYear.JournalTypeLegalSet(JournalTypeLegal.FM_JTL_COMPANY);
            _JournalPlanYear.JournalTypeObjectSet(JournalTypeObject.FM_JTO_SUBJECT);
            _JournalPlanYear.JournalTypePeriodSet(JournalTypePeriod.FM_JTP_YEAR);
            _JournalPlanYear.JournalTypeSourceSet(JournalTypeSource.FM_JTS_FINPLAN);

            crmUserParty user_org = crmUserParty.CurrentUserPartyGet(this.Session);
            if (user_org != null) {
                AccountingContract = user_org.AccountingContract;
                AccountingFact = user_org.AccountingFact;
            }
        }

        private fmCSubject _Subject;
        [RuleRequiredField]
        [Appearance("", AppearanceItemType.ViewItem, "Subject != Null", Enabled = false)]
        public fmCSubject Subject {
            get { return _Subject; }
            set {
                if (!IsLoading && _Subject != null)
                    throw new InvalidOperationException("Изменить нельзя если уже задано");
                SetPropertyValue<fmCSubject>("Subject", ref _Subject, value);
                if (!IsLoading) {
                    Journal.SubjectSet(value);
                    if (value != null) {
                        CodeSet("ФПТ." + value.Code);
                        _Journal.CodeSet(Code + ".П0");
                        _JournalPlanYear.CodeSet(Code + ".ПГ");
                    }
                }
            } 
        }

        [Association("FmFinPlanSubject-FmFinPlanSubjectDoc"), Aggregated]
        public XPCollection<FmFinPlanSubjectDoc> FinPlanSubjectDocs {
            get { return GetCollection<FmFinPlanSubjectDoc>("FinPlanSubjectDocs"); }
        }

        [Persistent("JournalPlanYear")]
        [Aggregated]
        protected FmJournal _JournalPlanYear;
        [PersistentAlias("_JournalPlanYear")]
//        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public FmJournal JournalPlanYear {
            get { return _JournalPlanYear; }
        }

        [PersistentAlias("JournalPlanYear.Operations")]
        [Aggregated]
        public XPCollection<FmJournalOperation> PlanYearOperations {
            get {
                return JournalPlanYear.Operations;
            }
        }

        [PersistentAlias("Journal.Operations")]
        [Aggregated]
        public XPCollection<FmJournalOperation> PlanFullOperations {
            get {
                return Journal.Operations;
            }
        }

        protected override CriteriaOperator OperationsCriteria {
            get {  
                return XPQuery<FmJournalOperation>.TransformExpression(this.Session, 
                    x => x.Subject == this.Subject && ( 
                        x.Journal == Journal ||
                        x.Journal == JournalPlanYear ||
                        x.Journal == AccountingFact.Journal ||
                        x.Journal == AccountingContract.Journal 
                        )
                    );
            }
        }

        public void Transact(FmFinPlanSubjectDocFull doc) { 
            FmFinPlanSubjectLogic.TransactPlan0(ObjectSpace.FindObjectSpaceByObject(this), this, doc);
        }
    }

}
