using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

using IntecoAG.ERM.FM.FinJurnal;

namespace IntecoAG.ERM.FM.DatabaseUpdate {

    public class fmSaleDocUpdater01 : ModuleUpdater {
        public fmSaleDocUpdater01(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            if (this.CurrentDBVersion.ToString() != "1.1.1.163")   // Поправить на правильный номер!
                return;

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
