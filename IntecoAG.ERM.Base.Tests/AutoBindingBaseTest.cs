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

namespace IntecoAG.ERM.FM
{

    //[TestFixture, Description("Проверка создания простого договора без этапов")]
    public class AutoBindingBaseTest
    {

        [TestFixtureSetUp]
        public virtual void Init() {
            // Метод выполняется один раз до любых тестов
            Common.PrepareDB();

            // Прочистка БД
            //if (Common.dataStore != null) ((IDataStoreForTests)Common.dataStore).ClearDatabase();
        }

        [SetUp]
        public virtual void TestSetup() {
            // Метод выполняется перед каждым ("элементарным") тестом.
            Trace.WriteLine("Test started at " + DateTime.Now);
        }

        [TearDown]
        public virtual void TestDispose() {
            // Метод выполняется после каждого ("элементарного") теста.
            Trace.WriteLine("Test finished at " + DateTime.Now);
        }

        [TestFixtureTearDown]
        public virtual void Cleanup() {
            // Метод выполняется после завершения всех тестов.
            Trace.WriteLine("Completed at " + DateTime.Now);
        }
    }
}
