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
using IntecoAG.ERM.CS.Security;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.FinJurnal;
using IntecoAG.ERM.FM.Order;
//
namespace IntecoAG.ERM.FM {

    [Persistent("fmSettingsFinance")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class fmCSettingsFinanceExt: csCSettings {
        protected internal fmCSettingsFinanceExt(Session ses)
            : base(ses) {
        }

        public static fmCSettingsFinanceExt GetInstance(IObjectSpace os) {
            fmCSettingsFinanceExt result = os.FindObject<fmCSettingsFinanceExt>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = os.CreateObject<fmCSettingsFinanceExt>();
                result = InstanceInit(result);
                result.Save();
            }
            return result;
        } 
        public static fmCSettingsFinanceExt GetInstance(Session session) {
            //Get the Singleton's instance if it exists 
            fmCSettingsFinanceExt result = session.FindObject<fmCSettingsFinanceExt>(null);
            //Create the Singleton's instance 
            if (result == null) {
                result = new fmCSettingsFinanceExt(session);
                result = InstanceInit(result);
                result.Save();
            }
            return result;
        }
        public static fmCSettingsFinanceExt InstanceInit(fmCSettingsFinanceExt instance) {
            instance.UseCounter = 1;
            instance.Code = "Финансы Продажи";
            instance.Name = "Настройки продаж финансового модуля";
            return instance;
        }

        private fmCFJSaleOperation _CacheSaleOperationDefault;
        public fmCFJSaleOperation CacheSaleOperationDefault {
            get { return _CacheSaleOperationDefault; }
            set { SetPropertyValue<fmCFJSaleOperation>("CacheSaleOperationDefault", ref _CacheSaleOperationDefault, value); }
        }

        private fmCOrder _CacheSaleOrderDefault;
        public fmCOrder CacheSaleOrderDefault {
            get { return _CacheSaleOrderDefault; }
            set { SetPropertyValue<fmCOrder>("CacheSaleOrderDefault", ref _CacheSaleOrderDefault, value); }
        }

    }
}
