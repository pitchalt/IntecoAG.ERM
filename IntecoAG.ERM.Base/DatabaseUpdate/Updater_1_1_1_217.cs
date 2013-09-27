using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.IO;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Country;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CRM.Contract;
using IntecoAG.ERM.CRM.Contract.Obligation;
using IntecoAG.ERM.CRM.Contract.Deal;
using IntecoAG.ERM.CRM.Party;
using IntecoAG.ERM.FM.Subject;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.Trw;
using IntecoAG.ERM.Trw.Party;
using IntecoAG.ERM.Trw.Contract;
using IntecoAG.ERM.Trw.Nomenclature;
//using IntecoAG.ERM.FM.AVT;

namespace IntecoAG.ERM.FM {
    public class Updater_1_1_1_217 : ModuleUpdater {
        public Updater_1_1_1_217(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        private void UpdateNomenclatures() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                IList<csService> nom_mat = os.GetObjects<csService>();
                foreach (csService nom in nom_mat) {
                    if (nom.Code == null)
                        nom.Code = "";
                    if (nom.Code.Length > 0 && nom.Code.Length < 5)
                        nom.Code = '9' + nom.Code.PadLeft(4, '0');
                    nom.TrwMeasurementUnit = TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_WORK;
                }
                IList<crmDeliveryItem> items = os.GetObjects<crmDeliveryItem>();
                foreach (crmDeliveryItem item in items) {
                    item.UpdateTrwNomenclature();
                }
                IList<TrwSaleNomenclature> sale_noms = os.GetObjects<TrwSaleNomenclature>(null, true);
                foreach (TrwSaleNomenclature nom in sale_noms) {
                    nom.TrwExportStateSet(Trw.Exchange.TrwExchangeExportStates.CREATED);
                    nom.UpdatePropertys();
                }

                os.CommitChanges();
            }
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            // Disable version
            if (this.CurrentDBVersion == new Version("1.1.1.217"))
                UpdateNomenclatures();
        }

    }
}
