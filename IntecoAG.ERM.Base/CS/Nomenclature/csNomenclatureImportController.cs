using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
//
using FileHelpers;
using FileHelpers.DataLink;
//
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.CS.Measurement;
//
namespace IntecoAG.ERM.CS.Nomenclature {
    public partial class csNomenclatureImportController : ViewController {
        public csNomenclatureImportController() {
            InitializeComponent();
            RegisterActions(components);
        }

        [DelimitedRecord(";")]
        public class csCNomenclatureMaterialImport {
            public String Code;
            public String CodeTechnical;
            public String NameShort;
            public String NameFull;
            public String MesUnit;
            public String TrwMesUnit;
            public String TrwType;

            public static void ImportData (IObjectSpace object_space, String file_name) {
                FileHelperEngine<csCNomenclatureMaterialImport> engine = new FileHelperEngine<csCNomenclatureMaterialImport>();
                engine.Options.IgnoreFirstLines = 1;
                csCNomenclatureMaterialImport[] imp_noms = engine.ReadFile(file_name);
                using (IObjectSpace os = object_space.CreateNestedObjectSpace()) {
                    IList<csUnit> mes_units = os.GetObjects<csUnit>();
                    IList<csMaterial> materials = os.GetObjects<csMaterial>();
                    foreach (csCNomenclatureMaterialImport imp_nom in imp_noms) {
                        csMaterial nom = materials.FirstOrDefault(x => x.Code == imp_nom.Code);
                        if (nom == null) {
                            nom = os.CreateObject<csMaterial>();
                            nom.Code = imp_nom.Code;
                        }
                        nom.NameShort = imp_nom.NameShort;
                        nom.NameFull = imp_nom.NameFull;
                        nom.NameFull = imp_nom.NameFull;
                        nom.CodeTechnical = imp_nom.CodeTechnical;
                        nom.BaseUnit = mes_units.FirstOrDefault(x => x.Code == imp_nom.MesUnit);
                        switch (imp_nom.TrwMesUnit) { 
                            case "Штуки":
                                nom.TrwMeasurementUnit = Trw.TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_ITEM;
                                break;
                            case "Тонна":
                                nom.TrwMeasurementUnit = Trw.TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_TONNE;
                                break;
                            case "Килограмм":
                                nom.TrwMeasurementUnit = Trw.TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_KILOGRAM;
                                break;
                            case "Единица работ":
                                nom.TrwMeasurementUnit = Trw.TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_WORK;
                                break;
                            default:
                                nom.TrwMeasurementUnit = Trw.TrwSaleNomenclatureMeasurementUnit.SALE_NOMENCLATURE_MEASUREMET_UNIT_UNKNOW;
                                break;
                        }
                        switch (imp_nom.TrwType) {
                            case "Группа 1":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_SPECIAL_MACHINE;
                                break;
                            case "Группа 2":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_SCIENCE_WORK;
                                break;
                            case "Товары народного потребления(ТНП)":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_CIVIL_GOODS;
                                break;
                            case "Полуфабрикаты":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_COMPONENT;
                                break;
                            case "Работы и услуги (кроме НИОРК)":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_OTHER_WORK;
                                break;
                            case "Прочая продукция":
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_OTHER;
                                break;
                            default:
                                nom.TrwSaleNomenclatureType = Trw.TrwSaleNomenclatureType.SALE_NOMENCLATURE_TYPE_UNKNOW;
                                break;
                        }
                        
                    }
                    os.CommitChanges();
                }

            }
        }

        private void ImportMaterialAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                csCNomenclatureMaterialImport.ImportData(ObjectSpace, dialog.FileName);
                ObjectSpace.CommitChanges();
            }
        }
    }
}
