using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;

namespace IntecoAG.ERM.CRM.Contract.Utils {
    public class TestCommandAdapter {
        private ICommandAdapter adapter;
        public TestCommandAdapter(ICommandAdapter webAdapter) {
            this.adapter = webAdapter;
        }
        internal void DoAction(string name, string paramValue) {
            ActionCommand.DoAction(adapter, name, paramValue);
        }
        internal string GetActionValue(string name) {
            IControlText control = adapter.CreateTestControl(TestControlType.Action, name).GetInterface<IControlText>();
            return control.Text;
        }
        internal string GetFieldValue(string fieldName) {
            return CheckFieldValuesCommand.GetFieldValue(adapter, fieldName);
        }
        internal void ProcessRecord(string tableName, string[] columnNames, string[] values, string actionName) {
            ProcessRecordCommand command = new ProcessRecordCommand();
            command.ProcessRecord(adapter, tableName, actionName, columnNames, values);
        }
        internal void SetFieldValue(string fieldName, string value) {
            FillFieldCommand.SetFieldCommand(adapter, fieldName, value);
        }
        public IGridColumn GetColumn(ITestControl testControl, string columnName) {
            foreach(IGridColumn column in testControl.GetInterface<IGridBase>().Columns) {
                if(column.Caption == columnName) {
                    return column;
                }
            }
            return null;
        }
        internal string GetCellValue(string tableName, int row, string columnName) {
            ITestControl testControl = adapter.CreateTestControl(TestControlType.Table, tableName);
            IGridBase gridControl = testControl.GetInterface<IGridBase>();
            return gridControl.GetCellValue(row, GetColumn(testControl, columnName));
        }
        internal object GetTableRowCount(string tableName) {
            IGridBase gridControl = adapter.CreateTestControl(TestControlType.Table, tableName).GetInterface<IGridBase>();
            return gridControl.GetRowCount();
        }
    }
}