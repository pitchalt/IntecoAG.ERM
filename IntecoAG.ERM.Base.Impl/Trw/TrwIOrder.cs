using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace IntecoAG.ERM.Trw {

    public enum TrwOrderWorkType {
        WORK_TYPE_UNKNOW = 0,
        WORK_TYPE_LINE = 11,
        WORK_TYPE_NIOKR = 21,
        WORK_TYPE_OTHER_LEASE = 31,
        WORK_TYPE_OTHER_BUILDING = 32,
        WORK_TYPE_OTHER_COMMUNAL = 33,
        WORK_TYPE_OTHER_SALE = 34,
        WORK_TYPE_OTHER_SOCIAL = 35,
        WORK_TYPE_OTHER_OTHER = 36
    }

    public static class TrwOrderWorkTypeLogic {
        public static String GetOrderWorkTypeCode(TrwOrderWorkType work_type) {
            switch (work_type) { 
                case TrwOrderWorkType.WORK_TYPE_LINE:
                    return "Группа 1";
                case TrwOrderWorkType.WORK_TYPE_NIOKR:
                    return "Группа 2";
                case TrwOrderWorkType.WORK_TYPE_OTHER_LEASE:
                    return "Предоставление в аренду";
                case TrwOrderWorkType.WORK_TYPE_OTHER_BUILDING:
                    return "Строительно-ремонтные услуги";
                case TrwOrderWorkType.WORK_TYPE_OTHER_COMMUNAL:
                    return "Коммунальные услуги";
                case TrwOrderWorkType.WORK_TYPE_OTHER_SALE:
                    return "Торговые услуги";
                case TrwOrderWorkType.WORK_TYPE_OTHER_SOCIAL:
                    return "Использование объектов социального назначения";
                case TrwOrderWorkType.WORK_TYPE_OTHER_OTHER:
                    return "Прочие услуги и работы";
                default:
                    return null;
            }
        }
        public static String GetFinWorkTypeCode(TrwOrderWorkType work_type) {
            switch (work_type) { 
                case TrwOrderWorkType.WORK_TYPE_LINE:
                    return "11";
                case TrwOrderWorkType.WORK_TYPE_NIOKR:
                    return "22";
                case TrwOrderWorkType.WORK_TYPE_OTHER_LEASE:
                    return "33";
                case TrwOrderWorkType.WORK_TYPE_OTHER_BUILDING:
                    return "33";
                case TrwOrderWorkType.WORK_TYPE_OTHER_COMMUNAL:
                    return "33";
                case TrwOrderWorkType.WORK_TYPE_OTHER_SALE:
                    return "33";
                case TrwOrderWorkType.WORK_TYPE_OTHER_SOCIAL:
                    return "33";
                case TrwOrderWorkType.WORK_TYPE_OTHER_OTHER:
                    return "33";
                default:
                    return null;
            }
        }
    }

    [DomainComponent]
    public interface TrwIOrder {
        TrwIContract TrwContract { get; }
        String TrwCode { get; }
        String TrwInternalCode { get; }

        TrwOrderWorkType TrwOrderWorkType { get; }
        String TrwOrderWorkTypeCode { get; }
        String TrwFinWorkTypeCode { get; }
        //string CalculatedProperty { get; }
        //int SumMethod(int val1, int val2);
    }

    // To use a Domain Component in an XAF application, the Component should be registered.
    // Override the ModuleBase.Setup method in the application's module and invoke the ITypesInfo.RegisterEntity method in it:
    //
    // public override void Setup(XafApplication application) {
    //     if (!XafTypesInfo.IsInitialized) {
    //         XafTypesInfo.Instance.RegisterEntity("MyComponent", typeof(TrwIOrder));
    //     }
    //     base.Setup(application);
    // }

    //[DomainLogic(typeof(TrwIOrder))]
    //public class TrwIOrderLogic {
    //    public static string Get_CalculatedProperty(TrwIOrder instance) {
    //        // A "Get_" method is executed when getting a target property value. The target property should be readonly.
    //        // Use this method to implement calculated properties.
    //        return "";
    //    }
    //    public static void AfterChange_PersistentProperty(TrwIOrder instance) {
    //        // An "AfterChange_" method is executed after a target property is changed. The target property should not be readonly. 
    //        // Use this method to refresh dependant property values.
    //    }
    //    public static void AfterConstruction(TrwIOrder instance) {
    //        // The "AfterConstruction" method is executed only once, after an object is created. 
    //        // Use this method to initialize new objects with default property values.
    //    }
    //    public static int SumMethod(TrwIOrder instance, int val1, int val2) {
    //        // You can also define custom methods.
    //        return val1 + val2;
    //    }
    //}
}
