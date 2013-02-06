using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.AVT;
//
namespace IntecoAG.ERM.FM.DatabaseUpdate {
    public class AVTInvoiceUpdaterFix01 : ModuleUpdater {
        public AVTInvoiceUpdaterFix01(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                csValuta val_rub = os.FindObject<csValuta>(
                    XPQuery<csValuta>.TransformExpression(((ObjectSpace)os).Session,
                        val => val.Code == "RUB"));
//                fmCAVTInvoiceVersion ver;
                IList<fmCAVTInvoiceVersion> ver_list = os.GetObjects<fmCAVTInvoiceVersion>(
                    XPQuery<fmCAVTInvoiceVersion>.TransformExpression(((ObjectSpace)os).Session, 
                        inv => inv.Valuta == null));
                foreach (fmCAVTInvoiceVersion ver in ver_list)
                    ver.Valuta = val_rub;
                os.CommitChanges();
            }
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<fmCAVTInvoiceBase> inv_list = os.GetObjects<fmCAVTInvoiceBase>(new UnaryOperator(UnaryOperatorType.IsNull, "InvoiceIntType"));
                foreach (fmCAVTInvoiceBase inv in inv_list)
                    inv.InvoiceIntType = fmCAVTInvoiceIntType.NORMAL;
                os.CommitChanges();
            }
            ObjectSpace.CommitChanges();
        }
    }
}
