using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using System.Diagnostics;

using System.Reflection;

using System.Data;

using IntecoAG.ERM.CRM.Contract;

namespace IntecoAG.ERM.Test
{
    using NUnit.Framework;

    [TestFixture, Description("Проверка класса Stage")]
    public class RegionNUnitTestCommon
    {
        Session session;

        [TestFixtureSetUp]
        public void Init() {
            session = new Session();
            Trace.WriteLine("Initialized session at " + DateTime.Now);
            Trace.WriteLine("Initialized at " + DateTime.Now);
        }

        [SetUp]
        public void TestSetup() {
            Trace.WriteLine("Test started at " + DateTime.Now);
        }

        [TearDown]
        public void TestDispose() {
            Trace.WriteLine("Test finished at " + DateTime.Now);
        }

        [TestFixtureTearDown]
        public void Cleanup() {
            Trace.WriteLine("Completed at " + DateTime.Now);
        }


        #region Тестирование кода для проверки корректности записи обобщённых дат в БД (Access, MS SQL)

        // Конкретно тестируем класс IntecoAG.ERM.CRM.Contract.Stage

        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTestStandartDate() {

            // Тест стандартной даты
            DateTimeExt dtextStandart = new DateTimeExt(System.DateTime.Now, session);
            DateTimeExtFieldTest(dtextStandart);
        }


        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTestMinDate() {

            // Тест минимального в C# значения стандартной даты
            DateTimeExt dtextMin = new DateTimeExt(System.DateTime.MinValue, session);
            DateTimeExtFieldTest(dtextMin);
        }


        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTestMaxDate() {

            // Тест максимального в C# значения стандартной даты
            DateTimeExt dtextMax = new DateTimeExt(System.DateTime.MaxValue, session);
            DateTimeExtFieldTest(dtextMax);
        }


        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTestNegativeInfinityDate() {

            // Тест на бесконечное прошлое
            DateTimeExt dtextNegativeInfinity = new DateTimeExt(CS.TimeSingularity.NegativeInfinity, session);
            DateTimeExtFieldTest(dtextNegativeInfinity);
        }


        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTestPositiveInfinityDate() {

            // Тест на бесконечное будущее
            DateTimeExt dtextPositiveInfinity = new DateTimeExt(CS.TimeSingularity.PositiveInfinity, session);
            DateTimeExtFieldTest(dtextPositiveInfinity);
        }


        [Test, Description("Тестирование класса IntecoAG.ERM.CRM.Contract.Stage на возможность работы с расширенными датами и интервалами дат")]
        [Category("Расширенные даты")]
        public void DateTimeExtTest() {

            // Тест стандартной даты
            DateTimeExt dtextStandart = new DateTimeExt(System.DateTime.Now, session);
            DateTimeExtFieldTest(dtextStandart);

            // Тест минимального в C# значения стандартной даты
            DateTimeExt dtextMin = new DateTimeExt(System.DateTime.MinValue, session);
            DateTimeExtFieldTest(dtextMin);

            // Тест максимального в C# значения стандартной даты
            DateTimeExt dtextMax = new DateTimeExt(System.DateTime.MaxValue, session);
            DateTimeExtFieldTest(dtextMax);

            // Тест на бесконечное прошлое
            DateTimeExt dtextNegativeInfinity = new DateTimeExt(CS.TimeSingularity.NegativeInfinity, session);
            DateTimeExtFieldTest(dtextNegativeInfinity);

            // Тест на бесконечное будущее
            DateTimeExt dtextPositiveInfinity = new DateTimeExt(CS.TimeSingularity.PositiveInfinity, session);
            DateTimeExtFieldTest(dtextPositiveInfinity);
        }

        /// <summary>
        /// Означивание заданного поля указанным значением
        /// </summary>
        /// <param name="checkedObject"></param>
        /// <param name="checkedFieldOrProperty"></param>
        /// <param name="checkedDateTimeExt"></param>
        private void CheckOfFieldOrPropertyTest(object checkedObject, string checkedFieldOrProperty, Time checkedTime) {
            try {
                Type ObjType = checkedObject.GetType();

                PropertyInfo prop = ObjType.GetProperty(checkedFieldOrProperty);
                FieldInfo fld = ObjType.GetField(checkedFieldOrProperty);

                if (prop != null) {
                    // Проверяем для свойства
                    setProperty(checkedObject, checkedFieldOrProperty, checkedTime);
                }
                else if (fld != null) {
                    // Проверяем для поля
                    setField(checkedObject, checkedFieldOrProperty, checkedTime);
                }
                else {
                    Assert.Fail("Не найдено поле или свойство с названием " + checkedFieldOrProperty);
                    return;
                }
            }
            catch (Exception ex) {
                Assert.Fail("Не пройден тест на размещение в объекте расширенной даты " + checkedFieldOrProperty + ". " + ex.InnerException.ToString());
            }
        }


        private void DateTimeExtFieldTest(DateTimeExt dtext) {
            session.ClearDatabase();

            try {
                Stage st1 = new Stage(session);
                {
                    session.BeginTransaction();

                    Time eTime = new Time(dtext, session);
                    CheckOfFieldOrPropertyTest(st1, "DateEnd", eTime);

                    Time bTime = new Time(dtext, session);
                    CheckOfFieldOrPropertyTest(st1, "DateBegin", bTime);

                    st1.Save();
                    session.CommitTransaction();
                }
            }
            catch (Exception ex) {
                Assert.Fail("Не пройден тест на ввод расширенной даты" + ex.InnerException.ToString());
            }

        }

        #endregion


        #region РЕФЛЕКСИЯ ДЛЯ СВОЙСТВ ОБЪЕКТОВ
        private object getProperty(object containingObject, string propertyName) {
            return containingObject.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, containingObject, null);
        }

        private void setProperty(object containingObject, string propertyName, object newValue) {
            containingObject.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, containingObject, new object[] { newValue });
        }

        private void setField(object containingObject, string fieldName, object newValue) {
            containingObject.GetType().InvokeMember(fieldName, BindingFlags.SetField, null, containingObject, new object[] { newValue });
        }



        //PropertyInfo[] propertyInfos;
        //propertyInfos = typeof(TestRegion).GetProperties(BindingFlags.GetProperty | BindingFlags.Public |
        //                                              BindingFlags.Static);
        //// sort properties by name
        //Array.Sort(propertyInfos,
        //        delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
        //        { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

        //// write property names
        //foreach (PropertyInfo propertyInfo in propertyInfos) {
        //    Console.WriteLine(propertyInfo.Name);
        //}


        #endregion

    }
}
