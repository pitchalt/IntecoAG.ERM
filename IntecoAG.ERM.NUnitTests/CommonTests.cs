using System;
using System.Collections.Generic;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using IntecoAG.ERM.CRM.Contract.Utils;

namespace IntecoAG.ERM.CRM.Contract {
    public abstract class CommonTests<T> : EasyTestTestsBase<T> where T : IEasyTestFixtureHelper, new() {       
        public void ChangeContactNameTest_() {
            ITestControl control = adapter.CreateTestControl(TestControlType.Table, "");
            IGridBase table = control.GetInterface<IGridBase>();
            Assert.AreEqual(2, table.GetRowCount());

            List<IGridColumn> columns = new List<IGridColumn>(table.Columns);
            IGridColumn column = commandAdapter.GetColumn(control, "Full Name");

            Assert.AreEqual("John Nilsen", table.GetCellValue(0, column));
            Assert.AreEqual("Mary Tellitson", table.GetCellValue(1, column));

            commandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");

            Assert.AreEqual("Mary Tellitson", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Development Department", commandAdapter.GetFieldValue("Department"));
            Assert.AreEqual("Manager", commandAdapter.GetFieldValue("Position"));

            commandAdapter.DoAction("Edit", null);

            commandAdapter.SetFieldValue("First Name", "User_1");
            commandAdapter.SetFieldValue("Last Name", "User_2");

            commandAdapter.SetFieldValue("Position", "Developer");

            commandAdapter.DoAction("Save", null);

            Assert.AreEqual("User_1 User_2", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Developer", commandAdapter.GetFieldValue("Position"));
        }
        public void WorkingWithTasks_() {
            commandAdapter.DoAction("Navigation", "Task");
            commandAdapter.ProcessRecord("Task", new string[] { "Subject" }, new string[] { "Fix breakfast" }, "");

            ITestControl control = adapter.CreateTestControl(TestControlType.Table, "Contacts");
            IGridBase table = control.GetInterface<IGridBase>();
            Assert.AreEqual(0, table.GetRowCount());

            commandAdapter.DoAction("Contacts.Link", null);
            control = adapter.CreateTestControl(TestControlType.Table, "Contact");
            control.GetInterface<IGridRowsSelection>().SelectRow(0);
            commandAdapter.DoAction("OK", null);

            control = adapter. CreateTestControl(TestControlType.Table, "Contacts");
            table = control.GetInterface<IGridBase>();
            Assert.AreEqual(1, table.GetRowCount());
            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contacts", 0, "Full Name"));
        }
    }
}
