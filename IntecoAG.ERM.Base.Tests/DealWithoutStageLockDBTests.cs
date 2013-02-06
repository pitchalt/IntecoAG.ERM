using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Data.Filtering;

using IntecoAG.ERM.CRM;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Common;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Measurement;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Finance;
using IntecoAG.ERM.FM;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.HRM.Organization;

using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Contract.Forms;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using NUnit.Framework;

namespace IntecoAG.ERM.CRM.Contract
{

    [RequiresMTA]
    [TestFixture, Description("Проверка правильности выдачи номеров контрактам при одновременном доступе к счётчику в БД")]
    public class DealWithoutStageLockDBTests : DealBaseTest
    {
        crmDealWithoutStageVersion dealWithoutStage;
        crmDealWithoutStageVersion dealWithoutStage1;

        public override void Init() {
            base.Init();

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);
            FillDatabase(uow);
            uow.CommitChanges();

            ruleSet = new RuleSet();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        #region Тестирование создания договороа без этапов

        private crmContractRegistrationForm NewRegistrationFormPrepare(Session ssn) {
            crmContractRegistrationForm frm = new crmContractRegistrationForm(ssn);
            frm.RegistrationKind = RegistrationKind.NEW;
            frm.UserRegistrator = ssn.GetObjectByKey<hrmStaff>(staff1.Oid);
            frm.ContractKind = ContractKind.CONTRACT;
            frm.DocumentCategory = ssn.GetObjectByKey<crmContractDocumentType>(documentCategory1.Oid);
            frm.NewNumberRequired = true;
            frm.Category = ssn.GetObjectByKey<crmContractCategory>(contractCategory1.Oid);
            frm.KindOfDeal = KindOfDeal.DEAL_WITHOUT_STAGE;
            frm.CuratorDepartment = ssn.GetObjectByKey<hrmDepartment>(department1.Oid);
            frm.Price = 10000;
            frm.Valuta = ssn.GetObjectByKey<csValuta>(valuta1.Oid);
            frm.DateBegin = new DateTime(2011, 10, 1);
            frm.DateEnd = new DateTime(2011, 10, 31);
            frm.DateFinish = new DateTime(2011, 11, 11);
            frm.DescriptionShort = "Это тестовое описание";
            frm.OurRole = PartyRole.CUSTOMER;
            frm.OurParty = ssn.GetObjectByKey<crmCParty>(partyRu1.Oid);
            frm.PartnerParty = ssn.GetObjectByKey<crmCParty>(partyRu2.Oid);

            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(frm, "Next");  //DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);  // GetResultItem(EmployeeValidationRules.EmployeeNameIsRequired).State);
            }

            return frm;
        }

        [RequiresMTA]
        [Test, Description("Тестирование создания двух договоров без этапов")]
        //[Category("Производительность создания договоров")]
        [Category("Debug")]
        public void CreateDealWithoutStageLockDBTest() {

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);

            crmContractRegistrationForm frm = NewRegistrationFormPrepare(uow);
            IWizardSupport wiz = frm;
            dealWithoutStage = (crmDealWithoutStageVersion)wiz.Complete();

            // ---
            UnitOfWork uow1 = new UnitOfWork(Common.dataLayer);

            crmContractRegistrationForm frm1 = NewRegistrationFormPrepare(uow1);
            IWizardSupport wiz1 = frm1;
            dealWithoutStage1 = (crmDealWithoutStageVersion)wiz1.Complete();

            uow1.CommitChanges();
            // ---

            uow.CommitChanges();
            Assert.AreEqual(dealWithoutStage1.ContractDocument.Number, "2/000000-2011");
            Assert.AreEqual(dealWithoutStage.ContractDocument.Number,  "1/000000-2011");
        }

        #endregion

    }
}
