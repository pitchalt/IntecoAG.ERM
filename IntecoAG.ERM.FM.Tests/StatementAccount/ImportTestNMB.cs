using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
using NUnit.Framework;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
//
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Docs;
using IntecoAG.ERM.FM.StatementAccount;
using IntecoAG.ERM.FM.Controllers;

namespace IntecoAG.ERM.FM.Tests.StatementAccount {
    class ImportTestNMB : BaseTest {

        public override void TestFixtureSetUp() {
            //base.TestFixtureSetUp();
        }

        protected override void SetupModuleTypes() {
            base.SetupModuleTypes();
            Module.AdditionalExportedTypes.Add(typeof(crmBank));
            Module.AdditionalExportedTypes.Add(typeof(crmCLegalPerson));
            Module.AdditionalExportedTypes.Add(typeof(crmCLegalPersonUnit));
            Module.AdditionalExportedTypes.Add(typeof(fmCSAImporter1C));
            Module.AdditionalExportedTypes.Add(typeof(fmCSATaskImporterFile));
            Module.AdditionalExportedTypes.Add(typeof(fmCSAImportResult));
        }

        public override void TestFixtureTearDown() {
            //base.TestFixtureTearDown();
        }

        public override void SetUp() {
            base.TestFixtureSetUp();
            base.SetUp();
            _accounts = new Dictionary<Guid, Dictionary<string, crmBankAccount>>();
            _tasks = new Dictionary<Guid, fmCSATaskImporterFile>(10);
            _banks = new Dictionary<string, crmBank>(10);
            _leg_pers = new Dictionary<String, crmCLegalPerson>();
        }

        public override void TearDown() {
            base.TearDown();
            base.TestFixtureTearDown();
        }

        Dictionary<Guid, Dictionary<String, crmBankAccount>> _accounts;
        private Dictionary<Guid, fmCSATaskImporterFile> _tasks;
        private Dictionary<String, crmBank> _banks;
        Dictionary<String, crmCLegalPerson> _leg_pers;
        [Test]
        public void TestImportNMB() {
            IObjectSpace os;
            using (os = Application.CreateObjectSpace()) {
                fmCSAImportResult result = ImportNMB(os);
                os.CommitChanges();
                //
                Assert.AreEqual(result.ResultCode, 1);
                Assert.AreEqual(result.StatementOfAccounts.Count, 1);
            }
        }
        [Test]
        public void TestImportResultProcessActionNMB() {
            IObjectSpace os;
            using (os = Application.CreateObjectSpace()) {
                fmCSAImportResult result = ImportNMB(os);
                os.CommitChanges();
                fmCDocRCBImportResultViewController controller = new fmCDocRCBImportResultViewController();
                controller.SetView(Application.CreateDetailView(os, result));
                controller.ProcessAction.DoExecute();
                Assert.AreEqual(result.ResultCode, 3);
                TestImportResultPostprocess(os, result);
            }
        }
        
        protected void TestImportResultPostprocess(IObjectSpace os, fmCSAImportResult result) {
            foreach (fmCSAStatementAccount sa in result.StatementOfAccounts) {
                TestStatementAccountPostProcess(os, sa);
            }
        }

        protected void TestStatementAccountPostProcess(IObjectSpace os, fmCSAStatementAccount sa) {
            Assert.AreNotEqual(sa.ImportResult, null);
            IList<fmCSAOperationJournal> opers = os.GetObjects<fmCSAOperationJournal>(
                        CriteriaOperator.And(new BinaryOperator("BankAccount", sa.BankAccount),
                                             new BinaryOperator("OperationDate", sa.DateFrom.Date, BinaryOperatorType.GreaterOrEqual),
                                             new BinaryOperator("OperationDate", sa.DateTo.Date.AddDays(1), BinaryOperatorType.Less)), true);
            Decimal summ = 0;
            summ = opers.Sum(oper => oper.SumIn);
            Assert.AreEqual(sa.TotalRecaivedAtAccount, summ);
            summ = opers.Sum(oper => oper.SumOut);
            Assert.AreEqual(sa.TotalWriteOfAccount, summ);
            foreach (fmCSAStatementAccountDoc doc in sa.PayInDocs) {
                summ = opers.Where(oper => oper.PaymentDocument == doc.PaymentDocument).Sum(oper => oper.SumIn);
                Assert.AreEqual(doc.PaymentCost, summ);
            }
            foreach (fmCSAStatementAccountDoc doc in sa.PayOutDocs) {
                summ = opers.Where(oper => oper.PaymentDocument == doc.PaymentDocument).Sum(oper => oper.SumOut);
                Assert.AreEqual(doc.PaymentCost, summ);
            }
            foreach (fmCDocRCBRequisites req in sa.DocRCBRequisites) {
                Assert.AreNotEqual(req.StatementOfAccountDoc, null);
                Assert.AreEqual(sa.BankAccount, req.BankAccount);
                Assert.AreEqual(sa.Bank, req.Bank);
                fmCSAStatementAccountDoc doc = req.StatementOfAccountDoc;
                Assert.AreEqual(doc.ImportResult, sa.ImportResult);
                Assert.AreNotEqual(doc.PaymentPayerRequisites.BankAccount, doc.PaymentReceiverRequisites.BankAccount);
                TestDocRCBRequsites(os, doc.PaymentPayerRequisites);
                TestDocRCBRequsites(os, doc.PaymentReceiverRequisites);
                Assert.AreNotEqual(doc.PaymentDocument, null);
                fmCDocRCB paydoc = doc.PaymentDocument;
                TestDocRCBRequsites(os, paydoc.PaymentPayerRequisites);
                TestDocRCBRequsites(os, paydoc.PaymentReceiverRequisites);
                Assert.AreEqual(doc.PaymentPayerRequisites.BankAccount, paydoc.PaymentPayerRequisites.BankAccount);
                Assert.AreEqual(doc.PaymentPayerRequisites.Bank, paydoc.PaymentPayerRequisites.Bank);
                Assert.AreEqual(doc.PaymentReceiverRequisites.BankAccount, paydoc.PaymentReceiverRequisites.BankAccount);
                Assert.AreEqual(doc.PaymentReceiverRequisites.Bank, paydoc.PaymentReceiverRequisites.Bank);
                if (doc.PaymentReceiverRequisites == req) {
                    Assert.AreEqual(doc.StatementAccountIn, sa);
                }
                if (doc.PaymentPayerRequisites == req) {
                    Assert.AreEqual(doc.StatementAccountOut, sa);
                }
            }
        }

