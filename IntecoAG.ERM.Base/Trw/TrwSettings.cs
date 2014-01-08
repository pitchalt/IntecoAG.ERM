using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.Security;
//
using IntecoAG.ERM.CS;
using IntecoAG.ERM.CS.Settings; 
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.CRM.Party;
//
namespace IntecoAG.ERM.Trw {

    [Persistent("TrwSettings")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class TrwSettings: csCSettings {
        public TrwSettings(Session ses) : base(ses) {
        }

        public static TrwSettings GetInstance(IObjectSpace os) {
            TrwSettings result = os.FindObject<TrwSettings>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = os.CreateObject<TrwSettings>();
                result = InstanceInit(result);
//                result.Save();
            }
            return result;
        }
        public static TrwSettings GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            TrwSettings result = session.FindObject<TrwSettings>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new TrwSettings(session);
                result = InstanceInit(result);
//                result.Save();
            }
            return result;
        }
        public static TrwSettings InstanceInit(TrwSettings instance) {
            instance.UseCounter = 1;
            instance.Code = "ТРВ";
            instance.Name = "Настройки модуля интеграции с АСФЭУ";
            return instance;
        }

        private crmCPerson _PersonOtherSale;
        public crmCPerson PersonOtherSale {
            get { return _PersonOtherSale; }
            set { SetPropertyValue<crmCPerson>("PersonOtherSale", ref _PersonOtherSale, value); }
        }

        private crmCPerson _PersonOtherBay;
        public crmCPerson PersonOtherBay {
            get { return _PersonOtherBay; }
            set { SetPropertyValue<crmCPerson>("PersonOtherBay", ref _PersonOtherBay, value); }
        }
    }
}
