using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using NUnit.Framework;
//
using DevExpress.ExpressApp;
//
using IntecoAG.ERM.CS.Nomenclature;
//using IntecoAG.ERM.FM


namespace IntecoAG.ERM.FM.Tests {

    public class ValutaEditorTest:BaseTest {

        protected override void SetupModuleTypes() {
            base.SetupModuleTypes();
            Module.AdditionalExportedTypes.Add(typeof(csValuta));
            Module.AdditionalExportedTypes.Add(typeof(csCNMCourseEditor));
        }

        public override void SetUp() {
            base.SetUp();
        }
        [Test]
        public void TestEditor() {
            IObjectSpace os;
            using (os = Application.CreateObjectSpace()) {
                csValuta val1 = os.CreateObject<csValuta>();
                val1.Code = "USD";
                val1.ConversionIndex = 1;
                csValuta val2 = os.CreateObject<csValuta>();
                val2.Code = "EUR";
                val2.ConversionIndex = 1;
                //
                os.CommitChanges();
            }
            DateTime date = DateTime.Now;
            os = Application.CreateObjectSpace();
            csCNMCourseEditor editor = os.CreateObject<csCNMCourseEditor>();
            Assert.AreEqual(editor.CourseDayTable.Count, 0);
            editor.CourseDate = date;
            Assert.AreEqual(editor.CourseDayTable.Count, 2);
            foreach(var course in editor.CourseDayTable) {
                Assert.AreEqual(course.CourseDate, editor.CourseDate);
            }
            editor.CourseDate = editor.CourseDate.AddDays(1);
            Assert.AreEqual(editor.CourseDayTable.Count, 2);
            foreach (var course in editor.CourseDayTable) {
                Assert.AreEqual(course.CourseDate, editor.CourseDate);
            }
            using (IObjectSpace os2 = Application.CreateObjectSpace()) {
                csValuta val3 = os2.CreateObject<csValuta>();
                val3.Code = "RUB";
                val3.ConversionIndex = 1;
                //
                os2.CommitChanges();
            }
            editor.CourseDate = editor.CourseDate.AddDays(-1);
            Assert.AreEqual(editor.CourseDayTable.Count, 3);
            foreach (var course in editor.CourseDayTable) {
                Assert.AreEqual(course.CourseDate, editor.CourseDate);
            }
            os.CommitChanges();
            os = Application.CreateObjectSpace();
            editor = os.CreateObject<csCNMCourseEditor>();
            Assert.AreEqual(editor.CourseDayTable.Count, 0);
            editor.CourseDate = date;
            Assert.AreEqual(editor.CourseDayTable.Count, 3);
            foreach (var course in editor.CourseDayTable) {
                Assert.AreEqual(course.CourseDate, editor.CourseDate);
            }
        }
    }
}
