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
    [RequiresSTA] // Так на всякий случай, вдруг NUnit в разных тредах будет тесты запускать
    [TestFixture, Description("Проверка создания договора с этапами, этапа, а затем смена PaymentPlan в этапе")]
    public class DealWithStagePaymentMethodChangeTests : DealBaseTest
    {
        crmDealWithStageVersion dealWithStage;
        crmStage stage;

        public override void Init() {
            base.Init();

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);
            FillDatabase(uow);
            uow.CommitChanges();

            ruleSet = new RuleSet();

            CreateStageTest();

            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }


        #region Тестирование создания договороа без этапов

        crmContractRegistrationForm NewRegistrationFormPrepare(Session ses) {
            crmContractRegistrationForm frm = new crmContractRegistrationForm(ses);
            frm.RegistrationKind = RegistrationKind.NEW;
            frm.UserRegistrator = ses.GetObjectByKey<hrmStaff>(staff1.Oid);
            frm.ContractKind = ContractKind.CONTRACT;
            frm.DocumentCategory = ses.GetObjectByKey<crmContractDocumentType>(documentCategory1.Oid);
            frm.NewNumberRequired = true;
            frm.Category = ses.GetObjectByKey<crmContractCategory>(contractCategory1.Oid);
            frm.KindOfDeal = KindOfDeal.DEAL_WITH_STAGE;
            frm.CuratorDepartment = ses.GetObjectByKey<hrmDepartment>(department1.Oid);
            frm.Price = 10000;
            frm.Valuta = ses.GetObjectByKey<csValuta>(valuta1.Oid);
            frm.DateBegin = new DateTime(2011, 10, 1);
            frm.DateEnd = new DateTime(2011, 10, 31);
            frm.DateFinish = new DateTime(2011, 11, 11);
            frm.DescriptionShort = "Это тестовое описание";
            frm.OurRole = PartyRole.CUSTOMER;
            frm.OurParty = ses.GetObjectByKey<crmPartyRu>(partyRu1.Oid);
            frm.PartnerParty = ses.GetObjectByKey<crmPartyRu>(partyRu2.Oid);

            if (Common.CheckValidationRule) {
                ruleResult = ruleSet.ValidateTarget(frm, "Next");  //DefaultContexts.Save);
                Assert.AreEqual(ValidationState.Valid, ruleResult.State);  // GetResultItem(EmployeeValidationRules.EmployeeNameIsRequired).State);
            }

            return frm;
        }

        //[Test, Description("Тестирование создания договороа без этапов")]
        public void CreateDealWithStageTest() {

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);

            crmContractRegistrationForm frm = NewRegistrationFormPrepare(uow);
            IWizardSupport wiz = frm;
            dealWithStage = (crmDealWithStageVersion) wiz.Complete();

            uow.CommitChanges();
        }

        //[Test, Description("Тестирование создания этапа")]
        public void CreateStageTest() {

            CreateDealWithStageTest();

            UnitOfWork uow = new UnitOfWork(Common.dataLayer);

            crmDealWithStageVersion dws = uow.GetObjectByKey<crmDealWithStageVersion>(dealWithStage.Oid);
            stage = new crmStage(uow, VersionStates.VERSION_NEW);
            dws.Stages.Add(stage);
            stage.StageType = StageType.FINANCE;

            uow.CommitChanges();
        }

        // Проверка скорости выполнения смены свойства в XPO. В окружении XAF смена SCHEDULE на ADVANCE очень медленная
        // и забирает больщие ресурсы процессора
        [Test, Description("Тестирование смены PaymentMethod в этапе")]
        [Category("Debug")]
        public void PaymentMethodChangeTest([Range(0, 100, 1)]  int counter) {
            stage.PaymentMethod = PaymentMethod.SCHEDULE;
            stage.PaymentMethod = PaymentMethod.ADVANCE;
        }

        #endregion
    }
}
