using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using NUnit.Framework;

using DevExpress.ExpressApp;

//using DevExpress.Xpo;
//using DevExpress.Xpo.DB;

using IntecoAG.ERM.FM.Order;

namespace IntecoAG.ERM.FM.FinPlan.Subject {

    [TestFixture, Description("Парсинг Excel XML")]
    public class FmFinPlanSubjectDocFullTests: BaseTest {

        protected override void SetupModuleTypes() {
            base.SetupModuleTypes();
//            Module.AdditionalExportedTypes.Add(typeof(FmFinPlanSubjectDocFull));
//            Module.AdditionalExportedTypes.Add(typeof(fmСOrderAnalitycBigCustomer));
//            Module.AdditionalExportedTypes.Add(typeof(fmСOrderAnalitycRegion));
        }

        public override void TestFixtureSetUp() {
//            base.TestFixtureSetUp();
        }

        public override void TestFixtureTearDown() {
//            base.TestFixtureTearDown();
        }

        public override void SetUp() {
            base.TestFixtureSetUp();
            base.SetUp();
        }

        public override void TearDown() {
            base.TearDown();
            base.TestFixtureTearDown();
        }

        protected void BaseTest(String test) {
            BaseTest(GetTestDataStream(test));
        }

        protected void BaseTest(Stream stream) {
//            IObjectSpace os;
//            using (os = Application.CreateObjectSpace()) {
//                FmFinPlanSubjectDocFull doc = os.CreateObject<FmFinPlanSubjectDocFull>();
                FmFinPlanSubjectDocXMLLoader loader = new FmFinPlanSubjectDocXMLLoader(null, null, stream);
                loader.Load();
                //
//            }

        }

        [Test]
        public void TestSequencialTags() {
            BaseTest("FmFinPlanDocImportTest01.txt");
        }
        
        [Test]
        public void TestSequencialWithSpaceTags() {
            BaseTest("FmFinPlanDocImportTest02.txt");
        }

        [Test]
        public void TestRealDoc() {
            BaseTest("FmFinPlanDocImportTest03.txt");
        }
    }
}
