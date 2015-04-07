using System;
using System.IO;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
//using DevExpress.ExpressApp.Reports;
//using DevExpress.ExpressApp.PivotChart;
//using DevExpress.ExpressApp.Security.Strategy;
//using TaxRu.Module.BusinessObjects;

namespace IntecoAG.ERM.FM.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            String exename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fi = new FileInfo(exename);
            String path = fi.Directory.FullName + "\\Import\\FmTaxRuVatOperationTypes.xml";
            using (Stream stream = new FileStream(path, FileMode.Open)) {
                FmTaxRuVatOperationTypesImpoter ot_loader = new FmTaxRuVatOperationTypesImpoter(ObjectSpace, stream);
                ot_loader.Load();
            }

            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
