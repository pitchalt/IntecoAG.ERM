using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Editors;

namespace IntecoAG.ERM {

    public class TestApplication : XafApplication {
        public TestApplication(): base() {
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ERMWindowsFormsApplication_DatabaseVersionMismatch);
        }
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }
        protected override ListEditor CreateListEditorCore(
            IModelListView modelListView, CollectionSourceBase collectionSource) {
            return new TestListEditor(modelListView);
        }
        
        private void ERMWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
//            if (System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
//            } else {
//                throw new InvalidOperationException(
//                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
//                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
//                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
//                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
//                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
//                    "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " +
//                    "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
//            }
        }
    }

    internal class TestListEditor : ListEditor {
        public TestListEditor(IModelListView modelListView) {
            SetModel(modelListView);
        }
        protected override object CreateControlsCore() {
            throw new NotImplementedException();
        }
        protected override void AssignDataSourceToControl(object dataSource) { }
        public override void Refresh() { }
        public override System.Collections.IList GetSelectedObjects() {
            return new List<object>();
        }
        public override SelectionType SelectionType {
            get { return SelectionType.MultipleSelection; }
        }
        public override DevExpress.ExpressApp.Templates.IContextMenuTemplate ContextMenuTemplate {
            get { return null; }
        }
    }
}