        protected void TestDocRCBRequsites(IObjectSpace os, fmCDocRCBRequisites req) {
            Assert.AreNotEqual(req.Bank, null);
            if (req.Party != null) {
                Assert.AreNotEqual(req.BankAccount, null);
                Assert.AreEqual(req.BankAccount.Bank, req.Bank);
            }
            else {
                if (!String.IsNullOrEmpty(req.INN)) {
                    IList<crmCParty> partys = os.GetObjects<crmCParty>(new BinaryOperator("INN", req.INN), true);
                    foreach (crmCParty party in partys) {
                        if (party.IsClosed) continue;
                        Assert.AreNotEqual(party.KPP, req.KPP);
//                        Assert.AreEqual(req.BankAccount.Bank, req.Bank);
                    }
                }
            }
        }

        protected fmCSAImportResult ImportNMB(IObjectSpace os) {
            crmBank bank = BankGet(os, "044579852", "НМБ");
            fmCSATaskImporterFile task = ImportTaskGet(os, bank, "NMB", "fmSAImportTest03.txt");
            crmCLegalPerson pers = LegalPersonGet(os, "2518", "НПО машиностроения", "5012039795", "509950001");
            crmBankAccount acc;
            acc = BankAccountGet(os, bank, "40502810000000000007", pers.Person);
            acc.PrefferedParty = pers.Party;
            os.CommitChanges();
            fmCSAImportResult result = task.ExecuteTask(GetTestDataStream("fmSAImportTest03.txt"));
            return result;
        }
        protected crmCLegalPerson LegalPersonGet(IObjectSpace os, String code, String name, String INN, String KPP) {
            if (_leg_pers.ContainsKey(code)) {
                return os.GetObject<crmCLegalPerson>(_leg_pers[code]);
            }
            else {
                crmCLegalPerson pers = os.CreateObject<crmCLegalPerson>();
                pers.Code = code;
                pers.Name = name;
                pers.INN = INN;
                pers.KPP = KPP;
                return pers;
            }
        }
        //        private fmCSATaskImporterFile _ImportTask;
        protected fmCSATaskImporterFile ImportTaskGet(IObjectSpace os, crmBank bank, String code, String path) {
            if (_tasks.ContainsKey(bank.Oid)) {
                return os.GetObject<fmCSATaskImporterFile>(_tasks[bank.Oid]);
            }
            else {
                fmCSATaskImporterFile task = os.CreateObject<fmCSATaskImporterFile>();
                task.Code = code;
                task.Importer = ImporterGet(os);
                task.Bank = bank;
                task.CheckedPath = path;
                _tasks[bank.Oid] = task;
                return task;
            }
        }

        protected crmBank BankGet(IObjectSpace os, String BIC, String Name) {
            if (_banks.ContainsKey(BIC))
                return os.GetObject<crmBank>(_banks[BIC]);
            else {
                crmBank bank = os.CreateObject<crmBank>();
                bank.RCBIC = BIC;
                bank.Name = Name;
                _banks[BIC] = bank;
                return bank;
            }
        }

        protected crmBankAccount BankAccountGet(IObjectSpace os, crmBank bank, String number, crmCPerson pers) {
            if (_accounts.ContainsKey(bank.Oid) && _accounts[bank.Oid].ContainsKey(number)) {
                return os.GetObject<crmBankAccount>(_accounts[bank.Oid][number]);
            }
            else {
                crmBankAccount acc = os.CreateObject<crmBankAccount>();
                acc.Bank = bank;
                acc.Number = number;
                acc.Person = pers;
                if (!_accounts.ContainsKey(bank.Oid)) {
                    _accounts[bank.Oid] = new Dictionary<string, crmBankAccount>();
                }
                _accounts[bank.Oid][number] = acc;
                return acc;
            }
        }

        protected fmCSAImporter1C ImporterGet(IObjectSpace os) {
            return fmCSAImporter1C.GetInstance(((ObjectSpace)os).Session);
        }
    }
}
