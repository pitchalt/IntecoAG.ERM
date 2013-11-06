using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

using IntecoAG.ERM.CRM.Party;

namespace IntecoAG.ERM.Trw {

    public enum TrwSaleNomenclatureMilitaryType {
        SALE_NOMENCLATURE_MILITARY_TYPE_UNKNOW = 0,
        SALE_NOMENCLATURE_MILITARY_TYPE_CIVIL = 1,
        SALE_NOMENCLATURE_MILITARY_TYPE_MILITARY = 2
    }
    //
    public enum TrwSaleNomenclatureType {
        SALE_NOMENCLATURE_TYPE_UNKNOW = 0,
        SALE_NOMENCLATURE_TYPE_SPECIAL_MACHINE = 1,
        SALE_NOMENCLATURE_TYPE_CIVIL_GOODS = 2,
        SALE_NOMENCLATURE_TYPE_COMPONENT = 3,
        SALE_NOMENCLATURE_TYPE_SCIENCE_WORK = 4,
        SALE_NOMENCLATURE_TYPE_OTHER_WORK = 5,
        SALE_NOMENCLATURE_TYPE_OTHER = 6
    }
    //
    public enum TrwSaleNomenclatureMeasurementUnit { 
        SALE_NOMENCLATURE_MEASUREMET_UNIT_UNKNOW = 0,
        SALE_NOMENCLATURE_MEASUREMET_UNIT_ITEM = 1,
        SALE_NOMENCLATURE_MEASUREMET_UNIT_KILOGRAM = 2,
        SALE_NOMENCLATURE_MEASUREMET_UNIT_TONNE = 3,
        SALE_NOMENCLATURE_MEASUREMET_UNIT_WORK = 4
    }
    //
    [DomainComponent]
    public interface TrwISaleNomenclature {
        [FieldSize(22)]
        String TrwCode { get; }
//        [FieldSize(128)]
        String TrwName { get; }
        TrwSaleNomenclatureMilitaryType TrwSaleNomenclatureMilitaryType { get; }
        TrwSaleNomenclatureType TrwSaleNomenclatureType { get; }
        TrwSaleNomenclatureMeasurementUnit TrwMeasurementUnit {get; }
        String TrwDescription { get; }
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwICfr));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwICfr))]
    //public class TrwICfrLogic {
    //    public static string Get_CalculatedProperty(TrwICfr instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwICfr instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwICfr instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwICfr instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
