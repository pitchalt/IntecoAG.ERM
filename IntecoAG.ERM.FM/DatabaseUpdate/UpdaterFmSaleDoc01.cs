using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

using IntecoAG.ERM.FM.FinJurnal;

namespace IntecoAG.ERM.FM.DatabaseUpdate {

    public class UpdaterFmSaleDoc01 : ModuleUpdater {
        public UpdaterFmSaleDoc01(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            // Disable version
            if (this.CurrentDBVersion != new Version("0.0.0.0"))
                return;
            //

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCFJSaleDoc> doc_col = os.GetObjects<fmCFJSaleDoc>();
                foreach (fmCFJSaleDoc doc in doc_col) {
                    if (String.IsNullOrEmpty(doc.SalePeriod)) {
                        doc.SalePeriod = doc.Period.ToString();
                    }
                }
                os.CommitChanges();
            }
       }
    }

}
