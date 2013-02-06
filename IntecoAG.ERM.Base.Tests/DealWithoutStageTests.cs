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

    [TestFixture, Description("Проверка создания простого договора без этапов")]
    public class DealWithoutStageTests : DealBaseTest
    {
        crmDealWithoutStageVersion dealWithoutStage;

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

        [Test, Description("Тестирование создания договороа без этапов")]
        //[Category("Производительность создания договоров")]
        [Category("Debug")]
        public void CreateDealWithoutStageTest() {

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);

            crmContractRegistrationForm frm = NewRegistrationFormPrepare(uow);
            IWizardSupport wiz = frm;
            dealWithoutStage = (crmDealWithoutStageVersion)wiz.Complete();

            uow.CommitChanges();
        }

        [Test, Description("Тестирование создания объектов по отдельности")]
        //[Category("Производительность создания договоров")]
        [Category("Debug")]
        public void CreateAllReferencesOnlyTest([Values("", "0", "1", "2", "3", "4")]  string modificator) {
            //Session ssn = session1;
            UnitOfWork ssn = new UnitOfWork(Common.dataLayer);

            crmContractCategory contractCategory = Prepare_crmContractCategory(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(contractCategory, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            crmCostModel costModel = Prepare_crmCostModel(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(costModel, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csCountry country = Prepare_csCountry(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(country, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csUnit unit = Prepare_csUnit(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(unit, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csNomenclatureType nomenclatureType = Prepare_csNomenclatureType(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(nomenclatureType, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            fmCostItem costItem = Prepare_fmCostItem(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(costItem, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csMaterial material = Prepare_csMaterial(ssn, modificator, nomenclatureType, unit, costItem);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(material, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csService service = Prepare_csService(ssn, modificator, nomenclatureType, unit, costItem);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(service, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csValuta valuta = Prepare_csValuta(ssn, modificator, nomenclatureType, unit, costItem);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(valuta, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            csAddress address = Prepare_csAddress(ssn, modificator, country);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(address, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            crmPhysicalPerson physicalPersonRu = Prepare_crmPhysicalPerson(ssn, modificator, address);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(physicalPersonRu, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            crmCBusinessman businessmanRu = Prepare_crmBusinessmanRu(ssn, modificator, physicalPersonRu, address);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(businessmanRu, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            crmCLegalPerson legalPersonRussianRu = Prepare_crmCLegalPerson(ssn, modificator, address);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(legalPersonRussianRu, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            //crmPartyRu partyRu = Prepare_crmPartyRu(ssn, modificator, address, legalPersonRussianRu);
            //if (Common.CheckValidationRule) {
            //    ruleResult = ruleSet.ValidateTarget(partyRu, DefaultContexts.Save);
            //    Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            //}

            fmCSubject subject = Prepare_fmSubject(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(subject, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            hrmDepartment department = Prepare_hrmDepartment(ssn, modificator);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(department, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            hrmStaff staff = Prepare_hrmStaff(ssn, modificator, physicalPersonRu, department);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(staff, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            crmUserParty userParty = Prepare_crmUserParty(ssn, modificator, legalPersonRussianRu.Party);
            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(userParty, DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);
            }

            ssn.CommitChanges();
        }

        #endregion

    }
}
